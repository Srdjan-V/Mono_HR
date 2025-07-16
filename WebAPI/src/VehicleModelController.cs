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
public class VehicleModelController(
    IMapper mapper,
    IRepositoryFactory<VehicleModel> modelFactory) :
    ControllerBase
{
    [HttpGet(Name = nameof(GetAllModels))]
    public async Task<ActionResult> GetAllModels(ApiVersion version, [FromQuery] QueryParameters queryParameters)
    {
        using var repository = modelFactory.Build();
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
            Comparer<VehicleModel>.Default //todo figure out how to build the correct sorter in c#
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
            var dto = mapper.Map<VehicleModel, VehicleModelDto>(item);
            data.Add(dto);
        }

        return Ok(new
        {
            value = data,
        });
    }

    [HttpPost(Name = nameof(RegisterModel))]
    public async Task<ActionResult> RegisterModel(ApiVersion version, [FromBody] VehicleModelCreateUpdateDto updateDto)
    {
        var vehicleModel = mapper.Map<VehicleModelCreateUpdateDto, VehicleModel>(updateDto);
        using var repository = modelFactory.Build();
        var i = await repository.AddAsync(vehicleModel);
        if (i != 1)
        {
            throw new IOException("Failed to register new model");
        }

        var vehicleModelDto = mapper.Map<VehicleModelDto>(vehicleModel);
        return Ok(vehicleModelDto);
    }

    [HttpPatch(Name = nameof(UpdateModel))]
    public async Task<ActionResult> UpdateModel(ApiVersion version, [FromBody] VehicleModelCreateUpdateDto updateDto)
    {
        var vehicleModel = mapper.Map<VehicleModelCreateUpdateDto, VehicleModel>(updateDto);
        using var repository = modelFactory.Build();
        var updateAsync = await repository.UpdateAsync(vehicleModel);
        var commitAsync = await repository.CommitAsync();
        if (updateAsync != 1 || commitAsync != 1)
        {
            throw new IOException("Failed to update new make");
        }

        var vehicleMakeDto = mapper.Map<VehicleMakeDto>(vehicleModel);
        return Ok(vehicleMakeDto);
    }

    [HttpDelete(Name = nameof(DeleteModel))]
    public async Task<ActionResult> DeleteModel(ApiVersion version, [FromQuery] long id)
    {
        using var repository = modelFactory.Build();
        var vehicleModel = await repository.GetAsync(id);
        if (vehicleModel == null)
        {
            return NotFound();
        }

        var modelDto = mapper.Map<VehicleModelDto>(vehicleModel);
        return Ok(modelDto);
    }
}