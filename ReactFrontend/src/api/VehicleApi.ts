import axios from 'axios';
import type {VehicleEngineType, VehicleMake, VehicleModel, VehicleOwner, VehicleRegistration} from './VehicleTypes.ts';
import type {QueryParameters} from "../store/QueryParameters.ts";

const API_URL = 'http://localhost:5191/api'; // Adjust to your backend URL

export const fetchMakes = async (params: QueryParameters): Promise<VehicleMake[]> => {
    const response = await axios.get(`${API_URL}/v1.0/vehiclemake`, {
        params: params
    });
    return response.data.value;
};

export const updateMake = async (id: string, updatedMake: Partial<VehicleMake>): Promise<VehicleMake> => {
    const response = await axios.patch(`${API_URL}/v1.0/vehiclemake/${id}`, updatedMake);
    return response.data.value;
};

export const createMake = async (newMake: Omit<VehicleMake, 'id'>): Promise<VehicleMake> => {
    const response = await axios.post(`${API_URL}/v1.0/vehiclemake`, newMake);
    return response.data.value;
};

export const deleteMake = async (id: string): Promise<void> => {
    await axios.delete(`${API_URL}/v1.0/vehiclemake/${id}`);
};

export const fetchModels = async (params: QueryParameters): Promise<VehicleModel[]> => {
    const response = await axios.get(`${API_URL}/v1.0/vehiclemodel`, {
        params: params
    });
    return response.data.value;
};

export const updateModel = async (id: string, updatedModel: Partial<VehicleModel>): Promise<VehicleModel> => {
    const response = await axios.patch(`${API_URL}/v1.0/vehiclemodel/${id}`, updatedModel);
    return response.data.value;
};

export const createModel = async (newModel: Omit<VehicleModel, 'id'>): Promise<VehicleModel> => {
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

export const updateOwner = async (id: string, updatedOwner: Partial<VehicleOwner>): Promise<VehicleOwner> => {
    const response = await axios.patch(`${API_URL}/v1.0/vehicleowner/${id}`, updatedOwner);
    return response.data.value;
};

export const createOwner = async (newOwner: Omit<VehicleOwner, 'id'>): Promise<VehicleOwner> => {
    const response = await axios.post(`${API_URL}/v1.0/vehicleowner`, newOwner);
    return response.data.value;
};

export const deleteOwner = async (id: string): Promise<void> => {
    await axios.delete(`${API_URL}/v1.0/vehicleowner/${id}`);
};

export const fetchRegistrations = async (): Promise<VehicleRegistration[]> => {
    const response = await axios.get(`${API_URL}/v1.0/vehicle/registrations`);
    return response.data.value;
};

//todo
export const fetchEngineTypes = async (): Promise<VehicleEngineType[]> => {
    const response = await axios.get(`${API_URL}/engineTypes`);
    return response.data.value;
};