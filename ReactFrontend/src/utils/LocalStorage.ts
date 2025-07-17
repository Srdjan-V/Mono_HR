export class LocalStorage<T> {
    readonly key: string;

    constructor(key: string) {
        this.key = key;
    }

    loadDefault(data: T): T {
        let loaded = this.load();
        if (loaded == null) {
            return data;
        }
        return loaded;
    }

    load(): T | null {
        try {
            const item = localStorage.getItem(this.key);
            return item ? JSON.parse(item) as T : null;
        } catch (error) {
            console.error(`Error loading data from localStorage for key "${this.key}":`, error);
            return null;
        }
    }

    save(data: T): void {
        try {
            localStorage.setItem(this.key, JSON.stringify(data));
        } catch (error) {
            console.error(`Error saving data to localStorage for key "${this.key}":`, error);
        }
    }

    clear(): void {
        try {
            localStorage.removeItem(this.key);
        } catch (error) {
            console.error(`Error clearing localStorage for key "${this.key}":`, error);
        }
    }

    hasData(): boolean {
        return localStorage.getItem(this.key) !== null;
    }

    update(updater: (current: T | null) => T): void {
        const current = this.load();
        this.save(updater(current));
    }
}

