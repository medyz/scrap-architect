# Руководство по интеграции UI системы в Unity

## Шаг 1: Настройка базовой структуры

### 1.1 Создание Canvas
1. В иерархии сцены: **Right Click → UI → Canvas**
2. Настройки Canvas:
   - **Render Mode**: Screen Space - Overlay
   - **UI Scale Mode**: Scale With Screen Size
   - **Reference Resolution**: X=1920, Y=1080
   - **Screen Match Mode**: Match Width Or Height
   - **Match**: 0.5 (баланс между шириной и высотой)

### 1.2 Добавление EventSystem
1. Если EventSystem не создался автоматически: **Right Click → UI → Event System**
2. Настройки EventSystem:
   - **Standalone Input Module** должен быть активен
   - **Horizontal Axis**: "Horizontal"
   - **Vertical Axis**: "Vertical"
   - **Submit Button**: "Submit"
   - **Cancel Button**: "Cancel"

### 1.3 Создание папок для организации
В Project Window создайте следующую структуру:
```
Assets/
├── Prefabs/
│   ├── UI/
│   │   ├── Panels/
│   │   ├── Components/
│   │   └── Buttons/
├── Materials/
│   └── UI/
├── Textures/
│   └── UI/
└── Audio/
    └── UI/
```

## Шаг 2: Создание UIManager

### 2.1 Создание GameObject
1. В иерархии Canvas: **Create Empty** → назвать "UIManager"
2. Добавить компонент **UIManager** скрипт
3. Настройки:
   - **Panel Animation Duration**: 0.3
   - **Panel Animation Curve**: EaseInOut

### 2.2 Настройка аудио
Добавить AudioClip'ы в UIManager:
- **Button Click Sound**: UI звук клика
- **Panel Open Sound**: Звук открытия панели
- **Panel Close Sound**: Звук закрытия панели

## Шаг 3: Создание MainMenuUI префаба

### 3.1 Базовая структура
1. В Canvas: **Create Empty** → назвать "MainMenuPanel"
2. Добавить компонент **MainMenuUI**
3. Добавить **CanvasGroup** для анимаций

### 3.2 UI элементы
Создать следующие дочерние объекты:

#### Заголовок
- **GameObject** → "TitleContainer"
  - **TextMeshPro - Text (UI)** → "GameTitleText"
    - Text: "SCRAP ARCHITECT"
    - Font Size: 72
    - Color: White
    - Alignment: Center

#### Подзаголовок
- **TextMeshPro - Text (UI)** → "SubtitleText"
  - Text: "Создавайте безумные машины из хлама!"
  - Font Size: 24
  - Color: Light Gray

#### Версия
- **TextMeshPro - Text (UI)** → "VersionText"
  - Text: "Версия 1.0.0"
  - Font Size: 16
  - Color: Gray

#### Кнопки
- **Button** → "PlayButton"
  - **TextMeshPro - Text (UI)** → "PlayText"
    - Text: "ИГРАТЬ"
- **Button** → "SettingsButton"
  - **TextMeshPro - Text (UI)** → "SettingsText"
    - Text: "НАСТРОЙКИ"
- **Button** → "CreditsButton"
  - **TextMeshPro - Text (UI)** → "CreditsText"
    - Text: "ОБ ИГРЕ"
- **Button** → "QuitButton"
  - **TextMeshPro - Text (UI)** → "QuitText"
    - Text: "ВЫХОД"

### 3.3 Настройка MainMenuUI
Подключить все UI элементы к скрипту:
- **Game Title Text**: GameTitleText
- **Version Text**: VersionText
- **Subtitle Text**: SubtitleText
- **Play Button**: PlayButton
- **Settings Button**: SettingsButton
- **Credits Button**: CreditsButton
- **Quit Button**: QuitButton

### 3.4 Создание префаба
1. Перетащить MainMenuPanel в папку Prefabs/UI/Panels/
2. Удалить из сцены
3. Подключить к UIManager в поле "Main Menu Panel"

## Шаг 4: Создание ContractSelectionUI префаба

### 4.1 Базовая структура
1. **Create Empty** → "ContractSelectionPanel"
2. Добавить **ContractSelectionUI** компонент
3. Добавить **CanvasGroup**

### 4.2 UI элементы

#### Заголовок
- **TextMeshPro - Text (UI)** → "TitleText"
  - Text: "ВЫБОР КОНТРАКТА"

#### Фильтры
- **TMP_Dropdown** → "DifficultyFilter"
  - Options: Все, Легкие, Средние, Сложные
- **TMP_Dropdown** → "TypeFilter"
  - Options: Все, Доставка, Сбор, Гонки
- **Button** → "ClearFiltersButton"
  - Text: "Очистить фильтры"

#### Контейнер контрактов
- **Scroll View** → "ContractsScrollView"
  - **Viewport** → "Viewport"
    - **Content** → "ContractsContainer"

#### Информационная панель
- **Panel** → "InfoPanel"
  - **TextMeshPro - Text (UI)** → "ContractTitleText"
  - **TextMeshPro - Text (UI)** → "ContractDescriptionText"
  - **TextMeshPro - Text (UI)** → "ContractRewardText"
  - **Button** → "AcceptContractButton"
    - Text: "ПРИНЯТЬ КОНТРАКТ"

#### Кнопки управления
- **Button** → "BackButton"
  - Text: "НАЗАД"
- **Button** → "RefreshButton"
  - Text: "ОБНОВИТЬ"

### 4.3 Настройка ContractSelectionUI
Подключить все элементы к скрипту и создать префаб.

## Шаг 5: Создание компонентных префабов

### 5.1 ContractItemUI префаб
1. **Create Empty** → "ContractItem"
2. Добавить **ContractItemUI** компонент
3. Создать UI элементы:
   - **TextMeshPro - Text (UI)** → "TitleText"
   - **TextMeshPro - Text (UI)** → "DescriptionText"
   - **TextMeshPro - Text (UI)** → "RewardText"
   - **TextMeshPro - Text (UI)** → "DifficultyText"
   - **Button** → "SelectButton"
4. Создать префаб в Prefabs/UI/Components/

### 5.2 ObjectiveItemUI префаб
1. **Create Empty** → "ObjectiveItem"
2. Добавить **ObjectiveItemUI** компонент
3. Создать UI элементы:
   - **TextMeshPro - Text (UI)** → "DescriptionText"
   - **TextMeshPro - Text (UI)** → "ProgressText"
   - **Slider** → "ProgressSlider"
   - **Image** → "CheckmarkIcon"
   - **Image** → "CrossIcon"
4. Создать префаб в Prefabs/UI/Components/

## Шаг 6: Настройка визуальных элементов

### 6.1 Цветовая схема
Создать цветовую палитру:
- **Primary**: #2C3E50 (темно-синий)
- **Secondary**: #3498DB (голубой)
- **Accent**: #E74C3C (красный)
- **Success**: #27AE60 (зеленый)
- **Warning**: #F39C12 (оранжевый)
- **Background**: #34495E (серо-синий)
- **Text**: #ECF0F1 (светло-серый)

### 6.2 Материалы UI
Создать материалы в Materials/UI/:
- **UI_Button_Normal**
- **UI_Button_Hover**
- **UI_Button_Selected**
- **UI_Panel_Background**
- **UI_Text_Default**

### 6.3 Шрифты
Настроить TextMeshPro шрифты:
- **Основной**: Roboto или Arial
- **Заголовки**: Bold версия основного шрифта
- **Размеры**: 16px (обычный), 24px (средний), 48px (большой), 72px (заголовок)

## Шаг 7: Тестирование

### 7.1 Базовая навигация
1. Запустить сцену
2. Проверить появление главного меню
3. Протестировать переходы между экранами
4. Проверить работу кнопок

### 7.2 Анимации
1. Проверить анимации появления/исчезновения
2. Протестировать анимации кнопок
3. Проверить плавность переходов

### 7.3 Аудио
1. Проверить звуки кликов
2. Протестировать звуки открытия/закрытия панелей
3. Настроить громкость

## Шаг 8: Оптимизация

### 8.1 Производительность
- Использовать Object Pooling для динамических элементов
- Оптимизировать анимации
- Минимизировать количество Draw Calls

### 8.2 Адаптивность
- Протестировать на разных разрешениях
- Настроить Anchors для правильного масштабирования
- Проверить работу на мобильных устройствах

## Полезные советы

1. **Используйте Layout Groups** для автоматического позиционирования
2. **Применяйте Content Size Fitter** для динамического размера
3. **Настройте Anchors** для адаптивности
4. **Используйте Prefabs** для переиспользования элементов
5. **Тестируйте на разных устройствах** для совместимости

## Следующие шаги

После завершения интеграции:
1. Создать остальные UI панели
2. Настроить визуальные эффекты
3. Добавить анимации переходов
4. Интегрировать с игровой логикой
5. Провести полное тестирование
