using UnityEngine;
using System;
using ScrapArchitect.Physics;
using ScrapArchitect.Controls;

namespace ScrapArchitect.Parts
{
    public class DriverSeat : PartBase
    {
        [Header("Driver Seat Settings")]
        public SeatType seatType = SeatType.Basic;
        public bool hasSteeringWheel = true;
        public bool hasGearShift = false;
        public bool hasDashboard = false;
        public float comfortLevel = 1.0f;
        public float safetyLevel = 1.0f;
        
        [Header("Control Settings")]
        public bool autoConnectToMotors = true;
        public bool autoConnectToWheels = true;
        public float controlRadius = 10f;
        public LayerMask controllableLayers = -1;
        
        [Header("Visual Settings")]
        public GameObject driverModel;
        public GameObject steeringWheelModel;
        public GameObject gearShiftModel;
        public GameObject dashboardModel;
        public Material seatMaterial;
        public Material leatherMaterial;
        public Material racingMaterial;
        
        [Header("Audio Settings")]
        public AudioClip seatSound;
        public AudioClip engineStartSound;
        public AudioClip hornSound;
        
        private bool isOccupied = false;
        private GameObject currentDriver;
        private GameController gameController;
        private Motor[] connectedMotors;
        private Wheel[] connectedWheels;
        private CameraController cameraController;
        
        public enum SeatType
        {
            Basic,      // Базовое сиденье
            Comfort,    // Комфортное
            Racing,     // Спортивное
            Luxury,     // Люкс
            Industrial  // Промышленное
        }
        
        protected override void InitializePart()
        {
            base.InitializePart();
            
            // Настройка параметров детали
            partID = "seat_" + seatType.ToString().ToLower();
            partName = GetSeatName(seatType);
            description = GetSeatDescription(seatType);
            unlockLevel = GetUnlockLevel(seatType);
            cost = GetCost(seatType);
            
            // Настройка физических свойств
            mass = GetMass(seatType);
            currentHealth = GetHealth(seatType);
            maxHealth = GetHealth(seatType);
            
            // Настройка визуальных элементов
            SetupVisualElements();
            
            Debug.Log($"DriverSeat initialized: {partName} (Type: {seatType})");
        }
        
        /// <summary>
        /// Специфичное действие для сиденья водителя
        /// </summary>
        protected override void OnPartSpecificAction()
        {
            // Логика специфичная для сиденья водителя
            if (isOccupied)
            {
                // Проверка состояния водителя и управление
                UpdateDriverControls();
            }
        }
        
        /// <summary>
        /// Обновление управления водителя
        /// </summary>
        private void UpdateDriverControls()
        {
            // Здесь можно добавить логику управления
            // например, проверка состояния двигателей, колес и т.д.
        }
        
        private void SetupVisualElements()
        {
            // Настройка материалов в зависимости от типа сиденья
            Material targetMaterial = GetSeatMaterial(seatType);
            if (targetMaterial != null)
            {
                GetComponent<Renderer>().material = targetMaterial;
            }
            
            // Активировать/деактивировать элементы в зависимости от типа
            if (steeringWheelModel != null)
            {
                steeringWheelModel.SetActive(hasSteeringWheel);
            }
            
            if (gearShiftModel != null)
            {
                gearShiftModel.SetActive(hasGearShift);
            }
            
            if (dashboardModel != null)
            {
                dashboardModel.SetActive(hasDashboard);
            }
        }
        
        public void OnConnect(PartController otherPart)
        {
            // Автоматическое подключение к двигателям и колесам
            if (autoConnectToMotors)
            {
                Motor motor = otherPart.GetComponent<Motor>();
                if (motor != null)
                {
                    AddMotor(motor);
                }
            }
            
            if (autoConnectToWheels)
            {
                Wheel wheel = otherPart.GetComponent<Wheel>();
                if (wheel != null)
                {
                    AddWheel(wheel);
                }
            }
        }
        
        public void OnDisconnect(PartController otherPart)
        {
            // Отключение от двигателей и колес
            Motor motor = otherPart.GetComponent<Motor>();
            if (motor != null)
            {
                RemoveMotor(motor);
            }
            
            Wheel wheel = otherPart.GetComponent<Wheel>();
            if (wheel != null)
            {
                RemoveWheel(wheel);
            }
        }
        
        public bool TryOccupy(GameObject driver)
        {
            if (isOccupied)
            {
                return false;
            }
            
            isOccupied = true;
            currentDriver = driver;
            
            // Найти и настроить GameController
            gameController = FindObjectOfType<GameController>();
            if (gameController != null)
            {
                gameController.SetFollowVehicle(gameObject);
            }
            
            // Найти и настроить CameraController
            cameraController = FindObjectOfType<CameraController>();
            if (cameraController != null)
            {
                cameraController.SetTarget(transform);
            }
            
            // Воспроизвести звук посадки
            PlaySound("seat");
            
            // Активировать водителя
            if (driverModel != null)
            {
                driverModel.SetActive(true);
            }
            
            Debug.Log($"Driver occupied seat: {partName}");
            return true;
        }
        
        public void Vacate()
        {
            if (!isOccupied)
            {
                return;
            }
            
            isOccupied = false;
            currentDriver = null;
            
            // Отключить управление
            if (gameController != null)
            {
                gameController.SetFollowVehicle(null);
            }
            
            if (cameraController != null)
            {
                cameraController.SetTarget(null);
            }
            
            // Деактивировать водителя
            if (driverModel != null)
            {
                driverModel.SetActive(false);
            }
            
            Debug.Log($"Driver vacated seat: {partName}");
        }
        
        public void StartEngine()
        {
            if (!isOccupied)
            {
                return;
            }
            
            foreach (Motor motor in connectedMotors)
            {
                if (motor != null)
                {
                    motor.StartEngine();
                }
            }
            
            // Воспроизвести звук запуска двигателя
            PlaySound("engine_start");
            
            Debug.Log("Engine started from driver seat");
        }
        
        public void StopEngine()
        {
            if (!isOccupied)
            {
                return;
            }
            
            foreach (Motor motor in connectedMotors)
            {
                if (motor != null)
                {
                    motor.StopEngine();
                }
            }
            
            Debug.Log("Engine stopped from driver seat");
        }
        
        public void Honk()
        {
            if (!isOccupied)
            {
                return;
            }
            
            // Воспроизвести звук клаксона
            PlaySound("horn");
            
            Debug.Log("Horn honked from driver seat");
        }
        
        public void ShiftGear(int gear)
        {
            if (!isOccupied || !hasGearShift)
            {
                return;
            }
            
            // Логика переключения передач
            foreach (Motor motor in connectedMotors)
            {
                if (motor != null)
                {
                    // Применяем мощность к колесам в зависимости от передачи
                    float throttle = gear switch
                    {
                        1 => 0.2f,  // Первая передача - низкая мощность
                        2 => 0.4f,  // Вторая передача
                        3 => 0.6f,  // Третья передача
                        4 => 0.8f,  // Четвертая передача
                        5 => 1.0f,  // Пятая передача - полная мощность
                        _ => 0.0f   // Нейтраль
                    };
                    motor.ApplyPowerToWheels(throttle);
                }
            }
            
            Debug.Log($"Gear shifted to {gear}");
        }
        
        private void AddMotor(Motor motor)
        {
            if (connectedMotors == null)
            {
                connectedMotors = new Motor[0];
            }
            
            Motor[] newMotors = new Motor[connectedMotors.Length + 1];
            Array.Copy(connectedMotors, newMotors, connectedMotors.Length);
            newMotors[connectedMotors.Length] = motor;
            connectedMotors = newMotors;
            
            Debug.Log($"Motor connected to seat: {motor.partName}");
        }
        
        private void RemoveMotor(Motor motor)
        {
            if (connectedMotors == null)
            {
                return;
            }
            
            for (int i = 0; i < connectedMotors.Length; i++)
            {
                if (connectedMotors[i] == motor)
                {
                    Motor[] newMotors = new Motor[connectedMotors.Length - 1];
                    Array.Copy(connectedMotors, 0, newMotors, 0, i);
                    Array.Copy(connectedMotors, i + 1, newMotors, i, connectedMotors.Length - i - 1);
                    connectedMotors = newMotors;
                    break;
                }
            }
            
            Debug.Log($"Motor disconnected from seat: {motor.partName}");
        }
        
        private void AddWheel(Wheel wheel)
        {
            if (connectedWheels == null)
            {
                connectedWheels = new Wheel[0];
            }
            
            Wheel[] newWheels = new Wheel[connectedWheels.Length + 1];
            Array.Copy(connectedWheels, newWheels, connectedWheels.Length);
            newWheels[connectedWheels.Length] = wheel;
            connectedWheels = newWheels;
            
            Debug.Log($"Wheel connected to seat: {wheel.partName}");
        }
        
        private void RemoveWheel(Wheel wheel)
        {
            if (connectedWheels == null)
            {
                return;
            }
            
            for (int i = 0; i < connectedWheels.Length; i++)
            {
                if (connectedWheels[i] == wheel)
                {
                    Wheel[] newWheels = new Wheel[connectedWheels.Length - 1];
                    Array.Copy(connectedWheels, 0, newWheels, 0, i);
                    Array.Copy(connectedWheels, i + 1, newWheels, i, connectedWheels.Length - i - 1);
                    connectedWheels = newWheels;
                    break;
                }
            }
            
            Debug.Log($"Wheel disconnected from seat: {wheel.partName}");
        }
        
        public bool IsOccupied()
        {
            return isOccupied;
        }
        
        public GameObject GetCurrentDriver()
        {
            return currentDriver;
        }
        
        public int GetConnectedMotorCount()
        {
            return connectedMotors != null ? connectedMotors.Length : 0;
        }
        
        public int GetConnectedWheelCount()
        {
            return connectedWheels != null ? connectedWheels.Length : 0;
        }
        
        public float GetComfortLevel()
        {
            return comfortLevel;
        }
        
        public float GetSafetyLevel()
        {
            return safetyLevel;
        }
        
        private string GetSeatName(SeatType type)
        {
            switch (type)
            {
                case SeatType.Basic: return "Базовое сиденье";
                case SeatType.Comfort: return "Комфортное сиденье";
                case SeatType.Racing: return "Спортивное сиденье";
                case SeatType.Luxury: return "Люкс сиденье";
                case SeatType.Industrial: return "Промышленное сиденье";
                default: return "Сиденье";
            }
        }
        
        private string GetSeatDescription(SeatType type)
        {
            switch (type)
            {
                case SeatType.Basic: return "Простое сиденье для управления машиной";
                case SeatType.Comfort: return "Удобное сиденье с подушками";
                case SeatType.Racing: return "Спортивное сиденье с боковой поддержкой";
                case SeatType.Luxury: return "Роскошное сиденье с подогревом";
                case SeatType.Industrial: return "Прочное сиденье для тяжелых условий";
                default: return "Сиденье водителя";
            }
        }
        
        private int GetUnlockLevel(SeatType type)
        {
            switch (type)
            {
                case SeatType.Basic: return 1;
                case SeatType.Comfort: return 2;
                case SeatType.Racing: return 3;
                case SeatType.Luxury: return 4;
                case SeatType.Industrial: return 2;
                default: return 1;
            }
        }
        
        private int GetCost(SeatType type)
        {
            switch (type)
            {
                case SeatType.Basic: return 50;
                case SeatType.Comfort: return 100;
                case SeatType.Racing: return 200;
                case SeatType.Luxury: return 500;
                case SeatType.Industrial: return 150;
                default: return 50;
            }
        }
        
        private float GetMass(SeatType type)
        {
            switch (type)
            {
                case SeatType.Basic: return 5f;
                case SeatType.Comfort: return 8f;
                case SeatType.Racing: return 6f;
                case SeatType.Luxury: return 12f;
                case SeatType.Industrial: return 10f;
                default: return 5f;
            }
        }
        
        private float GetHealth(SeatType type)
        {
            switch (type)
            {
                case SeatType.Basic: return 80f;
                case SeatType.Comfort: return 100f;
                case SeatType.Racing: return 120f;
                case SeatType.Luxury: return 150f;
                case SeatType.Industrial: return 200f;
                default: return 80f;
            }
        }
        
        private float GetStrength(SeatType type)
        {
            switch (type)
            {
                case SeatType.Basic: return 60f;
                case SeatType.Comfort: return 80f;
                case SeatType.Racing: return 100f;
                case SeatType.Luxury: return 120f;
                case SeatType.Industrial: return 150f;
                default: return 60f;
            }
        }
        
        private Material GetSeatMaterial(SeatType type)
        {
            switch (type)
            {
                case SeatType.Basic: return seatMaterial;
                case SeatType.Comfort: return leatherMaterial;
                case SeatType.Racing: return racingMaterial;
                case SeatType.Luxury: return leatherMaterial;
                case SeatType.Industrial: return seatMaterial;
                default: return seatMaterial;
            }
        }
    }
}
