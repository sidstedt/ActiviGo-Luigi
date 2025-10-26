import React, { useState, useEffect } from 'react';
import '../styles/PaymentModal.css';

export default function PaymentModal({ booking, onClose, onPaymentSuccess }) {
  const [step, setStep] = useState(1); // 1: Payment form, 2: Processing, 3: Success
  const [paymentData, setPaymentData] = useState({
    cardNumber: '',
    expiryDate: '',
    cvv: '',
    cardholderName: '',
    email: ''
  });
  const [errors, setErrors] = useState({});

  // Simulera betalningsprocess
  const handlePayment = async (e) => {
    e.preventDefault();
    
    // Validera formulär
    const newErrors = {};
    if (!paymentData.cardNumber || paymentData.cardNumber.length < 16) {
      newErrors.cardNumber = 'Kortnummer måste vara 16 siffror';
    }
    if (!paymentData.expiryDate || !/^\d{2}\/\d{2}$/.test(paymentData.expiryDate)) {
      newErrors.expiryDate = 'Ange datum i format MM/YY';
    }
    if (!paymentData.cvv || paymentData.cvv.length < 3) {
      newErrors.cvv = 'CVV måste vara 3 siffror';
    }
    if (!paymentData.cardholderName.trim()) {
      newErrors.cardholderName = 'Kortinnehavarens namn krävs';
    }
    if (!paymentData.email || !/\S+@\S+\.\S+/.test(paymentData.email)) {
      newErrors.email = 'Giltig e-postadress krävs';
    }

    if (Object.keys(newErrors).length > 0) {
      setErrors(newErrors);
      return;
    }

    // Simulera betalningsprocess
    setStep(2);
    
    // Simulera API-anrop (2-3 sekunder)
    setTimeout(() => {
      setStep(3);
      
      // Efter framgång, uppdatera bokningsstatus
      setTimeout(() => {
        onPaymentSuccess(booking.id);
        onClose();
      }, 2000);
    }, 2500);
  };

  const formatCardNumber = (value) => {
    return value.replace(/\s/g, '').replace(/(.{4})/g, '$1 ').trim();
  };

  const formatExpiryDate = (value) => {
    return value.replace(/\D/g, '').replace(/(\d{2})(\d{0,2})/, '$1/$2');
  };

  const handleInputChange = (field, value) => {
    let formattedValue = value;
    
    if (field === 'cardNumber') {
      formattedValue = formatCardNumber(value);
    } else if (field === 'expiryDate') {
      formattedValue = formatExpiryDate(value);
    } else if (field === 'cvv') {
      formattedValue = value.replace(/\D/g, '').slice(0, 3);
    }
    
    setPaymentData(prev => ({
      ...prev,
      [field]: formattedValue
    }));
    
    // Rensa fel när användaren börjar skriva
    if (errors[field]) {
      setErrors(prev => ({
        ...prev,
        [field]: ''
      }));
    }
  };

  const handleClose = () => {
    if (step === 2) return; // Förhindra stängning under betalning
    onClose();
  };

  return (
    <div className="modal-backdrop payment-modal-backdrop" onClick={handleClose}>
      <div className="modal payment-modal" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h2>💳 Betala bokning</h2>
          <button 
            className="modal-close" 
            onClick={handleClose}
            disabled={step === 2}
            aria-label="Stäng"
          >
            ×
          </button>
        </div>

        <div className="modal-body">
          {/* Bokningssammanfattning */}
          <div className="booking-summary">
            <h3>Bokningssammanfattning</h3>
            <div className="summary-row">
              <span>Aktivitet:</span>
              <span>{booking.activityName}</span>
            </div>
            <div className="summary-row">
              <span>Datum:</span>
              <span>{new Date(booking.startTime).toLocaleDateString('sv-SE')}</span>
            </div>
            <div className="summary-row">
              <span>Tid:</span>
              <span>
                {new Date(booking.startTime).toLocaleTimeString('sv-SE', { 
                  hour: '2-digit', 
                  minute: '2-digit' 
                })} - {new Date(booking.endTime).toLocaleTimeString('sv-SE', { 
                  hour: '2-digit', 
                  minute: '2-digit' 
                })}
              </span>
            </div>
            <div className="summary-row">
              <span>Plats:</span>
              <span>{booking.zoneName}</span>
            </div>
            <div className="summary-row total-row">
              <span><strong>Totalt att betala:</strong></span>
              <span><strong>{booking.price} kr</strong></span>
            </div>
          </div>

          {/* Steg 1: Betalningsformulär */}
          {step === 1 && (
            <form onSubmit={handlePayment} className="payment-form">
              <div className="form-section">
                <h4>💳 Kortinformation</h4>
                
                <div className="form-group">
                  <label htmlFor="cardNumber">Kortnummer</label>
                  <input
                    type="text"
                    id="cardNumber"
                    value={paymentData.cardNumber}
                    onChange={(e) => handleInputChange('cardNumber', e.target.value)}
                    placeholder="1234 5678 9012 3456"
                    maxLength="19"
                    className={errors.cardNumber ? 'error' : ''}
                  />
                  {errors.cardNumber && <span className="error-text">{errors.cardNumber}</span>}
                </div>

                <div className="form-row">
                  <div className="form-group">
                    <label htmlFor="expiryDate">Giltigt till</label>
                    <input
                      type="text"
                      id="expiryDate"
                      value={paymentData.expiryDate}
                      onChange={(e) => handleInputChange('expiryDate', e.target.value)}
                      placeholder="MM/YY"
                      maxLength="5"
                      className={errors.expiryDate ? 'error' : ''}
                    />
                    {errors.expiryDate && <span className="error-text">{errors.expiryDate}</span>}
                  </div>

                  <div className="form-group">
                    <label htmlFor="cvv">CVV</label>
                    <input
                      type="text"
                      id="cvv"
                      value={paymentData.cvv}
                      onChange={(e) => handleInputChange('cvv', e.target.value)}
                      placeholder="123"
                      maxLength="3"
                      className={errors.cvv ? 'error' : ''}
                    />
                    {errors.cvv && <span className="error-text">{errors.cvv}</span>}
                  </div>
                </div>

                <div className="form-group">
                  <label htmlFor="cardholderName">Kortinnehavarens namn</label>
                  <input
                    type="text"
                    id="cardholderName"
                    value={paymentData.cardholderName}
                    onChange={(e) => handleInputChange('cardholderName', e.target.value)}
                    placeholder="Förnamn Efternamn"
                    className={errors.cardholderName ? 'error' : ''}
                  />
                  {errors.cardholderName && <span className="error-text">{errors.cardholderName}</span>}
                </div>
              </div>

              <div className="form-section">
                <h4>📧 Kontaktinformation</h4>
                
                <div className="form-group">
                  <label htmlFor="email">E-postadress</label>
                  <input
                    type="email"
                    id="email"
                    value={paymentData.email}
                    onChange={(e) => handleInputChange('email', e.target.value)}
                    placeholder="din@email.com"
                    className={errors.email ? 'error' : ''}
                  />
                  {errors.email && <span className="error-text">{errors.email}</span>}
                </div>
              </div>

              <div className="payment-security">
                <div className="security-badges">
                  <span className="security-badge">🔒 SSL-säker</span>
                  <span className="security-badge">🛡️ PCI-kompatibel</span>
                </div>
                <p className="security-note">
                  <small>
                    ⚠️ <strong>Demo-betalning:</strong> Detta är en simulering. 
                    Ingen riktig betalning genomförs.
                  </small>
                </p>
              </div>
            </form>
          )}

          {/* Steg 2: Bearbetar betalning */}
          {step === 2 && (
            <div className="payment-processing">
              <div className="processing-spinner">
                <div className="spinner"></div>
              </div>
              <h3>Bearbetar betalning...</h3>
              <p>Vänligen vänta medan vi behandlar din betalning.</p>
              <div className="processing-steps">
                <div className="step active">✓ Validerar kortinformation</div>
                <div className="step active">✓ Kontaktar bank</div>
                <div className="step processing">⏳ Bearbetar betalning</div>
                <div className="step">⏳ Bekräftar bokning</div>
              </div>
            </div>
          )}

          {/* Steg 3: Betalning lyckades */}
          {step === 3 && (
            <div className="payment-success">
              <div className="success-icon">✅</div>
              <h3>Betalning lyckades!</h3>
              <p>Din bokning har bekräftats och du kommer att få en bekräftelse via e-post.</p>
              <div className="success-details">
                <div className="detail-item">
                  <span>Transaktions-ID:</span>
                  <span>TXN-{Math.random().toString(36).substr(2, 9).toUpperCase()}</span>
                </div>
                <div className="detail-item">
                  <span>Betalat belopp:</span>
                  <span>{booking.price} kr</span>
                </div>
                <div className="detail-item">
                  <span>Status:</span>
                  <span className="status-confirmed">Bekräftad</span>
                </div>
              </div>
            </div>
          )}
        </div>

        <div className="modal-footer">
          {step === 1 && (
            <>
              <button type="button" className="secondary-button" onClick={handleClose}>
                Avbryt
              </button>
              <button 
                type="submit" 
                className="primary-button payment-button"
                onClick={handlePayment}
              >
                💳 Betala {booking.price} kr
              </button>
            </>
          )}
          {step === 2 && (
            <div className="processing-footer">
              <p>Betalningen behandlas...</p>
            </div>
          )}
          {step === 3 && (
            <div className="success-footer">
              <p>Stänger automatiskt...</p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
