import Modal from "./Modal";
import { getCurrentUser } from "../services/api";
import { formatDateShort, formatTimeHm } from "../utils/format";

export default function BookingConfirmModal({
  isOpen,
  occurrence,
  activity,
  onClose,
  onConfirm,
  loading,
  error,
}) {
  if (!isOpen || !occurrence) return null;
  const user = getCurrentUser();
  const dateStr = (() => {
    const dt = new Date(occurrence.startTime);
    return `${formatDateShort(dt)}.${dt.getFullYear()}`;
  })();
  const timeStr = `${formatTimeHm(occurrence.startTime)} – ${formatTimeHm(occurrence.endTime)}`;

  return (
    <Modal title="Bekräfta bokning" onClose={() => { if (!loading) onClose(); }}>
      <div>
        {activity && (
          <div style={{ marginBottom: 8 }}>
            <strong>{activity.name}</strong>
            <div style={{ color: '#555' }}>{dateStr} kl {timeStr}</div>
          </div>
        )}
        <div style={{ marginBottom: 8 }}>
          <div><strong>Namn:</strong> {user?.username || 'Okänd'}</div>
          <div><strong>E-post:</strong> {user?.email || 'Okänd'}</div>
        </div>
        {error && (
          <div style={{ color: '#b00020', marginBottom: 8 }}>{error}</div>
        )}
        <div style={{ display: 'flex', gap: 8, justifyContent: 'flex-end' }}>
          <button className="secondary-button" onClick={() => { if (!loading) onClose(); }} disabled={loading}>Avbryt</button>
          <button className={`book-button ${loading ? 'disabled' : ''}`} onClick={onConfirm} disabled={loading}>
            {loading ? 'Bokar…' : 'Bekräfta'}
          </button>
        </div>
      </div>
    </Modal>
  );
}
