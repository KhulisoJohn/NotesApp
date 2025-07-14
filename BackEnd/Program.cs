using BackEnd.DTOs;
using BackEnd.Services;
using BackEnd.Models;
using DotNetEnv;
using MongoDB.Driver;



var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load(); 

builder.Services.AddScoped<INoteService, NoteService>();


builder.Services.AddSingleton<IMongoClient>(_ =>
{
    var connString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");
    return new MongoClient(connString);
});

var app = builder.Build();



// GET all notes
app.MapGet("/notes", async (INoteService noteService) =>
{
    List<Note> notes = await noteService.GetAllAsync();
    return notes.Any() ? Results.Ok(notes) : Results.NoContent();
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

app.Run();
