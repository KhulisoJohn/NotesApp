
import type { Note } from "../types/Note";

type NoteCardProps = {
  note: Note;
  onDelete: (id: string) => void;
  onEdit: (id: string) => void;
};

export const NoteCard = ({ note, onDelete, onEdit }: NoteCardProps) => {
  return (
    <div className="note-card">
      <h3>{note.title}</h3>
      <p>{note.content}</p>

      <p>Status: {note.status}</p>
     <p>
  Created: {note.createdAt ? new Date(note.createdAt).toLocaleString() : "N/A"}
</p>
{note.createdAt !== note.lastUpdatedAt && note.lastUpdatedAt && (
  <p>Edited: {new Date(note.lastUpdatedAt).toLocaleString()}</p>
)}


      <div>
        <button onClick={() => onEdit(note.id)}>✏️ Edit</button>
        <button onClick={() => onDelete(note.id)}>🗑️ Delete</button>
      </div>
    </div>
  );
};
