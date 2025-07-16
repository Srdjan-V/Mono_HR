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
    [HttpGet(Name = nameof(GetAllModels))]
    public async Task<ActionResult> GetAllModels(ApiVersion version, [FromQuery] QueryParameters queryParameters)
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
}