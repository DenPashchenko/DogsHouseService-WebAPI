using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DogsHouseService.WebApi.Controllers.Abstractoins
{	
	[ApiController]
	[Route("[controller]")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status429TooManyRequests)]
	public abstract class BaseController : ControllerBase
	{
		private IMediator? _mediator;

		protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
