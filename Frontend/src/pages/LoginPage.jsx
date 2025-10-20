import { useState } from "react";
import { login } from "../services/api";
import RegisterModal from "../components/RegisterModal";
import ForgotPasswordModal from "../components/ForgotPasswordModal";
import "../styles/LoginPage.css";

export default function LoginPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [showRegister, setShowRegister] = useState(false);
  const [showForgotPassword, setShowForgotPassword] = useState(false);

  const handleLogin = async (e) => {
    e.preventDefault();
    setError("");

    try {
  const data = await login(email, password);
      localStorage.setItem("accessToken", data.accessToken);
      const roles = Array.isArray(data?.user?.roles) ? data.user.roles : [];
      // If there's a next= param, prefer redirecting there after login
      const params = new URLSearchParams(window.location.search);
      const next = params.get('next');
      if (next) {
        window.location.href = next;
        return;
      }
      if (roles.includes("admin")) {
        window.location.href = "/admin";
      } else if (roles.includes("staff")) {
        window.location.href = "/staff";
      } else {
        window.location.href = "/";
      }
    } catch (err) {
      setError(err.message);
    }
  };

  const handleShowRegister = () => {
    setShowRegister(true);
  };

  const handleShowForgotPassword = () => {
    setShowForgotPassword(true);
  };

  return (
    <div className="wrapper">
      <div className="container">
        <h2 className="title">Logga in</h2>

        <form className="form" onSubmit={handleLogin}>
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
          <button type="submit" className="login-button">
            Logga in
          </button>
        </form>

        <div className="button-group">
          <button
            type="button"
            onClick={handleShowRegister}
            className="secondary-button"
          >
            Skapa konto
          </button>
          <button
            type="button"
            onClick={handleShowForgotPassword}
            className="secondary-button"
          >
            Glömt lösenord
          </button>
        </div>

        {error && <p className="error">{error}</p>}

        {showRegister && (
          <RegisterModal
            onClose={() => {
              setShowRegister(false);
            }}
          />
        )}
        {showForgotPassword && (
          <ForgotPasswordModal
            onClose={() => {
              setShowForgotPassword(false);
            }}
          />
        )}
      </div>
    </div>
  );
}
