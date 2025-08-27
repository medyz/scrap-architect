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
        
        /// <summary>
        /// Проверка, инициализирована ли панель
        /// </summary>
        public bool IsInitialized => isInitialized;
        
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
            Debug.Log($"UIBase.Initialize: Panel {name} - manager: {manager?.name ?? "null"}");
            
            uiManager = manager;
            isInitialized = true;
            
            // Убеждаемся, что CanvasGroup существует
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
                    Debug.Log($"UIBase.Initialize: Added CanvasGroup to {name}");
                }
            }
            
            Debug.Log($"UIBase.Initialize: Panel {name} - CanvasGroup: {canvasGroup?.name ?? "null"}");
            
            // Скрываем панель по умолчанию только если она не должна быть видимой
            if (useAnimation)
            {
                // Проверяем, не является ли это главным меню, которое должно быть видимым
                if (this is MainMenuUI)
                {
                    Debug.Log($"UIBase.Initialize: Panel {name} - MainMenuUI, keeping visible");
                    canvasGroup.alpha = 1f;
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                    isVisible = true;
                }
                else
                {
                    canvasGroup.alpha = 0f;
                    canvasGroup.interactable = false;
                    canvasGroup.blocksRaycasts = false;
                    Debug.Log($"UIBase.Initialize: Panel {name} - Hidden with animation (alpha: 0)");
                }
            }
            else
            {
                Debug.Log($"UIBase.Initialize: Panel {name} - No animation, keeping current state");
            }
            
            OnInitialize();
            Debug.Log($"UIBase.Initialize: Panel {name} - Initialization complete");
        }
        
        /// <summary>
        /// Показать панель
        /// </summary>
        public virtual void Show()
        {
            Debug.Log($"UIBase.Show: Panel {name} - initialized: {isInitialized}, visible: {isVisible}");
            
            if (!isInitialized)
            {
                Debug.LogWarning($"Panel {name} is not initialized!");
                return;
            }
            
            // Для MainMenuUI принудительно устанавливаем видимость, даже если isVisible = true
            bool forceShow = (this is MainMenuUI);
            
            if (isVisible && !forceShow) 
            {
                Debug.Log($"UIBase.Show: Panel {name} - Already visible, skipping");
                return;
            }
            
            gameObject.SetActive(true);
            Debug.Log($"UIBase.Show: Panel {name} - GameObject active: {gameObject.activeInHierarchy}");
            OnShowStarted?.Invoke();
            
            // Проверяем, что объект активен перед запуском корутины
            if (useAnimation && gameObject.activeInHierarchy)
            {
                Debug.Log($"UIBase.Show: Panel {name} - Starting animation");
                StartCoroutine(ShowAnimation());
            }
            else
            {
                Debug.Log($"UIBase.Show: Panel {name} - No animation, setting visibility directly");
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                isVisible = true;
                OnShowCompleted?.Invoke();
                Debug.Log($"UIBase.Show: Panel {name} - Visibility set - alpha: {canvasGroup.alpha}");
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
            
            if (useAnimation && gameObject.activeInHierarchy)
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
