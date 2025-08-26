# Быстрая настройка GameplayUI

## 🚀 Быстрый старт (15 минут)

### 1. Создание панели
```
Canvas → Create Empty → "GameplayPanel"
→ Add Component: GameplayUI
→ Add Component: CanvasGroup
```

### 2. Основные элементы (копируйте структуру)
```
GameplayPanel/
├── ContractInfoPanel/
│   ├── ContractTitleText (TextMeshPro) - "Название контракта"
│   ├── ContractDescriptionText (TextMeshPro) - "Описание контракта..."
│   ├── TimeText (TextMeshPro) - "Время: 00:00"
│   └── DifficultyText (TextMeshPro) - "Сложность: Средняя"
├── ObjectivesPanel/
│   ├── ObjectivesTitleText (TextMeshPro) - "ЦЕЛИ КОНТРАКТА"
│   └── ObjectivesContainer/
├── ProgressPanel/
│   ├── OverallProgressSlider (Slider)
│   └── ProgressText (TextMeshPro) - "Прогресс: 0%"
├── ControlsPanel/
│   ├── PauseButton (Button) - "ПАУЗА"
│   ├── ObjectivesButton (Button) - "ЦЕЛИ"
│   └── HelpButton (Button) - "ПОМОЩЬ"
└── HelpPanel/
    └── HelpText (TextMeshPro) - "Справка по управлению..."
```

### 3. Быстрые настройки

#### Layout Groups
- **GameplayPanel**: Vertical Layout Group
- **ContractInfoPanel**: Horizontal Layout Group
- **ObjectivesContainer**: Vertical Layout Group
- **ControlsPanel**: Horizontal Layout Group

#### Размеры текста
- **ContractTitleText**: Font Size 24, Color #ECF0F1
- **ContractDescriptionText**: Font Size 16, Color #BDC3C7
- **TimeText**: Font Size 18, Color #F39C12
- **DifficultyText**: Font Size 16, Color #E74C3C
- **ObjectivesTitleText**: Font Size 20, Color #ECF0F1
- **ProgressText**: Font Size 18, Color #27AE60
- **Кнопки**: Font Size 16, Color #ECF0F1

#### Размеры элементов
- **GameplayPanel**: Stretch (0,0,1,1)
- **ContractInfoPanel**: Height 100, Top Left
- **ObjectivesPanel**: Width 300, Height 200, Top Right
- **ProgressPanel**: Height 60, Bottom Center
- **ControlsPanel**: Height 50, Bottom Left
- **HelpPanel**: Width 400, Height 300, Center (скрыта)

### 4. Подключение к скрипту
В GameplayUI подключить все поля в Inspector:
- **Contract Title Text** → ContractTitleText
- **Contract Description Text** → ContractDescriptionText
- **Time Text** → TimeText
- **Difficulty Text** → DifficultyText
- **Objectives Container** → ObjectivesContainer
- **Overall Progress Slider** → OverallProgressSlider
- **Progress Text** → ProgressText
- **Pause Button** → PauseButton
- **Objectives Button** → ObjectivesButton
- **Help Button** → HelpButton
- **Objectives Panel** → ObjectivesPanel
- **Help Panel** → HelpPanel
- **Help Text** → HelpText

### 5. Настройка кнопок
Для каждой кнопки:
- **Image Color**: #2C3E50
- **Button Colors**:
  - Normal: #2C3E50
  - Highlighted: #34495E
  - Pressed: #3498DB
  - Fade Duration: 0.1

### 6. Настройка Slider
- **Min Value**: 0
- **Max Value**: 1
- **Value**: 0
- **Fill Rect Color**: #27AE60
- **Background Color**: #7F8C8D

### 7. Создание префаба
1. Перетащить в Prefabs/UI/Panels/
2. Удалить из сцены
3. Подключить к UIManager

## 🎨 Цвета (из UIColors)
- **Primary**: #2C3E50
- **Secondary**: #3498DB
- **Text**: #ECF0F1
- **TextSecondary**: #BDC3C7
- **Success**: #27AE60
- **Warning**: #F39C12
- **Error**: #E74C3C

## ⚡ Готово!
GameplayUI создан и готов к использованию!

## 🔧 Следующие шаги
1. Создать ObjectiveItemUI префаб
2. Настроить динамическое создание целей
3. Интегрировать с ContractManager
4. Протестировать функциональность
