using DogsHouseService.WebApi.Controllers.Abstractoins;
using Microsoft.AspNetCore.Mvc;

namespace DogsHouseService.WebApi.Controllers
{
	public class Ping : BaseController
	{
		/// <summary>
		/// Gets a ping
		/// </summary>
		/// <remarks>
		/// Sample request: GET /ping
		/// </remarks>
		/// <returns>Returns a string with this app's name and version</returns>
		/// <response code="429">Too Many Requests</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<string> GetPing()
		{			
			return Ok("Dogs house service. Version 1.0.1");
		}
	}
}
