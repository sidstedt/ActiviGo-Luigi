import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import {
  fetchActivityOccurrences,
  fetchWeatherForecastBatch,
  fetchActivities,
  fetchZones,
  fetchLocations,
  fetchCategories,
  createBooking,
} from "../services/api";
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
  const [categories, setCategories] = useState([]);
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
  const [selectedCategory, setSelectedCategory] = useState("");
  const [selectedPlace, setSelectedPlace] = useState("");
  const [dateFrom, setDateFrom] = useState("");
  const [dateTo, setDateTo] = useState("");

  useEffect(() => {
    loadData();
  }, []);

  useEffect(() => {
    applyFilters();
  }, [
    occurrences,
    debouncedSearchTerm,
    selectedActivity,
    dateFrom,
    dateTo,
    selectedCategory,
    selectedPlace,
  ]);

  // Debounce text search to avoid excessive filtering on every keystroke
  useEffect(() => {
    const t = setTimeout(() => setDebouncedSearchTerm(searchTerm), 300);
    return () => clearTimeout(t);
  }, [searchTerm]);

  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);

      const [
        occurrencesData,
        activitiesData,
        zonesData,
        locationsData,
        categoriesData,
      ] = await Promise.all([
        fetchActivityOccurrences(),
        fetchActivities(),
        fetchZones(),
        fetchLocations(),
        fetchCategories(),
      ]);

      // Filter active occurrences
      const now = new Date();
      const activeOccurrences = occurrencesData
        .filter(
          (occ) =>
            occ.isActive && !occ.isCancelled && new Date(occ.startTime) > now
        )
        .sort((a, b) => new Date(a.startTime) - new Date(b.startTime));

      setOccurrences(activeOccurrences);
      setAllActivities(activitiesData);
      setZones(zonesData);
      setLocations(locationsData);
      setCategories(Array.isArray(categoriesData) ? categoriesData : []);

      const activitiesMap = Object.fromEntries(
        activitiesData.map((a) => [a.id, a])
      );
      setActivities(activitiesMap);

      // Load weather data
      const weatherQueries = activeOccurrences
        .filter((o) => o.isOutdoor && o.latitude != null && o.longitude != null)
        .map((o) => ({
          occurrenceId: o.id,
          latitude: o.latitude,
          longitude: o.longitude,
          at: new Date(o.startTime).toISOString(),
        }));

      if (weatherQueries.length > 0) {
        try {
          const batch = await fetchWeatherForecastBatch(weatherQueries);
          const map = {};
          batch.forEach((item) => {
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
      const activityId = searchParams.get("activity");
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
      filtered = filtered.filter((occurrence) => {
        const activity = activities[occurrence.activityId];
        return (
          activity &&
          (activity.name
            .toLowerCase()
            .includes(debouncedSearchTerm.toLowerCase()) ||
            activity.description
              .toLowerCase()
              .includes(debouncedSearchTerm.toLowerCase()))
        );
      });
    }

    // Activity filter
    if (selectedActivity) {
      filtered = filtered.filter(
        (occurrence) => occurrence.activityId === parseInt(selectedActivity)
      );
    }

    // Category filter — be tolerant about how categories are stored (id, categoryId, or numeric index)
    if (selectedCategory) {
      const selRaw = String(selectedCategory);
      // Try to resolve selected category object (by id/categoryId or by index)
      const selById = categories.find(
        (c) => String(c.id ?? c.categoryId) === selRaw
      );
      const selByIdx =
        !isNaN(Number(selRaw)) && categories[Number(selRaw)]
          ? categories[Number(selRaw)]
          : null;
      const selCat = selById || selByIdx || null;

      filtered = filtered.filter((occurrence) => {
        const act = activities[occurrence.activityId];
        if (!act) return false;

        // Raw category value(s) on activity
        const actCatRaw =
          act.categoryId ??
          act.CategoryId ??
          act.category?.id ??
          act.category?.categoryId ??
          act.categoryIndex ??
          act.catIndex ??
          null;

        const actCatName = String(
          act.categoryName || act.category?.name || ""
        ).toLowerCase();

        // If activity stores the same id/string as the selected raw value -> match
        if (actCatRaw != null && String(actCatRaw) === selRaw) return true;

        // If activity stored a numeric index into the categories array, resolve it and compare
        if (actCatRaw != null && !isNaN(Number(actCatRaw))) {
          const idx = Number(actCatRaw);
          const catAtIdx = categories[idx];
          if (catAtIdx) {
            const catAtIdxId = String(catAtIdx.id ?? catAtIdx.categoryId ?? "");
            if (catAtIdxId && catAtIdxId === selRaw) return true;
            // also if selected was an index and matches this index
            if (!isNaN(Number(selRaw)) && Number(selRaw) === idx) return true;
          }
        }

        // If we resolved a selected category object, compare by its id/categoryId or name
        if (selCat) {
          const selId = String(selCat.id ?? selCat.categoryId ?? "");
          if (selId && actCatRaw != null && String(actCatRaw) === selId)
            return true;
          if (selId && !isNaN(Number(actCatRaw))) {
            const idx = Number(actCatRaw);
            const catAtIdx = categories[idx];
            if (
              catAtIdx &&
              String(catAtIdx.id ?? catAtIdx.categoryId) === selId
            )
              return true;
          }
          const selName = String(selCat.name || "").toLowerCase();
          if (selName && actCatName && selName === actCatName) return true;
        }

        // Fallback: compare activity category name to selected raw string
        if (actCatName && actCatName === selRaw.toLowerCase()) return true;

        return false;
      });
    }

    // Place filter
    if (selectedPlace) {
      if (selectedPlace === "outdoor") {
        filtered = filtered.filter((o) => getOccurrencePlace(o) === "outdoor");
      } else if (selectedPlace === "indoor") {
        filtered = filtered.filter((o) => getOccurrencePlace(o) !== "outdoor");
      }
    }

    // Date filters
    if (dateFrom) {
      const fromDate = new Date(dateFrom);
      filtered = filtered.filter(
        (occurrence) => new Date(occurrence.startTime) >= fromDate
      );
    }

    if (dateTo) {
      const toDate = new Date(dateTo);
      filtered = filtered.filter(
        (occurrence) => new Date(occurrence.startTime) <= toDate
      );
    }

    setFilteredOccurrences(filtered);
  };

  // derive place (indoor/outdoor) for an occurrence
  const getOccurrencePlace = (occurrence) => {
    // prefer explicit flag on occurrence
    if (occurrence.isOutdoor === true) return "outdoor";
    if (occurrence.isOutdoor === false) return "indoor";
    // fallback to zone/location flags
    try {
      const zid = occurrence.zoneId ?? null;
      if (!zid) return null;
      const zone = zones.find((z) => (z.id ?? z.zoneId) === zid) || null;
      if (zone) {
        if (zone.isOutdoor === true) return "outdoor";
        if (zone.isOutdoor === false) return "indoor";
        if (zone.isIndoor === true) return "indoor";
        if (zone.isIndoor === false) return "outdoor";
      }
      const loc =
        locations.find((l) => l.id === zone?.locationId) ||
        locations.find(
          (l) =>
            Array.isArray(l.zones) &&
            l.zones.some((zz) => (zz.id ?? zz.zoneId) === zid)
        );
      if (loc) {
        if (loc.isOutdoor === true) return "outdoor";
        if (loc.isOutdoor === false) return "indoor";
        if (loc.isIndoor === true) return "indoor";
        if (loc.isIndoor === false) return "outdoor";
      }
    } catch {
      // ignore
    }
    return null;
  };

  const clearFilters = () => {
    setSearchTerm("");
    setSelectedActivity("");
    setSelectedCategory("");
    setSelectedPlace("");
    setDateFrom("");
    setDateTo("");
  };

  // Open confirm modal (auth-gated)
  const openConfirm = (occurrenceId) => {
    // Require auth; if missing, redirect to login with next back to current page
    const token = localStorage.getItem("accessToken");
    if (!token) {
      const next = encodeURIComponent(
        window.location.pathname + window.location.search
      );
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
      setOccurrences((prev) =>
        prev.map((o) =>
          o.id === selectedOccurrenceId
            ? { ...o, availableSlots: Math.max(0, (o.availableSlots || 0) - 1) }
            : o
        )
      );
      setFilteredOccurrences((prev) =>
        prev.map((o) =>
          o.id === selectedOccurrenceId
            ? { ...o, availableSlots: Math.max(0, (o.availableSlots || 0) - 1) }
            : o
        )
      );

      // Close modal on success
      setShowConfirm(false);
      setSelectedOccurrenceId(null);
    } catch (e) {
      console.error("Booking failed", e);
      setBookingError(e?.message || "Kunde inte skapa bokningen. Försök igen.");
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
          <div className="filters-row filters-row--top">
            {/* Category (moved first) */}
            <div className="filter-group">
              <label htmlFor="category">Kategori</label>
              <select
                id="category"
                value={selectedCategory}
                onChange={(e) => setSelectedCategory(e.target.value)}
                className="filter-select"
              >
                <option value="">Alla kategorier</option>
                {categories.map((c) => (
                  <option
                    key={c.id ?? c.categoryId}
                    value={c.id ?? c.categoryId}
                  >
                    {c.name}
                  </option>
                ))}
              </select>
            </div>

            {/* Activity (second) */}
            <div className="filter-group">
              <label htmlFor="activity">Aktivitet</label>
              <select
                id="activity"
                value={selectedActivity}
                onChange={(e) => setSelectedActivity(e.target.value)}
                className="filter-select"
              >
                <option value="">Alla aktiviteter</option>
                {allActivities.map((activity) => (
                  <option key={activity.id} value={activity.id}>
                    {activity.name}
                  </option>
                ))}
              </select>
            </div>

            {/* Place (third) */}
            <div className="filter-group">
              <label htmlFor="place">Plats</label>
              <select
                id="place"
                value={selectedPlace}
                onChange={(e) => setSelectedPlace(e.target.value)}
                className="filter-select"
              >
                <option value="">Alla</option>
                <option value="indoor">Inomhus</option>
                <option value="outdoor">Utomhus</option>
              </select>
            </div>

            {/* Clear Filters moved to dates row */}
          </div>

          {/* Dates row - explicit second row (dates + clear button) */}
          <div className="filters-row filters-row--dates">
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

            <div className="filter-group filter-group--right">
              <label style={{ visibility: "hidden" }}>.</label>
              <button onClick={clearFilters} className="clear-filters-btn">
                Rensa filter
              </button>
            </div>
          </div>
        </div>
      </div>

      {/* Results */}
      <div className="results-section">
        <div className="results-header">
          <h2>
            {filteredOccurrences.length} aktivitetshändelse
            {filteredOccurrences.length !== 1 ? "r" : ""} hittade
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
              const { locationName, address, lat, lon } =
                resolveLocationForOccurrence(occurrence, zones, locations);
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
                  imageUrl={activity?.imageUrl}
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
          occurrence={occurrences.find((o) => o.id === selectedOccurrenceId)}
          activity={
            (selectedOccurrenceId &&
              activities[
                occurrences.find((o) => o.id === selectedOccurrenceId)
                  ?.activityId
              ]) ||
            null
          }
          onClose={() => {
            if (!bookingLoading) {
              setShowConfirm(false);
              setSelectedOccurrenceId(null);
            }
          }}
          onConfirm={confirmBooking}
          loading={bookingLoading}
          error={bookingError}
        />
      )}
    </div>
  );
}
