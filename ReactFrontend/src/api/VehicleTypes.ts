export interface VehicleMake {
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

export interface VehicleOwner {
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