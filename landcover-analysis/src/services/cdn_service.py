import cloudinary # type: ignore
from config.app_config import AppConfig
from cloudinary.uploader import upload # type: ignore

class CdnService():
    def __init__(self, app_config: AppConfig):
        self.__app_config = app_config
        self.__initialize_cloudinary()
    
    def __initialize_cloudinary(self):
        cloudinary.config( 
            cloud_name = self.__app_config.CLOUD_NAME, 
            api_key = self.__app_config.API_KEY, 
            api_secret = self.__app_config.API_SECRET,
            secure=True
        )
    
    def upload_image(self, image_path, id):
        upload_result = cloudinary.uploader.upload(image_path, public_id=id)
        return upload_result["secure_url"]