using UnityEngine;
using System.Collections.Generic;

namespace ScrapArchitect.Physics
{
    public class PartAttacher : MonoBehaviour
    {
        [Header("Attacher Settings")]
        public float snapDistance = 1.0f;
        public float snapAngle = 15f;
        public bool autoSnap = true;
        public bool showSnapPreview = true;
        
        [Header("Visual Settings")]
        public Material snapPreviewMaterial;
        public Material validSnapMaterial;
        public Material invalidSnapMaterial;
        public GameObject snapPreview;
        
        [Header("Audio Settings")]
        public AudioClip snapSound;
        public AudioClip unsnapSound;
        public AudioClip snapErrorSound;
        
        private PartController currentPart;
        private SnapPoint[] snapPoints;
        private SnapPoint[] nearbySnapPoints;
        private SnapPoint bestSnapPoint;
        private bool isDragging = false;
        private GameObject previewObject;
        private Renderer previewRenderer;
        
        private void Start()
        {
            currentPart = GetComponent<PartController>();
            snapPoints = GetComponentsInChildren<SnapPoint>();
            
            if (snapPreview != null)
            {
                CreateSnapPreview();
            }
        }
        
        private void CreateSnapPreview()
        {
            previewObject = Instantiate(snapPreview, Vector3.zero, Quaternion.identity);
            previewObject.SetActive(false);
            previewRenderer = previewObject.GetComponent<Renderer>();
            
            if (previewRenderer != null && snapPreviewMaterial != null)
            {
                previewRenderer.material = snapPreviewMaterial;
            }
        }
        
        public void StartDragging()
        {
            isDragging = true;
            
            if (previewObject != null)
            {
                previewObject.SetActive(true);
            }
            
            Debug.Log($"Started dragging part: {currentPart?.partName}");
        }
        
        public void StopDragging()
        {
            isDragging = false;
            
            if (previewObject != null)
            {
                previewObject.SetActive(false);
            }
            
            // Попытаться выполнить привязку
            if (bestSnapPoint != null && autoSnap)
            {
                TryAttachToSnapPoint(bestSnapPoint);
            }
            
            bestSnapPoint = null;
            Debug.Log($"Stopped dragging part: {currentPart?.partName}");
        }
        
        public void UpdateDragPosition(Vector3 position, Quaternion rotation)
        {
            if (!isDragging)
            {
                return;
            }
            
            // Обновить позицию перетаскиваемой детали
            transform.position = position;
            transform.rotation = rotation;
            
            // Найти ближайшие snap-points
            FindNearbySnapPoints();
            
            // Обновить превью привязки
            UpdateSnapPreview();
        }
        
        private void FindNearbySnapPoints()
        {
            nearbySnapPoints = FindObjectsOfType<SnapPoint>();
            bestSnapPoint = null;
            float bestDistance = float.MaxValue;
            
            foreach (SnapPoint snapPoint in nearbySnapPoints)
            {
                if (snapPoint == null || snapPoint.GetParentPart() == currentPart)
                {
                    continue;
                }
                
                float distance = Vector3.Distance(transform.position, snapPoint.GetSnapPosition());
                if (distance <= snapDistance)
                {
                    // Проверить совместимость с нашими snap-points
                    SnapPoint compatibleSnapPoint = FindCompatibleSnapPoint(snapPoint);
                    if (compatibleSnapPoint != null)
                    {
                        if (distance < bestDistance)
                        {
                            bestDistance = distance;
                            bestSnapPoint = snapPoint;
                        }
                    }
                }
            }
        }
        
        private SnapPoint FindCompatibleSnapPoint(SnapPoint targetSnapPoint)
        {
            foreach (SnapPoint snapPoint in snapPoints)
            {
                if (snapPoint != null && snapPoint.CanSnapTo(targetSnapPoint))
                {
                    return snapPoint;
                }
            }
            return null;
        }
        
        private void UpdateSnapPreview()
        {
            if (previewObject == null || !showSnapPreview)
            {
                return;
            }
            
            if (bestSnapPoint != null)
            {
                // Показать превью привязки
                previewObject.SetActive(true);
                previewObject.transform.position = bestSnapPoint.GetSnapPosition();
                previewObject.transform.rotation = bestSnapPoint.GetSnapRotation();
                
                // Обновить материал в зависимости от валидности
                if (previewRenderer != null)
                {
                    if (IsSnapValid(bestSnapPoint))
                    {
                        previewRenderer.material = validSnapMaterial;
                    }
                    else
                    {
                        previewRenderer.material = invalidSnapMaterial;
                    }
                }
            }
            else
            {
                previewObject.SetActive(false);
            }
        }
        
        private bool IsSnapValid(SnapPoint snapPoint)
        {
            if (snapPoint == null)
            {
                return false;
            }
            
            // Проверить расстояние
            float distance = Vector3.Distance(transform.position, snapPoint.GetSnapPosition());
            if (distance > snapDistance)
            {
                return false;
            }
            
            // Проверить угол
            SnapPoint compatibleSnapPoint = FindCompatibleSnapPoint(snapPoint);
            if (compatibleSnapPoint == null)
            {
                return false;
            }
            
            float angle = Vector3.Angle(compatibleSnapPoint.GetSnapDirection(), snapPoint.GetSnapDirection());
            if (angle > snapAngle)
            {
                return false;
            }
            
            return true;
        }
        
        public bool TryAttachToSnapPoint(SnapPoint targetSnapPoint)
        {
            if (targetSnapPoint == null || !IsSnapValid(targetSnapPoint))
            {
                PlaySnapErrorSound();
                return false;
            }
            
            SnapPoint compatibleSnapPoint = FindCompatibleSnapPoint(targetSnapPoint);
            if (compatibleSnapPoint == null)
            {
                PlaySnapErrorSound();
                return false;
            }
            
            // Выполнить привязку
            if (compatibleSnapPoint.TrySnapTo(targetSnapPoint))
            {
                // Позиционировать деталь точно на snap-point
                Vector3 snapPosition = targetSnapPoint.GetSnapPosition();
                Quaternion snapRotation = targetSnapPoint.GetSnapRotation();
                
                transform.position = snapPosition;
                transform.rotation = snapRotation;
                
                PlaySnapSound();
                
                Debug.Log($"Part attached: {currentPart?.partName} to {targetSnapPoint.GetParentPart()?.partName}");
                return true;
            }
            
            PlaySnapErrorSound();
            return false;
        }
        
        public void DetachFromSnapPoint(SnapPoint snapPoint)
        {
            if (snapPoint == null)
            {
                return;
            }
            
            SnapPoint connectedSnapPoint = snapPoint.GetConnectedSnapPoint();
            if (connectedSnapPoint != null)
            {
                connectedSnapPoint.Unsnap();
                PlayUnsnapSound();
                
                Debug.Log($"Part detached: {currentPart?.partName}");
            }
        }
        
        public void DetachAll()
        {
            foreach (SnapPoint snapPoint in snapPoints)
            {
                if (snapPoint != null && snapPoint.IsOccupied())
                {
                    snapPoint.Unsnap();
                }
            }
            
            PlayUnsnapSound();
            Debug.Log($"All connections detached from: {currentPart?.partName}");
        }
        
        public SnapPoint[] GetSnapPoints()
        {
            return snapPoints;
        }
        
        public SnapPoint GetBestSnapPoint()
        {
            return bestSnapPoint;
        }
        
        public bool IsDragging()
        {
            return isDragging;
        }
        
        public bool CanAttachTo(SnapPoint snapPoint)
        {
            return IsSnapValid(snapPoint);
        }
        
        public int GetConnectedSnapPointsCount()
        {
            int count = 0;
            foreach (SnapPoint snapPoint in snapPoints)
            {
                if (snapPoint != null && snapPoint.IsOccupied())
                {
                    count++;
                }
            }
            return count;
        }
        
        public List<SnapPoint> GetConnectedSnapPoints()
        {
            List<SnapPoint> connected = new List<SnapPoint>();
            foreach (SnapPoint snapPoint in snapPoints)
            {
                if (snapPoint != null && snapPoint.IsOccupied())
                {
                    connected.Add(snapPoint);
                }
            }
            return connected;
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
        
        private void PlaySnapErrorSound()
        {
            if (snapErrorSound != null)
            {
                AudioSource.PlayClipAtPoint(snapErrorSound, transform.position);
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            // Визуализация зоны привязки в редакторе
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, snapDistance);
            
            // Визуализация snap-points
            if (snapPoints != null)
            {
                foreach (SnapPoint snapPoint in snapPoints)
                {
                    if (snapPoint != null)
                    {
                        Gizmos.color = snapPoint.IsOccupied() ? Color.red : Color.green;
                        Gizmos.DrawWireSphere(snapPoint.transform.position, snapPoint.GetSnapRadius());
                    }
                }
            }
        }
        
        private void OnDestroy()
        {
            if (previewObject != null)
            {
                DestroyImmediate(previewObject);
            }
        }
    }
}
