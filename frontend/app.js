// Главный файл приложения - роутинг и инициализация
class TimeMapApp {
    constructor() {
        this.currentPage = 'users';
        this.init();
    }

    init() {
        this.setupNavigation();
        this.loadPage('users');
    }

    setupNavigation() {
        const navButtons = document.querySelectorAll('.nav-btn');
        navButtons.forEach(btn => {
            btn.addEventListener('click', (e) => {
                const page = e.target.dataset.page;
                this.navigateToPage(page);
            });
        });
    }

    navigateToPage(page) {
        // Обновляем активную кнопку навигации
        document.querySelectorAll('.nav-btn').forEach(btn => {
            btn.classList.remove('active');
        });
        document.querySelector(`[data-page="${page}"]`).classList.add('active');

        this.currentPage = page;
        this.loadPage(page);
    }

    async loadPage(page) {
        const mainContent = document.getElementById('main-content');
        
        try {
            this.showLoading(true);
            this.hideError();

            switch (page) {
                case 'users':
                    await this.loadUsersPage();
                    break;
                case 'availabilities':
                    await this.loadAvailabilitiesPage();
                    break;
                case 'schedule':
                    await this.loadSchedulePage();
                    break;
                default:
                    mainContent.innerHTML = '<h2>Страница не найдена</h2>';
            }
        } catch (error) {
            this.showError(`Ошибка загрузки страницы: ${error.message}`);
        } finally {
            this.showLoading(false);
        }
    }

    async loadUsersPage() {
        const mainContent = document.getElementById('main-content');
        mainContent.innerHTML = `
            <h2>Управление пользователями</h2>
            <div id="users-content">
                <div class="form-section">
                    <h3>Создать нового пользователя</h3>
                    <form id="create-user-form">
                        <div class="form-group">
                            <label for="user-name">Имя пользователя:</label>
                            <input type="text" id="user-name" required>
                        </div>
                        <div class="form-group">
                            <label for="user-password">Пароль:</label>
                            <input type="password" id="user-password" required>
                        </div>
                        <button type="submit">Создать пользователя</button>
                    </form>
                </div>
                <div id="users-list" class="user-list">
                    <!-- Список пользователей будет загружен здесь -->
                </div>
            </div>
        `;

        // Инициализируем функциональность страницы пользователей
        if (typeof UsersPage !== 'undefined') {
            new UsersPage();
        }
    }

    async loadAvailabilitiesPage() {
        const mainContent = document.getElementById('main-content');
        mainContent.innerHTML = `
            <h2>Управление доступностью</h2>
            <div id="availabilities-content">
                <div class="form-section">
                    <h3>Добавить доступность</h3>
                    <form id="add-availability-form">
                        <div class="form-group">
                            <label for="availability-user">Пользователь:</label>
                            <select id="availability-user" required>
                                <option value="">Выберите пользователя</option>
                            </select>
                        </div>
                        <div class="form-group">
                            <label for="availability-password">Пароль:</label>
                            <input type="password" id="availability-password" required>
                        </div>
                        <div class="form-group">
                            <label for="availability-start">Начало (UTC):</label>
                            <input type="datetime-local" id="availability-start" required>
                        </div>
                        <div class="form-group">
                            <label for="availability-end">Конец (UTC):</label>
                            <input type="datetime-local" id="availability-end" required>
                        </div>
                        <button type="submit">Добавить доступность</button>
                    </form>
                </div>
                <div id="availabilities-list" class="availability-list">
                    <!-- Список доступности будет загружен здесь -->
                </div>
            </div>
        `;

        // Инициализируем функциональность страницы доступности
        if (typeof AvailabilitiesPage !== 'undefined') {
            new AvailabilitiesPage();
        }
    }

    async loadSchedulePage() {
        const mainContent = document.getElementById('main-content');
        mainContent.innerHTML = `
            <h2>Поиск лучших слотов</h2>
            <div id="schedule-content">
                <div class="form-section">
                    <h3>Найти лучшие слоты</h3>
                    <form id="find-slots-form">
                        <div class="form-group">
                            <label for="schedule-start">Начало периода (UTC):</label>
                            <input type="datetime-local" id="schedule-start" required>
                        </div>
                        <div class="form-group">
                            <label for="schedule-end">Конец периода (UTC):</label>
                            <input type="datetime-local" id="schedule-end" required>
                        </div>
                        <button type="submit">Найти лучшие слоты</button>
                    </form>
                </div>
                <div id="slots-results" class="slots-results">
                    <!-- Результаты поиска слотов будут отображены здесь -->
                </div>
            </div>
        `;

        // Инициализируем функциональность страницы расписания
        if (typeof SchedulePage !== 'undefined') {
            new SchedulePage();
        }
    }

    showLoading(show) {
        const loading = document.getElementById('loading');
        if (show) {
            loading.classList.remove('hidden');
        } else {
            loading.classList.add('hidden');
        }
    }

    showError(message) {
        const error = document.getElementById('error');
        const errorMessage = document.getElementById('error-message');
        errorMessage.textContent = message;
        error.classList.remove('hidden');
    }

    hideError() {
        const error = document.getElementById('error');
        error.classList.add('hidden');
    }
}

// Инициализация приложения при загрузке страницы
document.addEventListener('DOMContentLoaded', () => {
    window.app = new TimeMapApp();
});
