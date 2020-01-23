using Microsoft.AspNetCore.Mvc;
using RedditEmblemAPI.Services;

namespace RedditEmblemAPI.Controllers
{
    [Route("api/sheets")]
    [ApiController]
    public class SheetsController : ControllerBase
    {
        private readonly ISheetsService _sheetsService;

        public SheetsController()
        {
            this._sheetsService = new SheetsService();
        }

        [HttpGet]
        public IActionResult GetSheetsData()
        {
            var data = _sheetsService.LoadData();
            return Ok(data);
        }
    }
}