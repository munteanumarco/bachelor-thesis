from dotenv import load_dotenv


load_dotenv()
from ai_model.road_segmentation_model import RoadSegmentationModel
from utils.dependency_container import DependencyContainer
from services.road_segmentation_service import RoadSegmentationService
from services.gee_service import GEEService
from utils.model_config import ModelConfig
from config.app_config import AppConfig
from fastapi import FastAPI
import logging
import uvicorn
from routers import satellite
from routers import analysis

app = FastAPI()

def add_dependencies():
    dependency_container = DependencyContainer()
    app_config = AppConfig()
    gee_service = GEEService(app_config)
    model_config = ModelConfig(encoder='resnet50', encoder_weights='imagenet', classes=2, activation='sigmoid')
    road_segmentation_model = RoadSegmentationModel(app_config.MODEL_PATH, model_config, app_config.RAW_DATA_FOLDER_PATH, app_config.PROCESSED_DATA_FOLDER_PATH)
    road_segmentation_service = RoadSegmentationService(road_segmentation_model)
    dependency_container.add_dependency(GEEService, gee_service)
    dependency_container.add_dependency(RoadSegmentationService, road_segmentation_service)

if __name__ == '__main__':
    add_dependencies()
    app_config = AppConfig()
    app.include_router(satellite.router)
    app.include_router(analysis.router)
    try:
        uvicorn.run(
            app,
            host=app_config.HOST,
            port=int(app_config.PORT),
            log_level=app_config.LOG_LEVEL,
            reload=False)
    except TypeError as ex:
        logging.error(ex)