import React, {useState} from 'react';
import type {QueryParameters} from "../store/QueryParameters.ts";
import {LocalStorage} from "../utils/LocalStorage.ts";
import type {VehicleOwner} from "../api/VehicleTypes.ts";
import {createOwner, deleteOwner, fetchOwners, updateOwner} from "../api/VehicleApi.ts";
import {SearchComponent, SearchResults} from "../components/SearchComponent.tsx";
import {PopupCrud} from "../components/CrudComponent.tsx";


const DEFAULT_PARAMS: QueryParameters = {
    page: 1,
    pageCount: 10,
    query: '',
    orderBy: 'name asc'
};

const VehicleOwnersPage: React.FC = () => {
    const localStorage = new LocalStorage<QueryParameters>('VehicleOwnersSearch');

    const [results, setResults] = useState<VehicleOwner[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [params, setParams] = useState<QueryParameters>(localStorage.loadDefault(DEFAULT_PARAMS));

    const fetchVehicleMakes = async (searchParams: QueryParameters) => {
        setLoading(true);
        setError(null);

        try {
            const response = await fetchOwners(searchParams);
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

            <PopupCrud<VehicleOwner>
                trigger={<button style={{padding: '10px 20px'}}>Create</button>}
                fields={[
                    {
                        name: "id",
                        label: 'Id',
                        type: 'text'
                    },
                    {
                        name: "firstName",
                        label: 'First Name',
                        type: 'text'
                    },
                    {
                        name: 'lastName',
                        label: 'Last Name',
                        type: 'text'
                    },
                    {
                        name: 'dob',
                        label: 'Date of birth',
                        type: 'date'
                    }
                ]}
                onSave={function (item: VehicleOwner): Promise<void> {
                    return createOwner(item) as unknown as Promise<void>;
                }}
                emptyItem={function (): VehicleOwner {
                    return {} as VehicleOwner
                }}
            />


            <SearchResults
                results={results}
                renderItem={(vehicleOwner: VehicleOwner) => (
                    <div className="owner-card">
                        <h3>{vehicleOwner.firstName}</h3>
                        <div className="owner-details">
                            <span>Name: ${vehicleOwner.lastName}</span>
                            <span>  </span>
                            <span>Abrv: ${vehicleOwner.dob}</span>
                            <span>  </span>

                            <div className="popup-content">
                                <PopupCrud<VehicleOwner>
                                    trigger={<button style={{padding: '10px 20px'}}>Edit</button>}
                                    item={vehicleOwner}
                                    fields={[
                                        {
                                            name: "firstName",
                                            label: 'First Name',
                                            type: 'text'
                                        },
                                        {
                                            name: 'lastName',
                                            label: 'Last Name',
                                            type: 'text'
                                        },
                                        {
                                            name: 'dob',
                                            label: 'Date of birth',
                                            type: 'date'
                                        }
                                    ]}
                                    onSave={function (item: VehicleOwner): Promise<void> {
                                        return updateOwner(item.id, item) as unknown as Promise<void>;
                                    }}
                                    onDelete={function (id: string): Promise<void> {
                                        return deleteOwner(id) as unknown as Promise<void>;
                                    }}
                                    emptyItem={function (): VehicleOwner {
                                        return {} as VehicleOwner
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
export default VehicleOwnersPage;