using UnityEngine;
using System;
using ScrapArchitect.Physics;
using ScrapArchitect.Core;
using System.Collections.Generic;

namespace ScrapArchitect.Controls
{
    /// <summary>
    /// Контроллер для манипуляций с деталями - выбор, перемещение, вращение
    /// </summary>
    public class PartManipulator : MonoBehaviour
    {
        [Header("Selection Settings")]
        public LayerMask selectableLayers = -1;
        public float selectionRadius = 0.5f;
        public Material selectionMaterial;
        public Color selectionColor = Color.yellow;
        
        [Header("Manipulation Settings")]
        public float moveSpeed = 10f;
        public float rotateSpeed = 90f;
        public float snapDistance = 1f;
        public bool enableSnapping = true;
        
        [Header("Input Settings")]
        public KeyCode selectKey = KeyCode.Mouse0;
        public KeyCode deleteKey = KeyCode.Delete;
        public KeyCode duplicateKey = KeyCode.D;
        public KeyCode rotateKey = KeyCode.R;
        
        // Приватные переменные
        private PartController selectedPart;
        private PartController hoveredPart;
        private bool isDragging = false;
        private Vector3 dragOffset;
        private Vector3 originalPosition;
        private Quaternion originalRotation;
        private CameraController cameraController;
        
        // События
        public Action<PartController> OnPartSelected;
        public Action<PartController> OnPartDeselected;
        public Action<PartController> OnPartDeleted;
        
        private void Start()
        {
            InitializeManipulator();
        }
        
        private void Update()
        {
            if (Core.GameManager.Instance != null && 
                Core.GameManager.Instance.currentGameState != GameState.Paused)
            {
                HandleInput();
                UpdateHover();
            }
        }
        
        /// <summary>
        /// Инициализация манипулятора
        /// </summary>
        private void InitializeManipulator()
        {
            cameraController = FindObjectOfType<CameraController>();
            
            if (selectionMaterial == null)
            {
                selectionMaterial = new Material(Shader.Find("Standard"));
                selectionMaterial.color = selectionColor;
            }
            
            Debug.Log("PartManipulator initialized");
        }
        
        /// <summary>
        /// Обработка ввода
        /// </summary>
        private void HandleInput()
        {
            // Выбор детали
            if (Input.GetKeyDown(selectKey))
            {
                if (isDragging)
                {
                    EndDrag();
                }
                else
                {
                    SelectPartUnderCursor();
                }
            }
            
            // Начало перетаскивания
            if (Input.GetKey(selectKey) && selectedPart != null && !isDragging)
            {
                StartDrag();
            }
            
            // Удаление детали
            if (Input.GetKeyDown(deleteKey) && selectedPart != null)
            {
                DeleteSelectedPart();
            }
            
            // Дублирование детали
            if (Input.GetKeyDown(duplicateKey) && selectedPart != null)
            {
                DuplicateSelectedPart();
            }
            
            // Вращение детали
            if (Input.GetKeyDown(rotateKey) && selectedPart != null)
            {
                RotateSelectedPart();
            }
            
            // Обновление перетаскивания
            if (isDragging)
            {
                UpdateDrag();
            }
        }
        
        /// <summary>
        /// Обновление наведения мыши
        /// </summary>
        private void UpdateHover()
        {
            PartController newHoveredPart = GetPartUnderCursor();
            
            if (newHoveredPart != hoveredPart)
            {
                if (hoveredPart != null && hoveredPart != selectedPart)
                {
                    SetPartHighlight(hoveredPart, false);
                }
                
                hoveredPart = newHoveredPart;
                
                if (hoveredPart != null && hoveredPart != selectedPart)
                {
                    SetPartHighlight(hoveredPart, true);
                }
            }
        }
        
        /// <summary>
        /// Выбор детали под курсором
        /// </summary>
        private void SelectPartUnderCursor()
        {
            PartController part = GetPartUnderCursor();
            
            if (part != null)
            {
                SelectPart(part);
            }
            else
            {
                DeselectPart();
            }
        }
        
        /// <summary>
        /// Получить деталь под курсором
        /// </summary>
        private PartController GetPartUnderCursor()
        {
            if (cameraController == null)
                return null;
            
            Ray ray = cameraController.GetScreenRay(Input.mousePosition);
            RaycastHit hit;
            
            if (UnityEngine.Physics.Raycast(ray, out hit, 100f, selectableLayers))
            {
                return hit.collider.GetComponent<PartController>();
            }
            
            return null;
        }
        
        /// <summary>
        /// Выбрать деталь
        /// </summary>
        public void SelectPart(PartController part)
        {
            if (selectedPart != null)
            {
                DeselectPart();
            }
            
            selectedPart = part;
            
            if (selectedPart != null)
            {
                SetPartHighlight(selectedPart, true);
                selectedPart.Select();
                OnPartSelected?.Invoke(selectedPart);
                
                Debug.Log($"Selected part: {selectedPart.partName}");
            }
        }
        
        /// <summary>
        /// Снять выбор с детали
        /// </summary>
        public void DeselectPart()
        {
            if (selectedPart != null)
            {
                SetPartHighlight(selectedPart, false);
                selectedPart.Deselect();
                OnPartDeselected?.Invoke(selectedPart);
                
                Debug.Log($"Deselected part: {selectedPart.partName}");
                selectedPart = null;
            }
        }
        
        /// <summary>
        /// Установить подсветку детали
        /// </summary>
        private void SetPartHighlight(PartController part, bool highlight)
        {
            Renderer renderer = part.GetComponent<Renderer>();
            if (renderer != null)
            {
                if (highlight)
                {
                    renderer.material = selectionMaterial;
                }
                else
                {
                    // Восстанавливаем оригинальный материал
                    renderer.material = part.GetComponent<PartBase>()?.defaultMaterial ?? renderer.material;
                }
            }
        }
        
        /// <summary>
        /// Начать перетаскивание
        /// </summary>
        private void StartDrag()
        {
            if (selectedPart == null)
                return;
            
            isDragging = true;
            originalPosition = selectedPart.transform.position;
            originalRotation = selectedPart.transform.rotation;
            
            // Вычисляем смещение от курсора
            Vector3 worldPoint = cameraController.GetWorldPointUnderCursor();
            dragOffset = selectedPart.transform.position - worldPoint;
            
            selectedPart.StartDragging();
            
            Debug.Log("Started dragging part");
        }
        
        /// <summary>
        /// Обновление перетаскивания
        /// </summary>
        private void UpdateDrag()
        {
            if (!isDragging || selectedPart == null)
                return;
            
            Vector3 worldPoint = cameraController.GetWorldPointUnderCursor();
            Vector3 targetPosition = worldPoint + dragOffset;
            
            // Применяем снэппинг
            if (enableSnapping)
            {
                targetPosition = SnapToGrid(targetPosition);
            }
            
            selectedPart.MoveTo(targetPosition);
        }
        
        /// <summary>
        /// Завершить перетаскивание
        /// </summary>
        private void EndDrag()
        {
            if (!isDragging || selectedPart == null)
                return;
            
            isDragging = false;
            selectedPart.StopDragging();
            
            // Проверяем соединения
            CheckConnections();
            
            Debug.Log("Ended dragging part");
        }
        
        /// <summary>
        /// Снэппинг к сетке
        /// </summary>
        private Vector3 SnapToGrid(Vector3 position)
        {
            float gridSize = 1f;
            
            position.x = Mathf.Round(position.x / gridSize) * gridSize;
            position.y = Mathf.Round(position.y / gridSize) * gridSize;
            position.z = Mathf.Round(position.z / gridSize) * gridSize;
            
            return position;
        }
        
        /// <summary>
        /// Проверка соединений
        /// </summary>
        private void CheckConnections()
        {
            if (selectedPart == null)
                return;
            
            // Ищем ближайшие детали для соединения
            Collider[] nearbyColliders = Physics.OverlapSphere(selectedPart.transform.position, snapDistance);
            
            foreach (var collider in nearbyColliders)
            {
                PartController otherPart = collider.GetComponent<PartController>();
                
                if (otherPart != null && otherPart != selectedPart)
                {
                    // Проверяем возможность соединения
                    TryConnectParts(selectedPart, otherPart);
                }
            }
        }
        
        /// <summary>
        /// Попытка соединения деталей
        /// </summary>
        private void TryConnectParts(PartController part1, PartController part2)
        {
            // Находим ближайшие точки соединения
            var point1 = part1.FindNearestConnectionPoint(part2.transform.position);
            var point2 = part2.FindNearestConnectionPoint(part1.transform.position);
            
            if (point1 != null && point2 != null && !point1.isOccupied && !point2.isOccupied)
            {
                float distance = Vector3.Distance(
                    part1.transform.TransformPoint(point1.position),
                    part2.transform.TransformPoint(point2.position)
                );
                
                if (distance <= snapDistance)
                {
                    part1.ConnectTo(part2, point1, point2);
                    Debug.Log($"Connected {part1.partName} to {part2.partName}");
                }
            }
        }
        
        /// <summary>
        /// Удалить выбранную деталь
        /// </summary>
        private void DeleteSelectedPart()
        {
            if (selectedPart == null)
                return;
            
            PartController partToDelete = selectedPart;
            DeselectPart();
            
            partToDelete.DestroyPart();
            OnPartDeleted?.Invoke(partToDelete);
            
            Debug.Log($"Deleted part: {partToDelete.partName}");
        }
        
        /// <summary>
        /// Дублировать выбранную деталь
        /// </summary>
        private void DuplicateSelectedPart()
        {
            if (selectedPart == null)
                return;
            
            Vector3 newPosition = selectedPart.transform.position + Vector3.right * 2f;
            GameObject duplicatedObject = Instantiate(selectedPart.gameObject, newPosition, selectedPart.transform.rotation);
            
            PartController duplicatedPart = duplicatedObject.GetComponent<PartController>();
            if (duplicatedPart != null)
            {
                SelectPart(duplicatedPart);
                Debug.Log($"Duplicated part: {duplicatedPart.partName}");
            }
        }
        
        /// <summary>
        /// Вратить выбранную деталь
        /// </summary>
        private void RotateSelectedPart()
        {
            if (selectedPart == null)
                return;
            
            Vector3 currentRotation = selectedPart.transform.eulerAngles;
            currentRotation.y += 90f;
            selectedPart.Rotate(currentRotation);
            
            Debug.Log($"Rotated part: {selectedPart.partName}");
        }
        
        /// <summary>
        /// Получить выбранную деталь
        /// </summary>
        public PartController GetSelectedPart()
        {
            return selectedPart;
        }
        
        /// <summary>
        /// Очистить все детали
        /// </summary>
        public void ClearAllParts()
        {
            DeselectPart();
            
            PartController[] allParts = FindObjectsOfType<PartController>();
            foreach (var part in allParts)
            {
                part.DestroyPart();
            }
            
            Debug.Log("Cleared all parts");
        }
    }
}
