# 🔧 Исправление ошибки CanvasGroup

## ❌ Проблема
```
UnassignedReferenceException: The variable canvasGroup of MainMenuUI has not been assigned.
You probably need to assign the canvasGroup variable of the MainMenuUI script in the inspector.
```

## ✅ Решение

### Шаг 1: Открыть префаб
1. **В Project Window найти**: Assets/Prefabs/UI/Panels/MainMenuPanel.prefab
2. **Дважды кликнуть** для открытия префаба

### Шаг 2: Подключить CanvasGroup
1. **Выбрать MainMenuPanel** в Hierarchy
2. **В Inspector найти компонент MainMenuUI**
3. **Найти поле "Canvas Group"**
4. **Перетащить CanvasGroup компонент** в это поле
   - Или **нажать кружок справа** от поля и выбрать CanvasGroup

### Шаг 3: Повторить для всех префабов
Проверить и исправить все UI префабы:

#### MainMenuPanel.prefab
- **Скрипт**: MainMenuUI
- **Поле**: Canvas Group
- **Подключить**: CanvasGroup компонент

#### ContractSelectionPanel.prefab
- **Скрипт**: ContractSelectionUI
- **Поле**: Canvas Group
- **Подключить**: CanvasGroup компонент

#### GameplayPanel.prefab
- **Скрипт**: GameplayUI
- **Поле**: Canvas Group
- **Подключить**: CanvasGroup компонент

#### VictoryPanel.prefab
- **Скрипт**: VictoryScreen
- **Поле**: Canvas Group
- **Подключить**: CanvasGroup компонент

#### DefeatPanel.prefab
- **Скрипт**: DefeatScreen
- **Поле**: Canvas Group
- **Подключить**: CanvasGroup компонент

## 🎯 Быстрое исправление

### Для каждого префаба:
1. **Открыть префаб** (двойной клик)
2. **Выбрать главную панель**
3. **В Inspector найти UI скрипт**
4. **Найти поле "Canvas Group"**
5. **Перетащить CanvasGroup компонент**
6. **Сохранить префаб** (Ctrl+S)

## ⚠️ Важно
- **CanvasGroup должен быть на том же GameObject**, что и UI скрипт
- **Поле "Canvas Group" должно быть подключено** в Inspector
- **Проверить все префабы** перед тестированием

## 🔍 Проверка
После исправления:
1. **Запустить сцену**
2. **Проверить Console** на наличие ошибок
3. **Протестировать переходы** между экранами

## 📋 Обновленные инструкции
Все файлы MD обновлены с добавлением важного шага подключения CanvasGroup:
- UI_SETUP_STEPS.md
- MAIN_MENU_UI_GUIDE.md
- QUICK_MAIN_MENU_SETUP.md
- CONTRACT_SELECTION_UI_GUIDE.md
- QUICK_CONTRACT_SELECTION_SETUP.md
- GAMEPLAY_UI_GUIDE.md
- QUICK_GAMEPLAY_UI_SETUP.md
- VICTORY_SCREEN_GUIDE.md
- QUICK_VICTORY_SCREEN_SETUP.md
- DEFEAT_SCREEN_GUIDE.md
- QUICK_DEFEAT_SCREEN_SETUP.md
- DETAILED_GAMEPLAY_UI_STEPS.md
- DETAILED_VICTORY_SCREEN_STEPS.md
