// Модуль для управления пользователями
class UsersPage {
    constructor() {
        this.init();
    }

    init() {
        this.setupEventListeners();
        this.loadUsers();
    }

    setupEventListeners() {
        const form = document.getElementById('create-user-form');
        if (form) {
            form.addEventListener('submit', (e) => {
                e.preventDefault();
                this.handleCreateUser();
            });
        }
    }

    async loadUsers() {
        try {
            const users = await api.getUsers();
            this.renderUsers(users);
        } catch (error) {
            this.showError(`Ошибка загрузки пользователей: ${error.message}`);
        }
    }

    async handleCreateUser() {
        const nameInput = document.getElementById('user-name');
        const passwordInput = document.getElementById('user-password');
        
        const name = nameInput.value.trim();
        const password = passwordInput.value.trim();

        // Валидация
        if (!name || !password) {
            this.showError('Пожалуйста, заполните все поля');
            return;
        }

        try {
            await api.createUser(name, password);
            
            // Очищаем форму
            nameInput.value = '';
            passwordInput.value = '';
            
            // Обновляем список пользователей
            await this.loadUsers();
            
            this.showSuccess('Пользователь успешно создан');
        } catch (error) {
            this.showError(`Ошибка создания пользователя: ${error.message}`);
        }
    }

    renderUsers(users) {
        const usersList = document.getElementById('users-list');
        if (!usersList) return;

        if (!users || users.length === 0) {
            usersList.innerHTML = '<p class="text-center">Пользователи не найдены</p>';
            return;
        }

        const usersHtml = users.map(user => `
            <div class="user-item">
                <div class="user-info">
                    <strong>ID:</strong> ${user.id}<br>
                    <strong>Имя:</strong> ${user.name}
                </div>
            </div>
        `).join('');

        usersList.innerHTML = usersHtml;
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
        
        const form = document.getElementById('create-user-form');
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
