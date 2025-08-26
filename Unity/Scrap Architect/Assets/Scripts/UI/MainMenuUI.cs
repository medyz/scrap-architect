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
        }
        
        /// <summary>
        /// Настройка кнопок
        /// </summary>
        private void SetupButtons()
        {
            if (playButton != null)
            {
                playButton.onClick.AddListener(OnPlayButtonClick);
            }
            
            if (settingsButton != null)
            {
                settingsButton.onClick.AddListener(OnSettingsButtonClick);
            }
            
            if (creditsButton != null)
            {
                creditsButton.onClick.AddListener(OnCreditsButtonClick);
            }
            
            if (quitButton != null)
            {
                quitButton.onClick.AddListener(OnQuitButtonClick);
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
        /// Обработчик кнопки "Настройки"
        /// </summary>
        public void OnSettingsButtonClick()
        {
            if (uiManager != null)
            {
                uiManager.PlayButtonClickSound();
                uiManager.ShowSettings();
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
            
            // Запускаем анимации
            StartCoroutine(AnimateMainMenu());
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
