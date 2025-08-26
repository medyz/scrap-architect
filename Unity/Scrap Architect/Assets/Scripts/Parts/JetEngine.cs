using UnityEngine;

namespace ScrapArchitect.Parts
{
    /// <summary>
    /// Реактивный двигатель - мощный воздушный движитель
    /// </summary>
    public class JetEngine : PartBase
    {
        [Header("Jet Engine Settings")]
        public JetEngineType engineType = JetEngineType.Standard;
        public float engineThrust = 2000f;
        public float engineEfficiency = 0.75f;
        public float engineFuelConsumption = 1f;
        public float engineHeat = 0f;
        public float maxHeat = 100f;

        [Header("Jet Engine Physics")]
        public float engineRPM = 0f;
        public float maxRPM = 5000f;
        public float engineStartupTime = 3f;
        public float engineShutdownTime = 2f;

        [Header("Jet Engine Visual")]
        public Transform engineExhaust;
        public Material engineMaterial;
        public bool enableExhaustEffect = true;
        public ParticleSystem exhaustParticles;

        // Компоненты двигателя
        private bool isEngineActive = false;
        private bool isEngineStarting = false;
        private bool isEngineShuttingDown = false;
        private float currentRPM = 0f;
        private float startupProgress = 0f;
        private float shutdownProgress = 0f;
        private Vector3 thrustDirection;

        public enum JetEngineType
        {
            Standard,    // Стандартный реактивный двигатель
            HighThrust,  // Высокотемпературный двигатель
            LowConsumption, // Экономичный двигатель
            Military     // Военный двигатель
        }

        protected override void InitializePart()
        {
            base.InitializePart();
            
            // Настройка типа двигателя
            ConfigureEngineType();
            
            // Создание компонентов двигателя
            CreateEngineComponents();
            
            Debug.Log($"Jet engine initialized: {engineType} type");
        }

        /// <summary>
        /// Специфичное действие для реактивного двигателя
        /// </summary>
        protected override void OnPartSpecificAction()
        {
            // Логика специфичная для реактивного двигателя
            if (isEngineActive)
            {
                UpdateEngineOperation();
                UpdateEngineVisuals();
            }
        }

        /// <summary>
        /// Настройка типа двигателя
        /// </summary>
        private void ConfigureEngineType()
        {
            switch (engineType)
            {
                case JetEngineType.Standard:
                    engineThrust = 2000f;
                    engineEfficiency = 0.75f;
                    engineFuelConsumption = 1f;
                    maxHeat = 100f;
                    maxRPM = 5000f;
                    engineStartupTime = 3f;
                    break;
                    
                case JetEngineType.HighThrust:
                    engineThrust = 3500f;
                    engineEfficiency = 0.65f;
                    engineFuelConsumption = 1.5f;
                    maxHeat = 150f;
                    maxRPM = 6000f;
                    engineStartupTime = 4f;
                    break;
                    
                case JetEngineType.LowConsumption:
                    engineThrust = 1200f;
                    engineEfficiency = 0.9f;
                    engineFuelConsumption = 0.6f;
                    maxHeat = 80f;
                    maxRPM = 4000f;
                    engineStartupTime = 2f;
                    break;
                    
                case JetEngineType.Military:
                    engineThrust = 4000f;
                    engineEfficiency = 0.7f;
                    engineFuelConsumption = 2f;
                    maxHeat = 200f;
                    maxRPM = 7000f;
                    engineStartupTime = 5f;
                    break;
            }
        }

        /// <summary>
        /// Создание компонентов двигателя
        /// </summary>
        private void CreateEngineComponents()
        {
            // Создание сопла двигателя
            if (engineExhaust == null)
            {
                CreateEngineExhaust();
            }

            // Настройка направления тяги
            thrustDirection = transform.forward;

            // Применение материала
            if (engineMaterial != null)
            {
                Renderer[] renderers = GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    renderer.material = engineMaterial;
                }
            }

            // Создание системы частиц для выхлопа
            if (enableExhaustEffect && exhaustParticles == null)
            {
                CreateExhaustParticles();
            }
        }

        /// <summary>
        /// Создание сопла двигателя
        /// </summary>
        private void CreateEngineExhaust()
        {
            GameObject exhaust = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            exhaust.name = "EngineExhaust";
            exhaust.transform.SetParent(transform);
            exhaust.transform.localPosition = Vector3.zero;
            exhaust.transform.localRotation = Quaternion.identity;
            exhaust.transform.localScale = new Vector3(0.3f, 0.5f, 0.3f);

            // Удаление коллайдера у сопла
            DestroyImmediate(exhaust.GetComponent<Collider>());

            engineExhaust = exhaust.transform;
        }

        /// <summary>
        /// Создание системы частиц для выхлопа
        /// </summary>
        private void CreateExhaustParticles()
        {
            GameObject particleObj = new GameObject("ExhaustParticles");
            particleObj.transform.SetParent(engineExhaust);
            particleObj.transform.localPosition = new Vector3(0, 0, 0.3f);

            exhaustParticles = particleObj.AddComponent<ParticleSystem>();
            
            // Настройка системы частиц
            var main = exhaustParticles.main;
            main.startLifetime = 0.5f;
            main.startSpeed = 5f;
            main.startSize = 0.1f;
            main.maxParticles = 100;
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = exhaustParticles.emission;
            emission.rateOverTime = 0f; // Начинаем с 0

            var shape = exhaustParticles.shape;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 0.1f;

            var colorOverLifetime = exhaustParticles.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.orange, 0.0f), new GradientColorKey(Color.gray, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
            colorOverLifetime.color = gradient;
        }

        /// <summary>
        /// Запустить двигатель
        /// </summary>
        public void StartEngine()
        {
            if (isEngineActive || isEngineStarting) return;

            isEngineStarting = true;
            startupProgress = 0f;
            Debug.Log($"Jet engine starting: {engineType}");
        }

        /// <summary>
        /// Остановить двигатель
        /// </summary>
        public void StopEngine()
        {
            if (!isEngineActive || isEngineShuttingDown) return;

            isEngineShuttingDown = true;
            shutdownProgress = 0f;
            Debug.Log($"Jet engine shutting down: {engineType}");
        }

        /// <summary>
        /// Установить мощность двигателя
        /// </summary>
        public void SetEnginePower(float power)
        {
            if (!isEngineActive) return;

            engineRPM = Mathf.Clamp(power, 0f, maxRPM);
        }

        /// <summary>
        /// Получить тягу двигателя
        /// </summary>
        public Vector3 GetEngineThrust()
        {
            if (!isEngineActive || currentRPM <= 0f) return Vector3.zero;

            // Расчет тяги на основе RPM и эффективности
            float thrustForce = (currentRPM / maxRPM) * engineThrust * engineEfficiency;
            
            // Учет перегрева
            float heatFactor = 1f - (engineHeat / maxHeat) * 0.3f; // Потеря до 30% тяги при перегреве
            thrustForce *= heatFactor;

            return thrustDirection * thrustForce;
        }

        /// <summary>
        /// Применить тягу к объекту
        /// </summary>
        public void ApplyThrust(Rigidbody targetRigidbody)
        {
            if (!isEngineActive) return;

            Vector3 thrust = GetEngineThrust();
            if (thrust.magnitude > 0f)
            {
                targetRigidbody.AddForce(thrust, ForceMode.Force);
            }
        }

        /// <summary>
        /// Получить текущий RPM двигателя
        /// </summary>
        public float GetCurrentRPM()
        {
            return currentRPM;
        }

        /// <summary>
        /// Получить температуру двигателя
        /// </summary>
        public float GetEngineHeat()
        {
            return engineHeat;
        }

        /// <summary>
        /// Получить максимальную температуру
        /// </summary>
        public float GetMaxHeat()
        {
            return maxHeat;
        }

        /// <summary>
        /// Получить тип двигателя
        /// </summary>
        public JetEngineType GetEngineType()
        {
            return engineType;
        }

        /// <summary>
        /// Получить информацию о двигателе
        /// </summary>
        public override string GetPartInfo()
        {
            string info = base.GetPartInfo();
            info += $"\nТип: {GetLocalizedEngineType()}";
            info += $"\nТяга: {engineThrust}N";
            info += $"\nМакс. RPM: {maxRPM}";
            info += $"\nЭффективность: {engineEfficiency:P0}";
            info += $"\nРасход топлива: {engineFuelConsumption}";
            info += $"\nТемпература: {engineHeat:F1}°C";
            return info;
        }

        /// <summary>
        /// Получить локализованное название типа двигателя
        /// </summary>
        private string GetLocalizedEngineType()
        {
            return engineType switch
            {
                JetEngineType.Standard => "Стандартный",
                JetEngineType.HighThrust => "Высокотемпературный",
                JetEngineType.LowConsumption => "Экономичный",
                JetEngineType.Military => "Военный",
                _ => engineType.ToString()
            };
        }

        /// <summary>
        /// Получить стоимость двигателя
        /// </summary>
        public int GetPartCost()
        {
            return engineType switch
            {
                JetEngineType.Standard => 1500,
                JetEngineType.HighThrust => 2500,
                JetEngineType.LowConsumption => 2000,
                JetEngineType.Military => 3000,
                _ => 1500
            };
        }

        /// <summary>
        /// Получить вес двигателя
        /// </summary>
        public float GetPartWeight()
        {
            return engineType switch
            {
                JetEngineType.Standard => 40f,
                JetEngineType.HighThrust => 60f,
                JetEngineType.LowConsumption => 35f,
                JetEngineType.Military => 70f,
                _ => 40f
            };
        }

        private void Update()
        {
            // Обработка запуска двигателя
            if (isEngineStarting)
            {
                startupProgress += Time.deltaTime / engineStartupTime;
                if (startupProgress >= 1f)
                {
                    isEngineStarting = false;
                    isEngineActive = true;
                    Debug.Log($"Jet engine started: {engineType}");
                }
            }

            // Обработка остановки двигателя
            if (isEngineShuttingDown)
            {
                shutdownProgress += Time.deltaTime / engineShutdownTime;
                if (shutdownProgress >= 1f)
                {
                    isEngineShuttingDown = false;
                    isEngineActive = false;
                    currentRPM = 0f;
                    engineHeat = 0f;
                    Debug.Log($"Jet engine stopped: {engineType}");
                }
            }

            // Обновление RPM
            if (isEngineActive)
            {
                currentRPM = Mathf.Lerp(currentRPM, engineRPM, Time.deltaTime * 2f);
                
                // Нагрев двигателя
                if (currentRPM > 0f)
                {
                    engineHeat += Time.deltaTime * (currentRPM / maxRPM) * 10f;
                    engineHeat = Mathf.Min(engineHeat, maxHeat);
                }
                else
                {
                    // Охлаждение при выключенном двигателе
                    engineHeat -= Time.deltaTime * 5f;
                    engineHeat = Mathf.Max(engineHeat, 0f);
                }
            }

            // Обновление системы частиц
            if (exhaustParticles != null)
            {
                var emission = exhaustParticles.emission;
                if (isEngineActive && currentRPM > 0f)
                {
                    emission.rateOverTime = (currentRPM / maxRPM) * 50f;
                }
                else
                {
                    emission.rateOverTime = 0f;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Отображение направления тяги в редакторе
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, thrustDirection * 3f);
            
            // Отображение температуры
            Gizmos.color = Color.Lerp(Color.blue, Color.red, engineHeat / maxHeat);
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
}
