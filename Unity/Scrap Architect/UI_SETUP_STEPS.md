# Пошаговая настройка UI в Unity

## Этап 1: Создание структуры папок

### В Project Window создайте следующие папки:
1. **Prefabs** (если нет)
   - **UI**
     - **Panels** (для UI панелей)
     - **Components** (для компонентных элементов)
     - **Buttons** (для кнопок)

2. **Materials** (если нет)
   - **UI** (для материалов интерфейса)

3. **Textures** (если нет)
   - **UI** (для текстур интерфейса)

4. **Audio** (если нет)
   - **UI** (для звуков интерфейса)

## Этап 2: Создание Canvas

### В иерархии сцены:
1. **Right Click → UI → Canvas**
2. **Настройки Canvas:**
   - **Render Mode**: Screen Space - Overlay
   - **UI Scale Mode**: Scale With Screen Size
   - **Reference Resolution**: X=1920, Y=1080
   - **Screen Match Mode**: Match Width Or Height
   - **Match**: 0.5

### Добавление EventSystem:
1. Если EventSystem не создался автоматически: **Right Click → UI → Event System**
2. **Настройки EventSystem:**
   - **Standalone Input Module** должен быть активен
   - **Horizontal Axis**: "Horizontal"
   - **Vertical Axis**: "Vertical"
   - **Submit Button**: "Submit"
   - **Cancel Button**: "Cancel"

## Этап 3: Создание UIManager

### В иерархии Canvas:
1. **Create Empty** → назвать "UIManager"
2. **Добавить компонент UIManager** (скрипт)
3. **Настройки UIManager:**
   - **Panel Animation Duration**: 0.3
   - **Panel Animation Curve**: EaseInOut
   - **Button Click Sound**: (пока оставить пустым)
   - **Panel Open Sound**: (пока оставить пустым)
   - **Panel Close Sound**: (пока оставить пустым)

## Этап 4: Создание базовых материалов

### В папке Materials/UI создайте материалы:

#### UI_Button_Normal
- **Shader**: UI/Default
- **Color**: #2C3E50 (Primary)

#### UI_Button_Hover
- **Shader**: UI/Default
- **Color**: #34495E (Background)

#### UI_Button_Selected
- **Shader**: UI/Default
- **Color**: #3498DB (Secondary)

#### UI_Panel_Background
- **Shader**: UI/Default
- **Color**: #34495E (Background)

#### UI_Text_Default
- **Shader**: UI/Default
- **Color**: #ECF0F1 (Text)

## Этап 5: Настройка TextMeshPro

### Импорт шрифтов:
1. **Window → TextMeshPro → Import TMP Essential Resources**
2. **Window → TextMeshPro → Import TMP Examples and Extras** (опционально)

### Создание шрифтовых ресурсов:
1. **Window → TextMeshPro → Font Asset Creator**
2. Выберите базовый шрифт (Arial или Roboto)
3. Создайте Font Asset для основного текста

## Этап 6: Создание MainMenuUI префаба

### В Canvas создайте:
1. **Create Empty** → "MainMenuPanel"
2. **Добавить компонент MainMenuUI**
3. **Добавить CanvasGroup** для анимаций

### Создать дочерние элементы:

#### Заголовок
- **GameObject** → "TitleContainer"
  - **TextMeshPro - Text (UI)** → "GameTitleText"
    - Text: "SCRAP ARCHITECT"
    - Font Size: 72
    - Color: #ECF0F1
    - Alignment: Center

#### Подзаголовок
- **TextMeshPro - Text (UI)** → "SubtitleText"
  - Text: "Создавайте безумные машины из хлама!"
  - Font Size: 24
  - Color: #BDC3C7

#### Версия
- **TextMeshPro - Text (UI)** → "VersionText"
  - Text: "Версия 1.0.0"
  - Font Size: 16
  - Color: #7F8C8D

#### Кнопки
- **Button** → "PlayButton"
  - **TextMeshPro - Text (UI)** → "PlayText"
    - Text: "ИГРАТЬ"
- **Button** → "SettingsButton"
  - **TextMeshPro - Text (UI)** → "SettingsText"
    - Text: "НАСТРОЙКИ"
- **Button** → "CreditsButton"
  - **TextMeshPro - Text (UI)** → "CreditsText"
    - Text: "ОБ ИГРЕ"
- **Button** → "QuitButton"
  - **TextMeshPro - Text (UI)** → "QuitText"
    - Text: "ВЫХОД"

### Настройка MainMenuUI скрипта:
Подключить все UI элементы к соответствующим полям в Inspector.

### Создание префаба:
1. Перетащить MainMenuPanel в папку Prefabs/UI/Panels/
2. Удалить из сцены
3. Подключить к UIManager в поле "Main Menu Panel"

## Этап 7: Тестирование

### Запуск и проверка:
1. Запустить сцену
2. Проверить появление главного меню
3. Протестировать кнопки (должны выводить сообщения в Console)
4. Проверить анимации появления

## Следующие шаги

После завершения этого этапа:
1. Создать остальные UI панели
2. Настроить навигацию между экранами
3. Добавить визуальные эффекты
4. Интегрировать с игровой логикой
