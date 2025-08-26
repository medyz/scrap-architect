using UnityEngine;
using System;

namespace ScrapArchitect.Level
{
    /// <summary>
    /// Компонент для объектов, которые можно убирать/чистить
    /// </summary>
    public class CleanableObject : MonoBehaviour
    {
        [Header("Cleaning Settings")]
        public float cleanProgress = 0f;
        public float requiredCleaning = 100f;
        public float cleaningSpeed = 10f;
        public bool isFullyCleaned = false;
        
        [Header("Visual")]
        public Material dirtyMaterial;
        public Material partiallyCleanMaterial;
        public Material cleanMaterial;
        public GameObject cleaningEffect;
        public float swaySpeed = 2f;
        public float swayAmount = 0.1f;
        
        [Header("Audio")]
        public AudioClip cleaningSound;
        public AudioClip cleanCompleteSound;
        
        private Renderer objectRenderer;
        private AudioSource audioSource;
        private Vector3 startPosition;
        private float swayTime;
        
        // Events
        public Action<CleanableObject> OnCleaningProgressChanged;
        public Action<CleanableObject> OnFullyCleaned;
        
        private void Awake()
        {
            objectRenderer = GetComponent<Renderer>();
            audioSource = GetComponent<AudioSource>();
            startPosition = transform.position;
            swayTime = 0f;
            
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            
            // Настроить начальный материал
            if (objectRenderer != null && dirtyMaterial != null)
            {
                objectRenderer.material = dirtyMaterial;
            }
        }
        
        private void Update()
        {
            if (!isFullyCleaned)
            {
                // Покачивание объекта
                swayTime += Time.deltaTime * swaySpeed;
                float swayX = Mathf.Sin(swayTime) * swayAmount;
                transform.position = startPosition + new Vector3(swayX, 0f, 0f);
            }
        }
        
        /// <summary>
        /// Убрать/почистить объект
        /// </summary>
        public void CleanObject(float cleanAmount)
        {
            if (isFullyCleaned) return;
            
            cleanProgress += cleanAmount * cleaningSpeed * Time.deltaTime;
            cleanProgress = Mathf.Clamp(cleanProgress, 0f, requiredCleaning);
            
            // Обновить материал
            UpdateCleaningMaterial();
            
            // Воспроизвести звук
            PlayCleaningSound();
            
            // Создать эффект уборки
            CreateCleaningEffect();
            
            // Проверить завершение уборки
            if (cleanProgress >= requiredCleaning && !isFullyCleaned)
            {
                CompleteCleaning();
            }
            
            // Вызвать событие
            OnCleaningProgressChanged?.Invoke(this);
        }
        
        /// <summary>
        /// Обновить материал в зависимости от прогресса уборки
        /// </summary>
        private void UpdateCleaningMaterial()
        {
            if (objectRenderer == null) return;
            
            float cleanPercentage = cleanProgress / requiredCleaning;
            
            if (cleanPercentage >= 1f)
            {
                // Полностью убрано
                if (cleanMaterial != null)
                {
                    objectRenderer.material = cleanMaterial;
                }
            }
            else if (cleanPercentage >= 0.5f)
            {
                // Частично убрано
                if (partiallyCleanMaterial != null)
                {
                    objectRenderer.material = partiallyCleanMaterial;
                }
            }
            else
            {
                // Грязное
                if (dirtyMaterial != null)
                {
                    objectRenderer.material = dirtyMaterial;
                }
            }
        }
        
        /// <summary>
        /// Завершить уборку
        /// </summary>
        private void CompleteCleaning()
        {
            isFullyCleaned = true;
            
            // Воспроизвести звук завершения
            PlayCleanCompleteSound();
            
            // Скрыть объект
            gameObject.SetActive(false);
            
            // Вызвать событие
            OnFullyCleaned?.Invoke(this);
            
            Debug.Log($"Object fully cleaned: {gameObject.name}");
        }
        
        /// <summary>
        /// Сбросить уборку
        /// </summary>
        public void ResetCleaning()
        {
            cleanProgress = 0f;
            isFullyCleaned = false;
            gameObject.SetActive(true);
            
            // Вернуть начальный материал
            if (objectRenderer != null && dirtyMaterial != null)
            {
                objectRenderer.material = dirtyMaterial;
            }
            
            // Вернуть в начальную позицию
            transform.position = startPosition;
            swayTime = 0f;
            
            Debug.Log($"Cleaning reset: {gameObject.name}");
        }
        
        /// <summary>
        /// Установить прогресс уборки
        /// </summary>
        public void SetCleaningProgress(float progress)
        {
            cleanProgress = Mathf.Clamp(progress, 0f, requiredCleaning);
            UpdateCleaningMaterial();
            
            if (cleanProgress >= requiredCleaning && !isFullyCleaned)
            {
                CompleteCleaning();
            }
        }
        
        /// <summary>
        /// Получить процент уборки
        /// </summary>
        public float GetCleaningPercentage()
        {
            return cleanProgress / requiredCleaning;
        }
        
        /// <summary>
        /// Воспроизвести звук уборки
        /// </summary>
        private void PlayCleaningSound()
        {
            if (cleaningSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(cleaningSound);
            }
        }
        
        /// <summary>
        /// Воспроизвести звук завершения уборки
        /// </summary>
        private void PlayCleanCompleteSound()
        {
            if (cleanCompleteSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(cleanCompleteSound);
            }
        }
        
        /// <summary>
        /// Создать эффект уборки
        /// </summary>
        private void CreateCleaningEffect()
        {
            if (cleaningEffect != null)
            {
                Instantiate(cleaningEffect, transform.position, transform.rotation);
            }
        }
        
        /// <summary>
        /// Получить информацию об уборке
        /// </summary>
        public string GetCleaningInfo()
        {
            string info = $"Object: {gameObject.name}\n";
            info += $"Cleaning Progress: {cleanProgress:F1}/{requiredCleaning:F1}\n";
            info += $"Percentage: {GetCleaningPercentage():P0}\n";
            info += $"Fully Cleaned: {(isFullyCleaned ? "Yes" : "No")}\n";
            
            return info;
        }
        
        /// <summary>
        /// Установить требуемое количество уборки
        /// </summary>
        public void SetRequiredCleaning(float required)
        {
            requiredCleaning = Mathf.Max(1f, required);
        }
        
        /// <summary>
        /// Установить скорость уборки
        /// </summary>
        public void SetCleaningSpeed(float speed)
        {
            cleaningSpeed = Mathf.Max(0.1f, speed);
        }
        
        /// <summary>
        /// Установить скорость покачивания
        /// </summary>
        public void SetSwaySpeed(float speed)
        {
            swaySpeed = Mathf.Max(0.1f, speed);
        }
        
        /// <summary>
        /// Установить амплитуду покачивания
        /// </summary>
        public void SetSwayAmount(float amount)
        {
            swayAmount = Mathf.Max(0f, amount);
        }
    }
}
