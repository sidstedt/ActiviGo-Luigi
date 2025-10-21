import React, { useState, useEffect } from "react";
import "../styles/ActivityModal.css"; // separat CSS för modalens styling

export default function ActivityModal({
  editing = null,
  initialData = {},
  onClose,
  onSave,
}) {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [maxParticipants, setMaxParticipants] = useState(10);
  const [durationMinutes, setDurationMinutes] = useState(60);
  const [isActive, setIsActive] = useState(true);

  useEffect(() => {
    if (editing && initialData) {
      setName(initialData.name || "");
      setDescription(initialData.description || "");
      setMaxParticipants(initialData.maxParticipants || 10);
      setDurationMinutes(initialData.durationMinutes || 60);
      setIsActive(initialData.isActive ?? true);
    }
  }, [editing, initialData]);

  const handleSubmit = (e) => {
    e.preventDefault();
    const activityData = {
      name,
      description,
      maxParticipants,
      durationMinutes,
      isActive,
    };
    onSave(activityData);
  };

  return (
    <div className="modal-backdrop" role="dialog" aria-modal="true">
      <div className="modal">
        <div className="modal-header">
          <h2>{editing ? "Ändra aktivitet" : "Ny aktivitet"}</h2>
          <button className="close-btn" onClick={onClose} aria-label="Stäng">
            ×
          </button>
        </div>

        <form onSubmit={handleSubmit} className="modal-body">
          <label>
            Aktivitetens namn
            <input
              type="text"
              value={name}
              onChange={(e) => setName(e.target.value)}
              required
            />
          </label>

          <label>
            Beskrivning
            <textarea
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              rows={3}
            />
          </label>

          <label>
            Max deltagare
            <input
              type="number"
              value={maxParticipants}
              min={1}
              onChange={(e) => setMaxParticipants(Number(e.target.value))}
              required
            />
          </label>

          <div className="duration-choices">
            <div className="label">Aktivitetslängd</div>
            <div className="choices">
              {[30, 45, 60, 75, 90, 105, 120].map((min) => (
                <span
                  key={min}
                  className={`choice ${
                    durationMinutes === min ? "selected" : ""
                  }`}
                  onClick={() => setDurationMinutes(min)}
                  role="button"
                  tabIndex={0}
                >
                  {min} min
                </span>
              ))}
            </div>
          </div>

          <div className="modal-footer">
            <button type="button" className="btn-secondary" onClick={onClose}>
              Avbryt
            </button>
            <button
              type="submit"
              className="btn-primary"
              disabled={!name || !maxParticipants || !durationMinutes}
            >
              Spara
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
