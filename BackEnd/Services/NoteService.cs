using MongoDB.Driver;
using BackEnd.Models;
using BackEnd.DTOs;
using Microsoft.Extensions.Options;

namespace BackEnd.Services;

public class NoteService : INoteService
{
   private readonly IMongoCollection<Note> _notes;

    public NoteService(IOptions<MongoDbSetting> settings, IMongoClient client)
    {
        var dbName = settings.Value.DatabaseName;
        var database = client.GetDatabase(dbName);
        _notes = database.GetCollection<Note>("notes");
    }

    public async Task<List<Note>> GetAllAsync()
    {
        return await _notes.Find(_ => true).ToListAsync();
    }

    public async Task<Note?> GetByIdAsync(string id)
    {
        return await _notes.Find(n => n.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Note> CreateAsync(NoteCreateDto dto)
    {
        var note = new Note
        {
            Title = dto.Title,
            Content = dto.Content,
            CreatedAt = DateTime.UtcNow
        };

        await _notes.InsertOneAsync(note);
        return note;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _notes.DeleteOneAsync(n => n.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<bool> UpdateAsync(string id, NoteUpdateDto dto)
    {
        var update = Builders<Note>.Update
            .Set(n => n.Title, dto.Title)
            .Set(n => n.Content, dto.Content)
            .Set(n => n.Status, dto.Status)
            .Set(n => n.LastUpdatedAt, DateTime.UtcNow);

        var result = await _notes.UpdateOneAsync(n => n.Id == id, update);
        return result.ModifiedCount > 0;
    }
}
