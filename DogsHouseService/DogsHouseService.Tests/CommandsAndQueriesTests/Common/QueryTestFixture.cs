using AutoMapper;
using DogsHouseService.Application.Common.Mappings;
using DogsHouseService.Application.Interfaces;
using DogsHouseService.Persistence;

namespace DogsHouseService.Tests.CommandsAndQueriesTests.Common
{
	public class QueryTestFixture : TestFixtureBase
	{
		public AppDbContext context;
		public IMapper mapper;

		public QueryTestFixture()
		{
			context = DogsHouseServiceContextFactory.Create();
			var configurationProvider = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new AssemblyMappingProfile(
					typeof(IAppDbContext).Assembly));
			});
			mapper = configurationProvider.CreateMapper();
		}

		public void Dispose()
		{
			DogsHouseServiceContextFactory.Destroy(context);
		}
	}

	[CollectionDefinition("QueryCollection")]
	public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
}
