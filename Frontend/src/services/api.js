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
    const msg = error.message || error.Message || `HTTP ${response.status}: ${response.statusText}`;
    throw new Error(msg);
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
  // data: { startTime, durationMinutes?, activityId, zoneId? }
  return apiRequest('ActivityOccurrence', 'POST', data);
}

export async function updateActivityOccurrence(id, data) {
  // data: { startTime, endTime, durationMinutes, activityId, zoneId, isActive }
  return apiRequest(`ActivityOccurrence/${id}`, 'PUT', data);
}

// ============================================
// WEATHER
// ============================================
export async function fetchWeatherForecastBatch(queries) {
  // queries: [{ occurrenceId, latitude, longitude, at }]
  return apiRequest('Weather/forecast', 'POST', queries, true);
}

// ============================================
// BOOKINGS
// ============================================
export async function fetchUserBookings() {
  return apiRequest('Bookings/UserGetBookings');
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

export async function fetchAdminBookings() {
  return apiRequest('Bookings/AdminGetBookings');
}


// ============================================
// CATEGORIES
// ============================================
// Categories (unused currently)
export async function fetchCategories()
{ return apiRequest('Category'); }
// export async function fetchCategoriesWithActivities() { return apiRequest('Category/withActivities'); }

// ============================================
// ZONES
// ============================================
export async function fetchZones() {
  return apiRequest('Zone');
}

// Zones (unused currently)
// export async function fetchZonesWithRelations() { return apiRequest('Zone/withRelations'); }
// export async function fetchZonesByLocation(locationId) { return apiRequest(`Zone/location/${locationId}`); }

// ============================================
// STAFF
// ============================================
export async function fetchStaff() {
  return apiRequest('Users/staff');
}

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

// Current user (from token or cache)
export function getCurrentUser() {
  const token = localStorage.getItem('accessToken');
  if (token) {
    try {
      const decoded = jwtDecode(token);
      // roles may be a comma string, or an array under 'role'
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

// (unused) export async function fetchLocationById(id) { return apiRequest(`Location/${id}`); }

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
// ADMIN CRUD – Användare, Aktiviteter, Bokningar, Statistik
// ============================================

// ========== USERS ==========
// export async function fetchAllUsers() {
//   return apiRequest('Admin/users');
// }

// export async function fetchUserById(id) {
//   return apiRequest(`Admin/users/${id}`);
// }

// export async function createUser(userData) {
//   return apiRequest('Admin/users', 'POST', userData);
// }

// export async function updateUser(id, userData) {
//   return apiRequest(`Admin/users/${id}`, 'PUT', userData);
// }

// export async function deleteUser(id) {
//   return apiRequest(`Admin/users/${id}`, 'DELETE');
// }

// // ========== ACTIVITIES ==========
// export async function fetchAllActivities() {
//   return apiRequest('Admin/activities');
// }

// export async function fetchActivityById(id) {
//   return apiRequest(`Admin/activities/${id}`);
// }

// export async function createActivity(activityData) {
//   return apiRequest('Activities', 'POST', activityData);
// }

// export async function updateActivity(id, activityData) {
//   return apiRequest(`Admin/activities/${id}`, 'PUT', activityData);
// }

// export async function deleteActivity(id) {
//   return apiRequest(`Admin/activities/${id}`, 'DELETE');
// }

// ========== BOOKINGS ==========
// export async function fetchAllBookings() {
//   return apiRequest('Admin/bookings');
// }

// export async function fetchBookingDetails(id) {
//   return apiRequest(`Admin/bookings/${id}`);
// }

// export async function updateBooking(id, bookingData) {
//   return apiRequest(`Admin/bookings/${id}`, 'PUT', bookingData);
// }

// export async function deleteBooking(id) {
//   return apiRequest(`Admin/bookings/${id}`, 'DELETE');
// }

// // ========== STATISTICS ==========
// export async function fetchStatistics() {
//   return apiRequest('Admin/statistics');
// }
