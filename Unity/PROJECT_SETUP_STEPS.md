# Пошаговая настройка Unity проекта - Scrap Architect

## 🎯 Этап 1: Player Settings

### Шаг 1.1: Открыть Player Settings
1. В Unity Editor нажмите **File → Build Settings**
2. В открывшемся окне нажмите **Player Settings** (кнопка внизу слева)

### Шаг 1.2: Настроить Other Settings
В разделе **Other Settings** найдите и измените:

```
Company Name: Scrap Architect Team
Product Name: Scrap Architect  
Version: 0.1.0
Scripting Backend: Mono (важно для прототипа!)
```

### Шаг 1.3: Настроить Resolution and Presentation
```
Default Screen Width: 1920
Default Screen Height: 1080
Run In Background: ✓ (отмечено)
```

---

## 🎯 Этап 2: Physics Settings

### Шаг 2.1: Открыть Physics Settings
1. **Edit → Project Settings → Physics**

### Шаг 2.2: Настроить параметры
Найдите и измените следующие значения:

```
Default Solver Iterations: 6
Default Solver Velocity Iterations: 2
Default Max Angular Speed: 7
Sleep Threshold: 0.005
Default Contact Offset: 0.01
```

### Шаг 2.3: Настроить Layer Collision Matrix
В разделе **Layer Collision Matrix** убедитесь, что:
- Default с Default: ✓
- Default с UI: ✗ (не отмечено)

---

## 🎯 Этап 3: Quality Settings

### Шаг 3.1: Открыть Quality Settings
1. **Edit → Project Settings → Quality**

### Шаг 3.2: Настроить параметры
Для уровня **Medium** (или создайте новый):

```
Anti Aliasing: 4x Multi Sampling
Texture Quality: Full Res
Anisotropic Textures: Per Texture
Soft Particles: ✓
Realtime Reflection Probes: ✓
Billboards Face Camera Position: ✓
```

---

## 🎯 Этап 4: Установка пакетов

### Шаг 4.1: Открыть Package Manager
1. **Window → Package Manager**

### Шаг 4.2: Установить Input System
1. В поиске введите "Input System"
2. Нажмите **Install** (или **Update** если уже установлен)
3. При запросе перезапуска Unity нажмите **Yes**

### Шаг 4.3: Установить TextMeshPro
1. В поиске введите "TextMeshPro"
2. Нажмите **Install**
3. При запросе импорта TMP Essentials нажмите **Import TMP Essentials**

### Шаг 4.4: Установить Cinemachine
1. В поиске введите "Cinemachine"
2. Нажмите **Install**

### Шаг 4.5: Установить Post Processing
1. В поиске введите "Post Processing"
2. Нажмите **Install**

---

## 🎯 Этап 5: Создание структуры папок

### Шаг 5.1: Создать основные папки
В **Project** панели (внизу слева):
1. Правый клик на **Assets** → **Create → Folder**
2. Создайте следующие папки:

```
Scripts/
Prefabs/
Materials/
Textures/
Models/
Audio/
Scenes/
Resources/
```

### Шаг 5.2: Создать подпапки
В каждой папке создайте подпапки:

**Scripts:**
- Core/
- Physics/
- UI/
- Parts/

**Prefabs:**
- Parts/
- UI/
- Effects/

**Materials:**
- Parts/
- UI/
- Effects/

**Textures:**
- Parts/
- UI/
- Backgrounds/

**Models:**
- Parts/
- Environment/
- Props/

**Audio:**
- SFX/
- Music/
- UI/

**Scenes:**
- (оставьте пустой, сцены создадим позже)

**Resources:**
- Configs/
- Localization/

---

## 🎯 Этап 6: Настройка Git

### Шаг 6.1: Настроить Editor Settings
1. **Edit → Project Settings → Editor**
2. Измените:
```
Version Control Mode: Visible Meta Files
Asset Serialization Mode: Force Text
```

### Шаг 6.2: Создать .gitignore
В корне проекта создайте файл `.gitignore` с содержимым:

```
# Unity generated
[Ll]ibrary/
[Tt]emp/
[Oo]bj/
[Bb]uild/
[Bb]uilds/
[Ll]ogs/
[Uu]ser[Ss]ettings/

# VS/Rider/MD/Consulo generated
.vs/
.idea/
ExportedObj/
.consulo/
*.csproj
*.unityproj
*.sln
*.suo
*.tmp
*.user
*.userprefs
*.pidb
*.booproj
*.svd
*.pdb
*.mdb
*.opendb
*.VC.db

# Unity3D generated meta files
*.pidb.meta
*.pdb.meta
*.mdb.meta

# Unity3D generated file on crash reports
sysinfo.txt

# Builds
*.apk
*.aab
*.unitypackage
*.app

# Crashlytics generated file
crashlytics-build.properties

# OS generated
.DS_Store
.DS_Store?
._*
.Spotlight-V100
.Trashes
ehthumbs.db
Thumbs.db
```

---

## 🎯 Этап 7: Создание базовых сцен

### Шаг 7.1: Создать сцены
1. В папке **Scenes** правый клик → **Create → Scene**
2. Создайте:
- MainMenu
- BuildMode  
- TestMode

### Шаг 7.2: Сохранить текущую сцену
1. **File → Save As**
2. Сохраните как "SampleScene" в папке Scenes

---

## 🎯 Этап 8: Первый коммит

### Шаг 8.1: Открыть терминал
1. В Unity: **Window → General → Console**
2. Или откройте PowerShell в папке проекта

### Шаг 8.2: Выполнить команды
```bash
git add .
git commit -m "Initial Unity project setup for Phase 1"
git push
```

---

## ✅ Проверка настройки

После завершения всех этапов проверьте:

- [ ] Player Settings настроены
- [ ] Physics Settings оптимизированы
- [ ] Quality Settings установлены
- [ ] Все пакеты установлены
- [ ] Структура папок создана
- [ ] Git настроен
- [ ] Базовые сцены созданы
- [ ] Первый коммит выполнен

---

## 🚀 Готово к разработке!

После завершения настройки мы сможем:
1. Создать первые скрипты
2. Настроить физику
3. Создать базовые детали
4. Прототипировать систему соединений

**Готовы начать настройку?** 🛠️
