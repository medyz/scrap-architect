using UnityEngine;
using ScrapArchitect.Parts;

namespace ScrapArchitect.Physics
{
    public class SnapPoint : MonoBehaviour
    {
        [Header("Snap Point Settings")]
        public SnapPointType snapType = SnapPointType.Universal;
        public Vector3 snapDirection = Vector3.up;
        public float snapRadius = 0.5f;
        public bool isOccupied = false;
        public bool isActive = true;
        
        [Header("Visual Settings")]
        public GameObject snapIndicator;
        public Material availableMaterial;
        public Material occupiedMaterial;
        public Material highlightedMaterial;
        public Color snapColor = Color.green;
        public Color occupiedColor = Color.red;
        public Color highlightColor = Color.yellow;
        
        [Header("Audio Settings")]
        public AudioClip snapSound;
        public AudioClip unsnapSound;
        
        private Renderer indicatorRenderer;
        private SnapPoint connectedSnapPoint;
        private PartController parentPart;
        private bool isHighlighted = false;
        
        public enum SnapPointType
        {
            Universal,      // Универсальный
            Block,          // Только для блоков
            Wheel,          // Только для колес
            Motor,          // Только для двигателей
            Connection,     // Только для соединений
            Tool,           // Только для инструментов
            Seat            // Только для сидений
        }
        
        private void Start()
        {
            parentPart = GetComponentInParent<PartController>();
            
            if (snapIndicator != null)
            {
                indicatorRenderer = snapIndicator.GetComponent<Renderer>();
                UpdateVisualState();
            }
            
            // Настройка коллайдера для snap-point
            SetupCollider();
        }
        
        private void SetupCollider()
        {
            SphereCollider snapCollider = gameObject.GetComponent<SphereCollider>();
            if (snapCollider == null)
            {
                snapCollider = gameObject.AddComponent<SphereCollider>();
            }
            
            snapCollider.radius = snapRadius;
            snapCollider.isTrigger = true;
            
            // Настройка слоя для snap-point
            gameObject.layer = LayerMask.NameToLayer("SnapPoint");
        }
        
        public bool CanSnapTo(SnapPoint otherSnapPoint)
        {
            if (!isActive || !otherSnapPoint.isActive)
            {
                return false;
            }
            
            if (isOccupied || otherSnapPoint.isOccupied)
            {
                return false;
            }
            
            // Проверка совместимости типов
            if (!AreTypesCompatible(snapType, otherSnapPoint.snapType))
            {
                return false;
            }
            
            // Проверка расстояния
            float distance = Vector3.Distance(transform.position, otherSnapPoint.transform.position);
            if (distance > snapRadius + otherSnapPoint.snapRadius)
            {
                return false;
            }
            
            // Проверка направления
            if (!AreDirectionsCompatible(snapDirection, otherSnapPoint.snapDirection))
            {
                return false;
            }
            
            return true;
        }
        
        public bool TrySnapTo(SnapPoint otherSnapPoint)
        {
            if (!CanSnapTo(otherSnapPoint))
            {
                return false;
            }
            
            // Выполнить привязку
            isOccupied = true;
            otherSnapPoint.isOccupied = true;
            
            connectedSnapPoint = otherSnapPoint;
            otherSnapPoint.connectedSnapPoint = this;
            
            // Обновить визуальное состояние
            UpdateVisualState();
            otherSnapPoint.UpdateVisualState();
            
            // Воспроизвести звук привязки
            PlaySnapSound();
            
            // Уведомить родительские детали о соединении
            // TODO: Добавить события соединения в PartController
            
            Debug.Log($"Snap points connected: {parentPart?.partName} -> {otherSnapPoint.parentPart?.partName}");
            return true;
        }
        
        public void Unsnap()
        {
            if (!isOccupied || connectedSnapPoint == null)
            {
                return;
            }
            
            // Отвязать snap-points
            isOccupied = false;
            connectedSnapPoint.isOccupied = false;
            
            // Уведомить родительские детали о разъединении
            // TODO: Добавить события разъединения в PartController
            
            // Воспроизвести звук отвязки
            PlayUnsnapSound();
            
            // Обновить визуальное состояние
            UpdateVisualState();
            connectedSnapPoint.UpdateVisualState();
            
            Debug.Log($"Snap points disconnected: {parentPart?.partName} -> {connectedSnapPoint.parentPart?.partName}");
            
            connectedSnapPoint.connectedSnapPoint = null;
            connectedSnapPoint = null;
        }
        
        public void SetHighlighted(bool highlighted)
        {
            isHighlighted = highlighted;
            UpdateVisualState();
        }
        
        public void SetActive(bool active)
        {
            isActive = active;
            UpdateVisualState();
        }
        
        private void UpdateVisualState()
        {
            if (indicatorRenderer == null)
            {
                return;
            }
            
            Material targetMaterial = null;
            Color targetColor = snapColor;
            
            if (!isActive)
            {
                targetMaterial = occupiedMaterial;
                targetColor = Color.gray;
            }
            else if (isOccupied)
            {
                targetMaterial = occupiedMaterial;
                targetColor = occupiedColor;
            }
            else if (isHighlighted)
            {
                targetMaterial = highlightedMaterial;
                targetColor = highlightColor;
            }
            else
            {
                targetMaterial = availableMaterial;
                targetColor = snapColor;
            }
            
            if (targetMaterial != null)
            {
                indicatorRenderer.material = targetMaterial;
            }
            
            indicatorRenderer.material.color = targetColor;
        }
        
        private bool AreTypesCompatible(SnapPointType type1, SnapPointType type2)
        {
            // Универсальный тип совместим со всеми
            if (type1 == SnapPointType.Universal || type2 == SnapPointType.Universal)
            {
                return true;
            }
            
            // Одинаковые типы совместимы
            if (type1 == type2)
            {
                return true;
            }
            
            // Специальные правила совместимости
            switch (type1)
            {
                case SnapPointType.Block:
                    return type2 == SnapPointType.Block || type2 == SnapPointType.Connection;
                    
                case SnapPointType.Wheel:
                    return type2 == SnapPointType.Wheel || type2 == SnapPointType.Connection;
                    
                case SnapPointType.Motor:
                    return type2 == SnapPointType.Motor || type2 == SnapPointType.Connection;
                    
                case SnapPointType.Connection:
                    return type2 == SnapPointType.Block || type2 == SnapPointType.Wheel || 
                           type2 == SnapPointType.Motor || type2 == SnapPointType.Tool || 
                           type2 == SnapPointType.Seat;
                    
                case SnapPointType.Tool:
                    return type2 == SnapPointType.Connection;
                    
                case SnapPointType.Seat:
                    return type2 == SnapPointType.Connection;
                    
                default:
                    return false;
            }
        }
        
        private bool AreDirectionsCompatible(Vector3 dir1, Vector3 dir2)
        {
            // Проверка противоположности направлений (с небольшим допуском)
            float dotProduct = Vector3.Dot(dir1, dir2);
            return dotProduct < -0.8f; // Угол менее ~37 градусов
        }
        
        private void PlaySnapSound()
        {
            if (snapSound != null)
            {
                AudioSource.PlayClipAtPoint(snapSound, transform.position);
            }
        }
        
        private void PlayUnsnapSound()
        {
            if (unsnapSound != null)
            {
                AudioSource.PlayClipAtPoint(unsnapSound, transform.position);
            }
        }
        
        public Vector3 GetSnapDirection()
        {
            return snapDirection;
        }
        
        public float GetSnapRadius()
        {
            return snapRadius;
        }
        
        public bool IsOccupied()
        {
            return isOccupied;
        }
        
        public bool IsActive()
        {
            return isActive;
        }
        
        public SnapPoint GetConnectedSnapPoint()
        {
            return connectedSnapPoint;
        }
        
        public PartController GetParentPart()
        {
            return parentPart;
        }
        
        public Vector3 GetSnapPosition()
        {
            return transform.position;
        }
        
        public Quaternion GetSnapRotation()
        {
            return transform.rotation;
        }
        
        private void OnDrawGizmosSelected()
        {
            // Визуализация snap-point в редакторе
            Gizmos.color = isOccupied ? Color.red : (isActive ? Color.green : Color.gray);
            Gizmos.DrawWireSphere(transform.position, snapRadius);
            
            // Направление snap-point
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, snapDirection * snapRadius);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            // Обработка входа в зону snap-point
            SnapPoint otherSnapPoint = other.GetComponent<SnapPoint>();
            if (otherSnapPoint != null && CanSnapTo(otherSnapPoint))
            {
                SetHighlighted(true);
                otherSnapPoint.SetHighlighted(true);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            // Обработка выхода из зоны snap-point
            SnapPoint otherSnapPoint = other.GetComponent<SnapPoint>();
            if (otherSnapPoint != null)
            {
                SetHighlighted(false);
                otherSnapPoint.SetHighlighted(false);
            }
        }
    }
}
