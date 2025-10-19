import React, { useEffect, useState } from "react";
import { fetchBookings } from "../services/api";

export default function Statistics() {
  const [bookingStats, setBookingStats] = useState(null);
  const [sessionStats, setSessionStats] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    setLoading(true);
    setError(null);
    Promise.all([
      fetchBookings().then(res => {
        if (!res.ok) throw new Error("Network response was not ok (stats)");
        return res.json();
      }),
      fetchBookings().then(res => {
        if (!res.ok) throw new Error("Network response was not ok (session-stats)");
        return res.json();
      })
    ])
      .then(([bookingData, sessionData]) => {
        setBookingStats(bookingData);
        setSessionStats(sessionData);
      })
      .catch(err => setError(err))
      .finally(() => setLoading(false));
  }, []);

  if (loading) {
    return <div>Loading...</div>;
  }
  if (error) {
    return <div>Error: {error.message}</div>;
  }
  return (
    <div>
      <h1>Statistics</h1>
      <h2>Booking Stats</h2>
      <pre>{JSON.stringify(bookingStats, null, 2)}</pre>
      <h2>Session Stats</h2>
      <pre>{JSON.stringify(sessionStats, null, 2)}</pre>
    </div>
  );
}
