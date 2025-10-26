import React, { useEffect, useState } from "react";

function getInitialZoneName(initial) {
  if (!initial) return "";
  return (
    initial.zoneName ??
    initial.name ??
    initial.Name ??
    initial.zone_name ??
    initial.zone ??
    ""
  );
}

function getInitialLocationId(initial) {
  if (!initial) return "";
  return (
    initial.locationId ??
    initial.LocationId ??
    initial.Location?.id ??
    initial.LocationId ??
    ""
  );
}

export default function ZoneModal({
  initial = null,
  locations = [],
  onClose,
  onSave,
}) {
  const editing = Boolean(
    initial && (initial.id ?? initial.Id ?? initial.zoneId ?? initial.ZoneId)
  );

  const [name, setName] = useState(getInitialZoneName(initial));
  const [locationId, setLocationId] = useState(getInitialLocationId(initial));
  const [isOutdoor, setIsOutdoor] = useState(
    Boolean(initial?.isOutdoor ?? initial?.IsOutdoor ?? false)
  );
  const [isActive, setIsActive] = useState(
    Boolean(initial?.isActive ?? initial?.IsActive ?? true)
  );

  useEffect(() => {
    setName(getInitialZoneName(initial));
    setLocationId(getInitialLocationId(initial));
    setIsOutdoor(Boolean(initial?.isOutdoor ?? initial?.IsOutdoor ?? false));
    setIsActive(Boolean(initial?.isActive ?? initial?.IsActive ?? true));
  }, [initial]);

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!name.trim()) return alert("Zonens namn krävs.");
    if (!locationId) return alert("Välj plats.");

    onSave({
      id: initial?.id ?? initial?.Id ?? null,
      name: name,
      isOutdoor,
      locationId: Number(locationId),
      isActive,
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
              value={name}
              onChange={(e) => setName(e.target.value)}
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
              <option value="">Välj plats</option>
              {Array.isArray(locations) &&
                locations.map((loc) => (
                  <option key={loc.id ?? loc.Id} value={loc.id ?? loc.Id}>
                    {loc.name ?? loc.Name ?? loc.locationName ?? ""}
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

          <label style={{ display: "flex", gap: "8px", alignItems: "center" }}>
            <input
              type="checkbox"
              checked={isActive}
              onChange={(e) => setIsActive(e.target.checked)}
            />
            Aktiv
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
