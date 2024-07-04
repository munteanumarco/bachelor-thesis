import datetime
from sqlalchemy import select
from entities.LandCoverAnalyses import LandCoverAnalyses
from models.LandCoverAnalysisStatus import LandCoverAnalysisStatus


class LandCoverService:
    def __init__(self, session_factory):
        self.session_factory = session_factory

    async def get_landcover_analysis(self, emergency_event_id: str):
        async with self.session_factory() as session:
            async with session.begin():
                query = select(LandCoverAnalyses).where(
                    LandCoverAnalyses.EmergencyEventId == emergency_event_id)
                result = await session.execute(query)
                return result.scalars().first()

    async def update_landcover_analysis_status(self, emergency_event_id: str, status: LandCoverAnalysisStatus):
        async with self.session_factory() as session:
            async with session.begin():
                query = select(LandCoverAnalyses).where(
                    LandCoverAnalyses.EmergencyEventId == emergency_event_id)
                result = await session.execute(query)
                analysis = result.scalars().first()

                if not analysis:
                    raise Exception("Landcover analysis not found")

                analysis.Status = status.value
                analysis.TriggeredAt = datetime.datetime.now()
    
    async def update_analysis_urls(self, emergency_event_id: str, raw_url: str, proccessed_url: str):
        async with self.session_factory() as session:
            async with session.begin():
                query = select(LandCoverAnalyses).where(
                    LandCoverAnalyses.EmergencyEventId == emergency_event_id)
                result = await session.execute(query)
                analysis: LandCoverAnalyses = result.scalars().first()

                if not analysis:
                    raise Exception("Landcover analysis not found")

                analysis.RawImage = raw_url
                analysis.ProcessedImage = proccessed_url
                analysis.Status = LandCoverAnalysisStatus.COMPLETED.value
                analysis.CompletedAt = datetime.datetime.now()
