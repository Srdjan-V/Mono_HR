import type {QueryParameters} from "../store/QueryParameters.ts";
import React, {useState} from "react";
import {LocalStorage} from "../utils/LocalStorage.ts";
import type {VehicleEngineType} from "../api/VehicleTypes.ts";
import {fetchEngineTypes} from "../api/VehicleApi.ts";
import {SearchComponent, SearchResults} from "../components/SearchComponent.tsx";
import {TableFromObject} from "../components/TableFromObject.tsx";

const DEFAULT_PARAMS: QueryParameters = {
    page: 1,
    pageCount: 10,
    query: '',
    orderBy: 'name asc'
};

const VehicleEngineTypesPage: React.FC = () => {
    const localStorage = new LocalStorage<QueryParameters>('VehicleEngineTypesSearch');

    const [results, setResults] = useState<VehicleEngineType[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [params, setParams] = useState<QueryParameters>(localStorage.loadDefault(DEFAULT_PARAMS));

    const fetchVehicleMakes = async (searchParams: QueryParameters) => {
        setLoading(true);
        setError(null);

        try {
            const response = await fetchEngineTypes(searchParams);
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
        <div className="engine-search-page">
            <h1>Vehicle Engine Type Search</h1>

            <SearchComponent
                onSearch={handleSearch}
                local={localStorage}
                defaultState={DEFAULT_PARAMS}
            />

            <SearchResults
                results={results}
                renderItem={(engineType: VehicleEngineType) => (
                    <div className="engine-card">
                        <TableFromObject
                            data={engineType}
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


export default VehicleEngineTypesPage;