using FluentAssertions;
using Mono.DAL;
using Mono.Model;
using Mono.Model.Common;
using Mono.Repository.Common;
using Ninject;
using Ninject.Extensions.Factory;
using Xunit;

namespace Mono.Repository.Tests;

//todo add test for other repo types? 
public class GenericRepositoryTests
{
    private IRepositoryFactory<T> Factory<T, R>() where T : IBaseEntity where R : IRepository<T>
    {
        KernelBase kernel = new StandardKernel();
        kernel.Bind<IRepositoryFactory<T>>().ToFactory();
        kernel.Bind<IRepository<T>>().To<R>();

        kernel.Bind<IMonoDbContextFactory>().ToFactory();
        kernel.Bind<IMonoDbContext>().To<InMemorySqliteMonoDbContext>();

        return kernel.Get<IRepositoryFactory<T>>();
    }

    [Fact]
    private async void TestOwnerCreate()
    {
        using var repository = Factory<VehicleOwner, OwnerRepository>().Build();
        await repository.AddAsync(new VehicleOwner
        {
            Id = 2500,
            FirstName = "First",
            LastName = "Last",
            DOB = new DateTime(2000, 1, 1)
        });
        await repository.CommitAsync();

        var data = await repository.GetAsync(2500);
        data.Should().NotBeNull();
        data.Id.Should().Be(2500);
        data.FirstName.Should().Be("First");
        data.LastName.Should().Be("Last");
    }

    [Fact]
    private async void TestOwnerDeleateCreate()
    {
        using var repository = Factory<VehicleOwner, OwnerRepository>().Build();
        await repository.AddAsync(new VehicleOwner
        {
            Id = 2500,
            FirstName = "First",
            LastName = "Last",
            DOB = new DateTime(2000, 1, 1)
        });
        await repository.CommitAsync();

        var data = await repository.GetAsync(2500);
        data.Should().NotBeNull();
        data.Id.Should().Be(2500);
        data.FirstName.Should().Be("First");
        data.LastName.Should().Be("Last");

        var deleteAsync = await repository.DeleteAsync(2500);
        deleteAsync.Should().Be(1);
    }

    [Fact]
    private async void TestOwnerUpdateCreate()
    {
        using var repository = Factory<VehicleOwner, OwnerRepository>().Build();
        await repository.AddAsync(new VehicleOwner
        {
            Id = 2500,
            FirstName = "First",
            LastName = "Last",
            DOB = new DateTime(2000, 1, 1)
        });
        await repository.CommitAsync();

        await repository.UpdateAsync(new VehicleOwner
        {
            Id = 2500,
            FirstName = "First_Updated",
            LastName = "Last_Updated",
            DOB = new DateTime(2020, 1, 1)
        });
        await repository.CommitAsync();

        var data = await repository.GetAsync(2500);

        data.Should().NotBeNull();
        data.Id.Should().Be(2500);
        data.FirstName.Should().Be("First_Updated");
        data.LastName.Should().Be("Last_Updated");
        data.DOB.Should().Be(new DateTime(2020, 1, 1));
    }

    [Fact]
    private async void TestOwnerPagedFindCreate()
    {
        using var repository = Factory<VehicleOwner, OwnerRepository>().Build();

        for (int i = 1; i < 80; i++)
        {
            await repository.AddAsync(new VehicleOwner
            {
                Id = i,
                FirstName = "NAME_" + i,
                LastName = "LAST_" + i,
                DOB = new DateTime(2000, 1, 1)
            });
            await repository.CommitAsync();
        }

        var paged = await repository.FindPaged(
            2,
            20,
            null,
            null
        );

        paged.Should().NotBeNull();
        paged.Items.Should().NotBeNull();
        paged.Items.Should().HaveCount(20);

        paged.Items[0].Id.Should().Be(21);
    }
}