import { useState } from "react";
import type { NoteCreateDto, NoteUpdateDto, NoteStatus } from "../types/Note";

type Props<T> = {
  onSubmit: (dto: T) => void;
  initialValue?: Partial<T>;
};

export function NoteForm<T extends NoteCreateDto | NoteUpdateDto>({
  onSubmit,
  initialValue = {},
}: Props<T>) {
  const [title, setTitle] = useState(initialValue.title || "");
  const [content, setContent] = useState(initialValue.content || "");
  const [status, setStatus] = useState<NoteStatus>(
    (initialValue as NoteUpdateDto).status || "Active"
  );

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const base = { title, content };
    if ("status" in initialValue) {
      onSubmit({ ...base, status } as T); // for update
    } else {
      onSubmit(base as T); // for create
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <input
        placeholder="Title"
        value={title}
        onChange={(e) => setTitle(e.target.value)}
        required
      />
      <br />
      <textarea
        placeholder="Content"
        value={content}
        onChange={(e) => setContent(e.target.value)}
        required
      />
      <br />

      {"status" in initialValue && (
        <>
          <label>Status:</label>
          <select value={status} onChange={(e) => setStatus(e.target.value as NoteStatus)}>
            <option value="Active">Active</option>
            <option value="Archived">Archived</option>
          </select>
          <br />
        </>
      )}

      <button type="submit">💾 Save</button>
    </form>
  );
}
