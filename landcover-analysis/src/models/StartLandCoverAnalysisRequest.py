from pydantic import BaseModel

class StartLandcoverAnalysisRequest(BaseModel):
    latitude: float
    longitude: float
    client_id: str
