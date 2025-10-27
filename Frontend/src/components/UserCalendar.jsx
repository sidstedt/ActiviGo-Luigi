import React, { useEffect, useMemo, useState } from "react";
import { getCurrentUser, fetchUserBookings } from "../services/api";
import "../styles/UserCalendar.css";

function getWeekNumber(date) {
  const d = new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate()));
  const dayNum = d.getUTCDay() || 7;
  d.setUTCDate(d.getUTCDate() + 4 - dayNum);
  const yearStart = new Date(Date.UTC(d.getUTCFullYear(), 0, 1));
  return Math.ceil(((d - yearStart) / 86400000 + 1) / 7);
}
function getMondayOfWeek(year, week) {
  const simple = new Date(year, 0, 1 + (week - 1) * 7);
  const dow = simple.getDay();
  const monday = new Date(simple);
  if (dow <= 4) monday.setDate(simple.getDate() - simple.getDay() + 1);
  else monday.setDate(simple.getDate() + 8 - simple.getDay());
  monday.setHours(0, 0, 0, 0);
  return monday;
}

function getStartEnd(booking) {
  const s = booking.startTime || booking.start || booking.from || booking.startsAt;
  const e = booking.endTime || booking.end || booking.to || booking.endsAt;

  const st = s ? new Date(s) : null;
  const en = e ? new Date(e) : st ? new Date(st.getTime() + 60 * 60000) : null;

  return { st, en };
}

export default function UserWeekCalendar() {
  const [bookings, setBookings] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const today = new Date();
  const currentYear = today.getFullYear();
  const [selectedYear, setSelectedYear] = useState(currentYear);
  const [selectedWeek, setSelectedWeek] = useState(getWeekNumber(today));

  const [isMobile, setIsMobile] = useState(window.innerWidth < 768);
  useEffect(() => {
    const handleResize = () => setIsMobile(window.innerWidth < 768);
    window.addEventListener("resize", handleResize);
    return () => window.removeEventListener("resize", handleResize);
  }, []);

useEffect(() => {
  async function load() {
    try {
      setLoading(true);
      setError(null);

      const user = getCurrentUser();
      if (!user) {
        setBookings([]);
        return;
      }

      const all = await fetchUserBookings();

      const monday = getMondayOfWeek(selectedYear, selectedWeek);
      const weekStart = new Date(monday);
      const weekEnd = new Date(monday);
      weekEnd.setDate(monday.getDate() + 7);
      const inWeek = (Array.isArray(all) ? all : []).filter((b) => {
        const { st } = getStartEnd(b);
        if (!st) return false;
        const local = new Date(st.getFullYear(), st.getMonth(), st.getDate(), st.getHours(), st.getMinutes());
        return local >= weekStart && local < weekEnd;
      });

      setBookings(inWeek);
    } catch (e) {
      setError(e?.message || "Kunde inte hämta bokningar");
    } finally {
      setLoading(false);
    }
  }

  load();
}, [selectedYear, selectedWeek]);


  const startHour = 6;
  const endHour = 22;
  const ROW_HEIGHT = 48;
  const hours = useMemo(
    () => Array.from({ length: endHour - startHour + 1 }, (_, i) => startHour + i),
    []
  );

  const weekDays = useMemo(() => {
    const monday = getMondayOfWeek(selectedYear, selectedWeek);
    return [...Array(7)].map((_, i) => {
      const d = new Date(monday);
      d.setDate(monday.getDate() + i);
      return d;
    });
  }, [selectedYear, selectedWeek]);

  const handlePrevWeek = () => setSelectedWeek(w => (w > 1 ? w - 1 : 52));
  const handleNextWeek = () => setSelectedWeek(w => (w < 52 ? w + 1 : 1));

  return (
    <div className="user-schedule">
      <div className="user-schedule-toolbar">
        <h2>Mina bokningar</h2>
        <div className="week-nav">
          <button onClick={handlePrevWeek}>&lt;</button>
          <span>{`v${selectedWeek}`}</span>
          <button onClick={handleNextWeek}>&gt;</button>
        </div>
      </div>

      {error && <div className="error">{error}</div>}
      {loading && <div className="loading">Laddar…</div>}

      {!loading && (
        <div className="calendar-grid">
          <div
            className="grid-header"
            style={{
              gridTemplateColumns: isMobile ? "repeat(7, 1fr)" : "80px repeat(7, 1fr)",
            }}
          >
            {!isMobile && <div className="time-col" />}
            {weekDays.map((d, i) => (
              <div key={i} className="day-col">
                {d.toLocaleDateString("sv-SE", {
                  weekday: "short",
                  day: "2-digit",
                  month: "2-digit",
                })}
              </div>
            ))}
          </div>

          <div
            className="grid-body"
            style={{
              gridTemplateRows: `repeat(${hours.length}, ${ROW_HEIGHT}px)`,
              gridTemplateColumns: isMobile ? "repeat(7, 1fr)" : "80px repeat(7, 1fr)",
            }}
          >
            {!isMobile &&
              hours.map((h, i) => (
                <div
                  key={`t-${i}`}
                  className="time-col"
                  style={{ gridColumn: 1, gridRow: i + 1 }}
                >
                  {`${String(h).padStart(2, "0")}:00`}
                </div>
              ))}

            {hours.map((_, rowIdx) =>
              weekDays.map((_, dayIdx) => (
                <div
                  key={`cell-${dayIdx}-${rowIdx}`}
                  className="cell"
                  style={{
                    gridColumn: isMobile ? dayIdx + 1 : dayIdx + 2,
                    gridRow: rowIdx + 1,
                  }}
                />
              ))
            )}

            {weekDays.map((d, dayIdx) => {
              const dY = d.getFullYear();
              const dM = d.getMonth();
              const dD = d.getDate();
              const dayBookings = bookings.filter((b) => {
                const st = new Date(b.startTime || b.start);
                return (
                  st.getFullYear() === dY &&
                  st.getMonth() === dM &&
                  st.getDate() === dD
                );
              });

              return (
                <div
                  key={`overlay-${dayIdx}`}
                  className="day-overlay"
                  style={{
                    gridColumn: isMobile ? dayIdx + 1 : dayIdx + 2,
                    gridRow: `1 / ${hours.length + 1}`,
                    position: "relative",
                    overflow: "hidden",
                  }}
                >
                  {dayBookings.map((b, i) => {
                    const st = new Date(b.startTime || b.start);
                    const en = new Date(b.endTime || b.end || st.getTime() + 60 * 60000);
                    const visStart = new Date(d);
                    visStart.setHours(startHour, 0, 0, 0);
                    const minutesFromStart =
                      (st.getTime() - visStart.getTime()) / 60000;
                    const durationMin = (en.getTime() - st.getTime()) / 60000;
                    const top = (minutesFromStart / 60) * ROW_HEIGHT;
                    const height = (durationMin / 60) * ROW_HEIGHT;
                    const startLabel = `${String(st.getHours()).padStart(2, "0")}:${String(
                      st.getMinutes()
                    ).padStart(2, "0")}`;
                    const endLabel = `${String(en.getHours()).padStart(2, "0")}:${String(
                      en.getMinutes()
                    ).padStart(2, "0")}`;

                    return (
                      <div
                        key={`b-${i}`}
                        className="booking overlay"
                        style={{
                          position: "absolute",
                          top: `${top}px`,
                          left: 0,
                          right: 0,
                          height: `${height}px`,
                        }}
                        title={`${startLabel}–${endLabel} ${
                          b.activityName || "Aktivitet"
                        }`}
                      >
                        <span className="booking-title">
                          {b.activityName || "Aktivitet"}
                        </span>
                        <span className="booking-time">
                          {startLabel}–{endLabel}
                        </span>
                      </div>
                    );
                  })}
                </div>
              );
            })}
          </div>
        </div>
      )}

      {!loading && bookings.length === 0 && (
        <div className="empty">Inga bokningar denna vecka.</div>
      )}
    </div>
  );
}
