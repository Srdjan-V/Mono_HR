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
[Route("api/v{version:apiVersion}/[controller]")]
public class VehicleOwnerController(
    IMapper mapper,
    IRepositoryFactory<VehicleOwner> makeFactory) :
    ControllerBase
{
    [HttpGet(Name = nameof(GetAllOwners))]
    public async Task<ActionResult> GetAllOwners(ApiVersion version, [FromQuery] QueryParameters queryParameters)
    {
        using var repository = makeFactory.Build();
        var query = queryParameters.Query;
        var pagedResult = await repository.FindPaged(
            queryParameters.Page,
            queryParameters.PageCount,
            make =>
                query == null ||
                make.FirstName.ToString().Contains(query, StringComparison.InvariantCultureIgnoreCase) ||
                make.FirstName.Contains(query, StringComparison.InvariantCultureIgnoreCase) ||
                make.LastName.ToString().Contains(query, StringComparison.InvariantCultureIgnoreCase) ||
                make.LastName.Contains(query, StringComparison.InvariantCultureIgnoreCase),
            Comparer<VehicleOwner>.Default //todo figure out how to build the correct sorter in c#
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
    public async Task<ActionResult> RegisterOwner(ApiVersion version, [FromBody] VehicleOwnerCreateUpdateDto updateDto)
    {
        var vehicleOwner = mapper.Map<VehicleOwnerCreateUpdateDto, VehicleOwner>(updateDto);
        using var repository = makeFactory.Build();
        var i = await repository.AddAsync(vehicleOwner);
        if (i != 1)
        {
            throw new IOException("Failed to register new make");
        }

        var ownerDto = mapper.Map<VehicleOwnerDto>(vehicleOwner);
        return Ok(ownerDto);
    }

    [HttpPatch(Name = nameof(UpdateOwner))]
    public async Task<ActionResult> UpdateOwner(ApiVersion version, [FromBody] VehicleOwnerCreateUpdateDto updateDto)
    {
        var vehicleOwner = mapper.Map<VehicleOwnerCreateUpdateDto, VehicleOwner>(updateDto);
        using var repository = makeFactory.Build();
        var updateAsync = await repository.UpdateAsync(vehicleOwner);
        var commitAsync = await repository.CommitAsync();
        if (updateAsync != 1 || commitAsync != 1)
        {
            throw new IOException("Failed to update new make");
        }

        var ownerDto = mapper.Map<VehicleOwnerDto>(vehicleOwner);
        return Ok(ownerDto);
    }

    [HttpDelete(Name = nameof(DeleteOwner))]
    public async Task<ActionResult> DeleteOwner(ApiVersion version, [FromQuery] long id)
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