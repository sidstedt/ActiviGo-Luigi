import React, { useState, useEffect } from "react";
import "../styles/ActivityModal.css";

export default function ActivityModal({
  editing,
  initialData,
  onClose,
  onSave,
  zones,
  staffList,
  categories,
  occurrences,
}) {
  const [activityName, setActivityName] = useState(initialData?.name || "");
  const [description, setDescription] = useState(
    initialData?.description || ""
  );
  const [price, setPrice] = useState(initialData?.price ?? 0);
  const [maxParticipants, setMaxParticipants] = useState(
    initialData?.maxParticipants ?? 0
  );
  const [dateStr, setDateStr] = useState(
    initialData?.startTime?.slice(0, 10) || ""
  );
  const [timeStr, setTimeStr] = useState(
    initialData?.startTime?.slice(11, 16) || ""
  );
  const [durationMinutes, setDurationMinutes] = useState(
    initialData?.durationMinutes || 30
  );
  const [isActive, setIsActive] = useState(initialData?.isActive ?? true);
  const [isPrivate, setIsPrivate] = useState(initialData?.isPrivate ?? false);
  const [isAvailable, setIsAvailable] = useState(
    initialData?.isAvailable ?? true
  );
  const [imageUrl, setImageUrl] = useState(initialData?.imageUrl ?? "");
  const [zoneId, setZoneId] = useState(
    initialData?.zoneId || zones?.[0]?.zoneId || 0
  );
  const [categoryId, setCategoryId] = useState(
    initialData?.categoryId || categories?.[0]?.categoryId || 0
  );
  const [staffId, setStaffId] = useState(initialData?.staffId || "");
  const [availableTimes, setAvailableTimes] = useState([]);

  useEffect(() => {
    if (!initialData) {
      if (zones?.length > 0 && !zoneId) {
        setZoneId(zones[0].zoneId);
      }
      if (categories?.length > 0 && !categoryId) {
        setCategoryId(categories[0].categoryId);
      }
    }
  }, []);

  // räkna fram lediga tider och auto-välj första om ingen giltig tid är satt
  useEffect(() => {
    const times = getAvailableTimes({
      dateStr,
      zoneId: Number(zoneId),
      durationMinutes: Number(durationMinutes),
      occurrences,
      editingId: editing?.id,
    });
    setAvailableTimes(times);
    if (times.length > 0 && (!timeStr || !times.includes(timeStr))) {
      setTimeStr(times[0]);
    }
  }, [dateStr, zoneId, durationMinutes, occurrences, editing?.id]);

  const handleSubmit = (e) => {
    e.preventDefault();
    onSave({
      name: activityName,
      description: description || "",
      price: Number(price) || 0,
      maxParticipants: Number(maxParticipants) || 0,
      durationMinutes: Number(durationMinutes) || 0,
      isPrivate: Boolean(isPrivate),
      isAvailable: Boolean(isAvailable),
      categoryId: Number(categoryId),
      categoryName: categories[Number(categoryId)]?.name || "",
      zoneId: Number(zoneId) || 0,
      staffId: staffId || null,
      imageUrl: imageUrl?.trim() || null,
      startTime: dateStr && timeStr ? `${dateStr}T${timeStr}:00` : undefined,
    });
  };

  const computedZoneId = zoneId;

  const getAvailableTimes = ({
    dateStr,
    zoneId,
    durationMinutes,
    occurrences = [],
    editingId,
  }) => {
    if (!dateStr || !zoneId || !durationMinutes) return [];

    const dayStartHour = 6;
    const dayEndHour = 22;
    const pad = (n) => String(n).padStart(2, "0");

    // generera kandidat-tider (15 min intervall)
    const candidateTimes = [];
    for (let h = dayStartHour; h < dayEndHour; h++) {
      for (let m = 0; m < 60; m += 15) {
        candidateTimes.push(`${pad(h)}:${pad(m)}`);
      }
    }

    // samla konflikter för samma zon (och inte det som redigeras)
    const conflicts = (occurrences || [])
      .filter((o) => Number(o.zoneId) === Number(zoneId) && o.id !== editingId)
      .map((o) => {
        const startIso = o.startTime || o.StartTime || o.start || o.Start;
        const start = startIso ? new Date(startIso) : null;
        let end = null;
        if (o.endTime || o.EndTime) {
          end = new Date(o.endTime || o.EndTime);
        } else if (o.durationMinutes) {
          end = start
            ? new Date(start.getTime() + Number(o.durationMinutes) * 60000)
            : null;
        } else if (start) {
          end = new Date(start.getTime() + 30 * 60000); // fallback 30 min
        }
        return start && end ? { start, end } : null;
      })
      .filter(Boolean);

    const dateStartLimit = new Date(`${dateStr}T00:00:00`);
    const dayEndLimit = new Date(`${dateStr}T${pad(dayEndHour)}:00:00`);

    const isConflict = (sMs, eMs) =>
      conflicts.some(
        (c) => !(eMs <= c.start.getTime() || sMs >= c.end.getTime())
      );

    const available = candidateTimes.filter((t) => {
      const start = new Date(`${dateStr}T${t}:00`);
      const end = new Date(start.getTime() + Number(durationMinutes) * 60000);
      // reject if end is after allowed dayEndLimit
      if (end.getTime() > dayEndLimit.getTime()) return false;
      // reject if start before day start (safety)
      if (start.getTime() < dateStartLimit.getTime()) return false;
      return !isConflict(start.getTime(), end.getTime());
    });

    return available;
  };

  return (
    <div className="modal-backdrop" role="dialog" aria-modal="true">
      <div className="modal">
        {/* Modal Header */}
        <div className="modal-header">
          <h2>{editing ? "Redigera aktivitet" : "Ny aktivitet"}</h2>
          <button className="close-btn" onClick={onClose} aria-label="Stäng">
            ×
          </button>
        </div>

        {/* Modal Body */}
        <form onSubmit={handleSubmit} className="modal-body">
          <label>
            Aktivitet
            <input
              type="text"
              value={activityName}
              onChange={(e) => setActivityName(e.target.value)}
              required
              placeholder="Skriv aktivitetens namn"
            />
          </label>

          <label>
            Kategori
            <select
              value={String(categoryId)}
              onChange={(e) => setCategoryId(e.target.value)}
              required
            >
              <option value="">Välj kategori</option>
              {categories?.map((c, index) => (
                <option key={index} value={String(index)}>
                  {c.name}
                </option>
              ))}
            </select>
          </label>

          <label>
            Beskrivning
            <textarea
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              placeholder="Valfri beskrivning"
            />
          </label>

          <div className="form-row">
            <label htmlFor="imageUrl">Bild-URL</label>
            <input
              id="imageUrl"
              type="url"
              placeholder="https://example.com/image.jpg"
              value={imageUrl}
              onChange={(e) => setImageUrl(e.target.value)}
            />
          </div>

          {/* Optional quick preview */}
          {!!imageUrl && (
            <div className="form-row">
              <img
                src={imageUrl}
                alt="Förhandsvisning"
                style={{
                  maxWidth: "100%",
                  maxHeight: 160,
                  objectFit: "cover",
                  borderRadius: 8,
                }}
                onError={(e) => (e.currentTarget.style.display = "none")}
              />
            </div>
          )}

          <div style={{ display: "flex", gap: "1rem" }}>
            <label>
              Pris
              <input
                type="number"
                min="0"
                step="0.01"
                value={price}
                onChange={(e) => setPrice(e.target.value)}
              />
            </label>
            <label>
              Max deltagare
              <input
                type="number"
                min="0"
                value={maxParticipants}
                onChange={(e) => setMaxParticipants(e.target.value)}
              />
            </label>
          </div>

          <label style={{ display: "flex", gap: "8px", alignItems: "center" }}>
            <input
              type="checkbox"
              checked={isPrivate}
              onChange={(e) => setIsPrivate(e.target.checked)}
            />
            Privat aktivitet
          </label>

          <label style={{ display: "flex", gap: "8px", alignItems: "center" }}>
            <input
              type="checkbox"
              checked={isAvailable}
              onChange={(e) => setIsAvailable(e.target.checked)}
            />
            Tillgänglig
          </label>

          <label>
            Plats / Zon
            <select
              value={zoneId}
              onChange={(e) => setZoneId(Number(e.target.value))}
              required
            >
              {zones.map((z) => (
                <option key={z.zoneId} value={z.zoneId}>
                  {z.zoneName}
                </option>
              ))}
            </select>
          </label>

          <label>
            Personal (valfritt)
            <select
              value={staffId || ""}
              onChange={(e) => setStaffId(e.target.value)}
            >
              <option value="">-- Ingen tilldelad --</option>
              {staffList.map((s) => (
                <option key={s.id} value={s.id}>
                  {s.firstName} {s.lastName}
                </option>
              ))}
            </select>
          </label>

          {/* Modal Footer */}
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
