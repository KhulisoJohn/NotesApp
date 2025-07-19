import React, { useState } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import NoteForm from '../components/NoteForm'; // Use the form
import type { NoteCreateDto } from '../types/Note'; // Optional

const AddNote: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const handleSubmit = async (note: NoteCreateDto) => {
    setLoading(true);
    setError(null);
    setSuccessMessage(null);

    try {
      await axios.post('http://localhost:5053/notes', note); 
      setSuccessMessage('Note created successfully!');
    } catch (err) {
      console.error(err);
      setError('Failed to create note');
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <div className="nav">
        <h2>NotesApp</h2>
        <Link to="/">
          <button>Back</button>
        </Link>
      </div>

      <div className="add-note">
        <h2>Add New Note</h2>
        <NoteForm onSubmit={handleSubmit} />
        {loading && <p>Submitting...</p>}
        {error && <p style={{ color: 'red' }}>{error}</p>}
        {successMessage && <p style={{ color: 'green' }}>{successMessage}</p>}
      </div>
    </>
  );
};

export default AddNote;
