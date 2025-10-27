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
    headers: { 'Content-Type': 'application/json' },
  };

  if (!skipAuth) {
    const token = localStorage.getItem('accessToken');
    if (token) config.headers['Authorization'] = `Bearer ${token}`;
  }

  if (data && ['POST', 'PUT', 'PATCH'].includes(method)) {
    config.body = JSON.stringify(data);
  }

  const url = `${API_BASE_URL}/${endpoint}`;

  let response = await fetch(url, config);

  if (response.status === 401 && !skipAuth) {
    try {
      const refreshed = await refreshAccessToken();
      if (refreshed) {
        const newToken = localStorage.getItem('accessToken');
        if (newToken) config.headers['Authorization'] = `Bearer ${newToken}`;
        response = await fetch(url, config);
      }
    } catch (err) {
    }
  }

  if (response.status === 204) {
    return null;
  }

  if (!response.ok) {
    let bodyText = '';
    try { bodyText = await response.text(); } catch (e) {}
    let errorJson = {};
    try { errorJson = bodyText ? JSON.parse(bodyText) : {}; } catch (e) {}

    const identityErrors =
      (errorJson.errors && Array.isArray(errorJson.errors) && errorJson.errors.map(e => e.description || e.code || e).join('\n')) ||
      (errorJson.Errors && Array.isArray(errorJson.Errors) && errorJson.Errors.map(e => e.description || e.code || e).join('\n')) ||
      errorJson.title ||
      errorJson.message ||
      errorJson.Message ||
      bodyText;

    const msg = identityErrors || `HTTP ${response.status}: ${response.statusText}`;
    const err = new Error(msg);
    err.details = errorJson;
    throw err;
  }

  return response.json();
}

// ============================================
// ACTIVITIES
// ============================================
export async function fetchActivities() {
  return apiRequest(`Activities`);
}

export async function createActivity(activityData) {
  return apiRequest('Activities', 'POST', activityData);
}

export async function updateActivity(id, activityData) {
  return apiRequest(`Activities/${id}`, 'PUT', activityData);
}

export async function deleteActivity(id) {
  return apiRequest(`Activities/${id}`, 'DELETE');
}

// ============================================
// ACTIVITY OCCURRENCES
// ============================================
export async function fetchActivityOccurrences() {
  return apiRequest('ActivityOccurrence');
}

export async function createActivityOccurrence(data) {
  return apiRequest('ActivityOccurrence', 'POST', data);
}

export async function updateActivityOccurrence(id, data) {
  return apiRequest(`ActivityOccurrence/${id}`, 'PUT', data);
}

// ============================================
// WEATHER
// ============================================
export async function fetchWeatherForecastBatch(queries) {
  return apiRequest('Weather/forecast', 'POST', queries, true);
}

// ============================================
// BOOKINGS
// ============================================
export async function fetchUserBookings(userId) {
  const endpoint = userId
    ? `Bookings/UserGetBookings?userId=${encodeURIComponent(userId)}`
    : "Bookings/UserGetBookings";
  return apiRequest(endpoint);
}

export async function fetchBookingById(id) {
  return apiRequest(`Bookings/${id}`);
}

export async function createBooking(bookingData) {
  return apiRequest('Bookings', 'POST', bookingData);
}

export async function cancelBooking(id) {
  return apiRequest(`Bookings/${id}`, 'DELETE');
}

export async function confirmBooking(id) {
  return apiRequest(`Bookings/${id}`, 'PUT', { status: 'Confirmed' });
}

export async function fetchAdminBookings() {
  return apiRequest('Bookings/AdminGetBookings');
}


// ============================================
// CATEGORIES
// ============================================
export async function fetchCategories()
{ return apiRequest('Category'); }

// ============================================
// ZONES
// ============================================
export async function fetchZones() {
  return apiRequest('Zone');
}

export async function createZone(zoneData) {
  return apiRequest('Zone', 'POST', zoneData)
}

export async function updateZone(id, zoneData) {
  return apiRequest(`Zone/${id}`, 'PUT', zoneData)
}

export async function deleteZone(id) {
  return apiRequest(`Zone/${id}`, 'DELETE')
}

// ============================================
// STAFF
// ============================================
export async function fetchStaff() {
  return apiRequest('Users/staff');
}

// ============================================
// AUTH
// ============================================

export async function login(email, password) {
  const data = await apiRequest('Auth/login', 'POST', { email, password }, true);
  
  localStorage.setItem('accessToken', data.accessToken);
  localStorage.setItem('refreshToken', data.refreshToken);
  
  const decoded = jwtDecode(data.accessToken);
  const roles = (decoded.roles?.split(',') || []).map(r => String(r).trim().toLowerCase());
  const firstName = decoded.given_name || decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"]; 
  const lastName = decoded.family_name || decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname"]; 
  const displayName = [firstName, lastName].filter(Boolean).join(' ') || decoded.unique_name || decoded.name || decoded.email;
  const jwtEmail = decoded.email || decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"] || null;
  const user = {
    id: decoded.sub,
    username: displayName,
    firstName: firstName || null,
    lastName: lastName || null,
    email: jwtEmail,
    roles: Array.isArray(roles) ? roles : [roles]
  };
  
  localStorage.setItem('user', JSON.stringify(user));
  return { ...data, user };
}

export function logout() {
  localStorage.removeItem('accessToken');
  localStorage.removeItem('refreshToken');
  localStorage.removeItem('user');
  window.location.href = '/';
}

export async function refreshAccessToken() {
  const refreshToken = localStorage.getItem('refreshToken');
  if (!refreshToken) return false;
  
  try {
    const data = await apiRequest('Auth/refresh', 'POST', {refreshToken}, true);
    localStorage.setItem('accessToken', data.accessToken);
    localStorage.setItem('refreshToken', data.refreshToken);
    return true;
  } catch {
    logout();
    return false;
  }
}

export function getCurrentUser() {
  const token = localStorage.getItem('accessToken');
  if (token) {
    try {
      const decoded = jwtDecode(token);
      let roles = [];
      if (decoded.roles) {
        roles = String(decoded.roles).split(',').map(r => String(r).trim().toLowerCase()).filter(Boolean);
      } else if (decoded.role) {
        roles = Array.isArray(decoded.role) ? decoded.role.map(r => String(r).toLowerCase()) : [String(decoded.role).toLowerCase()];
      }
      const firstName = decoded.given_name || decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"]; 
      const lastName = decoded.family_name || decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname"]; 
      const email = decoded.email || decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"] || null;
      const displayName = [firstName, lastName].filter(Boolean).join(' ') 
        || decoded.unique_name || decoded.name || decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] 
        || email || null;
      const user = {
        id: decoded.sub,
        username: displayName,
        firstName: firstName || null,
        lastName: lastName || null,
        email,
        roles
      };
      localStorage.setItem('user', JSON.stringify(user));
      return user;
    } catch {
      // fall through to cached
    }
  }
  try {
    const cached = localStorage.getItem('user');
    if (cached) {
      const user = JSON.parse(cached);
      return user || null;
    }
  } catch {
    // ignore
  }
  return null;
}

// ============================================
// LOCATIONS
// ============================================
export async function fetchLocations() {
  return apiRequest('Location');
}

export async function createLocation(locationData) {
  return apiRequest('Location', 'POST', locationData)
}

export async function updateLocation(id, locationData) {
  return apiRequest(`Location/${id}`, 'PUT', locationData);
}

export async function deleteLocation(id) {
  return apiRequest(`Location/${id}`, 'DELETE');
}

// ============================================
// USERS
// ============================================
export async function registerUser(userData) {
  return apiRequest('Auth/register-user', 'POST', userData, true);
}

export async function forgotPassword(email) {
  return apiRequest('Users/forgot-password', 'POST', {
    email,
    resetUrl: 'http://localhost:5173/reset-password'
  }, true);
}

export async function resetPassword(email, token, newPassword) {
  return apiRequest('Users/reset-password', 'POST', {
    email,
    token,
    newPassword
  }, true);
}

export async function changePassword(currentPassword, newPassword, confirmPassword) {
  const payload = {
    currentPassword,
    newPassword,
    confirmPassword,
    oldPassword: currentPassword,
    confirmNewPassword: confirmPassword
  };

  return apiRequest('Users/change-password', 'POST', payload);
}

export async function fetchUsers() {
  return apiRequest("Users");
}

// ============================================
// ADMIN CRUD – Användare, Aktiviteter, Bokningar, Statistik
// ============================================

// ========== USERS ==========
export async function fetchAllUsers() {
  return apiRequest("Admin/users");
}

export async function fetchUserById(id) {
  return apiRequest(`Admin/users/${id}`);
}

export async function createAdminUser(userData) {
  return apiRequest("Admin/users", "POST", userData);
}

export async function updateAdminUser(id, userData) {
  return apiRequest(`Admin/users/${id}`, "PUT", userData);
}

export async function deleteAdminUser(id) {
  return apiRequest(`Admin/users/${id}`, "DELETE");
}