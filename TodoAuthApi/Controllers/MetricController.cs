using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoAuthApi.Services.SummaryService;

namespace TodoAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MetricController : ControllerBase
    {
        private readonly SummaryService _functionsSummaryService;
        private readonly DirectSummaryService _directSummaryService;
        private readonly MetricService _metricService;

        public MetricController(IConfiguration config)
        {
            _functionsSummaryService = new SummaryService(new HttpClient(), config);
            _directSummaryService = new DirectSummaryService(config);
            _metricService = new MetricService();
        }

        [HttpPost("functions")]
        public async Task<ActionResult> GenerateFunctionsSummary([FromBody]string description)
        {
            var result = await _metricService.MeasureSummaryAsync(_functionsSummaryService, description);
            return Ok(new { result });
        }

        [HttpPost("direct")]
        public async Task<ActionResult> GenerateDirectSummary([FromBody]string description)
        {
            var result = await _metricService.MeasureSummaryAsync(_directSummaryService, description);
            return Ok(new { result });
        }
    }
}
