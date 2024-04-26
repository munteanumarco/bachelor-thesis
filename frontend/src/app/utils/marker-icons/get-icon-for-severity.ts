import { Severity } from '../../interfaces/emergency/Severity';
import { getMarkerIcon } from './get-marker-icon';
import { MarkerColor } from './marker-color';

export const getIconForSeverity = (severity: Severity): L.Icon => {
  switch (severity) {
    case Severity.CRITICAL:
      return getMarkerIcon(MarkerColor.RED);
    case Severity.HIGH:
      return getMarkerIcon(MarkerColor.ORANGE);
    case Severity.MEDIUM:
      return getMarkerIcon(MarkerColor.YELLOW);
    case Severity.LOW:
      return getMarkerIcon(MarkerColor.GREEN);
    default:
      return getMarkerIcon(MarkerColor.BLACK);
  }
};
