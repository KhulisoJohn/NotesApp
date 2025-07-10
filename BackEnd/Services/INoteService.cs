using System;
using BackEnd.Models;
using BackEnd.DTOs;

namespace BackEnd.Services;

public interface INoteService
{
    Task<List<Note>> GetAllAsync(); 
    Task<Note?> GetByIdAsync(string id);
    Task<Note> CreateAsync(NoteCreateDto dto);
    Task<bool> UpdateAsync(string id, NoteUpdateDto dto);
    Task<bool> DeleteAsync(string id);
}

