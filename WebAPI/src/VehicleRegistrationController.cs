using System.Collections;
using System.Text.Json;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mono.Model;
using Mono.Repository.Common;
using Mono.WebAPI.dto;

namespace Mono.WebAPI;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version}/[controller]")]
public class VehicleRegistrationController(
    IMapper mapper,
    IRepositoryFactory<VehicleRegistration> registrationFactory
) : ControllerBase
{
    [HttpGet(Name = nameof(GetAllRegistrations))]
    public async Task<ActionResult> GetAllRegistrations([FromQuery] QueryParameters queryParameters)
    {
        using var repository = registrationFactory.Build();
        var query = queryParameters.Query;
        Func<VehicleRegistration, bool>? filter = string.IsNullOrEmpty(query)
            ? null
            : make => make.RegistrationNumber.ToString().Contains(query, StringComparison.InvariantCultureIgnoreCase) ||
                      make.RegistrationNumber.Contains(query, StringComparison.InvariantCultureIgnoreCase);

        var sorter = queryParameters.CreateComparer<VehicleRegistration>([
            typeof(VehicleRegistration).GetProperty(nameof(VehicleRegistration.RegistrationNumber))
        ], typeof(VehicleRegistration).GetProperty(nameof(VehicleRegistration.RegistrationNumber)));

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
            var dto = mapper.Map<VehicleRegistration, VehicleRegistrationDto>(item);
            data.Add(dto);
        }

        return Ok(new
        {
            value = data,
        });
    }

    [HttpPost(Name = nameof(RegisterRegistration))]
    public async Task<ActionResult> RegisterRegistration([FromBody] VehicleRegistrationCreateUpdateDto updateDto)
    {
        var vehicleRegistration = mapper.Map<VehicleRegistrationCreateUpdateDto, VehicleRegistration>(updateDto);
        using var repository = registrationFactory.Build();
        var addAsync = await repository.AddAsync(vehicleRegistration);
        var commitAsync = await repository.CommitAsync();
        if (addAsync != 1 || commitAsync != 1)
        {
            throw new IOException("Failed to register new make");
        }

        var ownerDto = mapper.Map<VehicleRegistrationDto>(vehicleRegistration);
        return Ok(new
        {
            value = ownerDto,
        });
    }

    [HttpPatch("{id:long}", Name = nameof(UpdateRegistration))]
    public async Task<ActionResult> UpdateRegistration(long id, [FromBody] VehicleRegistrationDto updateDto)
    {
        using var repository = registrationFactory.Build();
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

        var ownerDto = mapper.Map<VehicleRegistrationDto>(updateOwner);
        return Ok(new
        {
            value = ownerDto,
        });
    }

    [HttpDelete("{id:long}", Name = nameof(DeleteRegistration))]
    public async Task<ActionResult> DeleteRegistration(long id)
    {
        using var repository = registrationFactory.Build();
        var vehicleOwner = await repository.GetAsync(id);
        if (vehicleOwner == null)
        {
            return NotFound();
        }

        await repository.DeleteAsync(id);
        await repository.CommitAsync();

        var ownerDto = mapper.Map<VehicleOwnerDto>(vehicleOwner);
        return Ok(new
        {
            value = ownerDto,
        });
    }
}