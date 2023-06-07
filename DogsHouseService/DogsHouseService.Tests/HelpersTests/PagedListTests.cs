using AutoMapper.QueryableExtensions;
using DogsHouseService.Application.Common.Helpers;
using DogsHouseService.Application.Dogs.Queries.GetDogList;
using DogsHouseService.Tests.Common;

namespace DogsHouseService.Tests.HelpersTests
{
    public class PagedListTests : TestFixture
	{
		[Fact]
		public void PagedList_Initialization_SetsPropertiesCorrectly()
		{
			var items = new List<string> { "Item 1", "Item 2", "Item 3" };
			var count = 3;
			var pageNumber = 1;
			var pageSize = 10;

			var pagedList = new PagedList<string>(items, count, pageNumber, pageSize);

			Assert.Equal(count, pagedList.TotalCount);
			Assert.Equal(pageSize, pagedList.PageSize);
			Assert.Equal(pageNumber, pagedList.CurrentPage);
			Assert.Equal(1, pagedList.TotalPages);
			Assert.False(pagedList.HasNext);
			Assert.False(pagedList.HasPrevious);
			Assert.Equal(items, pagedList);
		}

		[Fact]
		public async Task ToPagedListAsync_SecondPageOutOfThree_ReturnsPagedListWithTwoDogs()
		{
			var items = context.Dogs
				.ProjectTo<DogListDto>(mapper.ConfigurationProvider)
				.Select(d => new DogListDto
				{
					Id = d.Id,
					Name = d.Name,
					Color = d.Color,
					TailLength = d.TailLength,
					Weight = d.Weight
				});
			// Now we have 5 dogs (3 in DogsHouseServiceContextFactory + 2 initially seeded)
			var pageNumber = 2;
			var pageSize = 2;
			var expectedResult = context.Dogs
				.ProjectTo<DogListDto>(mapper.ConfigurationProvider)
				.Where(d => d.Id == 3 || d.Id == 4);

			var pagedList = await PagedList<DogListDto>.ToPagedListAsync(items, pageNumber, pageSize);

			Assert.Equal(context.Dogs.Count(), pagedList.TotalCount);
			Assert.Equal(pageSize, pagedList.PageSize);
			Assert.Equal(pageNumber, pagedList.CurrentPage);
			Assert.Equal(3, pagedList.TotalPages);
			Assert.True(pagedList.HasNext);
			Assert.True(pagedList.HasPrevious);
			Assert.Equal(expectedResult.Count(), pagedList.Count);
			Assert.Equal(expectedResult.ToList(), pagedList.ToList());

		}

		[Fact]
		public async Task ToPagedListAsync_ThirdPageOutOfThree_ReturnsPagedListWithOneDog()
		{
			var items = context.Dogs
				.ProjectTo<DogListDto>(mapper.ConfigurationProvider)
				.Select(d => new DogListDto
				{
					Id = d.Id,
					Name = d.Name,
					Color = d.Color,
					TailLength = d.TailLength,
					Weight = d.Weight
				});
			// Now we have 5 dogs (3 in DogsHouseServiceContextFactory + 2 initially seeded)
			var pageNumber = 3;
			var pageSize = 2;
			var expectedResult = context.Dogs
				.ProjectTo<DogListDto>(mapper.ConfigurationProvider)
				.Where(d => d.Id == 5);

			var pagedList = await PagedList<DogListDto>.ToPagedListAsync(items, pageNumber, pageSize);

			Assert.False(pagedList.HasNext);
			Assert.True(pagedList.HasPrevious);
			Assert.Equal(expectedResult.Count(), pagedList.Count);
			Assert.Equal(expectedResult.ToList(), pagedList.ToList());

		}
	}
}
