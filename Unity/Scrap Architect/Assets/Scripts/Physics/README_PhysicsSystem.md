# Физическая система - Система строительства

## Обзор

Физическая система обеспечивает реалистичное взаимодействие деталей в игре "Scrap Architect". Система включает в себя snap-points, привязку деталей, соединения и физическую симуляцию.

## Компоненты системы

### 1. PartController - Контроллер детали
**Файл:** `PartController.cs`

Основной контроллер для всех деталей в игре.

**Основные функции:**
- Управление физикой (Rigidbody, Collider)
- Обработка соединений и Joints
- Интеграция с snap-points
- Управление состоянием детали

**Ключевые свойства:**
- `mass` - Масса детали
- `maxHealth` - Максимальное здоровье
- `snapDistance` - Расстояние привязки
- `snapAngle` - Угол привязки
- `useSnapPoints` - Использовать snap-points
- `autoSnap` - Автоматическая привязка

### 2. SnapPoint - Точка привязки
**Файл:** `SnapPoint.cs`

Точки привязки для соединения деталей.

**Типы snap-points:**
- **Universal** - Универсальный (совместим со всеми)
- **Block** - Только для блоков
- **Wheel** - Только для колес
- **Motor** - Только для двигателей
- **Connection** - Только для соединений
- **Tool** - Только для инструментов
- **Seat** - Только для сидений

**Ключевые функции:**
- `CanSnapTo()` - Проверка совместимости
- `TrySnapTo()` - Попытка привязки
- `Unsnap()` - Отвязка
- `SetHighlighted()` - Подсветка

### 3. PartAttacher - Система привязки
**Файл:** `PartAttacher.cs`

Управляет процессом привязки деталей друг к другу.

**Основные функции:**
- `StartDragging()` - Начать перетаскивание
- `StopDragging()` - Остановить перетаскивание
- `UpdateDragPosition()` - Обновить позицию
- `TryAttachToSnapPoint()` - Привязать к snap-point
- `DetachAll()` - Отвязать все соединения

**Визуальные эффекты:**
- Превью привязки
- Подсветка совместимых snap-points
- Материалы для разных состояний

## Система соединений

### Типы соединений
1. **Fixed Joint** - Жесткое соединение
2. **Hinge Joint** - Шарнирное соединение
3. **Spring Joint** - Пружинное соединение
4. **Configurable Joint** - Настраиваемое соединение

### Правила совместимости
- **Universal** совместим со всеми типами
- **Connection** совместим с Block, Wheel, Motor, Tool, Seat
- **Block** совместим с Block и Connection
- **Wheel** совместим с Wheel и Connection
- **Motor** совместим с Motor и Connection
- **Tool** совместим только с Connection
- **Seat** совместим только с Connection

## Процесс привязки

### 1. Поиск snap-points
```csharp
// Найти все snap-points в радиусе
SnapPoint[] nearbySnapPoints = FindObjectsOfType<SnapPoint>();
```

### 2. Проверка совместимости
```csharp
// Проверить тип и направление
bool canSnap = snapPoint1.CanSnapTo(snapPoint2);
```

### 3. Выполнение привязки
```csharp
// Привязать snap-points
bool success = snapPoint1.TrySnapTo(snapPoint2);
```

### 4. Позиционирование детали
```csharp
// Установить точную позицию и поворот
transform.position = snapPoint.GetSnapPosition();
transform.rotation = snapPoint.GetSnapRotation();
```

## Визуальная обратная связь

### Состояния snap-points
- **Доступный** - Зеленый цвет
- **Занятый** - Красный цвет
- **Подсвеченный** - Желтый цвет
- **Неактивный** - Серый цвет

### Превью привязки
- Показывает, где будет размещена деталь
- Меняет цвет в зависимости от валидности
- Автоматически скрывается при отмене

## Звуковые эффекты

### Типы звуков
- `snapSound` - Звук успешной привязки
- `unsnapSound` - Звук отвязки
- `snapErrorSound` - Звук ошибки привязки

### Воспроизведение
```csharp
AudioSource.PlayClipAtPoint(snapSound, transform.position);
```

## Интеграция с другими системами

### PartManipulator
- Использует PartAttacher для перетаскивания
- Обрабатывает ввод пользователя
- Управляет визуальными эффектами

### GameController
- Мониторит состояние соединений
- Управляет физической симуляцией
- Обрабатывает события привязки

### UIManager
- Показывает информацию о snap-points
- Отображает состояние привязки
- Предоставляет подсказки пользователю

## Оптимизация

### Производительность
- Использование Object Pooling для превью
- Кэширование компонентов
- Оптимизация поиска snap-points

### Лимиты
- Максимум 10 snap-points на деталь
- Максимум 100 snap-points в сцене
- Радиус поиска: 5 единиц

## Отладка

### Gizmos в редакторе
- Отображение зон snap-points
- Визуализация направлений
- Показ состояния соединений

### Логирование
```csharp
Debug.Log($"Snap points connected: {part1} -> {part2}");
Debug.Log($"Part attached: {partName} to {targetPart}");
```

## Создание новых snap-points

### Шаг 1: Создание GameObject
```csharp
GameObject snapPointObj = new GameObject("SnapPoint");
snapPointObj.transform.SetParent(partTransform);
```

### Шаг 2: Добавление компонента
```csharp
SnapPoint snapPoint = snapPointObj.AddComponent<SnapPoint>();
snapPoint.snapType = SnapPointType.Block;
snapPoint.snapDirection = Vector3.up;
```

### Шаг 3: Настройка визуализации
```csharp
snapPoint.snapIndicator = indicatorPrefab;
snapPoint.availableMaterial = greenMaterial;
snapPoint.occupiedMaterial = redMaterial;
```

## Заключение

Физическая система предоставляет мощный и гибкий механизм для строительства машин. Система snap-points обеспечивает интуитивное соединение деталей, а интеграция с Unity Physics обеспечивает реалистичное поведение конструкций.
