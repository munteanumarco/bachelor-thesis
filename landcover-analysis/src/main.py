from dotenv import load_dotenv


load_dotenv()

from services.landcover_service import LandCoverService
from services.cdn_service import CdnService
from services.orchestrator_service import OrchestratorService
from services.websocket_manager import WebSocketManager
from ai_model.road_segmentation_model import RoadSegmentationModel
from utils.dependency_container import DependencyContainer
from services.road_segmentation_service import RoadSegmentationService
from services.gee_service import GEEService
from utils.model_config import ModelConfig
from config.app_config import AppConfig
from sqlalchemy.ext.asyncio import create_async_engine, AsyncSession
from sqlalchemy.orm import sessionmaker
from fastapi import FastAPI
import logging
import uvicorn
from routers import analysis
from fastapi.middleware.cors import CORSMiddleware

app = FastAPI()

origins = [
    "http://localhost:4200",
]

app.add_middleware(
    CORSMiddleware,
    allow_origins=origins,
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

def add_dependencies():
    dependency_container = DependencyContainer()
    app_config = AppConfig()
    gee_service = GEEService(app_config)
    model_config = ModelConfig(encoder='resnet50', encoder_weights='imagenet', classes=2, activation='sigmoid')
    road_segmentation_model = RoadSegmentationModel(app_config.MODEL_PATH, model_config, app_config.RAW_DATA_FOLDER_PATH, app_config.PROCESSED_DATA_FOLDER_PATH)
    road_segmentation_service = RoadSegmentationService(road_segmentation_model)
    dependency_container.add_dependency(GEEService, gee_service)
    dependency_container.add_dependency(RoadSegmentationService, road_segmentation_service)
    DATABASE_URL = app_config.DATABASE_URL
    engine = create_async_engine(DATABASE_URL, echo=True)
    async_session = sessionmaker(
        engine,
        expire_on_commit=False,
        class_=AsyncSession
    )
    landcover_service = LandCoverService(async_session)
    dependency_container.add_dependency(LandCoverService, landcover_service)
    cdn_service = CdnService(app_config)
    websocket_manager = WebSocketManager()
    dependency_container.add_dependency(WebSocketManager, websocket_manager)
    orchestrator_service = OrchestratorService(landcover_service, gee_service, cdn_service, road_segmentation_service, websocket_manager)
    dependency_container.add_dependency(OrchestratorService, orchestrator_service)


if __name__ == '__main__':
    add_dependencies()
    app_config = AppConfig()
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