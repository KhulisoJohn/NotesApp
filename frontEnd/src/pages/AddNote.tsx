// src/pages/AddNote.tsx
import { useNavigate } from 'react-router-dom';
import '../App.css';
import {NoteForm} from '../components/NoteForm';
import { CreateNote } from '../api/notes';
import type { NoteCreateDto } from '../types/Note';

const AddNote = () => {
  const navigate = useNavigate();

  const handleAddNote = async (data: NoteCreateDto) => {
    try {
      await CreateNote(data);
      navigate('/'); // Go back home after success
    } catch (error) {
      console.error("Failed to create note:", error);
    }
  };

  return (
    <div className="container">
      <h2>Add a New Note</h2>
      <NoteForm<NoteCreateDto> onSubmit={handleAddNote} />

    </div>
  );
};

export default AddNote;
