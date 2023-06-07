using DogsHouseService.WebApi.Controllers.Abstractoins;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace DogsHouseService.WebApi.Controllers
{
	public class Ping : BaseController
	{
		/// <summary>
		/// Gets a string with this app's name and version
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
			var version = Assembly.GetExecutingAssembly().GetName().Version;
			var appName = "Dogs house service";

			return Ok($"{appName}. Version {version?.Major}.{version?.Minor}.{version?.Build}");
		}
	}
}
