import React, {useState} from 'react';
import type {QueryParameters} from "../store/QueryParameters.ts";
import {LocalStorage} from "../utils/LocalStorage.ts";
import {createModel, deleteModel, fetchModels, updateModel} from "../api/VehicleApi.ts";
import {SearchComponent, SearchResults} from "../components/SearchComponent.tsx";
import {PopupCrud} from "../components/CrudComponent.tsx";
import type {VehicleModel, VehicleModelCreateUpdateDto} from "../api/VehicleTypes.ts";
import {TableFromObject} from "../components/TableFromObject.tsx";

const DEFAULT_PARAMS: QueryParameters = {
    page: 1,
    pageCount: 10,
    query: '',
    orderBy: 'name asc'
};

const VehicleModelsPage: React.FC = () => {
    const localStorage = new LocalStorage<QueryParameters>('VehicleModelsSearch');
    const localStorageCreate = new LocalStorage<VehicleModelCreateUpdateDto>('VehicleModelsCreate');
    
    const [results, setResults] = useState<VehicleModel[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [params, setParams] = useState<QueryParameters>(localStorage.loadDefault(DEFAULT_PARAMS));

    const fetchVehicleModels = async (searchParams: QueryParameters) => {
        setLoading(true);
        setError(null);

        try {
            const response = await fetchModels(searchParams);
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
        fetchVehicleModels(searchParams);
    };

    const handlePageChange = (page: number) => {
        const newParams = {...params, page};
        setParams(newParams);
        fetchVehicleModels(newParams);
    };

    return (
        <div className="models-search-page">
            <h1>Vehicle Models Search</h1>

            <SearchComponent
                onSearch={handleSearch}
                local={localStorage}
                defaultState={DEFAULT_PARAMS}
            />

            <PopupCrud<VehicleModelCreateUpdateDto>
                local={localStorageCreate}
                trigger={<button style={{padding: '10px 20px'}}>Create</button>}
                fields={[
                    {
                        name: "id",
                        label: 'Id',
                        type: 'text'
                    },
                    {
                        name: "VehicleMakeId",
                        label: 'Make Id',
                        type: 'text'
                    },
                    {
                        name: "name",
                        label: 'Name',
                        type: 'text'
                    },
                    {
                        name: 'abrv',
                        label: 'Abrv',
                        type: 'text'
                    }
                ]}
                onSave={function (item: VehicleModelCreateUpdateDto): Promise<void> {
                    return createModel(item) as unknown as Promise<void>;
                }}
                emptyItem={function (): VehicleModelCreateUpdateDto {
                    return {} as VehicleModelCreateUpdateDto
                }}
            />

            <SearchResults
                results={results}
                renderItem={(vehicleModel: VehicleModel) => (
                    <div className="model-card">
                        <TableFromObject
                            data={vehicleModel}
                        />
                        <PopupCrud<VehicleModelCreateUpdateDto>
                            trigger={<button style={{padding: '10px 20px'}}>Edit</button>}
                            item={{
                                VehicleMakeId: vehicleModel.make.id,
                                abrv: vehicleModel.abrv,
                                id: vehicleModel.id,
                                name: vehicleModel.name
                            } as VehicleModelCreateUpdateDto}
                            fields={[
                                {
                                    name: "name",
                                    label: 'name',
                                    type: 'text'
                                },
                                {
                                    name: "VehicleMakeId",
                                    label: 'Make Id',
                                    type: 'text'
                                },
                                {
                                    name: 'abrv',
                                    label: 'abrv',
                                    type: 'text'
                                },

                            ]}
                            onSave={function (item: VehicleModelCreateUpdateDto): Promise<void> {
                                return updateModel(item.id, item) as unknown as Promise<void>;
                            }}
                            onDelete={function (id: string): Promise<void> {
                                return deleteModel(id) as unknown as Promise<void>;
                            }}
                            emptyItem={function (): VehicleModelCreateUpdateDto {
                                return {} as VehicleModelCreateUpdateDto
                            }}
                        />
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

export default VehicleModelsPage;