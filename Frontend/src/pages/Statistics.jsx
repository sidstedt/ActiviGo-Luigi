import React, { useEffect, useState } from "react";
import {
  fetchAdminBookings,
  fetchUserBookings,
  fetchActivityOccurrences,
  getCurrentUser,
} from "../services/api";
import {
  getBookingDate,
  calculateBookingsPerMonth,
  monthsSvShort,
  Tableau10,
} from "../utils/statistics";
import MonthlyBookingsLineChart from "../components/stats/MonthlyBookingsLineChart";
import ActivityDistributionPie from "../components/stats/ActivityDistributionPie";

export default function Statistics() {
  const [bookings, setBookings] = useState([]);
  const [activityOccurrences, setActivityOccurrences] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [bookingsPerMonth, setBookingsPerMonth] = useState(Array(12).fill(0));
  const [selectedYear, setSelectedYear] = useState(null);

  // Fetch data
  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      setError(null);
      const user = getCurrentUser();
      const roles = Array.isArray(user?.roles) ? user.roles : [];
      const isAdmin = roles
        .map((r) => String(r).toLowerCase())
        .includes("admin");

      try {
        const [bookingsData, occurrencesData] = isAdmin
          ? await Promise.all([
              fetchAdminBookings(),
              fetchActivityOccurrences(),
            ])
          : await Promise.all([
              fetchUserBookings(),
              fetchActivityOccurrences(),
            ]);

        setBookings(bookingsData || []);
        setActivityOccurrences(occurrencesData || []);
      } catch (err) {
        console.error("Fel vid hämtning av data:", err);
        setError(err);
      } finally {
        setLoading(false);
      }
    };
    fetchData();
  }, []);

  useEffect(() => {
    const years = Array.from(
      new Set(
        (bookings || [])
          .map((b) => getBookingDate(b))
          .filter((d) => d && !isNaN(d))
          .map((d) => d.getFullYear())
      )
    ).sort((a, b) => b - a);
    if (years.length > 0) {
      const current = new Date().getFullYear();
      setSelectedYear(years.includes(current) ? current : years[0]);
    } else {
      setSelectedYear(null);
    }
  }, [bookings]);

  useEffect(() => {
    const data = calculateBookingsPerMonth(
      bookings || [],
      selectedYear || undefined
    );
    setBookingsPerMonth(data);
  }, [bookings, selectedYear]);

  if (loading) {
    return <div style={{ padding: "2rem" }}>Laddar statistik...</div>;
  }

  if (error) {
    return (
      <div style={{ padding: "2rem", color: "red" }}>
        <h2>Fel vid hämtning</h2>
        <p>
          <strong>Felmeddelande:</strong> {error.message}
        </p>
        {error.message.includes("401") && (
          <div>
            <ul>
              <li>Logga ut och logga in igen</li>
            </ul>
          </div>
        )}
      </div>
    );
  }

  const totalBookings = bookings.length;
  const totalOccurrences = activityOccurrences.length;

  return (
    <div style={{ padding: "2rem" }}>
      <h1>Statistik</h1>

      {/* Översikt */}
      <div style={{ marginBottom: "2rem" }}>
        <h2>Översikt</h2>
        <p>
          Totalt antal bokningar: <strong>{totalBookings}</strong>
        </p>
        <p>
          Totalt antal pass/tillfällen: <strong>{totalOccurrences}</strong>
        </p>
      </div>

      {/* Bokningar per månad */}
      <div style={{ marginBottom: "2rem" }}>
        <h2>Bokningar per månad {selectedYear ? `(${selectedYear})` : ""}</h2>

        {/* Årsval */}
        {(() => {
          const years = Array.from(
            new Set(
              (bookings || [])
                .map((b) => getBookingDate(b))
                .filter((d) => d && !isNaN(d))
                .map((d) => d.getFullYear())
            )
          ).sort((a, b) => b - a);
          return years.length > 1 ? (
            <div style={{ margin: "0.5rem 0 1rem" }}>
              <label style={{ marginRight: 8 }}>Välj år:</label>
              <select
                value={selectedYear || ""}
                onChange={(e) => setSelectedYear(Number(e.target.value))}
              >
                {years.map((y) => (
                  <option key={y} value={y}>
                    {y}
                  </option>
                ))}
              </select>
            </div>
          ) : null;
        })()}

        <MonthlyBookingsLineChart
          months={monthsSvShort}
          bookingsPerMonth={bookingsPerMonth}
          selectedYear={selectedYear}
          palette={Tableau10}
        />
      </div>

      <ActivityDistributionPie activityOccurrences={activityOccurrences} />

      {/* Pass/tillfällen */}
      <div style={{ marginBottom: "2rem" }}>
        <h2>Pass/Tillfällen</h2>
        {activityOccurrences.length > 0 ? (
          <p>{activityOccurrences.length} tillfällen hämtade.</p>
        ) : (
          <p>Inga pass/tillfällen hittades</p>
        )}
      </div>
    </div>
  );
}
