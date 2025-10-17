import React from "react";
import { Outlet } from "react-router-dom";
import Sidebar from "../components/Sidebar.jsx";
import "../styles/StartPage.css";

// Static layout: Sidebar + body (Outlet). No CSS or API logic here.
export default function StartPage({ userRole = "guest", roles = [] }) {
  const [collapsed, setCollapsed] = React.useState(false);

  return (
    <div className={`app-layout ${collapsed ? "sidebar-collapsed" : ""}`}>
      <Sidebar
        userRole={userRole}
        roles={roles}
        collapsed={collapsed}
        onToggle={() => setCollapsed((c) => !c)}
      />
      <div className="app-body">
        <div className="top-header">
          <div className="brand">SportBook</div>
        </div>
        <Outlet />
      </div>
    </div>
  );
}