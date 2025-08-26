# Руководство: Создание GameplayUI префаба

## Обзор
GameplayUI - это игровой интерфейс, который отображается во время выполнения контракта. Включает информацию о контракте, цели, прогресс и элементы управления.

## Шаг 1: Создание базовой структуры

### В Canvas создайте:
1. **Create Empty** → "GameplayPanel"
2. **Добавить компонент GameplayUI** (скрипт)
3. **Добавить CanvasGroup** для анимаций

## Шаг 2: Создание UI элементов

### 2.1 Панель информации о контракте
- **GameObject** → "ContractInfoPanel"
  - **TextMeshPro - Text (UI)** → "ContractTitleText"
    - Text: "Название контракта"
    - Font Size: 24
    - Color: #ECF0F1
    - Alignment: Left
  - **TextMeshPro - Text (UI)** → "ContractDescriptionText"
    - Text: "Описание контракта..."
    - Font Size: 16
    - Color: #BDC3C7
    - Alignment: Left
  - **TextMeshPro - Text (UI)** → "TimeText"
    - Text: "Время: 00:00"
    - Font Size: 18
    - Color: #F39C12
    - Alignment: Right
  - **TextMeshPro - Text (UI)** → "DifficultyText"
    - Text: "Сложность: Средняя"
    - Font Size: 16
    - Color: #E74C3C
    - Alignment: Right

### 2.2 Панель целей
- **GameObject** → "ObjectivesPanel"
  - **TextMeshPro - Text (UI)** → "ObjectivesTitleText"
    - Text: "ЦЕЛИ КОНТРАКТА"
    - Font Size: 20
    - Color: #ECF0F1
    - Alignment: Center
  - **GameObject** → "ObjectivesContainer"
    - **Vertical Layout Group** для размещения целей

### 2.3 Панель прогресса
- **GameObject** → "ProgressPanel"
  - **Slider** → "OverallProgressSlider"
    - Min Value: 0
    - Max Value: 1
    - Value: 0
  - **TextMeshPro - Text (UI)** → "ProgressText"
    - Text: "Прогресс: 0%"
    - Font Size: 18
    - Color: #27AE60
    - Alignment: Center

### 2.4 Панель управления
- **GameObject** → "ControlsPanel"
  - **Button** → "PauseButton"
    - **TextMeshPro - Text (UI)** → "PauseText"
      - Text: "ПАУЗА"
      - Font Size: 16
      - Color: #ECF0F1
  - **Button** → "ObjectivesButton"
    - **TextMeshPro - Text (UI)** → "ObjectivesText"
      - Text: "ЦЕЛИ"
      - Font Size: 16
      - Color: #ECF0F1
  - **Button** → "HelpButton"
    - **TextMeshPro - Text (UI)** → "HelpText"
      - Text: "ПОМОЩЬ"
      - Font Size: 16
      - Color: #ECF0F1

### 2.5 Панель помощи
- **GameObject** → "HelpPanel"
  - **Image** (фон)
  - **TextMeshPro - Text (UI)** → "HelpText"
    - Text: "Справка по управлению..."
    - Font Size: 16
    - Color: #ECF0F1
    - Alignment: Left

## Шаг 3: Настройка Layout Groups

### 3.1 Основной Layout
- **Vertical Layout Group** на GameplayPanel
  - Spacing: 10
  - Padding: 20, 20, 20, 20
  - Child Alignment: Upper Left
  - Child Control Width: true
  - Child Control Height: false
  - Child Force Expand Width: false
  - Child Force Expand Height: false

### 3.2 Панель информации о контракте
- **Horizontal Layout Group** на ContractInfoPanel
  - Spacing: 20
  - Padding: 10, 10, 10, 10
  - Child Alignment: Middle Left
  - Child Control Width: true
  - Child Control Height: true
  - Child Force Expand Width: false
  - Child Force Expand Height: false

### 3.3 Контейнер целей
- **Vertical Layout Group** на ObjectivesContainer
  - Spacing: 5
  - Padding: 5, 5, 5, 5
  - Child Alignment: Upper Center
  - Child Control Width: true
  - Child Control Height: false
  - Child Force Expand Width: false
  - Child Force Expand Height: false

### 3.4 Панель управления
- **Horizontal Layout Group** на ControlsPanel
  - Spacing: 10
  - Padding: 10, 10, 10, 10
  - Child Alignment: Middle Center
  - Child Control Width: true
  - Child Control Height: true
  - Child Force Expand Width: false
  - Child Force Expand Height: false

## Шаг 4: Настройка размеров и позиций

### 4.1 Основная панель
- **RectTransform**: Anchor Min (0, 0), Anchor Max (1, 1)
- **Offset**: Min (0, 0), Max (0, 0)

### 4.2 Панель информации о контракте
- **Height**: 100
- **Anchor**: Top Left

### 4.3 Панель целей
- **Width**: 300
- **Height**: 200
- **Anchor**: Top Right

### 4.4 Панель прогресса
- **Height**: 60
- **Anchor**: Bottom Center

### 4.5 Панель управления
- **Height**: 50
- **Anchor**: Bottom Left

### 4.6 Панель помощи
- **Width**: 400
- **Height**: 300
- **Anchor**: Center
- **Изначально скрыта**

## Шаг 5: Настройка GameplayUI скрипта

### Подключить все UI элементы к скрипту:
- **Contract Title Text**: ContractTitleText
- **Contract Description Text**: ContractDescriptionText
- **Time Text**: TimeText
- **Difficulty Text**: DifficultyText
- **Objectives Container**: ObjectivesContainer
- **Overall Progress Slider**: OverallProgressSlider
- **Progress Text**: ProgressText
- **Pause Button**: PauseButton
- **Objectives Button**: ObjectivesButton
- **Help Button**: HelpButton
- **Objectives Panel**: ObjectivesPanel
- **Help Panel**: HelpPanel
- **Help Text**: HelpText

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

## Шаг 7: Настройка Slider

### OverallProgressSlider:
- **Min Value**: 0
- **Max Value**: 1
- **Value**: 0
- **Fill Rect Color**: #27AE60 (Success)
- **Background Color**: #7F8C8D (Border)

## Шаг 8: Создание префаба

### 8.1 Создание префаба
1. Перетащить GameplayPanel в папку Prefabs/UI/Panels/
2. Удалить из сцены
3. Подключить к UIManager в поле "Gameplay Panel"

### 8.2 Настройка UIManager
- Добавить GameplayPanel в список панелей
- Настроить переходы между экранами

## Шаг 9: Тестирование

### 9.1 Базовая функциональность
1. Запустить сцену
2. Перейти к игровому экрану
3. Проверить отображение информации о контракте
4. Протестировать кнопки паузы, целей и помощи

### 9.2 Анимации
1. Проверить анимацию появления панели
2. Протестировать переключение панелей целей и помощи
3. Проверить обновление прогресса

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

После создания GameplayUI:
1. Создать ObjectiveItemUI префаб для отдельных целей
2. Настроить динамическое создание целей
3. Интегрировать с ContractManager
4. Добавить анимации и эффекты
