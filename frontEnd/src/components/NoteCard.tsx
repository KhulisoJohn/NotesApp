import type { Note } from "../types/Note";

interface Props {
    note: Note;
    onDelete: (id: string) => void;
    onEdit: (id: string) => void;
}

export const NoteCard = ({note, onDelete, onEdit}: Props) =>{
    return(
        <div className="note-card">
            <h3>{note.title}</h3>
            <p>{note.content}</p>
            <small>Status: {note.status}</small>

            <div>
                <button onClick={() => onEdit(note.id)}>Edit</button>
                <button onClick={() => onDelete(note.id)}>Delete</button>
            </div>
        </div>
    )
}