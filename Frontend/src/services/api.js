// ============================================
// API Base URL
// ============================================
const API_BASE_URL = "https://localhost:7127/api";

// ============================================
// Generic API Request with Error Handling
// ============================================
async function apiRequest(endpoint, method = 'GET', data = null) {
  const config = {
    method,
    headers: {
      'Content-Type': 'application/json',
    },
  };

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

// ============================================
// USERS
// ============================================

// ============================================
// ADMIN
// ============================================