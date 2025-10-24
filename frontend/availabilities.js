// Модуль для управления доступностью
class AvailabilitiesPage {
    constructor() {
        this.users = [];
        this.init();
    }

    init() {
        this.setupEventListeners();
        this.loadData();
    }

    setupEventListeners() {
        const form = document.getElementById('add-availability-form');
        if (form) {
            form.addEventListener('submit', (e) => {
                e.preventDefault();
                this.handleAddAvailability();
            });
        }
    }

    async loadData() {
        try {
            // Загружаем пользователей и доступность параллельно
            const [users, availabilities] = await Promise.all([
                api.getUsers(),
                api.getAvailabilities()
            ]);
            
            this.users = users;
            this.renderUsersSelect(users);
            this.renderAvailabilities(availabilities);
        } catch (error) {
            this.showError(`Ошибка загрузки данных: ${error.message}`);
        }
    }

    renderUsersSelect(users) {
        const select = document.getElementById('availability-user');
        if (!select) return;

        // Очищаем и добавляем опции
        select.innerHTML = '<option value="">Выберите пользователя</option>';
        
        users.forEach(user => {
            const option = document.createElement('option');
            option.value = user.id;
            option.textContent = `${user.name} (ID: ${user.id})`;
            select.appendChild(option);
        });
    }

    async handleAddAvailability() {
        const userSelect = document.getElementById('availability-user');
        const passwordInput = document.getElementById('availability-password');
        const startInput = document.getElementById('availability-start');
        const endInput = document.getElementById('availability-end');

        const userId = userSelect.value;
        const password = passwordInput.value.trim();
        const startTime = startInput.value;
        const endTime = endInput.value;

        // Валидация
        if (!userId || !password || !startTime || !endTime) {
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
            const startTimeUtc = startDate.toISOString();
            const endTimeUtc = endDate.toISOString();

            await api.addAvailability(userId, password, startTimeUtc, endTimeUtc);
            
            // Очищаем форму
            userSelect.value = '';
            passwordInput.value = '';
            startInput.value = '';
            endInput.value = '';
            
            // Обновляем список доступности
            await this.loadAvailabilities();
            
            this.showSuccess('Доступность успешно добавлена');
        } catch (error) {
            this.showError(`Ошибка добавления доступности: ${error.message}`);
        }
    }

    async loadAvailabilities() {
        try {
            const availabilities = await api.getAvailabilities();
            this.renderAvailabilities(availabilities);
        } catch (error) {
            this.showError(`Ошибка загрузки доступности: ${error.message}`);
        }
    }

    renderAvailabilities(availabilities) {
        const availabilitiesList = document.getElementById('availabilities-list');
        if (!availabilitiesList) return;

        if (!availabilities || availabilities.length === 0) {
            availabilitiesList.innerHTML = '<p class="text-center">Доступность не найдена</p>';
            return;
        }

        const availabilitiesHtml = availabilities.map(availability => {
            const user = this.users.find(u => u.id === availability.userId);
            const userName = user ? user.name : `ID: ${availability.userId}`;
            
            const startDate = new Date(availability.startTimeUtc);
            const endDate = new Date(availability.endTimeUtc);
            
            return `
                <div class="availability-item">
                    <div class="availability-info">
                        <strong>Пользователь:</strong> ${userName}<br>
                        <strong>Начало:</strong> ${this.formatDateTime(startDate)}<br>
                        <strong>Конец:</strong> ${this.formatDateTime(endDate)}<br>
                        <strong>ID:</strong> ${availability.id}
                    </div>
                </div>
            `;
        }).join('');

        availabilitiesList.innerHTML = availabilitiesHtml;
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

    showError(message) {
        // Используем глобальную функцию показа ошибок из app.js
        if (window.app && window.app.showError) {
            window.app.showError(message);
        } else {
            alert(message);
        }
    }

    showSuccess(message) {
        // Простое уведомление об успехе
        const successDiv = document.createElement('div');
        successDiv.className = 'success-message';
        successDiv.style.cssText = `
            background: #27ae60;
            color: white;
            padding: 10px;
            border-radius: 4px;
            margin: 10px 0;
            text-align: center;
        `;
        successDiv.textContent = message;
        
        const form = document.getElementById('add-availability-form');
        if (form) {
            form.parentNode.insertBefore(successDiv, form);
            
            // Убираем уведомление через 3 секунды
            setTimeout(() => {
                if (successDiv.parentNode) {
                    successDiv.parentNode.removeChild(successDiv);
                }
            }, 3000);
        }
    }
}
