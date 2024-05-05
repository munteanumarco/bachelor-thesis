from config.app_config import AppConfig
from fastapi import FastAPI
import logging
import uvicorn
from routers import satellite

app = FastAPI()

if __name__ == '__main__':
    app.include_router(satellite.router)
    app_config = AppConfig()
    try:
        uvicorn.run(
            app,
            host=app_config.HOST,
            port=int(app_config.PORT),
            log_level=app_config.LOG_LEVEL,
            reload=False)
    except TypeError as ex:
        logging.error(ex)