using System.Collections;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mono.Model;
using Mono.Repository.Common;
using Mono.WebAPI.dto;

namespace Mono.WebAPI;

[ApiVersion("1.0")]
[Route("api/v{version}/[controller]")]
public class VehicleEngineTypeController(
    IMapper mapper,
    IRepositoryFactory<VehicleEngineType> engineFactory) :
    ControllerBase
{
    [HttpGet(Name = nameof(GetAllTypes))]
    public async Task<ActionResult> GetAllTypes()
    {
        using var repository = engineFactory.Build();
        var all = await repository.FindAll();
        var data = new ArrayList();
        foreach (var item in all)
        {
            var dto = mapper.Map<VehicleEngineType, VehicleEngineTypeDto>(item);
            data.Add(dto);
        }

        return Ok(new
        {
            value = data
        });
    }
}