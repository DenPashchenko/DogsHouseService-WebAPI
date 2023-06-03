using DogsHouseService.Application.Dogs.Queries.GetDogList;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogsHouseService.WebApi.Controllers
{
	[Route("[controller]")]
	[ApiController]
	[Produces("application/json")]
	public class Ping : ControllerBase
	{
		/// <summary>
		/// Gets a ping
		/// </summary>
		/// <returns>Returns the string with app name and version</returns>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<string> PingAction()
		{			
			return Ok("Dogs house service. Version 1.0.1");
		}
	}
}
