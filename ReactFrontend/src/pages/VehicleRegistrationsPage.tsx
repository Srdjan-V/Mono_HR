import React, {useState} from 'react';
import type {QueryParameters} from "../store/QueryParameters.ts";
import {LocalStorage} from "../utils/LocalStorage.ts";
import type {VehicleRegistration, VehicleRegistrationCreateUpdateDto} from "../api/VehicleTypes.ts";
import {createRegistration, deleteRegistration, fetchRegistrations} from "../api/VehicleApi.ts";
import {SearchComponent, SearchResults} from "../components/SearchComponent.tsx";
import {PopupCrud} from "../components/CrudComponent.tsx";
import {TableFromObject} from "../components/TableFromObject.tsx";

const DEFAULT_PARAMS: QueryParameters = {
    page: 1,
    pageCount: 10,
    query: '',
    orderBy: 'name asc'
};

const VehicleRegistrationsPage: React.FC = () => {
    const localStorage = new LocalStorage<QueryParameters>('VehicleOwnersSearch');
    const localStorageCreate = new LocalStorage<VehicleRegistrationCreateUpdateDto>('VehicleRegistrationCreate');

    const [results, setResults] = useState<VehicleRegistration[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [params, setParams] = useState<QueryParameters>(localStorage.loadDefault(DEFAULT_PARAMS));

    const fetchVehicleMakes = async (searchParams: QueryParameters) => {
        setLoading(true);
        setError(null);

        try {
            const response = await fetchRegistrations(searchParams);
            setResults(response);
        } catch (err) {
            // @ts-ignore
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    const handleSearch = (searchParams: QueryParameters) => {
        setParams(searchParams);
        fetchVehicleMakes(searchParams);
    };

    const handlePageChange = (page: number) => {
        const newParams = {...params, page};
        setParams(newParams);
        fetchVehicleMakes(newParams);
    };

    return (
        <div className="makes-search-page">
            <h1>Vehicle Makes Search</h1>

            <SearchComponent
                onSearch={handleSearch}
                local={localStorage}
                defaultState={DEFAULT_PARAMS}
            />


            <PopupCrud<VehicleRegistrationCreateUpdateDto>
                local={localStorageCreate}
                trigger={<button style={{padding: '10px 20px'}}>Create</button>}
                fields={[
                    {
                        name: "id",
                        label: 'Reg id',
                        type: 'text'
                    },
                    {
                        name: "registrationNumber",
                        label: 'Registration Number',
                        type: 'text'
                    },
                    {
                        name: "VehicleModelId",
                        label: 'Vehicle Model Id',
                        type: 'text'
                    },
                    {
                        name: "VehicleOwnerId",
                        label: 'Owner Id',
                        type: 'text'
                    },
                    {
                        name: 'VehicleEngineTypeId',
                        label: 'Vehicle engine type id',
                        type: 'text'
                    }
                ]}
                onSave={function (item: VehicleRegistrationCreateUpdateDto): Promise<void> {
                    return createRegistration(item) as unknown as Promise<void>;
                }}
                emptyItem={function (): VehicleRegistrationCreateUpdateDto {
                    return {} as VehicleRegistrationCreateUpdateDto
                }}
            />

            <SearchResults
                results={results}
                renderItem={(vehicleRegistration: VehicleRegistration) => (
                    <div className="reg-card">
                        <TableFromObject
                            data={vehicleRegistration}
                        />

                        <div className="popup-content">
                            <PopupCrud<VehicleRegistrationCreateUpdateDto>
                                trigger={<button style={{padding: '10px 20px'}}>Edit</button>}
                                item={{
                                    VehicleEngineTypeId: vehicleRegistration.engineType.id,
                                    VehicleModelId: vehicleRegistration.model.id,
                                    VehicleOwnerId: vehicleRegistration.owner.id,
                                    id: vehicleRegistration.id,
                                    registrationNumber: vehicleRegistration.registrationNumber
                                } as VehicleRegistrationCreateUpdateDto}
                                fields={[
                                    {
                                        name: "id",
                                        label: 'Reg id',
                                        type: 'text'
                                    },
                                    {
                                        name: "registrationNumber",
                                        label: 'Registration Number',
                                        type: 'text'
                                    },
                                    {
                                        name: "VehicleModelId",
                                        label: 'Vehicle Model Id',
                                        type: 'text'
                                    },
                                    {
                                        name: "VehicleOwnerId",
                                        label: 'Owner Id',
                                        type: 'text'
                                    },
                                    {
                                        name: 'VehicleEngineTypeId',
                                        label: 'Vehicle engine type id',
                                        type: 'text'
                                    }
                                ]}
                                onDelete={function (id: string): Promise<void> {
                                    return deleteRegistration(id) as unknown as Promise<void>;
                                }}
                                emptyItem={function (): VehicleRegistrationCreateUpdateDto {
                                    return {} as VehicleRegistrationCreateUpdateDto
                                }}
                            />
                        </div>
                    </div>
                )}
                loading={loading}
                error={error}
                params={params}
                onPageChange={handlePageChange}
            />
        </div>
    );
};

export default VehicleRegistrationsPage;