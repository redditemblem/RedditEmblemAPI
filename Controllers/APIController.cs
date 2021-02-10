using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedditEmblemAPI.Services;
using System;
using RedditEmblemAPI.Models.Exceptions.Query;
using RedditEmblemAPI.Models.Input.Turns;

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

        [HttpGet("map/{teamName}")]
        public IActionResult GetTeamMapData(string teamName)
        {
            try
            {
                var data = _sheetsService.LoadMapData(teamName);
                return Ok(data);
            }
            catch(MapDataLockedException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet("map/{teamName}/turns")]
        public IActionResult GetTeamMapTurnData(string teamName)
        {
            try
            {
                var data = _sheetsService.LoadMapTurnData(teamName);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("map/{teamName}/turns/create")]
        public IActionResult CreateTeamMapTurn(string teamName, [FromBody] ClientTurnData postData)
        {
            int statusCode;
            try
            {
                _sheetsService.CreateTeamMapTurn(teamName, postData);
                statusCode = StatusCodes.Status200OK;
            }
            catch(Exception ex)
            {
                statusCode = StatusCodes.Status500InternalServerError;
            }

            var data = _sheetsService.LoadMapTurnData(teamName);
            return StatusCode(statusCode, data);
        }

        [HttpGet("convoy/{teamName}")]
        public IActionResult GetTeamConvoy(string teamName)
        {
            try
            {
                var data = _sheetsService.LoadConvoyData(teamName);
                return Ok(data);
            }
            catch (ConvoyNotConfiguredException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet("shop/{teamName}")]
        public IActionResult GetTeamShop(string teamName)
        {
            try
            {
                var data = _sheetsService.LoadShopData(teamName);
                return Ok(data);
            }
            catch (ShopNotConfiguredException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex);
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