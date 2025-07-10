using System;

namespace BackEnd.DTOs;

public class NoteCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
