using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Вспомогательный класс для быстрой настройки UI элементов
    /// </summary>
    public static class UIHelper
    {
        /// <summary>
        /// Создать кнопку с настройками по умолчанию
        /// </summary>
        public static Button CreateButton(GameObject parent, string buttonName, string buttonText)
        {
            // Создать GameObject для кнопки
            GameObject buttonGO = new GameObject(buttonName);
            buttonGO.transform.SetParent(parent.transform, false);

            // Добавить компоненты
            RectTransform rectTransform = buttonGO.AddComponent<RectTransform>();
            Image image = buttonGO.AddComponent<Image>();
            Button button = buttonGO.AddComponent<Button>();

            // Настроить изображение
            image.color = UIColors.ButtonNormal;
            image.type = Image.Type.Sliced;

            // Создать текст кнопки
            GameObject textGO = new GameObject("Text");
            textGO.transform.SetParent(buttonGO.transform, false);

            RectTransform textRectTransform = textGO.AddComponent<RectTransform>();
            TextMeshProUGUI textComponent = textGO.AddComponent<TextMeshProUGUI>();

            // Настроить текст
            textComponent.text = buttonText;
            textComponent.color = UIColors.Text;
            textComponent.fontSize = 24;
            textComponent.alignment = TextAlignmentOptions.Center;

            // Настроить RectTransform текста
            textRectTransform.anchorMin = Vector2.zero;
            textRectTransform.anchorMax = Vector2.one;
            textRectTransform.offsetMin = Vector2.zero;
            textRectTransform.offsetMax = Vector2.zero;

            // Настроить RectTransform кнопки
            rectTransform.sizeDelta = new Vector2(200, 50);

            return button;
        }

        /// <summary>
        /// Создать текстовый элемент
        /// </summary>
        public static TextMeshProUGUI CreateText(GameObject parent, string textName, string textContent, int fontSize = 24)
        {
            GameObject textGO = new GameObject(textName);
            textGO.transform.SetParent(parent.transform, false);

            RectTransform rectTransform = textGO.AddComponent<RectTransform>();
            TextMeshProUGUI textComponent = textGO.AddComponent<TextMeshProUGUI>();

            textComponent.text = textContent;
            textComponent.color = UIColors.Text;
            textComponent.fontSize = fontSize;
            textComponent.alignment = TextAlignmentOptions.Center;

            rectTransform.sizeDelta = new Vector2(400, 50);

            return textComponent;
        }

        /// <summary>
        /// Создать панель с фоном
        /// </summary>
        public static GameObject CreatePanel(GameObject parent, string panelName)
        {
            GameObject panelGO = new GameObject(panelName);
            panelGO.transform.SetParent(parent.transform, false);

            RectTransform rectTransform = panelGO.AddComponent<RectTransform>();
            Image image = panelGO.AddComponent<Image>();
            CanvasGroup canvasGroup = panelGO.AddComponent<CanvasGroup>();

            image.color = UIColors.Background;
            image.type = Image.Type.Sliced;

            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            return panelGO;
        }

        /// <summary>
        /// Создать вертикальный Layout Group
        /// </summary>
        public static VerticalLayoutGroup CreateVerticalLayout(GameObject parent, string layoutName)
        {
            GameObject layoutGO = new GameObject(layoutName);
            layoutGO.transform.SetParent(parent.transform, false);

            RectTransform rectTransform = layoutGO.AddComponent<RectTransform>();
            VerticalLayoutGroup layoutGroup = layoutGO.AddComponent<VerticalLayoutGroup>();

            layoutGroup.spacing = 10;
            layoutGroup.padding = new RectOffset(20, 20, 20, 20);
            layoutGroup.childAlignment = TextAnchor.MiddleCenter;
            layoutGroup.childControlWidth = true;
            layoutGroup.childControlHeight = true;
            layoutGroup.childForceExpandWidth = false;
            layoutGroup.childForceExpandHeight = false;

            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            return layoutGroup;
        }

        /// <summary>
        /// Создать горизонтальный Layout Group
        /// </summary>
        public static HorizontalLayoutGroup CreateHorizontalLayout(GameObject parent, string layoutName)
        {
            GameObject layoutGO = new GameObject(layoutName);
            layoutGO.transform.SetParent(parent.transform, false);

            RectTransform rectTransform = layoutGO.AddComponent<RectTransform>();
            HorizontalLayoutGroup layoutGroup = layoutGO.AddComponent<HorizontalLayoutGroup>();

            layoutGroup.spacing = 10;
            layoutGroup.padding = new RectOffset(20, 20, 20, 20);
            layoutGroup.childAlignment = TextAnchor.MiddleCenter;
            layoutGroup.childControlWidth = true;
            layoutGroup.childControlHeight = true;
            layoutGroup.childForceExpandWidth = false;
            layoutGroup.childForceExpandHeight = false;

            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            return layoutGroup;
        }

        /// <summary>
        /// Настроить кнопку с анимацией
        /// </summary>
        public static void SetupButtonAnimation(Button button)
        {
            if (button == null) return;

            // Получить компонент Image
            Image image = button.GetComponent<Image>();
            if (image == null) return;

            // Настроить цвета для разных состояний
            ColorBlock colors = button.colors;
            colors.normalColor = UIColors.ButtonNormal;
            colors.highlightedColor = UIColors.ButtonHover;
            colors.pressedColor = UIColors.ButtonSelected;
            colors.selectedColor = UIColors.ButtonSelected;
            colors.disabledColor = UIColors.ButtonDisabled;
            colors.fadeDuration = 0.1f;

            button.colors = colors;
        }

        /// <summary>
        /// Создать Slider
        /// </summary>
        public static Slider CreateSlider(GameObject parent, string sliderName)
        {
            GameObject sliderGO = new GameObject(sliderName);
            sliderGO.transform.SetParent(parent.transform, false);

            RectTransform rectTransform = sliderGO.AddComponent<RectTransform>();
            Slider slider = sliderGO.AddComponent<Slider>();
            Image backgroundImage = sliderGO.AddComponent<Image>();

            // Настроить фон
            backgroundImage.color = UIColors.Background;
            backgroundImage.type = Image.Type.Sliced;

            // Создать Fill Area
            GameObject fillAreaGO = new GameObject("Fill Area");
            fillAreaGO.transform.SetParent(sliderGO.transform, false);

            RectTransform fillAreaRect = fillAreaGO.AddComponent<RectTransform>();
            fillAreaRect.anchorMin = Vector2.zero;
            fillAreaRect.anchorMax = Vector2.one;
            fillAreaRect.offsetMin = Vector2.zero;
            fillAreaRect.offsetMax = Vector2.zero;

            // Создать Fill
            GameObject fillGO = new GameObject("Fill");
            fillGO.transform.SetParent(fillAreaGO.transform, false);

            RectTransform fillRect = fillGO.AddComponent<RectTransform>();
            Image fillImage = fillGO.AddComponent<Image>();

            fillImage.color = UIColors.Secondary;
            fillImage.type = Image.Type.Sliced;

            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;

            // Настроить Slider
            slider.fillRect = fillRect;
            slider.minValue = 0;
            slider.maxValue = 1;
            slider.value = 0;

            rectTransform.sizeDelta = new Vector2(300, 20);

            return slider;
        }

        /// <summary>
        /// Создать Toggle
        /// </summary>
        public static Toggle CreateToggle(GameObject parent, string toggleName, string toggleText)
        {
            GameObject toggleGO = new GameObject(toggleName);
            toggleGO.transform.SetParent(parent.transform, false);

            RectTransform rectTransform = toggleGO.AddComponent<RectTransform>();
            Toggle toggle = toggleGO.AddComponent<Toggle>();
            Image backgroundImage = toggleGO.AddComponent<Image>();

            backgroundImage.color = UIColors.Background;
            backgroundImage.type = Image.Type.Sliced;

            // Создать Checkmark
            GameObject checkmarkGO = new GameObject("Checkmark");
            checkmarkGO.transform.SetParent(toggleGO.transform, false);

            RectTransform checkmarkRect = checkmarkGO.AddComponent<RectTransform>();
            Image checkmarkImage = checkmarkGO.AddComponent<Image>();

            checkmarkImage.color = UIColors.Success;
            checkmarkImage.type = Image.Type.Sliced;

            checkmarkRect.anchorMin = Vector2.zero;
            checkmarkRect.anchorMax = Vector2.one;
            checkmarkRect.offsetMin = Vector2.zero;
            checkmarkRect.offsetMax = Vector2.zero;

            // Создать Label
            GameObject labelGO = new GameObject("Label");
            labelGO.transform.SetParent(toggleGO.transform, false);

            RectTransform labelRect = labelGO.AddComponent<RectTransform>();
            TextMeshProUGUI labelText = labelGO.AddComponent<TextMeshProUGUI>();

            labelText.text = toggleText;
            labelText.color = UIColors.Text;
            labelText.fontSize = 16;

            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = new Vector2(30, 0);
            labelRect.offsetMax = Vector2.zero;

            // Настроить Toggle
            toggle.graphic = checkmarkImage;
            toggle.targetGraphic = backgroundImage;

            rectTransform.sizeDelta = new Vector2(200, 30);

            return toggle;
        }

        /// <summary>
        /// Применить стандартные настройки к UI элементу
        /// </summary>
        public static void ApplyDefaultSettings(GameObject uiElement)
        {
            if (uiElement == null) return;

            // Настроить RectTransform
            RectTransform rectTransform = uiElement.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;
            }

            // Настроить цвета
            Image image = uiElement.GetComponent<Image>();
            if (image != null)
            {
                image.color = UIColors.Background;
            }

            TextMeshProUGUI text = uiElement.GetComponent<TextMeshProUGUI>();
            if (text != null)
            {
                text.color = UIColors.Text;
            }
        }
    }
}
