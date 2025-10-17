import React from "react";
import { NavLink } from "react-router-dom";
import "../styles/Sidebar.css";
import { logout } from "../services/api";
import sidebarIcon from "../assets/weather/sidebaricon/image.png";

// Collapsible, role-based sidebar (no CSS here)
export default function Sidebar({ userRole = "guest", roles = [], collapsed = false, onToggle }) {
  const isLoggedIn = Array.isArray(roles) && roles.length > 0;

  const guestMenu = [
    { title: "Hem", url: "/", icon: "🏠" },
    { title: "Logga in", url: "/login", icon: "🔐" },
  ];

  const userMenu = [
    { title: "Hem", url: "/", icon: "🏠" },
    { title: "Bokningar", url: "/bookings", icon: "📅" },
    { title: "Mitt konto", url: "/account", icon: "👤" },
  ];

  const staffExtra = [
    { title: "Personalpanel", url: "/staff", icon: "🛠️" },
  ];

  const adminExtra = [
    { title: "Admin", url: "/admin", icon: "⚙️" },
  ];

  const hasRole = (r) => roles.includes(r) || userRole === r;
  let menuItems = guestMenu;
  if (hasRole("user") || hasRole("staff") || hasRole("admin")) {
    menuItems = userMenu;
  }
  if (hasRole("staff")) {
    menuItems = [...menuItems, ...staffExtra];
  }
  if (hasRole("admin")) {
    menuItems = [...menuItems, ...adminExtra];
  }

  return (
    <aside className={`sidebar ${collapsed ? "collapsed" : ""}`}>
      <div className="sidebar-header">
        {!collapsed && <div className="brand">ActivityGo</div>}
        <button
          type="button"
          onClick={onToggle}
          aria-expanded={!collapsed}
          aria-label={collapsed ? "Expandera sidomeny" : "Kollapsa sidomeny"}
          className="collapse-btn"
        >
          <img src={sidebarIcon} alt="toggle sidebar" width={18} height={18} />
        </button>
      </div>

      <nav className="nav">
        {menuItems.map((item) => (
          <NavLink
            key={item.title}
            to={item.url}
            className={({ isActive }) =>
              `nav-item ${isActive ? "active" : ""} ${collapsed ? "is-collapsed" : ""}`
            }
            title={collapsed ? item.title : undefined}
          >
            <span className="icon" aria-hidden="true">{item.icon || "•"}</span>
            {!collapsed && <span>{item.title}</span>}
          </NavLink>
        ))}
        {isLoggedIn && (
          <button
            type="button"
            onClick={logout}
            className={`nav-item ${collapsed ? "is-collapsed" : ""}`}
            title={collapsed ? "Logga ut" : undefined}
          >
            <span className="icon" aria-hidden="true">⎋</span>
            {!collapsed && <span>Logga ut</span>}
          </button>
        )}
      </nav>
    </aside>
  );
}
