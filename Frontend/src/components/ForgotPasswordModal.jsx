import { useState } from "react";
import { forgotPassword } from "../services/api";
import Modal from "./Modal";
import "../styles/ForgotPasswordModal.css";

export default function ForgotPasswordModal({ onClose }) {
  const [email, setEmail] = useState("");
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setMessage("");

    try {
      const res = await forgotPassword(email);
      setMessage(
        res.message || "Om e-posten finns skickas en återställningslänk."
      );
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <Modal title="Glömt lösenord" onClose={onClose}>
      <form className="form" onSubmit={handleSubmit}>
        <input
          type="email"
          placeholder="Din e-postadress"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
          className="input"
        />
        <button type="submit" className="submit-button">
          Skicka återställningslänk
        </button>
      </form>
      {message && <p className="success">{message}</p>}
      {error && <p className="error">{error}</p>}
    </Modal>
  );
}
