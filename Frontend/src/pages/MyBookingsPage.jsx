import { useEffect, useState } from "react";
import { fetchUserBookings, cancelBooking } from "../services/api";
import "../styles/MyBookingsPage.css";
import BookingCard from "../components/BookingCard";
import { formatDateLong, formatTimeHm } from "../utils/format"; 


export default function MyBookingsPage() {
  const [bookings, setBookings] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [filter, setFilter] = useState("all"); // all, upcoming, past, cancelled
  const [showDetails, setShowDetails] = useState(false);
  const [selectedBooking, setSelectedBooking] = useState(null);

  useEffect(() => {
    loadBookings();
  }, []);

  const loadBookings = async () => {
    try {
      setLoading(true);
      setError(null);
      
      const bookingsData = await fetchUserBookings();
      setBookings(bookingsData);
    } catch (err) {
      setError(err.message || "Kunde inte hämta dina bokningar");
    } finally {
      setLoading(false);
    }
  };

  const handleCancelBooking = async (bookingId) => {
    if (!window.confirm("Är du säker på att du vill avboka denna aktivitet?")) {
      return;
    }

    try {
      await cancelBooking(bookingId);
      await loadBookings(); // Reload bookings
    } catch (err) {
      alert("Kunde inte avboka aktiviteten: " + err.message);
    }
  };

  const handleShowDetails = (bk) => {
    setSelectedBooking(bk);
    setShowDetails(true);
  };
  const handleCloseDetails = () => {
    setShowDetails(false);
    setSelectedBooking(null);
  };

  const getFilteredBookings = () => {
    const now = new Date();
    
    switch (filter) {
      case "upcoming":
        return bookings.filter(booking => 
          new Date(booking.startTime) > now && 
          booking.status !== "Cancelled" && booking.status !== "Canceled"
        );
      case "past":
        return bookings.filter(booking => 
          new Date(booking.startTime) <= now || 
          booking.status === "Completed"
        );
      case "cancelled":
        return bookings.filter(booking => booking.status === "Cancelled" || booking.status === "Canceled");
      default:
        return bookings;
    }
  };

  // formatting and status handling are in BookingCard

  if (loading) {
    return (
      <div className="my-bookings-page">
        <div className="loading">
          <div className="spinner"></div>
          <p>Laddar dina bokningar...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="my-bookings-page">
        <div className="error-message">
          <h2>⚠️ Ett fel uppstod</h2>
          <p>{error}</p>
          <button onClick={loadBookings} className="retry-button">
            Försök igen
          </button>
        </div>
      </div>
    );
  }

  const filteredBookings = getFilteredBookings();
  // Precompute counts to align with the filter logic and avoid duplicate conditions
  const now = new Date();
  const upcomingCount = bookings.filter(b => new Date(b.startTime) > now && b.status !== "Cancelled" && b.status !== "Canceled").length;
  const pastCount = bookings.filter(b => new Date(b.startTime) <= now || b.status === "Completed").length;
  const cancelledCount = bookings.filter(b => b.status === "Cancelled" || b.status === "Canceled").length;

  return (
    <div className="my-bookings-page">
      <header className="page-header">
        <h1>Mina bokningar</h1>
        <p>Hantera dina aktivitetsbokningar</p>
      </header>

      {/* Filter Tabs */}
      <div className="filter-tabs">
        <button 
          className={`filter-tab ${filter === "all" ? "active" : ""}`}
          onClick={() => setFilter("all")}
        >
          Alla ({bookings.length})
        </button>
        <button 
          className={`filter-tab ${filter === "upcoming" ? "active" : ""}`}
          onClick={() => setFilter("upcoming")}
        >
          Kommande ({upcomingCount})
        </button>
        <button 
          className={`filter-tab ${filter === "past" ? "active" : ""}`}
          onClick={() => setFilter("past")}
        >
          Tidigare ({pastCount})
        </button>
        <button 
          className={`filter-tab ${filter === "cancelled" ? "active" : ""}`}
          onClick={() => setFilter("cancelled")}
        >
          Avbokade ({cancelledCount})
        </button>
      </div>

      {/* Bookings List */}
      <div className="bookings-section">
        {filteredBookings.length === 0 ? (
          <div className="no-bookings">
            <div className="no-bookings-icon">📅</div>
            <h3>Inga bokningar hittades</h3>
            <p>
              {filter === "all" 
                ? "Du har inga bokningar än. Gå till Sök & Boka för att hitta aktiviteter!"
                : filter === "upcoming"
                ? "Du har inga kommande bokningar."
                : filter === "past"
                ? "Du har inga tidigare bokningar."
                : "Du har inga avbokade aktiviteter."
              }
            </p>
            {filter === "all" && (
              <a href="/bookings" className="browse-activities-btn">
                Bläddra aktiviteter
              </a>
            )}
          </div>
        ) : (
          <div className="bookings-grid">
            {filteredBookings.map(booking => (
              <BookingCard
                key={booking.id}
                booking={booking}
                onCancel={() => handleCancelBooking(booking.id)}
                onDetails={(bk) => handleShowDetails(bk)}
              />
            ))}
          </div>
        )}
      </div>

    {showDetails && selectedBooking && (
        <BookingDetailsModal
          booking={selectedBooking}
          onClose={handleCloseDetails}
        />
      )}
    </div>
  );
}
function BookingDetailsModal({ booking, onClose }) {
  // Stäng på ESC
  useEffect(() => {
    const onKey = (e) => e.key === "Escape" && onClose();
    window.addEventListener("keydown", onKey);
    return () => window.removeEventListener("keydown", onKey);
  }, [onClose]);

  const stop = (e) => e.stopPropagation();

  return (
    <div className="modal-backdrop" onClick={onClose}>
      <div className="modal" onClick={stop} role="dialog" aria-modal="true" aria-labelledby="detailsTitle">
        <div className="modal-header">
          <h2 id="detailsTitle">{booking.activityName}</h2>
          <button className="modal-close" onClick={onClose} aria-label="Stäng">×</button>
        </div>

        <div className="modal-body">
          <div className="detail-row"><strong>Datum:</strong> {formatDateLong(booking.startTime)}</div>
          <div className="detail-row"><strong>Tid:</strong> {formatTimeHm(booking.startTime)} – {formatTimeHm(booking.endTime)}</div>
          <div className="detail-row"><strong>Plats:</strong> {booking.zoneName}</div>
          <div className="detail-row"><strong>Status:</strong> {booking.status}</div>
          <div className="detail-row"><strong>Pris:</strong> {booking.price} kr</div>
          <div className="detail-row"><strong>Bokad:</strong> {formatDateLong(booking.createdAt)}</div>
        </div>

        <div className="modal-footer">
          <button className="secondary-button" onClick={onClose}>Stäng</button>
        </div>
      </div>
    </div>
  );
}
