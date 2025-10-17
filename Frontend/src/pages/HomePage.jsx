import React from "react";
import { Link } from "react-router-dom";

export default function HomePage() {
  return (
    <div className="hero">
      <div className="hero-inner">
        <h1 className="hero-title">SportBook</h1>
        <p className="hero-subtitle">
          Boka ditt nästa pass snabbt och enkelt. Padel, Tennis, Fotboll - allt på ett ställe.
        </p>
        <div className="hero-actions">
          <Link to="/activities" className="btn btn-primary">Se tillgängliga tider</Link>
          <Link to="/login" className="btn btn-secondary">Logga in</Link>
        </div>
      </div>
    </div>
  );
}


