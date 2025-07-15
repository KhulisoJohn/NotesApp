import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom"; // ✅ added useNavigate
import type { Note } from "../types/Note";
import { getAllNotes, deleteNote } from "../api/notes";
import { NoteCard } from "../components/NoteCard";

export const Home = () => {
  const [notes, setNotes] = useState<Note[]>([]);
  const navigate = useNavigate(); // ✅ hook for programmatic navigation

  const loadNotes = async () => {
    const data = await getAllNotes();
    setNotes(data);
  };

  useEffect(() => {
    loadNotes();
  }, []);

  const handleDelete = async (id: string) => {
    await deleteNote(id);
    loadNotes();
  };

  const handleEdit = (id: string) => {
    navigate(`/edit/${id}`); // ✅ navigate to the edit page with note id
  };

  return (
    <div>
      <h2>Notes</h2>
      <Link to="/add">
        <button style={{ marginBottom: "1rem" }}>➕ Add New Note</button>
      </Link>
      {notes.map((note) => (
        <NoteCard
          key={note.id}
          note={note}
          onDelete={handleDelete}
          onEdit={handleEdit}
        />
      ))}
    </div>
  );
};
