import os
import ee
import logging
import requests
import numpy as np
from config.app_config import AppConfig


class GEEService:
    def __init__(self, app_config: AppConfig):
        self.__app_config = app_config
        self.__landsat = self.__initialize_earth_engine(app_config)

    async def image_download(self, point, name):
        image_res=30
        n_pixels=1024

        visParams={'min': 0, 'max': 3000, 'gamma': 1.4,  
                'bands' : ['B4', 'B3', 'B2'], 'dimensions' : str(n_pixels)+"x"+str(n_pixels),
                'format' : 'jpg'}
        
        len=image_res*n_pixels
        region= point.buffer(len/12).bounds().getInfo()['coordinates']
        coords=np.array(region)
        coords=[np.min(coords[:,:,0]), np.min(coords[:,:,1]), np.max(coords[:,:,0]), np.max(coords[:,:,1])]
        rectangle=ee.Geometry.Rectangle(coords)
        clipped_image= self.__landsat.mean().clip(rectangle)
            
        requests.get(clipped_image.getThumbUrl(visParams))
        path=os.path.join(self.__app_config.RAW_DATA_FOLDER_PATH, name)
        open(path, 'wb').write(requests.get(clipped_image.getThumbUrl(visParams)).content)
    
    def __initialize_earth_engine(self, app_config: AppConfig):
        try:
            service_account_key_path = os.path.join(
                (os.path.abspath("src")), app_config.SERVICE_ACCOUNT_KEY)
            credentials = ee.ServiceAccountCredentials(
                app_config.SERVICE_ACCOUNT, service_account_key_path)
            ee.Initialize(credentials)
            logging.info('Earth Engine initialized')
            startDate = '2020-01-01'
            endDate = '2020-12-31'
            
            sentinel = ee.ImageCollection("COPERNICUS/S2_SR")

            def mask_sentinel_sr(image):
                cloudBitMask = ee.Number(2).pow(10).int()
                cirrusBitMask = ee.Number(2).pow(11).int()
                qa = image.select('QA60')
                mask = qa.bitwiseAnd(cloudBitMask).eq(0).And(
                    qa.bitwiseAnd(cirrusBitMask).eq(0))
                return image.updateMask(mask).select(["B4", "B3", "B2"])

            sentinel = sentinel.filterDate(startDate, endDate).map(mask_sentinel_sr)
            return sentinel
        except ee.EEException as e:
            logging.error(f"Failed to initialize Google Earth Engine: {e}")

        
