import { Link } from "react-router-dom";
import type { Note } from "../types/Note";

type Props = {
  note: Note;
  onDelete: (id: string) => void;
   onEdit: (id: string) => void;
};

export const NoteCard = ({ note, onDelete }: Props) => {
  return (
    <div style={{ border: "1px solid #ccc", padding: "1rem", margin: "1rem 0" }}>
      <h3>{note.title}</h3>
      <p>{note.content}</p>
      <Link to={`/edit/${note.id}`}>
        <button>✏️ Edit</button>
      </Link>
      <button onClick={() => onDelete(note.id)}>🗑️ Delete</button>
    </div>
  );
};
