using AutoMapper;
using DogsHouseService.Application.Common.Mappings;
using DogsHouseService.Application.Interfaces;
using DogsHouseService.Persistence;

namespace DogsHouseService.Tests.Common
{
	public abstract class TestFixtureBase : IDisposable
	{
		public AppDbContext _context;
		public IMapper _mapper;

		public TestFixtureBase()
		{
			_context = DogsHouseServiceContextFactory.Create();
			var configurationProvider = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new AssemblyMappingProfile(
					typeof(IAppDbContext).Assembly));
			});
			_mapper = configurationProvider.CreateMapper();
		}

		public void Dispose()
		{
			DogsHouseServiceContextFactory.Destroy(_context);
		}
	}
}
