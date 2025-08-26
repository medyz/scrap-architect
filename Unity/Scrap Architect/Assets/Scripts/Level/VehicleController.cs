using UnityEngine;
using ScrapArchitect.Physics;
using ScrapArchitect.Parts;
using ScrapArchitect.Controls;
using System.Collections.Generic;

namespace ScrapArchitect.Level
{
    /// <summary>
    /// Контроллер машины - управляет состоянием транспорта
    /// </summary>
    public class VehicleController : MonoBehaviour
    {
        [Header("Vehicle Settings")]
        public string vehicleName = "Test Vehicle";
        public float maxSpeed = 50f;
        public float acceleration = 10f;
        public float turnRadius = 5f;
        
        [Header("Components")]
        public List<PartController> vehicleParts = new List<PartController>();
        public List<Motor> motors = new List<Motor>();
        public List<Wheel> wheels = new List<Wheel>();
        
        [Header("State")]
        public bool isActive = false;
        public bool isBroken = false;
        public float currentSpeed = 0f;
        public float currentHealth = 100f;
        public float maxHealth = 100f;
        
        // Приватные переменные
        private Rigidbody vehicleRigidbody;
        private Vector3 lastPosition;
        private float totalDistance = 0f;
        private float startTime = 0f;
        private bool hasStarted = false;
        
        // События
        public System.Action<float> OnSpeedChanged;
        public System.Action<float> OnHealthChanged;
        public System.Action<bool> OnVehicleStateChanged;
        public System.Action OnVehicleBroken;
        public System.Action OnVehicleStarted;
        
        private void Start()
        {
            InitializeVehicle();
        }
        
        private void Update()
        {
            if (isActive)
            {
                UpdateVehicle();
            }
        }
        
        /// <summary>
        /// Инициализация машины
        /// </summary>
        private void InitializeVehicle()
        {
            vehicleRigidbody = GetComponent<Rigidbody>();
            if (vehicleRigidbody == null)
            {
                vehicleRigidbody = gameObject.AddComponent<Rigidbody>();
            }
            
            // Настройка Rigidbody
            vehicleRigidbody.mass = 1000f;
            vehicleRigidbody.drag = 0.1f;
            vehicleRigidbody.angularDrag = 0.5f;
            
            // Сканируем детали
            ScanVehicleParts();
            
            lastPosition = transform.position;
            
            Debug.Log($"Vehicle '{vehicleName}' initialized with {vehicleParts.Count} parts");
        }
        
        /// <summary>
        /// Сканирование деталей машины
        /// </summary>
        public void ScanVehicleParts()
        {
            vehicleParts.Clear();
            motors.Clear();
            wheels.Clear();
            
            // Ищем все детали в дочерних объектах
            PartController[] parts = GetComponentsInChildren<PartController>();
            vehicleParts.AddRange(parts);
            
            // Категоризируем детали
            foreach (var part in parts)
            {
                if (part is Motor motor)
                {
                    motors.Add(motor);
                }
                else if (part is Wheel wheel)
                {
                    wheels.Add(wheel);
                }
            }
            
            // Вычисляем общее здоровье
            CalculateTotalHealth();
            
            Debug.Log($"Scanned vehicle: {motors.Count} motors, {wheels.Count} wheels, {vehicleParts.Count} total parts");
        }
        
        /// <summary>
        /// Вычисление общего здоровья машины
        /// </summary>
        private void CalculateTotalHealth()
        {
            maxHealth = 0f;
            currentHealth = 0f;
            
            foreach (var part in vehicleParts)
            {
                maxHealth += part.maxHealth;
                currentHealth += part.maxHealth; // Начинаем с полного здоровья
            }
        }
        
        /// <summary>
        /// Обновление машины
        /// </summary>
        private void UpdateVehicle()
        {
            if (!hasStarted)
            {
                StartVehicle();
            }
            
            // Обновляем скорость
            UpdateSpeed();
            
            // Обновляем здоровье
            UpdateHealth();
            
            // Проверяем состояние
            CheckVehicleState();
        }
        
        /// <summary>
        /// Запуск машины
        /// </summary>
        private void StartVehicle()
        {
            hasStarted = true;
            startTime = Time.time;
            OnVehicleStarted?.Invoke();
            Debug.Log($"Vehicle '{vehicleName}' started");
        }
        
        /// <summary>
        /// Обновление скорости
        /// </summary>
        private void UpdateSpeed()
        {
            Vector3 velocity = vehicleRigidbody.velocity;
            float newSpeed = velocity.magnitude;
            
            if (Mathf.Abs(newSpeed - currentSpeed) > 0.1f)
            {
                currentSpeed = newSpeed;
                OnSpeedChanged?.Invoke(currentSpeed);
            }
            
            // Обновляем пройденное расстояние
            float distance = Vector3.Distance(transform.position, lastPosition);
            totalDistance += distance;
            lastPosition = transform.position;
        }
        
        /// <summary>
        /// Обновление здоровья
        /// </summary>
        private void UpdateHealth()
        {
            float newHealth = 0f;
            
            foreach (var part in vehicleParts)
            {
                newHealth += part.maxHealth; // Используем maxHealth как текущее здоровье
            }
            
            if (Mathf.Abs(newHealth - currentHealth) > 1f)
            {
                currentHealth = newHealth;
                OnHealthChanged?.Invoke(currentHealth / maxHealth);
                
                // Проверяем, не сломалась ли машина
                if (currentHealth <= 0f && !isBroken)
                {
                    BreakVehicle();
                }
            }
        }
        
        /// <summary>
        /// Проверка состояния машины
        /// </summary>
        private void CheckVehicleState()
        {
            // Проверяем, не упала ли машина
            if (transform.position.y < -10f)
            {
                BreakVehicle("Vehicle fell off the map");
            }
            
            // Проверяем, не застряла ли машина
            if (currentSpeed < 0.1f && Time.time - startTime > 10f)
            {
                // Машина не двигается более 10 секунд
                Debug.Log("Vehicle appears to be stuck");
            }
        }
        
        /// <summary>
        /// Поломка машины
        /// </summary>
        private void BreakVehicle(string reason = "Unknown")
        {
            if (isBroken)
                return;
                
            isBroken = true;
            isActive = false;
            OnVehicleBroken?.Invoke();
            OnVehicleStateChanged?.Invoke(false);
            
            Debug.Log($"Vehicle '{vehicleName}' broken: {reason}");
        }
        
        /// <summary>
        /// Активировать машину
        /// </summary>
        public void ActivateVehicle()
        {
            if (isBroken)
                return;
                
            isActive = true;
            hasStarted = false;
            OnVehicleStateChanged?.Invoke(true);
            
            Debug.Log($"Vehicle '{vehicleName}' activated");
        }
        
        /// <summary>
        /// Деактивировать машину
        /// </summary>
        public void DeactivateVehicle()
        {
            isActive = false;
            OnVehicleStateChanged?.Invoke(false);
            
            Debug.Log($"Vehicle '{vehicleName}' deactivated");
        }
        
        /// <summary>
        /// Ремонт машины
        /// </summary>
        public void RepairVehicle()
        {
            isBroken = false;
            currentHealth = maxHealth;
            
            // Восстанавливаем все детали
            foreach (var part in vehicleParts)
            {
                // TODO: Восстановить здоровье деталей
            }
            
            Debug.Log($"Vehicle '{vehicleName}' repaired");
        }
        
        /// <summary>
        /// Сброс машины
        /// </summary>
        public void ResetVehicle()
        {
            isActive = false;
            isBroken = false;
            hasStarted = false;
            currentSpeed = 0f;
            currentHealth = maxHealth;
            totalDistance = 0f;
            startTime = 0f;
            
            // Сбрасываем позицию
            transform.position = Vector3.up * 2f;
            transform.rotation = Quaternion.identity;
            
            if (vehicleRigidbody != null)
            {
                vehicleRigidbody.velocity = Vector3.zero;
                vehicleRigidbody.angularVelocity = Vector3.zero;
            }
            
            OnVehicleStateChanged?.Invoke(false);
            Debug.Log($"Vehicle '{vehicleName}' reset");
        }
        
        /// <summary>
        /// Получить текущую скорость
        /// </summary>
        public float GetCurrentSpeed()
        {
            return currentSpeed;
        }
        
        /// <summary>
        /// Получить пройденное расстояние
        /// </summary>
        public float GetTotalDistance()
        {
            return totalDistance;
        }
        
        /// <summary>
        /// Получить время работы
        /// </summary>
        public float GetRunningTime()
        {
            if (!hasStarted)
                return 0f;
                
            return Time.time - startTime;
        }
        
        /// <summary>
        /// Получить здоровье в процентах
        /// </summary>
        public float GetHealthPercentage()
        {
            return maxHealth > 0f ? currentHealth / maxHealth : 0f;
        }
        
        /// <summary>
        /// Получить количество деталей
        /// </summary>
        public int GetPartsCount()
        {
            return vehicleParts.Count;
        }
        
        /// <summary>
        /// Получить количество моторов
        /// </summary>
        public int GetMotorsCount()
        {
            return motors.Count;
        }
        
        /// <summary>
        /// Получить количество колес
        /// </summary>
        public int GetWheelsCount()
        {
            return wheels.Count;
        }
        
        /// <summary>
        /// Проверить, работает ли машина
        /// </summary>
        public bool IsWorking()
        {
            return isActive && !isBroken;
        }
        
        /// <summary>
        /// Проверить, сломана ли машина
        /// </summary>
        public bool IsBroken()
        {
            return isBroken;
        }
        
        /// <summary>
        /// Получить информацию о машине
        /// </summary>
        public string GetVehicleInfo()
        {
            return $"Vehicle: {vehicleName}\n" +
                   $"Parts: {vehicleParts.Count}\n" +
                   $"Motors: {motors.Count}\n" +
                   $"Wheels: {wheels.Count}\n" +
                   $"Health: {GetHealthPercentage():P0}\n" +
                   $"Speed: {currentSpeed:F1} m/s\n" +
                   $"Distance: {totalDistance:F1} m\n" +
                   $"Status: {(isBroken ? "Broken" : (isActive ? "Active" : "Inactive"))}";
        }
        
        /// <summary>
        /// Добавить деталь к машине
        /// </summary>
        public void AddPart(PartController part)
        {
            if (!vehicleParts.Contains(part))
            {
                vehicleParts.Add(part);
                
                if (part is Motor motor)
                {
                    motors.Add(motor);
                }
                else if (part is Wheel wheel)
                {
                    wheels.Add(wheel);
                }
                
                CalculateTotalHealth();
            }
        }
        
        /// <summary>
        /// Удалить деталь из машины
        /// </summary>
        public void RemovePart(PartController part)
        {
            if (vehicleParts.Contains(part))
            {
                vehicleParts.Remove(part);
                
                if (part is Motor motor)
                {
                    motors.Remove(motor);
                }
                else if (part is Wheel wheel)
                {
                    wheels.Remove(wheel);
                }
                
                CalculateTotalHealth();
            }
        }
        
        private void OnDestroy()
        {
            // Очищаем события
            OnSpeedChanged = null;
            OnHealthChanged = null;
            OnVehicleStateChanged = null;
            OnVehicleBroken = null;
            OnVehicleStarted = null;
        }
    }
}
