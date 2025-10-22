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

  // No search UI: we list all activities directly

  useEffect(() => {
    loadData();
  }, []);

  useEffect(() => {
    // Always show all activities (no search/filtering on Activities page)
    setFilteredActivities(Array.isArray(activities) ? activities : []);
  }, [activities]);

  // (removed search debounce — page lists all activities)

  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);

      const [activitiesData, zonesData, locationsData] = await Promise.all([
        fetchActivities(),
        fetchZones(),
        fetchLocations(),
      ]);

      setActivities(activitiesData || []);
      setZones(zonesData || []);
      setLocations(locationsData || []);
    } catch (err) {
      setError(err.message || "Kunde inte hämta data");
      console.error("Fel vid hämtning av data:", err);
    } finally {
      setLoading(false);
    }
  };

  // applyFilters removed — page always shows all activities via useEffect above

  // No clearFilters function — this page shows all activities

  // Address resolution via shared helper
  const getLocationAddress = (zoneId) =>
    getLocationAddressByZoneId(zones, locations, zoneId);

  // (no indoor/outdoor helper on this page)

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

      {/* Results */}
      <div className="results-section">
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
      </div>
    </div>
  );
}
