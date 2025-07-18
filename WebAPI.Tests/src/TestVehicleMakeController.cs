using System.Net;
using System.Text;
using System.Text.Json;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Mono.Model;
using Mono.WebAPI.dto;
using Ninject;
using Xunit;

namespace Mono.WebAPI.Tests;

public class TestVehicleMakeController : IClassFixture<WebApplicationFactory<VehicleMakeController>>
{
    private readonly WebApplicationFactory<VehicleMakeController> _factory;
    private readonly IMapper mapper;

    public TestVehicleMakeController(WebApplicationFactory<VehicleMakeController> factory)
    {
        _factory = factory;
        var kernel = new StandardKernel();
        kernel.Load(new ServiceModule());
        mapper = kernel.Get<IMapper>();
    }

    [Fact]
    public async void GetAllMakes()
    {
        var client = _factory.CreateClient();

        var allMakes = await client.GetAsync("/api/v1.0/vehiclemake");
        allMakes.EnsureSuccessStatusCode();
        var data = await ResponseValue<List<VehicleMake>>.Parse(allMakes);

        data.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public async Task RegisterMake()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsync("/api/v1.0/vehiclemake", new StringContent(
            JsonSerializer.Serialize(new VehicleMakeCreateUpdateDto
            {
                Name = "Make 1",
                Abrv = "Abrv 1"
            }),
            Encoding.UTF8,
            "application/json"
        ));
        response.EnsureSuccessStatusCode();
        var data = await ResponseValue<VehicleMake>.Parse(response);

        data.Should().NotBeNull();

        var allMakes = await client.GetAsync("/api/v1.0/vehiclemake");
        allMakes.EnsureSuccessStatusCode();
        var allData = await ResponseValue<List<VehicleMake>>.Parse(allMakes);

        allData.Should().NotBeNull().And.ContainEquivalentOf(data);
    }

    [Fact]
    public async Task FailUpdateMake()
    {
        var client = _factory.CreateClient();
        var patchResponse = await client.PatchAsync("/api/v1.0/vehiclemake/55", new StringContent(
            JsonSerializer.Serialize(new VehicleMakeCreateUpdateDto
            {
                Name = "NOOP",
                Abrv = "NOOP"
            }),
            Encoding.UTF8,
            "application/json"
        ));
        patchResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateMake()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsync("/api/v1.0/vehiclemake", new StringContent(
            JsonSerializer.Serialize(new VehicleMakeCreateUpdateDto
            {
                Name = "Make 1",
                Abrv = "Abrv 1"
            }),
            Encoding.UTF8,
            "application/json"
        ));
        response.EnsureSuccessStatusCode();
        var data = await ResponseValue<VehicleMake>.Parse(response);

        var patchResponse = await client.PatchAsync("/api/v1.0/vehiclemake/" + data.Id, new StringContent(
            JsonSerializer.Serialize(new VehicleMakeCreateUpdateDto
            {
                Name = "Make 2",
                Abrv = "Abrv 1"
            }),
            Encoding.UTF8,
            "application/json"
        ));
        patchResponse.EnsureSuccessStatusCode();
        var patchData = await ResponseValue<VehicleMake>.Parse(patchResponse);

        patchData.Should().NotBeNull().And.BeEquivalentTo(new VehicleMake
        {
            Id = data.Id,
            Name = "Make 2",
            Abrv = "Abrv 1"
        });

        var allMakes = await client.GetAsync("/api/v1.0/vehiclemake");
        allMakes.EnsureSuccessStatusCode();
        var allData = await ResponseValue<List<VehicleMake>>.Parse(allMakes);

        allData.Should().NotBeNull().And.ContainEquivalentOf(new VehicleMake
        {
            Id = data.Id,
            Name = "Make 2",
            Abrv = "Abrv 1"
        });
    }

    [Fact]
    public async Task FailDeleteMake()
    {
        var client = _factory.CreateClient();
        var patchResponse = await client.DeleteAsync("/api/v1.0/vehiclemake/55");
        patchResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteMake()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsync("/api/v1.0/vehiclemake", new StringContent(
            JsonSerializer.Serialize(new VehicleMakeCreateUpdateDto
            {
                Name = "Make 1",
                Abrv = "Abrv 1"
            }),
            Encoding.UTF8,
            "application/json"
        ));
        response.EnsureSuccessStatusCode();
        var item = await ResponseValue<VehicleMake>.Parse(response);

        var patchResponse = await client.DeleteAsync("/api/v1.0/vehiclemake/" + item.Id);
        patchResponse.EnsureSuccessStatusCode();

        var allMakes = await client.GetAsync("/api/v1.0/vehiclemake");
        var allMakesData = await ResponseValue<List<VehicleMake>>.Parse(allMakes);
        allMakesData.Should().NotBeNull().And.BeEmpty();
    }
}