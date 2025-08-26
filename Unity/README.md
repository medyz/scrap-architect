# Unity Project - Scrap Architect

## 🎮 Описание проекта

Это Unity проект для игры "Scrap Architect" - физического симулятора строительства машин из подручных материалов.

## 📁 Структура проекта

После создания Unity проекта в этой папке, структура должна быть следующей:

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

## 🚀 Создание проекта

1. Откройте Unity Hub
2. Нажмите "New Project"
3. Выберите "3D Core"
4. Название: `Scrap Architect`
5. Расположение: `C:\Users\meduz\Desktop\Scrap Architect\Unity`
6. Unity версия: 2022.3.62f1

## ⚙️ Настройки проекта

### Player Settings
- Company Name: `Scrap Architect Team`
- Product Name: `Scrap Architect`
- Version: `0.1.0`

### Physics Settings
- Default Solver Iterations: 6
- Default Solver Velocity Iterations: 2
- Default Max Angular Speed: 7
- Sleep Threshold: 0.005

### Quality Settings
- Anti Aliasing: 4x Multi Sampling
- Texture Quality: Full Res
- Anisotropic Textures: Per Texture

## 📦 Рекомендуемые пакеты

- Input System
- TextMeshPro
- Cinemachine
- Post Processing
- ProBuilder (опционально)
- Unity Recorder (опционально)

## 🔧 Настройка Git

В Unity:
1. Edit → Project Settings → Editor
2. Version Control Mode: Visible Meta Files
3. Asset Serialization Mode: Force Text

## 📞 Поддержка

Если возникнут проблемы:
- [Unity Documentation](https://docs.unity3d.com/)
- [Unity Forums](https://forum.unity.com/)
- [Unity Learn](https://learn.unity.com/)
