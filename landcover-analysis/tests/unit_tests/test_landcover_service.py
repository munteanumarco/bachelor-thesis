import pytest
from unittest.mock import AsyncMock, MagicMock, patch
from datetime import datetime

from services.landcover_service import LandCoverService
from models.LandCoverAnalysisStatus import LandCoverAnalysisStatus
from entities.LandCoverAnalyses import LandCoverAnalyses


@pytest.fixture
def session_factory():
    session = AsyncMock()
    session.begin = AsyncMock()
    yield session

@pytest.fixture
def landcover_service(session_factory):
    return LandCoverService(lambda: session_factory)

@pytest.mark.asyncio
async def test_get_landcover_analysis_found(landcover_service, session_factory):
    # Setup mock session behavior
    expected_result = LandCoverAnalyses(EmergencyEventId="123", Status="IN_PROGRESS")
    session_factory.execute.return_value.scalars.first.return_value = expected_result

    # Perform the test
    result = await landcover_service.get_landcover_analysis("123")
    assert result == expected_result

@pytest.mark.asyncio
async def test_get_landcover_analysis_not_found(landcover_service, session_factory):
    session_factory.execute.return_value.scalars.first.return_value = None

    result = await landcover_service.get_landcover_analysis("123")
    assert result is None

@pytest.mark.asyncio
async def test_update_landcover_analysis_status(landcover_service, session_factory):
    analysis = LandCoverAnalyses(EmergencyEventId="123", Status="IN_PROGRESS")
    session_factory.execute.return_value.scalars.first.return_value = analysis

    await landcover_service.update_landcover_analysis_status("123", LandCoverAnalysisStatus.COMPLETED)
    
    assert analysis.Status == LandCoverAnalysisStatus.COMPLETED.value
    assert isinstance(analysis.TriggeredAt, datetime)

@pytest.mark.asyncio
async def test_update_landcover_analysis_status_not_found(landcover_service, session_factory):
    session_factory.execute.return_value.scalars.first.return_value = None

    with pytest.raises(Exception) as excinfo:
        await landcover_service.update_landcover_analysis_status("123", LandCoverAnalysisStatus.COMPLETED)
    assert "Landcover analysis not found" in str(excinfo.value)

@pytest.mark.asyncio
async def test_update_analysis_urls(landcover_service, session_factory):
    analysis = LandCoverAnalyses(EmergencyEventId="123")
    session_factory.execute.return_value.scalars.first.return_value = analysis

    await landcover_service.update_analysis_urls("123", "http://example.com/raw", "http://example.com/processed")

    assert analysis.RawImage == "http://example.com/raw"
    assert analysis.ProcessedImage == "http://example.com/processed"
    assert analysis.Status == LandCoverAnalysisStatus.COMPLETED.value
    assert isinstance(analysis.CompletedAt, datetime)
