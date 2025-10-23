import React, { useState } from "react";
import "../styles/ActivityModal.css";

export default function LocationModal({ editing, initialData, onClose, onSave }) {
  const [name, setName] = useState(initialData?.name || "");
  // Dela upp adressen i tre fält
  const [street, setStreet] = useState(initialData?.street || "");
  const [number, setNumber] = useState(initialData?.number || "");
  const [city, setCity] = useState(initialData?.city || "");

  // Om initialData har address (sträng), försök splitta till fält
  React.useEffect(() => {
    if (initialData?.address && (!street && !number && !city)) {
      // Försök splitta "Gatan 1, Stad"
      const [streetAndNumber, cityPart] = initialData.address.split(",");
      if (streetAndNumber) {
        const match = streetAndNumber.trim().match(/^(.*?)(\d+.*)$/);
        if (match) {
          setStreet(match[1].trim());
          setNumber(match[2].trim());
        } else {
          setStreet(streetAndNumber.trim());
        }
      }
      if (cityPart) setCity(cityPart.trim());
    }
  }, [initialData]);

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!name.trim()) return alert("Platsens namn krävs.");
    if (!street.trim()) return alert("Gatunamn krävs.");
    if (!number.trim()) return alert("Gatunummer krävs.");
    if (!city.trim()) return alert("Postort krävs.");
    // Slå ihop till address-sträng för backend
    const address = `${street} ${number}, ${city}`;
    onSave({
      name,
      address,
      street,
      number,
      city,
    });
  };

  return (
    <div className="modal-backdrop" role="dialog" aria-modal="true">
      <div className="modal">
        <div className="modal-header">
          <h2>{editing ? "Redigera plats" : "Ny plats"}</h2>
          <button className="close-btn" onClick={onClose}>
            ×
          </button>
        </div>
        <form onSubmit={handleSubmit} className="modal-body">
          <label>
            Namn
            <input
              type="text"
              value={name}
              onChange={(e) => setName(e.target.value)}
              required
              placeholder="Ange platsens namn"
            />
          </label>
          <label>
            Gatunamn
            <input
              type="text"
              value={street}
              onChange={(e) => setStreet(e.target.value)}
              required
              placeholder="Gatunamn"
            />
          </label>
          <label>
            Gatunummer
            <input
              type="text"
              value={number}
              onChange={(e) => setNumber(e.target.value)}
              required
              placeholder="Nummer"
            />
          </label>
          <label>
            Postort
            <input
              type="text"
              value={city}
              onChange={(e) => setCity(e.target.value)}
              required
              placeholder="Postort"
            />
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
