# Руководство: Создание VictoryScreen префаба

## Обзор
VictoryScreen - это экран победы, который отображается при успешном завершении контракта. Включает информацию о выполненном контракте, награды, время выполнения и кнопки навигации.

## Шаг 1: Создание базовой структуры

### В Canvas создайте:
1. **Create Empty** → "VictoryPanel"
2. **Добавить компонент VictoryScreen** (скрипт)
3. **Добавить CanvasGroup** для анимаций

## Шаг 2: Создание UI элементов

### 2.1 Заголовок победы
- **GameObject** → "VictoryHeader"
  - **TextMeshPro - Text (UI)** → "VictoryTitleText"
    - Text: "ПОБЕДА!"
    - Font Size: 72
    - Color: #27AE60 (Success)
    - Alignment: Center
    - Font Style: Bold

### 2.2 Информация о контракте
- **GameObject** → "ContractInfoPanel"
  - **TextMeshPro - Text (UI)** → "ContractTitleText"
    - Text: "Контракт выполнен!"
    - Font Size: 32
    - Color: #ECF0F1
    - Alignment: Center
  - **TextMeshPro - Text (UI)** → "CompletionTimeText"
    - Text: "Время выполнения: 00:00"
    - Font Size: 24
    - Color: #F39C12
    - Alignment: Center
  - **TextMeshPro - Text (UI)** → "ScoreText"
    - Text: "Очки: 1000"
    - Font Size: 28
    - Color: #3498DB
    - Alignment: Center

### 2.3 Панель наград
- **GameObject** → "RewardsPanel"
  - **TextMeshPro - Text (UI)** → "RewardsTitleText"
    - Text: "НАГРАДЫ"
    - Font Size: 24
    - Color: #ECF0F1
    - Alignment: Center
    - Font Style: Bold
  - **TextMeshPro - Text (UI)** → "ScrapRewardText"
    - Text: "Металлолом: +500"
    - Font Size: 20
    - Color: #F39C12
    - Alignment: Center
  - **TextMeshPro - Text (UI)** → "ExperienceRewardText"
    - Text: "Опыт: +100"
    - Font Size: 20
    - Color: #3498DB
    - Alignment: Center
  - **GameObject** → "RewardIconsContainer"
    - **Horizontal Layout Group** для размещения иконок наград

### 2.4 Панель целей
- **GameObject** → "ObjectivesPanel"
  - **TextMeshPro - Text (UI)** → "ObjectivesTitleText"
    - Text: "ВЫПОЛНЕННЫЕ ЦЕЛИ"
    - Font Size: 20
    - Color: #ECF0F1
    - Alignment: Center
    - Font Style: Bold
  - **GameObject** → "ObjectivesContainer"
    - **Vertical Layout Group** для размещения целей

### 2.5 Кнопки действий
- **GameObject** → "ButtonsPanel"
  - **Button** → "ContinueButton"
    - **TextMeshPro - Text (UI)** → "ContinueText"
      - Text: "ПРОДОЛЖИТЬ"
      - Font Size: 20
      - Color: #ECF0F1
      - Alignment: Center
  - **Button** → "RetryButton"
    - **TextMeshPro - Text (UI)** → "RetryText"
      - Text: "ПОВТОРИТЬ"
      - Font Size: 20
      - Color: #ECF0F1
      - Alignment: Center
  - **Button** → "MainMenuButton"
    - **TextMeshPro - Text (UI)** → "MainMenuText"
      - Text: "ГЛАВНОЕ МЕНЮ"
      - Font Size: 20
      - Color: #ECF0F1
      - Alignment: Center

## Шаг 3: Настройка Layout Groups

### 3.1 Основной Layout
- **Vertical Layout Group** на VictoryPanel
  - Spacing: 30
  - Padding: 50, 50, 50, 50
  - Child Alignment: Middle Center
  - Child Control Width: true
  - Child Control Height: false
  - Child Force Expand Width: false
  - Child Force Expand Height: false

### 3.2 Панель информации о контракте
- **Vertical Layout Group** на ContractInfoPanel
  - Spacing: 15
  - Padding: 20, 20, 20, 20
  - Child Alignment: Middle Center
  - Child Control Width: true
  - Child Control Height: false
  - Child Force Expand Width: false
  - Child Force Expand Height: false

### 3.3 Панель наград
- **Vertical Layout Group** на RewardsPanel
  - Spacing: 10
  - Padding: 20, 20, 20, 20
  - Child Alignment: Middle Center
  - Child Control Width: true
  - Child Control Height: false
  - Child Force Expand Width: false
  - Child Force Expand Height: false

### 3.4 Контейнер иконок наград
- **Horizontal Layout Group** на RewardIconsContainer
  - Spacing: 20
  - Padding: 10, 10, 10, 10
  - Child Alignment: Middle Center
  - Child Control Width: true
  - Child Control Height: true
  - Child Force Expand Width: false
  - Child Force Expand Height: false

### 3.5 Контейнер целей
- **Vertical Layout Group** на ObjectivesContainer
  - Spacing: 10
  - Padding: 10, 10, 10, 10
  - Child Alignment: Upper Center
  - Child Control Width: true
  - Child Control Height: false
  - Child Force Expand Width: false
  - Child Force Expand Height: false

### 3.6 Панель кнопок
- **Horizontal Layout Group** на ButtonsPanel
  - Spacing: 20
  - Padding: 20, 20, 20, 20
  - Child Alignment: Middle Center
  - Child Control Width: true
  - Child Control Height: true
  - Child Force Expand Width: false
  - Child Force Expand Height: false

## Шаг 4: Настройка размеров и позиций

### 4.1 Основная панель
- **RectTransform**: Anchor Min (0, 0), Anchor Max (1, 1)
- **Offset**: Min (0, 0), Max (0, 0)

### 4.2 Заголовок победы
- **Height**: 120
- **Anchor**: Top Center

### 4.3 Панель информации о контракте
- **Height**: 150
- **Anchor**: Top Center

### 4.4 Панель наград
- **Height**: 200
- **Anchor**: Middle Center

### 4.5 Панель целей
- **Height**: 300
- **Anchor**: Middle Center

### 4.6 Панель кнопок
- **Height**: 80
- **Anchor**: Bottom Center

## Шаг 5: Настройка VictoryScreen скрипта

### Подключить все UI элементы к скрипту:
- **Victory Title Text**: VictoryTitleText
- **Contract Title Text**: ContractTitleText
- **Completion Time Text**: CompletionTimeText
- **Score Text**: ScoreText
- **Scrap Reward Text**: ScrapRewardText
- **Experience Reward Text**: ExperienceRewardText
- **Reward Icons**: массив иконок наград
- **Objectives Container**: ObjectivesContainer
- **Continue Button**: ContinueButton
- **Retry Button**: RetryButton
- **Main Menu Button**: MainMenuButton

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
1. Перетащить VictoryPanel в папку Prefabs/UI/Panels/
2. Удалить из сцены
3. Подключить к UIManager в поле "Victory Panel"

### 7.2 Настройка UIManager
- Добавить VictoryPanel в список панелей
- Настроить переходы между экранами

## Шаг 8: Тестирование

### 8.1 Базовая функциональность
1. Запустить сцену
2. Перейти к экрану победы
3. Проверить отображение всех элементов
4. Протестировать кнопки "Продолжить", "Повторить", "Главное меню"

### 8.2 Анимации
1. Проверить анимацию появления панели
2. Протестировать анимацию наград
3. Проверить переходы между экранами

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
- **Success**: #27AE60 (зеленый)
- **Warning**: #F39C12 (оранжевый)
- **Error**: #E74C3C (красный)

## Следующие шаги

После создания VictoryScreen:
1. Создать DefeatScreen префаб
2. Настроить анимации наград
3. Интегрировать с ContractManager
4. Добавить звуковые эффекты
