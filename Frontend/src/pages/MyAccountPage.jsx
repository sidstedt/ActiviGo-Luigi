import { useEffect, useState, useMemo } from "react";
// Importera fetchUserBookings från din befintliga API-tjänst
import { fetchUserBookings } from "../services/api"; 
import "../styles/MyAccountPage.css"; 
import Modal from "../components/Modal";
import { changePassword } from "../services/api";

const COST_THRESHOLD = 1000;
/**
 * Hjälpfunktion för att formatera valuta till svensk standard (SEK).
 * @param {number|string} amount 
 * @returns {string} Formaterad valuta
 */
const formatCurrency = (amount) => {
    const numericAmount = parseFloat(amount);
    if (isNaN(numericAmount)) return "0.00";
    
    return numericAmount.toLocaleString('sv-SE', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
    });
};

export default function MyAccountPage() {
  const [bookings, setBookings] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [showGymOfferModal, setShowGymOfferModal] = useState(false);
  const [showChangePw, setShowChangePw] = useState(false);
  const [oldPw, setOldPw] = useState("");
  const [newPw, setNewPw] = useState("");
  const [confirmPw, setConfirmPw] = useState("");
  const [pwError, setPwError] = useState("");
  const [pwSuccess, setPwSuccess] = useState("");
  const [pwLoading, setPwLoading] = useState(false);

  useEffect(() => {
    loadBookings();
  }, []);

  const loadBookings = async () => {
    try {
      setLoading(true);
      setError(null);
      
      const bookingsData = await fetchUserBookings();
      setBookings(bookingsData); 
    } catch (err) {
      setError(err.message || "Kunde inte hämta dina bokningsdata");
      console.error("Fel vid hämtning av bokningsdata:", err);
    } finally {
      setLoading(false);
    }
  };
      const handleChangePassword = async (e) => {
      e.preventDefault();
      setPwError("");
      setPwSuccess("");

      if (newPw !== confirmPw) {
        setPwError("De nya lösenorden matchar inte.");
        return;
      }

      try {
        setPwLoading(true);
        await changePassword(oldPw, newPw, confirmPw); 
        setPwSuccess("Lösenordet har ändrats!");
        setOldPw("");
        setNewPw("");
        setConfirmPw("");
        setTimeout(() => setShowChangePw(false), 1500);
      } catch (err) {
        setPwError(err.message || "Misslyckades med att ändra lösenordet.");
      } finally {
        setPwLoading(false);
      }
    };

  /**
   * Beräknar den totala kostnaden för alla aktiviteter 
   * som INTE har status "Cancelled" eller "Canceled".
   */
  const totalCost = useMemo(() => {
    if (!Array.isArray(bookings) || bookings.length === 0) {
      return 0;
    }

    // Filtrera bort avbokade bokningar och summera kostnaden för resten
    const sum = bookings
      .filter(booking => 

        booking.status !== "Cancelled" && 
        booking.status !== "Canceled"
      )
      .reduce((total, booking) => {
        // Hämta priset. 
        // Lägger till en kontroll för att hantera null/undefined om det behövs.
        const cost = parseFloat(booking.price || 0); 
        return total + cost;
      }, 0);

    return sum; 
  }, [bookings]); 

  useEffect(() => {
    if(!loading && totalCost >= COST_THRESHOLD) {  
      const hasSeenOffer = sessionStorage.getItem("seenOffer");
      if(!hasSeenOffer) {
        setShowGymOfferModal(true);
        sessionStorage.setItem("seenOffer", "true");
      }
    }
  }, [loading, totalCost]);

  // --- Renderingslogik för laddning och fel ---

  if (loading) {
    return (
      <div className="my-profile-page">
        <div className="loading">
          <div className="spinner"></div>
          <p>Laddar din profilinformation...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="my-profile-page">
        <div className="error-message">
          <h2>⚠️ Ett fel uppstod</h2>
          <p>{error}</p>
          <button onClick={loadBookings} className="retry-button">
            Försök igen
          </button>
        </div>
      </div>
    );
  }

  // --- Huvudrendering ---

  return (
    <div className="my-profile-page">
      <header className="page-header">
        <h1>Mina Sidor</h1>
        <p style={{ color: 'black' }}>En översikt av din aktivitetshistorik och utgifter</p>
      </header>

      <section className="profile-stats">
        <h2>Aktivitetsstatistik 📊</h2>

        <div className="stat-card total-spent">
          <h3>Totala utgifter (Betalda/Genomförda)</h3>
          <p className="stat-value">
            <span className="amount">{formatCurrency(totalCost)}</span> kr
          </p>
          <small>
              Baserat på {bookings.length} hämtade bokningar. 
              Avbokade aktiviteter är exkluderade.
          </small>
        </div>
        
        <div className="stat-card total-bookings">
          <h3>Totalt antal bokningar</h3>
          <p className="stat-value">
            <span className="amount">{bookings.length}</span> st
          </p>
        </div>
        <button
            type="button"
            className="primary-btn"
            onClick={() => setShowChangePw(true)}
          >
            Byt lösenord
          </button>
      </section>
{showGymOfferModal && (
        <Modal 
          title="🎉 Grattis! Exklusivt erbjudande!" 
          onClose={() => setShowGymOfferModal(false)}
        >
          <div className="offer-modal-content">
            <p>Vi ser att du har spenderat över <strong>{formatCurrency(COST_THRESHOLD)} kr</strong> på våra aktiviteter! Du är en riktig hälsohjälte.</p>
            <p>
          +   Som tack erbjuder vi dig <strong>20% rabatt</strong> på vårt årsabonnemang för gymkort. 
          +   Då får du obegränsad tillgång till alla våra träningsanläggningar!
          + </p>
            <p className="modal-cta">
              <button 
                className="modal-button primary-btn"
                onClick={() => {
                  alert("Tack! Vi kontaktar dig via e-post med din rabattkod.");
                  setShowGymOfferModal(false);
                }}
              >
                Jag vill ha erbjudandet!
              </button>
              
              <button 
                className="modal-button secondary-btn"
                onClick={() => setShowGymOfferModal(false)}
              >
                Inte nu
              </button>
            </p>
          </div>
        </Modal>
      )}
      {showChangePw && (
  <Modal title="Byt lösenord" onClose={() => { 
    setShowChangePw(false); 
    setPwError(""); setPwSuccess("");
    setOldPw(""); setNewPw(""); setConfirmPw("");
  }}>
    <form onSubmit={handleChangePassword} className="change-password-form">
      <div className="form-row">
        <label>Nuvarande lösenord</label>
        <input
          type="password"
          value={oldPw}
          onChange={(e) => setOldPw(e.target.value)}
          required
          autoComplete="current-password"
        />
      </div>

      <div className="form-row">
        <label>Nytt lösenord</label>
        <input
          type="password"
          value={newPw}
          onChange={(e) => setNewPw(e.target.value)}
          required
          autoComplete="new-password"
          placeholder="Minst 8 tecken"
        />
      </div>

      <div className="form-row">
        <label>Upprepa nytt lösenord</label>
        <input
          type="password"
          value={confirmPw}
          onChange={(e) => setConfirmPw(e.target.value)}
          required
          autoComplete="new-password"
        />
      </div>

      {pwError && <p className="form-error">{pwError}</p>}
      {pwSuccess && <p className="form-success">{pwSuccess}</p>}

      <div className="modal-actions">
        <button type="button" className="secondary-btn" onClick={() => {
          setShowChangePw(false);
          setPwError(""); setPwSuccess("");
          setOldPw(""); setNewPw(""); setConfirmPw("");
        }}>
          Avbryt
        </button>
        <button type="submit" className="primary-btn" disabled={pwLoading}>
          {pwLoading ? "Sparar..." : "Spara nytt lösenord"}
        </button>
      </div>
    </form>
  </Modal>
)}

    </div>
  );
}