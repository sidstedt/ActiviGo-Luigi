import React, { useEffect, useMemo, useState } from "react";
import { getCurrentUser, fetchUserBookings } from "../services/api.js";
import "../styles/UserCalendar.css";

/** ISO-vecka för ett datum */
function getWeekNumber(date) {
  const d = new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate()));
  const dayNum = d.getUTCDay() || 7; // 1..7 (mån..sön)
  d.setUTCDate(d.getUTCDate() + 4 - dayNum);
  const yearStart = new Date(Date.UTC(d.getUTCFullYear(), 0, 1));
  return Math.ceil(((d - yearStart) / 86400000 + 1) / 7);
}

/** Antal ISO-veckor i ett år (52 eller 53) */
function getWeeksInYear(year) {
  const d = new Date(Date.UTC(year, 0, 1));
  const day = d.getUTCDay() || 7;
  const isLeap = (year % 4 === 0 && year % 100 !== 0) || (year % 400 === 0);
  return day === 4 || (isLeap && day === 3) ? 53 : 52;
}

/** Måndag för given ISO-vecka/år (lokal tid 00:00) */
function getMondayOfWeek(year, week) {
  const simple = new Date(year, 0, 1 + (week - 1) * 7);
  const dow = simple.getDay(); // 0..6 (sön..lör)
  const monday = new Date(simple);
  if (dow <= 4) monday.setDate(simple.getDate() - simple.getDay() + 1);
  else monday.setDate(simple.getDate() + 8 - simple.getDay());
  monday.setHours(0, 0, 0, 0);
  return monday;
}

/** Start/Slut från olika möjliga fältnamn */
function getStartEnd(booking) {
  const s = booking.startTime || booking.start || booking.from || booking.startsAt;
  const e = booking.endTime || booking.end || booking.to || booking.endsAt;
  const st = s ? new Date(s) : null;
  const en = e ? new Date(e) : st ? new Date(st.getTime() + 60 * 60000) : null;
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
    [startHour, endHour]
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

        // Filtrera bokningar som startar under veckan
        const inWeek = (Array.isArray(all) ? all : []).filter((b) => {
          const { st } = getStartEnd(b);
          return st && st >= weekStart && st < weekEnd;
        });

        setBookings(inWeek);
      } catch (e) {
        console.error(e);
        setError(e?.message || "Kunde inte hämta bokningar");
      } finally {
        setLoading(false);
      }
    }

    load();
  }, [selectedYear, selectedWeek]);

  function handlePrevWeek() {
    if (selectedWeek === 1) {
      const prevYear = selectedYear - 1;
      setSelectedYear(prevYear);
      setSelectedWeek(getWeeksInYear(prevYear));
    } else {
      setSelectedWeek((w) => w - 1);
    }
  }
  function handleNextWeek() {
    const maxWeek = getWeeksInYear(selectedYear);
    if (selectedWeek === maxWeek) {
      setSelectedYear((y) => y + 1);
      setSelectedWeek(1);
    } else {
      setSelectedWeek((w) => w + 1);
    }
  }
  function handleToday() {
    const t = new Date();
    setSelectedYear(t.getFullYear());
    setSelectedWeek(getWeekNumber(t));
  }

  const isSameDay = (a, b) =>
    a.getFullYear() === b.getFullYear() &&
    a.getMonth() === b.getMonth() &&
    a.getDate() === b.getDate();

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

      {error && (
        <div className="uwc-error" role="status" aria-live="polite">
          {error}
        </div>
      )}
      {loading && (
        <div className="uwc-loading" role="status" aria-live="polite">
          Laddar…
        </div>
      )}

      <div className="uwc-grid" role="grid" aria-label="Veckokalender">
        {/* Header-rad */}
        <div className="uwc-grid-header" role="row">
          <div className="uwc-time-col" role="columnheader" aria-hidden="true" />
          {weekDays.map((d, i) => (
            <div
              key={i}
              className={`uwc-day-col ${isSameDay(d, today) ? "is-today" : ""}`}
              role="columnheader"
              aria-current={isSameDay(d, today) ? "date" : undefined}
            >
              {d.toLocaleDateString("sv-SE", {
                weekday: "short",
                day: "2-digit",
                month: "2-digit",
              })}
            </div>
          ))}
        </div>

        {/* Kroppen (timmar + celler + overlay) */}
        <div
          className="uwc-grid-body"
          style={{
            gridTemplateRows: `repeat(${hours.length}, ${ROW_HEIGHT}px)`,
            // Kolumnlayouten ligger i CSS: grid-template-columns: 80px repeat(7, 1fr);
          }}
        >
          {/* Tid- och cells-rader */}
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

          {/* Overlay per dag */}
          {weekDays.map((d, dayIdx) => {
            const dY = d.getFullYear();
            const dM = d.getMonth();
            const dD = d.getDate();

            // Robust dagsfilter via intervall
            const dayStart = new Date(dY, dM, dD, 0, 0, 0, 0);
            const dayEnd = new Date(dY, dM, dD + 1, 0, 0, 0, 0);

            const dayBookings = bookings.filter((b) => {
              const { st } = getStartEnd(b);
              return st && st >= dayStart && st < dayEnd;
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

                  // Visningsfönster för dagen
                  const visStart = new Date(d);
                  visStart.setHours(startHour, 0, 0, 0);
                  const visEnd = new Date(d);
                  visEnd.setHours(endHour, 0, 0, 0);

                  // Klipp till visningsfönstret
                  const clampedStart = st < visStart ? visStart : st;
                  const clampedEnd = end > visEnd ? visEnd : end;

                  // Helt utanför fönstret?
                  if (clampedEnd <= visStart || clampedStart >= visEnd) return null;

                  const minutesFromStart =
                    (clampedStart.getTime() - visStart.getTime()) / 60000;
                  let durationMin =
                    (clampedEnd.getTime() - clampedStart.getTime()) / 60000;
                  if (!isFinite(durationMin) || durationMin <= 0) durationMin = 60;

                  const top = (minutesFromStart / 60) * ROW_HEIGHT;
                  const height = Math.max(2, (durationMin / 60) * ROW_HEIGHT); // min-höjd

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
          <div className="uwc-empty" role="status" aria-live="polite">
            Inga bokningar denna vecka.
          </div>
        )}
      </div>
    </div>
  );
}
