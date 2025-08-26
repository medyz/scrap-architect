# Установка и настройка Unity 2022.3 LTS

## 🚀 Пошаговая установка Unity

### Шаг 1: Установка Unity Hub

1. **Скачайте Unity Hub:**
   - Перейдите на [unity.com/download](https://unity.com/download)
   - Нажмите "Download Unity Hub"
   - Выберите версию для Windows

2. **Установите Unity Hub:**
   - Запустите скачанный файл
   - Следуйте инструкциям установщика
   - Рекомендуемые настройки:
     - ✅ Установить для всех пользователей
     - ✅ Создать ярлык на рабочем столе
     - ✅ Добавить в PATH

### Шаг 2: Установка Unity 2022.3 LTS

1. **Запустите Unity Hub**
2. **Перейдите в раздел "Installs"**
3. **Нажмите "Install Editor"**
4. **Выберите версию:**
   - Unity 2022.3.15f1 LTS (рекомендуется)
   - Или Unity 2022.3.20f1 LTS (последняя)

5. **Выберите модули для установки:**
   - ✅ Microsoft Visual Studio Community 2022
   - ✅ Windows Build Support
   - ✅ Android Build Support (опционально)
   - ✅ iOS Build Support (опционально)
   - ✅ Linux Build Support (опционально)

### Шаг 3: Настройка проекта

1. **Создайте новый проект:**
   - В Unity Hub нажмите "New Project"
   - Выберите "3D Core" или "3D"
   - Название: `Scrap Architect`
   - Расположение: `C:\Users\meduz\Desktop\Scrap Architect\Unity`
   - Unity версия: 2022.3.15f1

2. **Настройте проект:**
   - Откройте Edit → Project Settings
   - Player Settings:
     - Company Name: `Scrap Architect Team`
     - Product Name: `Scrap Architect`
     - Version: `0.1.0`
   - Quality Settings:
     - Установите качество "High" по умолчанию
   - Physics Settings:
     - Default Solver Iterations: 6
     - Default Solver Velocity Iterations: 2

## 📁 Структура папок Unity проекта

```
Assets/
├── Scripts/
│   ├── Core/
│   │   ├── GameManager.cs
│   │   ├── SaveSystem.cs
│   │   └── AudioManager.cs
│   ├── Physics/
│   │   ├── PartController.cs
│   │   ├── JointManager.cs
│   │   └── PhysicsOptimizer.cs
│   ├── UI/
│   │   ├── UIManager.cs
│   │   ├── BuildModeUI.cs
│   │   └── ContractUI.cs
│   └── Parts/
│       ├── PartBase.cs
│       ├── Motor.cs
│       └── Wheel.cs
├── Prefabs/
│   ├── Parts/
│   ├── UI/
│   └── Effects/
├── Materials/
│   ├── Parts/
│   ├── UI/
│   └── Effects/
├── Textures/
│   ├── Parts/
│   ├── UI/
│   └── Backgrounds/
├── Models/
│   ├── Parts/
│   ├── Environment/
│   └── Props/
├── Audio/
│   ├── SFX/
│   ├── Music/
│   └── UI/
├── Scenes/
│   ├── MainMenu.unity
│   ├── BuildMode.unity
│   └── TestMode.unity
└── Resources/
    ├── Configs/
    └── Localization/
```

## ⚙️ Настройки проекта

### Physics Settings (Edit → Project Settings → Physics)
```
Default Solver Iterations: 6
Default Solver Velocity Iterations: 2
Default Max Angular Speed: 7
Sleep Threshold: 0.005
Default Contact Offset: 0.01
Default Skin Width: 0.01
Bounce Threshold: 2
Sleep Velocity: 0.15
Sleep Angular Velocity: 0.14
```

### Quality Settings (Edit → Project Settings → Quality)
```
Anti Aliasing: 4x Multi Sampling
Texture Quality: Full Res
Anisotropic Textures: Per Texture
Soft Particles: Enabled
Realtime Reflection Probes: Enabled
Billboards Face Camera Position: Enabled
```

### Player Settings (Edit → Project Settings → Player)
```
Company Name: Scrap Architect Team
Product Name: Scrap Architect
Version: 0.1.0
Default Icon: [установить позже]
Cursor Hotspot: (0, 0)
```

## 🔧 Дополнительные настройки

### 1. Настройка Visual Studio
- В Unity: Edit → Preferences → External Tools
- External Script Editor: Visual Studio 2022
- ✅ Generate .csproj files for
- ✅ Regenerate project files

### 2. Настройка Git
- В Unity: Edit → Project Settings → Editor
- Version Control Mode: Visible Meta Files
- Asset Serialization Mode: Force Text

### 3. Настройка Input System
- Window → Package Manager
- Установите "Input System" package
- Включите "Active Input Handling" в Project Settings

## 📦 Рекомендуемые пакеты

### Обязательные пакеты:
- **Input System** - новая система ввода
- **TextMeshPro** - улучшенный текст
- **Cinemachine** - камера и кинематография
- **Post Processing** - пост-обработка

### Дополнительные пакеты:
- **ProBuilder** - 3D моделирование в Unity
- **ProGrids** - сетка для точного позиционирования
- **Unity Recorder** - запись видео и скриншотов

## 🎯 Следующие шаги

После установки Unity:

1. **Создайте базовую структуру папок**
2. **Настройте первую сцену**
3. **Создайте базовые скрипты**
4. **Настройте физику**
5. **Создайте простой прототип**

## 🔍 Проверка установки

После установки проверьте:

1. Unity Hub запускается
2. Unity 2022.3.15f1 установлен
3. Visual Studio 2022 установлен
4. Проект создается без ошибок
5. Git интегрирован с Unity

## 📞 Поддержка

Если возникнут проблемы:
- [Unity Documentation](https://docs.unity3d.com/)
- [Unity Forums](https://forum.unity.com/)
- [Unity Learn](https://learn.unity.com/)
