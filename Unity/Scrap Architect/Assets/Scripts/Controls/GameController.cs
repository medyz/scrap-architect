using UnityEngine;
using ScrapArchitect.Physics;
using ScrapArchitect.Parts;
using ScrapArchitect.Core;
using System.Collections.Generic;

namespace ScrapArchitect.Controls
{
    /// <summary>
    /// Контроллер игровых контролов - управление транспортом
    /// </summary>
    public class GameController : MonoBehaviour
    {
        [Header("Vehicle Control")]
        public float throttleSensitivity = 1f;
        public float steeringSensitivity = 1f;
        public float brakeForce = 10f;
        
        [Header("Input Settings")]
        public KeyCode throttleKey = KeyCode.W;
        public KeyCode reverseKey = KeyCode.S;
        public KeyCode leftSteerKey = KeyCode.A;
        public KeyCode rightSteerKey = KeyCode.D;
        public KeyCode brakeKey = KeyCode.Space;
        public KeyCode engineToggleKey = KeyCode.E;
        
        [Header("Camera Settings")]
        public KeyCode followVehicleKey = KeyCode.F;
        public KeyCode freeCameraKey = KeyCode.C;
        
        // Приватные переменные
        private List<Motor> vehicleMotors = new List<Motor>();
        private List<Wheel> vehicleWheels = new List<Wheel>();
        private Transform vehicleCenter;
        private CameraController cameraController;
        private PartManipulator partManipulator;
        
        // Состояние управления
        private float currentThrottle = 0f;
        private float currentSteering = 0f;
        private bool isEngineRunning = false;
        private bool isFollowingVehicle = true;
        
        // События
        public System.Action<float> OnThrottleChanged;
        public System.Action<float> OnSteeringChanged;
        public System.Action<bool> OnEngineStateChanged;
        
        private void Start()
        {
            InitializeController();
        }
        
        private void Update()
        {
            if (Core.GameManager.Instance != null && 
                Core.GameManager.Instance.currentGameState != GameState.Paused)
            {
                HandleInput();
                UpdateVehicle();
            }
        }
        
        /// <summary>
        /// Инициализация контроллера
        /// </summary>
        private void InitializeController()
        {
            cameraController = FindObjectOfType<CameraController>();
            partManipulator = FindObjectOfType<PartManipulator>();
            
            // Подписываемся на события выбора деталей
            if (partManipulator != null)
            {
                partManipulator.OnPartSelected += OnPartSelected;
                partManipulator.OnPartDeselected += OnPartDeselected;
            }
            
            Debug.Log("GameController initialized");
        }
        
        /// <summary>
        /// Обработка ввода
        /// </summary>
        private void HandleInput()
        {
            HandleVehicleInput();
            HandleCameraInput();
        }
        
        /// <summary>
        /// Обработка ввода для транспорта
        /// </summary>
        private void HandleVehicleInput()
        {
            // Газ/тормоз
            float throttleInput = 0f;
            if (Input.GetKey(throttleKey))
                throttleInput += 1f;
            if (Input.GetKey(reverseKey))
                throttleInput -= 1f;
            
            currentThrottle = Mathf.Lerp(currentThrottle, throttleInput * throttleSensitivity, Time.deltaTime * 5f);
            
            // Руление
            float steeringInput = 0f;
            if (Input.GetKey(leftSteerKey))
                steeringInput -= 1f;
            if (Input.GetKey(rightSteerKey))
                steeringInput += 1f;
            
            currentSteering = Mathf.Lerp(currentSteering, steeringInput * steeringSensitivity, Time.deltaTime * 5f);
            
            // Тормоз
            bool isBraking = Input.GetKey(brakeKey);
            
            // Переключение двигателя
            if (Input.GetKeyDown(engineToggleKey))
            {
                ToggleEngine();
            }
            
            // Применяем управление
            ApplyThrottle(currentThrottle);
            ApplySteering(currentSteering);
            ApplyBrake(isBraking);
            
            // Вызываем события
            OnThrottleChanged?.Invoke(currentThrottle);
            OnSteeringChanged?.Invoke(currentSteering);
        }
        
        /// <summary>
        /// Обработка ввода для камеры
        /// </summary>
        private void HandleCameraInput()
        {
            if (Input.GetKeyDown(followVehicleKey))
            {
                SetFollowVehicle(true);
            }
            
            if (Input.GetKeyDown(freeCameraKey))
            {
                SetFollowVehicle(false);
            }
        }
        
        /// <summary>
        /// Обновление транспорта
        /// </summary>
        private void UpdateVehicle()
        {
            // Обновляем центр транспорта
            UpdateVehicleCenter();
            
            // Обновляем камеру если следим за транспортом
            if (isFollowingVehicle && vehicleCenter != null && cameraController != null)
            {
                cameraController.SetTarget(vehicleCenter);
            }
        }
        
        /// <summary>
        /// Обновление центра транспорта
        /// </summary>
        private void UpdateVehicleCenter()
        {
            if (vehicleMotors.Count == 0 && vehicleWheels.Count == 0)
                return;
            
            Vector3 centerPosition = Vector3.zero;
            int partCount = 0;
            
            // Вычисляем центр по моторам
            foreach (var motor in vehicleMotors)
            {
                if (motor != null)
                {
                    centerPosition += motor.transform.position;
                    partCount++;
                }
            }
            
            // Вычисляем центр по колесам
            foreach (var wheel in vehicleWheels)
            {
                if (wheel != null)
                {
                    centerPosition += wheel.transform.position;
                    partCount++;
                }
            }
            
            if (partCount > 0)
            {
                centerPosition /= partCount;
                
                if (vehicleCenter == null)
                {
                    GameObject centerObject = new GameObject("VehicleCenter");
                    vehicleCenter = centerObject.transform;
                }
                
                vehicleCenter.position = centerPosition;
            }
        }
        
        /// <summary>
        /// Применить газ
        /// </summary>
        private void ApplyThrottle(float throttle)
        {
            if (!isEngineRunning)
                return;
            
            foreach (var motor in vehicleMotors)
            {
                if (motor != null && motor.isRunning)
                {
                    motor.ApplyPowerToWheels(throttle);
                }
            }
        }
        
        /// <summary>
        /// Применить руление
        /// </summary>
        private void ApplySteering(float steering)
        {
            foreach (var wheel in vehicleWheels)
            {
                if (wheel != null && wheel.isSteering)
                {
                    wheel.SetSteeringAngle(steering * 30f); // Максимальный угол поворота 30 градусов
                }
            }
        }
        
        /// <summary>
        /// Применить тормоз
        /// </summary>
        private void ApplyBrake(bool isBraking)
        {
            foreach (var wheel in vehicleWheels)
            {
                if (wheel != null)
                {
                    // Применяем тормоз через WheelCollider
                    WheelCollider wheelCollider = wheel.GetComponent<WheelCollider>();
                    if (wheelCollider != null)
                    {
                        wheelCollider.brakeTorque = isBraking ? brakeForce : 0f;
                    }
                }
            }
        }
        
        /// <summary>
        /// Переключить двигатель
        /// </summary>
        private void ToggleEngine()
        {
            isEngineRunning = !isEngineRunning;
            
            foreach (var motor in vehicleMotors)
            {
                if (motor != null)
                {
                    if (isEngineRunning)
                    {
                        motor.StartEngine();
                    }
                    else
                    {
                        motor.StopEngine();
                    }
                }
            }
            
            OnEngineStateChanged?.Invoke(isEngineRunning);
            Debug.Log($"Engine {(isEngineRunning ? "started" : "stopped")}");
        }
        
        /// <summary>
        /// Установить следование за транспортом
        /// </summary>
        private void SetFollowVehicle(bool follow)
        {
            isFollowingVehicle = follow;
            
            if (cameraController != null)
            {
                cameraController.SetFollowTarget(follow);
                
                if (follow && vehicleCenter != null)
                {
                    cameraController.SetTarget(vehicleCenter);
                }
                else
                {
                    cameraController.SetTarget(null);
                }
            }
            
            Debug.Log($"Camera follow vehicle: {follow}");
        }
        
        /// <summary>
        /// Обработчик выбора детали
        /// </summary>
        private void OnPartSelected(PartController part)
        {
            // Проверяем тип детали и добавляем в соответствующие списки
            if (part is Motor motor)
            {
                if (!vehicleMotors.Contains(motor))
                {
                    vehicleMotors.Add(motor);
                    Debug.Log($"Motor added to vehicle: {motor.partName}");
                }
            }
            else if (part is Wheel wheel)
            {
                if (!vehicleWheels.Contains(wheel))
                {
                    vehicleWheels.Add(wheel);
                    Debug.Log($"Wheel added to vehicle: {wheel.partName}");
                }
            }
        }
        
        /// <summary>
        /// Обработчик снятия выбора с детали
        /// </summary>
        private void OnPartDeselected(PartController part)
        {
            // Удаляем деталь из списков
            if (part is Motor motor)
            {
                vehicleMotors.Remove(motor);
                Debug.Log($"Motor removed from vehicle: {motor.partName}");
            }
            else if (part is Wheel wheel)
            {
                vehicleWheels.Remove(wheel);
                Debug.Log($"Wheel removed from vehicle: {wheel.partName}");
            }
        }
        
        /// <summary>
        /// Сканировать сцену на предмет деталей транспорта
        /// </summary>
        public void ScanForVehicleParts()
        {
            vehicleMotors.Clear();
            vehicleWheels.Clear();
            
            // Ищем все моторы и колеса в сцене
            Motor[] motors = FindObjectsOfType<Motor>();
            Wheel[] wheels = FindObjectsOfType<Wheel>();
            
            vehicleMotors.AddRange(motors);
            vehicleWheels.AddRange(wheels);
            
            Debug.Log($"Found {vehicleMotors.Count} motors and {vehicleWheels.Count} wheels");
        }
        
        /// <summary>
        /// Получить текущий газ
        /// </summary>
        public float GetCurrentThrottle()
        {
            return currentThrottle;
        }
        
        /// <summary>
        /// Получить текущее руление
        /// </summary>
        public float GetCurrentSteering()
        {
            return currentSteering;
        }
        
        /// <summary>
        /// Получить состояние двигателя
        /// </summary>
        public bool IsEngineRunning()
        {
            return isEngineRunning;
        }
        
        /// <summary>
        /// Получить количество моторов
        /// </summary>
        public int GetMotorCount()
        {
            return vehicleMotors.Count;
        }
        
        /// <summary>
        /// Получить количество колес
        /// </summary>
        public int GetWheelCount()
        {
            return vehicleWheels.Count;
        }
        
        /// <summary>
        /// Остановить все двигатели
        /// </summary>
        public void StopAllEngines()
        {
            isEngineRunning = false;
            
            foreach (var motor in vehicleMotors)
            {
                if (motor != null)
                {
                    motor.StopEngine();
                }
            }
            
            OnEngineStateChanged?.Invoke(false);
            Debug.Log("All engines stopped");
        }
        
        /// <summary>
        /// Сбросить управление
        /// </summary>
        public void ResetControls()
        {
            currentThrottle = 0f;
            currentSteering = 0f;
            
            ApplyThrottle(0f);
            ApplySteering(0f);
            ApplyBrake(false);
            
            Debug.Log("Controls reset");
        }
    }
}
