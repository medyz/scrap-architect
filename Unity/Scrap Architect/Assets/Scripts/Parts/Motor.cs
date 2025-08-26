using UnityEngine;
using System.Collections.Generic;
using ScrapArchitect.Physics;

namespace ScrapArchitect.Parts
{
    /// <summary>
    /// Двигатель - источник энергии для движения транспортных средств
    /// </summary>
    public class Motor : PartBase
    {
        [Header("Motor Settings")]
        public MotorType motorType = MotorType.Electric;
        public float power = 100f;
        public float maxRPM = 3000f;
        public float fuelConsumption = 1f;
        public float currentFuel = 100f;
        public float maxFuel = 100f;
        
        [Header("Motor State")]
        public bool isRunning = false;
        public bool isOverheating = false;
        public float temperature = 20f;
        public float maxTemperature = 100f;
        
        [Header("Motor Connections")]
        public List<Wheel> connectedWheels = new List<Wheel>();
        public int maxWheels = 4;
        
        [Header("Motor Visual")]
        public Color motorColor = Color.red;
        public GameObject engineEffect;
        
        // Компоненты двигателя
        private AudioSource engineSound;
        private ParticleSystem exhaustEffect;
        
        private void Start()
        {
            InitializeMotor();
        }
        
        private void Update()
        {
            if (isRunning)
            {
                UpdateMotor();
            }
        }
        
        /// <summary>
        /// Инициализация двигателя
        /// </summary>
        private void InitializeMotor()
        {
            // Устанавливаем тип детали
            partType = PartType.Motor;
            
            // Настраиваем параметры в зависимости от типа двигателя
            SetupMotorProperties();
            
            // Создаем звук двигателя
            CreateEngineSound();
            
            // Создаем эффект выхлопа
            CreateExhaustEffect();
            
            Debug.Log($"Motor {partName} ({motorType}) initialized");
        }
        
        /// <summary>
        /// Настройка свойств двигателя в зависимости от типа
        /// </summary>
        private void SetupMotorProperties()
        {
            switch (motorType)
            {
                case MotorType.Electric:
                    mass = 2f;
                    maxHealth = 60f;
                    power = 80f;
                    maxRPM = 4000f;
                    fuelConsumption = 0.5f;
                    maxFuel = 100f;
                    cost = 20f;
                    unlockLevel = 2;
                    break;
                    
                case MotorType.Gasoline:
                    mass = 3f;
                    maxHealth = 80f;
                    power = 120f;
                    maxRPM = 6000f;
                    fuelConsumption = 1.5f;
                    maxFuel = 80f;
                    cost = 30f;
                    unlockLevel = 3;
                    break;
                    
                case MotorType.Diesel:
                    mass = 4f;
                    maxHealth = 100f;
                    power = 150f;
                    maxRPM = 2500f;
                    fuelConsumption = 1f;
                    maxFuel = 120f;
                    cost = 40f;
                    unlockLevel = 4;
                    break;
                    
                case MotorType.Jet:
                    mass = 1f;
                    maxHealth = 40f;
                    power = 200f;
                    maxRPM = 8000f;
                    fuelConsumption = 3f;
                    maxFuel = 60f;
                    cost = 50f;
                    unlockLevel = 5;
                    break;
            }
            
            // Обновляем текущее топливо
            currentFuel = maxFuel;
            currentHealth = maxHealth;
        }
        
        /// <summary>
        /// Создание звука двигателя
        /// </summary>
        private void CreateEngineSound()
        {
            engineSound = GetComponent<AudioSource>();
            if (engineSound == null)
            {
                engineSound = gameObject.AddComponent<AudioSource>();
            }
            
            // Настройка AudioSource для двигателя
            engineSound.loop = true;
            engineSound.volume = 0.3f;
            engineSound.pitch = 1f;
            engineSound.spatialBlend = 1f; // 3D звук
            engineSound.rolloffMode = AudioRolloffMode.Linear;
            engineSound.maxDistance = 20f;
        }
        
        /// <summary>
        /// Создание эффекта выхлопа
        /// </summary>
        private void CreateExhaustEffect()
        {
            if (engineEffect != null)
            {
                exhaustEffect = Instantiate(engineEffect, transform).GetComponent<ParticleSystem>();
                exhaustEffect.Stop();
            }
        }
        
        /// <summary>
        /// Специфичная логика двигателя при соединении
        /// </summary>
        protected override void OnPartSpecificAction()
        {
            // При соединении двигателя проверяем подключенные колеса
            CheckConnectedWheels();
        }
        
        /// <summary>
        /// Проверка подключенных колес
        /// </summary>
        private void CheckConnectedWheels()
        {
            connectedWheels.Clear();
            
            // Ищем все колеса в радиусе соединения
            Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, 2f);
            
            foreach (var collider in nearbyColliders)
            {
                Wheel wheel = collider.GetComponent<Wheel>();
                if (wheel != null && wheel != this)
                {
                    connectedWheels.Add(wheel);
                    wheel.isMotorized = true;
                }
            }
            
            Debug.Log($"Motor {partName} connected to {connectedWheels.Count} wheels");
        }
        
        /// <summary>
        /// Запуск двигателя
        /// </summary>
        public void StartEngine()
        {
            if (!isRunning && currentFuel > 0 && !isOverheating)
            {
                isRunning = true;
                
                // Запускаем звук двигателя
                if (engineSound != null)
                {
                    engineSound.Play();
                }
                
                // Запускаем эффект выхлопа
                if (exhaustEffect != null)
                {
                    exhaustEffect.Play();
                }
                
                Debug.Log($"Motor {partName} started");
            }
        }
        
        /// <summary>
        /// Остановка двигателя
        /// </summary>
        public void StopEngine()
        {
            if (isRunning)
            {
                isRunning = false;
                
                // Останавливаем звук двигателя
                if (engineSound != null)
                {
                    engineSound.Stop();
                }
                
                // Останавливаем эффект выхлопа
                if (exhaustEffect != null)
                {
                    exhaustEffect.Stop();
                }
                
                Debug.Log($"Motor {partName} stopped");
            }
        }
        
        /// <summary>
        /// Обновление работы двигателя
        /// </summary>
        private void UpdateMotor()
        {
            if (currentFuel <= 0)
            {
                StopEngine();
                return;
            }
            
            // Расход топлива
            currentFuel -= fuelConsumption * Time.deltaTime;
            
            // Нагрев двигателя
            temperature += 10f * Time.deltaTime;
            
            // Проверка перегрева
            if (temperature >= maxTemperature)
            {
                isOverheating = true;
                StopEngine();
                Debug.Log($"Motor {partName} overheated!");
            }
            
            // Охлаждение при выключенном двигателе
            if (!isRunning)
            {
                temperature -= 5f * Time.deltaTime;
                if (temperature <= 20f)
                {
                    temperature = 20f;
                    isOverheating = false;
                }
            }
            
            // Обновление звука двигателя
            UpdateEngineSound();
        }
        
        /// <summary>
        /// Обновление звука двигателя
        /// </summary>
        private void UpdateEngineSound()
        {
            if (engineSound != null && isRunning)
            {
                // Изменяем высоту звука в зависимости от RPM
                float rpmRatio = Mathf.Clamp01(GetAverageWheelRPM() / maxRPM);
                engineSound.pitch = 0.5f + rpmRatio * 1.5f;
                
                // Изменяем громкость в зависимости от нагрузки
                engineSound.volume = 0.2f + rpmRatio * 0.3f;
            }
        }
        
        /// <summary>
        /// Получение среднего RPM подключенных колес
        /// </summary>
        private float GetAverageWheelRPM()
        {
            if (connectedWheels.Count == 0)
                return 0f;
            
            float totalRPM = 0f;
            foreach (var wheel in connectedWheels)
            {
                totalRPM += Mathf.Abs(wheel.GetWheelRPM());
            }
            
            return totalRPM / connectedWheels.Count;
        }
        
        /// <summary>
        /// Применение мощности к подключенным колесам
        /// </summary>
        public void ApplyPowerToWheels(float throttle)
        {
            if (!isRunning || connectedWheels.Count == 0)
                return;
            
            float powerPerWheel = power / connectedWheels.Count;
            float torque = powerPerWheel * throttle;
            
            foreach (var wheel in connectedWheels)
            {
                wheel.ApplyTorque(torque);
            }
        }
        
        /// <summary>
        /// Заправка двигателя
        /// </summary>
        public void Refuel(float amount)
        {
            currentFuel = Mathf.Clamp(currentFuel + amount, 0f, maxFuel);
            Debug.Log($"Motor {partName} refueled. Current fuel: {currentFuel:F1}/{maxFuel}");
        }
        
        /// <summary>
        /// Получение информации о двигателе
        /// </summary>
        public override string GetPartInfo()
        {
            string baseInfo = base.GetPartInfo();
            string motorInfo = $"\nMotor Type: {motorType}\nPower: {power}\nMax RPM: {maxRPM}\nFuel: {currentFuel:F1}/{maxFuel}\nTemperature: {temperature:F1}°C\nRunning: {isRunning}\nConnected Wheels: {connectedWheels.Count}";
            
            return baseInfo + motorInfo;
        }
        
        /// <summary>
        /// Получение урона с учетом типа двигателя
        /// </summary>
        public override void TakeDamage(float damage)
        {
            // Двигатели уязвимы к урону
            float actualDamage = damage * 1.5f;
            
            switch (motorType)
            {
                case MotorType.Diesel:
                    actualDamage *= 0.8f; // Дизельные двигатели более прочные
                    break;
                case MotorType.Jet:
                    actualDamage *= 1.3f; // Реактивные двигатели очень хрупкие
                    break;
            }
            
            base.TakeDamage(actualDamage);
            
            // При сильном уроне останавливаем двигатель
            if (currentHealth < maxHealth * 0.3f)
            {
                StopEngine();
            }
        }
    }
    
    /// <summary>
    /// Типы двигателей
    /// </summary>
    public enum MotorType
    {
        Electric,   // Электрический - тихий, экономичный
        Gasoline,   // Бензиновый - мощный, шумный
        Diesel,     // Дизельный - очень мощный, медленный
        Jet         // Реактивный - максимальная мощность, высокий расход
    }
}
