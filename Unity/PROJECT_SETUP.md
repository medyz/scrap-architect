# Создание Unity проекта - Фаза 1

## 🎮 Создание проекта

### Шаг 1: Открыть Unity Hub
1. Запустите Unity Hub: `C:\Program Files\Unity Hub\Unity Hub.exe`
2. Войдите в аккаунт Unity (если еще не вошли)

### Шаг 2: Создать новый проект
1. Нажмите **"New Project"**
2. Выберите **"3D Core"** (не 3D, а именно 3D Core)
3. Настройте проект:
   - **Project name:** `Scrap Architect`
   - **Location:** `C:\Users\meduz\Desktop\Scrap Architect\Unity`
   - **Unity version:** `2022.3.62f1`
4. Нажмите **"Create project"**

### Шаг 3: Дождаться создания проекта
- Unity создаст проект и откроет редактор
- Это может занять 5-10 минут

---

## ⚙️ Настройка проекта

### Player Settings
1. **File → Build Settings → Player Settings**
2. **Other Settings:**
   - Company Name: `Scrap Architect Team`
   - Product Name: `Scrap Architect`
   - Version: `0.1.0`
   - Scripting Backend: `Mono` (не IL2CPP для прототипа)

### Physics Settings
1. **Edit → Project Settings → Physics**
2. Настройте:
   - Default Solver Iterations: `6`
   - Default Solver Velocity Iterations: `2`
   - Default Max Angular Speed: `7`
   - Sleep Threshold: `0.005`

### Quality Settings
1. **Edit → Project Settings → Quality**
2. Настройте:
   - Anti Aliasing: `4x Multi Sampling`
   - Texture Quality: `Full Res`
   - Anisotropic Textures: `Per Texture`

---

## 📦 Установка пакетов

### Обязательные пакеты
1. **Window → Package Manager**
2. Установите:
   - **Input System** (для управления)
   - **TextMeshPro** (для UI)
   - **Cinemachine** (для камеры)
   - **Post Processing** (для эффектов)

### Опциональные пакеты
- **ProBuilder** (для создания геометрии)
- **Unity Recorder** (для записи видео)

---

## 📁 Создание структуры папок

После создания проекта создайте следующую структуру в папке `Assets`:

```
Assets/
├── Scripts/
│   ├── Core/
│   ├── Physics/
│   ├── UI/
│   └── Parts/
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

---

## 🔧 Настройка Git

### В Unity Editor
1. **Edit → Project Settings → Editor**
2. Настройте:
   - Version Control Mode: `Visible Meta Files`
   - Asset Serialization Mode: `Force Text`

### Первый коммит
После создания проекта:
1. Откройте терминал в папке проекта
2. Выполните:
   ```bash
   git add .
   git commit -m "Initial Unity project setup for Phase 1"
   git push
   ```

---

## 🎯 Следующие шаги

После создания проекта мы перейдем к:
1. **Создание базовых скриптов** (GameManager, PartController)
2. **Настройка физики** (Rigidbody, Joints)
3. **Создание первых деталей** (кубики, балки)
4. **Прототип системы соединений**

---

## 📞 Поддержка

Если возникнут проблемы:
- [Unity Documentation](https://docs.unity3d.com/)
- [Unity Forums](https://forum.unity.com/)
- [Unity Learn](https://learn.unity.com/)

**Готовы создать проект?** 🚀
