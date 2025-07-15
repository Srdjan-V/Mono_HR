using FluentAssertions;
using Mono.DAL;
using Mono.Model;
using Mono.Repository.Common;
using Ninject;
using Ninject.Extensions.Factory;
using Xunit;

namespace Mono.Repository.Tests;

public class OwnerRepositoryTests
{
    private KernelBase kernel;

    private IRepositoryFactory<VehicleOwner> repoFactory;

    public OwnerRepositoryTests()
    {
        kernel = new StandardKernel();
        kernel.Bind<IRepositoryFactory<VehicleOwner>>().ToFactory();
        kernel.Bind<IRepository<VehicleOwner>>().To<OwnerRepository>();

        kernel.Bind<IMonoDbContextFactory>().ToFactory();
        kernel.Bind<IMonoDbContext>().To<InMemorySqliteMonoDbContext>();

        repoFactory = kernel.Get<IRepositoryFactory<VehicleOwner>>();
    }

    [Fact]
    private async void TestCreate()
    {
        using (var repository = repoFactory.Build())
        {
            Task<int> addAsync = repository.AddAsync(new VehicleOwner
            {
                Id = 2500,
                FirstName = "First",
                LastName = "Last",
                DOB = new DateTime(2000, 1, 1)
            });

            await addAsync;
            //await repository.CommitAsync();//todo check if i need to create db tables on context init

            var data = await repository.GetAsync(2500);
            data.Should().NotBeNull();
        }
    }
}