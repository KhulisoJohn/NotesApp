
using Xunit;
using Moq;
using FluentAssertions;
using MongoDB.Driver;
using BackEnd.Models;
using BackEnd.DTOs;
using BackEnd.Services;


namespace NotesApp.Tests
{
    public class NoteServiceTests
    {
        private readonly Mock<IMongoCollection<Note>> _mockCollection;
        private readonly Mock<IMongoDatabase> _mockDatabase;
        private readonly Mock<IMongoClient> _mockClient;
        private readonly NoteService _service;

        public NoteServiceTests()
        {
            // Setup mocks
            _mockCollection = new Mock<IMongoCollection<Note>>();
            _mockDatabase = new Mock<IMongoDatabase>();
            _mockClient = new Mock<IMongoClient>();

            // Setup database to return mocked collection
            _mockDatabase.Setup(db => db.GetCollection<Note>("Notes", null))
                .Returns(_mockCollection.Object);

            // Setup client to return mocked database
            _mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), null))
                .Returns(_mockDatabase.Object);

            // Since NoteService expects IConfiguration for connection string,
            // we mock that with a stub configuration.
            var mockConfig = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            mockConfig.Setup(c => c["MongoDb:ConnectionString"]).Returns("mongodb://localhost:27017");
            mockConfig.Setup(c => c["MongoDb:DatabaseName"]).Returns("NoteAppDb");

            // We override NoteService to inject our mocked client and database:
            // So, create a derived class for testing that accepts mocks.
            _service = new TestableNoteService(mockConfig.Object, _mockClient.Object, _mockDatabase.Object, _mockCollection.Object);
        }

        // Test class to override constructor for injecting mocks
        private class TestableNoteService : NoteService
        {
            public TestableNoteService(Microsoft.Extensions.Configuration.IConfiguration config,
                IMongoClient client,
                IMongoDatabase database,
                IMongoCollection<Note> collection)
                : base(config)
            {
                _notes = collection;
            }
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfNotes()
        {
            // Arrange
            var notesList = new List<Note> { new Note { Id = "1", Title = "Test", Content = "Test Content" } };
            var mockCursor = new Mock<IAsyncCursor<Note>>();
            mockCursor.Setup(_ => _.Current).Returns(notesList);
            mockCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<System.Threading.CancellationToken>()))
                .Returns(true)
                .Returns(false);
            mockCursor
                .SetupSequence(_ => _.MoveNextAsync(It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            _mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Note>>(),
                It.IsAny<FindOptions<Note, Note>>(),
                default))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].Title.Should().Be("Test");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNote_WhenNoteExists()
        {
            // Arrange
            var note = new Note { Id = "1", Title = "Note1" };
            var mockCursor = new Mock<IAsyncCursor<Note>>();
            mockCursor.Setup(_ => _.Current).Returns(new List<Note> { note });
            mockCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<System.Threading.CancellationToken>()))
                .Returns(true)
                .Returns(false);
            mockCursor
                .SetupSequence(_ => _.MoveNextAsync(It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            _mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Note>>(),
                It.IsAny<FindOptions<Note, Note>>(),
                default))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _service.GetByIdAsync("1");

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be("1");
            result.Title.Should().Be("Note1");
        }

        [Fact]
        public async Task CreateAsync_ShouldInsertNoteAndReturnNote()
        {
            // Arrange
            Note? insertedNote = null;
            _mockCollection.Setup(c => c.InsertOneAsync(
                It.IsAny<Note>(),
                null,
                default))
                .Callback<Note, InsertOneOptions?, System.Threading.CancellationToken>((note, options, token) =>
                {
                    insertedNote = note;
                })
                .Returns(Task.CompletedTask);

            var dto = new NoteCreateDto { Title = "New Note", Content = "New Content" };

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            result.Should().NotBeNull();
            insertedNote.Should().NotBeNull();
            insertedNote!.Title.Should().Be("New Note");
            result.Title.Should().Be("New Note");
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenNoteDeleted()
        {
            // Arrange
            var deleteResult = new DeleteResult.Acknowledged(1);
            _mockCollection.Setup(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<Note>>(),
                default))
                .ReturnsAsync(deleteResult);

            // Act
            var result = await _service.DeleteAsync("1");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenNoteUpdated()
        {
            // Arrange
            var updateResult = new UpdateResult.Acknowledged(1, 1, null);
            _mockCollection.Setup(c => c.UpdateOneAsync(
                It.IsAny<FilterDefinition<Note>>(),
                It.IsAny<UpdateDefinition<Note>>(),
                null,
                default))
                .ReturnsAsync(updateResult);

            var dto = new NoteUpdateDto { Title = "Updated", Content = "Updated Content", Status =(NoteStatus) 1 };

            // Act
            var result = await _service.UpdateAsync("1", dto);

            // Assert
            result.Should().BeTrue();
        }
    }
}
