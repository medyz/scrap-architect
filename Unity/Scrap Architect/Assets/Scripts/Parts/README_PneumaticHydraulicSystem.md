# Пневматические и Гидравлические Системы

## Обзор

Пневматические и гидравлические системы предоставляют игрокам мощные инструменты для создания сложных механизмов с линейным движением и управлением потоком. Эти системы идеально подходят для создания подъемников, манипуляторов, систем управления и других механических устройств.

## Компоненты системы

### 1. Пневматический Цилиндр (PneumaticCylinder)

**Описание**: Устройство для создания линейного движения с помощью сжатого воздуха.

**Типы**:
- **Стандартный** - сбалансированные характеристики
- **Тяжелый** - высокая сила, низкая скорость
- **Быстрый** - высокая скорость, средняя сила
- **Точный** - точное позиционирование

**Характеристики**:
- Сила: 1000-2000N
- Скорость: 1-4 м/с
- Ход: 0.5-1.5м
- Давление: 0.6-1.5

**Визуальные эффекты**:
- Система частиц для отображения давления
- Анимированный шток цилиндра
- Gizmos для направления движения

### 2. Гидравлический Насос (HydraulicPump)

**Описание**: Устройство для создания давления в гидравлических системах.

**Типы**:
- **Стандартный** - сбалансированные характеристики
- **Высоконапорный** - высокое давление, низкий поток
- **Высокопроизводительный** - высокий поток, низкое давление
- **Эффективный** - высокая эффективность

**Характеристики**:
- Давление: 50-200 бар
- Поток: 5-25 л/мин
- Мощность: 800-1500 Вт
- Эффективность: 70-95%

**Визуальные эффекты**:
- Вращающийся ротор насоса
- Система частиц для гидравлики
- Индикатор температуры

### 3. Гидравлический Цилиндр (HydraulicCylinder)

**Описание**: Мощное устройство для создания линейного движения с помощью гидравлики.

**Типы**:
- **Стандартный** - сбалансированные характеристики
- **Тяжелый** - максимальная сила
- **Быстрый** - высокая скорость
- **Точный** - точное позиционирование

**Характеристики**:
- Сила: 2000-10000N
- Скорость: 0.5-2 м/с
- Ход: 1-3м
- Давление: 60-150 бар

**Визуальные эффекты**:
- Система частиц для гидравлики
- Анимированный шток цилиндра
- Gizmos для направления движения

### 4. Гидравлический Клапан (HydraulicValve)

**Описание**: Устройство для управления потоком жидкости в гидравлических системах.

**Типы**:
- **Стандартный** - сбалансированные характеристики
- **Высокопроизводительный** - высокий поток
- **Точный** - высокая точность управления
- **Напорный** - высокое давление

**Характеристики**:
- Поток: 10-40 л/мин
- Давление: 80-200 бар
- Эффективность: 80-95%
- Скорость отклика: 0.3-0.8

**Визуальные эффекты**:
- Поворачивающаяся ручка клапана
- Система частиц для потока
- Индикатор открытия

## Использование в коде

### Создание пневматического цилиндра

```csharp
// Создание объекта
GameObject cylinderObj = new GameObject("PneumaticCylinder");
PneumaticCylinder cylinder = cylinderObj.AddComponent<PneumaticCylinder>();

// Настройка типа
cylinder.cylinderType = PneumaticCylinder.PneumaticType.Heavy;

// Активация
cylinder.ActivateCylinder();

// Управление
cylinder.ExtendCylinder();  // Выдвинуть
cylinder.RetractCylinder(); // Втянуть
cylinder.SetCylinderPosition(0.5f); // Установить позицию
```

### Создание гидравлического насоса

```csharp
// Создание объекта
GameObject pumpObj = new GameObject("HydraulicPump");
HydraulicPump pump = pumpObj.AddComponent<HydraulicPump>();

// Настройка типа
pump.pumpType = HydraulicPump.HydraulicType.HighPressure;

// Запуск
pump.StartPump();

// Управление скоростью
pump.SetPumpSpeed(2000f); // RPM

// Получение параметров
float pressure = pump.GetPumpPressure();
float flow = pump.GetPumpFlow();
```

### Создание гидравлического цилиндра

```csharp
// Создание объекта
GameObject hydraulicObj = new GameObject("HydraulicCylinder");
HydraulicCylinder hydraulic = hydraulicObj.AddComponent<HydraulicCylinder>();

// Настройка типа
hydraulic.cylinderType = HydraulicCylinder.HydraulicCylinderType.Heavy;

// Активация
hydraulic.ActivateCylinder();

// Управление
hydraulic.ExtendCylinder();
hydraulic.RetractCylinder();
hydraulic.SetCylinderPosition(1.5f);
```

### Создание гидравлического клапана

```csharp
// Создание объекта
GameObject valveObj = new GameObject("HydraulicValve");
HydraulicValve valve = valveObj.AddComponent<HydraulicValve>();

// Настройка типа
valve.valveType = HydraulicValve.HydraulicValveType.Precise;

// Активация
valve.ActivateValve();

// Управление
valve.OpenValve();  // Полностью открыть
valve.CloseValve(); // Полностью закрыть
valve.SetValveOpening(0.7f); // Установить открытие
```

## Стратегии использования

### 1. Подъемные механизмы

```csharp
// Создание подъемника с гидравлическим цилиндром
HydraulicCylinder liftCylinder = CreateHydraulicCylinder(HydraulicCylinder.HydraulicCylinderType.Heavy);
HydraulicPump liftPump = CreateHydraulicPump(HydraulicPump.HydraulicType.HighPressure);
HydraulicValve liftValve = CreateHydraulicValve(HydraulicValve.HydraulicValveType.Precise);

// Подъем
liftValve.OpenValve();
liftCylinder.ExtendCylinder();

// Опускание
liftValve.CloseValve();
liftCylinder.RetractCylinder();
```

### 2. Манипуляторы

```csharp
// Создание манипулятора с пневматическими цилиндрами
PneumaticCylinder[] manipulatorCylinders = new PneumaticCylinder[3];

for (int i = 0; i < 3; i++)
{
    manipulatorCylinders[i] = CreatePneumaticCylinder(PneumaticCylinder.PneumaticType.Precise);
    manipulatorCylinders[i].ActivateCylinder();
}

// Управление позицией
void SetManipulatorPosition(Vector3 position)
{
    manipulatorCylinders[0].SetCylinderPosition(position.x);
    manipulatorCylinders[1].SetCylinderPosition(position.y);
    manipulatorCylinders[2].SetCylinderPosition(position.z);
}
```

### 3. Системы управления

```csharp
// Создание системы управления с клапанами
HydraulicValve[] controlValves = new HydraulicValve[4];

for (int i = 0; i < 4; i++)
{
    controlValves[i] = CreateHydraulicValve(HydraulicValve.HydraulicValveType.Standard);
    controlValves[i].ActivateValve();
}

// Управление системой
void ControlSystem(float[] openings)
{
    for (int i = 0; i < controlValves.Length && i < openings.Length; i++)
    {
        controlValves[i].SetValveOpening(openings[i]);
    }
}
```

## Балансировка

### Пневматические системы
- **Преимущества**: Быстрый отклик, простота управления
- **Недостатки**: Ограниченная сила, зависимость от давления
- **Применение**: Точные манипуляции, быстрые движения

### Гидравлические системы
- **Преимущества**: Высокая сила, точное позиционирование
- **Недостатки**: Медленный отклик, сложность системы
- **Применение**: Тяжелые грузы, точные операции

## Интеграция с другими системами

### С системой экономики
```csharp
// Получение стоимости компонентов
int totalCost = cylinder.GetPartCost() + pump.GetPartCost() + valve.GetPartCost();

// Проверка доступности
if (EconomyManager.Instance.GetScrap() >= totalCost)
{
    // Покупка компонентов
    EconomyManager.Instance.SpendScrap(totalCost);
}
```

### С системой прогресса
```csharp
// Разблокировка компонентов по уровню
if (PlayerProgress.Instance.GetLevel() >= 10)
{
    // Доступны гидравлические системы
    UnlockHydraulicSystems();
}
```

### С системой контрактов
```csharp
// Проверка выполнения контракта
if (contract.objectiveType == ContractObjective.ObjectiveType.BuildLifter)
{
    // Создание подъемника с пневматическими/гидравлическими системами
    CreateLifterSystem();
}
```

## Будущие расширения

### Планируемые компоненты
1. **Пневматические клапаны** - управление воздушным потоком
2. **Гидравлические аккумуляторы** - накопление энергии
3. **Пневматические ресиверы** - хранение сжатого воздуха
4. **Гидравлические фильтры** - очистка жидкости
5. **Пневматические регуляторы** - стабилизация давления

### Улучшения системы
1. **Реалистичная физика** - учет сжатия воздуха/жидкости
2. **Система утечек** - реалистичные потери давления
3. **Тепловые эффекты** - влияние температуры на работу
4. **Шумовые эффекты** - звуки работы систем
5. **Визуальные улучшения** - более детальные модели

## Заключение

Пневматические и гидравлические системы значительно расширяют возможности игроков в создании сложных механизмов. Они предоставляют мощные инструменты для решения различных инженерных задач и создания интересных конструкций.

Система спроектирована с учетом баланса между простотой использования и глубиной возможностей, что делает её доступной для новичков и интересной для опытных игроков.
