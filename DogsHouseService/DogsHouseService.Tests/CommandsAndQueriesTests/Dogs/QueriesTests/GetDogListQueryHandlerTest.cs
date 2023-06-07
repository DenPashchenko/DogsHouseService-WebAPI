using AutoMapper;
using DogsHouseService.Application.Dogs.Queries.GetDogList;
using DogsHouseService.Persistence;
using System.Linq.Dynamic.Core;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using DogsHouseService.Tests.Common;

namespace DogsHouseService.Tests.CommandsAndQueriesTests.Dogs.QueriesTests
{
	[Collection("QueryCollection")]
	public class GetDogListQueryHandlerTest
	{
		private AppDbContext _context;
		private IMapper _mapper;

		public GetDogListQueryHandlerTest(TestFixture fixture)
		{
			_context = fixture.context;
			_mapper = fixture.mapper;
		}

		[Fact]
		public async Task GetCategoryListQueryHandler_EmptyQueryString_DogListVm()
		{
			var handler = new GetDogListQueryHandler(_context, _mapper);
			var dogListQuery = new GetDogListQuery()
			{
				PageNumber = 1,
				PageSize = 10
			};
			var expectedMetadata = new Dictionary<string, object>()
			{
				{ "TotalCount", 5 },
				{ "PageSize", 10 },
				{ "CurrentPage", 1 },
				{ "TotalPages", 1 },
				{ "HasNext", false },
				{ "HasPrevious", false }
			};

			var result = await handler.Handle(dogListQuery, CancellationToken.None);

			Assert.Equal(_context.Dogs.Count(), result.Dogs?.Count);
			Assert.IsType<DogListVm>(result);
			Assert.Equal("Id ascending", dogListQuery.SortingQuery);
			foreach (var expectedPair in expectedMetadata)
			{
				Assert.Contains(expectedPair.Key, result.Metadata.Keys);
				Assert.Equal(expectedPair.Value, result.Metadata[expectedPair.Key]);
			}
		}

		[Fact]
		public async Task GetCategoryListQueryHandler_QueryStringContainsLastPageNumber_DogListVmContainsOneLastDog()
		{
			var handler = new GetDogListQueryHandler(_context, _mapper);
			var dogListQuery = new GetDogListQuery()
			{
				PageNumber = 3,
				PageSize = 2
			};
			var expectedMetadata = new Dictionary<string, object>()
			{
				{ "TotalCount", 5 },
				{ "PageSize", 2 },
				{ "CurrentPage", 3 },
				{ "TotalPages", 3 },
				{ "HasNext", false },
				{ "HasPrevious", true }
			};

			var result = await handler.Handle(dogListQuery, CancellationToken.None);

			Assert.Equal(1, result.Dogs?.Count);
			Assert.Equal(_context.Dogs.Last().Id, result.Dogs?.First().Id);
			Assert.Equal(_context.Dogs.Last().Name, result.Dogs?.First().Name);
			Assert.Equal(_context.Dogs.Last().Color, result.Dogs?.First().Color);
			Assert.Equal(_context.Dogs.Last().TailLength, result.Dogs?.First().TailLength);
			Assert.Equal(_context.Dogs.Last().Weight, result.Dogs?.First().Weight);

			foreach (var expectedPair in expectedMetadata)
			{
				Assert.Contains(expectedPair.Key, result.Metadata.Keys);
				Assert.Equal(expectedPair.Value, result.Metadata[expectedPair.Key]);
			}
		}

		[Fact]
		public async Task GetCategoryListQueryHandler_QueryStringContainsPageNumberAndPageSize_DogListVmContainsTwotDogs()
		{
			var handler = new GetDogListQueryHandler(_context, _mapper);
			var dogListQuery = new GetDogListQuery()
			{
				PageNumber = 2,
				PageSize = 2
			};
			var expectedMetadata = new Dictionary<string, object>()
			{
				{ "TotalCount", 5 },
				{ "PageSize", 2 },
				{ "CurrentPage", 2 },
				{ "TotalPages", 3 },
				{ "HasNext", true },
				{ "HasPrevious", true }
			};

			var result = await handler.Handle(dogListQuery, CancellationToken.None);

			Assert.Equal(2, result.Dogs?.Count);
			foreach (var expectedPair in expectedMetadata)
			{
				Assert.Contains(expectedPair.Key, result.Metadata.Keys);
				Assert.Equal(expectedPair.Value, result.Metadata[expectedPair.Key]);
			}
		}

		[Fact]
		public async Task GetCategoryListQueryHandler_QueryStringContainsNonExistentPageNumber_DogListVmContainsEmptyDogList()
		{
			var handler = new GetDogListQueryHandler(_context, _mapper);
			var dogListQuery = new GetDogListQuery()
			{
				PageNumber = 10,
				PageSize = 5
			};
			var expectedMetadata = new Dictionary<string, object>()
			{
				{ "TotalCount", 5 },
				{ "PageSize", 5 },
				{ "CurrentPage", 10 },
				{ "TotalPages", 1 },
				{ "HasNext", false },
				{ "HasPrevious", true }
			};

			var result = await handler.Handle(dogListQuery, CancellationToken.None);

			Assert.Empty(result.Dogs);
			foreach (var expectedPair in expectedMetadata)
			{
				Assert.Contains(expectedPair.Key, result.Metadata.Keys);
				Assert.Equal(expectedPair.Value, result.Metadata[expectedPair.Key]);
			}
		}

		[Theory]
		[InlineData("Name", "desc", "descending")]
		[InlineData("TailLength", "descending", "descending")]
		[InlineData("Id", "anyWord", "ascending")]
		[InlineData("Color", "asc", "ascending")]
		[InlineData("Weight", "desc", "descending")]
		public async Task GetCategoryListQueryHandler_QueryStringContainsAttributeNameAndOrderDescending_ProperlySortedDogListVm(
			string attribute, string initialOrderValue, string expectedOrderValue)
		{
			var handler = new GetDogListQueryHandler(_context, _mapper);
			var dogListQuery = new GetDogListQuery()
			{
				PageNumber = 1,
				PageSize = 10,
				Order = initialOrderValue,
				Attribute = attribute
			};
			var expectedDogs = await _context.Dogs.OrderBy(dogListQuery.SortingQuery)
				.ProjectTo<DogListDto>(_mapper.ConfigurationProvider)
				.ToListAsync();

			var result = await handler.Handle(dogListQuery, CancellationToken.None);

			Assert.Equal($"{attribute} {expectedOrderValue}", dogListQuery.SortingQuery);
			Assert.Equal(_context.Dogs.Count(), result.Dogs?.Count);
			Assert.Equal(expectedDogs, result.Dogs?.ToList());

		}
	}
}
