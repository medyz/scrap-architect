using UnityEngine;
using System.Collections.Generic;

namespace ScrapArchitect.Physics
{
    /// <summary>
    /// Контроллер детали - управляет физикой, соединениями и взаимодействием
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class PartController : MonoBehaviour
    {
        [Header("Part Settings")]
        public string partName = "Unknown Part";
        public PartType partType = PartType.Block;
        public float mass = 1f;
        public float maxHealth = 100f;
        
        [Header("Physics")]
        public bool useGravity = true;
        public bool isKinematic = false;
        public float drag = 0.5f;
        public float angularDrag = 0.05f;
        
        [Header("Connections")]
        public List<ConnectionPoint> connectionPoints = new List<ConnectionPoint>();
        public List<Joint> connectedJoints = new List<Joint>();
        public int maxConnections = 4;
        
        [Header("Snap Settings")]
        public float snapDistance = 0.5f;
        public float snapAngle = 15f;
        public LayerMask snapLayerMask = -1;
        public bool useSnapPoints = true;
        public bool autoSnap = true;
        
        [Header("State")]
        public bool isSelected = false;
        public bool isDragging = false;
        public bool isConnected = false;
        public float currentHealth;
        
        // Components
        private Rigidbody rb;
        private Collider col;
        private Renderer rend;
        private PartAttacher partAttacher;
        
        // Events
        public System.Action<PartController> OnPartSelected;
        public System.Action<PartController> OnPartDeselected;
        public System.Action<PartController, PartController> OnPartConnected;
        public System.Action<PartController, PartController> OnPartDisconnected;
        public System.Action<PartController> OnPartDestroyed;
        
        private void Awake()
        {
            InitializeComponents();
            InitializePart();
        }
        
        private void Start()
        {
            // Подписываемся на события GameManager
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.OnGameModeChanged += OnGameModeChanged;
            }
        }
        
        private void OnDestroy()
        {
            // Отписываемся от событий
            if (Core.GameManager.Instance != null)
            {
                Core.GameManager.Instance.OnGameModeChanged -= OnGameModeChanged;
            }
        }
        
        /// <summary>
        /// Инициализация компонентов
        /// </summary>
        private void InitializeComponents()
        {
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();
            rend = GetComponent<Renderer>();
            partAttacher = GetComponent<PartAttacher>();
            
            if (rb == null)
            {
                Debug.LogError($"Rigidbody not found on {gameObject.name}");
                return;
            }
            
            if (col == null)
            {
                Debug.LogError($"Collider not found on {gameObject.name}");
                return;
            }
            
            // Добавить PartAttacher если его нет
            if (partAttacher == null && useSnapPoints)
            {
                partAttacher = gameObject.AddComponent<PartAttacher>();
            }
        }
        
        /// <summary>
        /// Инициализация детали
        /// </summary>
        private void InitializePart()
        {
            // Настройка Rigidbody
            rb.mass = mass;
            rb.useGravity = useGravity;
            rb.isKinematic = isKinematic;
            rb.drag = drag;
            rb.angularDrag = angularDrag;
            
            // Настройка здоровья
            currentHealth = maxHealth;
            
            // Создание точек соединения если их нет
            if (connectionPoints.Count == 0)
            {
                CreateDefaultConnectionPoints();
            }
            
            Debug.Log($"Part {partName} initialized");
        }
        
        /// <summary>
        /// Создание точек соединения по умолчанию
        /// </summary>
        private void CreateDefaultConnectionPoints()
        {
            // Создаем точки соединения на гранях куба
            Vector3[] directions = {
                Vector3.forward, Vector3.back,
                Vector3.right, Vector3.left,
                Vector3.up, Vector3.down
            };
            
            for (int i = 0; i < directions.Length && i < maxConnections; i++)
            {
                Vector3 position = transform.position + directions[i] * 0.5f;
                ConnectionPoint point = new ConnectionPoint
                {
                    id = i,
                    position = position,
                    direction = directions[i],
                    isOccupied = false
                };
                
                connectionPoints.Add(point);
            }
        }
        
        /// <summary>
        /// Обработка изменения режима игры
        /// </summary>
        private void OnGameModeChanged(Core.GameMode newMode)
        {
            switch (newMode)
            {
                case Core.GameMode.Build:
                    EnableBuildMode();
                    break;
                    
                case Core.GameMode.Test:
                    EnableTestMode();
                    break;
                    
                case Core.GameMode.Sandbox:
                    EnableSandboxMode();
                    break;
            }
        }
        
        /// <summary>
        /// Включение режима строительства
        /// </summary>
        private void EnableBuildMode()
        {
            rb.isKinematic = true;
            isDragging = false;
            isSelected = false;
        }
        
        /// <summary>
        /// Включение режима тестирования
        /// </summary>
        private void EnableTestMode()
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        
        /// <summary>
        /// Включение режима песочницы
        /// </summary>
        private void EnableSandboxMode()
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        
        /// <summary>
        /// Выбор детали
        /// </summary>
        public void Select()
        {
            if (!isSelected)
            {
                isSelected = true;
                OnPartSelected?.Invoke(this);
                
                // Визуальная обратная связь
                if (rend != null)
                {
                    rend.material.color = Color.yellow;
                }
                
                Debug.Log($"Part {partName} selected");
            }
        }
        
        /// <summary>
        /// Отмена выбора детали
        /// </summary>
        public void Deselect()
        {
            if (isSelected)
            {
                isSelected = false;
                OnPartDeselected?.Invoke(this);
                
                // Возвращаем нормальный цвет
                if (rend != null)
                {
                    rend.material.color = Color.white;
                }
                
                Debug.Log($"Part {partName} deselected");
            }
        }
        
        /// <summary>
        /// Начало перетаскивания
        /// </summary>
        public void StartDragging()
        {
            if (!isDragging)
            {
                isDragging = true;
                rb.isKinematic = true;
                
                Debug.Log($"Started dragging {partName}");
            }
        }
        
        /// <summary>
        /// Остановка перетаскивания
        /// </summary>
        public void StopDragging()
        {
            if (isDragging)
            {
                isDragging = false;
                
                // Проверяем режим игры
                if (Core.GameManager.Instance != null && 
                    Core.GameManager.Instance.currentGameMode == Core.GameMode.Build)
                {
                    rb.isKinematic = true;
                }
                else
                {
                    rb.isKinematic = false;
                }
                
                Debug.Log($"Stopped dragging {partName}");
            }
        }
        
        /// <summary>
        /// Перемещение детали
        /// </summary>
        public void MoveTo(Vector3 position)
        {
            if (isDragging)
            {
                transform.position = position;
            }
        }
        
        /// <summary>
        /// Поворот детали
        /// </summary>
        public void Rotate(Vector3 rotation)
        {
            if (isDragging)
            {
                transform.rotation = Quaternion.Euler(rotation);
            }
        }
        
        /// <summary>
        /// Поиск ближайшей точки соединения
        /// </summary>
        public ConnectionPoint FindNearestConnectionPoint(Vector3 position)
        {
            ConnectionPoint nearest = null;
            float minDistance = float.MaxValue;
            
            foreach (var point in connectionPoints)
            {
                if (!point.isOccupied)
                {
                    float distance = Vector3.Distance(point.position, position);
                    if (distance < minDistance && distance <= snapDistance)
                    {
                        minDistance = distance;
                        nearest = point;
                    }
                }
            }
            
            return nearest;
        }
        
        /// <summary>
        /// Соединение с другой деталью
        /// </summary>
        public bool ConnectTo(PartController otherPart, ConnectionPoint thisPoint, ConnectionPoint otherPoint)
        {
            if (thisPoint == null || otherPoint == null)
                return false;
                
            if (thisPoint.isOccupied || otherPoint.isOccupied)
                return false;
                
            if (connectedJoints.Count >= maxConnections)
                return false;
            
            // Создаем соединение
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = otherPart.rb;
            joint.breakForce = 1000f;
            joint.breakTorque = 1000f;
            
            // Отмечаем точки как занятые
            thisPoint.isOccupied = true;
            otherPoint.isOccupied = true;
            
            // Добавляем в списки
            connectedJoints.Add(joint);
            otherPart.connectedJoints.Add(joint);
            
            // Обновляем состояние
            isConnected = true;
            otherPart.isConnected = true;
            
            // Вызываем события
            OnPartConnected?.Invoke(this, otherPart);
            otherPart.OnPartConnected?.Invoke(otherPart, this);
            
            Debug.Log($"Connected {partName} to {otherPart.partName}");
            return true;
        }
        
        /// <summary>
        /// Отсоединение от другой детали
        /// </summary>
        public void DisconnectFrom(PartController otherPart)
        {
            // Находим и удаляем соединение
            Joint jointToRemove = null;
            foreach (var joint in connectedJoints)
            {
                if (joint.connectedBody == otherPart.rb)
                {
                    jointToRemove = joint;
                    break;
                }
            }
            
            if (jointToRemove != null)
            {
                connectedJoints.Remove(jointToRemove);
                otherPart.connectedJoints.Remove(jointToRemove);
                DestroyImmediate(jointToRemove);
                
                // Обновляем состояние
                if (connectedJoints.Count == 0)
                {
                    isConnected = false;
                }
                if (otherPart.connectedJoints.Count == 0)
                {
                    otherPart.isConnected = false;
                }
                
                // Вызываем события
                OnPartDisconnected?.Invoke(this, otherPart);
                otherPart.OnPartDisconnected?.Invoke(otherPart, this);
                
                Debug.Log($"Disconnected {partName} from {otherPart.partName}");
            }
        }
        
        /// <summary>
        /// Получение урона
        /// </summary>
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            
            if (currentHealth <= 0)
            {
                DestroyPart();
            }
        }
        
        /// <summary>
        /// Уничтожение детали
        /// </summary>
        public void DestroyPart()
        {
            // Отсоединяем все соединения
            foreach (var joint in connectedJoints.ToArray())
            {
                if (joint != null && joint.connectedBody != null)
                {
                    PartController otherPart = joint.connectedBody.GetComponent<PartController>();
                    if (otherPart != null)
                    {
                        DisconnectFrom(otherPart);
                    }
                }
            }
            
            // Вызываем событие
            OnPartDestroyed?.Invoke(this);
            
            Debug.Log($"Part {partName} destroyed");
            
            // Уничтожаем объект
            Destroy(gameObject);
        }
        
        /// <summary>
        /// Получение информации о детали
        /// </summary>
        public string GetPartInfo()
        {
            return $"Name: {partName}\nType: {partType}\nHealth: {currentHealth}/{maxHealth}\nConnections: {connectedJoints.Count}/{maxConnections}";
        }
        
        #region Snap Points Integration
        
        /// <summary>
        /// Начать перетаскивание детали
        /// </summary>
        public void StartDragging()
        {
            isDragging = true;
            if (partAttacher != null)
            {
                partAttacher.StartDragging();
            }
        }
        
        /// <summary>
        /// Остановить перетаскивание детали
        /// </summary>
        public void StopDragging()
        {
            isDragging = false;
            if (partAttacher != null)
            {
                partAttacher.StopDragging();
            }
        }
        
        /// <summary>
        /// Обновить позицию при перетаскивании
        /// </summary>
        public void UpdateDragPosition(Vector3 position, Quaternion rotation)
        {
            if (partAttacher != null)
            {
                partAttacher.UpdateDragPosition(position, rotation);
            }
        }
        
        /// <summary>
        /// Попытаться привязать к snap-point
        /// </summary>
        public bool TryAttachToSnapPoint(SnapPoint snapPoint)
        {
            if (partAttacher != null)
            {
                return partAttacher.TryAttachToSnapPoint(snapPoint);
            }
            return false;
        }
        
        /// <summary>
        /// Отвязать от snap-point
        /// </summary>
        public void DetachFromSnapPoint(SnapPoint snapPoint)
        {
            if (partAttacher != null)
            {
                partAttacher.DetachFromSnapPoint(snapPoint);
            }
        }
        
        /// <summary>
        /// Отвязать все соединения
        /// </summary>
        public void DetachAll()
        {
            if (partAttacher != null)
            {
                partAttacher.DetachAll();
            }
        }
        
        /// <summary>
        /// Получить snap-points детали
        /// </summary>
        public SnapPoint[] GetSnapPoints()
        {
            if (partAttacher != null)
            {
                return partAttacher.GetSnapPoints();
            }
            return new SnapPoint[0];
        }
        
        /// <summary>
        /// Получить количество подключенных snap-points
        /// </summary>
        public int GetConnectedSnapPointsCount()
        {
            if (partAttacher != null)
            {
                return partAttacher.GetConnectedSnapPointsCount();
            }
            return 0;
        }
        
        /// <summary>
        /// Проверить, можно ли привязать к snap-point
        /// </summary>
        public bool CanAttachTo(SnapPoint snapPoint)
        {
            if (partAttacher != null)
            {
                return partAttacher.CanAttachTo(snapPoint);
            }
            return false;
        }
        
        #endregion
    }
    
    /// <summary>
    /// Точка соединения
    /// </summary>
    [System.Serializable]
    public class ConnectionPoint
    {
        public int id;
        public Vector3 position;
        public Vector3 direction;
        public bool isOccupied;
    }
    
    /// <summary>
    /// Типы деталей
    /// </summary>
    public enum PartType
    {
        Block,
        Beam,
        Wheel,
        Motor,
        Seat,
        Tool,
        Connector
    }
}
