using UnityEngine;
using System;

namespace ScrapArchitect.Level
{
    /// <summary>
    /// Компонент для объектов, которые можно красить
    /// </summary>
    public class PaintableObject : MonoBehaviour
    {
        [Header("Paint Settings")]
        public float paintProgress = 0f;
        public float requiredPaint = 100f;
        public float paintSpeed = 10f;
        public bool isFullyPainted = false;
        
        [Header("Visual")]
        public Material unpaintedMaterial;
        public Material partiallyPaintedMaterial;
        public Material fullyPaintedMaterial;
        public GameObject paintEffect;
        
        [Header("Audio")]
        public AudioClip paintSound;
        public AudioClip paintCompleteSound;
        
        private Renderer objectRenderer;
        private AudioSource audioSource;
        
        // Events
        public Action<PaintableObject> OnPaintProgressChanged;
        public Action<PaintableObject> OnFullyPainted;
        
        private void Awake()
        {
            objectRenderer = GetComponent<Renderer>();
            audioSource = GetComponent<AudioSource>();
            
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            
            // Настроить начальный материал
            if (objectRenderer != null && unpaintedMaterial != null)
            {
                objectRenderer.material = unpaintedMaterial;
            }
        }
        
        /// <summary>
        /// Покрасить объект
        /// </summary>
        public void PaintObject(float paintAmount)
        {
            if (isFullyPainted) return;
            
            paintProgress += paintAmount * paintSpeed * Time.deltaTime;
            paintProgress = Mathf.Clamp(paintProgress, 0f, requiredPaint);
            
            // Обновить материал
            UpdatePaintMaterial();
            
            // Воспроизвести звук
            PlayPaintSound();
            
            // Создать эффект покраски
            CreatePaintEffect();
            
            // Проверить завершение покраски
            if (paintProgress >= requiredPaint && !isFullyPainted)
            {
                CompletePainting();
            }
            
            // Вызвать событие
            OnPaintProgressChanged?.Invoke(this);
        }
        
        /// <summary>
        /// Обновить материал в зависимости от прогресса покраски
        /// </summary>
        private void UpdatePaintMaterial()
        {
            if (objectRenderer == null) return;
            
            float paintPercentage = paintProgress / requiredPaint;
            
            if (paintPercentage >= 1f)
            {
                // Полностью покрашено
                if (fullyPaintedMaterial != null)
                {
                    objectRenderer.material = fullyPaintedMaterial;
                }
            }
            else if (paintPercentage >= 0.5f)
            {
                // Частично покрашено
                if (partiallyPaintedMaterial != null)
                {
                    objectRenderer.material = partiallyPaintedMaterial;
                }
            }
            else
            {
                // Не покрашено
                if (unpaintedMaterial != null)
                {
                    objectRenderer.material = unpaintedMaterial;
                }
            }
        }
        
        /// <summary>
        /// Завершить покраску
        /// </summary>
        private void CompletePainting()
        {
            isFullyPainted = true;
            
            // Воспроизвести звук завершения
            PlayPaintCompleteSound();
            
            // Вызвать событие
            OnFullyPainted?.Invoke(this);
            
            Debug.Log($"Object fully painted: {gameObject.name}");
        }
        
        /// <summary>
        /// Сбросить покраску
        /// </summary>
        public void ResetPaint()
        {
            paintProgress = 0f;
            isFullyPainted = false;
            
            // Вернуть начальный материал
            if (objectRenderer != null && unpaintedMaterial != null)
            {
                objectRenderer.material = unpaintedMaterial;
            }
            
            Debug.Log($"Paint reset: {gameObject.name}");
        }
        
        /// <summary>
        /// Установить прогресс покраски
        /// </summary>
        public void SetPaintProgress(float progress)
        {
            paintProgress = Mathf.Clamp(progress, 0f, requiredPaint);
            UpdatePaintMaterial();
            
            if (paintProgress >= requiredPaint && !isFullyPainted)
            {
                CompletePainting();
            }
        }
        
        /// <summary>
        /// Получить процент покраски
        /// </summary>
        public float GetPaintPercentage()
        {
            return paintProgress / requiredPaint;
        }
        
        /// <summary>
        /// Воспроизвести звук покраски
        /// </summary>
        private void PlayPaintSound()
        {
            if (paintSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(paintSound);
            }
        }
        
        /// <summary>
        /// Воспроизвести звук завершения покраски
        /// </summary>
        private void PlayPaintCompleteSound()
        {
            if (paintCompleteSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(paintCompleteSound);
            }
        }
        
        /// <summary>
        /// Создать эффект покраски
        /// </summary>
        private void CreatePaintEffect()
        {
            if (paintEffect != null)
            {
                Instantiate(paintEffect, transform.position, transform.rotation);
            }
        }
        
        /// <summary>
        /// Получить информацию о покраске
        /// </summary>
        public string GetPaintInfo()
        {
            string info = $"Object: {gameObject.name}\n";
            info += $"Paint Progress: {paintProgress:F1}/{requiredPaint:F1}\n";
            info += $"Percentage: {GetPaintPercentage():P0}\n";
            info += $"Fully Painted: {(isFullyPainted ? "Yes" : "No")}\n";
            
            return info;
        }
        
        /// <summary>
        /// Установить требуемое количество краски
        /// </summary>
        public void SetRequiredPaint(float required)
        {
            requiredPaint = Mathf.Max(1f, required);
        }
        
        /// <summary>
        /// Установить скорость покраски
        /// </summary>
        public void SetPaintSpeed(float speed)
        {
            paintSpeed = Mathf.Max(0.1f, speed);
        }
    }
}
