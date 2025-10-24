import React, { useEffect, useMemo, useState } from "react";
import {
  fetchActivityOccurrences,
  fetchActivities,
  fetchZones,
  createActivityOccurrence,
  updateActivityOccurrence,
} from "../services/api";
import "../styles/AdminSchedulePage.css";

function getWeekNumber(date) {
  const d = new Date(
    Date.UTC(date.getFullYear(), date.getMonth(), date.getDate())
  );
  const dayNum = d.getUTCDay() || 7;
  d.setUTCDate(d.getUTCDate() + 4 - dayNum);
  const yearStart = new Date(Date.UTC(d.getUTCFullYear(), 0, 1));
  const weekNum = Math.ceil(((d - yearStart) / 86400000 + 1) / 7);
  return weekNum;
}

// Returns Date for Monday of ISO week/year
function getMondayOfWeek(year, week) {
  const simple = new Date(year, 0, 1 + (week - 1) * 7);
  const dow = simple.getDay();
  const monday = new Date(simple);
  if (dow <= 4) monday.setDate(simple.getDate() - simple.getDay() + 1);
  else monday.setDate(simple.getDate() + 8 - simple.getDay());
  monday.setHours(0, 0, 0, 0);
  return monday;
}
// Weekly calendar (Mon–Sun) 06:00–23:00 with hourly rows and overlays sized by exact duration.
export default function AdminSchedulePage() {
  const [occurrences, setOccurrences] = useState([]);
  const [activities, setActivities] = useState([]);
  const [zones, setZones] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Week/year navigation state
  const today = new Date();
  const currentYear = today.getFullYear();
  const [selectedYear, setSelectedYear] = useState(currentYear);
  const [selectedWeek, setSelectedWeek] = useState(getWeekNumber(today));
  // Modal state
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState(null); // occurrence or null
  const [slotInfo, setSlotInfo] = useState(null); // { date: Date, hour: number, minute: number }

  useEffect(() => {
    loadData();
  }, [selectedYear, selectedWeek]);

  async function loadData() {
    try {
      setLoading(true);
      setError(null);
      const [occ, acts, zns] = await Promise.all([
        fetchActivityOccurrences(),
        fetchActivities(),
        fetchZones(),
      ]);
      setOccurrences(Array.isArray(occ) ? occ : []);
      setActivities(Array.isArray(acts) ? acts : []);
      setZones(Array.isArray(zns) ? zns : []);
      try {
        console.debug("Schedule load:", {
          occurrences: occ?.length,
          activities: acts?.length,
          zones: zns?.length,
        });
      } catch {}
    } catch (e) {
      console.error(e);
      setError(e.message || "Kunde inte ladda data");
    } finally {
      setLoading(false);
    }
  }

  const activityMap = useMemo(() => {
    const m = {};
    for (const a of activities) m[a.id] = a;
    return m;
  }, [activities]);

  // Build current week days (Mon-Sun) based on selectedYear/selectedWeek
  const weekDays = useMemo(() => {
    // Get Monday of selected week/year
    const monday = getMondayOfWeek(selectedYear, selectedWeek);
    return [...Array(7)].map((_, i) => {
      const d = new Date(monday);
      d.setDate(monday.getDate() + i);
      return d;
    });
  }, [selectedYear, selectedWeek]);

  // Hourly grid and overlay layout
  const startHour = 6;
  const endHour = 22;
  const ROW_HEIGHT = 48; // px
  const hours = useMemo(
    () =>
      Array.from({ length: endHour - startHour + 1 }, (_, i) => startHour + i),
    []
  );

  function openCreate(date, hour, minute = 0) {
    setEditing(null);
    setSlotInfo({ date, hour, minute });
    setModalOpen(true);
  }

  function openEdit(occ) {
    setEditing(occ);
    setSlotInfo(null);
    setModalOpen(true);
  }

  function closeModal() {
    setModalOpen(false);
    setEditing(null);
    setSlotInfo(null);
  }

  async function handleSave(form) {
    try {
      setLoading(true);
      setError(null);

      if (editing) {
        const payload = {
          startTime: normalizeLocalString(form.startTime),
          endTime: normalizeLocalString(form.endTime),
          durationMinutes: form.durationMinutes,
          activityId: form.activityId,
          zoneId: form.zoneId ? Number(form.zoneId) : editing.zoneId,
          isActive: form.isActive ?? true,
        };
        await updateActivityOccurrence(editing.id, payload);
      } else {
        const payload = {
          startTime: normalizeLocalString(form.startTime),
          durationMinutes: form.durationMinutes,
          activityId: form.activityId,
        };
        if (!form.zoneId) throw new Error("Välj en zon (ZoneId krävs).");
        payload.zoneId = Number(form.zoneId);
        await createActivityOccurrence(payload);
      }

      await loadData();
      closeModal();
    } catch (e) {
      console.error(e);
      setError(e.message || "Kunde inte spara");
    } finally {
      setLoading(false);
    }
  }

  // Week/year dropdowns and navigation
  const yearOptions = Array.from({ length: 5 }, (_, i) => currentYear - 2 + i);
  const weekOptions = Array.from({ length: 52 }, (_, i) => i + 1);

  function handlePrevWeek() {
    if (selectedWeek === 1) {
      setSelectedYear((y) => y - 1);
      setSelectedWeek(52);
    } else {
      setSelectedWeek((w) => w - 1);
    }
  }
  function handleNextWeek() {
    if (selectedWeek === 52) {
      setSelectedYear((y) => y + 1);
      setSelectedWeek(1);
    } else {
      setSelectedWeek((w) => w + 1);
    }
  }

  return (
    <div className="admin-schedule">
      <h1 className="admin-schedule-title">Aktivitetsschema</h1>
      <div className="admin-schedule-search-bar">
        <select
          value={selectedYear}
          onChange={(e) => setSelectedYear(Number(e.target.value))}
        >
          {yearOptions.map((y) => (
            <option key={y} value={y}>
              {y}
            </option>
          ))}
        </select>
        <button
          onClick={handlePrevWeek}
        >
          &lt;
        </button>
        <span className="week-label">{`v${selectedWeek}`}</span>
        <button
          onClick={handleNextWeek}
        >
          &gt;
        </button>
        <select
          value={selectedWeek}
          onChange={(e) => setSelectedWeek(Number(e.target.value))}
        >
          {weekOptions.map((w) => (
            <option key={w} value={w}>{`v${w}`}</option>
          ))}
        </select>
      </div>
      {error && <div className="error-box">{error}</div>}
      {loading && <div className="loading">Laddar…</div>}
            {/* 📅 Kalendern */}
      <div className="calendar-grid" role="grid" aria-label="Veckokalender">
        {/** Håll koll på fönsterbredd för att bestämma om vi ska visa tidkolumnen */}
        {(() => {
          const [isMobile, setIsMobile] = React.useState(window.innerWidth < 768);
          React.useEffect(() => {
            const handleResize = () => setIsMobile(window.innerWidth < 768);
            window.addEventListener("resize", handleResize);
            return () => window.removeEventListener("resize", handleResize);
          }, []);

          return (
            <>
              {/* Grid header */}
              <div
                className="grid-header"
                style={{
                  gridTemplateColumns: isMobile
                    ? "repeat(7, 1fr)"
                    : "80px repeat(7, 1fr)",
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

              {/* Grid body */}
              <div
                className="grid-body"
                style={{
                  gridTemplateRows: `repeat(${hours.length}, ${ROW_HEIGHT}px)`,
                  gridTemplateColumns: isMobile
                    ? "repeat(7, 1fr)"
                    : "80px repeat(7, 1fr)",
                }}
              >
                {!isMobile &&
                  hours.map((h, rowIdx) => (
                    <div
                      key={`time-${rowIdx}`}
                      className="time-col"
                      style={{ gridColumn: 1, gridRow: rowIdx + 1 }}
                    >
                      {`${String(h).padStart(2, "0")}:00`}
                    </div>
                  ))}

                {hours.map((h, rowIdx) =>
                  weekDays.map((d, dayIdx) => (
                    <div
                      key={`cell-${dayIdx}-${rowIdx}`}
                      className="cell empty"
                      style={{
                        gridColumn: isMobile ? dayIdx + 1 : dayIdx + 2,
                        gridRow: rowIdx + 1,
                        position: "relative",
                        padding: 0,
                      }}
                    >
                      {[0, 15, 30, 45].map((m, qi) => (
                        <button
                          key={`q-${qi}`}
                          className="slot-quarter"
                          title={`Lägg till ${String(h).padStart(2, "0")}:${String(
                            m
                          ).padStart(2, "0")}`}
                          onClick={() => openCreate(d, h, m)}
                          style={{
                            position: "absolute",
                            left: 0,
                            width: "100%",
                            height: "25%",
                            top: `${qi * 25}%`,
                            background: "transparent",
                            border: 0,
                            cursor: "pointer",
                            padding: 0,
                            margin: 0,
                          }}
                        />
                      ))}
                    </div>
                  ))
                )}

                {/* Overlay-block (aktiviteter) */}
                {weekDays.map((d, dayIdx) => {
                  const dY = d.getFullYear();
                  const dM = d.getMonth();
                  const dD = d.getDate();
                  const dayOccs = occurrences.filter((o) => {
                    const st = new Date(o.startTime);
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
                        zIndex: 5,
                        pointerEvents: "none",
                      }}
                    >
                      {dayOccs.map((o) => {
                        const st = new Date(o.startTime);
                        const en = o.endTime
                          ? new Date(o.endTime)
                          : new Date(
                              st.getTime() +
                                (Number(o.durationMinutes) || 60) * 60000
                            );

                        const visStart = new Date(d);
                        visStart.setHours(startHour, 0, 0, 0);
                        const minutesFromStart =
                          (st.getTime() - visStart.getTime()) / 60000;
                        let durationMin = (en.getTime() - st.getTime()) / 60000;
                        if (!isFinite(durationMin) || durationMin <= 0)
                          durationMin = Number(o.durationMinutes) || 60;
                        const top = (minutesFromStart / 60) * ROW_HEIGHT;
                        const height = (durationMin / 60) * ROW_HEIGHT;
                        const startLabel = `${String(st.getHours()).padStart(
                          2,
                          "0"
                        )}:${String(st.getMinutes()).padStart(2, "0")}`;
                        const endLabel = `${String(en.getHours()).padStart(
                          2,
                          "0"
                        )}:${String(en.getMinutes()).padStart(2, "0")}`;

                        return (
                          <button
                            key={`occ-${o.id}`}
                            className="occurrence overlay show"
                            style={{
                              position: "absolute",
                              top: `${top}px`,
                              left: 0,
                              right: 0,
                              height: `${height}px`,
                              zIndex: 10,
                              pointerEvents: "auto",
                              opacity: 1,
                              fontSize: "12px",
                              padding: "2px 4px",
                              boxSizing: "border-box",
                            }}
                            onClick={() => openEdit(o)}
                            title={`${startLabel}–${endLabel} ${
                              activityMap[o.activityId]?.name ||
                              o.activityName ||
                              "Aktivitet"
                            }`}
                          >
                            <span
                              style={{
                                pointerEvents: "none",
                                overflow: "hidden",
                                textOverflow: "ellipsis",
                                whiteSpace: "nowrap",
                              }}
                            >
                              {o.activityName ||
                                activityMap[o.activityId]?.name ||
                                ""}
                            </span>
                            <span
                              style={{
                                position: "absolute",
                                top: "2px",
                                right: "6px",
                                fontSize: "9px",
                                color: "#065f46",
                                opacity: 0.9,
                                pointerEvents: "none",
                              }}
                            >
                              {startLabel}–{endLabel}
                            </span>
                          </button>
                        );
                      })}
                    </div>
                  );
                })}
              </div>
            </>
          );
        })()}
      </div>


      {modalOpen && (
        <OccurrenceModal
          onClose={closeModal}
          onSave={handleSave}
          editing={editing}
          slotInfo={slotInfo}
          activities={activities}
          occurrences={occurrences}
          zones={zones}
        />
      )}
    </div>
  );
}

function OccurrenceModal({
  onClose,
  onSave,
  editing,
  slotInfo,
  activities,
  occurrences,
  zones,
}) {
  const [activityId, setActivityId] = useState(
    editing?.activityId || activities[0]?.id || ""
  );
  const [dateStr, setDateStr] = useState(() => {
    const d = editing
      ? new Date(editing.startTime)
      : new Date(slotInfo?.date || new Date());
    return toLocalDateString(d);
  });
  const [timeStr, setTimeStr] = useState(() => {
    const d = editing
      ? new Date(editing.startTime)
      : new Date(slotInfo?.date || new Date());
    const h = editing ? d.getHours() : slotInfo?.hour ?? 8;
    const m = editing ? d.getMinutes() : slotInfo?.minute ?? 0;
    return `${String(h).padStart(2, "0")}.${String(m).padStart(2, "0")}`;
  });
  const [durationMinutes, setDurationMinutes] = useState(
    editing?.durationMinutes || 60
  );
  const computedZoneId = useMemo(() => {
    const act = activities.find((a) => a.id === Number(activityId));
    return act?.zoneId ?? editing?.zoneId ?? null;
  }, [activityId, activities, editing]);
  const [isActive, setIsActive] = useState(editing?.isActive ?? true);

  useEffect(() => {
    if (!editing && !activityId && activities && activities.length > 0) {
      setActivityId(activities[0].id);
    }
  }, [activities]);

  function handleSubmit(e) {
    e.preventDefault();
    if (!activityId || !dateStr || !timeStr || !durationMinutes) return;
    if (computedZoneId == null || Number(computedZoneId) <= 0) {
      alert("Kunde inte avgöra zon för vald aktivitet.");
      return;
    }
    const startLocal = `${dateStr}T${timeStr.replace(".", ":")}`;
    const start = fromLocalInput(startLocal);
    const end = new Date(start.getTime() + Number(durationMinutes) * 60000);
    onSave({
      activityId: Number(activityId),
      startTime: startLocal,
      durationMinutes: Number(durationMinutes),
      zoneId: Number(computedZoneId),
      isActive,
      endTime: toLocalInput(end),
    });
  }

  return (
    <div className="modal-backdrop" role="dialog" aria-modal="true">
      <div className="modal">
        <div className="modal-header">
          <h2>
            {editing
              ? "Redigera aktivitetstillfälle"
              : "Nytt aktivitetstillfälle"}
          </h2>
          <button className="close-btn" onClick={onClose} aria-label="Stäng">
            ×
          </button>
        </div>
        <form onSubmit={handleSubmit} className="modal-body">
          <label>
            Aktivitet
            <select
              value={activityId}
              onChange={(e) => setActivityId(e.target.value)}
              required
            >
              {activities.map((a) => (
                <option key={a.id} value={a.id}>
                  {a.name}
                </option>
              ))}
            </select>
          </label>
          <fieldset className="inline-fields">
            <legend>Starttid (15 min-intervall)</legend>
            <label>
              Datum
              <input
                type="date"
                value={dateStr}
                onChange={(e) => setDateStr(e.target.value)}
                required
              />
            </label>
            <label>
              Tid (lediga)
              <select
                value={timeStr}
                onChange={(e) => setTimeStr(e.target.value)}
              >
                {getAvailableTimes({
                  dateStr,
                  zoneId: computedZoneId,
                  durationMinutes,
                  occurrences,
                  editingId: editing?.id,
                }).map((t) => (
                  <option key={t} value={t}>
                    {t}
                  </option>
                ))}
              </select>
            </label>
          </fieldset>
          <div className="duration-choices">
            <div className="label">Aktivitetslängd</div>
            <div className="choices">
              {[30, 45, 60, 75, 90, 105, 120].map((min) => (
                <span
                  key={min}
                  className={`choice ${
                    Number(durationMinutes) === min ? "selected" : ""
                  }`}
                  onClick={() => setDurationMinutes(min)}
                  role="button"
                  tabIndex={0}
                >
                  {min} min
                </span>
              ))}
            </div>
          </div>
          {editing && (
            <label
              className="checkbox"
              style={{ display: "flex", alignItems: "center", gap: "6px" }}
            >
              <span>Aktiv</span>
              <input
                type="checkbox"
                checked={isActive}
                onChange={(e) => setIsActive(e.target.checked)}
              />
            </label>
          )}
          <div className="modal-footer">
            <button type="button" className="btn-secondary" onClick={onClose}>
              Avbryt
            </button>
            <button
              type="submit"
              className="btn-primary"
              disabled={!activityId || !dateStr || !timeStr || !durationMinutes}
            >
              Spara
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

function toLocalInput(d) {
  const pad = (n) => String(n).padStart(2, "0");
  const year = d.getFullYear();
  const month = pad(d.getMonth() + 1);
  const day = pad(d.getDate());
  const hours = pad(d.getHours());
  const minutes = pad(d.getMinutes());
  return `${year}-${month}-${day}T${hours}:${minutes}`;
}

function toLocalDateString(d) {
  const pad = (n) => String(n).padStart(2, "0");
  const year = d.getFullYear();
  const month = pad(d.getMonth() + 1);
  const day = pad(d.getDate());
  return `${year}-${month}-${day}`;
}

// Build available 15-min time strings (e.g., '13.00', '13.15') excluding overlaps for same zone/day
function getAvailableTimes({
  dateStr,
  zoneId,
  durationMinutes,
  occurrences,
  editingId,
}) {
  if (!dateStr || !zoneId || !durationMinutes) return [];
  const startOfDay = new Date(`${dateStr}T00:00`);
  const endOfDay = new Date(`${dateStr}T23:59`);

  const slots = [];
  const startHour = 6;
  const endHour = 22;
  for (let h = startHour; h <= endHour; h++) {
    for (let m = 0; m < 60; m += 15) {
      const s = new Date(startOfDay);
      s.setHours(h, m, 0, 0);
      const e = new Date(s.getTime() + Number(durationMinutes) * 60000);
      if (e > endOfDay) continue;
      slots.push({
        s,
        e,
        label: `${String(h).padStart(2, "0")}.${String(m).padStart(2, "0")}`,
      });
    }
  }

  const sameDayOccs = (occurrences || []).filter((o) => {
    if (!o) return false;
    if (o.zoneId !== Number(zoneId)) return false;
    if (editingId && o.id === editingId) return false;
    if (o.isCancelled) return false;
    if (o.isActive === false) return false;
    const st = new Date(o.startTime);
    return (
      st.getFullYear() === startOfDay.getFullYear() &&
      st.getMonth() === startOfDay.getMonth() &&
      st.getDate() === startOfDay.getDate()
    );
  });

  return slots
    .filter((slot) => {
      return !sameDayOccs.some((o) => {
        const oS = new Date(o.startTime);
        const oE = new Date(o.endTime);
        return slot.s < oE && slot.e > oS;
      });
    })
    .map((x) => x.label);
}

function fromLocalInput(v) {
  const [date, time] = v.split("T");
  const [y, m, d] = date.split("-").map(Number);
  const [hh, mm] = time.split(":").map(Number);
  const dt = new Date();
  dt.setFullYear(y);
  dt.setMonth(m - 1);
  dt.setDate(d);
  dt.setHours(hh, mm, 0, 0);
  return dt;
}

function normalizeLocalString(s) {
  if (!s) return s;
  if (s.length === 16) return `${s}:00`;
  return s;
}
