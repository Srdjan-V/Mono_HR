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
}