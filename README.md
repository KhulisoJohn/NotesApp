# 📝 NotesApp

A fullstack note-taking application built with:

- ⚙️ ASP.NET Core Minimal API (C#)
- 🧠 MongoDB for data storage
- 🌐 React for the frontend
- 🧪 Designed for local development (single repo setup)

---

## 📁 Project Structure

```bash
NotesApp/
├── backend/            # ASP.NET Core Minimal API (C#)
│   ├── Program.cs
│   ├── Models/
│   ├── Services/
│   └── ...
├── frontend/           # React application
│   ├── public/
│   ├── src/
│   │   ├── components/
│   │   └── App.jsx
│   └── package.json
└── README.md
```

## ✨ Features

- Create, view, edit, and delete personal notes
- Fully responsive UI
- Auto-refresh after CRUD operations
- Clean code with reusable components
- MongoDB-powered storage
- RESTful API using Minimal API pattern

---

---

## ⚙️ API Endpoints

| Method | Endpoint      | Description       |
| ------ | ------------- | ----------------- |
| GET    | `/notes`      | Get all notes     |
| GET    | `/notes/{id}` | Get a single note |
| POST   | `/notes`      | Create a new note |
| PUT    | `/notes/{id}` | Update a note     |
| DELETE | `/notes/{id}` | Delete a note     |

---

## 🧠 Tech Stack

| Tech         | Purpose            |
| ------------ | ------------------ |
| C# / .NET 8  | Backend REST API   |
| MongoDB      | NoSQL data storage |
| React        | Frontend UI        |
| Axios        | API communication  |
| Tailwind CSS | Optional styling   |

---

## 🚀 Getting Started

### 1️⃣ Start MongoDB

Make sure MongoDB is running locally or in Docker.

```
mongod --dbpath ./data
```

### 2️⃣ Run the Backend

```
cd backend
dotnet run
```

### 3️⃣ Run the Frontend

```
cd frontend

# Create a .env file in /frontend with:
# REACT_APP_API_URL=http://localhost:5000

npm install
npm run dev
```

## 🛠 Environment Config

Inside /frontend/.env:

```
REACT_APP_API_URL=http://localhost:5000
```

You can change this if your backend is running on another port.

## 🧪 Example Note Schema

```
{
  "id": "664fa3b2c85c8722b54321e1",
  "title": "Meeting Notes",
  "content": "Discussed project roadmap...",
  "createdAt": "2025-07-10T10:30:00Z"
}
```

## 🧑‍💻 Author

Built with ❤️ by Khulyso John

“Code it. Run it. Learn it. Ship it.”

## 📄 License

MIT License — use it, modify it, ship it.

---

## ✅ Summary

- Designed for a **single repo with `/backend` and `/frontend` folders**.
- No cloud deploy needed — you run everything **locally**.
- Easy to convert to production later with Docker or separate repos.
