# Система Blueprint - Сохранение и загрузка конструкций

## Обзор

Система Blueprint обеспечивает полное сохранение и загрузку конструкций машин в игре "Scrap Architect". Система включает в себя сериализацию всех деталей, соединений и их свойств в JSON формат.

## Компоненты системы

### 1. VehicleBlueprint - Основной класс blueprint
**Файл:** `VehicleBlueprint.cs`

Основной класс для хранения данных о конструкции машины.

**Структура данных:**
- **PartData** - Информация о детали (позиция, поворот, свойства)
- **ConnectionData** - Информация о соединениях между деталями
- **Metadata** - Дополнительная информация (автор, дата, теги)

**Ключевые функции:**
- `CreateFromVehicle()` - Создать blueprint из машины
- `CreateVehicle()` - Воссоздать машину из blueprint
- `GetBlueprintInfo()` - Получить информацию о blueprint
- `IsValid()` - Проверить валидность

### 2. BlueprintManager - Менеджер blueprint
**Файл:** `BlueprintManager.cs`

Управляет сохранением, загрузкой и организацией blueprint.

**Основные функции:**
- `SaveCurrentVehicle()` - Сохранить текущую машину
- `LoadBlueprint()` - Загрузить blueprint
- `DeleteBlueprint()` - Удалить blueprint
- `ExportBlueprint()` - Экспортировать blueprint
- `ImportBlueprint()` - Импортировать blueprint

**Автосохранение:**
- Автоматическое сохранение каждые 60 секунд
- Сохранение при потере фокуса приложения
- Сохранение при паузе

### 3. BlueprintListItem - UI элемент списка
**Файл:** `BlueprintListItem.cs`

UI элемент для отображения blueprint в списке.

**Функции:**
- Отображение информации о blueprint
- Кнопки загрузки, удаления, экспорта
- Визуальная обратная связь
- Звуковые эффекты

## Структура данных

### PartData
```csharp
public class PartData
{
    public string partID;           // Уникальный ID детали
    public string partName;         // Название детали
    public string partType;         // Тип детали
    public Vector3 position;        // Позиция в мире
    public Quaternion rotation;     // Поворот
    public Vector3 scale;           // Масштаб
    public float health;            // Здоровье
    public float mass;              // Масса
    public Dictionary<string, object> customProperties; // Специфичные свойства
}
```

### ConnectionData
```csharp
public class ConnectionData
{
    public int part1Index;          // Индекс первой детали
    public int part2Index;          // Индекс второй детали
    public string connectionType;   // Тип соединения
    public Vector3 connectionPoint1; // Точка соединения 1
    public Vector3 connectionPoint2; // Точка соединения 2
    public Dictionary<string, object> jointProperties; // Свойства соединения
}
```

### VehicleBlueprint
```csharp
public class VehicleBlueprint
{
    // Информация о blueprint
    public string blueprintName;
    public string description;
    public string author;
    public DateTime creationDate;
    public DateTime lastModified;
    public string version;
    
    // Данные машины
    public List<PartData> parts;
    public List<ConnectionData> connections;
    public Vector3 centerOfMass;
    public float totalMass;
    public int totalParts;
    
    // Метаданные
    public Dictionary<string, object> metadata;
    public List<string> tags;
    public float rating;
    public int downloadCount;
}
```

## Процесс сохранения

### 1. Создание blueprint
```csharp
// Создать blueprint из текущей машины
VehicleBlueprint blueprint = VehicleBlueprint.CreateFromVehicle(vehicleRoot);
```

### 2. Сохранение специфичных свойств
```csharp
// Сохранить свойства в зависимости от типа детали
switch (part.partType)
{
    case PartType.Block:
        // Сохранить тип блока и материал
        break;
    case PartType.Wheel:
        // Сохранить тип колеса, радиус, ширину
        break;
    case PartType.Motor:
        // Сохранить тип двигателя, мощность, уровень топлива
        break;
    // ... другие типы
}
```

### 3. Сохранение snap-points
```csharp
// Сохранить все snap-points детали
SnapPoint[] snapPoints = part.GetSnapPoints();
foreach (SnapPoint snapPoint in snapPoints)
{
    // Сохранить тип, направление, радиус, состояние
}
```

### 4. Сохранение соединений
```csharp
// Найти все активные соединения
foreach (SnapPoint snapPoint in snapPoints)
{
    if (snapPoint.IsOccupied())
    {
        // Сохранить информацию о соединении
    }
}
```

## Процесс загрузки

### 1. Создание деталей
```csharp
// Создать базовый объект
GameObject partObject = new GameObject(partData.partName);

// Добавить компоненты
Rigidbody rb = partObject.AddComponent<Rigidbody>();
BoxCollider col = partObject.AddComponent<BoxCollider>();
PartController partController = partObject.AddComponent<PartController>();
```

### 2. Восстановление свойств
```csharp
// Восстановить базовые свойства
partController.partName = partData.partName;
partController.partType = ParsePartType(partData.partType);
partController.mass = partData.mass;
partController.currentHealth = partData.health;
```

### 3. Восстановление специфичных компонентов
```csharp
// Добавить специфичные компоненты
switch (partController.partType)
{
    case PartType.Block:
        Block block = partObject.AddComponent<Block>();
        // Восстановить свойства блока
        break;
    case PartType.Wheel:
        Wheel wheel = partObject.AddComponent<Wheel>();
        // Восстановить свойства колеса
        break;
    // ... другие типы
}
```

### 4. Восстановление соединений
```csharp
// Восстановить все соединения
foreach (ConnectionData connectionData in connections)
{
    // Найти соответствующие snap-points
    // Выполнить соединение
    snapPoint1.TrySnapTo(snapPoint2);
}
```

## Файловая система

### Структура папок
```
Application.persistentDataPath/
├── Blueprints/           # Основная папка blueprint
│   ├── Vehicle1_20241201_143022.json
│   ├── Vehicle2_20241201_150045.json
│   └── ...
└── Exports/             # Экспортированные blueprint
    ├── Vehicle1_export.json
    └── ...
```

### Формат файлов
- **Расширение:** `.json`
- **Кодировка:** UTF-8
- **Структура:** JSON с полной информацией о машине

### Именование файлов
```csharp
// Формат: Name_YYYYMMDD_HHMMSS.json
string fileName = $"{blueprintName}_{creationDate:yyyyMMdd_HHmmss}.json";
```

## Интеграция с UI

### BlueprintPanel
- Список всех сохраненных blueprint
- Кнопки управления (сохранить, загрузить, удалить)
- Поиск и фильтрация
- Сортировка по дате/имени

### BlueprintListItem
- Отображение информации о blueprint
- Миниатюра (цветовая индикация типа)
- Кнопки быстрого доступа
- Визуальная обратная связь

## Оптимизация

### Производительность
- Ленивая загрузка blueprint
- Кэширование часто используемых данных
- Оптимизация JSON сериализации
- Сжатие больших файлов

### Безопасность
- Валидация загружаемых данных
- Проверка целостности файлов
- Резервное копирование
- Обработка ошибок

## Расширения

### Steam Workshop
- Загрузка blueprint в Workshop
- Подписка на blueprint других игроков
- Рейтинг и комментарии
- Автоматическое обновление

### Облачное сохранение
- Синхронизация между устройствами
- Автоматическое резервное копирование
- Совместное использование blueprint

### Версионирование
- Поддержка разных версий формата
- Автоматическая миграция данных
- Обратная совместимость

## Отладка

### Логирование
```csharp
Debug.Log($"Blueprint saved: {blueprintName}");
Debug.Log($"Blueprint loaded: {blueprint.blueprintName}");
Debug.LogError($"Failed to save blueprint: {e.Message}");
```

### Валидация
```csharp
// Проверить валидность blueprint
if (!blueprint.IsValid())
{
    Debug.LogError("Invalid blueprint data");
    return false;
}
```

### Тестирование
- Создание тестовых blueprint
- Проверка загрузки/сохранения
- Тестирование граничных случаев
- Производительные тесты

## Заключение

Система Blueprint предоставляет полное решение для сохранения и загрузки конструкций машин. Система масштабируема, оптимизирована и готова к интеграции с внешними сервисами.
