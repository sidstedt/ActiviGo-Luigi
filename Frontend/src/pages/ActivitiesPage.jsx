import { useEffect, useState } from "react";
import { fetchActivities } from "../services/api";

export default function ActivitiesPage() {
  const [activities, setActivities] = useState([]);

  useEffect(() => {
    fetchActivities()
      .then(setActivities)
      .catch((err) => console.error("Fel:", err.message));
  }, []);

  return (
    <div>
      <h1>Aktiviteter</h1>
      <ul>
        {activities.map((a) => (
          <li key={a.id}>{a.name}</li>
        ))}
      </ul>
    </div>
  );
}
