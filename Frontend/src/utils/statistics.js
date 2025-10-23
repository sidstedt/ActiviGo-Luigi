// Shared helpers for statistics

export const Tableau10 = [
  "#4e79a7",
  "#f28e2c",
  "#e15759",
  "#76b7b2",
  "#59a14f",
  "#edc949",
];

export function hexToRgba(hex, alpha) {
  try {
    const r = parseInt(hex.slice(1, 3), 16);
    const g = parseInt(hex.slice(3, 5), 16);
    const b = parseInt(hex.slice(5, 7), 16);
    return `rgba(${r}, ${g}, ${b}, ${alpha})`;
  } catch {
    return hex;
  }
}

// Bookings helpers
export function getBookingDate(booking) {
  const candidates = [
    booking?.date,
    booking?.bookingDate,
    booking?.startTime,
    booking?.start,
    booking?.createdAt,
    booking?.booking?.date,
    booking?.occurrence?.startTime,
    booking?.occurrenceStartTime,
  ];
  const iso = candidates.find(Boolean);
  const d = iso ? new Date(iso) : null;
  return d && !isNaN(d) ? d : null;
}

export function calculateBookingsPerMonth(bookings, yearFilter = null) {
  const bookingsByMonth = Array(12).fill(0);
  bookings.forEach((booking) => {
    const d = getBookingDate(booking);
    if (!d) return;
    if (yearFilter && d.getFullYear() !== yearFilter) return;
    bookingsByMonth[d.getMonth()]++;
  });
  return bookingsByMonth;
}

// Occurrence helpers
export function getOccurrenceStart(occ) {
  const candidates = [occ?.startTime, occ?.start, occ?.at, occ?.begin];
  const iso = candidates.find(Boolean);
  const d = iso ? new Date(iso) : null;
  return d && !isNaN(d) ? d : null;
}
export function getOccurrenceEnd(occ) {
  const candidates = [occ?.endTime, occ?.end, occ?.finish];
  const iso = candidates.find(Boolean);
  const d = iso ? new Date(iso) : null;
  return d && !isNaN(d) ? d : null;
}
export function isOccurrenceCompleted(occ) {
  const now = new Date();
  const end = getOccurrenceEnd(occ);
  const start = getOccurrenceStart(occ);
  if (end) return end <= now;
  if (start) return start <= now;
  return false;
}
export function getActivityName(occ) {
  return (
    occ?.activity?.name ||
    occ?.activityName ||
    (occ?.activityId != null
      ? `Aktivitet ${occ.activityId}`
      : "Okänd aktivitet")
  );
}

export const monthsSvShort = Array.from({ length: 12 }, (_, i) =>
  new Date(2000, i).toLocaleString("sv-SE", { month: "short" })
);
