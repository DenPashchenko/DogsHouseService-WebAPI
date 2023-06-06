using AutoMapper;
using DogsHouseService.Application.Dogs.Commands;
using DogsHouseService.Application.Dogs.Queries.GetDogById;
using DogsHouseService.Application.Dogs.Queries.GetDogList;
using DogsHouseService.WebApi.Controllers.Abstractoins;
using DogsHouseService.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DogsHouseService.WebApi.Controllers
{
	public class DogsController : BaseController
	{
		private readonly IMapper _mapper;

		public DogsController(IMapper mapper) => _mapper = mapper;

		/// <summary>
		/// Gets the list of dogs. Supports sorting and pagination
		/// </summary>
		/// <remarks>
		/// Sample requests:
		///  - GET /dogs
		///  - GET /dogs?attribute=name&amp;order=desc&amp;pageNumber=2&amp;pageSize=3
		/// </remarks>
		/// <returns>
		/// Returns DogListVm
		/// Returns metadata for a pagination in the response's header
		/// </returns>
		/// <response code="429">Too Many Requests</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<IList<DogListDto>>> GetAllAsync([FromQuery] DogListQueryParameters dogListQueryParameters)
		{
			var query = _mapper.Map<GetDogListQuery>(dogListQueryParameters);
			var viewModel = await Mediator.Send(query);

			Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(viewModel.Metadata));

			return Ok(viewModel.Dogs);
		}

		/// <summary>
		/// Gets a dog by id
		/// </summary>
		/// <remarks>
		/// Sample request: GET /dogs/3
		/// </remarks>
		/// <param name="id">Dog id (int)</param>
		/// <returns>Returns DogVm</returns>
		/// <response code="429">Too Many Requests</response>
		[HttpGet("{id}")]
		[ActionName("GetByIdAsync")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<DogVm>> GetByIdAsync(int id)
		{
			var query = new GetDogByIdQuery
			{
				Id = id
			};
			var viewModel = await Mediator.Send(query);
			return Ok(viewModel);
		}

		/// <summary>
		/// Creates a new dog
		/// </summary>
		/// <remarks>
		/// Sample request:
		/// POST /dogs
		/// {
		///		name: "dog's name",
		///		color: "dog's color",
		///		tail_length: dog's tail length,
		///		weight: dog's weight
		/// }
		/// </remarks>
		/// <param name="createDogDto">CreateDogDto object</param>
		/// <returns>Returns id (int)</returns>
		/// <response code="429">Too Many Requests</response>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<int>> CreateAsync([FromBody] CreateDogDto createDogDto)
		{
			var command = _mapper.Map<CreateDogCommand>(createDogDto);
			var dogId = await Mediator.Send(command);

			return CreatedAtAction(nameof(GetByIdAsync), new { id = dogId }, dogId);//?version={createdResource.Version}
		}
	}
}
