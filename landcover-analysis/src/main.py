from dotenv import load_dotenv
load_dotenv()
from utils.dependency_container import DependencyContainer
from services.gee_service import GEEService
from config.app_config import AppConfig
from fastapi import FastAPI
import logging
import uvicorn
from routers import satellite

app = FastAPI()

def add_dependencies():
    dependency_container = DependencyContainer()
    app_config = AppConfig()
    gee_service = GEEService(app_config)
    dependency_container.add_dependency(GEEService, gee_service)

if __name__ == '__main__':
    add_dependencies()
    app_config = AppConfig()
    app.include_router(satellite.router)
    try:
        uvicorn.run(
            app,
            host=app_config.HOST,
            port=int(app_config.PORT),
            log_level=app_config.LOG_LEVEL,
            reload=False)
    except TypeError as ex:
        logging.error(ex)