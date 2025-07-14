export type NoteStatus = 'Active' | 'Archived';


export interface Note {
    id: string;
    title:string;
    content: string;
    createdAt: string;
    lastUpdatedAt?:string;
    status: NoteStatus;
}

export interface NoteCreateDto {
    title: string;
    content: string;
}

export interface NoteUpdateDto {
    title: string;
    content: string;
    status: NoteStatus;
}