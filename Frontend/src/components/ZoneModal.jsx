import { useState, useEffect } from "react";
import "../styles/ActivityModal.css";

export default function ZoneModal({ editing, initialData, onClose, onSave, locations }) {
  const getInitialZoneName = (data) =>
    data?.zoneName ?? "";
  const [zoneName, setZoneName] = useState(getInitialZoneName(initialData));
  const [isOutdoor, setIsOutdoor] = useState(initialData?.isOutdoor ?? false);
  const [locationId, setLocationId] = useState(initialData?.locationId || "");

  useEffect(() => {
    if (!initialData && locations?.length > 0 && !locationId) {
      setLocationId(locations[0].id);
    }
  }, [locations, initialData]);

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!zoneName.trim()) return alert("Zonens namn krävs.");
    if (!locationId) return alert("Välj plats.");

    onSave({
      name: zoneName,
      isOutdoor,
      locationId: Number(locationId),
    });
  };

  return (
    <div className="modal-backdrop" role="dialog" aria-modal="true">
      <div className="modal">
        <div className="modal-header">
          <h2>{editing ? "Redigera zon" : "Ny zon"}</h2>
          <button className="close-btn" onClick={onClose}>
            ×
          </button>
        </div>

        <form onSubmit={handleSubmit} className="modal-body">
          <label>
            Zonens namn
            <input
              type="text"
              value={zoneName}
              onChange={(e) => setZoneName(e.target.value)}
              required
              placeholder="Ange zonens namn"
            />
          </label>

          <label>
            Plats
            <select
              value={locationId}
              onChange={(e) => setLocationId(e.target.value)}
              required
            >
              {locations.map((loc) => (
                <option key={loc.id} value={loc.id}>
                  {loc.name}
                </option>
              ))}
            </select>
          </label>

          <label style={{ display: "flex", gap: "8px", alignItems: "center" }}>
            <input
              type="checkbox"
              checked={isOutdoor}
              onChange={(e) => setIsOutdoor(e.target.checked)}
            />
            Utomhus
          </label>

          <div className="modal-footer">
            <button type="button" className="btn-secondary" onClick={onClose}>
              Avbryt
            </button>
            <button type="submit" className="btn-primary">
              Spara
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}