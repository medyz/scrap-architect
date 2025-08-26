using UnityEngine;
using System;

namespace ScrapArchitect.Level
{
    /// <summary>
    /// Компонент собираемого предмета
    /// </summary>
    public class CollectibleItem : MonoBehaviour
    {
        [Header("Item Settings")]
        public string itemType = "generic";
        public int itemValue = 1;
        public bool isCollected = false;
        public bool isRequired = true;
        
        [Header("Visual")]
        public Material defaultMaterial;
        public Material collectedMaterial;
        public GameObject collectionEffect;
        public float rotationSpeed = 50f;
        public float bobSpeed = 2f;
        public float bobHeight = 0.5f;
        
        [Header("Audio")]
        public AudioClip collectionSound;
        public AudioClip hoverSound;
        
        private Renderer itemRenderer;
        private AudioSource audioSource;
        private Vector3 startPosition;
        private float bobTime;
        
        // Events
        public Action<CollectibleItem> OnItemCollected;
        public Action<CollectibleItem> OnItemHovered;
        
        private void Awake()
        {
            itemRenderer = GetComponent<Renderer>();
            audioSource = GetComponent<AudioSource>();
            startPosition = transform.position;
            bobTime = 0f;
            
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            
            // Настроить начальный материал
            if (itemRenderer != null && defaultMaterial != null)
            {
                itemRenderer.material = defaultMaterial;
            }
        }
        
        private void Update()
        {
            if (!isCollected)
            {
                // Вращение предмета
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
                
                // Плавающее движение
                bobTime += Time.deltaTime * bobSpeed;
                float newY = startPosition.y + Mathf.Sin(bobTime) * bobHeight;
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            // Проверить, что это игрок или его транспорт
            if (other.CompareTag("Player") || other.CompareTag("Vehicle"))
            {
                CollectItem();
            }
        }
        
        private void OnMouseEnter()
        {
            // Воспроизвести звук при наведении мыши
            PlayHoverSound();
            
            // Вызвать событие
            OnItemHovered?.Invoke(this);
        }
        
        /// <summary>
        /// Собрать предмет
        /// </summary>
        public void CollectItem()
        {
            if (isCollected) return;
            
            isCollected = true;
            
            // Изменить материал
            if (itemRenderer != null && collectedMaterial != null)
            {
                itemRenderer.material = collectedMaterial;
            }
            
            // Воспроизвести звук
            PlayCollectionSound();
            
            // Создать эффект сбора
            CreateCollectionEffect();
            
            // Скрыть предмет
            gameObject.SetActive(false);
            
            // Вызвать событие
            OnItemCollected?.Invoke(this);
            
            Debug.Log($"Item collected: {itemType} (Value: {itemValue})");
        }
        
        /// <summary>
        /// Сбросить предмет
        /// </summary>
        public void ResetItem()
        {
            isCollected = false;
            gameObject.SetActive(true);
            
            // Вернуть начальный материал
            if (itemRenderer != null && defaultMaterial != null)
            {
                itemRenderer.material = defaultMaterial;
            }
            
            // Вернуть в начальную позицию
            transform.position = startPosition;
            bobTime = 0f;
            
            Debug.Log($"Item reset: {itemType}");
        }
        
        /// <summary>
        /// Воспроизвести звук сбора
        /// </summary>
        private void PlayCollectionSound()
        {
            if (collectionSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(collectionSound);
            }
        }
        
        /// <summary>
        /// Воспроизвести звук при наведении
        /// </summary>
        private void PlayHoverSound()
        {
            if (hoverSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(hoverSound);
            }
        }
        
        /// <summary>
        /// Создать эффект сбора
        /// </summary>
        private void CreateCollectionEffect()
        {
            if (collectionEffect != null)
            {
                Instantiate(collectionEffect, transform.position, transform.rotation);
            }
        }
        
        /// <summary>
        /// Получить информацию о предмете
        /// </summary>
        public string GetItemInfo()
        {
            string info = $"Item: {itemType}\n";
            info += $"Value: {itemValue}\n";
            info += $"Status: {(isCollected ? "Collected" : "Available")}\n";
            info += $"Required: {(isRequired ? "Yes" : "No")}\n";
            
            return info;
        }
        
        /// <summary>
        /// Установить тип предмета
        /// </summary>
        public void SetItemType(string type)
        {
            itemType = type;
        }
        
        /// <summary>
        /// Установить значение предмета
        /// </summary>
        public void SetItemValue(int value)
        {
            itemValue = value;
        }
    }
}
