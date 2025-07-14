import { useState } from "react";
import type { NoteCreateDto } from "../types/Note";

interface Props {
    onSubmit: (dto: NoteCreateDto) => void;
}

export const NoteFprm = ({ onSubmit}: Props) => {
    const [title,setTitle] = useState('')
    const [content, setContent] = useState('');

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        onSubmit({title, content});
        setTitle('');
        setContent('');
    };

    return (
        <div>
            <form onSubmit={handleSubmit}>
                <input value={title} onChange={e => setTitle(e.target.value)} placeholder="Totle" required />
                <textarea value={content} onChange={e =>setContent(e.target.value)} placeholder="content" required />
                    <button type="submit">Add Note</button>
            </form>
        </div>
    );
};
