from fastapi import APIRouter



router = APIRouter(
    prefix="/satellite",
)


@router.get("/", status_code=202)
async def test():
    return {"message": "up"}
 