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
public class VehicleOwnerController(
    IMapper mapper,
    IRepositoryFactory<VehicleOwner> makeFactory) :
    ControllerBase
{
    [HttpGet(Name = nameof(GetAllOwners))]
    public async Task<ActionResult> GetAllOwners([FromQuery] QueryParameters queryParameters)
    {
        using var repository = makeFactory.Build();
        var query = queryParameters.Query;
        Func<VehicleOwner, bool>? filter = string.IsNullOrEmpty(query)
            ? null
            : make => make.FirstName.ToString().Contains(query, StringComparison.InvariantCultureIgnoreCase) ||
                      make.FirstName.Contains(query, StringComparison.InvariantCultureIgnoreCase) ||
                      make.LastName.ToString().Contains(query, StringComparison.InvariantCultureIgnoreCase) ||
                      make.LastName.Contains(query, StringComparison.InvariantCultureIgnoreCase);

        var sorter = queryParameters.CreateComparer<VehicleOwner>([
            typeof(VehicleOwner).GetProperty(nameof(VehicleOwner.FirstName)),
            typeof(VehicleOwner).GetProperty(nameof(VehicleOwner.LastName))
        ], typeof(VehicleOwner).GetProperty(nameof(VehicleOwner.FirstName)));

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
            var dto = mapper.Map<VehicleOwner, VehicleOwnerDto>(item);
            data.Add(dto);
        }

        return Ok(new
        {
            value = data,
        });
    }

    [HttpPost(Name = nameof(RegisterOwner))]
    public async Task<ActionResult> RegisterOwner([FromBody] VehicleOwnerCreateUpdateDto updateDto)
    {
        var vehicleOwner = mapper.Map<VehicleOwnerCreateUpdateDto, VehicleOwner>(updateDto);
        using var repository = makeFactory.Build();
        var addAsync = await repository.AddAsync(vehicleOwner);
        var commitAsync = await repository.CommitAsync();
        if (addAsync != 1 || commitAsync != 1)
        {
            throw new IOException("Failed to register new make");
        }

        var ownerDto = mapper.Map<VehicleOwnerDto>(vehicleOwner);
        return Ok(ownerDto);
    }

    [HttpPatch("{id:long}", Name = nameof(UpdateOwner))]
    public async Task<ActionResult> UpdateOwner(long id, [FromBody] VehicleOwnerCreateUpdateDto updateDto)
    {
        using var repository = makeFactory.Build();
        var existingModel = await repository.GetAsync(id);
        if (existingModel == null)
        {
            return NotFound();
        }

        var updateOwner = mapper.Map(updateDto, existingModel);
        var updateAsync = await repository.UpdateAsync(updateOwner);
        var commitAsync = await repository.CommitAsync();
        if (updateAsync != 1 || commitAsync != 1)
        {
            throw new IOException("Failed to update new make");
        }

        var ownerDto = mapper.Map<VehicleOwnerDto>(updateOwner);
        return Ok(ownerDto);
    }

    [HttpDelete(Name = nameof(DeleteOwner))]
    public async Task<ActionResult> DeleteOwner([FromQuery] long id)
    {
        using var repository = makeFactory.Build();
        var vehicleOwner = await repository.GetAsync(id);
        if (vehicleOwner == null)
        {
            return NotFound();
        }

        var ownerDto = mapper.Map<VehicleOwnerDto>(vehicleOwner);
        return Ok(ownerDto);
    }
}