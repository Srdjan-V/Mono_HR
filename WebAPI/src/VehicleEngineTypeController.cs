using System.Collections;
using System.Text.Json;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mono.Model;
using Mono.Repository.Common;
using Mono.Service.Common;
using Mono.WebAPI.dto;

namespace Mono.WebAPI;

[ApiVersion("1.0")]
[Route("api/v{version}/[controller]")]
public class VehicleEngineTypeController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IEngineTypeService engineTypeService;
    private readonly IRepositoryFactory<VehicleEngineType> engineFactory;

    public VehicleEngineTypeController(IMapper mapper,
        IEngineTypeService engineTypeService,
        IRepositoryFactory<VehicleEngineType> engineFactory)
    {
        this.mapper = mapper;
        this.engineTypeService = engineTypeService;
        this.engineFactory = engineFactory;

        //build lookup table
        engineTypeService.Initialize(engineFactory);
    }

    [HttpGet(Name = nameof(GetAllTypes))]
    public async Task<ActionResult> GetAllTypes([FromQuery] QueryParameters queryParameters)
    {
        using var repository = engineFactory.Build();
        var query = queryParameters.Query;
        Func<VehicleEngineType, bool>? filter = string.IsNullOrEmpty(query)
            ? null
            : make => make.Type.ToString().Contains(query, StringComparison.InvariantCultureIgnoreCase) ||
                      make.Type.Contains(query, StringComparison.InvariantCultureIgnoreCase) ||
                      make.Abrv.ToString().Contains(query, StringComparison.InvariantCultureIgnoreCase) ||
                      make.Abrv.Contains(query, StringComparison.InvariantCultureIgnoreCase);

        var sorter = queryParameters.CreateComparer<VehicleEngineType>([
            typeof(VehicleEngineType).GetProperty(nameof(VehicleEngineType.Type)),
            typeof(VehicleEngineType).GetProperty(nameof(VehicleEngineType.Abrv))
        ], typeof(VehicleEngineType).GetProperty(nameof(VehicleEngineType.Type)));

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
            var dto = mapper.Map<VehicleEngineType, VehicleEngineTypeDto>(item);
            data.Add(dto);
        }

        return Ok(new
        {
            value = data,
        });
    }
}