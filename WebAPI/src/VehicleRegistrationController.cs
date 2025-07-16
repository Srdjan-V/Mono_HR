using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mono.Model;
using Mono.Model.Common;
using Mono.Repository.Common;
using Mono.Service.Common;
using Mono.WebAPI.dto;

namespace Mono.WebAPI;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class VehicleRegistrationController : ControllerBase
{
    private readonly IVehicleService _vehicleService;
    private readonly IEngineTypeService _engineTypeService;
    private readonly IRepositoryFactory<VehicleEngineType> _engineEngineFactory;
    private readonly IMapper _mapper;

    public VehicleRegistrationController(
        IVehicleService vehicleService,
        IEngineTypeService engineTypeService,
        IRepositoryFactory<VehicleEngineType> engineFactory,
        IMapper mapper)
    {
        _vehicleService = vehicleService;
        _engineTypeService = engineTypeService;
        _engineEngineFactory = engineFactory;
        _mapper = mapper;

        //build lookup table
        _engineTypeService.Initialize(_engineEngineFactory);
    }

    [HttpPost("register")]
    public async Task<ActionResult<VehicleRegistrationDto>> RegisterVehicle(
        [FromBody] CombinedRegistrationRequest request)
    {
        var registrationRequest = _mapper.Map<IVehicleRegistration>(request.Registration);
        var modelRequest = _mapper.Map<IVehicleModel>(request.Model);
        var makeRequest = _mapper.Map<IVehicleMake>(request.Make);
        var engineTypeRequest = _mapper.Map<IVehicleEngineType>(request.EngineType);
        var ownerRequest = _mapper.Map<IVehicleOwner>(request.Owner);

        var result = await _vehicleService.RegisterVehicleAsync(
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