import { useEffect, useState } from "react";
import { fetchActivityOccurrences, fetchActivityById } from "../services/api";
import ActivityCard from "../components/ActivityCard";
import "./ActivityOccurrencesPage.css";

export default function ActivityOccurrencesPage() {
  const [occurrences, setOccurrences] = useState([]);
  const [activities, setActivities] = useState({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    loadOccurrences();
  }, []);

  const loadOccurrences = async () => {
    try {
      setLoading(true);
      setError(null);
      const occurrencesData = await fetchActivityOccurrences();
      
      const now = new Date();
      const activeOccurrences = occurrencesData
        .filter(occ => occ.isActive && !occ.isCancelled && new Date(occ.startTime) > now)
        .sort((a, b) => new Date(a.startTime) - new Date(b.startTime));

      setOccurrences(activeOccurrences);
      
      const activityIds = [...new Set(activeOccurrences.map(occ => occ.activityId))];
      const activitiesMap = {};
      
      await Promise.all(
        activityIds.map(async (id) => {
          try {
            const activity = await fetchActivityById(id);
            activitiesMap[id] = activity;
          } catch (err) {

          }
        })
      );

      setActivities(activitiesMap);
    } catch (err) {
      setError(err.message || "Kunde inte hämta aktivitetshändelser");
    } finally {
      setLoading(false);
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
          <button onClick={loadOccurrences} className="retry-button">
            Försök igen
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="occurrences-page">
      <header className="page-header">
        <h1>Tillgängliga Aktivitetshändelser</h1>
      </header>

      {occurrences.length === 0 ? (
        <div className="no-occurrences">
          <p>Inga tillgängliga aktivitetshändelser just nu.</p>
        </div>
      ) : (
        <div className="occurrences-grid">
          {occurrences.map((occurrence) => {
            const activity = activities[occurrence.activityId];
            const price = activity ? activity.price : 0;
            return (
              <ActivityCard
                key={occurrence.id}
                occurrence={occurrence}
                price={price}
                onBook={() => {}}
              />
            );
          })}
        </div>
      )}
    </div>
  );
}
