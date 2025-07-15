using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mono.Model.Common;
using Mono.Service.Common;
using Mono.WebAPI.dto;

namespace Mono.WebAPI;

[ApiController]
[Route("api/registrations")]
public class VehicleRegistrationController(
    IVehicleService vehicleService,
    IMapper mapper
) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<VehicleRegistrationDto>> RegisterVehicle(
        [FromBody] CombinedRegistrationRequest request)
    {
        var registrationRequest = mapper.Map<IVehicleRegistration>(request.Registration);
        var modelRequest = mapper.Map<IVehicleModel>(request.Model);
        var makeRequest = mapper.Map<IVehicleMake>(request.Make);
        var engineTypeRequest = mapper.Map<IVehicleEngineType>(request.EngineType);
        var ownerRequest = mapper.Map<IVehicleOwner>(request.Owner);

        var result = await vehicleService.RegisterVehicleAsync(
            registrationRequest,
            modelRequest,
            makeRequest,
            engineTypeRequest,
            ownerRequest
        );

        return Ok(result);
    }
}

public class CombinedRegistrationRequest
{
    public VehicleRegistrationCreateUpdateDto Registration { get; set; }

    public VehicleModelCreateUpdateDto Model { get; set; }

    public VehicleMakeCreateUpdateDto Make { get; set; }

    public VehicleEngineTypeDto EngineType { get; set; }
    public VehicleOwnerCreateUpdateDto Owner { get; set; }
}