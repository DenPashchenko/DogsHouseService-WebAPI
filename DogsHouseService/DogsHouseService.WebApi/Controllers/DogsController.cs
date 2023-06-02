﻿using AutoMapper;
using DogsHouseService.Application.Dogs.Commands;
using DogsHouseService.Application.Dogs.Queries.GetDogById;
using DogsHouseService.Application.Dogs.Queries.GetDogList;
using DogsHouseService.WebApi.Controllers.Abstractions;
using DogsHouseService.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogsHouseService.WebApi.Controllers
{
	[Route("[controller]")]
	[Produces("application/json")]
	public class DogsController : BaseController
	{
		private readonly IMapper _mapper;

		public DogsController(IMapper mapper) => _mapper = mapper;

		/// <summary>
		/// Gets the list of dogs
		/// </summary>
		/// <returns>Returns DogListVm</returns>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<DogListVm>> GetAllAsync()
		{
			var query = new GetDogListQuery();
			var viewModel = await Mediator.Send(query);

			return Ok(viewModel);
		}

		/// <summary>
		/// Gets a dog by id
		/// </summary>
		/// <param name="id">Dog id (int)</param>
		/// <returns>Returns DogVm</returns>
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
		/// <param name="createDogDto">CreateDogDto object</param>
		/// <returns>Returns id (int)</returns>
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
