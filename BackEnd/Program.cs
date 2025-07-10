using BackEnd.DTOs;
using BackEnd.Services;
using BackEnd.Models;
using System.Linq;
using MongoDB.Driver;


var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddScoped<INoteService, NoteService>();

// MongoDB config (if not already in appsettings.json)
builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    var config = serviceProvider.GetRequiredService<IConfiguration>();
    return new MongoClient(config["MongoDb:ConnectionString"]);
});

// Build the app
var app = builder.Build();

// Define endpoints

// GET all notes
app.MapGet("/notes", async (INoteService noteService) =>
{
  List<Note> notes = await noteService.GetAllAsync();


    return notes.Any()
        ? Results.Ok(notes)
        : Results.NoContent();
});

// GET single note by ID
app.MapGet("/notes/{id}", async (string id, INoteService noteService) =>
{
    var note = await noteService.GetByIdAsync(id);
    return note is not null ? Results.Ok(note) : Results.NotFound();
});

// POST (create) a new note
app.MapPost("/notes", async (NoteCreateDto dto, INoteService noteService) =>
{
    var createdNote = await noteService.CreateAsync(dto);
    return Results.Created($"/notes/{createdNote.Id}", createdNote);
});

// PUT (update) an existing note
app.MapPut("/notes/{id}", async (string id, NoteUpdateDto dto, INoteService noteService) =>
{
    var isUpdated = await noteService.UpdateAsync(id, dto);
    return isUpdated ? Results.Ok() : Results.NotFound();
});

// DELETE a note by ID
app.MapDelete("/notes/{id}", async (string id, INoteService noteService) =>
{
    var isDeleted = await noteService.DeleteAsync(id);
    return isDeleted ? Results.Ok() : Results.NotFound();
});

// Run the app
app.Run();
