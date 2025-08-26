using UnityEngine;
using System;

namespace ScrapArchitect.Level
{
    /// <summary>
    /// Компонент чекпоинта для уровней
    /// </summary>
    public class Checkpoint : MonoBehaviour
    {
        [Header("Checkpoint Settings")]
        public int checkpointID = 0;
        public string checkpointName = "Checkpoint";
        public bool isActivated = false;
        public bool isRequired = true;
        
        [Header("Visual")]
        public Material inactiveMaterial;
        public Material activeMaterial;
        public Material completedMaterial;
        public GameObject activationEffect;
        
        [Header("Audio")]
        public AudioClip activationSound;
        public AudioClip completionSound;
        
        private Renderer checkpointRenderer;
        private AudioSource audioSource;
        
        // Events
        public Action<Checkpoint> OnCheckpointActivated;
        public Action<Checkpoint> OnCheckpointCompleted;
        
        private void Awake()
        {
            checkpointRenderer = GetComponent<Renderer>();
            audioSource = GetComponent<AudioSource>();
            
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            
            // Настроить начальный материал
            if (checkpointRenderer != null && inactiveMaterial != null)
            {
                checkpointRenderer.material = inactiveMaterial;
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            // Проверить, что это игрок или его транспорт
            if (other.CompareTag("Player") || other.CompareTag("Vehicle"))
            {
                ActivateCheckpoint();
            }
        }
        
        /// <summary>
        /// Активировать чекпоинт
        /// </summary>
        public void ActivateCheckpoint()
        {
            if (isActivated) return;
            
            isActivated = true;
            
            // Изменить материал
            if (checkpointRenderer != null && activeMaterial != null)
            {
                checkpointRenderer.material = activeMaterial;
            }
            
            // Воспроизвести звук
            PlayActivationSound();
            
            // Создать эффект активации
            CreateActivationEffect();
            
            // Вызвать событие
            OnCheckpointActivated?.Invoke(this);
            
            Debug.Log($"Checkpoint activated: {checkpointName}");
        }
        
        /// <summary>
        /// Завершить чекпоинт
        /// </summary>
        public void CompleteCheckpoint()
        {
            if (!isActivated) return;
            
            // Изменить материал
            if (checkpointRenderer != null && completedMaterial != null)
            {
                checkpointRenderer.material = completedMaterial;
            }
            
            // Воспроизвести звук
            PlayCompletionSound();
            
            // Вызвать событие
            OnCheckpointCompleted?.Invoke(this);
            
            Debug.Log($"Checkpoint completed: {checkpointName}");
        }
        
        /// <summary>
        /// Сбросить чекпоинт
        /// </summary>
        public void ResetCheckpoint()
        {
            isActivated = false;
            
            // Вернуть начальный материал
            if (checkpointRenderer != null && inactiveMaterial != null)
            {
                checkpointRenderer.material = inactiveMaterial;
            }
            
            Debug.Log($"Checkpoint reset: {checkpointName}");
        }
        
        /// <summary>
        /// Воспроизвести звук активации
        /// </summary>
        private void PlayActivationSound()
        {
            if (activationSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(activationSound);
            }
        }
        
        /// <summary>
        /// Воспроизвести звук завершения
        /// </summary>
        private void PlayCompletionSound()
        {
            if (completionSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(completionSound);
            }
        }
        
        /// <summary>
        /// Создать эффект активации
        /// </summary>
        private void CreateActivationEffect()
        {
            if (activationEffect != null)
            {
                Instantiate(activationEffect, transform.position, transform.rotation);
            }
        }
        
        /// <summary>
        /// Получить информацию о чекпоинте
        /// </summary>
        public string GetCheckpointInfo()
        {
            string info = $"Checkpoint: {checkpointName}\n";
            info += $"ID: {checkpointID}\n";
            info += $"Status: {(isActivated ? "Activated" : "Inactive")}\n";
            info += $"Required: {(isRequired ? "Yes" : "No")}\n";
            
            return info;
        }
    }
}
