export interface VehicleMake {
    id: string;
    name: string;
    abrv: string;
}

export interface VehicleMakeCreateUpdateDto {
    id: string;
    name: string;
    abrv: string;
}

export interface VehicleModel {
    id: string;
    name: string;
    abrv: string;
    make: VehicleMake;
}

export interface VehicleModelCreateUpdateDto {
    id: string;
    name: string;
    abrv: string;
    VehicleMakeId: string;
}

export interface VehicleOwner {
    id: string;
    firstName: string;
    lastName: string;
    dob: string;
}

export interface VehicleOwnerCreateUpdateDto {
    id: string;
    firstName: string;
    lastName: string;
    dob: string;
}

export interface VehicleEngineType {
    id: string;
    name: string;
    abrv: string;
}

export interface VehicleRegistration {
    id: string;
    registrationNumber: string;
    model: VehicleModel;
    engineType: VehicleEngineType;
    owner: VehicleOwner;
}


export interface VehicleRegistrationCreateUpdateDto {
    id: string;
    registrationNumber: string;
    VehicleModelId: string;
    VehicleEngineTypeId: string;
    VehicleOwnerId: string;
}

/*
export interface VehicleRegistrationFlatData {
    id: string;
    registrationNumber: string;

    modelId: string;
    modelName: string;
    modelAbrv: string;

    makeId: string;
    makeName: string;
    makeAbrv: string;

    engineTypeId: string;
    engineTypeName: string;
    engineTypeAbrv: string;

    ownerId: string;
    ownerFirstName: string;
    ownerLastName: string;
    ownerDob: string;
}*/
