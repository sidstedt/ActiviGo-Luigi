// ...imports
import React, { useEffect, useRef, useState } from "react";
import { NavLink } from "react-router-dom";
import { logout as apiLogout } from "../services/api";
import "../styles/HamburgerMenu.css"; 

export default function Sidebar({
  userRole = "guest",
  roles = [],
  brand = "ActivityGo",
}) {
  const [open, setOpen] = useState(false);
  const panelRef = useRef(null);
  const buttonRef = useRef(null);

  const isLoggedIn = Array.isArray(roles) && roles.length > 0;

  const baseMenu = [
    { title: "Hem", url: "/", icon: "🏠" },
    { title: "Aktiviteter", url: "/activities", icon: "🏃" },
  ];

  const userMenu = [
    { title: "Hem", url: "/", icon: "🏠" },
    { title: "Bokningar", url: "/bookings", icon: "📅" },
  ];

  // Only for authenticated users
  const authedMenu = [
    { title: "Sök & Boka", url: "/bookings", icon: "📅" },
    { title: "Mina bokningar", url: "/my-bookings", icon: "📋" },
    // Statistic link
    // { title: "Statistics", url: "/statistics", icon: "📊" },
    // { title: "Mitt konto", url: "/account", icon: "👤" },
    { title: "Mitt konto", url: "/my-account", icon: "👤" },
  ];
  const staffExtra = [{ title: "Personalpanel", url: "/staff", icon: "🛠️" }];
  const hasRole = (r) => roles.includes(r) || userRole === r;

  let menuItems = [...baseMenu];
  if (hasRole("admin")) {
    menuItems = [
      { title: "Hem", url: "/", icon: "🏠" },
      { title: "Aktiviteter", url: "/admin/activities", icon: "🏃" },
      { title: "Zoner", url: "/admin/zones", icon: "🟦"},
      { title: "Platser", url: "/admin/locations", icon: "📍"},
      { title: "Aktivitetsschema", url: "/admin/schedule", icon: "🗓️" },
      { title: "Användare", url: "/admin/users", icon: "👥" },
      { title: "Statistik", url: "/admin/statistics", icon: "📊" },
    ];
  } else if (hasRole("staff")) {
    menuItems = [...baseMenu, ...authedMenu, ...staffExtra];
  } else if (hasRole("user")) {
    menuItems = [...baseMenu, ...authedMenu];
  }

  useEffect(() => {
    const onKey = (e) => e.key === "Escape" && setOpen(false);
    document.addEventListener("keydown", onKey);
    document.body.style.overflow = open ? "hidden" : "";
    return () => {
      document.removeEventListener("keydown", onKey);
      document.body.style.overflow = "";
    };
  }, [open]);

  useEffect(() => {
    const onClickOutside = (e) => {
      if (open && panelRef.current && !panelRef.current.contains(e.target)) {
        setOpen(false);
        buttonRef.current?.focus();
      }
    };
    document.addEventListener("mousedown", onClickOutside);
    return () => document.removeEventListener("mousedown", onClickOutside);
  }, [open]);

  const handleLogout = async () => {
    try {
      await apiLogout();
    } finally {
      setOpen(false);
    }
  };

  return (
    <header className="hm-header" role="banner">
      <div className="hm-bar">
        {/* Burgarknapp med SVG (inga bilder/alt-texter) */}
        <button
          ref={buttonRef}
          className="hm-burger"
          aria-label={open ? "Stäng meny" : "Öppna meny"}
          aria-expanded={open}
          aria-controls="hm-drawer"
          onClick={() => setOpen((v) => !v)}
          type="button"
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 24 24"
            width="28"
            height="28"
            fill="none"
            stroke="currentColor"
            strokeWidth="2.5"
            strokeLinecap="round"
            strokeLinejoin="round"
            aria-hidden="true"
          >
            <line x1="3" y1="6" x2="21" y2="6" />
            <line x1="3" y1="12" x2="21" y2="12" />
            <line x1="3" y1="18" x2="21" y2="18" />
          </svg>
        </button>

        {/* Brand i topbaren (behålls) */}
        <NavLink to="/" className="hm-brand" onClick={() => setOpen(false)}>
          {brand}
        </NavLink>
      </div>

      {/* Backdrop */}
      <div className={`hm-backdrop ${open ? "open" : ""}`} />

      {/* Drawer */}
      <nav
        id="hm-drawer"
        className={`hm-drawer ${open ? "open" : ""}`}
        aria-hidden={!open}
        ref={panelRef}
      >
        <div className="hm-drawer-header">
          {/* Ta bort dubblett av brand här för att undvika “ActivityGo” x2 */}
          {/* <span className="hm-title">{brand}</span> */}
          <span className="sr-only" aria-hidden="true"></span>
          <button
            className="hm-close"
            onClick={() => setOpen(false)}
            aria-label="Stäng meny"
            type="button"
          >
            <svg
              width="26"
              height="26"
              viewBox="0 0 24 24"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
              aria-hidden="true"
            >
              <path
                d="M6 6l12 12M6 18L18 6"
                stroke="currentColor"
                strokeWidth="2"
                strokeLinecap="round"
              />
            </svg>
          </button>
        </div>

        <ul className="hm-list" role="menu">
          {menuItems.map((item) => (
            <li key={item.title} role="none">
              <NavLink
                to={item.url}
                className={({ isActive }) =>
                  `hm-link ${isActive ? "active" : ""}`
                }
                role="menuitem"
                onClick={() => setOpen(false)}
              >
                <span className="hm-icon" aria-hidden>
                  {item.icon || "•"}
                </span>
                <span className="hm-text">{item.title}</span>
              </NavLink>
            </li>
          ))}

          {!isLoggedIn ? (
            <li role="none">
              <NavLink
                to="/login"
                className="hm-link"
                role="menuitem"
                onClick={() => setOpen(false)}
              >
                <span className="hm-icon" aria-hidden>
                  🔐
                </span>
                <span className="hm-text">Logga in</span>
              </NavLink>
            </li>
          ) : (
            <li role="none">
              <button
                type="button"
                className="hm-link hm-logout"
                onClick={handleLogout}
                
              >
                <span className="hm-icon" aria-hidden>
                  ⎋
                </span>
                <span className="hm-text">Logga ut</span>
              </button>
            </li>
          )}
        </ul>

        <div className="hm-footer">
          <small>
            © {new Date().getFullYear()} {brand}
          </small>
        </div>
      </nav>
    </header>
  );
}
