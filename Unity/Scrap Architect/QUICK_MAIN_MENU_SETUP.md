# Быстрая настройка MainMenuUI

## 🚀 Быстрый старт (10 минут)

### 1. Создание панели
```
Canvas → Create Empty → "MainMenuPanel"
→ Add Component: MainMenuUI
→ Add Component: CanvasGroup
→ Add Component: Image (для фона)
→ В Inspector подключить CanvasGroup к скрипту
```

### 2. Основные элементы (копируйте структуру)
```
MainMenuPanel/
├── HeaderContainer/
│   ├── GameTitleText (TextMeshPro) - "SCRAP ARCHITECT"
│   ├── SubtitleText (TextMeshPro) - "Создавайте безумные машины из хлама!"
│   └── VersionText (TextMeshPro) - "Версия 1.0.0"
├── LogoContainer/ (опционально)
│   └── LogoImage (Image)
└── ButtonsContainer/
    ├── PlayButton (Button) - "ИГРАТЬ"
    ├── SettingsButton (Button) - "НАСТРОЙКИ"
    ├── CreditsButton (Button) - "ОБ ИГРЕ"
    └── QuitButton (Button) - "ВЫХОД"
```

### 3. Быстрые настройки

#### Layout Groups
- **MainMenuPanel**: Vertical Layout Group
- **HeaderContainer**: Vertical Layout Group
- **ButtonsContainer**: Vertical Layout Group

#### Размеры текста
- **GameTitleText**: Font Size 72, Color #ECF0F1, Bold
- **SubtitleText**: Font Size 24, Color #BDC3C7
- **VersionText**: Font Size 16, Color #7F8C8D
- **Кнопки**: Font Size 24, Color #ECF0F1

#### Размеры элементов
- **MainMenuPanel**: Stretch (0,0,1,1)
- **HeaderContainer**: Height 200, Top Center
- **ButtonsContainer**: Width 300, Height 300, Middle Center
- **Кнопки**: Width 250, Height 60

### 4. Подключение к скрипту
В MainMenuUI подключить все поля в Inspector:
- **Canvas Group** → CanvasGroup (ВАЖНО!)
- **Game Title Text** → GameTitleText
- **Version Text** → VersionText
- **Subtitle Text** → SubtitleText
- **Play Button** → PlayButton
- **Settings Button** → SettingsButton
- **Credits Button** → CreditsButton
- **Quit Button** → QuitButton
- **Logo Image** → LogoImage (если есть)

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
- **Background**: #34495E

## ⚡ Готово!
MainMenuUI создан и готов к использованию!

## 🔧 Следующие шаги
1. Создать ContractSelectionUI префаб
2. Настроить переходы между экранами
3. Добавить звуковые эффекты
4. Протестировать анимации
