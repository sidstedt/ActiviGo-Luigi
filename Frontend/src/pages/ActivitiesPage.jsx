import { useEffect, useState } from "react";
import {
  fetchActivities,
  fetchZones,
  fetchLocations,
  fetchCategories,
} from "../services/api";
import { getLocationAddressByZoneId } from "../utils/location";
import "../styles/ActivitiesPage.css";

export default function ActivitiesPage() {
  const [activities, setActivities] = useState([]);
  const [filteredActivities, setFilteredActivities] = useState([]);
  const [zones, setZones] = useState([]);
  const [locations, setLocations] = useState([]);
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Filter state
  const [searchTerm, setSearchTerm] = useState("");
  const [debouncedSearchTerm, setDebouncedSearchTerm] = useState("");
  const [selectedCategory, setSelectedCategory] = useState("");
  const [selectedPlace, setSelectedPlace] = useState(""); // "" | "indoor" | "outdoor"

  useEffect(() => {
    loadData();
  }, []);

  useEffect(() => {
    applyFilters();
  }, [activities, debouncedSearchTerm, selectedCategory, selectedPlace]);

  // Debounce text search
  useEffect(() => {
    const t = setTimeout(() => setDebouncedSearchTerm(searchTerm), 300);
    return () => clearTimeout(t);
  }, [searchTerm]);

  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);

      const [activitiesData, zonesData, locationsData, categoriesData] =
        await Promise.all([
          fetchActivities(),
          fetchZones(),
          fetchLocations(),
          fetchCategories(),
        ]);

      setActivities(activitiesData || []);
      setZones(zonesData || []);
      setLocations(locationsData || []);
      setCategories(Array.isArray(categoriesData) ? categoriesData : []);
    } catch (err) {
      setError(err.message || "Kunde inte hämta data");
      console.error("Fel vid hämtning av data:", err);
    } finally {
      setLoading(false);
    }
  };

  const applyFilters = () => {
    let filtered = [...activities];

    // Text search
    if (debouncedSearchTerm) {
      filtered = filtered.filter(
        (activity) =>
          activity.name
            .toLowerCase()
            .includes(debouncedSearchTerm.toLowerCase()) ||
          activity.description
            .toLowerCase()
            .includes(debouncedSearchTerm.toLowerCase())
      );
    }

    // Category filter — selectedCategory is the index into categories[]
    if (selectedCategory) {
      const selIdx = Number(selectedCategory);
      const selCat = categories[selIdx];
      const selName = selCat ? String(selCat.name || '').toLowerCase() : '';
      filtered = filtered.filter((a) => {
        // Possible places to find category info on activity
        const actCatRaw = a.categoryId ?? a.CategoryId ?? a.category?.id ?? a.category?.categoryId ?? a.categoryIndex ?? a.catIndex ?? null;
        const actCatName = String(a.categoryName || a.category?.name || '').toLowerCase();

        if (actCatRaw != null) {
          // numeric match against selected index
          if (!isNaN(Number(actCatRaw)) && Number(actCatRaw) === selIdx) return true;
          // match against selected category id if present
          if (selCat && !isNaN(Number(actCatRaw)) && Number(actCatRaw) === Number(selCat.id ?? selCat.categoryId)) return true;
          // string match (some APIs store id as string)
          if (selCat && String(actCatRaw) === String(selCat.id ?? selCat.categoryId)) return true;
        }

        // name-based fallback
        if (selName && actCatName && selName === actCatName) return true;

        return false;
      });
    }

    // Place filter (determine via activity.zoneId -> zone/location flags)
    if (selectedPlace) {
      if (selectedPlace === 'outdoor') {
        filtered = filtered.filter((a) => getActivityPlace(a) === 'outdoor');
      } else if (selectedPlace === 'indoor') {
        // treat anything not explicitly classified as 'outdoor' as indoor — backend often only marks outdoor zones
        filtered = filtered.filter((a) => getActivityPlace(a) !== 'outdoor');
      }
    }

    setFilteredActivities(filtered);
  };

  const clearFilters = () => {
    setSearchTerm("");
    setSelectedCategory("");
    setSelectedPlace("");
  };

  // Address resolution via shared helper
  const getLocationAddress = (zoneId) =>
    getLocationAddressByZoneId(zones, locations, zoneId);

  // Determine whether an activity is indoor/outdoor based on its zone or linked location.
  // Assumptions: zones or locations may include boolean flags `isOutdoor`/`isIndoor`.
  // If neither flag is present, we treat place as 'unknown'.
  const getActivityPlace = (activity) => {
    try {
      const zid =
        activity.zoneId ?? activity.ZoneId ?? activity.zone?.id ?? null;
      if (!zid) return null;
      const zone = zones.find((z) => (z.id ?? z.zoneId) === zid) || null;
      // zone-level flags
      if (zone) {
        // if isOutdoor explicitly true -> outdoor
        if (zone.isOutdoor === true) return 'outdoor';
        // if isOutdoor explicitly false -> indoor
        if (zone.isOutdoor === false) return 'indoor';
        if (zone.isIndoor === true) return 'indoor';
        if (zone.isIndoor === false) return 'outdoor';
      }
      // location-level flags
      const loc =
        locations.find(
          (l) => l.id === (zone?.locationId ?? zone?.LocationId)
        ) ||
        locations.find(
          (l) =>
            Array.isArray(l.zones) &&
            l.zones.some((zz) => (zz.id ?? zz.zoneId) === zid)
        );
      if (loc) {
        if (loc.isOutdoor === true) return 'outdoor';
        if (loc.isOutdoor === false) return 'indoor';
        if (loc.isIndoor === true) return 'indoor';
        if (loc.isIndoor === false) return 'outdoor';
      }
      return null;
    } catch {
      return null;
    }
  };

  if (loading) {
    return (
      <div className="activities-page">
        <div className="loading">
          <div className="spinner"></div>
          <p>Laddar aktiviteter...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="activities-page">
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
    <div className="activities-page">
      <header className="page-header">
        <h1>Aktiviteter</h1>
        <p>Hitta din perfekta aktivitet</p>
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

          {/* Category */}
          <div className="filter-group">
            <label htmlFor="category">Kategori</label>
            <select
              id="category"
              value={selectedCategory}
              onChange={(e) => setSelectedCategory(e.target.value)}
              className="filter-select"
            >
              <option value="">Alla kategorier</option>
              {categories.map((c, idx) => (
                // use index as option value because some parts of the app store category by index
                <option key={c.id ?? c.categoryId ?? idx} value={String(idx)}>
                  {c.name}
                </option>
              ))}
            </select>
          </div>

          {/* Place (Indoor / Outdoor) */}
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
          {/* removed: category, location, zone, indoor/outdoor filters */}

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
            {filteredActivities.length} aktivitet
            {filteredActivities.length !== 1 ? "er" : ""} hittade
          </h2>
        </div>

        {filteredActivities.length === 0 ? (
          <div className="no-results">
            <p>Inga aktiviteter matchade dina filter.</p>
            <button onClick={clearFilters} className="clear-filters-btn">
              Rensa alla filter
            </button>
          </div>
        ) : (
          <div className="activities-grid">
            {filteredActivities.map((activity) => (
              <div key={activity.id} className="activity-card">
                <div className="activity-header">
                  <h3 className="activity-name">{activity.name}</h3>
                </div>

                <div className="activity-content">
                  <p className="activity-description">{activity.description}</p>

                  <div className="activity-details">
                    {/* Address */}
                    <div className="detail-item">
                      <span className="detail-label">Adress:</span>
                      <span className="detail-value">
                        {getLocationAddress(activity.zoneId) || "Okänd adress"}
                      </span>
                    </div>

                    <div className="detail-item">
                      <span className="detail-label">Max deltagare:</span>
                      <span className="detail-value">
                        {activity.maxParticipants}
                      </span>
                    </div>

                    <div className="detail-item">
                      <span className="detail-label">Längd:</span>
                      <span className="detail-value">
                        {activity.durationMinutes} min
                      </span>
                    </div>
                  </div>
                </div>

                <div className="activity-footer">
                  <div className="activity-status">
                    {activity.isPrivate && (
                      <span className="status-badge private">Privat</span>
                    )}
                  </div>

                  <button
                    className="view-details-btn"
                    onClick={() => {
                      const token = localStorage.getItem("accessToken");
                      const target = token
                        ? `/bookings?activity=${activity.id}`
                        : `/login?next=/bookings%3Factivity%3D${activity.id}`;
                      window.location.href = target;
                    }}
                  >
                    Sök & Boka
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}
