// Notera: Du behöver INTE omsluta detta i <script type="text/babel">
// eftersom du länkade filen med det attributet i index.html.
import "../styles/Dashboard.css";
import { useState, useEffect, useMemo } from 'react';

// --- START: Simulerad API-tjänst ---

const mockActivities = [
    { id: 1, name: "CrossFit-Passet", description: "Högintensivt pass med fokus på styrka och kondition.", date: "Idag, 18:00", location: "Huvudsalen", capacity: 20, bookedCount: 15 },
    { id: 2, name: "Yoga Flow - Morgon", description: "Mjukt flöde för att starta dagen med fokus på andning och rörlighet.", date: "Imorgon, 07:00", location: "Studion", capacity: 15, bookedCount: 5 },
    { id: 3, name: "Spinning: Intervall", description: "Tufft intervallpass på cykel.", date: "Torsdag, 19:30", location: "Spinninghallen", capacity: 30, bookedCount: 30 },
    { id: 4, name: "Basket: Öppen träning", description: "Informell match och träning, öppen för alla nivåer.", date: "Måndag, 20:00", location: "Idrottshallen", capacity: 40, bookedCount: 10 },
];

async function fetchActivities() {
    try {
        await new Promise(resolve => setTimeout(resolve, 500)); 
        return mockActivities.map(a => ({...a})); 
    } catch (error) {
        console.error("Kunde inte nå backend, använder mock-data:", error);
        return mockActivities.map(a => ({...a}));
    }
}

async function bookActivity(activityId) {
    await new Promise(resolve => setTimeout(resolve, 1000)); 

    const mockActivity = mockActivities.find(a => a.id === activityId);

    if (!mockActivity) {
        return { success: false, message: "Aktivitet hittades inte." };
    }

    if (mockActivity.bookedCount >= mockActivity.capacity) {
        return { success: false, message: "Aktiviteten är redan fullbokad!" };
    }

    if (mockActivity) {
        mockActivity.bookedCount += 1;
    }

    return { 
        success: true, 
        message: "Bokning genomförd.",
        name: mockActivity.name 
    };
}
// --- SLUT: Simulerad API-tjänst ---


const StatCard = ({ title, value, icon, linkText, onClick }) => (
    <div className="stat-card"> 
        <div className="stat-card-content">
            <span className="stat-icon">{icon}</span>
            <div>
                <p className="stat-title">{title}</p>
                <p className="stat-value">{value}</p>
            </div>
        </div>
        <button 
            onClick={onClick}
            className="stat-button"
        >
            {linkText}
        </button>
    </div>
);

const ActivityCard = ({ activity, onBookingSuccess }) => {
    const slotsLeft = activity.capacity - activity.bookedCount;
    const isFull = slotsLeft <= 0;
    const isBookingPending = activity.isBookingPending;

    const handleBooking = () => {
        if (isFull || isBookingPending) return;
        onBookingSuccess(activity.id); 
    };

    return (
        <div className="activity-card">
            <div>
                <h3 className="activity-name">{activity.name}</h3>
                <p className="activity-details">{activity.date} - {activity.location}</p>
                <p className="activity-description">{activity.description}</p>
            </div>
            <div className="activity-footer">
                <div className="activity-slots">
                    <span className="slots-label">Platser kvar:</span>
                    <span className={`slots-count ${isFull ? 'status-full' : 'status-available'}`}>
                        {isFull ? 'Fullbokat!' : `${slotsLeft} av ${activity.capacity}`}
                    </span>
                </div>
                <button
                    className="activity-button"
                    disabled={isFull || isBookingPending}
                    onClick={handleBooking}
                >
                    {isBookingPending ? (
                        <>
                            <svg className="loading-icon" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" width="20" height="20">
                                <circle cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                                <path fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                            </svg>
                            Bokar...
                        </>
                    ) : (
                        isFull ? 'Väntelista' : 'Boka nu'
                    )}
                </button>
            </div>
        </div>
    );
};

function DashboardPage() {
    const [isSidebarOpen, setIsSidebarOpen] = useState(false);
    const [activities, setActivities] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [userName, setUserName] = useState("Användare"); 
    const [bookingMessage, setBookingMessage] = useState(null);

    const loadActivities = () => {
        setLoading(true);
        setError(null);
        fetchActivities()
            .then(data => {
                const initialActivities = data.map(a => ({ ...a, isBookingPending: false }));
                setActivities(initialActivities);
                setLoading(false);
            })
            .catch(err => {
                console.error("API-fel vid laddning:", err);
                setError("Kunde inte ladda aktiviteter. Kontrollera API-anslutningen.");
                setLoading(false);
            });
    };

    useEffect(() => {
        const loggedInUserName = "Luigi Svensson"; 
        setUserName(loggedInUserName);
        loadActivities();
    }, []);

    const handleActivityBooking = async (activityId) => {
        setActivities(prevActivities => 
            prevActivities.map(a => 
                a.id === activityId ? { ...a, isBookingPending: true } : a
            )
        );
        setBookingMessage(null);

        try {
            const result = await bookActivity(activityId); 

            if (result.success) {
                setActivities(prevActivities => 
                    prevActivities.map(a => 
                        a.id === activityId 
                            ? { ...a, bookedCount: a.bookedCount + 1, isBookingPending: false } 
                            : a
                    )
                );
                setBookingMessage({ type: 'success', text: `Du bokade framgångsrikt: ${result.name}!` });
            } else {
                setActivities(prevActivities => 
                    prevActivities.map(a => 
                        a.id === activityId ? { ...a, isBookingPending: false } : a
                    )
                );
                setBookingMessage({ type: 'error', text: result.message || "Ett okänt fel uppstod vid bokning." });
            }
        } catch (err) {
            console.error("Bokningsfel:", err);
            setActivities(prevActivities => 
                prevActivities.map(a => 
                    a.id === activityId ? { ...a, isBookingPending: false } : a
                )
            );
            setBookingMessage({ type: 'error', text: "Nätverksfel: Kunde inte slutföra bokningen." });
        }

        setTimeout(() => setBookingMessage(null), 4000);
    };


    let activitiesContent;
    if (loading) {
        activitiesContent = (
            <div className="loading-container">
                <svg className="loading-icon-large" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" width="32" height="32">
                    <circle cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                    <path fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
                <span className="loading-text">Laddar aktiviteter...</span>
            </div>
        );
    } else if (error) {
        activitiesContent = (
            <div className="error-message">
                <p className="error-title">Ett fel uppstod vid hämtning:</p>
                <p>{error}</p>
            </div>
        );
    } else if (activities.length === 0) {
        activitiesContent = (
            <div className="warning-message">
                <p>Inga aktiviteter hittades just nu. Perfekt tid att lägga till nya pass!</p>
            </div>
        );
    } else {
        activitiesContent = (
            <div className="activity-grid">
                {activities.map(a => (
                    <ActivityCard 
                        key={a.id} 
                        activity={a} 
                        onBookingSuccess={handleActivityBooking}
                    />
                ))}
            </div>
        );
    }


    return (
        <div id="app-container">

            {/* Bokningsmeddelande (Toast) */}
            {bookingMessage && (
                <div className={`toast-message ${bookingMessage.type}`}>
                    {bookingMessage.text}
                </div>
            )}

            {/* 1. Header: Topp-bar */}
            <header id="main-header">
                <div className="logo">
                    <span className="logo-highlight">Activity</span>Go
                </div>
                <div className="header-controls">
                    <span className="welcome-text">
                        Välkommen, {userName}!
                    </span>
                    <button 
                        className="menu-button"
                        onClick={() => setIsSidebarOpen(!isSidebarOpen)}
                    >
                        {/* Burgar-ikon */}
                        <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" width="24" height="24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M4 6h16M4 12h16M4 18h16"></path></svg>
                    </button>
                </div>
            </header>

            {/* 2. Main Layout: Sidomeny + Huvudinnehåll */}
            <div id="main-layout">

                {/* Sidebar: Navigering */}
                <nav id="sidebar" className={isSidebarOpen ? 'open' : ''}>
                    
                    <div className="nav-group">
                        <div className="nav-title">MENY</div>
                        
                        {['Startsida', 'Mina Bokningar', 'Sök & Boka', 'Träningshistorik', 'Profil & Inställningar'].map(item => (
                            <a 
                                key={item} 
                                href="#" 
                                className={`nav-link ${item === 'Startsida' ? 'active' : ''}`}
                            >
                                {item}
                            </a>
                        ))}
                    </div>
                </nav>

                {/* 3. Main Content Area: Din startsida/Dashboard */}
                <main id="main-content">
                    <div className="content-wrapper">
                        
                        {/* Välkomstbanner */}
                        <div className="welcome-banner">
                            
                            <div className="banner-content">
                                <h1 className="banner-title">
                                    Välkommen, <span className="highlight-text">{userName}!</span> 
                                </h1>
                                <p className="banner-subtitle">
                                    Boka ditt nästa pass snabbt och enkelt. Se dina kommande bokningar och upptäck nya sporter.
                                </p>
                                <button className="banner-button">
                                    Se tillgängliga tider
                                </button>
                            </div>
                        </div>

                        {/* Statistik / Genvägar */}
                        <div className="stat-grid">
                            <StatCard 
                                title="Mina Bokningar" 
                                value="3 Kommande Pass" 
                                icon="🗓️" 
                                linkText="Hantera bokningar"
                                onClick={() => console.log("Gå till Mina Bokningar")}
                            />
                            <StatCard 
                                title="Nästa Pass" 
                                value="CrossFit 18:00 (Idag)" 
                                icon="⏱️" 
                                linkText="Avboka / Ändra"
                                onClick={() => console.log("Gå till pass-detaljer")}
                            />
                            <StatCard 
                                title="Favoritaktivitet" 
                                value="Yoga Flow" 
                                icon="🧘‍♀️" 
                                linkText="Boka igen"
                                onClick={() => console.log("Gå till Sök & Boka")}
                            />
                        </div>

                        {/* Aktivitetssektion */}
                        <h2 className="section-title">Populära & Kommande Aktiviteter</h2>
                        {activitiesContent}

                    </div>
                </main>
            </div>
        </div>
    );
}

// Rendera applikationen till DOM-trädet

export default DashboardPage;