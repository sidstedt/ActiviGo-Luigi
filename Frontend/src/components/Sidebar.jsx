import React from "react";
import { NavLink } from "react-router-dom";
import "../styles/Sidebar.css";
import { logout } from "../services/api";
import sidebarIcon from "../assets/weather/sidebaricon/image.png";

// Collapsible, role-based sidebar (no CSS here)
export default function Sidebar({
  userRole = "guest",
  roles = [],
  collapsed = false,
  onToggle,
}) {
  const isLoggedIn = Array.isArray(roles) && roles.length > 0;

  // Always visible
  const baseMenu = [
    { title: "Hem", url: "/", icon: "🏠" },
    { title: "Aktiviteter", url: "/activities", icon: "🏃" },
  ];

  const userMenu = [
    { title: "Hem", url: "/", icon: "🏠" },
    { title: "Bokningar", url: "/bookings", icon: "📅" },
  ]
  // Only for authenticated users
  const authedMenu = [
    { title: "Sök & Boka", url: "/bookings", icon: "📅" },
    { title: "Mina bokningar", url: "/my-bookings", icon: "📋" },
    // Statistic link
    { title: "Statistics", url: "/statistics", icon: "📊" },
    { title: "Mitt konto", url: "/account", icon: "👤" },
  ];

  const staffExtra = [{ title: "Personalpanel", url: "/staff", icon: "🛠️" }];

  const hasRole = (r) => roles.includes(r) || userRole === r;
  let menuItems = [...baseMenu];
  if (hasRole("admin")) {
    menuItems = [
      { title: "Hem", url: "/", icon: "🏠" },
      { title: "Aktiviteter", url: "/admin/activities", icon: "🏃" },
      { title: "Aktivitetsschema", url: "/admin/schedule", icon: "🗓️" },
      { title: "Användare", url: "/admin/users", icon: "👥" },
      { title: "Statistik", url: "/admin/statistics", icon: "📊" },
    ];
  } else if (hasRole("staff")) {
    menuItems = [...baseMenu, ...authedMenu, ...staffExtra];
  } else if (hasRole("user")) {
    menuItems = [...baseMenu, ...authedMenu];
  } else {
    menuItems = [...baseMenu];
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
              `nav-item ${isActive ? "active" : ""} ${
                collapsed ? "is-collapsed" : ""
              }`
            }
            title={collapsed ? item.title : undefined}
          >
            <span className="icon" aria-hidden="true">
              {item.icon || "•"}
            </span>
            {!collapsed && <span>{item.title}</span>}
          </NavLink>
        ))}
        {!isLoggedIn && (
          <NavLink
            to="/login"
            className={`nav-item ${collapsed ? "is-collapsed" : ""}`}
            title={collapsed ? "Logga in" : undefined}
          >
            <span className="icon" aria-hidden="true">
              🔐
            </span>
            {!collapsed && <span>Logga in</span>}
          </NavLink>
        )}
        {isLoggedIn && (
          <button
            type="button"
            onClick={logout}
            className={`nav-item ${collapsed ? "is-collapsed" : ""}`}
            title={collapsed ? "Logga ut" : undefined}
          >
            <span className="icon" aria-hidden="true">
              ⎋
            </span>
            {!collapsed && <span>Logga ut</span>}
          </button>
        )}
      </nav>
    </aside>
  );
}
