# Git Setup для Scrap Architect

## 🎯 Текущий статус проекта

✅ **Проект полностью готов!**  
**Версия:** 0.1.5 (MVP)  
**Статус:** MVP завершен, готов к дальнейшей разработке

### Что уже реализовано:
- ✅ **35+ деталей** с полной физикой
- ✅ **7 разнообразных уровней** 
- ✅ **Полная система контрактов**
- ✅ **Экономическая система** (валюта "Скрап")
- ✅ **Система прогрессии** и достижений
- ✅ **Полный UI/UX** интерфейс
- ✅ **Система строительства** (drag & drop)
- ✅ **Тестовый полигон** для испытания машин
- ✅ **Система сохранения** и загрузки

---

## 🚀 Работа с Git репозиторием

### 1. Клонирование репозитория

```bash
# Клонирование проекта
git clone https://github.com/medyz/scrap-architect.git
cd scrap-architect

# Проверка статуса
git status
```

### 2. Настройка веток

```bash
# Создание основной ветки разработки
git checkout -b develop

# Создание ветки для новой функции
git checkout -b feature/new-feature

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
feature/steam-integration
feature/additional-parts
feature/workshop-system
bugfix/physics-optimization
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
- Временные файлы Unity (Library/, Temp/, Logs/)
- Скомпилированные файлы (.meta файлы)
- Настройки IDE (.vs/, .vscode/)
- Логи и кэш
- Build файлы

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
- **feat:** новая функция
- **fix:** исправление бага
- **docs:** изменения в документации
- **style:** форматирование кода
- **refactor:** рефакторинг кода
- **test:** добавление тестов
- **chore:** обновление зависимостей

### Примеры коммитов:
```bash
git commit -m "feat(parts): add new jet engine part

- Added JetEngine.cs with realistic physics
- Implemented thrust system
- Added fuel consumption mechanics
- Updated part catalog

Closes #123"
```

## 🔄 Рабочий процесс

### 1. Создание новой функции

```bash
# Создание ветки для новой функции
git checkout -b feature/steam-workshop

# Разработка функции
# ... работа с кодом ...

# Добавление изменений
git add .

# Коммит изменений
git commit -m "feat(workshop): implement Steam Workshop integration"

# Отправка в удаленный репозиторий
git push origin feature/steam-workshop
```

### 2. Слияние изменений

```bash
# Переключение на develop
git checkout develop

# Получение последних изменений
git pull origin develop

# Слияние feature ветки
git merge feature/steam-workshop

# Удаление feature ветки
git branch -d feature/steam-workshop
```

### 3. Создание релиза

```bash
# Переключение на main
git checkout main

# Слияние develop в main
git merge develop

# Создание тега для релиза
git tag -a v0.2.0 -m "Release version 0.2.0"

# Отправка тега
git push origin v0.2.0
```

## 📊 Отслеживание изменений

### 1. Просмотр истории

```bash
# Просмотр истории коммитов
git log --oneline

# Просмотр изменений в файле
git log -p Assets/Scripts/Parts/JetEngine.cs

# Просмотр статистики
git log --stat
```

### 2. Сравнение веток

```bash
# Сравнение с предыдущим коммитом
git diff HEAD~1

# Сравнение веток
git diff main..develop

# Сравнение конкретного файла
git diff main..develop Assets/Scripts/Parts/JetEngine.cs
```

### 3. Отмена изменений

```bash
# Отмена последнего коммита
git reset --soft HEAD~1

# Отмена изменений в файле
git checkout -- Assets/Scripts/Parts/JetEngine.cs

# Отмена всех изменений
git reset --hard HEAD
```

## 🚀 Автоматизация

### 1. GitHub Actions (рекомендуется)

Создайте файл `.github/workflows/ci.yml`:

```yaml
name: CI

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
    
    - name: Setup Unity
      uses: game-ci/setup-unity@v1
      with:
        unity-version: 2022.3.15f1
    
    - name: Build
      uses: game-ci/unity-builder@v2
      with:
        targetPlatform: StandaloneWindows64
```

### 2. Автоматические теги

```bash
# Создание скрипта для автоматических тегов
cat > scripts/create-release.sh << 'EOF'
#!/bin/bash
VERSION=$1
git checkout main
git merge develop
git tag -a v$VERSION -m "Release version $VERSION"
git push origin v$VERSION
echo "Release v$VERSION created successfully!"
EOF

chmod +x scripts/create-release.sh
```

## 📞 Поддержка

### Полезные команды:

```bash
# Просмотр статуса
git status

# Просмотр веток
git branch -a

# Просмотр удаленных репозиториев
git remote -v

# Очистка локальных веток
git branch --merged | grep -v "\*" | xargs -n 1 git branch -d
```

### Документация:
- [Git Documentation](https://git-scm.com/doc)
- [GitHub Guides](https://guides.github.com/)
- [Conventional Commits](https://www.conventionalcommits.org/)

### Сообщить о проблеме:
- Создайте Issue в GitHub
- Опишите проблему подробно
- Приложите скриншоты или логи
- Укажите версию Git и ОС

---

## 🎉 Что дальше?

### Для разработчиков:
1. **Изучите существующий код** в ветке main
2. **Создайте feature ветку** для новой функции
3. **Следуйте правилам коммитов** при разработке
4. **Создайте Pull Request** для слияния изменений

### Следующие этапы:
- **Фаза 2:** Альфа-версия (расширение контента)
- **Фаза 3:** Steam Early Access (интеграция со Steam)
- **Фаза 4:** Полный релиз (маркетинг и поддержка)

---

**Проект готов к разработке! Удачи в создании новых функций! 🚀**
