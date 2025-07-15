import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import { MemoryRouter } from 'react-router-dom'; // ✅ ADD THIS
import { NoteCard } from './NoteCard';
import { Note } from '../types/Note'; // ✅ OK

const mockNote: Note = {
  id: '1',
  title: 'Test Note',
  content: 'This is a test',
  createdAt: '2025-07-14',
  status: 'Active',
};

test('renders note title and content', () => {
  render(
    <MemoryRouter> {/* ✅ WRAP WITH ROUTER */}
      <NoteCard
        note={mockNote}
        onDelete={() => {}}
        onEdit={() => {}}
      />
    </MemoryRouter>
  );

  expect(screen.getByText('Test Note')).toBeInTheDocument();
  expect(screen.getByText('This is a test')).toBeInTheDocument();
});
