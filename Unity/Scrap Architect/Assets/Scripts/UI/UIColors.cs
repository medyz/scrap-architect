using UnityEngine;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Статический класс для работы с цветами UI
    /// </summary>
    public static class UIColors
    {
        // Основные цвета
        public static Color Primary = new Color(0.173f, 0.243f, 0.314f);        // #2C3E50
        public static Color Secondary = new Color(0.204f, 0.596f, 0.859f);      // #3498DB
        public static Color Accent = new Color(0.906f, 0.298f, 0.235f);         // #E74C3C
        public static Color Success = new Color(0.153f, 0.682f, 0.376f);        // #27AE60
        public static Color Warning = new Color(0.953f, 0.612f, 0.071f);        // #F39C12
        public static Color Background = new Color(0.204f, 0.286f, 0.369f);     // #34495E
        public static Color Text = new Color(0.925f, 0.941f, 0.945f);           // #ECF0F1

        // Дополнительные цвета
        public static Color TextSecondary = new Color(0.741f, 0.765f, 0.780f);  // #BDC3C7
        public static Color Border = new Color(0.498f, 0.549f, 0.553f);         // #7F8C8D
        public static Color Overlay = new Color(0.173f, 0.243f, 0.314f, 0.8f);  // #2C3E50 с Alpha 0.8
        public static Color Highlight = new Color(0.365f, 0.678f, 0.886f);      // #5DADE2

        // Состояния кнопок
        public static Color ButtonNormal = Primary;
        public static Color ButtonHover = Background;
        public static Color ButtonSelected = Secondary;
        public static Color ButtonDisabled = new Color(0.498f, 0.549f, 0.553f); // #7F8C8D

        // Состояния прогресса
        public static Color ProgressIncomplete = Border;
        public static Color ProgressInProgress = Warning;
        public static Color ProgressComplete = Success;
        public static Color ProgressFailed = Accent;

        /// <summary>
        /// Получить цвет по имени
        /// </summary>
        public static Color GetColor(string colorName)
        {
            switch (colorName.ToLower())
            {
                case "primary": return Primary;
                case "secondary": return Secondary;
                case "accent": return Accent;
                case "success": return Success;
                case "warning": return Warning;
                case "background": return Background;
                case "text": return Text;
                case "textsecondary": return TextSecondary;
                case "border": return Border;
                case "overlay": return Overlay;
                case "highlight": return Highlight;
                default: return Text;
            }
        }

        /// <summary>
        /// Получить цвет кнопки по состоянию
        /// </summary>
        public static Color GetButtonColor(ButtonState state)
        {
            switch (state)
            {
                case ButtonState.Normal: return ButtonNormal;
                case ButtonState.Hover: return ButtonHover;
                case ButtonState.Selected: return ButtonSelected;
                case ButtonState.Disabled: return ButtonDisabled;
                default: return ButtonNormal;
            }
        }

        /// <summary>
        /// Получить цвет прогресса по состоянию
        /// </summary>
        public static Color GetProgressColor(ProgressState state)
        {
            switch (state)
            {
                case ProgressState.Incomplete: return ProgressIncomplete;
                case ProgressState.InProgress: return ProgressInProgress;
                case ProgressState.Complete: return ProgressComplete;
                case ProgressState.Failed: return ProgressFailed;
                default: return ProgressIncomplete;
            }
        }

        /// <summary>
        /// Применить цвет к UI элементу
        /// </summary>
        public static void ApplyColor(GameObject uiElement, Color color)
        {
            if (uiElement == null) return;

            // Попробовать применить к Image
            var image = uiElement.GetComponent<UnityEngine.UI.Image>();
            if (image != null)
            {
                image.color = color;
                return;
            }

            // Попробовать применить к TextMeshProUGUI
            var tmpText = uiElement.GetComponent<TMPro.TextMeshProUGUI>();
            if (tmpText != null)
            {
                tmpText.color = color;
                return;
            }

            // Попробовать применить к Text
            var text = uiElement.GetComponent<UnityEngine.UI.Text>();
            if (text != null)
            {
                text.color = color;
                return;
            }
        }

        /// <summary>
        /// Применить цвет к кнопке
        /// </summary>
        public static void ApplyButtonColor(GameObject button, ButtonState state)
        {
            ApplyColor(button, GetButtonColor(state));
        }

        /// <summary>
        /// Применить цвет к прогрессу
        /// </summary>
        public static void ApplyProgressColor(GameObject progressElement, ProgressState state)
        {
            ApplyColor(progressElement, GetProgressColor(state));
        }
    }

    /// <summary>
    /// Состояния кнопки
    /// </summary>
    public enum ButtonState
    {
        Normal,
        Hover,
        Selected,
        Disabled
    }

    /// <summary>
    /// Состояния прогресса
    /// </summary>
    public enum ProgressState
    {
        Incomplete,
        InProgress,
        Complete,
        Failed
    }
}
