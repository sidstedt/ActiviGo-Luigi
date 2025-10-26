import { useLocation, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { fetchBookingById } from "../services/api"; // antas finnas

export default function BookingDetailsPage() {
  const { id } = useParams();
  const location = useLocation();
  const stateBooking = location.state?.booking;
  const [booking, setBooking] = useState(stateBooking || null);
  const [loading, setLoading] = useState(!stateBooking);

  useEffect(() => {
    if (stateBooking) return; // redan laddad via state
    let ok = true;
    (async () => {
      try {
        setLoading(true);
        const data = await fetchBookingById(id);
        if (ok) setBooking(data);
      } finally {
        if (ok) setLoading(false);
      }
    })();
    return () => { ok = false; };
  }, [id, stateBooking]);

  if (loading) return <div className="loading"><div className="spinner" />Laddar...</div>;
  if (!booking) return <p>Kunde inte hitta bokning.</p>;

  return (
    <div className="booking-details-page">
      <header className="page-header">
        <h1>{booking.activityName}</h1>
        <p>Detaljer för bokning #{booking.id}</p>
      </header>

      <section className="details-grid">
        <div><strong>Datum:</strong> {new Date(booking.startTime).toLocaleDateString()}</div>
        <div><strong>Tid:</strong> {new Date(booking.startTime).toLocaleTimeString()} – {new Date(booking.endTime).toLocaleTimeString()}</div>
        <div><strong>Plats:</strong> {booking.zoneName}</div>
        <div><strong>Status:</strong> {booking.status}</div>
        <div><strong>Pris:</strong> {booking.price} kr</div>
        <div><strong>Bokad:</strong> {new Date(booking.createdAt).toLocaleString()}</div>
      </section>
    </div>
  );
}