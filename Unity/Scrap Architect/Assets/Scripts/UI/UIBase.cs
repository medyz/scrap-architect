using UnityEngine;
using System;
using System.Collections;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Базовый класс для всех UI панелей
    /// </summary>
    public abstract class UIBase : MonoBehaviour
    {
        [Header("UI Base Settings")]
        public bool useAnimation = true;
        public float showAnimationDuration = 0.3f;
        public float hideAnimationDuration = 0.2f;
        public AnimationCurve showAnimationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public AnimationCurve hideAnimationCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
        
        [Header("Audio")]
        public AudioClip showSound;
        public AudioClip hideSound;
        
        protected UIManager uiManager;
        protected CanvasGroup canvasGroup;
        protected RectTransform rectTransform;
        protected bool isInitialized = false;
        protected bool isVisible = false;
        
        // Events
        public Action OnShowStarted;
        public Action OnShowCompleted;
        public Action OnHideStarted;
        public Action OnHideCompleted;
        
        protected virtual void Awake()
        {
            // Получаем компоненты
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            
            rectTransform = GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                rectTransform = gameObject.GetComponent<RectTransform>();
            }
        }
        
        /// <summary>
        /// Инициализация панели
        /// </summary>
        public virtual void Initialize(UIManager manager)
        {
            uiManager = manager;
            isInitialized = true;
            
            // Убеждаемся, что CanvasGroup существует
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }
            
            // Скрываем панель по умолчанию
            if (useAnimation)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
            
            OnInitialize();
        }
        
        /// <summary>
        /// Показать панель
        /// </summary>
        public virtual void Show()
        {
            if (!isInitialized)
            {
                Debug.LogWarning($"Panel {name} is not initialized!");
                return;
            }
            
            if (isVisible) return;
            
            gameObject.SetActive(true);
            OnShowStarted?.Invoke();
            
            // Проверяем, что объект активен перед запуском корутины
            if (useAnimation && gameObject.activeInHierarchy)
            {
                StartCoroutine(ShowAnimation());
            }
            else
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                isVisible = true;
                OnShowCompleted?.Invoke();
            }
            
            PlayShowSound();
            OnShow();
        }
        
        /// <summary>
        /// Скрыть панель
        /// </summary>
        public virtual void Hide()
        {
            if (!isVisible) return;
            
            OnHideStarted?.Invoke();
            
            if (useAnimation)
            {
                StartCoroutine(HideAnimation());
            }
            else
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                isVisible = false;
                gameObject.SetActive(false);
                OnHideCompleted?.Invoke();
            }
            
            PlayHideSound();
            OnHide();
        }
        
        /// <summary>
        /// Анимация показа
        /// </summary>
        protected virtual IEnumerator ShowAnimation()
        {
            float elapsedTime = 0f;
            float startAlpha = canvasGroup.alpha;
            
            while (elapsedTime < showAnimationDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                float progress = elapsedTime / showAnimationDuration;
                float curveValue = showAnimationCurve.Evaluate(progress);
                
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, curveValue);
                
                yield return null;
            }
            
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            isVisible = true;
            OnShowCompleted?.Invoke();
        }
        
        /// <summary>
        /// Анимация скрытия
        /// </summary>
        protected virtual IEnumerator HideAnimation()
        {
            float elapsedTime = 0f;
            float startAlpha = canvasGroup.alpha;
            
            while (elapsedTime < hideAnimationDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                float progress = elapsedTime / hideAnimationDuration;
                float curveValue = hideAnimationCurve.Evaluate(progress);
                
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, curveValue);
                
                yield return null;
            }
            
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            isVisible = false;
            gameObject.SetActive(false);
            OnHideCompleted?.Invoke();
        }
        
        /// <summary>
        /// Проверить, видима ли панель
        /// </summary>
        public bool IsVisible()
        {
            return isVisible;
        }
        
        /// <summary>
        /// Проверить, инициализирована ли панель
        /// </summary>
        public bool IsInitialized()
        {
            return isInitialized;
        }
        
        #region Audio Methods
        
        /// <summary>
        /// Воспроизвести звук показа
        /// </summary>
        protected virtual void PlayShowSound()
        {
            if (showSound != null && uiManager != null)
            {
                AudioSource.PlayClipAtPoint(showSound, Camera.main.transform.position);
            }
        }
        
        /// <summary>
        /// Воспроизвести звук скрытия
        /// </summary>
        protected virtual void PlayHideSound()
        {
            if (hideSound != null && uiManager != null)
            {
                AudioSource.PlayClipAtPoint(hideSound, Camera.main.transform.position);
            }
        }
        
        #endregion
        
        #region Virtual Methods
        
        /// <summary>
        /// Вызывается при инициализации
        /// </summary>
        protected virtual void OnInitialize()
        {
            // Переопределяется в наследниках
        }
        
        /// <summary>
        /// Вызывается при показе панели
        /// </summary>
        protected virtual void OnShow()
        {
            // Переопределяется в наследниках
        }
        
        /// <summary>
        /// Вызывается при скрытии панели
        /// </summary>
        protected virtual void OnHide()
        {
            // Переопределяется в наследниках
        }
        
        #endregion
        
        #region Button Handlers
        
        /// <summary>
        /// Обработчик кнопки "Назад"
        /// </summary>
        public virtual void OnBackButtonClick()
        {
            if (uiManager != null)
            {
                uiManager.PlayButtonClickSound();
                uiManager.GoBack();
            }
        }
        
        /// <summary>
        /// Обработчик кнопки "Закрыть"
        /// </summary>
        public virtual void OnCloseButtonClick()
        {
            if (uiManager != null)
            {
                uiManager.PlayButtonClickSound();
                uiManager.HideCurrentPanel();
            }
        }
        
        #endregion
    }
}
