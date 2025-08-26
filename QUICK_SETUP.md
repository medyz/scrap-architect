# Быстрая установка оставшихся компонентов

## 🎯 Текущий статус

✅ **Уже установлено:**
- Unity Hub: `C:\Program Files\Unity Hub\Unity Hub.exe`
- Visual Studio 2022 Community: `C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe`
- Git: версия 2.50.1.windows.1

❌ **Требует установки:**
- Unity Editor 2022.3.62f1 LTS
- .NET SDK

---

## 🚀 Быстрая установка

### 1. Установка Unity Editor

1. **Запустите Unity Hub**
2. **Перейдите в раздел "Installs"**
3. **Нажмите "Install Editor"**
4. **Выберите версию:** Unity 2022.3.62f1 LTS (рекомендуется)
5. **Выберите модули:**
   - ✅ Microsoft Visual Studio Community 2022
   - ✅ Windows Build Support
   - ✅ Android Build Support (опционально)

**Время установки:** ~30-60 минут

### 2. Установка Visual Studio 2022

✅ **Уже установлено!** Visual Studio 2022 Community найден в системе.

Если нужно переустановить или добавить компоненты:
1. **Скачайте Visual Studio 2022 Community:**
   - Перейдите на [visualstudio.microsoft.com](https://visualstudio.microsoft.com/)
   - Нажмите "Download Visual Studio"
   - Выберите "Community 2022"

2. **Установите с компонентами:**
   - ✅ .NET desktop development
   - ✅ Game development with Unity
   - ✅ C++ core features

**Время установки:** ~20-40 минут

### 3. Установка .NET SDK

1. **Скачайте .NET SDK:**
   - Перейдите на [dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
   - Скачайте .NET 6.0 SDK или новее

2. **Установите скачанный файл**

**Время установки:** ~5-10 минут

---

## 🔍 Проверка установки

После установки всех компонентов запустите:

```powershell
.\check_unity_installation.ps1
```

Должны увидеть:
- ✅ Unity Hub найден
- ✅ Unity Editor найден
- ✅ Visual Studio найден
- ✅ Git найден
- ✅ .NET SDK найден

---

## 🎮 Создание Unity проекта

После полной установки:

1. **В Unity Hub нажмите "New Project"**
2. **Выберите "3D Core"**
3. **Название:** `Scrap Architect`
4. **Расположение:** `C:\Users\meduz\Desktop\Scrap Architect\Unity`
5. **Unity версия:** 2022.3.62f1

---

## 📞 Поддержка

Если возникнут проблемы:
- **Unity:** [unity.com/support](https://unity.com/support)
- **Visual Studio:** [visualstudio.microsoft.com/support](https://visualstudio.microsoft.com/support)
- **.NET:** [dotnet.microsoft.com/support](https://dotnet.microsoft.com/support)

---

## ⏱️ Общее время установки

- Unity Editor: 30-60 минут
- Visual Studio: 20-40 минут
- .NET SDK: 5-10 минут
- **Итого:** ~1-2 часа
