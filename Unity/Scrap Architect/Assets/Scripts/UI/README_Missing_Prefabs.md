# Создание недостающих UI префабов

## Проблема
В UIManager есть несколько полей, которые помечены как "None", что означает отсутствие соответствующих префабов:

- **World Map Panel** - None (World Map UI)
- **Defeat Panel** - None (Defeat Screen)  
- **Settings Panel** - None (Settings UI)
- **Pause Panel** - None (Pause Menu)
- **Loading Panel** - None (Loading Screen)

## Решение

### Вариант 1: Автоматическое создание (рекомендуется)

1. Откройте Unity Editor
2. Перейдите в меню **Scrap Architect** → **Quick Create Missing UI Prefabs**
3. Префабы будут созданы автоматически в папке `Assets/Prefabs/UI/Panels/`

### Вариант 2: Ручное создание

Для каждого недостающего префаба:

1. **Создайте GameObject в сцене:**
   - Правый клик в Hierarchy → Create Empty
   - Переименуйте в соответствующее имя (например, "WorldMapPanel")

2. **Добавьте компоненты:**
   - RectTransform (автоматически)
   - CanvasRenderer
   - Соответствующий UI скрипт (например, WorldMapUI)

3. **Настройте RectTransform:**
   - Anchor Min: (0, 0)
   - Anchor Max: (1, 1)
   - Anchored Position: (0, 0)
   - Size Delta: (0, 0)

4. **Добавьте фон:**
   - Add Component → UI → Image
   - Установите цвет фона

5. **Создайте префаб:**
   - Перетащите GameObject в папку `Assets/Prefabs/UI/Panels/`
   - Удалите GameObject из сцены

6. **Назначьте в UIManager:**
   - Выберите UIManager в сцене
   - Перетащите созданный префаб в соответствующее поле

## Недостающие префабы

### 1. WorldMapPanel
- **Скрипт:** WorldMapUI
- **Путь:** Assets/Prefabs/UI/Panels/WorldMapPanel.prefab
- **Описание:** Панель карты мира с контрактами

### 2. DefeatPanel  
- **Скрипт:** DefeatScreen
- **Путь:** Assets/Prefabs/UI/Panels/DefeatPanel.prefab
- **Описание:** Экран поражения

### 3. SettingsPanel
- **Скрипт:** SettingsUI  
- **Путь:** Assets/Prefabs/UI/Panels/SettingsPanel.prefab
- **Описание:** Панель настроек игры

### 4. PausePanel
- **Скрипт:** PauseMenu
- **Путь:** Assets/Prefabs/UI/Panels/PausePanel.prefab  
- **Описание:** Меню паузы

### 5. LoadingPanel
- **Скрипт:** LoadingScreen
- **Путь:** Assets/Prefabs/UI/Panels/LoadingPanel.prefab
- **Описание:** Экран загрузки

## Проверка

После создания всех префабов:

1. Выберите UIManager в сцене
2. Проверьте, что все поля заполнены (не "None")
3. Запустите игру и проверьте работу всех панелей

## Устранение неполадок

### Проблема: Ошибка компиляции CS0234
- **Ошибка:** `The type or namespace name 'Type' does not exist in the namespace 'ScrapArchitect.System'`
- **Решение:** Добавьте `using System;` в начало скрипта
- **Статус:** ✅ Исправлено

### Проблема: Префаб не создается
- Убедитесь, что папка `Assets/Prefabs/UI/Panels/` существует
- Проверьте, что скрипт не содержит ошибок компиляции
- Используйте **Scrap Architect** → **Test Create Single Prefab** для тестирования

### Проблема: Префаб не назначается в UIManager
- Убедитесь, что префаб содержит правильный скрипт
- Проверьте, что префаб находится в правильной папке

### Проблема: Панель не отображается
- Проверьте, что RectTransform настроен правильно
- Убедитесь, что панель активна в иерархии
- Проверьте, что Canvas настроен правильно

## Дополнительные скрипты

В проекте есть дополнительные скрипты для помощи:

- **UIPrefabChecker.cs** - проверка недостающих префабов
- **QuickPrefabCreator.cs** - быстрое создание префабов
- **CreateMissingPrefabs.cs** - альтернативный способ создания
- **TestPrefabCreation.cs** - тестирование создания префабов

### Тестовые функции:
- **Scrap Architect** → **Test Create Single Prefab** - создание тестового префаба
- **Scrap Architect** → **Test Check All Scripts** - проверка всех UI скриптов

## Контакты

Если возникли проблемы, создайте Issue в GitHub с описанием проблемы.
