interface FlattenedObject {
    [key: string]: unknown;
}

const flattenObject = (obj: object, parentKey = ''): FlattenedObject => {
    return Object.entries(obj).reduce((acc, [key, value]) => {
        const fullKey = parentKey ? `${parentKey}_${key}` : key;

        if (typeof value === 'object' && value !== null && !Array.isArray(value)) {
            return {...acc, ...flattenObject(value as Record<string, unknown>, fullKey)};
        }

        return {...acc, [fullKey]: value};
    }, {});
};

interface TableFromObjectProps<T extends object> {
    data: T;
    parentKey?: string;
}

export const TableFromObject = <T extends object>({
                                                      data,
                                                      parentKey = ''
                                                  }: TableFromObjectProps<T>) => {
    const flattenedData = flattenObject(data, parentKey);
    const isEmpty = Object.keys(flattenedData).length === 0;

    return (
        <div className="object-table">
            {isEmpty ? (
                <div className="table-row">
                    <span className="table-key">{parentKey || 'Object'}:</span>
                    <span className="table-value">{"{}"}</span>
                </div>
            ) : (
                Object.entries(flattenedData).map(([fullKey, value]) => (
                    <div className="table-row" key={fullKey}>
                        <span className="table-key">{fullKey} : </span>
                        <span className="table-value">
              {Array.isArray(value)
                  ? `[${value.length} items]`
                  : String(value)}
            </span>
                    </div>
                ))
            )}
        </div>
    );
};