import { useEffect, useMemo, useState } from "react";
import {
  fetchAdminBookings,
  fetchUserBookings,
  fetchActivityOccurrences,
  getCurrentUser,
} from "../services/api";
import {
  calculateBookingsPerMonth,
  getBookingDate,
  monthsSvShort,
} from "../utils/statistics";

export function useStatisticsData() {
  const [bookings, setBookings] = useState([]);
  const [activityOccurrences, setActivityOccurrences] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedYear, setSelectedYear] = useState(null);
  const [bookingsPerMonth, setBookingsPerMonth] = useState(Array(12).fill(0));

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
        if (isAdmin) {
          const [bookingsData, occurrencesData] = await Promise.all([
            fetchAdminBookings(),
            fetchActivityOccurrences(),
          ]);
          setBookings(bookingsData || []);
          setActivityOccurrences(occurrencesData || []);
        } else {
          const [bookingsData, occurrencesData] = await Promise.all([
            fetchUserBookings(),
            fetchActivityOccurrences(),
          ]);
          setBookings(bookingsData || []);
          setActivityOccurrences(occurrencesData || []);
        }
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

  const months = monthsSvShort;

  return {
    bookings,
    activityOccurrences,
    loading,
    error,
    selectedYear,
    setSelectedYear,
    bookingsPerMonth,
    months,
  };
}
