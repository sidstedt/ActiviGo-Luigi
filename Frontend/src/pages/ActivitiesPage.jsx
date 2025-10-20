import { useEffect, useState } from "react";
import { fetchActivities, fetchZones, fetchLocations } from "../services/api";
import { getLocationAddressByZoneId } from "../utils/location";
import "../styles/ActivitiesPage.css";

export default function ActivitiesPage() {
  const [activities, setActivities] = useState([]);
  const [filteredActivities, setFilteredActivities] = useState([]);
  const [zones, setZones] = useState([]);
  const [locations, setLocations] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Filter state
  const [searchTerm, setSearchTerm] = useState("");
  const [debouncedSearchTerm, setDebouncedSearchTerm] = useState("");

  useEffect(() => {
    loadData();
  }, []);

  useEffect(() => {
    applyFilters();
  }, [activities, debouncedSearchTerm]);

  // Debounce text search
  useEffect(() => {
    const t = setTimeout(() => setDebouncedSearchTerm(searchTerm), 300);
    return () => clearTimeout(t);
  }, [searchTerm]);

  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);

      const [activitiesData, zonesData, locationsData] = await Promise.all([
        fetchActivities(),
        fetchZones(),
        fetchLocations()
      ]);

      setActivities(activitiesData);
      setZones(zonesData);
      setLocations(locationsData);
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
      filtered = filtered.filter(activity =>
        activity.name.toLowerCase().includes(debouncedSearchTerm.toLowerCase()) ||
        activity.description.toLowerCase().includes(debouncedSearchTerm.toLowerCase())
      );
    }

    // removed category, zone, location, and indoor/outdoor filters

    setFilteredActivities(filtered);
  };

  const clearFilters = () => {
    setSearchTerm("");
  };

  // Address resolution via shared helper
  const getLocationAddress = (zoneId) => getLocationAddressByZoneId(zones, locations, zoneId);

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
            {filteredActivities.length} aktivitet{filteredActivities.length !== 1 ? 'er' : ''} hittade
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
            {filteredActivities.map(activity => (
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
                        {getLocationAddress(activity.zoneId) || 'Okänd adress'}
                      </span>
                    </div>
                    
                    <div className="detail-item">
                      <span className="detail-label">Max deltagare:</span>
                      <span className="detail-value">{activity.maxParticipants}</span>
                    </div>
                    
                    <div className="detail-item">
                      <span className="detail-label">Längd:</span>
                      <span className="detail-value">{activity.durationMinutes} min</span>
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
                      const token = localStorage.getItem('accessToken');
                      const target = token ? `/bookings?activity=${activity.id}` : `/login?next=/bookings%3Factivity%3D${activity.id}`;
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

