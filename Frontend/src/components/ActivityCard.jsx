import "../styles/ActivityCard.css";
import WeatherBadge from "./WeatherBadge";
import { useState } from "react";
import { formatDateShort, formatTimeHm } from "../utils/format";
import Modal from "./Modal";

const ActivityCard = ({
  occurrence,
  price,
  onBook,
  forecast,
  locationName,
  address,
  description,
  imageUrl: propImageUrl,
}) => {
  const [showInfo, setShowInfo] = useState(false);

  const formatDate = formatDateShort;
  const formatTime = formatTimeHm;

  const hasAvailableSlots = occurrence.availableSlots > 0;
  const isFullyBooked = occurrence.availableSlots === 0;

  //fetch image url
  const imageUrl =
    (typeof propImageUrl === "string" && propImageUrl) ||
    occurrence?.imageUrl ||
    occurrence?.activityImageUrl ||
    occurrence?.activity?.imageUrl ||
    null;

  return (
    <div className="activity-card">
      {/* image url */}
      {imageUrl && (
        <div className="card-image">
          <img
            src={imageUrl}
            alt={occurrence.activityName}
            loading="lazy"
            onError={(e) => {
              e.currentTarget.style.display = "none";
            }}
          />
        </div>
      )}
      {/* Header med titel och pris */}
      <div className="card-header">
        <h3 className="card-title">
          {occurrence.activityName}
          {imageUrl && (
            <svg
              className="has-image-icon"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              strokeWidth="2"
              aria-label="Har bild"
              title="Har bild"
            >
              <path d="M23 19a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2V7a2 2 0 0 1 2-2h3l2-3h6l2 3h3a2 2 0 0 1 2 2z" />
              <circle cx="12" cy="13" r="3.5" />
            </svg>
          )}
        </h3>
        <span className="card-price">{price} kr</span>
      </div>

      {/* Innehåll med info */}
      <div className="card-content">
        {/* Adress */}
        {address && (
          <div className="card-info-row">
            <svg
              className="card-icon"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              strokeWidth="2"
            >
              <path d="M12 2C8.13 2 5 5.13 5 9c0 5.25 7 13 7 13s7-7.75 7-13c0-3.87-3.13-7-7-7z" />
              <circle cx="12" cy="9" r="2.5" />
            </svg>
            <span className="card-info-text">
              {locationName ? `${locationName} – ` : ""}
              {address}
            </span>
          </div>
        )}

        {/* Datum, tid och duration */}
        <div className="card-info-row">
          <svg
            className="card-icon"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            strokeWidth="2"
          >
            <rect x="3" y="4" width="18" height="18" rx="4" />
            <line x1="16" y1="2" x2="16" y2="6" />
            <line x1="8" y1="2" x2="8" y2="6" />
          </svg>
          <span className="card-info-text">
            {formatDate(occurrence.startTime)}
          </span>
          <svg
            className="card-icon"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            strokeWidth="2"
          >
            <circle cx="12" cy="12" r="10" />
            <polyline points="12 6 12 12 16 14" />
          </svg>
          <span className="card-info-text">
            {formatTime(occurrence.startTime)} -{" "}
            {formatTime(occurrence.endTime)}
          </span>
          <span className="card-duration">
            {occurrence.durationMinutes} min
          </span>
        </div>

        {/* Platser kvar */}
        <div className="card-info-row">
          <svg
            className="card-icon"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            strokeWidth="2"
          >
            <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2" />
            <circle cx="9" cy="7" r="4" />
            <path d="M23 21v-2a4 4 0 0 0-3-3.87" />
            <path d="M16 3.13a4 4 0 0 1 0 7.75" />
          </svg>
          <span
            className={`card-info-text ${
              isFullyBooked ? "text-red" : "text-green"
            }`}
          >
            {occurrence.availableSlots} platser kvar
          </span>
        </div>
      </div>

      {/* Footer med väder + boka knapp */}
      <div className="card-footer">
        {/* Väder (endast om utomhus och forecast finns) */}
        <div className="card-footer-left">
          {occurrence.isOutdoor && forecast && (
            <WeatherBadge forecast={forecast} />
          )}
        </div>
        <div className="card-actions">
          <button
            className="secondary-button"
            onClick={() => setShowInfo(true)}
          >
            Mer info
          </button>
          <button
            className={`book-button ${!hasAvailableSlots ? "disabled" : ""}`}
            onClick={() => onBook(occurrence.id)}
            disabled={!hasAvailableSlots || occurrence.isCancelled}
          >
            {isFullyBooked
              ? "Fullbokad"
              : occurrence.isCancelled
              ? "Inställd"
              : "Boka nu"}
          </button>
        </div>
      </div>

      {showInfo && (
        <Modal
          title={occurrence.activityName}
          onClose={() => setShowInfo(false)}
        >
          <div className="info-modal-content">
            {imageUrl && (
              <img
                className="info-image"
                src={imageUrl}
                alt={`${occurrence.activityName} bild`}
                loading="lazy"
                onError={(e) => {
                  e.currentTarget.style.display = "none";
                }}
              />
            )}
            <div className="info-row">
              <strong>Zon:</strong> {occurrence.zoneName}
            </div>
            {description && (
              <div className="info-row">
                <strong>Beskrivning:</strong> {description}
              </div>
            )}
            {address && (
              <div className="info-row">
                <strong>Adress:</strong> {address}
              </div>
            )}
            {!address && locationName && (
              <div className="info-row">
                <strong>Plats:</strong> {locationName}
              </div>
            )}
            <div className="info-row">
              <strong>Datum:</strong> {formatDate(occurrence.startTime)}
            </div>
            <div className="info-row">
              <strong>Tid:</strong> {formatTime(occurrence.startTime)} -{" "}
              {formatTime(occurrence.endTime)}
            </div>
            <div className="info-row">
              <strong>Längd:</strong> {occurrence.durationMinutes} min
            </div>
          </div>
        </Modal>
      )}
    </div>
  );
};

export default ActivityCard;
