using UnityEngine;

namespace ScrapArchitect.Parts
{
    /// <summary>
    /// Датчик - электронный компонент для измерения различных параметров
    /// </summary>
    public class Sensor : PartBase
    {
        [Header("Sensor Settings")]
        public SensorType sensorType = SensorType.Distance;
        public float sensorRange = 10f;
        public float sensorAccuracy = 0.1f;
        public float sensorUpdateRate = 10f;
        public bool sensorActive = false;

        [Header("Sensor Physics")]
        public LayerMask sensorLayerMask = -1;
        public Transform sensorEmitter;
        public Transform sensorReceiver;
        public Material sensorMaterial;
        public bool enableSensorEffect = true;
        public ParticleSystem sensorParticles;

        [Header("Sensor Output")]
        public float sensorValue = 0f;
        public float sensorMinValue = 0f;
        public float sensorMaxValue = 100f;
        public bool sensorTriggered = false;

        // Компоненты датчика
        private float lastUpdateTime = 0f;
        private RaycastHit sensorHit;
        private bool isInitialized = false;

        public enum SensorType
        {
            Distance,    // Датчик расстояния
            Proximity,   // Датчик приближения
            Pressure,    // Датчик давления
            Temperature, // Датчик температуры
            Light,       // Датчик освещенности
            Motion       // Датчик движения
        }

        protected override void InitializePart()
        {
            base.InitializePart();
            
            // Настройка типа датчика
            ConfigureSensorType();
            
            // Создание компонентов датчика
            CreateSensorComponents();
            
            Debug.Log($"Sensor initialized: {sensorType} type");
        }

        /// <summary>
        /// Специфичное действие для сенсора
        /// </summary>
        protected override void OnPartSpecificAction()
        {
            // Логика специфичная для сенсора
            if (sensorActive)
            {
                UpdateSensorReading();
                UpdateSensorVisuals();
            }
        }

        /// <summary>
        /// Настройка типа датчика
        /// </summary>
        private void ConfigureSensorType()
        {
            switch (sensorType)
            {
                case SensorType.Distance:
                    sensorRange = 10f;
                    sensorAccuracy = 0.1f;
                    sensorUpdateRate = 10f;
                    sensorMinValue = 0f;
                    sensorMaxValue = 10f;
                    break;
                    
                case SensorType.Proximity:
                    sensorRange = 5f;
                    sensorAccuracy = 0.05f;
                    sensorUpdateRate = 20f;
                    sensorMinValue = 0f;
                    sensorMaxValue = 5f;
                    break;
                    
                case SensorType.Pressure:
                    sensorRange = 2f;
                    sensorAccuracy = 0.01f;
                    sensorUpdateRate = 5f;
                    sensorMinValue = 0f;
                    sensorMaxValue = 100f;
                    break;
                    
                case SensorType.Temperature:
                    sensorRange = 3f;
                    sensorAccuracy = 0.5f;
                    sensorUpdateRate = 2f;
                    sensorMinValue = -50f;
                    sensorMaxValue = 150f;
                    break;
                    
                case SensorType.Light:
                    sensorRange = 8f;
                    sensorAccuracy = 0.1f;
                    sensorUpdateRate = 15f;
                    sensorMinValue = 0f;
                    sensorMaxValue = 100f;
                    break;
                    
                case SensorType.Motion:
                    sensorRange = 6f;
                    sensorAccuracy = 0.1f;
                    sensorUpdateRate = 30f;
                    sensorMinValue = 0f;
                    sensorMaxValue = 1f;
                    break;
            }
        }

        /// <summary>
        /// Создание компонентов датчика
        /// </summary>
        private void CreateSensorComponents()
        {
            // Создание излучателя датчика
            if (sensorEmitter == null)
            {
                CreateSensorEmitter();
            }

            // Создание приемника датчика
            if (sensorReceiver == null)
            {
                CreateSensorReceiver();
            }

            // Применение материала
            if (sensorMaterial != null)
            {
                if (sensorEmitter != null)
                {
                    sensorEmitter.GetComponent<Renderer>().material = sensorMaterial;
                }
                if (sensorReceiver != null)
                {
                    sensorReceiver.GetComponent<Renderer>().material = sensorMaterial;
                }
            }

            // Создание системы частиц для датчика
            if (enableSensorEffect && sensorParticles == null)
            {
                CreateSensorParticles();
            }

            isInitialized = true;
        }

        /// <summary>
        /// Создание излучателя датчика
        /// </summary>
        private void CreateSensorEmitter()
        {
            GameObject emitter = GameObject.CreatePrimitive(PrimitiveType.Cube);
            emitter.name = "SensorEmitter";
            emitter.transform.SetParent(transform);
            emitter.transform.localPosition = Vector3.zero;
            emitter.transform.localRotation = Quaternion.identity;
            emitter.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            // Удаление коллайдера у излучателя
            DestroyImmediate(emitter.GetComponent<Collider>());

            sensorEmitter = emitter.transform;
        }

        /// <summary>
        /// Создание приемника датчика
        /// </summary>
        private void CreateSensorReceiver()
        {
            GameObject receiver = GameObject.CreatePrimitive(PrimitiveType.Cube);
            receiver.name = "SensorReceiver";
            receiver.transform.SetParent(transform);
            receiver.transform.localPosition = new Vector3(0, 0, 0.05f);
            receiver.transform.localRotation = Quaternion.identity;
            receiver.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);

            // Удаление коллайдера у приемника
            DestroyImmediate(receiver.GetComponent<Collider>());

            sensorReceiver = receiver.transform;
        }

        /// <summary>
        /// Создание системы частиц для датчика
        /// </summary>
        private void CreateSensorParticles()
        {
            GameObject particleObj = new GameObject("SensorParticles");
            particleObj.transform.SetParent(transform);
            particleObj.transform.localPosition = Vector3.zero;

            sensorParticles = particleObj.AddComponent<ParticleSystem>();
            
            // Настройка системы частиц
            var main = sensorParticles.main;
            main.startLifetime = 0.3f;
            main.startSpeed = 2f;
            main.startSize = 0.02f;
            main.maxParticles = 50;
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = sensorParticles.emission;
            emission.rateOverTime = 0f; // Начинаем с 0

            var shape = sensorParticles.shape;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 15f;
            shape.radius = 0.05f;

            var colorOverLifetime = sensorParticles.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.yellow, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
            colorOverLifetime.color = gradient;
        }

        /// <summary>
        /// Активировать датчик
        /// </summary>
        public void ActivateSensor()
        {
            sensorActive = true;
            Debug.Log($"Sensor activated: {sensorType}");
        }

        /// <summary>
        /// Деактивировать датчик
        /// </summary>
        public void DeactivateSensor()
        {
            sensorActive = false;
            sensorValue = 0f;
            sensorTriggered = false;
            Debug.Log($"Sensor deactivated: {sensorType}");
        }

        /// <summary>
        /// Получить текущее значение датчика
        /// </summary>
        public float GetSensorValue()
        {
            return sensorValue;
        }

        /// <summary>
        /// Получить нормализованное значение датчика (0-1)
        /// </summary>
        public float GetNormalizedValue()
        {
            if (sensorMaxValue == sensorMinValue) return 0f;
            return Mathf.Clamp01((sensorValue - sensorMinValue) / (sensorMaxValue - sensorMinValue));
        }

        /// <summary>
        /// Проверить, сработал ли датчик
        /// </summary>
        public bool IsTriggered()
        {
            return sensorTriggered;
        }

        /// <summary>
        /// Получить тип датчика
        /// </summary>
        public SensorType GetSensorType()
        {
            return sensorType;
        }

        /// <summary>
        /// Установить диапазон датчика
        /// </summary>
        public void SetSensorRange(float min, float max)
        {
            sensorMinValue = min;
            sensorMaxValue = max;
        }

        /// <summary>
        /// Получить информацию о датчике
        /// </summary>
        public override string GetPartInfo()
        {
            string info = base.GetPartInfo();
            info += $"\nТип: {GetLocalizedSensorType()}";
            info += $"\nДиапазон: {sensorRange}м";
            info += $"\nТочность: {sensorAccuracy}";
            info += $"\nЧастота: {sensorUpdateRate} Гц";
            info += $"\nЗначение: {sensorValue:F2}";
            info += $"\nСтатус: {(sensorActive ? "Активен" : "Неактивен")}";
            return info;
        }

        /// <summary>
        /// Получить локализованное название типа датчика
        /// </summary>
        private string GetLocalizedSensorType()
        {
            return sensorType switch
            {
                SensorType.Distance => "Датчик расстояния",
                SensorType.Proximity => "Датчик приближения",
                SensorType.Pressure => "Датчик давления",
                SensorType.Temperature => "Датчик температуры",
                SensorType.Light => "Датчик освещенности",
                SensorType.Motion => "Датчик движения",
                _ => sensorType.ToString()
            };
        }

        /// <summary>
        /// Получить стоимость датчика
        /// </summary>
        public int GetPartCost()
        {
            return sensorType switch
            {
                SensorType.Distance => 150,
                SensorType.Proximity => 200,
                SensorType.Pressure => 300,
                SensorType.Temperature => 250,
                SensorType.Light => 180,
                SensorType.Motion => 220,
                _ => 150
            };
        }

        /// <summary>
        /// Получить вес датчика
        /// </summary>
        public float GetPartWeight()
        {
            return sensorType switch
            {
                SensorType.Distance => 2f,
                SensorType.Proximity => 2.5f,
                SensorType.Pressure => 3f,
                SensorType.Temperature => 2.8f,
                SensorType.Light => 2.2f,
                SensorType.Motion => 2.7f,
                _ => 2f
            };
        }

        private void Update()
        {
            if (!isInitialized || !sensorActive) return;

            // Обновление датчика с заданной частотой
            if (Time.time - lastUpdateTime >= 1f / sensorUpdateRate)
            {
                UpdateSensorValue();
                lastUpdateTime = Time.time;
            }

            // Обновление системы частиц
            if (sensorParticles != null)
            {
                var emission = sensorParticles.emission;
                if (sensorActive && sensorTriggered)
                {
                    emission.rateOverTime = 20f;
                }
                else
                {
                    emission.rateOverTime = 5f;
                }
            }
        }

        /// <summary>
        /// Обновление значения датчика
        /// </summary>
        private void UpdateSensorValue()
        {
            switch (sensorType)
            {
                case SensorType.Distance:
                    UpdateDistanceSensor();
                    break;
                    
                case SensorType.Proximity:
                    UpdateProximitySensor();
                    break;
                    
                case SensorType.Pressure:
                    UpdatePressureSensor();
                    break;
                    
                case SensorType.Temperature:
                    UpdateTemperatureSensor();
                    break;
                    
                case SensorType.Light:
                    UpdateLightSensor();
                    break;
                    
                case SensorType.Motion:
                    UpdateMotionSensor();
                    break;
            }
        }

        /// <summary>
        /// Обновление датчика расстояния
        /// </summary>
        private void UpdateDistanceSensor()
        {
            if (Physics.Raycast(transform.position, transform.forward, out sensorHit, sensorRange, sensorLayerMask))
            {
                sensorValue = sensorHit.distance;
                sensorTriggered = true;
            }
            else
            {
                sensorValue = sensorRange;
                sensorTriggered = false;
            }
        }

        /// <summary>
        /// Обновление датчика приближения
        /// </summary>
        private void UpdateProximitySensor()
        {
            if (Physics.Raycast(transform.position, transform.forward, out sensorHit, sensorRange, sensorLayerMask))
            {
                sensorValue = sensorHit.distance;
                sensorTriggered = sensorValue <= sensorRange * 0.5f;
            }
            else
            {
                sensorValue = sensorRange;
                sensorTriggered = false;
            }
        }

        /// <summary>
        /// Обновление датчика давления
        /// </summary>
        private void UpdatePressureSensor()
        {
            // Симуляция давления на основе близости объектов
            Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, sensorRange, sensorLayerMask);
            float pressure = 0f;
            
            foreach (Collider col in nearbyColliders)
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance > 0)
                {
                    pressure += 1f / distance;
                }
            }
            
            sensorValue = Mathf.Clamp(pressure, sensorMinValue, sensorMaxValue);
            sensorTriggered = pressure > sensorMaxValue * 0.7f;
        }

        /// <summary>
        /// Обновление датчика температуры
        /// </summary>
        private void UpdateTemperatureSensor()
        {
            // Симуляция температуры на основе времени и случайности
            float baseTemp = 20f + Mathf.Sin(Time.time * 0.1f) * 10f;
            float randomTemp = Random.Range(-5f, 5f);
            sensorValue = Mathf.Clamp(baseTemp + randomTemp, sensorMinValue, sensorMaxValue);
            sensorTriggered = sensorValue > 30f || sensorValue < 10f;
        }

        /// <summary>
        /// Обновление датчика освещенности
        /// </summary>
        private void UpdateLightSensor()
        {
            // Симуляция освещенности на основе времени дня
            float timeOfDay = (Time.time % 120f) / 120f; // 2-минутный цикл дня
            float lightLevel = Mathf.Sin(timeOfDay * Mathf.PI) * 50f + 50f;
            sensorValue = Mathf.Clamp(lightLevel, sensorMinValue, sensorMaxValue);
            sensorTriggered = lightLevel > 80f || lightLevel < 20f;
        }

        /// <summary>
        /// Обновление датчика движения
        /// </summary>
        private void UpdateMotionSensor()
        {
            // Симуляция движения на основе близости движущихся объектов
            Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, sensorRange, sensorLayerMask);
            bool motionDetected = false;
            
            foreach (Collider col in nearbyColliders)
            {
                Rigidbody rb = col.GetComponent<Rigidbody>();
                if (rb != null && rb.velocity.magnitude > 0.1f)
                {
                    motionDetected = true;
                    break;
                }
            }
            
            sensorValue = motionDetected ? 1f : 0f;
            sensorTriggered = motionDetected;
        }

        private void OnDrawGizmosSelected()
        {
            if (!sensorActive) return;

            // Отображение луча датчика в редакторе
            Gizmos.color = sensorTriggered ? Color.red : Color.green;
            Gizmos.DrawRay(transform.position, transform.forward * sensorRange);
            
            // Отображение зоны действия
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sensorRange);
        }
    }
}
