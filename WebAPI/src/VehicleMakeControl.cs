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
public class VehicleMakeControl(
    IMapper mapper,
    IRepositoryFactory<VehicleMake> makeFactory) :
    ControllerBase
{
    [HttpGet(Name = nameof(GetAllMakes))]
    public async Task<ActionResult> GetAllMakes(ApiVersion version, [FromQuery] QueryParameters queryParameters)
    {
        using var repository = makeFactory.Build();
        var query = queryParameters.Query;
        var pagedResult = await repository.FindPaged(
            queryParameters.Page,
            queryParameters.PageCount,
            make =>
                query == null ||
                make.Name.ToString().Contains(query, StringComparison.InvariantCultureIgnoreCase) ||
                make.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase) ||
                make.Abrv.ToString().Contains(query, StringComparison.InvariantCultureIgnoreCase) ||
                make.Abrv.Contains(query, StringComparison.InvariantCultureIgnoreCase),
            Comparer<VehicleMake>.Default //todo figure out how to build the correct sorter in c#
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
    public async Task<ActionResult> RegisterMake(ApiVersion version, [FromBody] VehicleMakeCreateUpdateDto updateDto)
    {
        var vehicleMake = mapper.Map<VehicleMakeCreateUpdateDto, VehicleMake>(updateDto);
        using var repository = makeFactory.Build();
        var i = await repository.AddAsync(vehicleMake);
        if (i != 1)
        {
            throw new IOException("Failed to register new make");
        }

        var vehicleMakeDto = mapper.Map<VehicleMakeDto>(vehicleMake);
        return Ok(vehicleMakeDto);
    }

    [HttpPatch(Name = nameof(UpdateMake))]
    public async Task<ActionResult> UpdateMake(ApiVersion version, [FromBody] VehicleMakeCreateUpdateDto updateDto)
    {
        var vehicleMake = mapper.Map<VehicleMakeCreateUpdateDto, VehicleMake>(updateDto);
        using var repository = makeFactory.Build();
        var updateAsync = await repository.UpdateAsync(vehicleMake);
        var commitAsync = await repository.CommitAsync();
        if (updateAsync != 1 || commitAsync != 1)
        {
            throw new IOException("Failed to update new make");
        }

        var vehicleMakeDto = mapper.Map<VehicleMakeDto>(vehicleMake);
        return Ok(vehicleMakeDto);
    }

    [HttpDelete(Name = nameof(DeleteMake))]
    public async Task<ActionResult> DeleteMake(ApiVersion version, [FromQuery] long id)
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