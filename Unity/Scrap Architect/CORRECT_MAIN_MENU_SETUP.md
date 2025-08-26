# Исправленное руководство: Создание MainMenuUI префаба

## 🚨 ВАЖНО: Правильные команды Unity Editor

### 1. Создание базовой панели
```
Canvas → Create Empty → "MainMenuPanel"
→ Add Component: MainMenuUI
→ Add Component: CanvasGroup
```

### 2. Создание HeaderContainer
```
MainMenuPanel → Create Empty → "HeaderContainer"
```

### 3. Создание текстовых элементов
```
HeaderContainer → UI → Text - TextMeshPro → "GameTitleText"
HeaderContainer → UI → Text - TextMeshPro → "SubtitleText"
HeaderContainer → UI → Text - TextMeshPro → "VersionText"
```

### 4. Создание ButtonsContainer
```
MainMenuPanel → Create Empty → "ButtonsContainer"
```

### 5. Создание кнопок
```
ButtonsContainer → UI → Button → "PlayButton"
ButtonsContainer → UI → Button → "SettingsButton"
ButtonsContainer → UI → Button → "CreditsButton"
ButtonsContainer → UI → Button → "QuitButton"
```

## 📝 Настройка текстов

### GameTitleText
- **Text**: "SCRAP ARCHITECT"
- **Font Size**: 72
- **Color**: #ECF0F1
- **Alignment**: Center
- **Font Style**: Bold

### SubtitleText
- **Text**: "Создавайте безумные машины из хлама!"
- **Font Size**: 24
- **Color**: #BDC3C7
- **Alignment**: Center

### VersionText
- **Text**: "Версия 1.0.0"
- **Font Size**: 16
- **Color**: #7F8C8D
- **Alignment**: Center

## 🎛️ Настройка кнопок

### Для каждой кнопки:
1. **Переименовать Text** в кнопке:
   - PlayButton/Text → "PlayText"
   - SettingsButton/Text → "SettingsText"
   - CreditsButton/Text → "CreditsText"
   - QuitButton/Text → "QuitText"

2. **Настроить текст кнопок**:
   - PlayText: "ИГРАТЬ"
   - SettingsText: "НАСТРОЙКИ"
   - CreditsText: "ОБ ИГРЕ"
   - QuitText: "ВЫХОД"

3. **Настройка Button компонента**:
   - Font Size: 24
   - Color: #ECF0F1
   - Alignment: Center

## 🔧 Настройка Layout Groups

### MainMenuPanel
- **Add Component**: Vertical Layout Group
- **Spacing**: 30
- **Padding**: 50, 50, 50, 50
- **Child Alignment**: Middle Center

### HeaderContainer
- **Add Component**: Vertical Layout Group
- **Spacing**: 10
- **Padding**: 0, 0, 0, 0
- **Child Alignment**: Middle Center

### ButtonsContainer
- **Add Component**: Vertical Layout Group
- **Spacing**: 15
- **Padding**: 0, 0, 0, 0
- **Child Alignment**: Middle Center

## 📐 Настройка размеров

### MainMenuPanel
- **RectTransform**: Anchor Min (0, 0), Anchor Max (1, 1)
- **Offset**: Min (0, 0), Max (0, 0)

### HeaderContainer
- **Height**: 200
- **Anchor**: Top Center

### ButtonsContainer
- **Width**: 300
- **Height**: 300
- **Anchor**: Middle Center

### Кнопки
- **Width**: 250
- **Height**: 60
- **Anchor**: Middle Center

## 🔗 Подключение к скрипту

В MainMenuUI подключить все поля в Inspector:
- **Game Title Text** → GameTitleText
- **Version Text** → VersionText
- **Subtitle Text** → SubtitleText
- **Play Button** → PlayButton
- **Settings Button** → SettingsButton
- **Credits Button** → CreditsButton
- **Quit Button** → QuitButton

## ✅ Создание префаба

1. Перетащить MainMenuPanel в папку Prefabs/UI/Panels/
2. Удалить из сцены
3. Подключить к UIManager

## 🎨 Цвета

- **Primary**: #2C3E50
- **Secondary**: #3498DB
- **Text**: #ECF0F1
- **TextSecondary**: #BDC3C7
- **Background**: #34495E
