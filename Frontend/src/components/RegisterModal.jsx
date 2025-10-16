import { useState } from "react";
import { registerUser } from "../services/api";
import Modal from "./Modal";
import "../styles/RegisterModal.css";

export default function RegisterModal({ onClose }) {
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setMessage("");

    if (password !== confirmPassword) {
      setError("Lösenorden matchar inte.");
      return;
    }

    try {
      await registerUser({ firstName, lastName, email, password });
      setMessage("Användare skapad! Du kan nu logga in.");
      setTimeout(onClose, 2000);
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <Modal title="Skapa konto" onClose={onClose}>
      <form className="form" onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Förnamn"
          value={firstName}
          onChange={(e) => setFirstName(e.target.value)}
          required
          className="input"
        />
        <input
          type="text"
          placeholder="Efternamn"
          value={lastName}
          onChange={(e) => setLastName(e.target.value)}
          required
          className="input"
        />
        <input
          type="email"
          placeholder="E-postadress"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
          className="input"
        />
        <input
          type="password"
          placeholder="Lösenord"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
          className="input"
        />
        <input
          type="password"
          placeholder="Bekräfta lösenord"
          value={confirmPassword}
          onChange={(e) => setConfirmPassword(e.target.value)}
          required
          className="input"
        />
        <button type="submit" className="submit-button">
          Skapa konto
        </button>
      </form>
      {message && <p className="success">{message}</p>}
      {error && <p className="error">{error}</p>}
    </Modal>
  );
}
