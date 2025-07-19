import type { Note } from "../types/Note";
import '../App.css'
import { FaEdit, FaTrash } from "react-icons/fa"; // Font Awesome icons

type NoteCardProps = {
  note: Note;
  onDelete: (id: string) => void;
  onEdit: (id: string) => void;
};

export const NoteCard = ({ note, onDelete, onEdit }: NoteCardProps) => {
  return (
    <div className="note-card">
      
        <h3 style={{ marginBottom: "0.5rem" }}>Title: {note.title}</h3>
      <p className="note-content" style={{ 
  wordWrap: 'break-word', 
  overflowWrap: 'break-word', 
  whiteSpace: 'pre-wrap' 
}}>
  {note.content}
</p>

        
  <div className="note-bottom">
      <div style={{ marginBottom: "1rem" }}>
        <p>Status: {note.status}</p>
        <p>
          Created:{" "}
          {note.createdAt ? new Date(note.createdAt).toLocaleString() : "N/A"}
        </p>
        {note.createdAt !== note.lastUpdatedAt && note.lastUpdatedAt && (
          <p>Edited: {new Date(note.lastUpdatedAt).toLocaleString()}</p>
        )}
      </div>

      <div style={{ display: "flex", gap: "1rem", justifyContent: "flex-end" }}>
        <button onClick={() => onEdit(note.id)}>
          <FaEdit />
        </button>
        <button onClick={() => onDelete(note.id)}>
          <FaTrash /> 
        </button>
      </div>
      </div>
    </div>
  );
};
