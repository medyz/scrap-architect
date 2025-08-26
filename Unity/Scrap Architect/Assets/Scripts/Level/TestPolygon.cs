using UnityEngine;
using ScrapArchitect.Core;
using ScrapArchitect.UI;
using System.Collections.Generic;

namespace ScrapArchitect.Level
{
    /// <summary>
    /// Тестовый полигон - основная сцена для тестирования машин
    /// </summary>
    public class TestPolygon : MonoBehaviour
    {
        [Header("Polygon Settings")]
        public Vector3 polygonSize = new Vector3(50f, 10f, 50f);
        public Material groundMaterial;
        public Material obstacleMaterial;
        
        [Header("Goals")]
        public List<TestGoal> goals = new List<TestGoal>();
        public Transform startPoint;
        public Transform finishPoint;
        
        [Header("Obstacles")]
        public List<GameObject> obstacles = new List<GameObject>();
        public bool generateRandomObstacles = true;
        public int obstacleCount = 10;
        
        [Header("Scoring")]
        public float timeLimit = 60f;
        public float distanceMultiplier = 1f;
        public float speedMultiplier = 2f;
        public float efficiencyMultiplier = 1.5f;
        
        // Приватные переменные
        private float currentTime = 0f;
        private bool isTestActive = false;
        private bool isTestCompleted = false;
        private Vector3 startPosition;
        private float bestDistance = 0f;
        private float currentScore = 0f;
        
        // События
        public System.Action<float> OnTimeChanged;
        public System.Action<float> OnScoreChanged;
        public System.Action<bool> OnTestStateChanged;
        public System.Action OnTestCompleted;
        public System.Action OnTestFailed;
        
        private void Start()
        {
            InitializePolygon();
        }
        
        private void Update()
        {
            if (isTestActive && !isTestCompleted)
            {
                UpdateTest();
            }
        }
        
        /// <summary>
        /// Инициализация полигона
        /// </summary>
        private void InitializePolygon()
        {
            CreateGround();
            CreateBoundaries();
            CreateStartFinishPoints();
            
            if (generateRandomObstacles)
            {
                GenerateObstacles();
            }
            
            CreateGoals();
            
            Debug.Log("Test polygon initialized");
        }
        
        /// <summary>
        /// Создание земли
        /// </summary>
        private void CreateGround()
        {
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.SetParent(transform);
            ground.transform.localScale = new Vector3(polygonSize.x / 10f, 1f, polygonSize.z / 10f);
            ground.transform.position = Vector3.zero;
            
            // Настройка материала
            if (groundMaterial != null)
            {
                ground.GetComponent<Renderer>().material = groundMaterial;
            }
            
            // Настройка коллайдера
            ground.GetComponent<Collider>().isTrigger = false;
        }
        
        /// <summary>
        /// Создание границ полигона
        /// </summary>
        private void CreateBoundaries()
        {
            float halfWidth = polygonSize.x / 2f;
            float halfLength = polygonSize.z / 2f;
            float height = polygonSize.y;
            
            // Создаем стены по периметру
            CreateWall("Wall_North", new Vector3(0, height/2, halfLength), new Vector3(polygonSize.x, height, 1));
            CreateWall("Wall_South", new Vector3(0, height/2, -halfLength), new Vector3(polygonSize.x, height, 1));
            CreateWall("Wall_East", new Vector3(halfWidth, height/2, 0), new Vector3(1, height, polygonSize.z));
            CreateWall("Wall_West", new Vector3(-halfWidth, height/2, 0), new Vector3(1, height, polygonSize.z));
        }
        
        /// <summary>
        /// Создание стены
        /// </summary>
        private void CreateWall(string name, Vector3 position, Vector3 scale)
        {
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = name;
            wall.transform.SetParent(transform);
            wall.transform.position = position;
            wall.transform.localScale = scale;
            
            if (obstacleMaterial != null)
            {
                wall.GetComponent<Renderer>().material = obstacleMaterial;
            }
            
            obstacles.Add(wall);
        }
        
        /// <summary>
        /// Создание стартовой и финишной точек
        /// </summary>
        private void CreateStartFinishPoints()
        {
            // Стартовая точка
            if (startPoint == null)
            {
                GameObject startObj = new GameObject("StartPoint");
                startObj.transform.SetParent(transform);
                startObj.transform.position = new Vector3(-polygonSize.x/4, 0.5f, -polygonSize.z/4);
                startPoint = startObj.transform;
                
                // Визуальный маркер старта
                CreateMarker(startObj, Color.green, "Start");
            }
            
            // Финишная точка
            if (finishPoint == null)
            {
                GameObject finishObj = new GameObject("FinishPoint");
                finishObj.transform.SetParent(transform);
                finishObj.transform.position = new Vector3(polygonSize.x/4, 0.5f, polygonSize.z/4);
                finishPoint = finishObj.transform;
                
                // Визуальный маркер финиша
                CreateMarker(finishObj, Color.red, "Finish");
            }
            
            startPosition = startPoint.position;
        }
        
        /// <summary>
        /// Создание визуального маркера
        /// </summary>
        private void CreateMarker(GameObject parent, Color color, string label)
        {
            GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            marker.transform.SetParent(parent.transform);
            marker.transform.localPosition = Vector3.zero;
            marker.transform.localScale = new Vector3(2f, 0.1f, 2f);
            
            Renderer renderer = marker.GetComponent<Renderer>();
            Material mat = new Material(Shader.Find("Standard"));
            mat.color = color;
            renderer.material = mat;
            
            // Добавляем текст
            GameObject textObj = new GameObject("Label");
            textObj.transform.SetParent(parent.transform);
            textObj.transform.localPosition = new Vector3(0, 1f, 0);
            
            // TODO: Добавить TextMesh для отображения текста
        }
        
        /// <summary>
        /// Генерация случайных препятствий
        /// </summary>
        private void GenerateObstacles()
        {
            for (int i = 0; i < obstacleCount; i++)
            {
                CreateRandomObstacle();
            }
        }
        
        /// <summary>
        /// Создание случайного препятствия
        /// </summary>
        private void CreateRandomObstacle()
        {
            float x = Random.Range(-polygonSize.x/3, polygonSize.x/3);
            float z = Random.Range(-polygonSize.z/3, polygonSize.z/3);
            float height = Random.Range(1f, 3f);
            float width = Random.Range(1f, 3f);
            
            GameObject obstacle = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obstacle.name = $"Obstacle_{obstacles.Count}";
            obstacle.transform.SetParent(transform);
            obstacle.transform.position = new Vector3(x, height/2, z);
            obstacle.transform.localScale = new Vector3(width, height, width);
            
            if (obstacleMaterial != null)
            {
                obstacle.GetComponent<Renderer>().material = obstacleMaterial;
            }
            
            obstacles.Add(obstacle);
        }
        
        /// <summary>
        /// Создание целей тестирования
        /// </summary>
        private void CreateGoals()
        {
            // Основная цель - добраться до финиша
            TestGoal mainGoal = new TestGoal
            {
                goalType = GoalType.ReachPoint,
                targetPosition = finishPoint.position,
                description = "Добраться до финишной точки",
                points = 1000
            };
            goals.Add(mainGoal);
            
            // Цель по времени
            TestGoal timeGoal = new TestGoal
            {
                goalType = GoalType.CompleteInTime,
                targetValue = timeLimit,
                description = $"Завершить за {timeLimit} секунд",
                points = 500
            };
            goals.Add(timeGoal);
            
            // Цель по эффективности (минимальное количество деталей)
            TestGoal efficiencyGoal = new TestGoal
            {
                goalType = GoalType.MinimizeParts,
                targetValue = 10, // Максимум 10 деталей
                description = "Использовать не более 10 деталей",
                points = 300
            };
            goals.Add(efficiencyGoal);
        }
        
        /// <summary>
        /// Обновление теста
        /// </summary>
        private void UpdateTest()
        {
            currentTime += Time.deltaTime;
            OnTimeChanged?.Invoke(currentTime);
            
            // Проверяем лимит времени
            if (currentTime >= timeLimit)
            {
                FailTest("Время истекло!");
                return;
            }
            
            // Проверяем цели
            CheckGoals();
            
            // Обновляем счет
            UpdateScore();
        }
        
        /// <summary>
        /// Проверка целей
        /// </summary>
        private void CheckGoals()
        {
            foreach (var goal in goals)
            {
                if (!goal.isCompleted)
                {
                    switch (goal.goalType)
                    {
                        case GoalType.ReachPoint:
                            CheckReachPointGoal(goal);
                            break;
                        case GoalType.CompleteInTime:
                            CheckTimeGoal(goal);
                            break;
                        case GoalType.MinimizeParts:
                            CheckPartsGoal(goal);
                            break;
                    }
                }
            }
        }
        
        /// <summary>
        /// Проверка цели достижения точки
        /// </summary>
        private void CheckReachPointGoal(TestGoal goal)
        {
            // Ищем ближайшую машину к цели
            GameObject[] vehicles = GameObject.FindGameObjectsWithTag("Vehicle");
            if (vehicles.Length > 0)
            {
                float minDistance = float.MaxValue;
                foreach (var vehicle in vehicles)
                {
                    float distance = Vector3.Distance(vehicle.transform.position, goal.targetPosition);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                    }
                }
                
                if (minDistance <= 2f) // Радиус достижения цели
                {
                    goal.isCompleted = true;
                    currentScore += goal.points;
                    Debug.Log($"Goal completed: {goal.description}");
                }
            }
        }
        
        /// <summary>
        /// Проверка цели по времени
        /// </summary>
        private void CheckTimeGoal(TestGoal goal)
        {
            if (currentTime <= goal.targetValue)
            {
                goal.isCompleted = true;
                currentScore += goal.points;
                Debug.Log($"Goal completed: {goal.description}");
            }
        }
        
        /// <summary>
        /// Проверка цели по количеству деталей
        /// </summary>
        private void CheckPartsGoal(TestGoal goal)
        {
            var parts = FindObjectsOfType<ScrapArchitect.Physics.PartController>();
            if (parts.Length <= goal.targetValue)
            {
                goal.isCompleted = true;
                currentScore += goal.points;
                Debug.Log($"Goal completed: {goal.description}");
            }
        }
        
        /// <summary>
        /// Обновление счета
        /// </summary>
        private void UpdateScore()
        {
            // Базовый счет за время
            float timeScore = Mathf.Max(0, timeLimit - currentTime) * timeMultiplier;
            
            // Счет за расстояние
            float distanceScore = CalculateDistanceScore();
            
            // Счет за эффективность
            float efficiencyScore = CalculateEfficiencyScore();
            
            currentScore = timeScore + distanceScore + efficiencyScore;
            OnScoreChanged?.Invoke(currentScore);
        }
        
        /// <summary>
        /// Вычисление счета за расстояние
        /// </summary>
        private float CalculateDistanceScore()
        {
            // TODO: Реализовать подсчет пройденного расстояния
            return 0f;
        }
        
        /// <summary>
        /// Вычисление счета за эффективность
        /// </summary>
        private float CalculateEfficiencyScore()
        {
            var parts = FindObjectsOfType<ScrapArchitect.Physics.PartController>();
            float efficiency = Mathf.Max(0, 100 - parts.Length * 10);
            return efficiency * efficiencyMultiplier;
        }
        
        /// <summary>
        /// Начать тест
        /// </summary>
        public void StartTest()
        {
            if (isTestActive)
                return;
                
            isTestActive = true;
            isTestCompleted = false;
            currentTime = 0f;
            currentScore = 0f;
            
            // Сброс целей
            foreach (var goal in goals)
            {
                goal.isCompleted = false;
            }
            
            OnTestStateChanged?.Invoke(true);
            Debug.Log("Test started");
        }
        
        /// <summary>
        /// Остановить тест
        /// </summary>
        public void StopTest()
        {
            if (!isTestActive)
                return;
                
            isTestActive = false;
            OnTestStateChanged?.Invoke(false);
            Debug.Log("Test stopped");
        }
        
        /// <summary>
        /// Сбросить тест
        /// </summary>
        public void ResetTest()
        {
            StopTest();
            currentTime = 0f;
            currentScore = 0f;
            isTestCompleted = false;
            
            // Сброс целей
            foreach (var goal in goals)
            {
                goal.isCompleted = false;
            }
            
            OnTimeChanged?.Invoke(0f);
            OnScoreChanged?.Invoke(0f);
            Debug.Log("Test reset");
        }
        
        /// <summary>
        /// Завершить тест успешно
        /// </summary>
        private void CompleteTest()
        {
            isTestActive = false;
            isTestCompleted = true;
            OnTestCompleted?.Invoke();
            Debug.Log($"Test completed! Score: {currentScore}");
        }
        
        /// <summary>
        /// Провалить тест
        /// </summary>
        private void FailTest(string reason)
        {
            isTestActive = false;
            isTestCompleted = true;
            OnTestFailed?.Invoke();
            Debug.Log($"Test failed: {reason}");
        }
        
        /// <summary>
        /// Получить текущее время
        /// </summary>
        public float GetCurrentTime()
        {
            return currentTime;
        }
        
        /// <summary>
        /// Получить текущий счет
        /// </summary>
        public float GetCurrentScore()
        {
            return currentScore;
        }
        
        /// <summary>
        /// Получить статус теста
        /// </summary>
        public bool IsTestActive()
        {
            return isTestActive;
        }
        
        /// <summary>
        /// Получить завершенность теста
        /// </summary>
        public bool IsTestCompleted()
        {
            return isTestCompleted;
        }
        
        /// <summary>
        /// Получить стартовую позицию
        /// </summary>
        public Vector3 GetStartPosition()
        {
            return startPosition;
        }
        
        /// <summary>
        /// Получить финишную позицию
        /// </summary>
        public Vector3 GetFinishPosition()
        {
            return finishPoint.position;
        }
    }
    
    /// <summary>
    /// Цель тестирования
    /// </summary>
    [System.Serializable]
    public class TestGoal
    {
        public GoalType goalType;
        public Vector3 targetPosition;
        public float targetValue;
        public string description;
        public int points;
        public bool isCompleted = false;
    }
    
    /// <summary>
    /// Типы целей
    /// </summary>
    public enum GoalType
    {
        ReachPoint,      // Достичь точки
        CompleteInTime,  // Завершить за время
        MinimizeParts    // Минимизировать детали
    }
}
