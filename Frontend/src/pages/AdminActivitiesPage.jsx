import React, { useEffect, useState } from "react";
import ActivityModal from "../components/ActivityModal";
import {
  fetchActivities,
  fetchZones,
  fetchLocations,
  fetchStaff,
  createActivity,
  updateActivity,
  deleteActivity,
  fetchCategories,
  fetchActivityOccurrences,
} from "../services/api";
import { getLocationAddressByZoneId } from "../utils/location";
import "../styles/ActivitiesPage.css";

export default function AdminActivitiesPage() {
  const [activities, setActivities] = useState([]);
  const [zones, setZones] = useState([]);
  const [locations, setLocations] = useState([]);
  const [staffList, setStaffList] = useState([]);
  const [categories, setCategories] = useState([]);
  const [occurrences, setOccurrences] = useState([]);
  const [categoriesLoading, setCategoriesLoading] = useState(false);

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState("");
  const [debouncedSearchTerm, setDebouncedSearchTerm] = useState("");

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingActivity, setEditingActivity] = useState(null);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [deleteTarget, setDeleteTarget] = useState(null);

  // Load all data
  useEffect(() => {
    loadData();
  }, []);

  // Debounce search
  useEffect(() => {
    const t = setTimeout(() => setDebouncedSearchTerm(searchTerm), 300);
    return () => clearTimeout(t);
  }, [searchTerm]);

  useEffect(() => {
    let mounted = true;
    const loadCategories = async () => {
      setCategoriesLoading(true);
      try {
        const data = await fetchCategories();
        if (mounted) setCategories(Array.isArray(data) ? data : []);
      } catch (err) {
        console.error("Kunde inte hämta kategorier:", err);
      } finally {
        if (mounted) setCategoriesLoading(false);
      }
    };
    loadCategories();
    return () => {
      mounted = false;
    };
  }, []);

  async function loadData() {
    try {
      setLoading(true);
      const [activitiesData, zonesData, locationsData, staffData, occData] =
        await Promise.all([
          fetchActivities(),
          fetchZones(),
          fetchLocations(),
          fetchStaff(),
          fetchActivityOccurrences(),
        ]);
      setActivities(activitiesData);
      setZones(zonesData);
      setLocations(locationsData);
      setStaffList(staffData);
      setOccurrences(Array.isArray(occData) ? occData : []);
    } catch (err) {
      setError("Kunde inte hämta aktiviteter");
      console.error(err);
    } finally {
      setLoading(false);
    }
  }

  // (Filtrering borttagen — renderar alla aktiviteter direkt)

  const clearFilters = () => setSearchTerm("");

  const getLocationAddress = (zoneId) =>
    getLocationAddressByZoneId(zones, locations, zoneId);

  const handleOpenNew = () => {
    setEditingActivity(null);
    setIsModalOpen(true);
  };

  const handleSaveActivity = async (payloadFromModal) => {
    // payloadFromModal kommer från ActivityModal.onSave
    const body = {
      name: String(payloadFromModal.name || "Ny aktivitet"),
      description: String(payloadFromModal.description || ""),
      price: Number(payloadFromModal.price ?? 0),
      maxParticipants: Number(payloadFromModal.maxParticipants ?? 1),
      durationMinutes: Number(payloadFromModal.durationMinutes ?? 30),
      isPrivate: Boolean(payloadFromModal.isPrivate),
      isAvailable: Boolean(payloadFromModal.isAvailable),
      categoryId: Number(payloadFromModal.categoryId) || 1,
      zoneId: Number(payloadFromModal.zoneId) || zones[0]?.zoneId || 1,
      staffId: payloadFromModal.staffId || null,
      imageUrl: payloadFromModal.imageUrl || null,
      startTime: payloadFromModal.startTime || new Date().toISOString(),
    };

    try {
      if (editingActivity) {
        await updateActivity(editingActivity.id, body);
      } else {
        await createActivity(body);
      }

      // Lyckats — stäng modal och uppdatera listor
      setIsModalOpen(false);
      setEditingActivity(null);
      // refresha data så UI visar ny aktivitet
      await loadData();
    } catch (err) {
      console.error("Fel vid sparande av aktivitet:", err);
      alert("Misslyckades spara aktivitet. Se console för mer info.");
    }
  };

  const handleEdit = (activity) => {
    setEditingActivity(activity);
    setIsModalOpen(true);
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

  if (loading) return <p>Laddar aktiviteter...</p>;
  if (error) return <p>{error}</p>;

  return (
    <div className="activities-page">
      <header className="page-header">
        <h1>Hantera Aktiviteter</h1>
        <p>Här kan du lägga till, ändra eller ta bort aktiviteter</p>
        <button className="btn-primary" onClick={handleOpenNew}>
          ➕ Lägg till ny aktivitet
        </button>
      </header>

      {/* Filter borttaget — visar alla aktiviteter */}

      {/* Aktivitetskorten */}
      <div className="activities-grid">
        {activities.map((activity) => (
          <div key={activity.id} className="activity-card">
            {activity.imageUrl && (
              <div className="card-image" style={{ marginBottom: "0.75rem" }}>
                <img
                  src={activity.imageUrl}
                  alt={activity.name}
                  loading="lazy"
                  onError={(e) => (e.currentTarget.style.display = "none")}
                />
              </div>
            )}
            <div className="activity-header">
              <h3>{activity.name}</h3>
            </div>
            <div className="activity-content">
              <p className="activity-description">{activity.description}</p>
              <div className="activity-details">
                <div className="detail-item">
                  <div className="detail-label">Adress</div>
                  <div className="detail-value">
                    {getLocationAddress(activity.zoneId) || "Okänd"}
                  </div>
                </div>
                <div className="detail-item">
                  <div className="detail-label">Pris</div>
                  <div className="detail-value">{activity.price} kr</div>
                </div>
                <div className="detail-item">
                  <div className="detail-label">Max deltagare</div>
                  <div className="detail-value">{activity.maxParticipants}</div>
                </div>
                <div className="detail-item">
                  <div className="detail-label">Längd</div>
                  <div className="detail-value">
                    {activity.durationMinutes} min
                  </div>
                </div>
              </div>
            </div>
            <div className="activity-footer">
              <div className="activity-status">
                {activity.isPrivate && (
                  <span className="status-badge private">Privat</span>
                )}
              </div>
              <div className="card-actions">
                <button
                  className="view-details-btn edit-btn"
                  onClick={() => handleEdit(activity)}
                >
                  Ändra
                </button>
                <button
                  className="view-details-btn delete-btn"
                  onClick={() => handleDelete(activity)}
                >
                  Ta bort
                </button>
              </div>
            </div>
          </div>
        ))}
      </div>

      {/* Modal */}
      {isModalOpen && (
        <ActivityModal
          editing={editingActivity}
          initialData={editingActivity}
          onClose={() => setIsModalOpen(false)}
          onSave={handleSaveActivity}
          zones={zones}
          staffList={staffList}
          categories={categories}
          categoriesLoading={categoriesLoading}
          occurrences={occurrences}
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
