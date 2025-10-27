import { useEffect, useState } from "react";
import {
  fetchAllUsers,
  createAdminUser,
  updateAdminUser,
  deleteAdminUser,
  fetchUserBookings,
} from "../services/api";
import "../styles/ActivitiesPage.css";
import "../styles/ActivityCard.css";

const getUserName = (u) => {
  if (!u) return "";
  const fn = u.firstName ?? u.first_name ?? u.firstname;
  const ln = u.lastName ?? u.last_name ?? u.lastname;
  if (fn || ln) return `${fn ?? ""}${ln ? " " + ln : ""}`.trim();
  return u.name ?? u.displayName ?? u.username ?? u.userName ?? u.email ?? "";
};

const getUserEmail = (u) =>
  u?.email ?? u?.Email ?? u?.mail ?? u?.username ?? "";

const getUserRoles = (u) => {
  if (!u) return [];

  const normalizeItem = (it) =>
    typeof it === "string"
      ? it.trim()
      : it?.name ?? it?.role ?? it?.roleName ?? it?.value ?? null;

  let roles = [];

  if (Array.isArray(u.roles) && u.roles.length) {
    roles = u.roles.map(normalizeItem).filter(Boolean);
  } else if (Array.isArray(u.Roles) && u.Roles.length) {
    roles = u.Roles.map(normalizeItem).filter(Boolean);
  } else if (typeof u.roles === "string") {
    roles = u.roles
      .split(",")
      .map((r) => r.trim())
      .filter(Boolean);
  } else if (Array.isArray(u.roleNames)) {
    roles = u.roleNames.map(String).filter(Boolean);
  } else if (Array.isArray(u.userRoles)) {
    roles = u.userRoles.map(normalizeItem).filter(Boolean);
  } else if (Array.isArray(u.rolesList)) {
    roles = u.rolesList.map(normalizeItem).filter(Boolean);
  } else if (u.role) {
    roles = [String(u.role)];
  } else if (Array.isArray(u.claims)) {
    roles = u.claims
      .filter(
        (c) =>
          c &&
          ((c.type && /role/i.test(c.type)) ||
            (c.name && /role/i.test(c.name)) ||
            (c.claimType && /role/i.test(c.claimType)))
      )
      .map((c) => normalizeItem(c) || (c.value ?? c.claim ?? ""))
      .filter(Boolean);
  }

  return Array.from(new Set(roles));
};

const getUserIsActive = (u) => {
  if (!u) return false;
  if (typeof u.isActive === "boolean") return u.isActive;
  if (typeof u.active === "boolean") return u.active;
  if (typeof u.isActive === "string") return u.isActive === "true";
  if (typeof u.status === "string") return /active|aktiv/i.test(u.status);
  return Boolean(u?.enabled ?? u?.isEnabled ?? u?.isActive ?? u?.active);
};

const getUserCreatedAt = (u) =>
  u?.createdAt ??
  u?.created ??
  u?.created_on ??
  u?.createdOn ??
  u?.created_at ??
  null;

export default function AdminUsersPage() {
  const [users, setUsers] = useState([]);
  const [filteredUsers, setFilteredUsers] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [editingUser, setEditingUser] = useState(null);
  const [isDetailsOpen, setIsDetailsOpen] = useState(false);
  const [detailsUser, setDetailsUser] = useState(null);
  const [detailsLoading, setDetailsLoading] = useState(false);
  const [detailsBookings, setDetailsBookings] = useState([]);
  const [showDeleteConfirm, setShowDeleteConfirm] = useState(false);
  const [deleteTarget, setDeleteTarget] = useState(null);

  useEffect(() => {
    loadUsers();
  }, []);

  useEffect(() => {
    const s = (searchTerm || "").toLowerCase().trim();
    if (!s) setFilteredUsers(users);
    else
      setFilteredUsers(
        users.filter(
          (u) =>
            getUserName(u).toLowerCase().includes(s) ||
            getUserEmail(u).toLowerCase().includes(s) ||
            getUserRoles(u).join(" ").toLowerCase().includes(s)
        )
      );
  }, [users, searchTerm]);

  async function loadUsers() {
    try {
      setLoading(true);
      setError(null);
      const data = await fetchAllUsers();

      setUsers(Array.isArray(data) ? data : []);
      setFilteredUsers(Array.isArray(data) ? data : []);
    } catch (err) {
      setError("Kunde inte hämta användare");
    } finally {
      setLoading(false);
    }
  }

  function openNewUser() {
    setEditingUser({
      id: null,
      name: "",
      email: "",
      roles: [],
      isActive: true,
      createdAt: null,
      _raw: null,
    });
    setIsEditModalOpen(true);
  }

  function handleEdit(user) {
    setEditingUser({
      id: user?.id ?? user?.userId ?? null,
      name: getUserName(user),
      email: getUserEmail(user),
      roles: getUserRoles(user),
      isActive: getUserIsActive(user),
      createdAt: getUserCreatedAt(user),
      _raw: user,
    });
    setIsEditModalOpen(true);
  }

  async function handleSaveUser(payload) {
    try {
      if (!payload) return;
      const apiPayload = { ...payload };
      if (!apiPayload.firstName && apiPayload.name) {
        const parts = String(apiPayload.name).trim().split(/\s+/);
        apiPayload.firstName = parts.shift() || "";
        apiPayload.lastName = parts.join(" ") || "";
      }
      delete apiPayload._raw;
      if (payload.id) {
        await updateAdminUser(payload.id, apiPayload);
      } else {
        await createAdminUser(apiPayload);
      }
      setIsEditModalOpen(false);
      setEditingUser(null);
      await loadUsers();
    } catch (err) {
      setIsEditModalOpen(false);
      setEditingUser(null);
      try {
        await loadUsers();
      } catch (_) {
        /* ignore */
      }
    }
  }

  function handleDelete(user) {
    setDeleteTarget(user);
    setShowDeleteConfirm(true);
  }

  async function confirmDelete() {
    if (!deleteTarget) return;
    try {
      await deleteAdminUser(deleteTarget.id);
    } catch (err) {
      // ignore errors from server-delete response here (frontend will sync below)
    } finally {
      setShowDeleteConfirm(false);
      setUsers((prev) => prev.filter((u) => u.id !== deleteTarget.id));
      setDeleteTarget(null);
      try {
        await loadUsers();
      } catch (_) {
        /* ignore */
      }
    }
  }

  async function openDetails(user) {
    setIsDetailsOpen(true);
    setDetailsLoading(true);
    try {
      setDetailsUser(user);
      const idForBookings = user?.id ?? user?.Id ?? user?.userId;
      const bookings = idForBookings
        ? await fetchUserBookings(idForBookings)
        : [];
      setDetailsBookings(Array.isArray(bookings) ? bookings : []);
    } catch (err) {
      setDetailsBookings([]);
    } finally {
      setDetailsLoading(false);
    }
  }

  if (loading)
    return (
      <div className="activities-page">
        <p>Laddar användare...</p>
      </div>
    );
  if (error)
    return (
      <div className="activities-page">
        <p>{error}</p>
      </div>
    );

  return (
    <div className="activities-page">
      <header className="page-header">
        <h1>Hantera Användare</h1>
        <p>Översikt och administration av systemanvändare</p>
      </header>

      <div className="filters-section">
        <div className="filters-grid">
          <div className="filter-group">
            <label htmlFor="searchUser">Sök användare</label>
            <input
              id="searchUser"
              className="filter-input"
              placeholder="Namn, e-post eller roll..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>

          <div className="filter-group">
            <button
              className="clear-filters-btn"
              onClick={() => setSearchTerm("")}
            >
              Rensa filter
            </button>
          </div>
        </div>
      </div>

      <div className="results-section">
        <div className="activities-grid">
          {filteredUsers.map((user) => (
            <div key={user.id} className="activity-card">
              <div className="activity-header">
                <h3 className="activity-title">
                  {getUserName(user) || "Namnlös"}
                </h3>
                <div className="activity-subtitle">{getUserEmail(user)}</div>
              </div>

              <div className="activity-content">
                <div className="activity-details">
                  <div className="detail-item">
                    <div className="detail-label">Roller</div>
                    <div className="detail-value">
                      {getUserRoles(user).length
                        ? getUserRoles(user).join(", ")
                        : "-"}
                    </div>
                  </div>

                  <div className="detail-item">
                    <div className="detail-label">Status</div>
                    <div className="detail-value">
                      {getUserIsActive(user) ? "Aktiv" : "Inaktiv"}
                    </div>
                  </div>

                  <div className="detail-item">
                    <div className="detail-label">Skapad</div>
                    <div className="detail-value">
                      {getUserCreatedAt(user)
                        ? new Date(getUserCreatedAt(user)).toLocaleString()
                        : "-"}
                    </div>
                  </div>
                </div>
              </div>

              <div className="activity-footer">
                <div className="card-actions">
                  <button
                    className="view-details-btn"
                    onClick={() => openDetails(user)}
                  >
                    Visa
                  </button>
                </div>
              </div>
            </div>
          ))}
          {filteredUsers.length === 0 && (
            <div style={{ padding: 20 }}>Inga användare hittades.</div>
          )}
        </div>
      </div>

      {isEditModalOpen && editingUser && (
        <EditUserModal
          initial={editingUser}
          onClose={() => {
            setIsEditModalOpen(false);
            setEditingUser(null);
          }}
          onSave={handleSaveUser}
        />
      )}

      {isDetailsOpen && detailsUser && (
        <div className="modal-backdrop">
          <div className="modal large">
            <h3>{getUserName(detailsUser) || getUserEmail(detailsUser)}</h3>
            <p>
              <strong>E-post:</strong> {getUserEmail(detailsUser)}
            </p>
            <p>
              <strong>Roller:</strong>{" "}
              {getUserRoles(detailsUser).length
                ? getUserRoles(detailsUser).join(", ")
                : "-"}
            </p>
            <p>
              <strong>Status:</strong>{" "}
              {getUserIsActive(detailsUser) ? "Aktiv" : "Inaktiv"}
            </p>

            <h4>Bokningar</h4>
            {detailsLoading ? (
              <p>Laddar bokningar...</p>
            ) : detailsBookings.length === 0 ? (
              <p>Inga bokningar hittades.</p>
            ) : (
              <div style={{ maxHeight: 260, overflow: "auto" }}>
                <ul>
                  {detailsBookings.map((b) => (
                    <li key={b.id || b.bookingId}>
                      <strong>{b.activityName || b.title}</strong> —{" "}
                      {b.startTime
                        ? new Date(b.startTime).toLocaleString()
                        : b.date}
                      <div style={{ fontSize: 12, color: "#666" }}>
                        {b.status || ""}
                      </div>
                    </li>
                  ))}
                </ul>
              </div>
            )}

            <div className="modal-actions">
              <button
                className="edit-btn"
                onClick={() => {
                  setIsDetailsOpen(false);
                  handleEdit(detailsUser);
                }}
              >
                Ändra
              </button>

              <button
                className="delete-btn"
                onClick={() => {
                  setIsDetailsOpen(false);
                  setDeleteTarget(detailsUser);
                  setShowDeleteConfirm(true);
                }}
              >
                Ta bort
              </button>

              <button
                onClick={() => setIsDetailsOpen(false)}
                className="cancel-btn"
              >
                Stäng
              </button>
            </div>
          </div>
        </div>
      )}

      {showDeleteConfirm && deleteTarget && (
        <div className="modal-backdrop">
          <div className="modal confirm-delete">
            <h3>Bekräfta borttagning</h3>
            <p>
              Är du säker på att du vill ta bort användaren{" "}
              <strong>
                {getUserName(deleteTarget) || getUserEmail(deleteTarget)}
              </strong>
              ?
            </p>

            <div className="modal-actions">
              <button
                className="btn-secondary"
                onClick={() => setShowDeleteConfirm(false)}
              >
                Avbryt
              </button>
              <button className="btn-danger" onClick={confirmDelete}>
                Ta bort
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

function EditUserModal({ initial = {}, onClose, onSave }) {
  const [form, setForm] = useState({ ...initial });

  useEffect(() => {
    setForm({ ...initial });
  }, [initial]);

  const updateField = (key, value) => setForm((f) => ({ ...f, [key]: value }));

  const submit = (e) => {
    e.preventDefault();
    const payload = {
      ...form,
      roles: Array.isArray(form.roles)
        ? form.roles
        : form.roles
        ? String(form.roles)
            .split(",")
            .map((r) => r.trim())
        : [],
    };
    onSave(payload);
  };

  return (
    <div className="modal-backdrop">
      <div className="modal large">
        <h3>{form.id ? "Redigera användare" : "Skapa användare"}</h3>

        <form
          onSubmit={submit}
          style={{ display: "grid", gap: 12, marginTop: 12 }}
        >
          <label>Namn</label>
          <input
            className="filter-input"
            value={form.name ?? ""}
            onChange={(e) => updateField("name", e.target.value)}
          />

          <label>E-post</label>
          <input
            className="filter-input"
            value={form.email ?? ""}
            onChange={(e) => updateField("email", e.target.value)}
          />

          <label>Roller (kommaseparerat)</label>
          <input
            className="filter-input"
            value={
              Array.isArray(form.roles)
                ? form.roles.join(", ")
                : form.roles ?? ""
            }
            onChange={(e) => updateField("roles", e.target.value)}
          />

          <label style={{ display: "flex", alignItems: "center", gap: 8 }}>
            <input
              type="checkbox"
              checked={!!form.isActive}
              onChange={(e) => updateField("isActive", !!e.target.checked)}
            />
            Aktiv
          </label>

          <div
            className="modal-actions"
            style={{ marginTop: 8, justifyContent: "flex-end" }}
          >
            <button type="submit" className="edit-btn">
              {form.id ? "Spara" : "Skapa"}
            </button>
            <button type="button" onClick={onClose} className="cancel-btn">
              Avbryt
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
