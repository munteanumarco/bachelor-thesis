from fastapi.testclient import TestClient
from fastapi import FastAPI
from unittest.mock import AsyncMock, patch
import pytest
from src.models import LandCoverAnalysisStatus

@pytest.fixture
def client():
    return TestClient(app)

@pytest.fixture
def mock_landcover_service():
    with patch('services.landcover_service.LandCoverService', autospec=True) as mock:
        yield mock()

@pytest.fixture
def mock_orchestrator_service():
    with patch('services.orchestrator_service.OrchestratorService', autospec=True) as mock:
        yield mock()

@pytest.fixture
def mock_websocket_manager():
    with patch('services.websocket_manager.WebSocketManager', autospec=True) as mock:
        yield mock()

def test_up_route(client):
    response = client.get("/analysis/up")
    assert response.status_code == 200
    assert response.json() == {"status": "up"}

def test_get_landcover_analysis(client, mock_landcover_service):
    mock_landcover_service.get_landcover_analysis.return_value = {"id": "123", "status": "complete"}
    response = client.get("/analysis/123")
    assert response.status_code == 200
    assert response.json() == {"id": "123", "status": "complete"}
    mock_landcover_service.get_landcover_analysis.assert_called_once_with("123")

def test_get_landcover_analysis_not_found(client, mock_landcover_service):
    mock_landcover_service.get_landcover_analysis.return_value = None
    response = client.get("/analysis/123")
    assert response.status_code == 404
    assert response.json() == {"detail": "Landcover analysis not found"}

def test_trigger_landcover_analysis(client, mock_landcover_service, mock_orchestrator_service):
    request_payload = {
        "client_id": "client-123",
        "latitude": 10.0,
        "longitude": 20.0
    }
    response = client.post("/analysis/123", json=request_payload)
    assert response.status_code == 202
    assert response.json() == {"message": "The landcover analysis has been triggered"}
    mock_orchestrator_service.process_analysis.assert_called_once()
    mock_landcover_service.update_landcover_analysis_status.assert_called_with(
        "123", LandCoverAnalysisStatus.IN_PROGRESS
    )
