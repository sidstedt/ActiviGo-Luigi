import { useEffect, useState } from "react";
import ZoneModal from "../components/ZoneModal";
import {
  fetchZones,
  fetchLocations,
  createZone,
  updateZone,
  deleteZone,
} from "../services/api";
import "../styles/ActivitiesPage.css";
import "../styles/ActivityCard.css";

export default function AdminZonesPage() {
  const [zones, setZones] = useState([]);
  const [locations, setLocations] = useState([]);
  const [filteredZones, setFilteredZones] = useState([]);
  const [selectedLocation, setSelectedLocation] = useState("");
  const [selectedPlaceType, setSelectedPlaceType] = useState("");
  const [searchTerm, setSearchTerm] = useState("");
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingZone, setEditingZone] = useState(null);
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
      const [zonesData, locationsData] = await Promise.all([
        fetchZones(),
        fetchLocations(),
      ]);
      setZones(Array.isArray(zonesData) ? zonesData : []);
      setLocations(Array.isArray(locationsData) ? locationsData : []);
      setFilteredZones(Array.isArray(zonesData) ? zonesData : []);
    } catch (err) {
      setError("Fel vid hämtning av zoner");
    } finally {
      setLoading(false);
    }
  }

  const getZoneName = (z) =>
    z?.name ?? z?.zoneName ?? z?.ZoneName ?? z?.Name ?? z?.zone?.name ?? "";

  const getZoneId = (z) => z?.id ?? z?.zoneId ?? z?.ZoneId ?? z?.Id ?? null;

  const getZoneIsOutdoor = (z) =>
    (typeof z?.isOutdoor === "boolean" && z.isOutdoor) ||
    (typeof z?.isOutdoor === "boolean" && !z.isOutdoor)
      ? Boolean(z.isOutdoor)
      : typeof z?.IsOutdoor === "boolean"
      ? Boolean(z.IsOutdoor)
      : typeof z?.isOutdoor === "string"
      ? z.isOutdoor === "true"
      : typeof z?.isOutdoor !== "undefined"
      ? Boolean(z.isOutdoor)
      : typeof z?.isIndoor !== "undefined"
      ? !Boolean(z.isIndoor)
      : Boolean(z?.IsIndoor) === true
      ? false
      : Boolean(z?.IsOutdoor);

  const getZoneLocationId = (z) =>
    z?.locationId ??
    z?.LocationId ??
    z?.location?.id ??
    z?.location?.Id ??
    null;

  const findLocationNameFromLocations = (zone) => {
    if (!zone || !locations?.length) return null;

    const zoneLocId =
      zone?.locationId ??
      zone?.LocationId ??
      zone?.location?.id ??
      zone?.location?.Id ??
      null;

    if (!zoneLocId) return null;

    const found = locations.find((l) => {
      const locId =
        l?.id ?? l?.Id ?? l?.locationId ?? l?.LocationId ?? l?.location?.id;
      return String(locId) === String(zoneLocId);
    });

    if (found) {
      return found?.name ?? found?.Name ?? found?.locationName ?? "";
    }
    return null;
  };

  const getZoneLocationName = (z) => {
    return (
      z?.locationName ??
      z?.LocationName ??
      z?.location?.name ??
      z?.location?.Name ??
      findLocationNameFromLocations(z) ??
      ""
    );
  };

  useEffect(() => {
    let filtered = [...zones];

    if (searchTerm) {
      const s = searchTerm.toLowerCase();
      filtered = filtered.filter((z) =>
        getZoneName(z).toLowerCase().includes(s)
      );
    }

    if (selectedLocation) {
      filtered = filtered.filter(
        (z) => String(getZoneLocationName(z)) === String(selectedLocation)
      );
    }

    if (selectedPlaceType) {
      if (selectedPlaceType === "outdoor")
        filtered = filtered.filter((z) => getZoneIsOutdoor(z) === true);
      if (selectedPlaceType === "indoor")
        filtered = filtered.filter((z) => getZoneIsOutdoor(z) === false);
    }

    setFilteredZones(filtered);
  }, [zones, searchTerm, selectedLocation, selectedPlaceType, locations]);

  const handleOpenNew = () => {
    setEditingZone(null);
    setIsModalOpen(true);
  };

  const handleSaveZone = async (payload) => {
    const body = {
      name: String(payload.name || "Ny zon"),
      isOutdoor: Boolean(payload.isOutdoor),
      locationId: Number(payload.locationId),
    };
    try {
      if (editingZone) {
        await updateZone(getZoneId(editingZone), body);
      } else {
        await createZone(body);
      }
      setIsModalOpen(false);
      setEditingZone(null);
      await loadData();
    } catch (err) {
      alert("Misslyckades spara zon.");
    }
  };

  const handleEdit = (zone) => {
    let zoneToEdit = { ...zone };
    if (
      !zoneToEdit.locationId &&
      zoneToEdit.locationName &&
      locations.length > 0
    ) {
      const found = locations.find((l) => l.name === zoneToEdit.locationName);
      if (found) {
        zoneToEdit.locationId = found.id;
      }
    }
    setEditingZone(zoneToEdit);
    setIsModalOpen(true);
  };

  const handleDelete = (zone) => {
    setDeleteTarget(zone);
    setShowDeleteModal(true);
  };

  const confirmDelete = async () => {
    if (!deleteTarget) return;
    try {
      await deleteZone(getZoneId(deleteTarget));
      setZones((prev) =>
        prev.filter((z) => getZoneId(z) !== getZoneId(deleteTarget))
      );
      setShowDeleteModal(false);
      setDeleteTarget(null);
    } catch (err) {
      alert("Fel vid borttagning: " + (err.message || err));
    }
  };

  if (loading) return <p>Laddar zoner...</p>;
  if (error) return <p>{error}</p>;

  return (
    <div className="activities-page">
      <header className="page-header">
        <h1>Hantera Zoner</h1>
        <p>Här kan du lägga till, ändra eller ta bort zoner</p>
        <button className="btn-primary" onClick={handleOpenNew}>
          ➕ Lägg till ny zon
        </button>
      </header>

      <div className="filters-section">
        <div className="filters-grid">
          <div className="filter-group">
            <label htmlFor="search">Sök zon</label>
            <input
              id="search"
              type="text"
              placeholder="Sök efter zon-namn..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="filter-input"
            />
          </div>

          <div className="filter-group">
            <label htmlFor="location">Plats</label>
            <select
              id="location"
              value={selectedLocation}
              onChange={(e) => setSelectedLocation(e.target.value)}
              className="filter-select"
            >
              <option value="">Alla platser</option>
              {locations.map((loc, idx) => {
                const name = loc?.name ?? loc?.Name ?? `Plats ${idx + 1}`;
                return (
                  <option key={name} value={name}>
                    {name}
                  </option>
                );
              })}
            </select>
          </div>

          <div className="filter-group">
            <label htmlFor="placeType">Typ</label>
            <select
              id="placeType"
              value={selectedPlaceType}
              onChange={(e) => setSelectedPlaceType(e.target.value)}
              className="filter-select"
            >
              <option value="">Alla</option>
              <option value="indoor">Inomhus</option>
              <option value="outdoor">Utomhus</option>
            </select>
          </div>

          <div className="filter-group">
            <button
              onClick={() => {
                setSearchTerm("");
                setSelectedLocation("");
                setSelectedPlaceType("");
              }}
              className="clear-filters-btn"
            >
              Rensa filter
            </button>
          </div>
        </div>
      </div>

      <div className="activities-grid">
        {filteredZones.map((zone) => {
          const name = getZoneName(zone) || "Namnlös zon";
          const isOut = getZoneIsOutdoor(zone);
          const locationName = getZoneLocationName(zone) || "Okänd plats";


          return (
            <div key={getZoneId(zone) ?? name} className="activity-card">
              <div className="activity-header">
                <h3>{name}</h3>
              </div>
              <div className="activity-content">
                <div className="activity-details">
                  <div className="detail-item">
                    <div className="detail-label">Typ</div>
                    <div className="detail-value">
                      {isOut ? "Utomhus" : "Inomhus"}
                    </div>
                  </div>

                  <div className="detail-item">
                    <div className="detail-label">Plats</div>
                    <div className="detail-value">{locationName}</div>
                  </div>
                </div>
              </div>
              <div className="activity-footer">
                <div className="card-actions">
                  <button className="edit-btn" onClick={() => handleEdit(zone)}>
                    Ändra
                  </button>
                  <button
                    className="delete-btn"
                    onClick={() => handleDelete(zone)}
                  >
                    Ta bort
                  </button>
                </div>
              </div>
            </div>
          );
        })}
      </div>

      {isModalOpen && (
        <ZoneModal
          editing={editingZone}
          initialData={editingZone}
          onClose={() => setIsModalOpen(false)}
          onSave={handleSaveZone}
          locations={locations}
        />
      )}

      {showDeleteModal && deleteTarget && (
        <ConfirmDeleteModal
          name={getZoneName(deleteTarget)}
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
        <h3>Ta bort zon?</h3>
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
