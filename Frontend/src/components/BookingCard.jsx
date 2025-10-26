import { formatDateLong, formatTimeHm } from "../utils/format";

function statusColor(status) {
  switch (status) {
    case "Reserved":
      return "status-reserved";
    case "Confirmed":
      return "status-confirmed";
    case "Cancelled":
    case "Canceled":
      return "status-cancelled";
    case "Completed":
      return "status-completed";
    default:
      return "status-pending";
  }
}

function statusText(status) {
  switch (status) {
    case "Reserved":
      return "Reserverad";
    case "Confirmed":
      return "Bekräftad";
    case "Cancelled":
    case "Canceled":
      return "Avbokad";
    case "Completed":
      return "Genomförd";
    default:
      return "Väntar";
  }
}

function canCancelBooking(booking) {
  const now = new Date();
  const start = new Date(booking.startTime);
  const twoHoursBefore = new Date(start.getTime() - 2 * 60 * 60 * 1000);
  return booking.status === "Reserved" && now < twoHoursBefore;
}

export default function BookingCard({ booking, onCancel, onDetails }) {
  const canCancel = canCancelBooking(booking);
  return (
    <div className="booking-card">
      <div className="booking-header">
        <h3 className="activity-name">{booking.activityName}</h3>
        <span className={`status-badge ${statusColor(booking.status)}`}>
          {statusText(booking.status)}
        </span>
      </div>

      <div className="booking-content">
        <div className="booking-details">
          <div className="detail-row">
            <span className="detail-label">📅 Datum:</span>
            <span className="detail-value">{formatDateLong(booking.startTime)}</span>
          </div>

          <div className="detail-row">
            <span className="detail-label">🕐 Tid:</span>
            <span className="detail-value">
              {formatTimeHm(booking.startTime)} - {formatTimeHm(booking.endTime)}
            </span>
          </div>

          <div className="detail-row">
            <span className="detail-label">📍 Plats:</span>
            <span className="detail-value">{booking.zoneName}</span>
          </div>

          <div className="detail-row">
            <span className="detail-label">💰 Pris:</span>
            <span className="detail-value">{booking.price} kr</span>
          </div>
        </div>
      </div>

      <div className="booking-footer">
        <div className="booking-actions">
          {canCancel && (
            <button className="cancel-btn" onClick={onCancel}>
              Avboka
            </button>
          )}
          <button
            className="details-btn"
            onClick={() => onDetails?.(booking)} 
          >
            Se detaljer
          </button>
        </div>
        <div className="booking-meta">
          <small>Bokad: {formatDateLong(booking.createdAt)}</small>
        </div>
      </div>
    </div>
  );
}
