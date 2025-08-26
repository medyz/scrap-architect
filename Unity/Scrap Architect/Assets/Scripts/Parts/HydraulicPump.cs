using UnityEngine;

namespace ScrapArchitect.Parts
{
    /// <summary>
    /// Гидравлический насос - устройство для создания давления в гидравлических системах
    /// </summary>
    public class HydraulicPump : PartBase
    {
        [Header("Hydraulic Settings")]
        public HydraulicType pumpType = HydraulicType.Standard;
        public float pumpPressure = 100f;
        public float pumpFlow = 10f;
        public float pumpEfficiency = 0.8f;
        public float pumpPower = 1000f;

        [Header("Hydraulic Physics")]
        public float pumpRPM = 0f;
        public float maxRPM = 3000f;
        public float pumpTemperature = 20f;
        public float maxTemperature = 80f;

        [Header("Hydraulic Visual")]
        public Transform pumpBody;
        public Transform pumpRotor;
        public Material pumpMaterial;
        public bool enableHydraulicEffect = true;
        public ParticleSystem hydraulicParticles;

        // Компоненты насоса
        private bool isPumpActive = false;
        private bool isPumpStarting = false;
        private bool isPumpStopping = false;
        private float currentRPM = 0f;
        private float startupProgress = 0f;
        private float shutdownProgress = 0f;
        private float startupTime = 2f;
        private float shutdownTime = 1f;

        public enum HydraulicType
        {
            Standard,    // Стандартный гидравлический насос
            HighPressure, // Высоконапорный насос
            HighFlow,    // Высокопроизводительный насос
            Efficient    // Эффективный насос
        }

        protected override void InitializePart()
        {
            base.InitializePart();
            
            // Настройка типа насоса
            ConfigurePumpType();
            
            // Создание компонентов насоса
            CreatePumpComponents();
            
            Debug.Log($"Hydraulic pump initialized: {pumpType} type");
        }

        /// <summary>
        /// Специфичное действие для гидравлического насоса
        /// </summary>
        protected override void OnPartSpecificAction()
        {
            // Логика специфичная для гидравлического насоса
            if (isPumpActive)
            {
                UpdatePumpOperation();
                UpdatePumpVisuals();
            }
        }

        /// <summary>
        /// Настройка типа насоса
        /// </summary>
        private void ConfigurePumpType()
        {
            switch (pumpType)
            {
                case HydraulicType.Standard:
                    pumpPressure = 100f;
                    pumpFlow = 10f;
                    pumpEfficiency = 0.8f;
                    pumpPower = 1000f;
                    maxRPM = 3000f;
                    startupTime = 2f;
                    break;
                    
                case HydraulicType.HighPressure:
                    pumpPressure = 200f;
                    pumpFlow = 5f;
                    pumpEfficiency = 0.7f;
                    pumpPower = 1500f;
                    maxRPM = 4000f;
                    startupTime = 3f;
                    break;
                    
                case HydraulicType.HighFlow:
                    pumpPressure = 50f;
                    pumpFlow = 25f;
                    pumpEfficiency = 0.75f;
                    pumpPower = 1200f;
                    maxRPM = 2500f;
                    startupTime = 1.5f;
                    break;
                    
                case HydraulicType.Efficient:
                    pumpPressure = 80f;
                    pumpFlow = 15f;
                    pumpEfficiency = 0.95f;
                    pumpPower = 800f;
                    maxRPM = 2000f;
                    startupTime = 1f;
                    break;
            }
        }

        /// <summary>
        /// Создание компонентов насоса
        /// </summary>
        private void CreatePumpComponents()
        {
            // Создание корпуса насоса
            if (pumpBody == null)
            {
                CreatePumpBody();
            }

            // Создание ротора насоса
            if (pumpRotor == null)
            {
                CreatePumpRotor();
            }

            // Применение материала
            if (pumpMaterial != null)
            {
                if (pumpBody != null)
                {
                    pumpBody.GetComponent<Renderer>().material = pumpMaterial;
                }
                if (pumpRotor != null)
                {
                    pumpRotor.GetComponent<Renderer>().material = pumpMaterial;
                }
            }

            // Создание системы частиц для гидравлики
            if (enableHydraulicEffect && hydraulicParticles == null)
            {
                CreateHydraulicParticles();
            }
        }

        /// <summary>
        /// Создание корпуса насоса
        /// </summary>
        private void CreatePumpBody()
        {
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            body.name = "PumpBody";
            body.transform.SetParent(transform);
            body.transform.localPosition = Vector3.zero;
            body.transform.localRotation = Quaternion.identity;
            body.transform.localScale = new Vector3(0.2f, 0.3f, 0.2f);

            // Удаление коллайдера у корпуса
            DestroyImmediate(body.GetComponent<Collider>());

            pumpBody = body.transform;
        }

        /// <summary>
        /// Создание ротора насоса
        /// </summary>
        private void CreatePumpRotor()
        {
            GameObject rotor = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            rotor.name = "PumpRotor";
            rotor.transform.SetParent(transform);
            rotor.transform.localPosition = Vector3.zero;
            rotor.transform.localRotation = Quaternion.identity;
            rotor.transform.localScale = new Vector3(0.15f, 0.25f, 0.15f);

            // Удаление коллайдера у ротора
            DestroyImmediate(rotor.GetComponent<Collider>());

            pumpRotor = rotor.transform;
        }

        /// <summary>
        /// Создание системы частиц для гидравлики
        /// </summary>
        private void CreateHydraulicParticles()
        {
            GameObject particleObj = new GameObject("HydraulicParticles");
            particleObj.transform.SetParent(transform);
            particleObj.transform.localPosition = new Vector3(0, 0.2f, 0);

            hydraulicParticles = particleObj.AddComponent<ParticleSystem>();
            
            // Настройка системы частиц
            var main = hydraulicParticles.main;
            main.startLifetime = 0.5f;
            main.startSpeed = 3f;
            main.startSize = 0.03f;
            main.maxParticles = 100;
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = hydraulicParticles.emission;
            emission.rateOverTime = 0f; // Начинаем с 0

            var shape = hydraulicParticles.shape;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 0.1f;

            var colorOverLifetime = hydraulicParticles.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.orange, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
            colorOverLifetime.color = gradient;
        }

        /// <summary>
        /// Запустить насос
        /// </summary>
        public void StartPump()
        {
            if (isPumpActive || isPumpStarting) return;

            isPumpStarting = true;
            startupProgress = 0f;
            Debug.Log($"Hydraulic pump starting: {pumpType}");
        }

        /// <summary>
        /// Остановить насос
        /// </summary>
        public void StopPump()
        {
            if (!isPumpActive || isPumpStopping) return;

            isPumpStopping = true;
            shutdownProgress = 0f;
            Debug.Log($"Hydraulic pump stopping: {pumpType}");
        }

        /// <summary>
        /// Установить скорость насоса
        /// </summary>
        public void SetPumpSpeed(float speed)
        {
            if (!isPumpActive) return;

            pumpRPM = Mathf.Clamp(speed, 0f, maxRPM);
        }

        /// <summary>
        /// Получить текущее давление насоса
        /// </summary>
        public float GetPumpPressure()
        {
            if (!isPumpActive || currentRPM <= 0f) return 0f;

            // Расчет давления на основе RPM и эффективности
            float pressure = (currentRPM / maxRPM) * pumpPressure * pumpEfficiency;
            
            // Учет температуры
            float tempFactor = 1f - (pumpTemperature / maxTemperature) * 0.2f; // Потеря до 20% давления при перегреве
            pressure *= tempFactor;

            return pressure;
        }

        /// <summary>
        /// Получить текущий поток насоса
        /// </summary>
        public float GetPumpFlow()
        {
            if (!isPumpActive || currentRPM <= 0f) return 0f;

            // Расчет потока на основе RPM и эффективности
            float flow = (currentRPM / maxRPM) * pumpFlow * pumpEfficiency;
            
            // Учет температуры
            float tempFactor = 1f - (pumpTemperature / maxTemperature) * 0.15f; // Потеря до 15% потока при перегреве
            flow *= tempFactor;

            return flow;
        }

        /// <summary>
        /// Получить текущий RPM насоса
        /// </summary>
        public float GetCurrentRPM()
        {
            return currentRPM;
        }

        /// <summary>
        /// Получить температуру насоса
        /// </summary>
        public float GetPumpTemperature()
        {
            return pumpTemperature;
        }

        /// <summary>
        /// Получить максимальную температуру
        /// </summary>
        public float GetMaxTemperature()
        {
            return maxTemperature;
        }

        /// <summary>
        /// Получить тип насоса
        /// </summary>
        public HydraulicType GetPumpType()
        {
            return pumpType;
        }

        /// <summary>
        /// Получить информацию о насосе
        /// </summary>
        public override string GetPartInfo()
        {
            string info = base.GetPartInfo();
            info += $"\nТип: {GetLocalizedPumpType()}";
            info += $"\nДавление: {pumpPressure} бар";
            info += $"\nПоток: {pumpFlow} л/мин";
            info += $"\nМощность: {pumpPower} Вт";
            info += $"\nЭффективность: {pumpEfficiency:P0}";
            info += $"\nТемпература: {pumpTemperature:F1}°C";
            return info;
        }

        /// <summary>
        /// Получить локализованное название типа насоса
        /// </summary>
        private string GetLocalizedPumpType()
        {
            return pumpType switch
            {
                HydraulicType.Standard => "Стандартный",
                HydraulicType.HighPressure => "Высоконапорный",
                HydraulicType.HighFlow => "Высокопроизводительный",
                HydraulicType.Efficient => "Эффективный",
                _ => pumpType.ToString()
            };
        }

        /// <summary>
        /// Получить стоимость насоса
        /// </summary>
        public int GetPartCost()
        {
            return pumpType switch
            {
                HydraulicType.Standard => 300,
                HydraulicType.HighPressure => 600,
                HydraulicType.HighFlow => 500,
                HydraulicType.Efficient => 400,
                _ => 300
            };
        }

        /// <summary>
        /// Получить вес насоса
        /// </summary>
        public float GetPartWeight()
        {
            return pumpType switch
            {
                HydraulicType.Standard => 12f,
                HydraulicType.HighPressure => 20f,
                HydraulicType.HighFlow => 18f,
                HydraulicType.Efficient => 10f,
                _ => 12f
            };
        }

        private void Update()
        {
            // Обработка запуска насоса
            if (isPumpStarting)
            {
                startupProgress += Time.deltaTime / startupTime;
                if (startupProgress >= 1f)
                {
                    isPumpStarting = false;
                    isPumpActive = true;
                    Debug.Log($"Hydraulic pump started: {pumpType}");
                }
            }

            // Обработка остановки насоса
            if (isPumpStopping)
            {
                shutdownProgress += Time.deltaTime / shutdownTime;
                if (shutdownProgress >= 1f)
                {
                    isPumpStopping = false;
                    isPumpActive = false;
                    currentRPM = 0f;
                    pumpTemperature = 20f;
                    Debug.Log($"Hydraulic pump stopped: {pumpType}");
                }
            }

            // Обновление RPM
            if (isPumpActive)
            {
                currentRPM = Mathf.Lerp(currentRPM, pumpRPM, Time.deltaTime * 2f);
                
                // Нагрев насоса
                if (currentRPM > 0f)
                {
                    pumpTemperature += Time.deltaTime * (currentRPM / maxRPM) * 5f;
                    pumpTemperature = Mathf.Min(pumpTemperature, maxTemperature);
                }
                else
                {
                    // Охлаждение при выключенном насосе
                    pumpTemperature -= Time.deltaTime * 2f;
                    pumpTemperature = Mathf.Max(pumpTemperature, 20f);
                }
            }

            // Вращение ротора
            if (pumpRotor != null && isPumpActive)
            {
                float rotationSpeed = (currentRPM / 60f) * 360f; // RPM в градусы в секунду
                pumpRotor.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            }

            // Обновление системы частиц
            if (hydraulicParticles != null)
            {
                var emission = hydraulicParticles.emission;
                if (isPumpActive && currentRPM > 0f)
                {
                    emission.rateOverTime = (currentRPM / maxRPM) * 30f;
                }
                else
                {
                    emission.rateOverTime = 0f;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Отображение направления потока в редакторе
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.up * 0.5f);
            
            // Отображение температуры
            Gizmos.color = Color.Lerp(Color.blue, Color.red, pumpTemperature / maxTemperature);
            Gizmos.DrawWireSphere(transform.position, 0.3f);
        }
    }
}
