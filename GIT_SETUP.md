# Настройка Git репозитория для Scrap Architect

## 🚀 Быстрый старт

### 1. Инициализация репозитория

```bash
# Инициализация Git в папке проекта
git init

# Добавление всех файлов в индекс
git add .

# Первый коммит
git commit -m "Initial commit: Project setup and documentation"

# Добавление удаленного репозитория (замените URL на ваш)
git remote add origin https://github.com/yourusername/scrap-architect.git

# Отправка в удаленный репозиторий
git push -u origin main
```

### 2. Настройка веток

```bash
# Создание основной ветки разработки
git checkout -b develop

# Создание ветки для текущей задачи
git checkout -b feature/physics-system

# Возврат в основную ветку
git checkout main
```

## 📋 Структура веток

### Основные ветки:
- **`main`** - стабильная версия, готовая к релизу
- **`develop`** - основная ветка разработки
- **`feature/*`** - ветки для новых функций
- **`bugfix/*`** - ветки для исправления багов
- **`hotfix/*`** - срочные исправления для main

### Примеры названий веток:
```
feature/physics-system
feature/building-ui
feature/parts-catalog
bugfix/joint-breaking
hotfix/critical-crash
```

## 🔧 Настройка Git

### 1. Конфигурация пользователя

```bash
# Настройка имени и email
git config --global user.name "Your Name"
git config --global user.email "your.email@example.com"

# Настройка редактора (опционально)
git config --global core.editor "code --wait"
```

### 2. Настройка .gitignore

Файл `.gitignore` уже создан и настроен для Unity проекта. Он исключает:
- Временные файлы Unity
- Скомпилированные файлы
- Настройки IDE
- Логи и кэш

### 3. Настройка Git Hooks (опционально)

```bash
# Создание папки для hooks
mkdir .git/hooks

# Создание pre-commit hook для проверки кода
cat > .git/hooks/pre-commit << 'EOF'
#!/bin/sh
# Проверка синтаксиса C# файлов
find Assets/Scripts -name "*.cs" -exec csc -t:library {} \;
EOF

# Делаем hook исполняемым
chmod +x .git/hooks/pre-commit
```

## 📝 Правила коммитов

### Формат сообщений коммитов:
```
<тип>(<область>): <краткое описание>

<подробное описание>

<ссылки на задачи>
```

### Типы коммитов:
- **`feat`** - новая функция
- **`fix`** - исправление бага
- **`docs`** - изменения в документации
- **`style`** - форматирование кода
- **`refactor`** - рефакторинг кода
- **`test`** - добавление тестов
- **`chore`** - обновление зависимостей

### Примеры коммитов:
```bash
git commit -m "feat(physics): Add joint breaking system

- Implement joint stress calculation
- Add visual feedback for broken joints
- Add sound effects for joint failure

Closes #123"
```

```bash
git commit -m "fix(building): Fix snap point alignment

- Correct snap point positioning
- Improve snap detection accuracy
- Fix visual indicators

Fixes #456"
```

## 🔄 Рабочий процесс

### 1. Создание новой функции

```bash
# Переключение на develop
git checkout develop

# Обновление develop
git pull origin develop

# Создание ветки для функции
git checkout -b feature/new-feature

# Разработка...
# Коммиты...

# Переключение обратно на develop
git checkout develop

# Слияние функции
git merge feature/new-feature

# Удаление ветки функции
git branch -d feature/new-feature
```

### 2. Исправление бага

```bash
# Создание ветки для багфикса
git checkout -b bugfix/critical-bug

# Исправление...
# Коммиты...

# Слияние в develop
git checkout develop
git merge bugfix/critical-bug

# Если критический баг - слияние в main
git checkout main
git merge bugfix/critical-bug
git tag -a v1.0.1 -m "Hotfix for critical bug"
```

### 3. Релиз

```bash
# Слияние develop в main
git checkout main
git merge develop

# Создание тега релиза
git tag -a v1.0.0 -m "Release version 1.0.0"

# Отправка тега
git push origin v1.0.0
```

## 🛠 Полезные команды

### Просмотр состояния:
```bash
# Статус репозитория
git status

# История коммитов
git log --oneline --graph

# Различия в файлах
git diff
```

### Управление ветками:
```bash
# Список всех веток
git branch -a

# Удаление ветки
git branch -d feature/old-feature

# Переименование ветки
git branch -m old-name new-name
```

### Отмена изменений:
```bash
# Отмена изменений в файле
git checkout -- filename

# Отмена последнего коммита
git reset --soft HEAD~1

# Отмена слияния
git merge --abort
```

### Стешинг:
```bash
# Сохранение изменений во временной области
git stash

# Просмотр сохраненных изменений
git stash list

# Применение последнего стеша
git stash pop

# Удаление стеша
git stash drop
```

## 🔗 Интеграция с GitHub

### 1. Создание репозитория на GitHub

1. Перейдите на [GitHub](https://github.com)
2. Нажмите "New repository"
3. Назовите репозиторий `scrap-architect`
4. Добавьте описание
5. Выберите лицензию MIT
6. Не инициализируйте с README (у нас уже есть)

### 2. Настройка GitHub Actions (опционально)

Создайте файл `.github/workflows/unity-build.yml`:

```yaml
name: Unity Build

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main ]

jobs:
  build:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v2
    
    - name: Cache Library
      uses: actions/cache@v2
      with:
        path: Library
        key: Library-${{ hashFiles('Assets/**', 'Packages/**', 'ProjectSettings/**') }}
        restore-keys: |
          Library-
    
    - name: Build Unity Project
      uses: game-ci/unity-builder@v2
      env:
        UNITY_LICENSE: ${{ secrets.UNITY_LICENSE }}
      with:
        targetPlatform: StandaloneWindows64
```

### 3. Настройка Issues и Projects

1. **Issues** - для отслеживания багов и задач
2. **Projects** - для управления проектом
3. **Milestones** - для группировки задач по версиям
4. **Labels** - для категоризации задач

### 4. Настройка защиты веток

1. Перейдите в Settings > Branches
2. Добавьте правило для `main`:
   - Require pull request reviews
   - Require status checks to pass
   - Include administrators
3. Добавьте правило для `develop`:
   - Require pull request reviews
   - Include administrators

## 📊 Мониторинг и аналитика

### 1. GitHub Insights

- **Contributors** - активность участников
- **Traffic** - просмотры и клоны
- **Commits** - график коммитов
- **Code frequency** - изменения в коде

### 2. Интеграция с внешними сервисами

- **Discord** - уведомления о коммитах
- **Slack** - интеграция с командой
- **Email** - уведомления о релизах

## 🚨 Безопасность

### 1. Защита чувствительных данных

```bash
# Добавление файлов с секретами в .gitignore
echo "config/secrets.json" >> .gitignore
echo "*.key" >> .gitignore
echo "*.pem" >> .gitignore
```

### 2. Проверка безопасности

```bash
# Проверка на утечки секретов
git log --all --full-history -- "**/password*" "**/secret*" "**/key*"
```

## 📚 Дополнительные ресурсы

### Документация:
- [Git Documentation](https://git-scm.com/doc)
- [GitHub Guides](https://guides.github.com/)
- [Unity Git Workflow](https://unity.com/how-to/version-control)

### Инструменты:
- [GitKraken](https://www.gitkraken.com/) - GUI для Git
- [SourceTree](https://www.sourcetreeapp.com/) - бесплатный GUI
- [GitHub Desktop](https://desktop.github.com/) - официальный GUI

### Плагины для Unity:
- [Git for Unity](https://github.com/github-for-unity/Unity) - интеграция с GitHub
- [Git LFS](https://git-lfs.github.com/) - для больших файлов

---

**Готово!** Теперь у вас есть полностью настроенный Git репозиторий для проекта Scrap Architect. 🎉
