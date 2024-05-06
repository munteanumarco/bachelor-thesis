import datetime
import os
import ee
import logging
from config.app_config import AppConfig

class GEEService:
    def __init__(self, app_config: AppConfig):
        self.__initialize_earth_engine(app_config)

    def fetch_satellite_image(self, latitude, longitude, date, scale=30):
        """
        Fetches satellite image for given coordinates and date.
        :param latitude: float, Latitude of the location
        :param longitude: float, Longitude of the location
        :param date: str, Date in 'YYYY-MM-DD' format
        :param scale: int, Scale in meters
        :return: Image URL
        """
        try:
            # Create a point with the given latitude and longitude
            point = ee.Geometry.Point([longitude, latitude]).buffer(500)
            # Filter the image collection for the given date
            start_date, end_date = "2022-01-01", "2022-01-31"
            print(start_date, end_date)
            image = ee.ImageCollection('LANDSAT/LC09/C02/T1') \
                    .filterDate(start_date, end_date) \
                    .filterBounds(point) \
                    .first() \
                    .select('B4', 'B3', 'B2')  # Selects the RGB channels
            # Get the URL of the image
            url = image.getThumbUrl({'min': 0, 'max': 30000, 'region': point, 'format': 'png', 'scale': scale})
            return url
        except Exception as e:
            logging.error(f"Error fetching satellite image: {e}")
            return None
        
    def __initialize_earth_engine(self, app_config: AppConfig):
        try:
            service_account_key_path = os.path.join((os.path.abspath("src")), app_config.SERVICE_ACCOUNT_KEY)
            credentials = ee.ServiceAccountCredentials(app_config.SERVICE_ACCOUNT, service_account_key_path)
            ee.Initialize(credentials)
            logging.info('Earth Engine initialized')
        except ee.EEException as e:
            logging.error(f"Failed to initialize Google Earth Engine: {e}")

    def __parse_date(self, date_str):
        # Convert the date string to a datetime object
        date = datetime.datetime.strptime(date_str, '%Y-%m-%d')
        # Create a date range from one day before to one day after
        start_date = date - datetime.timedelta(month=1)
        end_date = date + datetime.timedelta(month=1)
        return start_date.strftime('%Y-%m-%d'), end_date.strftime('%Y-%m-%d')

