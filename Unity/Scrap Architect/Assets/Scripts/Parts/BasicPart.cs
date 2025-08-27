using UnityEngine;

namespace ScrapArchitect.Parts
{
    /// <summary>
    /// Базовая деталь машины
    /// </summary>
    public class BasicPart : MonoBehaviour
    {
        [Header("Part Properties")]
        public string partName = "Базовая деталь";
        public string partType = "Frame";
        public float weight = 1f;
        public float cost = 10f;
        
        [Header("Visual")]
        public Color partColor = Color.gray;
        public Material partMaterial;
        
        [Header("Physics")]
        public bool hasCollider = true;
        public bool hasRigidbody = true;
        
        private Renderer partRenderer;
        private Collider partCollider;
        private Rigidbody partRigidbody;
        
        private void Awake()
        {
            SetupPart();
        }
        
        /// <summary>
        /// Настройка детали
        /// </summary>
        private void SetupPart()
        {
            // Настройка рендерера
            partRenderer = GetComponent<Renderer>();
            if (partRenderer != null)
            {
                if (partMaterial != null)
                {
                    partRenderer.material = partMaterial;
                }
                partRenderer.material.color = partColor;
            }
            
            // Настройка коллайдера
            if (hasCollider && GetComponent<Collider>() == null)
            {
                BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
                boxCollider.size = Vector3.one;
            }
            
            // Настройка риджидбоди
            if (hasRigidbody && GetComponent<Rigidbody>() == null)
            {
                partRigidbody = gameObject.AddComponent<Rigidbody>();
                partRigidbody.mass = weight;
            }
        }
        
        /// <summary>
        /// Получить информацию о детали
        /// </summary>
        public string GetPartInfo()
        {
            return $"{partName} ({partType})\nВес: {weight}кг\nСтоимость: {cost} скрапа";
        }
        
        /// <summary>
        /// Установить цвет детали
        /// </summary>
        public void SetColor(Color color)
        {
            partColor = color;
            if (partRenderer != null)
            {
                partRenderer.material.color = color;
            }
        }
        
        /// <summary>
        /// Активировать деталь
        /// </summary>
        public virtual void Activate()
        {
            Debug.Log($"Деталь {partName} активирована!");
        }
        
        /// <summary>
        /// Деактивировать деталь
        /// </summary>
        public virtual void Deactivate()
        {
            Debug.Log($"Деталь {partName} деактивирована!");
        }
    }
}
