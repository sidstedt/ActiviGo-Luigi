import React, { useState, useEffect } from "react";
import {
  fetchActivities,
  fetchZones,
  fetchLocations,
  createActivity,
  updateActivity,
  deleteActivity,
} from "../services/api";
import { getLocationAddressByZoneId } from "../utils/location";
import ActivityModal from "../components/ActivityModal";
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

  const [showModal, setShowModal] = useState(false);
  const [editingActivity, setEditingActivity] = useState(null);
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

  async function loadData() {
    try {
      setLoading(true);
      const [activitiesData, zonesData, locationsData] = await Promise.all([
        fetchActivities(),
        fetchZones(),
        fetchLocations(),
      ]);
      setActivities(activitiesData);
      setZones(zonesData);
      setLocations(locationsData);
    } catch (err) {
      setError("Kunde inte hämta aktiviteter");
      console.error(err);
    } finally {
      setLoading(false);
    }
  }

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

  const clearFilters = () => setSearchTerm("");

  const getLocationAddress = (zoneId) =>
    getLocationAddressByZoneId(zones, locations, zoneId);

  const handleAdd = () => {
    setEditingActivity(null);
    setShowModal(true);
  };

  const handleEdit = (activity) => {
    setEditingActivity(activity);
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
      if (editingActivity) {
        const updated = await updateActivity(editingActivity.id, formData);
        setActivities((prev) =>
          prev.map((a) => (a.id === updated.id ? updated : a))
        );
      } else {
        const created = await createActivity(formData);
        setActivities((prev) => [...prev, created]);
      }
      setShowModal(false);
      setEditingActivity(null);
    } catch (err) {
      alert("Fel vid sparande: " + err.message);
    }
  };

  if (loading) return <p>Laddar aktiviteter...</p>;
  if (error) return <p>{error}</p>;

  return (
    <div className="activities-page">
      <header className="page-header">
        <h1>Hantera Aktiviteter</h1>
        <p>Här kan du lägga till, ändra eller ta bort aktiviteter</p>
        <button className="btn-primary" onClick={handleAdd}>
          ➕ Lägg till ny aktivitet
        </button>
      </header>

      {/* Filter */}
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

      {/* Aktivitetskorten */}
      <div className="activities-grid">
        {filteredActivities.map((activity) => (
          <div key={activity.id} className="activity-card">
            <div className="activity-header">
              <h3>{activity.name}</h3>
            </div>
            <div className="activity-content">
              <p>{activity.description}</p>
              <div className="activity-details">
                <div>
                  <strong>Adress:</strong>{" "}
                  {getLocationAddress(activity.zoneId) || "Okänd"}
                </div>
                <div>
                  <strong>Max deltagare:</strong> {activity.maxParticipants}
                </div>
                <div>
                  <strong>Längd:</strong> {activity.durationMinutes} min
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

      {/* Modal */}
      {showModal && (
        <ActivityModal
          editing={editingActivity}
          initialData={editingActivity}
          onClose={() => setShowModal(false)}
          onSave={handleSave}
        />
      )}

      {showDeleteModal && deleteTarget && (
        <ConfirmDeleteModal
          name={deleteTarget.name}
          onConfirm={confirmDelete}
          onCancel={() => setShowDeleteModal(false)}
        />
      )}
    </div>
  );
}

/* --- Bekräfta borttagning --- */
function ConfirmDeleteModal({ name, onConfirm, onCancel }) {
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
