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
public class VehicleModelController(
    IMapper mapper,
    IRepositoryFactory<VehicleModel> modelFactory) :
    ControllerBase
{
    [HttpGet(Name = nameof(GetAllModels))]
    public async Task<ActionResult> GetAllModels([FromQuery] QueryParameters queryParameters)
    {
        using var repository = modelFactory.Build();
        var query = queryParameters.Query;
        Func<VehicleModel, bool>? filter = string.IsNullOrEmpty(query)
            ? null
            : make => make.Name.ToString().Contains(query, StringComparison.InvariantCultureIgnoreCase) ||
                      make.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase) ||
                      make.Abrv.ToString().Contains(query, StringComparison.InvariantCultureIgnoreCase) ||
                      make.Abrv.Contains(query, StringComparison.InvariantCultureIgnoreCase);

        var sorter = queryParameters.CreateComparer<VehicleModel>([
            typeof(VehicleModel).GetProperty(nameof(VehicleModel.Name)),
            typeof(VehicleModel).GetProperty(nameof(VehicleModel.Abrv))
        ], typeof(VehicleModel).GetProperty(nameof(VehicleModel.Name)));

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
            var dto = mapper.Map<VehicleModel, VehicleModelDto>(item);
            data.Add(dto);
        }

        return Ok(new
        {
            value = data,
        });
    }

    [HttpPost(Name = nameof(RegisterModel))]
    public async Task<ActionResult> RegisterModel([FromBody] VehicleModelCreateUpdateDto updateDto)
    {
        var vehicleModel = mapper.Map<VehicleModelCreateUpdateDto, VehicleModel>(updateDto);
        using var repository = modelFactory.Build();
        var addAsync = await repository.AddAsync(vehicleModel);
        var commitAsync = await repository.CommitAsync();
        if (addAsync != 1 || commitAsync != 1)
        {
            throw new IOException("Failed to register new make");
        }

        var vehicleModelDto = mapper.Map<VehicleModelDto>(vehicleModel);
        return Ok(vehicleModelDto);
    }

    [HttpPatch("{id:long}", Name = nameof(UpdateModel))]
    public async Task<ActionResult> UpdateModel(long id, [FromBody] VehicleModelCreateUpdateDto updateDto)
    {
        using var repository = modelFactory.Build();
        var existingModel = await repository.GetAsync(id);
        if (existingModel == null)
        {
            return NotFound();
        }

        var updateModel = mapper.Map(updateDto, existingModel);
        var updateAsync = await repository.UpdateAsync(updateModel);
        var commitAsync = await repository.CommitAsync();
        if (updateAsync != 1 || commitAsync != 1)
        {
            throw new IOException("Failed to update new make");
        }

        var vehicleMakeDto = mapper.Map<VehicleMakeDto>(updateModel);
        return Ok(vehicleMakeDto);
    }

    [HttpDelete(Name = nameof(DeleteModel))]
    public async Task<ActionResult> DeleteModel([FromQuery] long id)
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