import { useEffect, useState } from "react";
import '../App.css';
import { useParams, useNavigate, Link } from "react-router-dom";
import type { NoteUpdateDto, Note } from "../types/Note";
import { getNoteById, updateNote } from "../api/notes";
import  NoteForm  from "../components/NoteForm";

export const EditNote = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [note, setNote] = useState<Note | null>(null);

  useEffect(() => {
    const loadNote = async () => {
      if (!id) return;
      const data = await getNoteById(id);
      setNote(data);
    };

    loadNote();
  }, [id]);

  const handleUpdate = async (dto: NoteUpdateDto) => {
    if (!id) return;
    await updateNote(id, dto);
    navigate("/"); // redirect to home
  };

  if (!note) return <div>Loading...</div>;

  return (
    <>
    <div className="nav">
      <h2>NotesApp</h2>
      <Link to="/">
        <button>Back</button>
      </Link>
    </div>

    <div>
      <h2>Edit Note</h2>
      <NoteForm
        onSubmit={handleUpdate}
        initialValue={{ title: note.title, content: note.content, status: note.status }}
      />
    </div>
    </>
  );
};
