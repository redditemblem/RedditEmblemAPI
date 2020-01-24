using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedditEmblemAPI.Services;
using System;

namespace RedditEmblemAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly IAPIService _sheetsService;

        public APIController()
        {
            this._sheetsService = new APIService();
        }

        [HttpGet("team/{teamName}")]
        public IActionResult GetSheetsData(string teamName)
        {
            try
            {
                var data = _sheetsService.LoadData(teamName);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet("teamList")]
        public IActionResult GetTeamsList()
        {
            try
            {
                var data = _sheetsService.LoadTeamList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}