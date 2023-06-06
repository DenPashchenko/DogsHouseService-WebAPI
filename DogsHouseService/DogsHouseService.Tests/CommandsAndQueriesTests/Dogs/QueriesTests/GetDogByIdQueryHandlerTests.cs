using AutoMapper;
using DogsHouseService.Application.Common.Exceptions;
using DogsHouseService.Application.Dogs.Queries.GetDogById;
using DogsHouseService.Persistence;
using DogsHouseService.Tests.Common;

namespace DogsHouseService.Tests.CommandsAndQueriesTests.Dogs.QueriesTests
{
    [Collection("QueryCollection")]
	public class GetDogByIdQueryHandlerTests
	{
		private AppDbContext _context;
		private IMapper _mapper;

		public GetDogByIdQueryHandlerTests(QueryTestFixture fixture)
		{
			_context = fixture.context;
			_mapper = fixture._mapper;
		}

		[Fact]
		public async Task GetCategoryByIdQueryHandler_ValidId_Success()
		{
			var handler = new GetDogByIdQueryHandler(_context, _mapper);
			int id = 1; // the 1st dog from initially seeded to DbContext

			var result = await handler.Handle(
				new GetDogByIdQuery
				{
					Id = id
				},
				CancellationToken.None);

			Assert.IsType<DogVm>(result);
			Assert.Equal(id, result.Id);
			Assert.Equal("Neo", result.Name);
			Assert.Equal("red&amber", result.Color);
			Assert.Equal(22, result.TailLength);
			Assert.Equal(32, result.Weight);
		}

		[Fact]
		public async Task GetCategoryByIdQueryHandler_InvalidId_NotFoundException()
		{
			var handler = new GetDogByIdQueryHandler(_context, _mapper);
			int invalidId = 0;

			await Assert.ThrowsAsync<NotFoundException>(async () =>
			   await handler.Handle(
				   new GetDogByIdQuery
				   {
					   Id = invalidId
				   },
				   CancellationToken.None));
		}
	}
}
