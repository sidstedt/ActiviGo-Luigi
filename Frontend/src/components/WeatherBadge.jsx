import '../styles/WeatherBadge.css';
import { weatherIcons } from '../assets/weather/mapping-icons';

export default function WeatherBadge({ forecast, className = '' }) {
  if (!forecast) return null;

  const iconSrc = weatherIcons[forecast.icon || 'unknown'] || weatherIcons['unknown'];
  const altText = forecast.summary || forecast.icon || 'Väder';

  return (
    <div className={`weather-block ${className}`}>
      <div className="weather-row">
        <img src={iconSrc} alt={altText} className="weather-icon" />
        <div className="weather-metrics">
          {forecast.temperature != null && (
            <span className="weather-temp">{forecast.temperature}°</span>
          )}
          {forecast.windMs != null && (
            <span className="weather-wind">{forecast.windMs} m/s</span>
          )}
        </div>
      </div>
      {forecast.summary && (
        <div className="weather-summary">{forecast.summary}</div>
      )}
    </div>
  );
}
