using System;
using BackEnd.Models;
using Microsoft.AspNetCore.Components.Web;

namespace BackEnd.DTOs;

public class NoteUpdateDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public NoteStatus Status { get; set; } = NoteStatus.Active;
}
