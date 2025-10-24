// API модуль для работы с TimeMap API
class TimeMapAPI {
    constructor() {
        this.baseUrl = CONFIG.API_BASE;
    }

    // Обработка ошибок HTTP
    async handleResponse(response) {
        if (!response.ok) {
            let errorMessage = `HTTP ${response.status}: ${response.statusText}`;
            
            try {
                const errorData = await response.json();
                if (errorData.message) {
                    errorMessage = errorData.message;
                }
            } catch (e) {
                // Если не удалось распарсить JSON, используем стандартное сообщение
            }
            
            throw new Error(errorMessage);
        }
        
        return await response.json();
    }

    // Получить всех пользователей
    async getUsers() {
        try {
            const response = await fetch(`${this.baseUrl}/api/user`);
            return await this.handleResponse(response);
        } catch (error) {
            console.error('Ошибка получения пользователей:', error);
            throw error;
        }
    }

    // Создать нового пользователя
    async createUser(name, password) {
        try {
            const response = await fetch(`${this.baseUrl}/api/user`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    name: name,
                    password: password
                })
            });
            return await this.handleResponse(response);
        } catch (error) {
            console.error('Ошибка создания пользователя:', error);
            throw error;
        }
    }

    // Получить все доступности
    async getAvailabilities() {
        try {
            const response = await fetch(`${this.baseUrl}/api/availability`);
            return await this.handleResponse(response);
        } catch (error) {
            console.error('Ошибка получения доступности:', error);
            throw error;
        }
    }

    // Добавить доступность
    async addAvailability(userId, password, startTimeUtc, endTimeUtc) {
        try {
            const response = await fetch(`${this.baseUrl}/api/availability/${userId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    password: password,
                    startTimeUtc: startTimeUtc,
                    endTimeUtc: endTimeUtc
                })
            });
            return await this.handleResponse(response);
        } catch (error) {
            console.error('Ошибка добавления доступности:', error);
            throw error;
        }
    }

    // Получить лучшие слоты
    async getBestSlots(startUtc, endUtc) {
        try {
            const params = new URLSearchParams({
                startUtc: startUtc,
                endUtc: endUtc
            });
            
            const response = await fetch(`${this.baseUrl}/api/schedule/best-slots?${params}`);
            return await this.handleResponse(response);
        } catch (error) {
            console.error('Ошибка получения лучших слотов:', error);
            throw error;
        }
    }
}

// Создаем глобальный экземпляр API
const api = new TimeMapAPI();
