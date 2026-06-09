import azure.functions as func
import logging
from pydantic import ValidationError
import json

from models.summary_type import SummaryRequest, SummaryResponse

# あとでsummarize関数をopenAI連携取れるように改良する
def summarize(description:str)->str:
    if len(description) <= 30:
        return description
    return description[:30]

app = func.FunctionApp(http_auth_level=func.AuthLevel.FUNCTION)



@app.route(route="summary", methods=["POST"])
def summary(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    try:
        body_bytes = req.get_body()
        body = json.loads(body_bytes)

        summary_request = SummaryRequest(**body)
        description = summary_request.description

        summary_text = summarize(description)

        summary_response = SummaryResponse(summary=summary_text)

        return func.HttpResponse(
            summary_response.model_dump_json(ensure_ascii=False),
            mimetype="application/json",
            status_code=200
        )
    except ValidationError as e:
        return func.HttpResponse(
            json.dumps({"error": "Invalid request", "details": e.errors()}),
            mimetype="application/json",
            status_code=400
        )
    except Exception as e:
        logging.error(f"Unexpected error : {e}")
        return func.HttpResponse(
            json.dumps({"error": "Internal server error"}),
            mimetype="application/json",
            status_code=500
        )