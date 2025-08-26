# Быстрая настройка DefeatScreen

## 🚀 Быстрый старт (15 минут)

### 1. Создание панели
```
Canvas → Create Empty → "DefeatPanel"
→ Add Component: DefeatScreen
→ Add Component: CanvasGroup
```

### 2. Основные элементы (копируйте структуру)
```
DefeatPanel/
├── DefeatHeader/
│   └── DefeatTitleText (TextMeshPro) - "ПОРАЖЕНИЕ"
├── ContractInfoPanel/
│   ├── ContractTitleText (TextMeshPro) - "Контракт провален!"
│   ├── FailureReasonText (TextMeshPro) - "Причина: Время истекло"
│   └── TimeElapsedText (TextMeshPro) - "Время выполнения: 00:00"
├── ObjectivesPanel/
│   ├── ObjectivesTitleText (TextMeshPro) - "НЕВЫПОЛНЕННЫЕ ЦЕЛИ"
│   └── ObjectivesContainer/
└── ButtonsPanel/
    ├── RetryButton (Button) - "ПОВТОРИТЬ"
    ├── MainMenuButton (Button) - "ГЛАВНОЕ МЕНЮ"
    └── ContractSelectionButton (Button) - "ВЫБРАТЬ КОНТРАКТ"
```

### 3. Быстрые настройки

#### Layout Groups
- **DefeatPanel**: Vertical Layout Group
- **DefeatHeader**: Vertical Layout Group
- **ContractInfoPanel**: Vertical Layout Group
- **ObjectivesContainer**: Vertical Layout Group
- **ButtonsPanel**: Horizontal Layout Group

#### Размеры текста
- **DefeatTitleText**: Font Size 72, Color #E74C3C, Bold
- **ContractTitleText**: Font Size 32, Color #ECF0F1
- **FailureReasonText**: Font Size 24, Color #F39C12
- **TimeElapsedText**: Font Size 20, Color #BDC3C7
- **ObjectivesTitleText**: Font Size 20, Color #ECF0F1, Bold
- **Кнопки**: Font Size 20, Color #ECF0F1

#### Размеры элементов
- **DefeatPanel**: Stretch (0,0,1,1)
- **DefeatHeader**: Height 120, Top Center
- **ContractInfoPanel**: Height 150, Top Center
- **ObjectivesPanel**: Height 300, Middle Center
- **ButtonsPanel**: Height 80, Bottom Center

### 4. Подключение к скрипту
В DefeatScreen подключить все поля в Inspector:
- **Defeat Title Text** → DefeatTitleText
- **Contract Title Text** → ContractTitleText
- **Failure Reason Text** → FailureReasonText
- **Time Elapsed Text** → TimeElapsedText
- **Objectives Container** → ObjectivesContainer
- **Retry Button** → RetryButton
- **Main Menu Button** → MainMenuButton
- **Contract Selection Button** → ContractSelectionButton

### 5. Настройка кнопок
Для каждой кнопки:
- **Image Color**: #2C3E50
- **Button Colors**:
  - Normal: #2C3E50
  - Highlighted: #34495E
  - Pressed: #3498DB
  - Fade Duration: 0.1

### 6. Создание префаба
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
DefeatScreen создан и готов к использованию!

## 🔧 Следующие шаги
1. Создать SettingsUI префаб
2. Настроить анимации поражения
3. Интегрировать с ContractManager
4. Протестировать функциональность
