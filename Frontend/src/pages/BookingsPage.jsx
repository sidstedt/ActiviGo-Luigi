import { useEffect, useState } from "react";
import { fetchMyBookings, fetchActivityById, fetchActivityOccurrenceById, fetchWeatherForecastBatch, cancelBooking } from "../services/api";
import ActivityCard from "../components/ActivityCard";
import "../styles/ActivityOccurrencesPage.css";

export default function BookingsPage() {
  const [bookings, setBookings] = useState([]);
  const [occurrences, setOccurrences] = useState({});
  const [activities, setActivities] = useState({});
  const [forecasts, setForecasts] = useState({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    load();
  }, []);

  const load = async () => {
    try {
      setLoading(true);
      setError(null);
      const my = await fetchMyBookings();
      setBookings(my);

      // Load occurrences and activities referenced by bookings
      const occurrenceIds = [...new Set(my.map(b => b.activityOccurrenceId))];
      const occMap = {};
      const actMap = {};
      for (const occId of occurrenceIds) {
        try {
          const occ = await fetchActivityOccurrenceById(occId);
          occMap[occId] = occ;
          if (occ?.activityId && !actMap[occ.activityId]) {
            const act = await fetchActivityById(occ.activityId);
            actMap[occ.activityId] = act;
          }
        } catch {}
      }
      setOccurrences(occMap);
      setActivities(actMap);

      // Weather for outdoor
      const weatherQueries = Object.values(occMap)
        .filter(o => o?.isOutdoor && o.latitude != null && o.longitude != null)
        .map(o => ({
          occurrenceId: o.id,
          latitude: o.latitude,
          longitude: o.longitude,
          at: new Date(o.startTime).toISOString()
        }));
      if (weatherQueries.length > 0) {
        try {
          const batch = await fetchWeatherForecastBatch(weatherQueries);
          const map = {};
          batch.forEach(item => {
            if (item.occurrenceId != null && item.forecast) {
              map[item.occurrenceId] = item.forecast;
            }
          });
          setForecasts(map);
        } catch {}
      }
    } catch (e) {
      setError(e.message || "Kunde inte hämta bokningar");
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="occurrences-page">
        <div className="loading">
          <div className="spinner"></div>
          <p>Laddar dina bokningar…</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="occurrences-page">
        <div className="error-message">
          <h2>⚠️ Ett fel uppstod</h2>
          <p>{error}</p>
          <button onClick={load} className="retry-button">Försök igen</button>
        </div>
      </div>
    );
  }

  if (!bookings || bookings.length === 0) {
    return (
      <div className="occurrences-page">
        <header className="page-header">
          <h1>Mina bokningar</h1>
          <p className="subtitle">Du har inga bokningar ännu.</p>
        </header>
      </div>
    );
  }

  const isUpcoming = (occ) => {
    if (!occ) return false;
    return new Date(occ.startTime) > new Date();
  };

  const isCancelledStatus = (status) => {
    const raw = String(status || '').toLowerCase();
    return raw === 'cancelled' || raw === 'canceled' || raw === 'avbokad';
  };

  const handleCancel = async (bookingId) => {
    try {
      await cancelBooking(bookingId);
      await load();
    } catch (e) {
      alert(e.message || 'Kunde inte avboka');
    }
  };

  return (
    <div className="occurrences-page">
      <header className="page-header">
        <h1>Mina bokningar</h1>
        <p className="subtitle">Alla dina kommande och tidigare bokningar</p>
      </header>

      <div className="occurrences-grid" style={{ gridTemplateColumns: '1fr' }}>
        {bookings.map((b) => {
          const occ = occurrences[b.activityOccurrenceId];
          if (!occ) return null;
          const act = activities[occ.activityId];
          const price = act ? act.price : 0;
          const upcoming = isUpcoming(occ);
          const cancelled = isCancelledStatus(b.status);
          const showCancel = upcoming && !cancelled;
          return (
            <div key={b.id}>
              <ActivityCard
                occurrence={occ}
                price={price}
                forecast={forecasts[occ.id]}
                onBook={() => handleCancel(b.id)}
                actionLabel={showCancel ? 'Avboka' : (upcoming ? 'Avbokad' : 'Genomförd')}
                hideAction={!showCancel}
                bookingInfo={{
                  id: b.id,
                  status: b.status,
                  createdAt: b.createdAt || b.createdDate || new Date().toISOString(),
                  seats: typeof b.seats === 'number' ? b.seats : undefined
                }}
              />
            </div>
          );
        })}
      </div>
    </div>
  );
}


