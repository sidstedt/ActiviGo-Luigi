import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import { fetchActivityOccurrences, fetchWeatherForecastBatch, fetchActivities, fetchZones, fetchLocations, createBooking } from "../services/api";
import BookingConfirmModal from "../components/BookingConfirmModal";
import { resolveLocationForOccurrence } from "../utils/location";
import ActivityCard from "../components/ActivityCard";
import "../styles/ActivityOccurrencesPage.css";

export default function ActivityOccurrencesPage() {
  const [searchParams] = useSearchParams();
  const [occurrences, setOccurrences] = useState([]);
  const [filteredOccurrences, setFilteredOccurrences] = useState([]);
  const [activities, setActivities] = useState({});
  const [allActivities, setAllActivities] = useState([]);
  const [zones, setZones] = useState([]);
  const [locations, setLocations] = useState([]);
  const [forecasts, setForecasts] = useState({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Booking confirmation modal state
  const [showConfirm, setShowConfirm] = useState(false);
  const [selectedOccurrenceId, setSelectedOccurrenceId] = useState(null);
  const [bookingLoading, setBookingLoading] = useState(false);
  const [bookingError, setBookingError] = useState("");

  // Filter states
  const [searchTerm, setSearchTerm] = useState("");
  const [debouncedSearchTerm, setDebouncedSearchTerm] = useState("");
  const [selectedActivity, setSelectedActivity] = useState("");
  const [dateFrom, setDateFrom] = useState("");
  const [dateTo, setDateTo] = useState("");

  useEffect(() => {
    loadData();
  }, []);

  useEffect(() => {
    applyFilters();
  }, [occurrences, debouncedSearchTerm, selectedActivity, dateFrom, dateTo]);

  // Debounce text search to avoid excessive filtering on every keystroke
  useEffect(() => {
    const t = setTimeout(() => setDebouncedSearchTerm(searchTerm), 300);
    return () => clearTimeout(t);
  }, [searchTerm]);

  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);

      const [occurrencesData, activitiesData, zonesData, locationsData] = await Promise.all([
        fetchActivityOccurrences(),
        fetchActivities(),
        fetchZones(),
        fetchLocations()
      ]);

      // Filter active occurrences
      const now = new Date();
      const activeOccurrences = occurrencesData
        .filter(occ => occ.isActive && !occ.isCancelled && new Date(occ.startTime) > now)
        .sort((a, b) => new Date(a.startTime) - new Date(b.startTime));

      setOccurrences(activeOccurrences);
      setAllActivities(activitiesData);
      setZones(zonesData);
      setLocations(locationsData);

      const activitiesMap = Object.fromEntries(activitiesData.map(a => [a.id, a]));
      setActivities(activitiesMap);

      // Load weather data
      const weatherQueries = activeOccurrences
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
        } catch (e) {
          console.error("Failed to load weather data:", e);
        }
      }

      // Set pre-filtered activity from URL
      const activityId = searchParams.get('activity');
      if (activityId) {
        setSelectedActivity(activityId);
      }

    } catch (err) {
      setError(err.message || "Kunde inte hämta data");
      console.error("Fel vid hämtning av data:", err);
    } finally {
      setLoading(false);
    }
  };

  const applyFilters = () => {
    let filtered = [...occurrences];

    // Text search
    if (debouncedSearchTerm) {
      filtered = filtered.filter(occurrence => {
        const activity = activities[occurrence.activityId];
        return activity && (
          activity.name.toLowerCase().includes(debouncedSearchTerm.toLowerCase()) ||
          activity.description.toLowerCase().includes(debouncedSearchTerm.toLowerCase())
        );
      });
    }

    // Activity filter
    if (selectedActivity) {
      filtered = filtered.filter(occurrence => occurrence.activityId === parseInt(selectedActivity));
    }

    // Date filters
    if (dateFrom) {
      const fromDate = new Date(dateFrom);
      filtered = filtered.filter(occurrence => new Date(occurrence.startTime) >= fromDate);
    }

    if (dateTo) {
      const toDate = new Date(dateTo);
      filtered = filtered.filter(occurrence => new Date(occurrence.startTime) <= toDate);
    }

    setFilteredOccurrences(filtered);
  };

  const clearFilters = () => {
    setSearchTerm("");
    setSelectedActivity("");
    setDateFrom("");
    setDateTo("");
  };
  
  // Open confirm modal (auth-gated)
  const openConfirm = (occurrenceId) => {
    // Require auth; if missing, redirect to login with next back to current page
    const token = localStorage.getItem('accessToken');
    if (!token) {
      const next = encodeURIComponent(window.location.pathname + window.location.search);
      window.location.href = `/login?next=${next}`;
      return;
    }
    setSelectedOccurrenceId(occurrenceId);
    setBookingError("");
    setShowConfirm(true);
  };

  // Confirm booking
  const confirmBooking = async () => {
    if (selectedOccurrenceId == null) return;
    setBookingLoading(true);
    setBookingError("");
    try {
      await createBooking({ activityOccurrenceId: selectedOccurrenceId });

      // Optimistically update available slots for this occurrence
      setOccurrences(prev => prev.map(o => (
        o.id === selectedOccurrenceId
          ? { ...o, availableSlots: Math.max(0, (o.availableSlots || 0) - 1) }
          : o
      )));
      setFilteredOccurrences(prev => prev.map(o => (
        o.id === selectedOccurrenceId
          ? { ...o, availableSlots: Math.max(0, (o.availableSlots || 0) - 1) }
          : o
      )));

      // Close modal on success
      setShowConfirm(false);
      setSelectedOccurrenceId(null);
    } catch (e) {
      console.error('Booking failed', e);
      setBookingError(e?.message || 'Kunde inte skapa bokningen. Försök igen.');
    } finally {
      setBookingLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="occurrences-page">
        <div className="loading">
          <div className="spinner"></div>
          <p>Laddar aktivitetshändelser...</p>
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
          <button onClick={loadData} className="retry-button">
            Försök igen
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="occurrences-page">
      <header className="page-header">
        <h1>Sök & Boka</h1>
        <p>Hitta och boka aktivitetshändelser</p>
      </header>

      {/* Filter Section */}
      <div className="filters-section">
        <div className="filters-grid">
          {/* Search */}
          <div className="filter-group">
            <label htmlFor="search">Sök aktivitet</label>
            <input
              id="search"
              type="text"
              placeholder="Sök efter namn eller beskrivning..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="filter-input"
            />
          </div>

          {/* Activity */}
          <div className="filter-group">
            <label htmlFor="activity">Aktivitet</label>
            <select
              id="activity"
              value={selectedActivity}
              onChange={(e) => setSelectedActivity(e.target.value)}
              className="filter-select"
            >
              <option value="">Alla aktiviteter</option>
              {allActivities.map(activity => (
                <option key={activity.id} value={activity.id}>
                  {activity.name}
                </option>
              ))}
            </select>
          </div>

          {/* Date From */}
          <div className="filter-group">
            <label htmlFor="dateFrom">Från datum</label>
            <input
              id="dateFrom"
              type="date"
              value={dateFrom}
              onChange={(e) => setDateFrom(e.target.value)}
              className="filter-input"
            />
          </div>

          {/* Date To */}
          <div className="filter-group">
            <label htmlFor="dateTo">Till datum</label>
            <input
              id="dateTo"
              type="date"
              value={dateTo}
              onChange={(e) => setDateTo(e.target.value)}
              className="filter-input"
            />
          </div>

          {/* Clear Filters */}
          <div className="filter-group">
            <button onClick={clearFilters} className="clear-filters-btn">
              Rensa filter
            </button>
          </div>
        </div>
      </div>

      {/* Results */}
      <div className="results-section">
        <div className="results-header">
          <h2>
            {filteredOccurrences.length} aktivitetshändelse{filteredOccurrences.length !== 1 ? 'r' : ''} hittade
          </h2>
        </div>

        {filteredOccurrences.length === 0 ? (
          <div className="no-occurrences">
            <p>Inga aktivitetshändelser matchade dina filter.</p>
            <button onClick={clearFilters} className="clear-filters-btn">
              Rensa alla filter
            </button>
          </div>
        ) : (
          <div className="occurrences-grid">
            {filteredOccurrences.map((occurrence) => {
              const activity = activities[occurrence.activityId];
              const price = activity ? activity.price : 0;
              const { locationName, address, lat, lon } = resolveLocationForOccurrence(occurrence, zones, locations);
              return (
                <ActivityCard
                  key={occurrence.id}
                  occurrence={occurrence}
                  price={price}
                  forecast={forecasts[occurrence.id]}
                  onBook={openConfirm}
                  locationName={locationName}
                  address={address}
                  lat={lat}
                  lon={lon}
                  description={activity?.description}
                />
              );
            })}
          </div>
        )}
      </div>
  {/* Booking confirmation modal */}
  {showConfirm && (
    <BookingConfirmModal
      isOpen={showConfirm}
      occurrence={occurrences.find(o => o.id === selectedOccurrenceId)}
      activity={(selectedOccurrenceId && activities[occurrences.find(o => o.id === selectedOccurrenceId)?.activityId]) || null}
      onClose={() => { if (!bookingLoading) { setShowConfirm(false); setSelectedOccurrenceId(null); } }}
      onConfirm={confirmBooking}
      loading={bookingLoading}
      error={bookingError}
    />
  )}
    </div>
  );
}

