import React, {useState} from 'react';
import type {QueryParameters} from "../store/QueryParameters.ts";
import {LocalStorage} from "../utils/LocalStorage.ts";
import type {VehicleModel} from "../api/VehicleTypes.ts";
import {createModel, deleteModel, fetchModels, updateModel} from "../api/VehicleApi.ts";
import {SearchComponent, SearchResults} from "../components/SearchComponent.tsx";
import {PopupCrud} from "../components/CrudComponent.tsx";

const DEFAULT_PARAMS: QueryParameters = {
    page: 1,
    pageCount: 10,
    query: '',
    orderBy: 'name asc'
};

const VehicleMakesPage: React.FC = () => {
    const localStorage = new LocalStorage<QueryParameters>('VehicleModelsSearch');

    const [results, setResults] = useState<VehicleModel[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [params, setParams] = useState<QueryParameters>(localStorage.loadDefault(DEFAULT_PARAMS));

    const fetchVehicleMakes = async (searchParams: QueryParameters) => {
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

            <PopupCrud<VehicleModel>
                trigger={<button style={{padding: '10px 20px'}}>Create</button>}
                fields={[
                    {
                        name: "id",
                        label: 'Id',
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
                onSave={function (item: VehicleModel): Promise<void> {
                    return createModel(item) as unknown as Promise<void>;
                }}
                emptyItem={function (): VehicleModel {
                    return {} as VehicleModel
                }}
            />


            <SearchResults
                results={results}
                renderItem={(vehicleModel: VehicleModel) => (
                    <div className="model-card">
                        <h3>{vehicleModel.name}</h3>
                        <div className="model-details">
                            <span>Name: ${vehicleModel.name}</span>
                            <span>  </span>
                            <span>Abrv: ${vehicleModel.abrv}</span>
                            <span>  </span>

                            <div className="popup-content">
                                <PopupCrud<VehicleModel>
                                    trigger={<button style={{padding: '10px 20px'}}>Edit</button>}
                                    item={vehicleModel}
                                    fields={[
                                        {
                                            name: "name",
                                            label: 'name',
                                            type: 'text'
                                        },
                                        {
                                            name: 'abrv',
                                            label: 'abrv',
                                            type: 'text'
                                        }
                                    ]}
                                    onSave={function (item: VehicleModel): Promise<void> {
                                        return updateModel(item.id, item) as unknown as Promise<void>;
                                    }}
                                    onDelete={function (id: string): Promise<void> {
                                        return deleteModel(id) as unknown as Promise<void>;
                                    }}
                                    emptyItem={function (): VehicleModel {
                                        return {} as VehicleModel
                                    }}
                                />
                            </div>
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

export default VehicleMakesPage;