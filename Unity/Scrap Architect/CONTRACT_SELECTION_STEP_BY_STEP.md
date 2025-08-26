# Пошаговое создание ContractSelectionUI

## 🎯 Цель
Создать экран выбора контрактов с фильтрами, списком контрактов и детальной информацией.

## 📋 Шаг 1: Создание базовой панели

### 1.1 Создание панели
```
Canvas → Create Empty → "ContractSelectionPanel"
```

### 1.2 Добавление компонентов
```
ContractSelectionPanel → Add Component → ContractSelectionUI
ContractSelectionPanel → Add Component → CanvasGroup
```

## 📋 Шаг 2: Создание заголовка

### 2.1 Создание HeaderContainer
```
ContractSelectionPanel → Create Empty → "HeaderContainer"
```

### 2.2 Создание заголовка
```
HeaderContainer → UI → Text - TextMeshPro → "TitleText"
```

### 2.3 Настройка заголовка
- **Text**: "ВЫБОР КОНТРАКТА"
- **Font Size**: 48
- **Color**: #ECF0F1
- **Alignment**: Center

## 📋 Шаг 3: Создание панели фильтров

### 3.1 Создание FiltersPanel
```
ContractSelectionPanel → Create Empty → "FiltersPanel"
```

### 3.2 Создание фильтра сложности
```
FiltersPanel → UI → Dropdown - TextMeshPro → "DifficultyFilter"
```

### 3.3 Создание фильтра типа
```
FiltersPanel → UI → Dropdown - TextMeshPro → "TypeFilter"
```

### 3.4 Создание кнопки очистки
```
FiltersPanel → UI → Button → "ClearFiltersButton"
ClearFiltersButton/Text → "Очистить фильтры"
```

## 📋 Шаг 4: Создание Scroll View для контрактов

### 4.1 Создание Scroll View
```
ContractSelectionPanel → UI → Scroll View → "ContractsScrollView"
```

### 4.2 Настройка Viewport
```
ContractsScrollView → Viewport → Create Empty → "ContractsContainer"
```

## 📋 Шаг 5: Создание панели деталей контракта

### 5.1 Создание ContractDetailsPanel
```
ContractSelectionPanel → Create Empty → "ContractDetailsPanel"
```

### 5.2 Создание текстовых элементов
```
ContractDetailsPanel → UI → Text - TextMeshPro → "ContractTitleText"
ContractDetailsPanel → UI → Text - TextMeshPro → "ContractDescriptionText"
ContractDetailsPanel → UI → Text - TextMeshPro → "ContractDifficultyText"
ContractDetailsPanel → UI → Text - TextMeshPro → "ContractRewardText"
ContractDetailsPanel → UI → Text - TextMeshPro → "ContractClientText"
ContractDetailsPanel → UI → Text - TextMeshPro → "ContractObjectivesText"
```

### 5.3 Создание кнопок действий
```
ContractDetailsPanel → UI → Button → "AcceptContractButton"
AcceptContractButton/Text → "ПРИНЯТЬ КОНТРАКТ"

ContractDetailsPanel → UI → Button → "CloseDetailsButton"
CloseDetailsButton/Text → "ЗАКРЫТЬ"
```

## 📋 Шаг 6: Создание информационной панели

### 6.1 Создание InfoPanel
```
ContractSelectionPanel → Create Empty → "InfoPanel"
```

### 6.2 Создание информационных текстов
```
InfoPanel → UI → Text - TextMeshPro → "AvailableContractsText"
InfoPanel → UI → Text - TextMeshPro → "ActiveContractsText"
```

## 📋 Шаг 7: Создание кнопок управления

### 7.1 Создание ControlButtonsContainer
```
ContractSelectionPanel → Create Empty → "ControlButtonsContainer"
```

### 7.2 Создание кнопок
```
ControlButtonsContainer → UI → Button → "BackButton"
BackButton/Text → "НАЗАД"

ControlButtonsContainer → UI → Button → "RefreshButton"
RefreshButton/Text → "ОБНОВИТЬ"
```

## 📋 Шаг 8: Настройка Layout Groups

### 8.1 Основной Layout
```
ContractSelectionPanel → Add Component → Vertical Layout Group
- Spacing: 20
- Padding: 30, 30, 30, 30
- Child Alignment: Upper Center
```

### 8.2 Layout для фильтров
```
FiltersPanel → Add Component → Horizontal Layout Group
- Spacing: 15
- Padding: 0, 0, 0, 0
- Child Alignment: Middle Center
```

### 8.3 Layout для контрактов
```
ContractsContainer → Add Component → Vertical Layout Group
- Spacing: 10
- Padding: 10, 10, 10, 10
- Child Alignment: Upper Center
```

### 8.4 Layout для деталей
```
ContractDetailsPanel → Add Component → Vertical Layout Group
- Spacing: 10
- Padding: 20, 20, 20, 20
- Child Alignment: Upper Left
```

### 8.5 Layout для кнопок управления
```
ControlButtonsContainer → Add Component → Horizontal Layout Group
- Spacing: 20
- Padding: 0, 0, 0, 0
- Child Alignment: Middle Center
```

## 📋 Шаг 9: Настройка размеров и позиций

### 9.1 Основная панель
- **RectTransform**: Anchor Min (0, 0), Anchor Max (1, 1)
- **Offset**: Min (0, 0), Max (0, 0)

### 9.2 Заголовок
- **Height**: 80
- **Anchor**: Top Center

### 9.3 Панель фильтров
- **Height**: 60
- **Anchor**: Top Center

### 9.4 Scroll View
- **Anchor**: Stretch (занимает центр)

### 9.5 Панель деталей
- **Width**: 400
- **Height**: 500
- **Anchor**: Right Center

### 9.6 Кнопки управления
- **Height**: 60
- **Anchor**: Bottom Center

## 📋 Шаг 10: Подключение к скрипту

В ContractSelectionUI подключить все поля в Inspector:

### Основные элементы
- **Contracts Container** → ContractsContainer
- **Back Button** → BackButton
- **Refresh Button** → RefreshButton

### Панель деталей
- **Contract Details Panel** → ContractDetailsPanel
- **Contract Title Text** → ContractTitleText
- **Contract Description Text** → ContractDescriptionText
- **Contract Difficulty Text** → ContractDifficultyText
- **Contract Reward Text** → ContractRewardText
- **Contract Client Text** → ContractClientText
- **Contract Objectives Text** → ContractObjectivesText
- **Accept Contract Button** → AcceptContractButton
- **Close Details Button** → CloseDetailsButton

### Фильтры
- **Difficulty Filter** → DifficultyFilter
- **Type Filter** → TypeFilter
- **Clear Filters Button** → ClearFiltersButton

### Информация
- **Available Contracts Text** → AvailableContractsText
- **Active Contracts Text** → ActiveContractsText

## 📋 Шаг 11: Создание префаба

### 11.1 Создание префаба
1. Перетащить ContractSelectionPanel в папку Prefabs/UI/Panels/
2. Удалить из сцены
3. Подключить к UIManager

## ✅ Готово!

ContractSelectionUI создан и готов к использованию!

## 🎨 Цвета
- **Primary**: #2C3E50
- **Secondary**: #3498DB
- **Text**: #ECF0F1
- **TextSecondary**: #BDC3C7
- **Success**: #27AE60
