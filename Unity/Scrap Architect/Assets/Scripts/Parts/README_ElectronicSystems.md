# Электронные Системы

## Обзор

Электронные системы предоставляют игрокам возможность создавать автоматизированные механизмы, интеллектуальные устройства и сложные логические схемы. Эти системы позволяют создавать умные конструкции, которые могут реагировать на окружающую среду и выполнять сложные задачи без прямого вмешательства игрока.

## Компоненты системы

### 1. Датчик (Sensor)

**Описание**: Электронный компонент для измерения различных параметров окружающей среды.

**Типы**:
- **Датчик расстояния** - измеряет расстояние до объектов
- **Датчик приближения** - обнаруживает близкие объекты
- **Датчик давления** - измеряет давление в системе
- **Датчик температуры** - измеряет температуру
- **Датчик освещенности** - измеряет уровень освещения
- **Датчик движения** - обнаруживает движущиеся объекты

**Характеристики**:
- Диапазон: 2-10 метров
- Точность: 0.01-0.5
- Частота обновления: 2-30 Гц
- Выходные значения: 0-100%

**Визуальные эффекты**:
- Система частиц для отображения активности
- Gizmos для зоны действия
- Цветовая индикация состояния

### 2. Контроллер (Controller)

**Описание**: Электронный компонент для управления другими устройствами на основе логики.

**Типы**:
- **Базовый** - простые логические операции
- **Продвинутый** - расширенные возможности
- **Программируемый** - настраиваемая логика
- **Логический** - специализированные логические операции

**Характеристики**:
- Входы: 4-12
- Выходы: 4-12
- Скорость обработки: 10-50 Гц
- Типы логики: AND, OR, NOT, XOR, NAND, NOR, Threshold, Timer

**Визуальные эффекты**:
- Индикаторы входов и выходов
- Система частиц для активности
- Gizmos для подключений

### 3. Логический Элемент (LogicGate)

**Описание**: Базовый компонент для создания цифровых схем и логических операций.

**Типы**:
- **AND** - логическое И
- **OR** - логическое ИЛИ
- **NOT** - логическое НЕ
- **NAND** - И-НЕ
- **NOR** - ИЛИ-НЕ
- **XOR** - исключающее ИЛИ
- **XNOR** - исключающее ИЛИ-НЕ
- **Buffer** - буфер

**Характеристики**:
- Входы: 1-2
- Выходы: 1
- Задержка: 0.005-0.015 секунд
- Совместимость с цифровыми схемами

**Визуальные эффекты**:
- Цветные контакты для входов/выходов
- Система частиц при переключении
- Gizmos для отладки

## Использование в коде

### Создание датчика

```csharp
// Создание объекта
GameObject sensorObj = new GameObject("DistanceSensor");
Sensor sensor = sensorObj.AddComponent<Sensor>();

// Настройка типа
sensor.sensorType = Sensor.SensorType.Distance;
sensor.sensorRange = 10f;

// Активация
sensor.ActivateSensor();

// Получение данных
float distance = sensor.GetSensorValue();
bool triggered = sensor.IsTriggered();
```

### Создание контроллера

```csharp
// Создание объекта
GameObject controllerObj = new GameObject("BasicController");
Controller controller = controllerObj.AddComponent<Controller>();

// Настройка типа
controller.controllerType = Controller.ControllerType.Basic;
controller.logicType = Controller.LogicType.AND;

// Активация
controller.ActivateController();

// Подключение датчика
controller.ConnectSensor(sensor, 0);

// Подключение устройства
controller.ConnectDevice(motor, 0);
```

### Создание логического элемента

```csharp
// Создание объекта
GameObject gateObj = new GameObject("ANDGate");
LogicGate gate = gateObj.AddComponent<LogicGate>();

// Настройка типа
gate.gateType = LogicGate.LogicGateType.AND;

// Активация
gate.ActivateGate();

// Установка входов
gate.SetInput(0, true);
gate.SetInput(1, false);

// Получение выхода
bool output = gate.GetOutputState(0);
```

## Стратегии использования

### 1. Автоматические системы

```csharp
// Создание автоматической системы освещения
Sensor lightSensor = CreateSensor(Sensor.SensorType.Light);
Controller lightController = CreateController(Controller.ControllerType.Basic);
Light light = CreateLight();

// Настройка логики
lightController.SetLogicType(Controller.LogicType.Threshold);
lightController.thresholdValue = 0.3f;

// Подключения
lightController.ConnectSensor(lightSensor, 0);
lightController.ConnectDevice(light, 0);
```

### 2. Системы безопасности

```csharp
// Создание системы безопасности
Sensor motionSensor = CreateSensor(Sensor.SensorType.Motion);
Sensor proximitySensor = CreateSensor(Sensor.SensorType.Proximity);
Controller securityController = CreateController(Controller.ControllerType.Advanced);
Alarm alarm = CreateAlarm();

// Настройка логики
securityController.SetLogicType(Controller.LogicType.OR);

// Подключения
securityController.ConnectSensor(motionSensor, 0);
securityController.ConnectSensor(proximitySensor, 1);
securityController.ConnectDevice(alarm, 0);
```

### 3. Цифровые схемы

```csharp
// Создание простого сумматора
LogicGate andGate = CreateLogicGate(LogicGate.LogicGateType.AND);
LogicGate xorGate = CreateLogicGate(LogicGate.LogicGateType.XOR);
LogicGate orGate = CreateLogicGate(LogicGate.LogicGateType.OR);

// Подключение схемы
// A и B -> XOR -> Sum
// A и B -> AND -> Carry
// Carry и Cin -> OR -> Cout
```

### 4. Системы управления движением

```csharp
// Создание системы управления движением
Sensor distanceSensor = CreateSensor(Sensor.SensorType.Distance);
Controller movementController = CreateController(Controller.ControllerType.Programmable);
Motor leftMotor = CreateMotor();
Motor rightMotor = CreateMotor();

// Настройка логики
movementController.SetLogicType(Controller.LogicType.Threshold);

// Подключения
movementController.ConnectSensor(distanceSensor, 0);
movementController.ConnectDevice(leftMotor, 0);
movementController.ConnectDevice(rightMotor, 1);
```

## Интеграция с другими системами

### С пневматическими/гидравлическими системами

```csharp
// Автоматическое управление цилиндром
Sensor pressureSensor = CreateSensor(Sensor.SensorType.Pressure);
Controller cylinderController = CreateController(Controller.ControllerType.Basic);
PneumaticCylinder cylinder = CreatePneumaticCylinder();

// Логика: если давление низкое, выдвинуть цилиндр
cylinderController.SetLogicType(Controller.LogicType.Threshold);
cylinderController.thresholdValue = 0.5f;

cylinderController.ConnectSensor(pressureSensor, 0);
cylinderController.ConnectDevice(cylinder, 0);
```

### С двигателями и движителями

```csharp
// Автоматическое управление скоростью
Sensor speedSensor = CreateSensor(Sensor.SensorType.Motion);
Controller speedController = CreateController(Controller.ControllerType.Advanced);
Motor motor = CreateMotor();

// Логика: регулировка скорости на основе движения
speedController.SetLogicType(Controller.LogicType.Threshold);

speedController.ConnectSensor(speedSensor, 0);
speedController.ConnectDevice(motor, 0);
```

## Балансировка

### Датчики
- **Преимущества**: Высокая точность, быстрый отклик
- **Недостатки**: Ограниченный диапазон, энергопотребление
- **Применение**: Автоматизация, безопасность, контроль

### Контроллеры
- **Преимущества**: Гибкость, сложная логика
- **Недостатки**: Сложность настройки, стоимость
- **Применение**: Управление системами, автоматизация

### Логические элементы
- **Преимущества**: Простота, надежность, скорость
- **Недостатки**: Ограниченная функциональность
- **Применение**: Цифровые схемы, простые логические операции

## Будущие расширения

### Планируемые компоненты
1. **Микроконтроллеры** - программируемые устройства
2. **Память** - устройства хранения данных
3. **Таймеры** - устройства задержки и синхронизации
4. **Мультиплексоры** - устройства выбора сигналов
5. **Декодеры** - устройства преобразования кодов

### Улучшения системы
1. **Программируемая логика** - создание пользовательских алгоритмов
2. **Сетевые протоколы** - связь между устройствами
3. **Аналоговые сигналы** - непрерывные значения
4. **Шум и помехи** - реалистичные условия работы
5. **Энергопотребление** - учет потребления энергии

## Заключение

Электронные системы значительно расширяют возможности игроков в создании интеллектуальных механизмов. Они предоставляют инструменты для автоматизации, создания сложных логических схем и разработки умных устройств.

Система спроектирована с учетом баланса между простотой использования и глубиной возможностей, что делает её доступной для новичков и интересной для опытных игроков.
