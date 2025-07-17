import React, {useState} from 'react';

import type {QueryParameters} from "../store/QueryParameters.ts";
import {LocalStorage} from "../utils/LocalStorage.ts";


export interface SearchComponentProps {
    onSearch: (params: QueryParameters) => void;
    local: LocalStorage<QueryParameters>;
    defaultState: QueryParameters;
}

export const SearchComponent = ({
                                    onSearch,
                                    local,
                                    defaultState
                                }: SearchComponentProps) => {
    const [params, setParams] = useState<QueryParameters>(local.loadDefault(defaultState))

    const handleSearch = () => {
        local.save(params);
        onSearch(params);
    }

    const handleQueryChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setParams(prev => ({...prev, query: e.target.value, page: 1}));
    };

    const handlePageChange = (newPage: number) => {
        setParams(prev => ({...prev, page: newPage}));
    };

    const handleOrderByChange = (field: string) => {
        setParams(prev => {
            const isCurrentlyDesc = prev.orderBy.includes('desc');
            const newOrder = isCurrentlyDesc ? 'asc' : 'desc';
            return {...prev, orderBy: `${field} ${newOrder}`};
        });
    };

    const resetSearch = () => {
        setParams(() => {
            return defaultState;
        })
    };

    return (
        <div className="search-container">
            <div className="search-controls">
                <input
                    type="text"
                    value={params.query}
                    onChange={handleQueryChange}
                    placeholder="Search..."
                    className="search-input"
                />

                <div className="pagination-controls">
                    <button
                        onClick={() => handlePageChange(params.page - 1)}
                        disabled={params.page <= 1}
                    >
                        Previous
                    </button>

                    <span>Page {params.page}</span>

                    <button
                        onClick={() => handlePageChange(params.page + 1)}
                    >
                        Next
                    </button>
                </div>

                <div className="sort-controls">
                    <span>Sort by: </span>
                    <button
                        onClick={() => handleOrderByChange('name')}
                        className={params.orderBy.includes('name') ? 'active' : ''}
                    >
                        Name {params.orderBy.includes('name') ?
                        (params.orderBy.includes('asc') ? '↑' : '↓') : ''}
                    </button>
                    <button
                        onClick={() => handleOrderByChange('date')}
                        className={params.orderBy.includes('date') ? 'active' : ''}
                    >
                        Date {params.orderBy.includes('date') ?
                        (params.orderBy.includes('asc') ? '↑' : '↓') : ''}
                    </button>
                </div>

                <button onClick={resetSearch} className="reset-button">
                    Reset
                </button>
                <button onClick={handleSearch} className="search-button">
                    Search
                </button>
            </div>

            <div className="results-per-page">
                <span>Results per page: </span>
                <select
                    value={params.pageCount}
                    onChange={(e) => setParams(prev => ({
                        ...prev,
                        pageCount: Number(e.target.value),
                        page: 1
                    }))}
                >
                    <option value={5}>5</option>
                    <option value={10}>10</option>
                    <option value={20}>20</option>
                    <option value={50}>50</option>
                </select>
            </div>
        </div>
    );
};

export interface SearchResultsProps<T> {
    results: T[];
    renderItem: (item: T) => React.ReactNode;
    loading: boolean;
    error: string | null;
    params: QueryParameters;
    onPageChange: (page: number) => void;

}

export const SearchResults = <T extends object>({
                                                    results,
                                                    renderItem,
                                                    loading,
                                                    error,
                                                    params,
                                                    onPageChange
                                                }: SearchResultsProps<T>) => {
    const safeResults: T[] = (() => {
        try {
            // Handle null/undefined
            if (results == null) return [];

            // Handle arrays
            if (Array.isArray(results)) return results;

            // Handle array-like objects
            if (typeof results === 'object' && 'length' in results) {
                return Array.from(results as any);
            }

            return [results];
        } catch (e) {
            console.error('Error normalizing results:', e);
            return [];
        }
    })();

    if (loading) {
        return <div className="loading-indicator">Loading results...</div>;
    }

    if (error) {
        return <div className="error-message">Error: {error}</div>;
    }

    if (safeResults.length === 0) {
        return <div className="no-results">No results found</div>;
    }

    return (
        <div className="search-results-container">
            <div className="results-summary">
                Showing results {((params.page - 1) * params.pageCount) + 1}-
                {Math.min(params.page * params.pageCount, safeResults.length)} of {safeResults.length}
            </div>

            <ul className="results-list">
                {safeResults.map((item) => (
                    <li key="render" className="result-item">
                        {renderItem(item)}
                    </li>
                ))}
            </ul>

            <div className="pagination-controls">
                <button
                    onClick={() => onPageChange(params.page - 1)}
                    disabled={params.page <= 1}
                >
                    Previous
                </button>

                <span>Page {params.page} of {Math.ceil(safeResults.length / params.pageCount)}</span>

                <button
                    onClick={() => onPageChange(params.page + 1)}
                    disabled={params.page >= Math.ceil(safeResults.length / params.pageCount)}
                >
                    Next
                </button>
            </div>
        </div>
    );
};