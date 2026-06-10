import azure.functions as func
import logging
from pydantic import ValidationError
import json

from models.summary_type import SummaryRequest, SummaryResponse
from services.summary_service import summarize


app = func.FunctionApp(http_auth_level=func.AuthLevel.FUNCTION)


@app.route(route="summary", methods=["POST"])
def summary(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    try:
        try:
            body = req.get_json()
        except ValueError:
            # JSON と認識されなかった場合、body を手動で読む
            raw = req.get_body()
            body = json.loads(raw.decode("utf-8"))

        summary_request = SummaryRequest(**body)
        description = summary_request.description

        summary_text = summarize(description)
        logging.info(f"summary_text: {summary_text}")

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