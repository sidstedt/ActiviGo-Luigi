import { useEffect, useState, useRef } from "react";
import { createPortal } from "react-dom";
import "../styles/Modal.css";

export default function AppModal({ title, children, onClose }) {
  const [show, setShow] = useState(false);
  const overlayRef = useRef(null);
  const mouseDownInside = useRef(false);

  useEffect(() => {
    setShow(true);
  }, []);

  const handleMouseDown = (e) => {
    if (e.target.closest(".modal")) {
      mouseDownInside.current = true;
    } else {
      mouseDownInside.current = false;
    }
  };

  const handleMouseUp = (e) => {
    if (!mouseDownInside.current && e.target === overlayRef.current) {
      setShow(false);
      setTimeout(onClose, 200);
    }
  };

  return createPortal(
    (
      <div
        ref={overlayRef}
        className={`overlay ${show ? "show" : ""}`}
        onMouseDown={handleMouseDown}
        onMouseUp={handleMouseUp}
        role="dialog"
        aria-modal="true"
        aria-label={title}
      >
        <div className={`modal ${show ? "show" : ""}`}>
          <h3 className="title">{title}</h3>
          {children}
          <button
            type="button"
            onClick={() => {
              setShow(false);
              setTimeout(onClose, 200);
            }}
            className="close-button"
          >
            Stäng
          </button>
        </div>
      </div>
    ),
    document.body
  );
}
