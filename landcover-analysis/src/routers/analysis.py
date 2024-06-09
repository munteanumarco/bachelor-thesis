from fastapi import APIRouter, Depends
from services.road_segmentation_service import RoadSegmentationService
from utils.dependency_container import DependencyContainer

router = APIRouter(
    prefix="/analysis",
)

def get_road_segmentation_service() -> RoadSegmentationService:
    return DependencyContainer().get_dependency(RoadSegmentationService)

@router.get("/test")
async def satellite_image(road_segmentation_service: RoadSegmentationService = Depends(get_road_segmentation_service)):
    raw_image_path, proccessed_image_path = road_segmentation_service.segment_road("image.jpg")
    return {"raw_image_path": raw_image_path, "proccessed_image_path": proccessed_image_path}

@router.get("/up")
async def up():
    return {"status": "up"}