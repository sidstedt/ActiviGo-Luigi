const API_BASE_URL = "https://localhost:7127/api";

export async function fetchActivities() {
  const response = await fetch(`${API_BASE_URL}/Activities`);
  if (!response.ok) throw new Error("Fel vid hämtning av aktiviteter");
  return response.json();
}