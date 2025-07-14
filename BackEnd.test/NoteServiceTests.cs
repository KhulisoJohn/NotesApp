#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using MongoDB.Driver;
using BackEnd.Models;
using BackEnd.DTOs;
using BackEnd.Services;

namespace BackEnd.test
{
    public class NoteServiceTests
    {
        private readonly Mock<IMongoCollection<Note>> _mockNotesCollection;
        private readonly Mock<IMongoDatabase> _mockDatabase;
        private readonly Mock<IMongoClient> _mockClient;
        private readonly NoteService _service;

        public NoteServiceTests()
        {
            _mockNotesCollection = new Mock<IMongoCollection<Note>>();
            _mockDatabase = new Mock<IMongoDatabase>();
            _mockClient = new Mock<IMongoClient>();

            // Setup client to return mocked database
            _mockClient
                .Setup(c => c.GetDatabase(It.IsAny<string>(), null))
                .Returns(_mockDatabase.Object);

            // Setup database to return mocked collection
            _mockDatabase
                .Setup(db => db.GetCollection<Note>("Notes", null))
                .Returns(_mockNotesCollection.Object);

            // Setup service with mocked client and config
            var mockConfig = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            mockConfig.Setup(c => c["MongoDb:DatabaseName"]).Returns("TestDb");

            _service = new NoteService(mockConfig.Object, _mockClient.Object);
        }

        private Mock<IAsyncCursor<Note>> CreateMockCursor(IEnumerable<Note> notes)
        {
            var mockCursor = new Mock<IAsyncCursor<Note>>();
            var enumerator = notes.GetEnumerator();

            mockCursor.Setup(_ => _.Current).Returns(() => notes);
            mockCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            mockCursor
                .SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false);
            return mockCursor;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfNotes()
        {
            // Arrange
            var expectedNotes = new List<Note>
            {
                new Note { Id = "1", Title = "Note 1", Content = "Content 1", Status = NoteStatus.Active },
                new Note { Id = "2", Title = "Note 2", Content = "Content 2", Status = NoteStatus.Active }
            };

            var mockCursor = CreateMockCursor(expectedNotes);

            _mockNotesCollection
                .Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<Note>>(),
                    It.IsAny<FindOptions<Note, Note>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var notes = await _service.GetAllAsync();

            // Assert
            notes.Should().BeEquivalentTo(expectedNotes);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNote_WhenFound()
        {
            // Arrange
            var expectedNote = new Note { Id = "1", Title = "Note 1", Content = "Content 1", Status = NoteStatus.Active };

            var mockCursor = CreateMockCursor(new List<Note> { expectedNote });

            _mockNotesCollection
                .Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<Note>>(),
                    It.IsAny<FindOptions<Note, Note>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var note = await _service.GetByIdAsync("1");

            // Assert
            note.Should().NotBeNull();
            note.Should().BeEquivalentTo(expectedNote);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            var mockCursor = CreateMockCursor(new List<Note>());

            _mockNotesCollection
                .Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<Note>>(),
                    It.IsAny<FindOptions<Note, Note>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var note = await _service.GetByIdAsync("nonexistent-id");

            // Assert
            note.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ShouldInsertNote_AndReturnNote()
        {
            // Arrange
            Note? insertedNote = null;

            _mockNotesCollection
                .Setup(c => c.InsertOneAsync(
                    It.IsAny<Note>(),
                    It.IsAny<InsertOneOptions>(),
                    It.IsAny<CancellationToken>()))
                .Callback<Note, InsertOneOptions?, CancellationToken>((note, _, __) =>
                {
                    insertedNote = note;
                })
                .Returns(Task.CompletedTask);

            var dto = new NoteCreateDto
            {
                Title = "New Note",
                Content = "Note content"
            };

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            insertedNote.Should().NotBeNull();
            insertedNote!.Title.Should().Be(dto.Title);
            insertedNote.Content.Should().Be(dto.Content);
            insertedNote.Status.Should().Be(NoteStatus.Active);
            insertedNote.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

            result.Should().BeEquivalentTo(insertedNote);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenNoteDeleted()
        {
            // Arrange
            var deleteResult = new DeleteResult.Acknowledged(1);

            _mockNotesCollection
                .Setup(c => c.DeleteOneAsync(
                    It.IsAny<FilterDefinition<Note>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(deleteResult);

            // Act
            var result = await _service.DeleteAsync("1");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenNoNoteDeleted()
        {
            // Arrange
            var deleteResult = new DeleteResult.Acknowledged(0);

            _mockNotesCollection
                .Setup(c => c.DeleteOneAsync(
                    It.IsAny<FilterDefinition<Note>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(deleteResult);

            // Act
            var result = await _service.DeleteAsync("nonexistent-id");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenNoteUpdated()
        {
            // Arrange
            var updateResult = new UpdateResult.Acknowledged(1, 1, null);

            _mockNotesCollection
                .Setup(c => c.UpdateOneAsync(
                    It.IsAny<FilterDefinition<Note>>(),
                    It.IsAny<UpdateDefinition<Note>>(),
                    It.IsAny<UpdateOptions>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(updateResult);

            var dto = new NoteUpdateDto
            {
                Title = "Updated Title",
                Content = "Updated Content",
                Status = NoteStatus.Active
            };

            // Act
            var result = await _service.UpdateAsync("1", dto);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenNoNoteUpdated()
        {
            // Arrange
            var updateResult = new UpdateResult.Acknowledged(0, 0, null);

            _mockNotesCollection
                .Setup(c => c.UpdateOneAsync(
                    It.IsAny<FilterDefinition<Note>>(),
                    It.IsAny<UpdateDefinition<Note>>(),
                    It.IsAny<UpdateOptions>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(updateResult);

            var dto = new NoteUpdateDto
            {
                Title = "Updated Title",
                Content = "Updated Content",
                Status = NoteStatus.Active
            };

            // Act
            var result = await _service.UpdateAsync("nonexistent-id", dto);

            // Assert
            result.Should().BeFalse();
        }
    }
}
