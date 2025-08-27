# Руководство: Создание MainMenuUI префаба

## Обзор
MainMenuUI - это главное меню игры с анимированными элементами, кнопками навигации и красивым дизайном.

## Шаг 1: Создание базовой структуры

### В Canvas создайте:
1. **Create Empty** → "MainMenuPanel"
2. **Добавить компонент MainMenuUI** (скрипт)
3. **Добавить CanvasGroup** для анимаций
4. **ВАЖНО: Подключить CanvasGroup к скрипту**
   - В Inspector выбрать MainMenuPanel
   - В компоненте MainMenuUI найти поле "Canvas Group"
   - Перетащить CanvasGroup компонент в это поле
4. **Добавить Image** для фона (если нужен)

## Шаг 2: Создание UI элементов

### 2.1 Заголовок игры
- **GameObject** → "HeaderContainer"
  - **TextMeshPro - Text (UI)** → "GameTitleText"
    - Text: "SCRAP ARCHITECT"
    - Font Size: 72
    - Color: #ECF0F1
    - Alignment: Center
    - Font Style: Bold
  - **TextMeshPro - Text (UI)** → "SubtitleText"
    - Text: "Создавайте безумные машины из хлама!"
    - Font Size: 24
    - Color: #BDC3C7
    - Alignment: Center
  - **TextMeshPro - Text (UI)** → "VersionText"
    - Text: "Версия 1.0.0"
    - Font Size: 16
    - Color: #7F8C8D
    - Alignment: Center

### 2.2 Логотип (опционально)
- **GameObject** → "LogoContainer"
  - **Image** → "LogoImage"
    - Source Image: ваш логотип
    - Color: White
    - Preserve Aspect: true

### 2.3 Основные кнопки
- **GameObject** → "ButtonsContainer"
  - **Vertical Layout Group** для размещения кнопок

#### Кнопка "ИГРАТЬ"
- **Button** → "PlayButton"
  - **TextMeshPro - Text (UI)** → "PlayText"
    - Text: "ИГРАТЬ"
    - Font Size: 24
    - Color: #ECF0F1
    - Alignment: Center

#### Кнопка "НАСТРОЙКИ"
- **Button** → "SettingsButton"
  - **TextMeshPro - Text (UI)** → "SettingsText"
    - Text: "НАСТРОЙКИ"
    - Font Size: 24
    - Color: #ECF0F1
    - Alignment: Center

#### Кнопка "ОБ ИГРЕ"
- **Button** → "CreditsButton"
  - **TextMeshPro - Text (UI)** → "CreditsText"
    - Text: "ОБ ИГРЕ"
    - Font Size: 24
    - Color: #ECF0F1
    - Alignment: Center

#### Кнопка "ВЫХОД"
- **Button** → "QuitButton"
  - **TextMeshPro - Text (UI)** → "QuitText"
    - Text: "ВЫХОД"
    - Font Size: 24
    - Color: #ECF0F1
    - Alignment: Center

### 2.4 Декоративные элементы (опционально)
- **GameObject** → "DecorativeElements"
  - Добавить любые декоративные изображения или эффекты

## Шаг 3: Настройка Layout Groups

### 3.1 Основной Layout
- **Vertical Layout Group** на MainMenuPanel
  - Spacing: 30
  - Padding: 50, 50, 50, 50
  - Child Alignment: Middle Center
  - Child Control Width: true
  - Child Control Height: false
  - Child Force Expand Width: false
  - Child Force Expand Height: false

### 3.2 Контейнер заголовка
- **Vertical Layout Group** на HeaderContainer
  - Spacing: 10
  - Padding: 0, 0, 0, 0
  - Child Alignment: Middle Center
  - Child Control Width: true
  - Child Control Height: false
  - Child Force Expand Width: false
  - Child Force Expand Height: false

### 3.3 Контейнер кнопок
- **Vertical Layout Group** на ButtonsContainer
  - Spacing: 15
  - Padding: 0, 0, 0, 0
  - Child Alignment: Middle Center
  - Child Control Width: true
  - Child Control Height: false
  - Child Force Expand Width: false
  - Child Force Expand Height: false

## Шаг 4: Настройка размеров и позиций

### 4.1 Основная панель
- **RectTransform**: Anchor Min (0, 0), Anchor Max (1, 1)
- **Offset**: Min (0, 0), Max (0, 0)

### 4.2 Заголовок
- **Height**: 200
- **Anchor**: Top Center

### 4.3 Логотип
- **Width**: 200
- **Height**: 200
- **Anchor**: Middle Center

### 4.4 Контейнер кнопок
- **Width**: 300
- **Height**: 300
- **Anchor**: Middle Center

### 4.5 Отдельные кнопки
- **Width**: 250
- **Height**: 60
- **Anchor**: Middle Center

## Шаг 5: Настройка MainMenuUI скрипта

### Подключить все UI элементы к скрипту:
- **Game Title Text**: GameTitleText
- **Version Text**: VersionText
- **Subtitle Text**: SubtitleText
- **Play Button**: PlayButton
- **Settings Button**: SettingsButton
- **Credits Button**: CreditsButton
- **Quit Button**: QuitButton
- **Background Image**: (если есть)
- **Logo Image**: LogoImage
- **Decorative Elements**: массив декоративных элементов

### Настройка анимации:
- **Button Stagger Delay**: 0.1
- **Title Animation Duration**: 1.0

## Шаг 6: Настройка кнопок

### Для каждой кнопки:
1. **Image компонент**:
   - Color: #2C3E50 (Primary)
   - Type: Sliced
   - Source Image: UI Button sprite

2. **Button компонент**:
   - Transition: Color Tint
   - Normal Color: #2C3E50
   - Highlighted Color: #34495E
   - Pressed Color: #3498DB
   - Selected Color: #3498DB
   - Disabled Color: #7F8C8D
   - Fade Duration: 0.1

3. **Навигация**:
   - Navigation: Automatic

## Шаг 7: Создание префаба

### 7.1 Создание префаба
1. Перетащить MainMenuPanel в папку Prefabs/UI/Panels/
2. Удалить из сцены
3. Подключить к UIManager в поле "Main Menu Panel"

### 7.2 Настройка UIManager
- Добавить MainMenuPanel в список панелей
- Настроить как стартовую панель

## Шаг 8: Тестирование

### 8.1 Базовая функциональность
1. Запустить сцену
2. Проверить отображение главного меню
3. Протестировать все кнопки
4. Проверить переходы между экранами

### 8.2 Анимации
1. Проверить анимацию появления заголовка
2. Протестировать анимацию кнопок
3. Проверить анимацию декоративных элементов

## Полезные советы

1. **Используйте Content Size Fitter** для автоматического размера текста
2. **Примените цвета из UIColors** для единообразия
3. **Настройте правильные Anchors** для адаптивности
4. **Используйте Layout Groups** для автоматического позиционирования
5. **Тестируйте на разных разрешениях**

## Цветовая схема

- **Primary**: #2C3E50 (темно-синий)
- **Secondary**: #3498DB (голубой)
- **Text**: #ECF0F1 (белый)
- **TextSecondary**: #BDC3C7 (серый)
- **Background**: #34495E (темно-серый)

## Следующие шаги

После создания MainMenuUI:
1. Создать ContractSelectionUI префаб
2. Настроить переходы между экранами
3. Добавить звуковые эффекты
4. Интегрировать с игровой логикой
