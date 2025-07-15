import { render, screen, waitFor, fireEvent } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom'; // ✅ Add this
import { Home } from './Home';
import '@testing-library/jest-dom';
import * as api from '../api/notes';

jest.mock('../api/notes');

const sampleNotes = [
  {
    id: '1',
    title: 'Note 1',
    content: 'Content 1',
    createdAt: new Date().toISOString(),
    status: 'Active',
  },
];

describe('Home', () => {
  beforeEach(() => {
    (api.getAllNotes as jest.Mock).mockResolvedValue(sampleNotes);
    (api.deleteNote as jest.Mock).mockResolvedValue({});
  });

  test('loads and displays notes', async () => {
    render(<MemoryRouter><Home /></MemoryRouter>); // ✅ Wrap with Router
    expect(screen.getByText(/notes/i)).toBeInTheDocument();

    await waitFor(() => {
      expect(screen.getByText('Note 1')).toBeInTheDocument();
    });
  });

  test('delete button works', async () => {
    render(<MemoryRouter><Home /></MemoryRouter>); // ✅ Same here

    await waitFor(() => screen.getByText('Note 1'));

    fireEvent.click(screen.getByText(/delete/i));

    await waitFor(() => {
      expect(api.deleteNote).toHaveBeenCalledWith('1');
    });
  });
});
