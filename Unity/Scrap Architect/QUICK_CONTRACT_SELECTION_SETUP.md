# Быстрая настройка ContractSelectionUI

## 🚀 Быстрый старт (15 минут)

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
├── ContractDetailsPanel/
│   ├── ContractTitleText (TextMeshPro)
│   ├── ContractDescriptionText (TextMeshPro)
│   ├── ContractDifficultyText (TextMeshPro)
│   ├── ContractRewardText (TextMeshPro)
│   ├── ContractClientText (TextMeshPro)
│   ├── ContractObjectivesText (TextMeshPro)
│   ├── AcceptContractButton (Button)
│   └── CloseDetailsButton (Button)
├── InfoPanel/
│   ├── AvailableContractsText (TextMeshPro)
│   └── ActiveContractsText (TextMeshPro)
└── ControlButtonsContainer/
    ├── BackButton (Button)
    └── RefreshButton (Button)
```

### 3. Быстрые настройки

#### Layout Groups
- **ContractSelectionPanel**: Vertical Layout Group
- **FiltersPanel**: Horizontal Layout Group
- **ContractsContainer**: Vertical Layout Group
- **ContractDetailsPanel**: Vertical Layout Group
- **ControlButtonsContainer**: Horizontal Layout Group

#### Размеры текста
- **TitleText**: Font Size 48, Color #ECF0F1
- **Filters**: Font Size 16, Color #ECF0F1
- **Contract Details**: Font Size 18, Color #ECF0F1
- **Buttons**: Font Size 18, Color #ECF0F1

#### Размеры элементов
- **ContractSelectionPanel**: Stretch (0,0,1,1)
- **HeaderContainer**: Height 80, Top Center
- **FiltersPanel**: Height 60, Top Center
- **ContractsScrollView**: Stretch, занимает центр
- **ContractDetailsPanel**: Width 400, Right Center
- **ControlButtonsContainer**: Height 60, Bottom Center

### 4. Подключение к скрипту
В ContractSelectionUI подключить все поля в Inspector:
- **Contracts Container** → ContractsContainer
- **Back Button** → BackButton
- **Refresh Button** → RefreshButton
- **Contract Details Panel** → ContractDetailsPanel
- **Contract Title Text** → ContractTitleText
- **Contract Description Text** → ContractDescriptionText
- **Contract Difficulty Text** → ContractDifficultyText
- **Contract Reward Text** → ContractRewardText
- **Contract Client Text** → ContractClientText
- **Contract Objectives Text** → ContractObjectivesText
- **Accept Contract Button** → AcceptContractButton
- **Close Details Button** → CloseDetailsButton
- **Difficulty Filter** → DifficultyFilter
- **Type Filter** → TypeFilter
- **Clear Filters Button** → ClearFiltersButton
- **Available Contracts Text** → AvailableContractsText
- **Active Contracts Text** → ActiveContractsText

### 5. Настройка кнопок
Для каждой кнопки:
- **BackButton**: "НАЗАД"
- **RefreshButton**: "ОБНОВИТЬ"
- **AcceptContractButton**: "ПРИНЯТЬ КОНТРАКТ"
- **CloseDetailsButton**: "ЗАКРЫТЬ"
- **ClearFiltersButton**: "Очистить фильтры"

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

## ⚡ Готово!
ContractSelectionUI создан и готов к использованию!

## 🔧 Следующие шаги
1. Создать ContractItemUI префаб
2. Настроить динамическое создание контрактов
3. Интегрировать с ContractManager
4. Протестировать фильтры и навигацию
