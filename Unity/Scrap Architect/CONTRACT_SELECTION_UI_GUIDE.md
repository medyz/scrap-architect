# Руководство: Создание ContractSelectionUI префаба

## Обзор
ContractSelectionUI - это экран выбора контрактов, где игрок может просматривать доступные контракты, фильтровать их по сложности и типу, а также принимать контракты.

## Шаг 1: Создание базовой структуры

### В Canvas создайте:
1. **Create Empty** → "ContractSelectionPanel"
2. **Добавить компонент ContractSelectionUI** (скрипт)
3. **Добавить CanvasGroup** для анимаций

## Шаг 2: Создание UI элементов

### 2.1 Заголовок
- **GameObject** → "HeaderContainer"
  - **TextMeshPro - Text (UI)** → "TitleText"
    - Text: "ВЫБОР КОНТРАКТА"
    - Font Size: 48
    - Color: #ECF0F1
    - Alignment: Center

### 2.2 Панель фильтров
- **GameObject** → "FiltersPanel"
  - **Horizontal Layout Group** для размещения фильтров

#### Фильтр сложности
- **TMP_Dropdown** → "DifficultyFilter"
  - Options: Все, Легкие, Средние, Сложные
  - Font Size: 16
  - Color: #ECF0F1

#### Фильтр типа
- **TMP_Dropdown** → "TypeFilter"
  - Options: Все, Доставка, Сбор, Гонки
  - Font Size: 16
  - Color: #ECF0F1

#### Кнопка очистки фильтров
- **Button** → "ClearFiltersButton"
  - **TextMeshPro - Text (UI)** → "ClearFiltersText"
    - Text: "Очистить фильтры"
    - Font Size: 14
    - Color: #ECF0F1

### 2.3 Контейнер контрактов
- **Scroll View** → "ContractsScrollView"
  - **Viewport** → "Viewport"
    - **Content** → "ContractsContainer"
      - **Vertical Layout Group** для автоматического размещения контрактов

### 2.4 Информационная панель
- **Panel** → "InfoPanel"
  - **Vertical Layout Group** для размещения информации

#### Заголовок контракта
- **TextMeshPro - Text (UI)** → "ContractTitleText"
  - Text: "Название контракта"
  - Font Size: 24
  - Color: #ECF0F1
  - Alignment: Left

#### Описание контракта
- **TextMeshPro - Text (UI)** → "ContractDescriptionText"
  - Text: "Описание контракта..."
  - Font Size: 16
  - Color: #BDC3C7
  - Alignment: Left

#### Награда контракта
- **TextMeshPro - Text (UI)** → "ContractRewardText"
  - Text: "Награда: 1000 монет"
  - Font Size: 18
  - Color: #27AE60
  - Alignment: Left

#### Кнопка принятия контракта
- **Button** → "AcceptContractButton"
  - **TextMeshPro - Text (UI)** → "AcceptContractText"
    - Text: "ПРИНЯТЬ КОНТРАКТ"
    - Font Size: 18
    - Color: #ECF0F1

### 2.5 Кнопки управления
- **GameObject** → "ControlButtonsContainer"
  - **Horizontal Layout Group** для размещения кнопок

#### Кнопка "Назад"
- **Button** → "BackButton"
  - **TextMeshPro - Text (UI)** → "BackText"
    - Text: "НАЗАД"
    - Font Size: 18
    - Color: #ECF0F1

#### Кнопка "Обновить"
- **Button** → "RefreshButton"
  - **TextMeshPro - Text (UI)** → "RefreshText"
    - Text: "ОБНОВИТЬ"
    - Font Size: 18
    - Color: #ECF0F1

## Шаг 3: Настройка Layout Groups

### 3.1 Основной Layout
- **Vertical Layout Group** на ContractSelectionPanel
  - Spacing: 20
  - Padding: 30, 30, 30, 30
  - Child Alignment: Upper Center
  - Child Control Width: true
  - Child Control Height: false
  - Child Force Expand Width: false
  - Child Force Expand Height: false

### 3.2 Панель фильтров
- **Horizontal Layout Group** на FiltersPanel
  - Spacing: 15
  - Padding: 0, 0, 0, 0
  - Child Alignment: Middle Center
  - Child Control Width: true
  - Child Control Height: true
  - Child Force Expand Width: false
  - Child Force Expand Height: false

### 3.3 Контейнер контрактов
- **Vertical Layout Group** на ContractsContainer
  - Spacing: 10
  - Padding: 10, 10, 10, 10
  - Child Alignment: Upper Center
  - Child Control Width: true
  - Child Control Height: false
  - Child Force Expand Width: false
  - Child Force Expand Height: false

## Шаг 4: Настройка размеров и позиций

### 4.1 Основная панель
- **RectTransform**: Anchor Min (0, 0), Anchor Max (1, 1)
- **Offset**: Min (0, 0), Max (0, 0)

### 4.2 Заголовок
- **Height**: 80
- **Anchor**: Top Center

### 4.3 Панель фильтров
- **Height**: 60
- **Anchor**: Top Center

### 4.4 Контейнер контрактов
- **Anchor**: Stretch (занимает оставшееся пространство)
- **Scroll View**: Настроить для прокрутки

### 4.5 Информационная панель
- **Width**: 400
- **Height**: 300
- **Anchor**: Right Center

### 4.6 Кнопки управления
- **Height**: 60
- **Anchor**: Bottom Center

## Шаг 5: Настройка ContractSelectionUI скрипта

### Подключить все UI элементы к скрипту:
- **Title Text**: TitleText
- **Difficulty Filter**: DifficultyFilter
- **Type Filter**: TypeFilter
- **Clear Filters Button**: ClearFiltersButton
- **Contracts Container**: ContractsContainer
- **Contract Title Text**: ContractTitleText
- **Contract Description Text**: ContractDescriptionText
- **Contract Reward Text**: ContractRewardText
- **Accept Contract Button**: AcceptContractButton
- **Back Button**: BackButton
- **Refresh Button**: RefreshButton

## Шаг 6: Создание префаба

### 6.1 Создание префаба
1. Перетащить ContractSelectionPanel в папку Prefabs/UI/Panels/
2. Удалить из сцены
3. Подключить к UIManager в поле "Contract Selection Panel"

### 6.2 Настройка UIManager
- Добавить ContractSelectionPanel в список панелей
- Настроить переходы между MainMenu и ContractSelection

## Шаг 7: Тестирование

### 7.1 Базовая функциональность
1. Запустить сцену
2. Перейти к экрану выбора контрактов
3. Проверить работу фильтров
4. Протестировать кнопки "Назад" и "Обновить"

### 7.2 Анимации
1. Проверить анимацию появления панели
2. Протестировать переходы между экранами
3. Проверить работу Scroll View

## Полезные советы

1. **Используйте Content Size Fitter** для автоматического размера текста
2. **Настройте Scroll View** для плавной прокрутки
3. **Примените цвета из UIColors** для единообразия
4. **Используйте Layout Groups** для адаптивности
5. **Тестируйте на разных разрешениях**

## Следующие шаги

После создания ContractSelectionUI:
1. Создать ContractItemUI префаб для отдельных контрактов
2. Настроить динамическое создание контрактов
3. Интегрировать с ContractManager
4. Добавить анимации и эффекты
