# Быстрая настройка VictoryScreen

## 🚀 Быстрый старт (15 минут)

### 1. Создание панели
```
Canvas → Create Empty → "VictoryPanel"
→ Add Component: VictoryScreen
→ Add Component: CanvasGroup
```

### 2. Основные элементы (копируйте структуру)
```
VictoryPanel/
├── VictoryHeader/
│   └── VictoryTitleText (TextMeshPro) - "ПОБЕДА!"
├── ContractInfoPanel/
│   ├── ContractTitleText (TextMeshPro) - "Контракт выполнен!"
│   ├── CompletionTimeText (TextMeshPro) - "Время выполнения: 00:00"
│   └── ScoreText (TextMeshPro) - "Очки: 1000"
├── RewardsPanel/
│   ├── RewardsTitleText (TextMeshPro) - "НАГРАДЫ"
│   ├── ScrapRewardText (TextMeshPro) - "Металлолом: +500"
│   ├── ExperienceRewardText (TextMeshPro) - "Опыт: +100"
│   └── RewardIconsContainer/
├── ObjectivesPanel/
│   ├── ObjectivesTitleText (TextMeshPro) - "ВЫПОЛНЕННЫЕ ЦЕЛИ"
│   └── ObjectivesContainer/
└── ButtonsPanel/
    ├── ContinueButton (Button) - "ПРОДОЛЖИТЬ"
    ├── RetryButton (Button) - "ПОВТОРИТЬ"
    └── MainMenuButton (Button) - "ГЛАВНОЕ МЕНЮ"
```

### 3. Быстрые настройки

#### Layout Groups
- **VictoryPanel**: Vertical Layout Group
- **ContractInfoPanel**: Vertical Layout Group
- **RewardsPanel**: Vertical Layout Group
- **RewardIconsContainer**: Horizontal Layout Group
- **ObjectivesContainer**: Vertical Layout Group
- **ButtonsPanel**: Horizontal Layout Group

#### Размеры текста
- **VictoryTitleText**: Font Size 72, Color #27AE60, Bold
- **ContractTitleText**: Font Size 32, Color #ECF0F1
- **CompletionTimeText**: Font Size 24, Color #F39C12
- **ScoreText**: Font Size 28, Color #3498DB
- **RewardsTitleText**: Font Size 24, Color #ECF0F1, Bold
- **ScrapRewardText**: Font Size 20, Color #F39C12
- **ExperienceRewardText**: Font Size 20, Color #3498DB
- **ObjectivesTitleText**: Font Size 20, Color #ECF0F1, Bold
- **Кнопки**: Font Size 20, Color #ECF0F1

#### Размеры элементов
- **VictoryPanel**: Stretch (0,0,1,1)
- **VictoryHeader**: Height 120, Top Center
- **ContractInfoPanel**: Height 150, Top Center
- **RewardsPanel**: Height 200, Middle Center
- **ObjectivesPanel**: Height 300, Middle Center
- **ButtonsPanel**: Height 80, Bottom Center

### 4. Подключение к скрипту
В VictoryScreen подключить все поля в Inspector:
- **Victory Title Text** → VictoryTitleText
- **Contract Title Text** → ContractTitleText
- **Completion Time Text** → CompletionTimeText
- **Score Text** → ScoreText
- **Scrap Reward Text** → ScrapRewardText
- **Experience Reward Text** → ExperienceRewardText
- **Reward Icons** → массив иконок наград
- **Objectives Container** → ObjectivesContainer
- **Continue Button** → ContinueButton
- **Retry Button** → RetryButton
- **Main Menu Button** → MainMenuButton

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
VictoryScreen создан и готов к использованию!

## 🔧 Следующие шаги
1. Создать DefeatScreen префаб
2. Настроить анимации наград
3. Интегрировать с ContractManager
4. Протестировать функциональность
