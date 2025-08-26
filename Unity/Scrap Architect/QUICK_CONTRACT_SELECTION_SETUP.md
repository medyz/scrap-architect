# Быстрая настройка ContractSelectionUI

## 🚀 Быстрый старт (5 минут)

### 1. Создание панели
```
Canvas → Create Empty → "ContractSelectionPanel"
→ Add Component: ContractSelectionUI
→ Add Component: CanvasGroup
```

### 2. Основные элементы (копируйте структуру)
```
ContractSelectionPanel/
├── HeaderContainer/
│   └── TitleText (TextMeshPro) - "ВЫБОР КОНТРАКТА"
├── FiltersPanel/
│   ├── DifficultyFilter (TMP_Dropdown)
│   ├── TypeFilter (TMP_Dropdown)
│   └── ClearFiltersButton (Button)
├── ContractsScrollView/
│   ├── Viewport/
│   │   └── ContractsContainer/
├── InfoPanel/
│   ├── ContractTitleText (TextMeshPro)
│   ├── ContractDescriptionText (TextMeshPro)
│   ├── ContractRewardText (TextMeshPro)
│   └── AcceptContractButton (Button)
└── ControlButtonsContainer/
    ├── BackButton (Button)
    └── RefreshButton (Button)
```

### 3. Быстрые настройки

#### Layout Groups
- **ContractSelectionPanel**: Vertical Layout Group
- **FiltersPanel**: Horizontal Layout Group
- **ContractsContainer**: Vertical Layout Group
- **ControlButtonsContainer**: Horizontal Layout Group

#### Размеры
- **TitleText**: Font Size 48, Color #ECF0F1
- **Filters**: Font Size 16, Color #ECF0F1
- **Buttons**: Font Size 18, Color #ECF0F1

#### Anchors
- **ContractSelectionPanel**: Stretch (0,0,1,1)
- **HeaderContainer**: Top Center, Height 80
- **FiltersPanel**: Top Center, Height 60
- **ContractsScrollView**: Stretch, занимает центр
- **InfoPanel**: Right Center, Width 400
- **ControlButtonsContainer**: Bottom Center, Height 60

### 4. Подключение к скрипту
В ContractSelectionUI подключить все поля в Inspector:
- Title Text → TitleText
- Difficulty Filter → DifficultyFilter
- Type Filter → TypeFilter
- Clear Filters Button → ClearFiltersButton
- Contracts Container → ContractsContainer
- Contract Title Text → ContractTitleText
- Contract Description Text → ContractDescriptionText
- Contract Reward Text → ContractRewardText
- Accept Contract Button → AcceptContractButton
- Back Button → BackButton
- Refresh Button → RefreshButton

### 5. Создание префаба
1. Перетащить в Prefabs/UI/Panels/
2. Удалить из сцены
3. Подключить к UIManager

## 🎨 Цвета (из UIColors)
- **Primary**: #2C3E50
- **Secondary**: #3498DB
- **Text**: #ECF0F1
- **TextSecondary**: #BDC3C7
- **Success**: #27AE60

## ⚡ Готово!
ContractSelectionUI создан и готов к использованию!

## 🔧 Следующие шаги
1. Создать ContractItemUI префаб
2. Настроить динамическое создание контрактов
3. Интегрировать с ContractManager
