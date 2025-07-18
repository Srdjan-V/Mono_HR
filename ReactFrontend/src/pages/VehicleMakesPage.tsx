import React, {useState} from 'react';

import type {QueryParameters} from "../store/QueryParameters.ts";
import {LocalStorage} from "../utils/LocalStorage.ts";
import type {VehicleMake} from "../api/VehicleTypes.ts";
import {createMake, deleteMake, fetchMakes, updateMake} from "../api/VehicleApi.ts";
import {SearchComponent, SearchResults} from "../components/SearchComponent.tsx";
import {PopupCrud} from "../components/CrudComponent.tsx";

const DEFAULT_PARAMS: QueryParameters = {
    page: 1,
    pageCount: 10,
    query: '',
    orderBy: 'name asc'
};

const VehicleMakesPage: React.FC = () => {
    const localStorage = new LocalStorage<QueryParameters>('VehicleMakesSearch');

    const [results, setResults] = useState<VehicleMake[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [params, setParams] = useState<QueryParameters>(localStorage.loadDefault(DEFAULT_PARAMS));

    const fetchVehicleMakes = async (searchParams: QueryParameters) => {
        setLoading(true);
        setError(null);

        try {
            const response = await fetchMakes(searchParams);
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

            <PopupCrud<VehicleMake>
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
                onSave={function (item: VehicleMake): Promise<void> {
                    return createMake(item) as unknown as Promise<void>;
                }}
                emptyItem={function (): VehicleMake {
                    return {} as VehicleMake
                }}
            />
            

            <SearchResults
                results={results}
                renderItem={(vehicleMake: VehicleMake) => (
                    <div className="make-card">
                        <h3>{vehicleMake.name}</h3>
                        <div className="make-details">
                            <span>Name: ${vehicleMake.name}</span>
                            <span>  </span>
                            <span>Abrv: ${vehicleMake.abrv}</span>
                            <span>  </span>

                            <div className="popup-content">
                                <PopupCrud<VehicleMake>
                                    trigger={<button style={{padding: '10px 20px'}}>Edit</button>}
                                    item={vehicleMake}
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
                                    onSave={function (item: VehicleMake): Promise<void> {
                                        return updateMake(item.id, item) as unknown as Promise<void>;
                                    }}
                                    onDelete={function (id: string): Promise<void> {
                                        return deleteMake(id) as unknown as Promise<void>;
                                    }}
                                    emptyItem={function (): VehicleMake {
                                        return {} as VehicleMake
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
