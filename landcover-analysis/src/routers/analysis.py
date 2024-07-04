from fastapi import APIRouter, Depends, HTTPException, WebSocket, WebSocketDisconnect, status, BackgroundTasks
from services.landcover_service import LandCoverService
from models.StartLandCoverAnalysisRequest import StartLandcoverAnalysisRequest
from services.gee_service import GEEService
from services.orchestrator_service import OrchestratorService
from services.websocket_manager import WebSocketManager
from utils.dependency_container import DependencyContainer
from models.LandCoverAnalysisStatus import LandCoverAnalysisStatus

router = APIRouter(
    prefix="/analysis",
)

def get_landcover_service() -> LandCoverService:
    return DependencyContainer().get_dependency(LandCoverService)


def get_orchestrator_service() -> OrchestratorService:
    return DependencyContainer().get_dependency(OrchestratorService)

def get_websocket_manager() -> WebSocketManager:
    return DependencyContainer().get_dependency(WebSocketManager)

@router.get("/up")
async def up():
    return {"status": "up"}


@router.websocket("/ws/{client_id}")
async def websocket_endpoint(websocket: WebSocket, client_id: str, ws_manager: WebSocketManager = Depends(get_websocket_manager)):
    await ws_manager.connect(websocket, client_id)
    try:
        while True:
            data = await websocket.receive_text()
            await ws_manager.send_message(client_id, f"Echo: {data}")
    except WebSocketDisconnect:
        pass

@router.get("/{emergency_event_id}")
async def get_landcover_analysis(emergency_event_id: str, landcover_service: LandCoverService = Depends(get_landcover_service)):
    landcover_analysis = await landcover_service.get_landcover_analysis(emergency_event_id)
    if landcover_analysis is not None:
        return landcover_analysis
    raise HTTPException(status_code=404, detail="Landcover analysis not found")


@router.post("/{emergency_event_id}", status_code=status.HTTP_202_ACCEPTED)
async def trigger_landcover_analysis(
    emergency_event_id: str,
    request: StartLandcoverAnalysisRequest,
    background_tasks: BackgroundTasks,
    landcover_service: LandCoverService = Depends(get_landcover_service),
    orchestrator_service: OrchestratorService = Depends(
        get_orchestrator_service),
):
    background_tasks.add_task(
        orchestrator_service.process_analysis,
        emergency_event_id,
        request.client_id,
        request.latitude,
        request.longitude
    )
    await landcover_service.update_landcover_analysis_status(emergency_event_id, LandCoverAnalysisStatus.IN_PROGRESS)
    return {"message": "The landcover analysis has been triggered"}
