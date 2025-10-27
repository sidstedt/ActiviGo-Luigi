import React from "react";
import { Link } from "react-router-dom";
import { getCurrentUser } from "../services/api.js";
import UserWeekCalendar from "../components/UserCalendar";

export default function HomePage() {
  const [isAdmin, setIsAdmin] = React.useState(false);
  const [isLoggedIn, setIsLoggedIn] = React.useState(false);

  React.useEffect(() => {
    const user = getCurrentUser();
    if (user) {
      setIsLoggedIn(true);
      const userRoles = Array.isArray(user.roles) ? user.roles : [];
      if (userRoles.includes("admin")) {
        setIsAdmin(true);
      }
    } else {
      setIsLoggedIn(false);
      setIsAdmin(false);
    }
  }, []);

  let title, subtitle, primaryLink, secondaryLink;

  if (isAdmin) {
    title = "Administratörspanel";
    subtitle = "Hantera användare, aktiviteter och systemets statistik här.";
    primaryLink = { to: "/admin/activities", text: "Hantera Aktiviteter" };
    secondaryLink = { to: "/admin/statistics", text: "Se Statistik" };
  } else if (isLoggedIn) {
    title = "Välkommen till ActiviGo!";
    subtitle = "Hitta och boka dina nästa sporter och pass enkelt.";
    primaryLink = { to: "/activities", text: "Hitta aktiviteter" };
    secondaryLink = { to: "/my-bookings", text: "Mina bokningar" };
  } else {
    title = "ActiviGo";
    subtitle =
      "Boka ditt nästa pass snabbt och enkelt. Padel, Tennis, Fotboll - allt på ett ställe.";
    primaryLink = { to: "/activities", text: "Se tillgängliga tider" };
    secondaryLink = { to: "/login", text: "Logga in" };
  }

  return (
  <>
    <div className={`hero ${isAdmin ? "hero-admin" : ""}`}>
      <div className="hero-inner">
        <h1 className="hero-title">{title}</h1>
        <p className="hero-subtitle">{subtitle}</p>
        <div className="hero-actions">
          <Link to={primaryLink.to} className="btn btn-primary">
            {primaryLink.text}
          </Link>
          <Link to={secondaryLink.to} className="btn btn-secondary">
            {secondaryLink.text}
          </Link>
        </div>
      </div>
    </div>

    {isLoggedIn && !isAdmin && (
      <section className="homepage-calendar">
        <UserWeekCalendar />
      </section>
    )}
  </>
);

}
