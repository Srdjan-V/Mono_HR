using System.Collections;
using System.Text.Json;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mono.Model;
using Mono.Repository.Common;
using Mono.WebAPI.dto;

namespace Mono.WebAPI;

[ApiVersion("1.0")]
[Route("api/v{version}/[controller]")]
public class VehicleMakeController(
    IMapper mapper,
    IRepositoryFactory<VehicleMake> makeFactory) :
    ControllerBase
{
    [HttpGet(Name = nameof(GetAllMakes))]
    public async Task<ActionResult> GetAllMakes([FromQuery] QueryParameters queryParameters)
    {
        using var repository = makeFactory.Build();
        var query = queryParameters.Query;
        Func<VehicleMake, bool>? filter = string.IsNullOrEmpty(query)
            ? null
            : make => make.Name.ToString().Contains(query, StringComparison.InvariantCultureIgnoreCase) ||
                      make.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase) ||
                      make.Abrv.ToString().Contains(query, StringComparison.InvariantCultureIgnoreCase) ||
                      make.Abrv.Contains(query, StringComparison.InvariantCultureIgnoreCase);

        var sorter = queryParameters.CreateComparer<VehicleMake>([
            typeof(VehicleMake).GetProperty(nameof(VehicleMake.Name)),
            typeof(VehicleMake).GetProperty(nameof(VehicleMake.Abrv))
        ], typeof(VehicleMake).GetProperty(nameof(VehicleMake.Name)));

        var pagedResult = await repository.FindPaged(
            queryParameters.Page,
            queryParameters.PageCount,
            filter,
            sorter
        );

        var allItemCount = await repository.CountAsync();

        var paginationMetadata = new
        {
            totalCount = allItemCount,
            pageSize = queryParameters.PageCount,
            currentPage = queryParameters.Page,
            totalPages = queryParameters.GetTotalPages(allItemCount)
        };

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

        var data = new ArrayList();
        foreach (var item in pagedResult.Items)
        {
            var dto = mapper.Map<VehicleMake, VehicleMakeDto>(item);
            data.Add(dto);
        }

        return Ok(new
        {
            value = data,
        });
    }

    [HttpPost(Name = nameof(RegisterMake))]
    public async Task<ActionResult> RegisterMake([FromBody] VehicleMakeCreateUpdateDto updateDto)
    {
        var vehicleMake = mapper.Map<VehicleMakeCreateUpdateDto, VehicleMake>(updateDto);
        using var repository = makeFactory.Build();
        var addAsync = await repository.AddAsync(vehicleMake);
        var commitAsync = await repository.CommitAsync();
        if (addAsync != 1 || commitAsync != 1)
        {
            throw new IOException("Failed to register new make");
        }

        var vehicleMakeDto = mapper.Map<VehicleMakeDto>(vehicleMake);
        return Ok(vehicleMakeDto);
    }

    [HttpPatch("{id:long}", Name = nameof(UpdateMake))]
    public async Task<ActionResult> UpdateMake(long id, [FromBody] VehicleMakeCreateUpdateDto updateDto)
    {
        using var repository = makeFactory.Build();
        var existingMake = await repository.GetAsync(id);
        if (existingMake == null)
        {
            return NotFound();
        }

        var updatedMake = mapper.Map(updateDto, existingMake);
        var updateAsync = await repository.UpdateAsync(updatedMake);
        var commitAsync = await repository.CommitAsync();
        if (updateAsync != 1 || commitAsync != 1)
        {
            throw new IOException("Failed to update new make");
        }

        var vehicleMakeDto = mapper.Map<VehicleMakeDto>(updatedMake);
        return Ok(vehicleMakeDto);
    }

    [HttpDelete(Name = nameof(DeleteMake))]
    public async Task<ActionResult> DeleteMake([FromQuery] long id)
    {
        using var repository = makeFactory.Build();
        var vehicleMake = await repository.GetAsync(id);
        if (vehicleMake == null)
        {
            return NotFound();
        }

        var makeDto = mapper.Map<VehicleMakeDto>(vehicleMake);
        return Ok(makeDto);
    }
}