// Модуль для поиска лучших слотов
class SchedulePage {
    constructor() {
        this.users = [];
        this.init();
    }

    init() {
        this.setupEventListeners();
        this.loadUsers();
    }

    setupEventListeners() {
        const form = document.getElementById('find-slots-form');
        if (form) {
            form.addEventListener('submit', (e) => {
                e.preventDefault();
                this.handleFindSlots();
            });
        }
    }

    async loadUsers() {
        try {
            const users = await api.getUsers();
            this.users = users;
        } catch (error) {
            this.showError(`Ошибка загрузки пользователей: ${error.message}`);
        }
    }

    async handleFindSlots() {
        const startInput = document.getElementById('schedule-start');
        const endInput = document.getElementById('schedule-end');

        const startTime = startInput.value;
        const endTime = endInput.value;

        // Валидация
        if (!startTime || !endTime) {
            this.showError('Пожалуйста, заполните все поля');
            return;
        }

        // Проверяем, что начало раньше конца
        const startDate = new Date(startTime);
        const endDate = new Date(endTime);
        
        if (startDate >= endDate) {
            this.showError('Время начала должно быть раньше времени окончания');
            return;
        }

        try {
            // Конвертируем в ISO строки для API
            const startUtc = startDate.toISOString();
            const endUtc = endDate.toISOString();

            const slots = await api.getBestSlots(startUtc, endUtc);
            this.renderSlots(slots);
            
        } catch (error) {
            this.showError(`Ошибка поиска слотов: ${error.message}`);
        }
    }

    renderSlots(slots) {
        const resultsDiv = document.getElementById('slots-results');
        if (!resultsDiv) return;

        if (!slots || slots.length === 0) {
            resultsDiv.innerHTML = '<p class="text-center">Подходящие слоты не найдены</p>';
            return;
        }

        const slotsHtml = slots.map(slot => {
            const startDate = new Date(slot.startTimeUtc);
            const endDate = new Date(slot.endTimeUtc);
            
            // Получаем имена пользователей для отображения
            const userIds = slot.userIds || [];
            const userNames = userIds.map(userId => {
                const user = this.users.find(u => u.id === userId);
                return user ? user.name : `ID: ${userId}`;
            }).join(', ');

            return `
                <div class="slot-item">
                    <div class="slot-info">
                        <strong>Время:</strong> ${this.formatDateTime(startDate)} - ${this.formatDateTime(endDate)}<br>
                        <strong>Пользователи:</strong> ${userNames || 'Не указаны'}<br>
                        <strong>Продолжительность:</strong> ${this.formatDuration(startDate, endDate)}
                    </div>
                </div>
            `;
        }).join('');

        resultsDiv.innerHTML = `
            <h3>Найденные слоты (${slots.length}):</h3>
            <div class="slots-list">
                ${slotsHtml}
            </div>
        `;
    }

    formatDateTime(date) {
        return date.toLocaleString('ru-RU', {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit',
            hour: '2-digit',
            minute: '2-digit',
            second: '2-digit',
            timeZone: 'UTC'
        }) + ' UTC';
    }

    formatDuration(startDate, endDate) {
        const durationMs = endDate - startDate;
        const hours = Math.floor(durationMs / (1000 * 60 * 60));
        const minutes = Math.floor((durationMs % (1000 * 60 * 60)) / (1000 * 60));
        
        if (hours > 0) {
            return `${hours}ч ${minutes}м`;
        } else {
            return `${minutes}м`;
        }
    }

    showError(message) {
        // Используем глобальную функцию показа ошибок из app.js
        if (window.app && window.app.showError) {
            window.app.showError(message);
        } else {
            alert(message);
        }
    }
}
