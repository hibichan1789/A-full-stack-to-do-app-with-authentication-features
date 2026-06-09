from pydantic import BaseModel

class SummaryRequest(BaseModel):
    description: str

class SummaryResponse(BaseModel):
    summary:str
