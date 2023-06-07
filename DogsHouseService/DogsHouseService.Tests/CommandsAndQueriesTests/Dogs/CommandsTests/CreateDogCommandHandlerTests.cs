using DogsHouseService.Application.Dogs.Commands;
using DogsHouseService.Tests.Common;
using Microsoft.EntityFrameworkCore;

namespace DogsHouseService.Tests.CommandsAndQueriesTests.Dogs.CommandsTests
{
	public class CreateDogCommandHandlerTests : TestFixture
	{
		[Fact]
		public async Task CreateDogCommandHandler_ValidData_Success()
		{
			var handler = new CreateDogCommandHandler(context, mapper);
			var newDogId = 6; // Now we have 5 dogs (3 in DogsHouseServiceContextFactory + 2 initially seeded)

			var result = await handler.Handle(
				new CreateDogCommand
				{
					Name = "Name6",
					Color = "color6",
					TailLength = 6,
					Weight = 60
				},
				CancellationToken.None);

			Assert.NotNull(await context.Dogs.SingleOrDefaultAsync(dog =>
				dog.Id == 6 &&
				dog.Name == "Name6" &&
				dog.Color == "color6" &&
				dog.TailLength == 6 &&
				dog.Weight == 60));
			Assert.Equal(newDogId, result);
			Assert.IsType<int>(result);
		}
	}
}
