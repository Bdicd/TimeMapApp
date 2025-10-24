// Конфигурация API
const API_BASE = 'http://localhost:5229';

// Конфигурация приложения
const CONFIG = {
    API_BASE,
    API_ENDPOINTS: {
        USERS: `${API_BASE}/api/user`,
        AVAILABILITIES: `${API_BASE}/api/availability`,
        SCHEDULE: `${API_BASE}/api/schedule`
    }
};
