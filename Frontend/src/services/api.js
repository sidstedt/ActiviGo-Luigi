// ============================================
// API Base URL & Imports
// ============================================
const API_BASE_URL = "https://localhost:7127/api";
import { jwtDecode } from "jwt-decode";

// ============================================
// Generic API Request with Error Handling
// ============================================
async function apiRequest(endpoint, method = 'GET', data = null, skipAuth = false) {
  const config = {
    method,
    headers: {
      'Content-Type': 'application/json',
    },
  };

  // Add token if available (unless skipAuth is true)
  if (!skipAuth) {
    const token = localStorage.getItem('accessToken');
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}`;
    }
  }

  // Add body for POST/PUT/PATCH
  if (data && ['POST', 'PUT', 'PATCH'].includes(method)) {
    config.body = JSON.stringify(data);
  }

  const response = await fetch(`${API_BASE_URL}/${endpoint}`, config);

  // Handle 204 no content
  if (response.status === 204) {
    return null;
  }

  // Handle errors
  if (!response.ok) {
    const error = await response.json().catch(() => ({ 
      message: `HTTP ${response.status}: ${response.statusText}` 
    }));
    throw new Error(error.message || 'Ett fel uppstod');
  }
  
  return response.json();
}

// ============================================
// ACTIVITIES
// ============================================
export async function fetchActivities() {
  return apiRequest(`Activities`);
}
export async function fetchActivityById(id) {
  return apiRequest(`Activities/${id}`);
}

// ============================================
// ACTIVITY OCCURRENCES
// ============================================
export async function fetchActivityOccurrences() {
  return apiRequest('ActivityOccurrence');
}

// ============================================
// BOOKINGS
// ============================================

// ============================================
// CATEGORIES
// ============================================

// ============================================
// ZONES
// ============================================

// ============================================
// STAFF
// ============================================

// ============================================
// AUTH
// ============================================

// Login
export async function login(email, password) {
  const data = await apiRequest('Auth/login', 'POST', { email, password }, true);
  
  // Store tokens
  localStorage.setItem('accessToken', data.accessToken);
  localStorage.setItem('refreshToken', data.refreshToken);
  
  // Decode token for user info
  const decoded = jwtDecode(data.accessToken);
  const roles = decoded.roles?.split(',') || [];
  const user = {
    id: decoded.sub,
    username: decoded.unique_name,
    email: decoded.email,
    roles: Array.isArray(roles) ? roles : [roles]
  };
  
  localStorage.setItem('user', JSON.stringify(user));
  return { ...data, user };
}

// Logout
export function logout() {
  localStorage.removeItem('accessToken');
  localStorage.removeItem('refreshToken');
  localStorage.removeItem('user');
  window.location.href = '/';
}

// Refresh Access Token
export async function refreshAccessToken() {
  const refreshToken = localStorage.getItem('refreshToken');
  if (!refreshToken) return false;
  
  try {
    const data = await apiRequest('Auth/refresh', 'POST', refreshToken, true);
    localStorage.setItem('accessToken', data.accessToken);
    localStorage.setItem('refreshToken', data.refreshToken);
    return true;
  } catch {
    logout();
    return false;
  }
}

// ============================================
// USERS
// ============================================

// Register new user
export async function registerUser(userData) {
  return apiRequest('Auth/register-user', 'POST', userData, true);
}

// Forgot Password
export async function forgotPassword(email) {
  return apiRequest('Users/forgot-password', 'POST', {
    email,
    resetUrl: 'http://localhost:5173/reset-password'
  }, true);
}

// Reset Password
export async function resetPassword(email, token, newPassword) {
  return apiRequest('Users/reset-password', 'POST', {
    email,
    token,
    newPassword
  }, true);
}

// ============================================
// ADMIN
// ============================================
/**
 * src/services/api.js
 * * Hanterar alla nätverksanrop, t.ex. att hämta aktiviteter.
 * I ett riktigt projekt, byt ut mock-data mot riktiga fetch-anrop.
 */

// Simulerar din befintliga API-URL

// Mock-data för att demonstrera visuell presentation
export const mockActivities = [
    { id: 1, name: "CrossFit-Passet", description: "Högintensivt pass med fokus på styrka och kondition.", date: "Idag, 18:00", location: "Huvudsalen", capacity: 20, bookedCount: 15 },
    { id: 2, name: "Yoga Flow - Morgon", description: "Mjukt flöde för att starta dagen med fokus på andning och rörlighet.", date: "Imorgon, 07:00", location: "Studion", capacity: 15, bookedCount: 5 },
    { id: 3, name: "Spinning: Intervall", description: "Tufft intervallpass på cykel.", date: "Torsdag, 19:30", location: "Spinninghallen", capacity: 30, bookedCount: 30 },
    { id: 4, name: "Basket: Öppen träning", description: "Informell match och träning, öppen för alla nivåer.", date: "Måndag, 20:00", location: "Idrottshallen", capacity: 40, bookedCount: 10 },
];

/**
 * Hämtar kommande aktiviteter.
 */
export async function fetchActivities() {
    try {
        // I en riktig applikation:
        // const response = await fetch(`${API_BASE_URL}/Activities`);
        // if (!response.ok) throw new Error("Fel vid hämtning av aktiviteter");
        // return response.json();

        await new Promise(resolve => setTimeout(resolve, 500)); // Simulerar nätverksfördröjning
        return mockActivities;

    } catch (error) {
        console.error("Kunde inte nå backend, använder mock-data:", error);
        // Återgår till mock-data vid fel för att behålla UI-funktionalitet
        return mockActivities; 
    }
}

