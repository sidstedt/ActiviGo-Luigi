export function getLocationAddressByZoneId(zones, locations, zoneId) {
  const zone = zones.find(z => (z.id ?? z.zoneId) === zoneId);
  if (zone && zone.locationId != null) {
    const loc = locations.find(l => l.id === zone.locationId);
    return loc?.address ?? null;
  }
  const locByMembership = locations.find(l => Array.isArray(l.zones) && l.zones.some(zz => (zz.id ?? zz.zoneId) === zoneId));
  return locByMembership?.address ?? null;
}

export function resolveLocationForOccurrence(occurrence, zones, locations) {
  const zone = zones.find(z => (z.id ?? z.zoneId) === occurrence.zoneId);
  let location = null;
  if (zone && zone.locationId != null) {
    location = locations.find(l => l.id === zone.locationId) || null;
  }
  if (!location) {
    location = locations.find(l => Array.isArray(l.zones) && l.zones.some(zz => (zz.id ?? zz.zoneId) === occurrence.zoneId)) || null;
  }
  const locationName = location?.name || zone?.locationName || null;
  const address = location?.address || null;
  const lat = occurrence.latitude ?? location?.latitude ?? null;
  const lon = occurrence.longitude ?? location?.longitude ?? null;
  return { locationName, address, lat, lon };
}
