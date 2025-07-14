import { useEffect, useState } from "react";
import type { Note, NoteCreateDto } from "../types/Note";
import { getAllNotes, CreateNote, deleteNote } from "../api/notes";
import { NoteFprm } from "../components/NoteForm";
import { NoteCard } from "../components/NoteCard";


export const Home = () => {
    const [notes,setNotes] = useState<Note[]>([]);

    const loadNotes = async () => {
        const data = await getAllNotes();
        setNotes(data);
    };

    useEffect(() => {
        loadNotes();
    }, []);

    const handleAddNote = async (dto:NoteCreateDto) => {
        await CreateNote(dto);
        loadNotes();
    };

    const  handleDelete = async (id:string) => {
        await deleteNote(id);
        loadNotes();
    };

    const handleEdit = (id:string) => {
        alert(`Edit not ${id} (not yet implimented)`);
    };

    return (
        <div>
            <h2>Notes</h2>
            <NoteFprm onSubmit={handleAddNote} />
            {notes.map(note => (
                <NoteCard key={note.id} note= {note} onDelete={handleDelete} onEdit={handleEdit} />
            ))}
        </div>
    )

}