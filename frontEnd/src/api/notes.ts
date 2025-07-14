import axios from 'axios';
import type { Note, NoteCreateDto, NoteUpdateDto } from '../types/Note';


const api = axios.create({
  baseURL: 'http://localhost:5053/notes',
  headers: {
    'Content-Type': 'application/json',
  },
});

export default api;


export const getAllNotes = async(): Promise<Note[]> => {
    const res = await api.get('/');
    return res.data;
};

export const getNoteById = async(id: string): Promise<Note> => {
    const res = await api.get(`/${id}`);
    return res.data;
}

export const CreateNote = async(dto:NoteCreateDto): Promise<Note> => {
    const res = await api.post('/', dto);
    return res.data;
}

export const updateNote = async (id: string, dto: NoteUpdateDto): Promise<boolean> => {
    const res = await api.put(`/${id}`, dto);
    return res.data;
}

export const deleteNote = async ( id: string): Promise<boolean> => {
    const res = await api.delete(`/${id}`);
    return res.data
}

