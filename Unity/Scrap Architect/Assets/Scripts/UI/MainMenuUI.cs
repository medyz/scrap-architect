using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Главное меню игры
    /// </summary>
    public class MainMenuUI : UIBase
    {
        [Header("Main Menu Elements")]
        public TextMeshProUGUI gameTitleText;
        public TextMeshProUGUI versionText;
        public TextMeshProUGUI subtitleText;
        
        [Header("Main Buttons")]
        public Button playButton;
        public Button worldMapButton;
        public Button settingsButton;
        public Button creditsButton;
        public Button quitButton;
        
        [Header("Additional Elements")]
        public GameObject backgroundImage;
        public GameObject logoImage;
        public GameObject[] decorativeElements;
        
        [Header("Animation")]
        public float buttonStaggerDelay = 0.1f;
        public float titleAnimationDuration = 1f;
        
        private void Start()
        {
            SetupButtons();
            SetupTexts();
            SetupBackground();
            
            // Принудительно обновляем тексты кнопок
            Invoke(nameof(ForceUpdateButtonTexts), 0.1f);
        }
        
        /// <summary>
        /// Принудительное обновление текстов кнопок
        /// </summary>
        private void ForceUpdateButtonTexts()
        {
            Debug.Log("ForceUpdateButtonTexts: Updating button texts...");
            SetupButtonTexts();
        }
        
        /// <summary>
        /// Настройка кнопок
        /// </summary>
        private void SetupButtons()
        {
            Debug.Log("SetupButtons: Setting up button listeners...");
            
            if (playButton != null)
            {
                playButton.onClick.AddListener(OnPlayButtonClick);
                Debug.Log("SetupButtons: Play button listener added");
            }
            else
            {
                Debug.LogWarning("SetupButtons: Play button is null!");
            }
            
            if (worldMapButton != null)
            {
                worldMapButton.onClick.AddListener(OnWorldMapButtonClick);
                Debug.Log("SetupButtons: World Map button listener added");
            }
            else
            {
                Debug.LogWarning("SetupButtons: World Map button is null!");
            }
            
            if (settingsButton != null)
            {
                settingsButton.onClick.AddListener(OnSettingsButtonClick);
                Debug.Log("SetupButtons: Settings button listener added");
            }
            else
            {
                Debug.LogWarning("SetupButtons: Settings button is null!");
            }
            
            if (creditsButton != null)
            {
                creditsButton.onClick.AddListener(OnCreditsButtonClick);
                Debug.Log("SetupButtons: Credits button listener added");
            }
            else
            {
                Debug.LogWarning("SetupButtons: Credits button is null!");
            }
            
            if (quitButton != null)
            {
                quitButton.onClick.AddListener(OnQuitButtonClick);
                Debug.Log("SetupButtons: Quit button listener added");
            }
            else
            {
                Debug.LogWarning("SetupButtons: Quit button is null!");
            }
        }
        
        /// <summary>
        /// Настройка текстов
        /// </summary>
        private void SetupTexts()
        {
            if (gameTitleText != null)
            {
                gameTitleText.text = "SCRAP ARCHITECT";
            }
            
            if (subtitleText != null)
            {
                subtitleText.text = "Создавайте безумные машины из хлама!";
            }
            
            if (versionText != null)
            {
                versionText.text = $"Версия {Application.version}";
            }
            
            // Настройка кнопок для лучшей читаемости
            SetupButtonTexts();
        }
        
        /// <summary>
        /// Настройка текстов кнопок
        /// </summary>
        private void SetupButtonTexts()
        {
            if (playButton != null)
            {
                TextMeshProUGUI playText = playButton.GetComponentInChildren<TextMeshProUGUI>();
                if (playText != null)
                {
                    playText.text = "PLAY"; // Используем английский текст
                    playText.color = Color.black; // Черный текст для лучшего контраста
                    playText.fontSize = 24f;
                    playText.alignment = TextAlignmentOptions.Center;
                }
            }
            
            if (settingsButton != null)
            {
                TextMeshProUGUI settingsText = settingsButton.GetComponentInChildren<TextMeshProUGUI>();
                if (settingsText != null)
                {
                    settingsText.text = "SETTINGS";
                    settingsText.color = Color.black;
                    settingsText.fontSize = 20f;
                    settingsText.alignment = TextAlignmentOptions.Center;
                }
            }
            
            if (creditsButton != null)
            {
                TextMeshProUGUI creditsText = creditsButton.GetComponentInChildren<TextMeshProUGUI>();
                if (creditsText != null)
                {
                    creditsText.text = "ABOUT";
                    creditsText.color = Color.black;
                    creditsText.fontSize = 20f;
                    creditsText.alignment = TextAlignmentOptions.Center;
                }
            }
            
            if (quitButton != null)
            {
                TextMeshProUGUI quitText = quitButton.GetComponentInChildren<TextMeshProUGUI>();
                if (quitText != null)
                {
                    quitText.text = "EXIT";
                    quitText.color = Color.black;
                    quitText.fontSize = 20f;
                    quitText.alignment = TextAlignmentOptions.Center;
                }
            }
        }
        
        /// <summary>
        /// Настройка фонового изображения
        /// </summary>
        private void SetupBackground()
        {
            Debug.Log($"SetupBackground: backgroundImage is {(backgroundImage == null ? "null" : "assigned")}");
            
            if (backgroundImage != null)
            {
                Debug.Log($"SetupBackground: Found backgroundImage: {backgroundImage.name}");
                
                // Получаем RectTransform фонового изображения
                RectTransform backgroundRect = backgroundImage.GetComponent<RectTransform>();
                if (backgroundRect != null)
                {
                    Debug.Log($"SetupBackground: Setting RectTransform for {backgroundImage.name}");
                    
                    // Устанавливаем якоря на растяжение по всему экрану
                    backgroundRect.anchorMin = Vector2.zero;
                    backgroundRect.anchorMax = Vector2.one;
                    backgroundRect.offsetMin = Vector2.zero;
                    backgroundRect.offsetMax = Vector2.zero;
                    
                    // Принудительно устанавливаем размер
                    backgroundRect.sizeDelta = Vector2.zero;
                    
                    // Убеждаемся, что изображение растягивается на весь экран
                    Image backgroundImageComponent = backgroundImage.GetComponent<Image>();
                    if (backgroundImageComponent == null)
                    {
                        // Добавляем Image компонент, если его нет
                        backgroundImageComponent = backgroundImage.AddComponent<Image>();
                        Debug.Log($"SetupBackground: Added Image component to {backgroundImage.name}");
                    }
                    
                    backgroundImageComponent.type = Image.Type.Simple;
                    backgroundImageComponent.preserveAspect = false;
                    backgroundImageComponent.raycastTarget = false; // Отключаем raycast для фона
                    Debug.Log($"SetupBackground: Image component configured - type: {backgroundImageComponent.type}, preserveAspect: {backgroundImageComponent.preserveAspect}");
                    
                    // Перемещаем фон в начало иерархии (под всеми элементами)
                    backgroundImage.transform.SetAsFirstSibling();
                    Debug.Log($"SetupBackground: Moved {backgroundImage.name} to first sibling");
                }
                else
                {
                    Debug.LogError($"SetupBackground: No RectTransform found on {backgroundImage.name}");
                }
            }
            else
            {
                Debug.Log("SetupBackground: backgroundImage is null, skipping background creation");
                // НЕ создаем фон программно - используем существующий дизайн
            }
        }
        
        /// <summary>
        /// Создать фоновое изображение программно (отключено)
        /// </summary>
        private void CreateBackgroundImage()
        {
            Debug.Log("CreateBackgroundImage: Background creation disabled - using existing design");
            // Метод отключен - используем существующий дизайн
        }
        
        /// <summary>
        /// Обработчик кнопки "Играть"
        /// </summary>
        public void OnPlayButtonClick()
        {
            if (uiManager != null)
            {
                uiManager.PlayButtonClickSound();
                uiManager.StartNewGame();
            }
        }
        
        /// <summary>
        /// Обработчик кнопки "Карта мира"
        /// </summary>
        public void OnWorldMapButtonClick()
        {
            if (uiManager != null)
            {
                uiManager.PlayButtonClickSound();
                uiManager.ShowWorldMap();
            }
        }
        
        /// <summary>
        /// Обработчик кнопки "Настройки"
        /// </summary>
        public void OnSettingsButtonClick()
        {
            Debug.Log("OnSettingsButtonClick: Settings button clicked!");
            
            if (uiManager != null)
            {
                Debug.Log("OnSettingsButtonClick: UIManager found, playing sound and showing settings");
                uiManager.PlayButtonClickSound();
                uiManager.ShowSettings();
            }
            else
            {
                Debug.LogError("OnSettingsButtonClick: UIManager is null!");
            }
        }
        
        /// <summary>
        /// Обработчик кнопки "Об игре"
        /// </summary>
        public void OnCreditsButtonClick()
        {
            if (uiManager != null)
            {
                uiManager.PlayButtonClickSound();
                // TODO: Показать экран с информацией об игре
                Debug.Log("Credits button clicked");
            }
        }
        
        /// <summary>
        /// Обработчик кнопки "Выход"
        /// </summary>
        public void OnQuitButtonClick()
        {
            if (uiManager != null)
            {
                uiManager.PlayButtonClickSound();
                uiManager.QuitGame();
            }
        }
        
        /// <summary>
        /// Вызывается при показе главного меню
        /// </summary>
        protected override void OnShow()
        {
            base.OnShow();
            
            Debug.Log($"MainMenuUI OnShow: GameObject {name} is active: {gameObject.activeInHierarchy}");
            Debug.Log($"MainMenuUI OnShow: CanvasGroup alpha: {canvasGroup?.alpha}");
            Debug.Log($"MainMenuUI OnShow: CanvasGroup interactable: {canvasGroup?.interactable}");
            Debug.Log($"MainMenuUI OnShow: CanvasGroup blocksRaycasts: {canvasGroup?.blocksRaycasts}");
            
            // Принудительно делаем панель видимой
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                Debug.Log($"MainMenuUI OnShow: Forced visibility - alpha: {canvasGroup.alpha}");
            }
            
            // Принудительно настраиваем фон при каждом показе
            SetupBackground();
            
            // Запускаем анимации только если GameObject активен
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(AnimateMainMenu());
            }
            else
            {
                Debug.LogWarning($"MainMenuUI OnShow: GameObject {name} is not active, skipping animations");
            }
        }
        
        /// <summary>
        /// Анимация главного меню
        /// </summary>
        private IEnumerator AnimateMainMenu()
        {
            // Анимация заголовка
            if (gameTitleText != null)
            {
                yield return StartCoroutine(AnimateTitle());
            }
            
            // Анимация кнопок с задержкой
            yield return StartCoroutine(AnimateButtons());
            
            // Анимация декоративных элементов
            yield return StartCoroutine(AnimateDecorations());
        }
        
        /// <summary>
        /// Анимация заголовка
        /// </summary>
        private IEnumerator AnimateTitle()
        {
            if (gameTitleText == null) yield break;
            
            Color originalColor = gameTitleText.color;
            Vector3 originalScale = gameTitleText.transform.localScale;
            
            // Начальное состояние
            gameTitleText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
            gameTitleText.transform.localScale = originalScale * 0.5f;
            
            float elapsedTime = 0f;
            
            while (elapsedTime < titleAnimationDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                float progress = elapsedTime / titleAnimationDuration;
                float curveValue = showAnimationCurve.Evaluate(progress);
                
                gameTitleText.color = new Color(originalColor.r, originalColor.g, originalColor.b, curveValue);
                gameTitleText.transform.localScale = Vector3.Lerp(originalScale * 0.5f, originalScale, curveValue);
                
                yield return null;
            }
            
            gameTitleText.color = originalColor;
            gameTitleText.transform.localScale = originalScale;
        }
        
        /// <summary>
        /// Анимация кнопок
        /// </summary>
        private IEnumerator AnimateButtons()
        {
            Button[] buttons = { playButton, settingsButton, creditsButton, quitButton };
            
            foreach (Button button in buttons)
            {
                if (button != null)
                {
                    yield return StartCoroutine(AnimateButton(button));
                    yield return new WaitForSecondsRealtime(buttonStaggerDelay);
                }
            }
        }
        
        /// <summary>
        /// Анимация кнопки
        /// </summary>
        private IEnumerator AnimateButton(Button button)
        {
            if (button == null) yield break;
            
            RectTransform buttonRect = button.GetComponent<RectTransform>();
            if (buttonRect == null) yield break;
            
            Vector3 originalPosition = buttonRect.anchoredPosition;
            Vector3 startPosition = originalPosition + Vector3.right * 100f;
            
            // Начальное состояние
            buttonRect.anchoredPosition = startPosition;
            button.interactable = false;
            
            float elapsedTime = 0f;
            float animationDuration = 0.5f;
            
            while (elapsedTime < animationDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                float progress = elapsedTime / animationDuration;
                float curveValue = showAnimationCurve.Evaluate(progress);
                
                buttonRect.anchoredPosition = Vector3.Lerp(startPosition, originalPosition, curveValue);
                
                yield return null;
            }
            
            buttonRect.anchoredPosition = originalPosition;
            button.interactable = true;
        }
        
        /// <summary>
        /// Анимация декоративных элементов
        /// </summary>
        private IEnumerator AnimateDecorations()
        {
            if (decorativeElements == null) yield break;
            
            foreach (GameObject element in decorativeElements)
            {
                if (element != null)
                {
                    yield return StartCoroutine(AnimateDecoration(element));
                }
            }
        }
        
        /// <summary>
        /// Анимация декоративного элемента
        /// </summary>
        private IEnumerator AnimateDecoration(GameObject element)
        {
            if (element == null) yield break;
            
            CanvasGroup elementCanvasGroup = element.GetComponent<CanvasGroup>();
            if (elementCanvasGroup == null)
            {
                elementCanvasGroup = element.AddComponent<CanvasGroup>();
            }
            
            // Начальное состояние
            elementCanvasGroup.alpha = 0f;
            
            float elapsedTime = 0f;
            float animationDuration = 0.8f;
            
            while (elapsedTime < animationDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                float progress = elapsedTime / animationDuration;
                float curveValue = showAnimationCurve.Evaluate(progress);
                
                elementCanvasGroup.alpha = curveValue;
                
                yield return null;
            }
            
            elementCanvasGroup.alpha = 1f;
        }
        
        /// <summary>
        /// Обработка нажатия клавиши Escape
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnQuitButtonClick();
            }
        }
    }
}
