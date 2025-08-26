using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Меню паузы
    /// </summary>
    public class PauseMenu : UIBase
    {
        [Header("Pause Elements")]
        public TextMeshProUGUI pauseTitleText;
        public TextMeshProUGUI contractInfoText;
        public TextMeshProUGUI timeElapsedText;
        
        [Header("Buttons")]
        public Button resumeButton;
        public Button settingsButton;
        public Button mainMenuButton;
        public Button quitButton;
        
        [Header("Animation")]
        public float buttonStaggerDelay = 0.1f;
        
        private void Start()
        {
            SetupButtons();
        }
        
        /// <summary>
        /// Настройка кнопок
        /// </summary>
        private void SetupButtons()
        {
            if (resumeButton != null)
            {
                resumeButton.onClick.AddListener(OnResumeButtonClick);
            }
            
            if (settingsButton != null)
            {
                settingsButton.onClick.AddListener(OnSettingsButtonClick);
            }
            
            if (mainMenuButton != null)
            {
                mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
            }
            
            if (quitButton != null)
            {
                quitButton.onClick.AddListener(OnQuitButtonClick);
            }
        }
        
        /// <summary>
        /// Вызывается при показе меню паузы
        /// </summary>
        protected override void OnShow()
        {
            base.OnShow();
            
            UpdatePauseDisplay();
            StartCoroutine(AnimatePauseMenu());
        }
        
        /// <summary>
        /// Обновить отображение паузы
        /// </summary>
        private void UpdatePauseDisplay()
        {
            // Заголовок
            if (pauseTitleText != null)
            {
                pauseTitleText.text = "ПАУЗА";
            }
            
            // Информация о контракте
            if (contractInfoText != null)
            {
                // TODO: Получить информацию о текущем контракте
                contractInfoText.text = "Текущий контракт: Доставка арбузов";
            }
            
            // Прошедшее время
            if (timeElapsedText != null)
            {
                // TODO: Получить время из активного контракта
                timeElapsedText.text = "Время: 00:00";
            }
        }
        
        /// <summary>
        /// Анимация меню паузы
        /// </summary>
        private System.Collections.IEnumerator AnimatePauseMenu()
        {
            // Анимация заголовка
            if (pauseTitleText != null)
            {
                yield return StartCoroutine(AnimateTitle());
            }
            
            // Анимация кнопок с задержкой
            yield return StartCoroutine(AnimateButtons());
        }
        
        /// <summary>
        /// Анимация заголовка
        /// </summary>
        private System.Collections.IEnumerator AnimateTitle()
        {
            if (pauseTitleText == null) yield break;
            
            Color originalColor = pauseTitleText.color;
            Vector3 originalScale = pauseTitleText.transform.localScale;
            
            // Начальное состояние
            pauseTitleText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
            pauseTitleText.transform.localScale = originalScale * 0.5f;
            
            float elapsedTime = 0f;
            float animationDuration = 0.5f;
            
            while (elapsedTime < animationDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                float progress = elapsedTime / animationDuration;
                float curveValue = showAnimationCurve.Evaluate(progress);
                
                pauseTitleText.color = new Color(originalColor.r, originalColor.g, originalColor.b, curveValue);
                pauseTitleText.transform.localScale = Vector3.Lerp(originalScale * 0.5f, originalScale, curveValue);
                
                yield return null;
            }
            
            pauseTitleText.color = originalColor;
            pauseTitleText.transform.localScale = originalScale;
        }
        
        /// <summary>
        /// Анимация кнопок
        /// </summary>
        private System.Collections.IEnumerator AnimateButtons()
        {
            Button[] buttons = { resumeButton, settingsButton, mainMenuButton, quitButton };
            
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
        private System.Collections.IEnumerator AnimateButton(Button button)
        {
            if (button == null) yield break;
            
            RectTransform buttonRect = button.GetComponent<RectTransform>();
            if (buttonRect == null) yield break;
            
            Vector3 originalPosition = buttonRect.anchoredPosition;
            Vector3 startPosition = originalPosition + Vector3.right * 50f;
            
            // Начальное состояние
            buttonRect.anchoredPosition = startPosition;
            button.interactable = false;
            
            float elapsedTime = 0f;
            float animationDuration = 0.3f;
            
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
        /// Обработчик кнопки "Возобновить"
        /// </summary>
        public void OnResumeButtonClick()
        {
            if (uiManager != null)
            {
                uiManager.PlayButtonClickSound();
                uiManager.HidePauseMenu();
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
        /// Обработчик кнопки "Главное меню"
        /// </summary>
        public void OnMainMenuButtonClick()
        {
            if (uiManager != null)
            {
                uiManager.PlayButtonClickSound();
                
                // TODO: Показать диалог подтверждения
                Debug.Log("Вернуться в главное меню? Прогресс будет потерян.");
                
                uiManager.ShowMainMenu();
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
        /// Обработка нажатия клавиши Escape
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnResumeButtonClick();
            }
        }
    }
}
