import { useEffect, useMemo, useState } from "react";
import { fetchActivities, fetchActivityOccurrences, fetchWeatherForecastBatch } from "../services/api";
import ActivityCard from "../components/ActivityCard";
import "../styles/ActivityOccurrencesPage.css";

export default function ActivitiesPage() {
  // Data
  const [activities, setActivities] = useState([]);
  const [occurrences, setOccurrences] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [forecasts, setForecasts] = useState({});

  // Filters
  const [selectedActivityId, setSelectedActivityId] = useState("");
  const [date, setDate] = useState("");
  const [indoorOutdoor, setIndoorOutdoor] = useState(""); // "indoor" | "outdoor" | ""
  const [minParticipants, setMinParticipants] = useState("");
  const resetFilters = () => {
    setSelectedActivityId("");
    setDate("");
    setIndoorOutdoor("");
    setMinParticipants("");
  };

  useEffect(() => {
    const load = async () => {
      try {
        setLoading(true);
        setError("");
        const [acts, occs] = await Promise.all([
          fetchActivities(),
          fetchActivityOccurrences(),
        ]);
        setActivities(acts);
        setOccurrences(occs);

        // Fetch weather forecasts for outdoor occurrences if coordinates are present
        const weatherQueries = (occs || [])
          .filter(o => o.isOutdoor && o.latitude != null && o.longitude != null)
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
          } catch {
            // ignore weather errors for search page
          }
        }
      } catch (e) {
        setError(e.message || "Kunde inte hämta data");
      } finally {
        setLoading(false);
      }
    };
    load();
  }, []);

  const filteredOccurrences = useMemo(() => {
    return occurrences.filter((o) => {
      // Aktivitet
      if (selectedActivityId && String(o.activityId) !== String(selectedActivityId)) {
        return false;
      }
      // Datum (YYYY-MM-DD)
      if (date) {
        const d = new Date(o.startTime);
        const iso = d.toISOString().slice(0, 10);
        if (iso !== date) return false;
      }
      // Inomhus/Utomhus
      if (indoorOutdoor === "indoor" && o.isOutdoor) return false;
      if (indoorOutdoor === "outdoor" && !o.isOutdoor) return false;
      // Antal deltagare (kräver minst minParticipants lediga platser)
      if (minParticipants) {
        const min = Number(minParticipants);
        if (!Number.isNaN(min) && o.availableSlots < min) return false;
      }
      return true;
    });
  }, [occurrences, selectedActivityId, date, indoorOutdoor, minParticipants]);

  if (loading) {
    return (
      <div className="occurrences-page">
        <div className="loading">
          <div className="spinner"></div>
          <p>Hämtar aktiviteter…</p>
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
        </div>
      </div>
    );
  }

  const uniqueActivities = activities;

  return (
    <div className="occurrences-page">
      <header className="page-header">
        <h1>Sök aktivitet</h1>
        <p className="subtitle">Filtrera på aktivitet, datum, inomhus/utomhus och antal deltagare</p>
      </header>

      <div className="filters-panel">
        <div>
          <label className="filter-label">Aktivitet</label>
          <select className="filter-control" value={selectedActivityId} onChange={(e) => setSelectedActivityId(e.target.value)}>
            <option value="">Alla</option>
            {uniqueActivities.map((a) => (
              <option key={a.id} value={a.id}>{a.name}</option>
            ))}
          </select>
        </div>
        <div>
          <label className="filter-label">Datum</label>
          <input className="filter-control" type="date" value={date} onChange={(e) => setDate(e.target.value)} />
        </div>
        <div>
          <label className="filter-label">Inomhus/Utomhus</label>
          <select className="filter-control" value={indoorOutdoor} onChange={(e) => setIndoorOutdoor(e.target.value)}>
            <option value="">Alla</option>
            <option value="indoor">Inomhus</option>
            <option value="outdoor">Utomhus</option>
          </select>
        </div>
        <div>
          <label className="filter-label">Antal deltagare</label>
          <input
            className="filter-control"
            type="number"
            min="1"
            placeholder="Minst…"
            value={minParticipants}
            onChange={(e) => setMinParticipants(e.target.value)}
          />
        </div>
        <div className="filters-actions">
          <button type="button" className="reset-button" onClick={resetFilters}>Rensa filter</button>
        </div>
      </div>

      {filteredOccurrences.length === 0 ? (
        <div className="no-occurrences">
          <p>Inga träffar med valda filter.</p>
        </div>
      ) : (
        <div className="occurrences-grid">
          {filteredOccurrences.map((occurrence) => {
            const activity = activities.find(a => String(a.id) === String(occurrence.activityId));
            const price = activity ? activity.price : 0;
            return (
              <ActivityCard
                key={occurrence.id}
                occurrence={occurrence}
                price={price}
                forecast={forecasts[occurrence.id]}
                onBook={() => {}}
              />
            );
          })}
        </div>
      )}
    </div>
  );
}

