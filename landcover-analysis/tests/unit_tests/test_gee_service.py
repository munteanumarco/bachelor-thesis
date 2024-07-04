import pytest
from unittest.mock import Mock, patch
import numpy as np

from GEEService import GEEService
from config.app_config import AppConfig

@pytest.fixture
def app_config():
    config = AppConfig()
    config.SERVICE_ACCOUNT_KEY = "path/to/service_account.json"
    config.SERVICE_ACCOUNT = "my-service-account"
    config.RAW_DATA_FOLDER_PATH = "/path/to/raw/data"
    return config

@pytest.fixture
def ee_service(app_config):
    with patch('ee.Initialize'), patch('ee.ImageCollection'), patch('os.path.join', return_value='path/to/service_account.json'):
        return GEEService(app_config)

def test_initialize_earth_engine_success(app_config):
    with patch('ee.Initialize') as mock_initialize:
        service = GEEService(app_config)
        mock_initialize.assert_called_once()

def test_image_download(ee_service):
    point = Mock()
    point.buffer.return_value.bounds.return_value.getInfo.return_value = {'coordinates': [[[-123.123, 39.123], [-123.123, 39.123]]]}
    with patch('requests.get') as mock_get, patch('builtins.open', Mock()) as mock_open, patch('numpy.array', return_value=np.array([[[-123.123, 39.123], [-123.123, 39.123]]])):
        ee_service.image_download(point, "test_image.jpg")
        assert mock_get.called
        mock_open.assert_called_with('/path/to/raw/data/test_image.jpg', 'wb')

def test_image_download_requests_fail(ee_service):
    point = Mock()
    point.buffer.return_value.bounds.return_value.getInfo.return_value = {'coordinates': [[[-123.123, 39.123], [-123.123, 39.123]]]}
    with patch('requests.get', side_effect=Exception("Failed to download")) as mock_get, pytest.raises(Exception) as excinfo:
        ee_service.image_download(point, "test_image.jpg")
    assert "Failed to download" in str(excinfo.value)

