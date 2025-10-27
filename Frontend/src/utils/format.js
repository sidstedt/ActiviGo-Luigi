// Date/time formatting helpers (Swedish locale)

export function formatDateShort(dateString) {
  const d = new Date(dateString);
  return d.toLocaleDateString('sv-SE', { day: '2-digit', month: '2-digit' });
}

export function formatTimeHm(dateString) {
  const d = new Date(dateString);
  return d.toLocaleTimeString('sv-SE', { hour: '2-digit', minute: '2-digit', hour12: false, hourCycle: 'h23' });
}

export function formatDateLong(dateString) {
  const d = new Date(dateString);
  return d.toLocaleDateString('sv-SE', {
    weekday: 'long',
    year: 'numeric',
    month: 'long',
    day: 'numeric',
  });
}
