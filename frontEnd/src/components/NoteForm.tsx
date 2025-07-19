import React, { useState, useEffect } from 'react';
import '../App.css';


type NoteStatus = 'Active' | 'Archived';

interface NoteFormProps {
  onSubmit: (note: { title: string; content: string; status: NoteStatus }) => void;
  initialValue?: {
    title: string;
    content: string;
    status: NoteStatus;
  };
}

const NoteForm: React.FC<NoteFormProps> = ({ onSubmit, initialValue }) => {
  const [title, setTitle] = useState(initialValue?.title || '');
  const [content, setContent] = useState(initialValue?.content || '');
  const [status, setStatus] = useState<NoteStatus>(initialValue?.status || 'Active');

  useEffect(() => {
    if (initialValue) {
      setTitle(initialValue.title);
      setContent(initialValue.content);
      setStatus(initialValue.status);
    }
  }, [initialValue]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!title || !content) return;

    onSubmit({ title, content, status });

    
    if (!initialValue) {
      setTitle('');
      setContent('');
      setStatus('Active');
    }
  };

  return (
    <form className="note-form" onSubmit={handleSubmit}>
      <label htmlFor="title">Title</label>
      <input
        id="title"
        type="text"
        value={title}
        onChange={(e) => setTitle(e.target.value)}
      />

      <label htmlFor="content">Content</label>
      <textarea
        id="content"
        value={content}
        onChange={(e) => setContent(e.target.value)}
      ></textarea>

      <label htmlFor="status">Status</label>
      <select
        id="status"
        value={status}
        onChange={(e) => setStatus(e.target.value as NoteStatus)}
      >
        <option value="Active">Active</option>
        <option value="Archived">Archived</option>
      </select>

      <button type="submit" className="submit-btn">
        {initialValue ? 'Update Note' : 'Add Note'}
      </button>
    </form>
  );
};

export default NoteForm;
