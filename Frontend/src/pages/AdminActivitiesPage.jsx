import { useEffect, useState } from "react";
import {
  fetchActivities,
  fetchZones,
  fetchLocations,
  deleteActivity,
  createActivity,
  updateActivity,
} from "../services/api";
import { getLocationAddressByZoneId } from "../utils/location";
import "../styles/ActivitiesPage.css";

export default function AdminActivitiesPage() {
  const [activities, setActivities] = useState([]);
  const [filteredActivities, setFilteredActivities] = useState([]);
  const [zones, setZones] = useState([]);
  const [locations, setLocations] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState("");
  const [debouncedSearchTerm, setDebouncedSearchTerm] = useState("");

  // Modal states
  const [showModal, setShowModal] = useState(false);
  const [editMode, setEditMode] = useState(false);
  const [selectedActivity, setSelectedActivity] = useState(null);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [deleteTarget, setDeleteTarget] = useState(null);

  useEffect(() => {
    loadData();
  }, []);

  useEffect(() => {
    applyFilters();
  }, [activities, debouncedSearchTerm]);

  useEffect(() => {
    const t = setTimeout(() => setDebouncedSearchTerm(searchTerm), 300);
    return () => clearTimeout(t);
  }, [searchTerm]);

  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);
      const [activitiesData, zonesData, locationsData] = await Promise.all([
        fetchActivities(),
        fetchZones(),
        fetchLocations(),
      ]);
      setActivities(activitiesData);
      setZones(zonesData);
      setLocations(locationsData);
    } catch (err) {
      setError(err.message || "Kunde inte hämta data");
      console.error("Fel vid hämtning av data:", err);
    } finally {
      setLoading(false);
    }
  };

  const applyFilters = () => {
    let filtered = [...activities];
    if (debouncedSearchTerm) {
      filtered = filtered.filter(
        (activity) =>
          activity.name
            .toLowerCase()
            .includes(debouncedSearchTerm.toLowerCase()) ||
          activity.description
            .toLowerCase()
            .includes(debouncedSearchTerm.toLowerCase())
      );
    }
    setFilteredActivities(filtered);
  };

  const clearFilters = () => {
    setSearchTerm("");
  };

  const getLocationAddress = (zoneId) =>
    getLocationAddressByZoneId(zones, locations, zoneId);

  const handleAdd = () => {
    setSelectedActivity({
      name: "",
      description: "",
      maxParticipants: "",
      durationMinutes: "",
      zoneId: "",
    });
    setEditMode(false);
    setShowModal(true);
  };

  const handleEdit = (activity) => {
    setSelectedActivity(activity);
    setEditMode(true);
    setShowModal(true);
  };

  const handleDelete = (activity) => {
    setDeleteTarget(activity);
    setShowDeleteModal(true);
  };

  const confirmDelete = async () => {
    if (!deleteTarget) return;
    try {
      await deleteActivity(deleteTarget.id);
      setActivities((prev) => prev.filter((a) => a.id !== deleteTarget.id));
      setShowDeleteModal(false);
      setDeleteTarget(null);
    } catch (err) {
      alert("Fel vid borttagning: " + err.message);
    }
  };

  const handleSave = async (formData) => {
    try {
      if (editMode) {
        const updated = await updateActivity(formData);
        setActivities((prev) =>
          prev.map((a) => (a.id === updated.id ? updated : a))
        );
      } else {
        const created = await createActivity(formData);
        setActivities((prev) => [...prev, created]);
      }
      setShowModal(false);
      setSelectedActivity(null);
    } catch (err) {
      alert("Fel vid sparande: " + err.message);
    }
  };

  if (loading) {
    return (
      <div className="activities-page">
        <div className="loading">
          <div className="spinner"></div>
          <p>Laddar aktiviteter...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="activities-page">
      <header className="page-header">
        <h1>Hantera Aktiviteter</h1>
        <p>Här kan du lägga till, ändra eller ta bort aktiviteter</p>
        <button className="add-activity-btn" onClick={handleAdd}>
          ➕ Lägg till ny aktivitet
        </button>
      </header>

      {/* Filter Section */}
      <div className="filters-section">
        <div className="filters-grid">
          <div className="filter-group">
            <label htmlFor="search">Sök aktivitet</label>
            <input
              id="search"
              type="text"
              placeholder="Sök efter namn eller beskrivning..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="filter-input"
            />
          </div>
          <div className="filter-group">
            <button onClick={clearFilters} className="clear-filters-btn">
              Rensa filter
            </button>
          </div>
        </div>
      </div>

      {/* Results */}
      <div className="activities-grid">
        {filteredActivities.map((activity) => (
          <div key={activity.id} className="activity-card">
            <div className="activity-header">
              <h3 className="activity-name">{activity.name}</h3>
            </div>
            <div className="activity-content">
              <p className="activity-description">{activity.description}</p>
              <div className="activity-details">
                <div className="detail-item">
                  <span className="detail-label">Adress:</span>
                  <span className="detail-value">
                    {getLocationAddress(activity.zoneId) || "Okänd adress"}
                  </span>
                </div>
                <div className="detail-item">
                  <span className="detail-label">Max deltagare:</span>
                  <span className="detail-value">
                    {activity.maxParticipants}
                  </span>
                </div>
                <div className="detail-item">
                  <span className="detail-label">Längd:</span>
                  <span className="detail-value">
                    {activity.durationMinutes} min
                  </span>
                </div>
              </div>
            </div>
            <div className="activity-footer">
              <button className="edit-btn" onClick={() => handleEdit(activity)}>
                ✏️ Ändra
              </button>
              <button
                className="delete-btn"
                onClick={() => handleDelete(activity)}
              >
                🗑️ Ta bort
              </button>
            </div>
          </div>
        ))}
      </div>

      {/* Modaler */}
      {showModal && (
        <ActivityModal
          activity={selectedActivity}
          onClose={() => setShowModal(false)}
          onSave={handleSave}
          editMode={editMode}
        />
      )}

      {showDeleteModal && (
        <ConfirmDeleteModal
          onConfirm={confirmDelete}
          onCancel={() => setShowDeleteModal(false)}
          name={deleteTarget?.name}
        />
      )}
    </div>
  );
}

/* --- MODAL KOMPONENTER --- */
function ActivityModal({ activity, onClose, onSave, editMode }) {
  const [formData, setFormData] = useState(activity);

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  return (
    <div className="modal-backdrop">
      <div className="modal">
        <h2>{editMode ? "Redigera aktivitet" : "Ny aktivitet"}</h2>

        <label>Namn:</label>
        <input name="name" value={formData.name} onChange={handleChange} />

        <label>Beskrivning:</label>
        <textarea
          name="description"
          value={formData.description}
          onChange={handleChange}
        />

        <label>Max deltagare:</label>
        <input
          name="maxParticipants"
          type="number"
          value={formData.maxParticipants}
          onChange={handleChange}
        />

        <label>Längd (minuter):</label>
        <input
          name="durationMinutes"
          type="number"
          value={formData.durationMinutes}
          onChange={handleChange}
        />

        <div className="modal-actions">
          <button onClick={() => onSave(formData)} className="save-btn">
            💾 Spara
          </button>
          <button onClick={onClose} className="cancel-btn">
            Avbryt
          </button>
        </div>
      </div>
    </div>
  );
}

function ConfirmDeleteModal({ onConfirm, onCancel, name }) {
  return (
    <div className="modal-backdrop">
      <div className="modal small">
        <h3>Ta bort aktivitet?</h3>
        <p>
          Är du säker på att du vill ta bort <strong>{name}</strong>?
        </p>
        <div className="modal-actions">
          <button onClick={onConfirm} className="delete-btn">
            Ja, ta bort
          </button>
          <button onClick={onCancel} className="cancel-btn">
            Avbryt
          </button>
        </div>
      </div>
    </div>
  );
}
