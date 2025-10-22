import React, { useEffect, useMemo, useState } from "react";
import { getCurrentUser, fetchUserBookings } from "../services/api.js";
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
  const en = e ? new Date(e) : (st ? new Date(st.getTime() + 60 * 60000) : null);
  return { st, en };
}
function getActivityName(booking) {
  return (
    booking.activityName ||
    booking.activity?.name ||
    booking.name ||
    booking.title ||
    "Aktivitet"
  );
}
function getZoneName(booking) {
  return booking.zoneName || booking.zone?.name || booking.court?.name || null;
}

export default function UserWeekCalendar() {
  const [bookings, setBookings] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const today = new Date();
  const currentYear = today.getFullYear();
  const [selectedYear, setSelectedYear] = useState(currentYear);
  const [selectedWeek, setSelectedWeek] = useState(getWeekNumber(today));

  const weekDays = useMemo(() => {
    const monday = getMondayOfWeek(selectedYear, selectedWeek);
    return [...Array(7)].map((_, i) => {
      const d = new Date(monday);
      d.setDate(monday.getDate() + i);
      return d;
    });
  }, [selectedYear, selectedWeek]);

  const startHour = 6;
  const endHour = 22;
  const ROW_HEIGHT = 48;
  const hours = useMemo(
    () => Array.from({ length: endHour - startHour + 1 }, (_, i) => startHour + i),
    []
  );


  useEffect(() => {
    const user = getCurrentUser();
    if (!user) {
      setBookings([]);
      return;
    }

    async function load() {
      try {
        setLoading(true);
        setError(null);
        const all = await fetchUserBookings(); 
        const monday = getMondayOfWeek(selectedYear, selectedWeek);
        const weekStart = new Date(monday);
        const weekEnd = new Date(monday);
        weekEnd.setDate(monday.getDate() + 7); 

        const inWeek = (Array.isArray(all) ? all : []).filter((b) => {
          const { st } = getStartEnd(b);
          return st && st >= weekStart && st < weekEnd;
        });

        setBookings(inWeek);
      } catch (e) {
        console.error(e);
        setError(e.message || "Kunde inte hämta bokningar");
      } finally {
        setLoading(false);
      }
    }

    load();
  }, [selectedYear, selectedWeek]);


  function handlePrevWeek() {
    if (selectedWeek === 1) {
      setSelectedYear((y) => y - 1);
      setSelectedWeek(52);
    } else setSelectedWeek((w) => w - 1);
  }
  function handleNextWeek() {
    if (selectedWeek === 52) {
      setSelectedYear((y) => y + 1);
      setSelectedWeek(1);
    } else setSelectedWeek((w) => w + 1);
  }
  function handleToday() {
    const t = new Date();
    setSelectedYear(t.getFullYear());
    setSelectedWeek(getWeekNumber(t));
  }

  return (
    <div className="user-week-calendar">
      <div className="uwc-toolbar">
        <h2 className="uwc-title">Mina aktiviteter</h2>
        <div className="uwc-nav">
          <button onClick={handlePrevWeek} aria-label="Föregående vecka">
            &lt;
          </button>
          <span className="uwc-week">{`v${selectedWeek} ${selectedYear}`}</span>
          <button onClick={handleNextWeek} aria-label="Nästa vecka">
            &gt;
          </button>
          <button className="uwc-today" onClick={handleToday}>
            Idag
          </button>
        </div>
      </div>

      {error && <div className="uwc-error">{error}</div>}
      {loading && <div className="uwc-loading">Laddar…</div>}

      <div className="uwc-grid" role="grid" aria-label="Veckokalender">
        <div className="uwc-grid-header">
          <div className="uwc-time-col" />
          {weekDays.map((d, i) => (
            <div key={i} className="uwc-day-col">
              {d.toLocaleDateString("sv-SE", {
                weekday: "short",
                day: "2-digit",
                month: "2-digit",
              })}
            </div>
          ))}
        </div>

        <div
          className="uwc-grid-body"
          style={{
            gridTemplateRows: `repeat(${hours.length}, ${ROW_HEIGHT}px)`,
            gridTemplateColumns: `80px repeat(7, 1fr)`,
          }}
        >
          {hours.map((h, rowIdx) => (
            <React.Fragment key={`row-${rowIdx}`}>
              <div
                className="uwc-time-col"
                style={{ gridColumn: 1, gridRow: rowIdx + 1 }}
              >
                {`${String(h).padStart(2, "0")}:00`}
              </div>
              {weekDays.map((_, dayIdx) => (
                <div
                  key={`cell-${dayIdx}-${rowIdx}`}
                  className="uwc-cell"
                  style={{ gridColumn: dayIdx + 2, gridRow: rowIdx + 1 }}
                />
              ))}
            </React.Fragment>
          ))}

          {weekDays.map((d, dayIdx) => {
            const dY = d.getFullYear();
            const dM = d.getMonth();
            const dD = d.getDate();

            const dayBookings = bookings.filter((b) => {
              const { st } = getStartEnd(b);
              return (
                st &&
                st.getFullYear() === dY &&
                st.getMonth() === dM &&
                st.getDate() === dD
              );
            });

            return (
              <div
                key={`overlay-${dayIdx}`}
                className="uwc-day-overlay"
                style={{
                  gridColumn: dayIdx + 2,
                  gridRow: `1 / ${hours.length + 1}`,
                }}
              >
                {dayBookings.map((b) => {
                  const { st, en } = getStartEnd(b);
                  if (!st) return null;
                  const end = en || new Date(st.getTime() + 60 * 60000);

                  const visStart = new Date(d);
                  visStart.setHours(startHour, 0, 0, 0);

                  const minutesFromStart = (st.getTime() - visStart.getTime()) / 60000;
                  let durationMin = (end.getTime() - st.getTime()) / 60000;
                  if (!isFinite(durationMin) || durationMin <= 0) durationMin = 60;

                  const top = (minutesFromStart / 60) * ROW_HEIGHT;
                  const height = (durationMin / 60) * ROW_HEIGHT;

                  const startLabel = `${String(st.getHours()).padStart(2, "0")}:${String(
                    st.getMinutes()
                  ).padStart(2, "0")}`;
                  const endLabel = `${String(end.getHours()).padStart(2, "0")}:${String(
                    end.getMinutes()
                  ).padStart(2, "0")}`;

                  return (
                    <div
                      key={`b-${b.id ?? `${st.toISOString()}-${getActivityName(b)}`}`}
                      className="uwc-overlay-card"
                      style={{ top: `${top}px`, height: `${height}px` }}
                      title={`${startLabel}–${endLabel} ${getActivityName(b)}`}
                    >
                      <div className="uwc-overlay-title">{getActivityName(b)}</div>
                      <div className="uwc-overlay-sub">
                        {startLabel}–{endLabel}
                        {getZoneName(b) ? ` · ${getZoneName(b)}` : ""}
                      </div>
                    </div>
                  );
                })}
              </div>
            );
          })}
        </div>

        {!loading && bookings.length === 0 && (
          <div className="uwc-empty">Inga bokningar denna vecka.</div>
        )}
      </div>
    </div>
  );
}
