import React from "react";
import {
  Routes,
  Route,
  Navigate,
  BrowserRouter,
  useInRouterContext,
} from "react-router-dom";
import StartPage from "./pages/StartPage.jsx";
import HomePage from "./pages/HomePage.jsx";
import ActivitiesPage from "./pages/ActivitiesPage.jsx";
import ActivityOccurrencesPage from "./pages/ActivityOccurrencesPage.jsx";
import MyBookingsPage from "./pages/MyBookingsPage.jsx";
import LoginPage from "./pages/LoginPage.jsx";
// imported statistics page
import Statistics from "./pages/Statistics.jsx";
import AdminSchedulePage from "./pages/AdminSchedulePage.jsx";
import AdminActivitiesPage from "./pages/AdminActivitiesPage.jsx";

const DashboardHome = () => <HomePage />;
const AdminDashboard = () => <div>Adminpanel</div>;
const StaffPanel = () => <div>Personalpanel</div>;

function RoleRoute({ allowed = [], roles = [], children }) {
  if (allowed.length === 0) return children;
  const hasAccess = roles.some((r) => allowed.includes(r));
  if (hasAccess) return children;
  return <Navigate to="/" replace />;
}

function AppRoutes() {
  const storedUser = (() => {
    try {
      const raw = localStorage.getItem("user");
      return raw ? JSON.parse(raw) : null;
    } catch {
      return null;
    }
  })();

  const roles = Array.isArray(storedUser?.roles)
    ? storedUser.roles.map((r) => String(r).toLowerCase())
    : [];
  const userRole = roles.includes("admin")
    ? "admin"
    : roles.includes("staff")
    ? "staff"
    : roles.includes("user")
    ? "user"
    : "guest";

  return (
    <Routes>
      <Route path="/" element={<StartPage userRole={userRole} roles={roles} />}>
        <Route index element={<DashboardHome />} />

        <Route path="activities" element={<ActivitiesPage />} />
        <Route path="bookings" element={<ActivityOccurrencesPage />} />
        <Route path="login" element={<LoginPage />} />
        {/* Route for statistics-page */}
        <Route path="statistics" element={<Statistics />} />
        <Route path="activities" element={<ActivitiesPage />} />
        <Route path="bookings" element={<ActivityOccurrencesPage />} />
        <Route path="my-bookings" element={<MyBookingsPage />} />
        <Route path="login" element={<LoginPage />} />

        <Route
          path="admin"
          element={
            <RoleRoute allowed={["admin"]} roles={roles}>
              <AdminDashboard />
            </RoleRoute>
          }
        />
        <Route
          path="admin/schedule"
          element={
            <RoleRoute allowed={["admin"]} roles={roles}>
              <AdminSchedulePage />
            </RoleRoute>
          }
        />
        <Route
          path="admin/activities"
          element={
            <RoleRoute allowed={["admin"]} roles={roles}>
              <AdminActivitiesPage />
            </RoleRoute>
          }
        />
        <Route
          path="staff"
          element={
            <RoleRoute allowed={["staff", "admin"]} roles={roles}>
              <StaffPanel />
            </RoleRoute>
          }
        />
        <Route
          path="admin/statistics"
          element={
            <RoleRoute allowed={["admin"]} roles={roles}>
              <Statistics />
            </RoleRoute>
          }
        />

        <Route path="*" element={<Navigate to="/" replace />} />
      </Route>
    </Routes>
  );
}

export default function App() {
  const isInRouter = useInRouterContext();
  // Fallback: auto-wrap in BrowserRouter if rendered outside a Router
  if (!isInRouter) {
    return (
      <BrowserRouter>
        <AppRoutes />
      </BrowserRouter>
    );
  }

  return <AppRoutes />;
}
