import React, {useEffect, useState} from 'react';
import {LocalStorage} from "../utils/LocalStorage.ts";

type FieldType = 'text' | 'number' | 'select' | 'checkbox' | 'date' | 'custom';

interface FieldConfig<T> {
    name: keyof T;
    label: string;
    type: FieldType;
    required?: boolean;
    options?: { value: any; label: string }[];
    component?: React.ComponentType<{
        value: any;
        onChange: (value: any) => void;
        item?: T;
    }>;
    disabled?: boolean | ((item: T) => boolean);
}

interface SingleItemCrudProps<T> {
    item: T | null;
    local?: LocalStorage<T>;
    fields: FieldConfig<T>[];
    onSubmit?: (data: T) => Promise<void>;
    onDelete?: (id: string) => Promise<void>;
    onCancel?: () => void;
    isLoading?: boolean;
    isEditing?: boolean;
    emptyItem: () => T;
    title?: string;
}

export function SingleItemCrud<T extends { id: string }>({
                                                             item,
                                                             local,
                                                             fields,
                                                             onSubmit,
                                                             onDelete,
                                                             onCancel,
                                                             isLoading = false,
                                                             emptyItem,
                                                             title = 'Edit Item',
                                                         }: SingleItemCrudProps<T>) {
    let initItem: T;
    if (local) {
        initItem = local.loadDefault(emptyItem())
    } else {
        initItem = emptyItem();
    }

    const [formData, setFormData] = useState<T>(initItem);
    const [errors, setErrors] = useState<Record<string, string>>({});

    useEffect(() => {
        setFormData(item || emptyItem());
    }, [item]);

    const handleChange = (name: keyof T, value: any) => {
        setFormData(prev => ({
            ...prev,
            [name]: value,
        }));
        if (local) {
            local.save(formData)
        }
        // Clear error when field changes
        setErrors(prev => ({...prev, [name as string]: ''}));
    };

    const validate = () => {
        const newErrors: Record<string, string> = {};
        fields.forEach(field => {
            if (field.required && !formData[field.name]) {
                newErrors[field.name as string] = `${field.label} is required`;
            }
        });
        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleSubmit = async (e: React.FormEvent) => {
        if (!onSubmit) return;

        e.preventDefault();
        if (!validate()) return;

        try {
            await onSubmit(formData);
        } catch (error) {
            // Handle API errors
            setErrors(prev => ({
                ...prev,
                form: error instanceof Error ? error.message : 'An error occurred',
            }));
        }
    };

    const renderField = (field: FieldConfig<T>) => {
        const value = formData[field.name];
        const error = errors[field.name as string];
        const isDisabled = typeof field.disabled === 'function'
            ? field.disabled(formData)
            : field.disabled;

        if (field.component) {
            const FieldComponent = field.component;
            return (
                <FieldComponent
                    key={field.name as string}
                    value={value}
                    onChange={(val: any) => handleChange(field.name, val)}
                    item={formData}
                />
            );
        }

        switch (field.type) {
            case 'select':
                return (
                    <div key={field.name as string} className="form-group">
                        <label>{field.label}</label>
                        <select
                            value={value as any}
                            onChange={(e) => handleChange(field.name, e.target.value)}
                            disabled={isDisabled}
                        >
                            {field.options?.map(option => (
                                <option key={option.value} value={option.value}>
                                    {option.label}
                                </option>
                            ))}
                        </select>
                        {error && <div className="error">{error}</div>}
                    </div>
                );
            case 'checkbox':
                return (
                    <div key={field.name as string} className="form-group checkbox">
                        <label>
                            <input
                                type="checkbox"
                                checked={value as any}
                                onChange={(e) => handleChange(field.name, e.target.checked)}
                                disabled={isDisabled}
                            />
                            {field.label}
                        </label>
                        {error && <div className="error">{error}</div>}
                    </div>
                );
            default:
                return (
                    <div key={field.name as string} className="form-group">
                        <label>{field.label}</label>
                        <input
                            type={field.type}
                            value={value as any}
                            onChange={(e) => handleChange(field.name, e.target.value)}
                            required={field.required}
                            disabled={isDisabled}
                        />
                        {error && <div className="error">{error}</div>}
                    </div>
                );
        }
    };

    return (
        <div className="single-item-crud">
            <h2>{title}</h2>

            {errors.form && <div className="form-error">{errors.form}</div>}

            <form onSubmit={handleSubmit}>
                {fields.map(renderField)}

                <div className="actions">
                    {onSubmit && (<button
                            type="submit"
                            disabled={isLoading}
                        >
                            {isLoading ? 'Saving...' : 'Save'}
                        </button>
                    )}
                    {onCancel && (
                        <button
                            type="button"
                            onClick={onCancel}
                            disabled={isLoading}
                        >
                            Cancel
                        </button>
                    )}

                    {onDelete && item?.id && (
                        <button
                            type="button"
                            onClick={() => onDelete(item.id)}
                            disabled={isLoading}
                            className="delete"
                        >
                            Delete
                        </button>
                    )}
                </div>
            </form>
        </div>
    );
}


interface PopupCrudProps<T extends { id: string }> {
    trigger: React.ReactNode;
    local?: LocalStorage<T>;
    fields: FieldConfig<T>[];
    onSave?: (data: T) => Promise<void>;
    onDelete?: (id: string) => Promise<void>;
    emptyItem: () => T;
    item?: T | null;
    title?: string;
    size?: 'sm' | 'md' | 'lg';
}

export function PopupCrud<T extends { id: string }>({
                                                        trigger,
                                                        local,
                                                        fields,
                                                        onSave,
                                                        onDelete,
                                                        emptyItem,
                                                        item = null,
                                                        title = 'Edit Item',
                                                        size = 'md',
                                                    }: PopupCrudProps<T>) {
    const [isOpen, setIsOpen] = useState(false);
    const [isLoading, setIsLoading] = useState(false);
    const [currentItem, setCurrentItem] = useState<T | null>(item);

    const handleOpen = () => {
        setCurrentItem(item || emptyItem());
        setIsOpen(true);
    };

    const handleClose = () => {
        setIsOpen(false);
    };

    const handleSave = async (data: T) => {
        if (!onSave) return;

        setIsLoading(true);
        try {
            await onSave(data);
            handleClose();
        } finally {
            setIsLoading(false);
        }
    };

    const handleDelete = async (id: string) => {
        if (!onDelete) return;

        setIsLoading(true);
        try {
            await onDelete(id);
            handleClose();
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <>
            <div onClick={handleOpen} style={{display: 'inline-block'}}>
                {trigger}
            </div>

            {isOpen && (
                <div className="popup-overlay">
                    <div className={`popup-container ${size}`}>
                        <div className="popup-header">
                            <h3>{title}</h3>
                            <button onClick={handleClose} className="close-btn">
                                &times;
                            </button>
                        </div>

                        <div className="popup-content">
                            <SingleItemCrud<T>
                                item={currentItem}
                                local={local}
                                fields={fields}
                                onSubmit={onSave ? handleSave : undefined}
                                onDelete={onDelete ? handleDelete : undefined}
                                onCancel={handleClose}
                                isLoading={isLoading}
                                emptyItem={emptyItem}
                            />
                        </div>
                    </div>
                </div>
            )}
        </>
    );
}