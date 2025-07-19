import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import type { Note } from "../types/Note";
import "../App.css";
import { MdAdd } from "react-icons/md";
import { getAllNotes, deleteNote } from "../api/notes";
import { NoteCard } from "../components/NoteCard";

export const Home = () => {
  const [notes, setNotes] = useState<Note[]>([]);
  const navigate = useNavigate();

  const loadNotes = async () => {
    const data = await getAllNotes();
    setNotes(data ?? []); 
  };

  useEffect(() => {
    loadNotes();
  }, []);

  const handleDelete = async (id: string) => {
    await deleteNote(id);
    loadNotes(); 
  };

  const handleEdit = (id: string) => {
    navigate(`/edit/${id}`);
  };

  return (
    <div className="hero">
      <div className="nav">
        <h2>NotesApp</h2>
        <Link to="/add">
          <button
            style={{
              marginBottom: "1rem",
              display: "flex",
              alignItems: "center",
              gap: "0.5rem",
            }}
          >
            <MdAdd size={20} />
            Add New Note
          </button>
        </Link>
      </div>

      <div className="hero-content">
        <h3>My Notes</h3>
        <p>
          Welcome to <strong>NotesApp</strong> — your space to jot down thoughts, plans, reminders, or anything on your mind.
          All your notes are organized, editable, and safely stored for whenever you need them. Get started by creating
          a new note or managing your existing ones below.
        </p>

        {notes.length === 0 ? (
          <div style={{ marginTop: "2rem", textAlign: "center", color: "#ccc" }}>
            <p style={{ fontStyle: "italic" }}>
              You have no notes yet. Start by clicking <strong>"Add New Note"</strong> above.
            </p>
          </div>
        ) : (
          notes.map((note) => (
            <NoteCard
              key={note.id}
              note={note}
              onDelete={handleDelete}
              onEdit={handleEdit}
            />
          ))
        )}
      </div>
    </div>
  );
};
