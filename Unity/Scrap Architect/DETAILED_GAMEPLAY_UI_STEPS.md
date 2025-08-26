# Детальная пошаговая инструкция: Создание GameplayUI префаба

## 🎯 Цель
Создать игровой интерфейс, который отображается во время выполнения контракта. Включает информацию о контракте, цели, прогресс и элементы управления.

## 📋 Шаг 1: Создание базовой панели

### 1.1 Создание панели
```
Canvas → Create Empty → "GameplayPanel"
```

### 1.2 Добавление компонентов
```
GameplayPanel → Add Component → GameplayUI
GameplayPanel → Add Component → CanvasGroup
```

### 1.3 Настройка RectTransform
- **Anchor**: Stretch (0, 0, 1, 1)
- **Offset**: Min (0, 0), Max (0, 0)

## 📋 Шаг 2: Создание панели информации о контракте

### 2.1 Создание ContractInfoPanel
```
GameplayPanel → Create Empty → "ContractInfoPanel"
```

### 2.2 Создание ContractTitleText
```
ContractInfoPanel → UI → Text - TextMeshPro → "ContractTitleText"
```

### 2.3 Настройка ContractTitleText
- **Text**: "Название контракта"
- **Font Size**: 24
- **Color**: #ECF0F1
- **Alignment**: Left
- **Font Style**: Bold

### 2.4 Создание ContractDescriptionText
```
ContractInfoPanel → UI → Text - TextMeshPro → "ContractDescriptionText"
```

### 2.5 Настройка ContractDescriptionText
- **Text**: "Описание контракта..."
- **Font Size**: 16
- **Color**: #BDC3C7
- **Alignment**: Left

### 2.6 Создание TimeText
```
ContractInfoPanel → UI → Text - TextMeshPro → "TimeText"
```

### 2.7 Настройка TimeText
- **Text**: "Время: 00:00"
- **Font Size**: 18
- **Color**: #F39C12
- **Alignment**: Right

### 2.8 Создание DifficultyText
```
ContractInfoPanel → UI → Text - TextMeshPro → "DifficultyText"
```

### 2.9 Настройка DifficultyText
- **Text**: "Сложность: Средняя"
- **Font Size**: 16
- **Color**: #E74C3C
- **Alignment**: Right

## 📋 Шаг 3: Создание панели целей

### 3.1 Создание ObjectivesPanel
```
GameplayPanel → Create Empty → "ObjectivesPanel"
```

### 3.2 Создание ObjectivesTitleText
```
ObjectivesPanel → UI → Text - TextMeshPro → "ObjectivesTitleText"
```

### 3.3 Настройка ObjectivesTitleText
- **Text**: "ЦЕЛИ КОНТРАКТА"
- **Font Size**: 20
- **Color**: #ECF0F1
- **Alignment**: Center
- **Font Style**: Bold

### 3.4 Создание ObjectivesContainer
```
ObjectivesPanel → Create Empty → "ObjectivesContainer"
```

## 📋 Шаг 4: Создание панели прогресса

### 4.1 Создание ProgressPanel
```
GameplayPanel → Create Empty → "ProgressPanel"
```

### 4.2 Создание OverallProgressSlider
```
ProgressPanel → UI → Slider → "OverallProgressSlider"
```

### 4.3 Настройка OverallProgressSlider
- **Min Value**: 0
- **Max Value**: 1
- **Value**: 0
- **Fill Rect Color**: #27AE60
- **Background Color**: #7F8C8D

### 4.4 Создание ProgressText
```
ProgressPanel → UI → Text - TextMeshPro → "ProgressText"
```

### 4.5 Настройка ProgressText
- **Text**: "Прогресс: 0%"
- **Font Size**: 18
- **Color**: #27AE60
- **Alignment**: Center

## 📋 Шаг 5: Создание панели управления

### 5.1 Создание ControlsPanel
```
GameplayPanel → Create Empty → "ControlsPanel"
```

### 5.2 Создание PauseButton
```
ControlsPanel → UI → Button → "PauseButton"
```

### 5.3 Настройка PauseButton
- **Переименовать Text**: PauseButton/Text → "PauseText"
- **Text**: "ПАУЗА"
- **Font Size**: 16
- **Color**: #ECF0F1
- **Alignment**: Center

### 5.4 Создание ObjectivesButton
```
ControlsPanel → UI → Button → "ObjectivesButton"
```

### 5.5 Настройка ObjectivesButton
- **Переименовать Text**: ObjectivesButton/Text → "ObjectivesText"
- **Text**: "ЦЕЛИ"
- **Font Size**: 16
- **Color**: #ECF0F1
- **Alignment**: Center

### 5.6 Создание HelpButton
```
ControlsPanel → UI → Button → "HelpButton"
```

### 5.7 Настройка HelpButton
- **Переименовать Text**: HelpButton/Text → "HelpText"
- **Text**: "ПОМОЩЬ"
- **Font Size**: 16
- **Color**: #ECF0F1
- **Alignment**: Center

## 📋 Шаг 6: Создание панели помощи

### 6.1 Создание HelpPanel
```
GameplayPanel → Create Empty → "HelpPanel"
```

### 6.2 Добавление фона
```
HelpPanel → Add Component → Image
```

### 6.3 Настройка фона
- **Color**: #2C3E50 (с Alpha 0.9)
- **Type**: Sliced

### 6.4 Создание HelpText
```
HelpPanel → UI → Text - TextMeshPro → "HelpText"
```

### 6.5 Настройка HelpText
- **Text**: "Справка по управлению:\n\nWASD - Движение\nМышь - Вращение камеры\nE - Взаимодействие\nQ - Отмена\nESC - Меню"
- **Font Size**: 16
- **Color**: #ECF0F1
- **Alignment**: Left
- **Line Spacing**: 1.2

## 📋 Шаг 7: Настройка Layout Groups

### 7.1 Основной Layout
```
GameplayPanel → Add Component → Vertical Layout Group
```
**Настройки:**
- **Spacing**: 10
- **Padding**: 20, 20, 20, 20
- **Child Alignment**: Upper Left
- **Child Control Width**: ✓
- **Child Control Height**: ✗
- **Child Force Expand Width**: ✗
- **Child Force Expand Height**: ✗

### 7.2 Layout для информации о контракте
```
ContractInfoPanel → Add Component → Horizontal Layout Group
```
**Настройки:**
- **Spacing**: 20
- **Padding**: 10, 10, 10, 10
- **Child Alignment**: Middle Left
- **Child Control Width**: ✓
- **Child Control Height**: ✓
- **Child Force Expand Width**: ✗
- **Child Force Expand Height**: ✗

### 7.3 Layout для контейнера целей
```
ObjectivesContainer → Add Component → Vertical Layout Group
```
**Настройки:**
- **Spacing**: 5
- **Padding**: 5, 5, 5, 5
- **Child Alignment**: Upper Center
- **Child Control Width**: ✓
- **Child Control Height**: ✗
- **Child Force Expand Width**: ✗
- **Child Force Expand Height**: ✗

### 7.4 Layout для панели управления
```
ControlsPanel → Add Component → Horizontal Layout Group
```
**Настройки:**
- **Spacing**: 10
- **Padding**: 10, 10, 10, 10
- **Child Alignment**: Middle Center
- **Child Control Width**: ✓
- **Child Control Height**: ✓
- **Child Force Expand Width**: ✗
- **Child Force Expand Height**: ✗

## 📋 Шаг 8: Настройка размеров и позиций

### 8.1 ContractInfoPanel
- **Height**: 100
- **Anchor**: Top Left
- **Width**: Stretch

### 8.2 ObjectivesPanel
- **Width**: 300
- **Height**: 200
- **Anchor**: Top Right

### 8.3 ProgressPanel
- **Height**: 60
- **Anchor**: Bottom Center
- **Width**: 400

### 8.4 ControlsPanel
- **Height**: 50
- **Anchor**: Bottom Left
- **Width**: 300

### 8.5 HelpPanel
- **Width**: 400
- **Height**: 300
- **Anchor**: Center
- **Изначально скрыта**: SetActive(false)

## 📋 Шаг 9: Настройка кнопок

### 9.1 Настройка PauseButton
1. **Выбрать PauseButton**
2. **Image компонент**:
   - Color: #2C3E50
   - Type: Sliced
3. **Button компонент**:
   - Transition: Color Tint
   - Normal Color: #2C3E50
   - Highlighted Color: #34495E
   - Pressed Color: #3498DB
   - Selected Color: #3498DB
   - Disabled Color: #7F8C8D
   - Fade Duration: 0.1
4. **Navigation**: Automatic

### 9.2 Настройка ObjectivesButton
1. **Выбрать ObjectivesButton**
2. **Image компонент**:
   - Color: #2C3E50
   - Type: Sliced
3. **Button компонент**:
   - Transition: Color Tint
   - Normal Color: #2C3E50
   - Highlighted Color: #34495E
   - Pressed Color: #3498DB
   - Selected Color: #3498DB
   - Disabled Color: #7F8C8D
   - Fade Duration: 0.1
4. **Navigation**: Automatic

### 9.3 Настройка HelpButton
1. **Выбрать HelpButton**
2. **Image компонент**:
   - Color: #2C3E50
   - Type: Sliced
3. **Button компонент**:
   - Transition: Color Tint
   - Normal Color: #2C3E50
   - Highlighted Color: #34495E
   - Pressed Color: #3498DB
   - Selected Color: #3498DB
   - Disabled Color: #7F8C8D
   - Fade Duration: 0.1
4. **Navigation**: Automatic

## 📋 Шаг 10: Настройка Slider

### 10.1 Настройка OverallProgressSlider
1. **Выбрать OverallProgressSlider**
2. **Background Image**:
   - Color: #7F8C8D
   - Type: Sliced
3. **Fill Area → Fill Image**:
   - Color: #27AE60
   - Type: Sliced
4. **Handle Slide Area → Handle**:
   - Color: #ECF0F1
   - Type: Sliced

## 📋 Шаг 11: Подключение к скрипту

### 11.1 Выбрать GameplayPanel
### 11.2 В Inspector найти компонент GameplayUI
### 11.3 Подключить все поля:

**Contract Info:**
- **Contract Title Text** → ContractTitleText
- **Contract Description Text** → ContractDescriptionText
- **Time Text** → TimeText
- **Difficulty Text** → DifficultyText

**Objectives:**
- **Objectives Container** → ObjectivesContainer

**Progress:**
- **Overall Progress Slider** → OverallProgressSlider
- **Progress Text** → ProgressText

**Controls:**
- **Pause Button** → PauseButton
- **Objectives Button** → ObjectivesButton
- **Help Button** → HelpButton

**HUD Elements:**
- **Objectives Panel** → ObjectivesPanel
- **Help Panel** → HelpPanel
- **Help Text** → HelpText

## 📋 Шаг 12: Создание префаба

### 12.1 Создание префаба
1. **В Project Window найти папку**: Assets/Prefabs/UI/Panels/
2. **Перетащить GameplayPanel** из Hierarchy в папку Prefabs/UI/Panels/
3. **Префаб создан!**

### 12.2 Удаление из сцены
1. **В Hierarchy выбрать GameplayPanel**
2. **Нажать Delete** или **правый клик → Delete**

### 12.3 Подключение к UIManager
1. **В Hierarchy выбрать UIManager**
2. **В Inspector найти компонент UIManager**
3. **Найти поле "Gameplay Panel"**
4. **Перетащить GameplayPanel префаб** из Project Window в это поле

## 📋 Шаг 13: Тестирование

### 13.1 Базовая функциональность
1. **Запустить сцену** (Play)
2. **Перейти к игровому экрану** через UIManager
3. **Проверить отображение** всех элементов
4. **Протестировать кнопки**:
   - PauseButton
   - ObjectivesButton
   - HelpButton

### 13.2 Проверка Layout
1. **Изменить разрешение** в Game View
2. **Проверить адаптивность** элементов
3. **Убедиться**, что все элементы остаются видимыми

### 13.3 Проверка Slider
1. **Изменить значение** OverallProgressSlider
2. **Проверить обновление** ProgressText
3. **Убедиться**, что цвета корректные

## ✅ Готово!

GameplayUI префаб создан и готов к использованию!

## 🎨 Цветовая схема
- **Primary**: #2C3E50 (темно-синий)
- **Secondary**: #3498DB (голубой)
- **Text**: #ECF0F1 (белый)
- **TextSecondary**: #BDC3C7 (серый)
- **Success**: #27AE60 (зеленый)
- **Warning**: #F39C12 (оранжевый)
- **Error**: #E74C3C (красный)

## 🔧 Следующие шаги
1. Создать ObjectiveItemUI префаб для отдельных целей
2. Настроить динамическое создание целей
3. Интегрировать с ContractManager
4. Добавить анимации и эффекты
