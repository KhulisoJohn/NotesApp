import { BrowserRouter, Routes, Route } from "react-router-dom";
import { Home } from "./pages/Home";
import AddNote from "./pages/AddNote";
import { EditNote } from "./pages/EditNote";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/add" element={<AddNote />} />
        <Route path="/edit/:id" element={<EditNote />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
