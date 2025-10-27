import React from "react";
import { Outlet } from "react-router-dom";
import Sidebar from "../components/Sidebar";
import "../styles/StartPage.css";
import { getCurrentUser } from "../services/api";
export default function StartPage({ userRole = "guest", roles = [] }) {
  const [collapsed, setCollapsed] = React.useState(false);
  const [userName, setUserName] = React.useState("");
  const [isAdmin, setIsAdmin] = React.useState(false);
  
  React.useEffect(() => {
    const user = getCurrentUser();
        if (user && user.username) {
            setUserName(user.username);
        } else {
        setUserName("Användare");
}

}, []);

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
            <div className="brand">ActiviGo</div>

        <div className="user-info">Välkommen, {userName}!</div>
        </div>
        <Outlet />
        </div>
    </div>
    );
}