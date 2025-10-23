import React, { useEffect, useState } from "react";
import LocationModal from "../components/LocationModal";
import {
  fetchLocations,
  createLocation,
  updateLocation,
  deleteLocation,
} from "../services/api";
import "../styles/ActivitiesPage.css";

export default function LocationPage() {
  const [locations, setLocations] = useState([]);
  const [filteredLocations, setFilteredLocations] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingLocation, setEditingLocation] = useState(null);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [deleteTarget, setDeleteTarget] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    loadData();
  }, []);

  async function loadData() {
    try {
      setLoading(true);
      const locationsData = await fetchLocations();
      setLocations(Array.isArray(locationsData) ? locationsData : []);
      setFilteredLocations(Array.isArray(locationsData) ? locationsData : []);
    } catch (err) {
      setError("Fel vid hämtning av platser");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    let filtered = [...locations];
    if (searchTerm) {
      const s = searchTerm.toLowerCase();
      filtered = filtered.filter((l) => (l.name || "").toLowerCase().includes(s));
    }
    setFilteredLocations(filtered);
  }, [locations, searchTerm]);

  const handleOpenNew = () => {
    setEditingLocation(null);
    setIsModalOpen(true);
  };

  const handleSaveLocation = async (payload) => {
    try {
      if (editingLocation) {
        await updateLocation(editingLocation.id, payload);
      } else {
        await createLocation(payload);
      }
      setIsModalOpen(false);
      setEditingLocation(null);
      await loadData();
    } catch (err) {
      alert("Misslyckades spara plats. Se konsolen för detaljer.");
    }
  };

  const handleEdit = (location) => {
    setEditingLocation(location);
    setIsModalOpen(true);
  };

  const handleDelete = (location) => {
    setDeleteTarget(location);
    setShowDeleteModal(true);
  };

  const confirmDelete = async () => {
    if (!deleteTarget) return;
    try {
      await deleteLocation(deleteTarget.id);
      setLocations((prev) => prev.filter((l) => l.id !== deleteTarget.id));
      setShowDeleteModal(false);
      setDeleteTarget(null);
    } catch (err) {
      alert("Fel vid borttagning: " + (err.message || err));
    }
  };

  if (loading) return <p>Laddar platser...</p>;
  if (error) return <p>{error}</p>;

  return (
    <div className="activities-page">
      <header className="page-header">
        <h1>Hantera Platser</h1>
        <p>Här kan du lägga till, ändra eller ta bort platser</p>
        <button className="btn-primary" onClick={handleOpenNew}>
          ➕ Lägg till ny plats
        </button>
      </header>

      {/* Filter */}
      <div className="filters-section">
        <div className="filters-grid">
          <div className="filter-group">
            <label htmlFor="search">Sök plats</label>
            <input
              id="search"
              type="text"
              placeholder="Sök efter plats-namn..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="filter-input"
            />
          </div>
          <div className="filter-group">
            <button
              onClick={() => setSearchTerm("")}
              className="clear-filters-btn"
            >
              Rensa filter
            </button>
          </div>
        </div>
      </div>

      {/* Plats-korten */}
      <div className="activities-grid">
        {filteredLocations.map((location) => (
          <div key={location.id} className="activity-card">
            <div className="activity-header">
              <h3>{location.name}</h3>
            </div>
            <div className="activity-content">
              <p><strong>Adress:</strong> {location.address}</p>
              <p><strong>Lat:</strong> {location.latitude}</p>
              <p><strong>Long:</strong> {location.longitude}</p>
              {location.description && <p><strong>Beskrivning:</strong> {location.description}</p>}
            </div>
            <div className="activity-footer">
              <button className="edit-btn" onClick={() => handleEdit(location)}>
                ✏️ Ändra
              </button>
              <button className="delete-btn" onClick={() => handleDelete(location)}>
                🗑️ Ta bort
              </button>
            </div>
          </div>
        ))}
      </div>

      {/* Modal */}
      {isModalOpen && (
        <LocationModal
          editing={editingLocation}
          initialData={editingLocation}
          onClose={() => setIsModalOpen(false)}
          onSave={handleSaveLocation}
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

function ConfirmDeleteModal({ name, onConfirm, onCancel }) {
  return (
    <div className="modal-backdrop">
      <div className="modal small">
        <h3>Ta bort plats?</h3>
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
