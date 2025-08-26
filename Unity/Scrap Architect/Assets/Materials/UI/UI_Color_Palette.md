# Цветовая палитра UI - Scrap Architect

## Основные цвета

### Primary (Основной)
- **Hex**: #2C3E50
- **RGB**: 44, 62, 80
- **Использование**: Основной цвет интерфейса, фоны панелей

### Secondary (Вторичный)
- **Hex**: #3498DB
- **RGB**: 52, 152, 219
- **Использование**: Акцентные элементы, кнопки, ссылки

### Accent (Акцент)
- **Hex**: #E74C3C
- **RGB**: 231, 76, 60
- **Использование**: Важные действия, предупреждения, ошибки

### Success (Успех)
- **Hex**: #27AE60
- **RGB**: 39, 174, 96
- **Использование**: Успешные действия, завершенные задачи

### Warning (Предупреждение)
- **Hex**: #F39C12
- **RGB**: 243, 156, 18
- **Использование**: Предупреждения, внимание

### Background (Фон)
- **Hex**: #34495E
- **RGB**: 52, 73, 94
- **Использование**: Основной фон приложения

### Text (Текст)
- **Hex**: #ECF0F1
- **RGB**: 236, 240, 241
- **Использование**: Основной цвет текста

## Дополнительные цвета

### Text Secondary (Вторичный текст)
- **Hex**: #BDC3C7
- **RGB**: 189, 195, 199
- **Использование**: Подписи, дополнительная информация

### Border (Границы)
- **Hex**: #7F8C8D
- **RGB**: 127, 140, 141
- **Использование**: Границы элементов, разделители

### Overlay (Наложение)
- **Hex**: #2C3E50 (с Alpha 0.8)
- **RGB**: 44, 62, 80, 204
- **Использование**: Затемнение, модальные окна

### Highlight (Подсветка)
- **Hex**: #5DADE2
- **RGB**: 93, 173, 226
- **Использование**: Hover эффекты, выделение

## Состояния кнопок

### Normal (Обычное)
- **Background**: #2C3E50
- **Text**: #ECF0F1
- **Border**: #7F8C8D

### Hover (Наведение)
- **Background**: #34495E
- **Text**: #ECF0F1
- **Border**: #5DADE2

### Selected (Выбрано)
- **Background**: #3498DB
- **Text**: #ECF0F1
- **Border**: #5DADE2

### Disabled (Отключено)
- **Background**: #7F8C8D
- **Text**: #BDC3C7
- **Border**: #95A5A6

## Состояния прогресса

### Incomplete (Не завершено)
- **Background**: #34495E
- **Fill**: #7F8C8D

### In Progress (В процессе)
- **Background**: #34495E
- **Fill**: #F39C12

### Complete (Завершено)
- **Background**: #34495E
- **Fill**: #27AE60

### Failed (Провалено)
- **Background**: #34495E
- **Fill**: #E74C3C

## Градиенты

### Primary Gradient
- **Start**: #2C3E50
- **End**: #34495E
- **Использование**: Фоны панелей

### Button Gradient
- **Start**: #3498DB
- **End**: #2980B9
- **Использование**: Кнопки

### Success Gradient
- **Start**: #27AE60
- **End**: #229954
- **Использование**: Успешные элементы

### Warning Gradient
- **Start**: #F39C12
- **End**: #E67E22
- **Использование**: Предупреждения

## Тени

### Light Shadow
- **Color**: #2C3E50 (Alpha 0.1)
- **Offset**: 2, 2
- **Blur**: 4

### Medium Shadow
- **Color**: #2C3E50 (Alpha 0.2)
- **Offset**: 4, 4
- **Blur**: 8

### Heavy Shadow
- **Color**: #2C3E50 (Alpha 0.3)
- **Offset**: 8, 8
- **Blur**: 16

## Применение в Unity

### Создание материалов
1. Создать материал в Materials/UI/
2. Установить Shader: UI/Default
3. Настроить цвет в соответствии с палитрой

### Настройка в скриптах
```csharp
// Пример использования цветов в коде
public static class UIColors
{
    public static Color Primary = new Color(0.173f, 0.243f, 0.314f);
    public static Color Secondary = new Color(0.204f, 0.596f, 0.859f);
    public static Color Accent = new Color(0.906f, 0.298f, 0.235f);
    public static Color Success = new Color(0.153f, 0.682f, 0.376f);
    public static Color Warning = new Color(0.953f, 0.612f, 0.071f);
    public static Color Background = new Color(0.204f, 0.286f, 0.369f);
    public static Color Text = new Color(0.925f, 0.941f, 0.945f);
}
```

### Настройка в Inspector
- Использовать Color Picker для точной настройки
- Сохранять цвета как Presets для переиспользования
- Создать ScriptableObject с цветовой палитрой
