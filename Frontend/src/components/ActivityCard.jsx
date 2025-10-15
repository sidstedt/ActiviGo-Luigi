import React from 'react'
import './ActivityCard.css'

const ActivityCard = ({ occurrence, price, onBook }) => {

  const formatDate = (dateString) => {
    const date = new Date(dateString);
    // dd/MM
    return date.toLocaleDateString('sv-SE', {
      day: '2-digit',
      month: '2-digit',
    });
  };

  const formatTime = (dateString) => {
    const date = new Date(dateString);
    return date.toLocaleTimeString('sv-SE', { 
      hour: '2-digit', 
      minute: '2-digit' 
    });
  };

  const hasAvailableSlots = occurrence.availableSlots > 0;
  const isFullyBooked = occurrence.availableSlots === 0;

  return (
    <div className="activity-card">
      {/* Header med titel och pris */}
      <div className="card-header">
        <h3 className="card-title">{occurrence.activityName}</h3>
        <span className="card-price">{price} kr</span>
      </div>
      
      {/* Innehåll med info */}
      <div className="card-content">
        {/* Zon */}
        <div className="card-info-row">
          <svg className="card-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
            <path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z"/>
            <circle cx="12" cy="10" r="3"/>
          </svg>
          <span className="card-info-text">{occurrence.zoneName}</span>
        </div>


        {/* Datum, tid och duration */}
        <div className="card-info-row">
          <svg className="card-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
            <rect x="3" y="4" width="18" height="18" rx="4"/>
            <line x1="16" y1="2" x2="16" y2="6"/>
            <line x1="8" y1="2" x2="8" y2="6"/>
          </svg>
          <span className="card-info-text">
            {formatDate(occurrence.startTime)}
          </span>
          <svg className="card-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
            <circle cx="12" cy="12" r="10"/>
            <polyline points="12 6 12 12 16 14"/>
          </svg>
          <span className="card-info-text">
            {formatTime(occurrence.startTime)} - {formatTime(occurrence.endTime)}
          </span>
          <span className="card-duration">{occurrence.durationMinutes} min</span>
        </div>

        {/* Platser kvar */}
        <div className="card-info-row">
          <svg className="card-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
            <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/>
            <circle cx="9" cy="7" r="4"/>
            <path d="M23 21v-2a4 4 0 0 0-3-3.87"/>
            <path d="M16 3.13a4 4 0 0 1 0 7.75"/>
          </svg>
          <span className={`card-info-text ${isFullyBooked ? 'text-red' : 'text-green'}`}>
            {occurrence.availableSlots} platser kvar
          </span>
        </div>
      </div>

      {/* Footer med boka knapp */}
      <div className="card-footer">
        <button 
          className={`book-button ${!hasAvailableSlots ? 'disabled' : ''}`}
          onClick={() => onBook(occurrence.id)}
          disabled={!hasAvailableSlots || occurrence.isCancelled}
        >
          {isFullyBooked ? 'Fullbokad' : occurrence.isCancelled ? 'Inställd' : 'Boka nu'}
        </button>
      </div>
    </div>
  )
}

export default ActivityCard