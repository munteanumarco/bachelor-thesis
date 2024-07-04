import pytest
from unittest.mock import patch
from config.app_config import AppConfig
from services.cdn_service import CdnService

@pytest.fixture
def app_config():
    config = AppConfig()
    config.CLOUD_NAME = 'my_cloud'
    config.API_KEY = 'my_api_key'
    config.API_SECRET = 'my_api_secret'
    return config

@pytest.fixture
def cdn_service(app_config):
    return CdnService(app_config)

def test_initialize_cloudinary(cdn_service, app_config):
    with patch('cloudinary.config') as mock_config:
        cdn_service.__initialize_cloudinary()
        mock_config.assert_called_once_with(
            cloud_name=app_config.CLOUD_NAME,
            api_key=app_config.API_KEY,
            api_secret=app_config.API_SECRET,
            secure=True
        )

def test_upload_image(cdn_service):
    with patch('cloudinary.uploader.upload', return_value={'secure_url': 'http://example.com/image.jpg'}) as mock_upload:
        result = cdn_service.upload_image('path/to/image.jpg', 'image_id')
        assert result == 'http://example.com/image.jpg'
        mock_upload.assert_called_once_with('path/to/image.jpg', public_id='image_id')
