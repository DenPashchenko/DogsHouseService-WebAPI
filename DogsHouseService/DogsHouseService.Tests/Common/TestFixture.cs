using AutoMapper;
using DogsHouseService.Application.Common.Mappings;
using DogsHouseService.Application.Interfaces;
using DogsHouseService.Persistence;

namespace DogsHouseService.Tests.Common
{
	public class TestFixture : IDisposable
	{
		public AppDbContext context;
		public IMapper mapper;

		public TestFixture()
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
	public class QueryCollection : ICollectionFixture<TestFixture> { }
}
