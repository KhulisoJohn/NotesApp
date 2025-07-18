using BackEnd.DTOs;
using BackEnd.Models;
using BackEnd.Services;
using DotNetEnv;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// MongoDB configuration
builder.Services.Configure<MongoDbSetting>(
    builder.Configuration.GetSection("MongoDbSetting")
);

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSetting>>().Value;
    if (string.IsNullOrWhiteSpace(settings.ConnectionString))
        throw new InvalidOperationException("MongoDB connection string is missing.");

    return new MongoClient(settings.ConnectionString);
});

// CORS for frontend (React dev server)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhostFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Register services
builder.Services.AddScoped<INoteService, NoteService>();

var app = builder.Build();

// Validate DB config
var mongoSettings = app.Services.GetRequiredService<IOptions<MongoDbSetting>>().Value;
if (string.IsNullOrWhiteSpace(mongoSettings.DatabaseName))
    throw new InvalidOperationException("MongoDB database name is missing.");

app.UseCors("AllowLocalhostFrontend");


// ==========================
//        API ROUTES
// ==========================

// GET all notes
app.MapGet("/notes", async (INoteService noteService) =>
{
    var notes = await noteService.GetAllAsync();
    return notes.Any() ? Results.Ok(notes) : Results.NoContent();
});

// GET note by ID
app.MapGet("/notes/{id}", async (string id, INoteService noteService) =>
{
    var note = await noteService.GetByIdAsync(id);
    return note is not null ? Results.Ok(note) : Results.NotFound();
});

// POST create note
app.MapPost("/notes", async (NoteCreateDto dto, INoteService noteService) =>
{
    var createdNote = await noteService.CreateAsync(dto);
    return Results.Created($"/notes/{createdNote.Id}", createdNote);
});

// PUT update note
app.MapPut("/notes/{id}", async (string id, NoteUpdateDto dto, INoteService noteService) =>
{
    var isUpdated = await noteService.UpdateAsync(id, dto);
    return isUpdated ? Results.Ok() : Results.NotFound();
});

// DELETE note
app.MapDelete("/notes/{id}", async (string id, INoteService noteService) =>
{
    var isDeleted = await noteService.DeleteAsync(id);
    return isDeleted ? Results.Ok() : Results.NotFound();
});

app.Run();
