from fastapi import APIRouter, Depends
from services.gee_service import GEEService
from utils.dependency_container import DependencyContainer

router = APIRouter(
    prefix="/satellite",
)

def get_gee_service() -> GEEService:
    return DependencyContainer().get_dependency(GEEService)

@router.get("/image")
async def satellite_image(latitude: float, longitude: float, date: str, gee_service: GEEService = Depends(get_gee_service)):
    image_url = gee_service.fetch_satellite_image(latitude, longitude, date)
    if image_url:
        return {"url": image_url}
    else:
        return {"error": "Failed to fetch image"}
    