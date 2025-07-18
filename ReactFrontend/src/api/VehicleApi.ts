import axios from 'axios';
import type {
    VehicleEngineType,
    VehicleMake,
    VehicleMakeCreateUpdateDto,
    VehicleModel,
    VehicleModelCreateUpdateDto,
    VehicleOwner,
    VehicleOwnerCreateUpdateDto,
    VehicleRegistration,
    VehicleRegistrationCreateUpdateDto,
} from './VehicleTypes.ts';
import type {QueryParameters} from "../store/QueryParameters.ts";

const API_URL = 'http://localhost:5191/api'; // Adjust to your backend URL

export const fetchMakes = async (params: QueryParameters): Promise<VehicleMake[]> => {
    const response = await axios.get(`${API_URL}/v1.0/vehiclemake`, {
        params: params
    });
    return response.data.value;
};

export const updateMake = async (id: string, updatedMake: Partial<VehicleMakeCreateUpdateDto>): Promise<VehicleMake> => {
    const response = await axios.patch(`${API_URL}/v1.0/vehiclemake/${id}`, updatedMake);
    return response.data.value;
};

export const createMake = async (newMake: Partial<VehicleMakeCreateUpdateDto>): Promise<VehicleMake> => {
    const response = await axios.post(`${API_URL}/v1.0/vehiclemake`, newMake);
    return response.data.value;
};

export const deleteMake = async (id: string): Promise<void> => {
    await axios.delete(`${API_URL}/v1.0/vehiclemake/${id}`);
};

/*
export const fetchModelsFlat = async (params: QueryParameters): Promise<VehicleModelFlat[]> => {
    let data =  await fetchModels(params);
    var mapped = [] as VehicleModelFlat[];

    for (let i = 0; i < data.length; i++) {
        const item = data[i];

        mapped.push({
            id: item.id,
            name: item.name,
            abrv: item.abrv,
            makeId: item.make.id
        })
    }

    return mapped;
};
*/

export const fetchModels = async (params: QueryParameters): Promise<VehicleModel[]> => {
    const response = await axios.get(`${API_URL}/v1.0/vehiclemodel`, {
        params: params
    });
    return response.data.value;
};

export const updateModel = async (id: string, updatedModel: Partial<VehicleModelCreateUpdateDto>): Promise<VehicleModel> => {
    const response = await axios.patch(`${API_URL}/v1.0/vehiclemodel/${id}`, updatedModel);
    return response.data.value;
};

export const createModel = async (newModel: Partial<VehicleModelCreateUpdateDto>): Promise<VehicleModel> => {
    const response = await axios.post(`${API_URL}/v1.0/vehiclemodel`, newModel);
    return response.data.value;
};

export const deleteModel = async (id: string): Promise<void> => {
    await axios.delete(`${API_URL}/v1.0/vehiclemodel/${id}`);
};

export const fetchOwners = async (params: QueryParameters): Promise<VehicleOwner[]> => {
    const response = await axios.get(`${API_URL}/v1.0/vehicleowner`, {
        params: params
    });
    return response.data.value;
};

export const updateOwner = async (id: string, updatedOwner: Partial<VehicleOwnerCreateUpdateDto>): Promise<VehicleOwner> => {
    const response = await axios.patch(`${API_URL}/v1.0/vehicleowner/${id}`, updatedOwner);
    return response.data.value;
};

export const createOwner = async (newOwner: Partial<VehicleOwnerCreateUpdateDto>): Promise<VehicleOwner> => {
    const response = await axios.post(`${API_URL}/v1.0/vehicleowner`, newOwner);
    return response.data.value;
};

export const deleteOwner = async (id: string): Promise<void> => {
    await axios.delete(`${API_URL}/v1.0/vehicleowner/${id}`);
};

/*
export const fetchRegistrationsFlat = async (params: QueryParameters): Promise<VehicleRegistrationFlatData[]> => {
    let data =  await fetchRegistrations(params);
    var mapped = [] as VehicleRegistrationFlatData[];

    for (let i = 0; i < data.length; i++) {
        const item = data[i];

        mapped.push({
            id: item.id,
            registrationNumber: item.registrationNumber,
            modelId: item.model.id,
            modelName: item.model.name,
            modelAbrv: item.model.abrv,
            makeId: item.model.make.id,
            makeName: item.model.make.name,
            makeAbrv: item.model.abrv,
            engineTypeId: item.engineType.id,
            engineTypeName: item.engineType.name,
            engineTypeAbrv: item.engineType.abrv,
            ownerId: item.owner.id,
            ownerFirstName: item.owner.firstName,
            ownerLastName: item.owner.lastName,
            ownerDob: item.owner.dob,
        })
    }
    
    return mapped;
}
*/

export const fetchRegistrations = async (params: QueryParameters): Promise<VehicleRegistration[]> => {
    const response = await axios.get(`${API_URL}/v1.0/vehicleregistration`, {
        params: params
    });
    return response.data.value;
};

export const createRegistration = async (newRegistration: Partial<VehicleRegistrationCreateUpdateDto>): Promise<VehicleRegistration> => {
    const response = await axios.post(`${API_URL}/v1.0/vehicleregistration`, newRegistration);
    return response.data.value;
};

export const deleteRegistration = async (id: string): Promise<void> => {
    await axios.delete(`${API_URL}/v1.0/vehicleregistration/${id}`);
};

export const fetchEngineTypes = async (params: QueryParameters): Promise<VehicleEngineType[]> => {
    const response = await axios.get(`${API_URL}/v1.0/vehicleenginetype`, {
        params: params
    });
    return response.data.value;
};