using UnityEngine;

namespace ScrapArchitect.Parts
{
    /// <summary>
    /// Сварочный аппарат - продвинутый инструмент для соединения металлических деталей
    /// </summary>
    public class WeldingMachine : PartBase
    {
        [Header("Welding Settings")]
        public WeldingMachineType weldingType = WeldingMachineType.Standard;
        public float weldingPower = 300f;
        public float weldingRange = 2f;
        public float weldingSpeed = 0.5f;
        public float weldStrength = 1000f;
        public bool weldingActive = false;

        [Header("Welding Physics")]
        public LayerMask weldableLayers = -1;
        public Transform weldingTorch;
        public Transform weldingCable;
        public Material weldingMaterial;
        public bool enableWeldingEffect = true;
        public ParticleSystem weldingParticles;
        public Light weldingLight;

        [Header("Welding Output")]
        public float currentPower = 0f;
        public float temperature = 20f;
        public float maxTemperature = 400f;
        public bool isWelding = false;
        public float weldProgress = 0f;

        // Компоненты сварочного аппарата
        private RaycastHit weldingHit;
        private bool isInitialized = false;
        private float lastWeldTime = 0f;
        private GameObject currentWeldTarget = null;

        public enum WeldingMachineType
        {
            Standard,    // Стандартный сварочный аппарат
            Arc,         // Дуговая сварка
            Spot,        // Точечная сварка
            Industrial   // Промышленный сварочный аппарат
        }

        protected override void InitializePart()
        {
            base.InitializePart();
            
            // Настройка типа сварочного аппарата
            ConfigureWeldingType();
            
            // Создание компонентов сварочного аппарата
            CreateWeldingComponents();
            
            Debug.Log($"Welding machine initialized: {weldingType} type");
        }

        /// <summary>
        /// Специфичное действие для сварочного аппарата
        /// </summary>
        protected override void OnPartSpecificAction()
        {
            // Логика специфичная для сварочного аппарата
            if (weldingActive)
            {
                UpdateWeldingProcess();
                UpdateVisualEffects();
            }
        }

        /// <summary>
        /// Настройка типа сварочного аппарата
        /// </summary>
        private void ConfigureWeldingType()
        {
            switch (weldingType)
            {
                case WeldingMachineType.Standard:
                    weldingPower = 300f;
                    weldingRange = 2f;
                    weldingSpeed = 0.5f;
                    weldStrength = 1000f;
                    maxTemperature = 400f;
                    break;
                    
                case WeldingMachineType.Arc:
                    weldingPower = 500f;
                    weldingRange = 3f;
                    weldingSpeed = 0.3f;
                    weldStrength = 1500f;
                    maxTemperature = 600f;
                    break;
                    
                case WeldingMachineType.Spot:
                    weldingPower = 200f;
                    weldingRange = 1.5f;
                    weldingSpeed = 1f;
                    weldStrength = 800f;
                    maxTemperature = 300f;
                    break;
                    
                case WeldingMachineType.Industrial:
                    weldingPower = 800f;
                    weldingRange = 4f;
                    weldingSpeed = 0.2f;
                    weldStrength = 2000f;
                    maxTemperature = 800f;
                    break;
            }
        }

        /// <summary>
        /// Создание компонентов сварочного аппарата
        /// </summary>
        private void CreateWeldingComponents()
        {
            // Создание сварочной горелки
            if (weldingTorch == null)
            {
                CreateWeldingTorch();
            }

            // Создание сварочного кабеля
            if (weldingCable == null)
            {
                CreateWeldingCable();
            }

            // Создание света для сварки
            if (weldingLight == null)
            {
                CreateWeldingLight();
            }

            // Применение материала
            if (weldingMaterial != null)
            {
                if (weldingTorch != null)
                {
                    weldingTorch.GetComponent<Renderer>().material = weldingMaterial;
                }
                if (weldingCable != null)
                {
                    weldingCable.GetComponent<Renderer>().material = weldingMaterial;
                }
            }

            // Создание системы частиц для сварки
            if (enableWeldingEffect && weldingParticles == null)
            {
                CreateWeldingParticles();
            }

            isInitialized = true;
        }

        /// <summary>
        /// Создание сварочной горелки
        /// </summary>
        private void CreateWeldingTorch()
        {
            GameObject torch = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            torch.name = "WeldingTorch";
            torch.transform.SetParent(transform);
            torch.transform.localPosition = Vector3.zero;
            torch.transform.localRotation = Quaternion.identity;
            torch.transform.localScale = new Vector3(0.05f, 0.3f, 0.05f);

            // Удаление коллайдера у горелки
            DestroyImmediate(torch.GetComponent<Collider>());

            weldingTorch = torch.transform;
        }

        /// <summary>
        /// Создание сварочного кабеля
        /// </summary>
        private void CreateWeldingCable()
        {
            GameObject cable = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cable.name = "WeldingCable";
            cable.transform.SetParent(transform);
            cable.transform.localPosition = new Vector3(0, -0.2f, 0);
            cable.transform.localRotation = Quaternion.Euler(0, 0, 90);
            cable.transform.localScale = new Vector3(0.02f, 0.5f, 0.02f);

            // Удаление коллайдера у кабеля
            DestroyImmediate(cable.GetComponent<Collider>());

            weldingCable = cable.transform;
        }

        /// <summary>
        /// Создание света для сварки
        /// </summary>
        private void CreateWeldingLight()
        {
            GameObject lightObj = new GameObject("WeldingLight");
            lightObj.transform.SetParent(transform);
            lightObj.transform.localPosition = Vector3.zero;

            weldingLight = lightObj.AddComponent<Light>();
            weldingLight.type = LightType.Spot;
            weldingLight.range = weldingRange;
            weldingLight.spotAngle = 30f;
            weldingLight.intensity = 0f;
            weldingLight.color = Color.blue;
            weldingLight.enabled = false;
        }

        /// <summary>
        /// Создание системы частиц для сварки
        /// </summary>
        private void CreateWeldingParticles()
        {
            GameObject particleObj = new GameObject("WeldingParticles");
            particleObj.transform.SetParent(transform);
            particleObj.transform.localPosition = Vector3.zero;

            weldingParticles = particleObj.AddComponent<ParticleSystem>();
            
            // Настройка системы частиц
            var main = weldingParticles.main;
            main.startLifetime = 0.5f;
            main.startSpeed = 3f;
            main.startSize = 0.02f;
            main.maxParticles = 150;
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = weldingParticles.emission;
            emission.rateOverTime = 0f; // Начинаем с 0

            var shape = weldingParticles.shape;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 20f;
            shape.radius = 0.05f;

            var colorOverLifetime = weldingParticles.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.blue, 0.0f), new GradientColorKey(Color.white, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
            colorOverLifetime.color = gradient;
        }

        /// <summary>
        /// Активировать сварочный аппарат
        /// </summary>
        public void ActivateWelding()
        {
            weldingActive = true;
            Debug.Log($"Welding machine activated: {weldingType}");
        }

        /// <summary>
        /// Деактивировать сварочный аппарат
        /// </summary>
        public void DeactivateWelding()
        {
            weldingActive = false;
            currentPower = 0f;
            isWelding = false;
            weldProgress = 0f;
            currentWeldTarget = null;
            
            // Отключение визуальных эффектов
            if (weldingLight != null)
            {
                weldingLight.enabled = false;
            }
            
            Debug.Log($"Welding machine deactivated: {weldingType}");
        }

        /// <summary>
        /// Установить мощность сварочного аппарата
        /// </summary>
        public void SetWeldingPower(float power)
        {
            if (!weldingActive) return;
            
            currentPower = Mathf.Clamp(power, 0f, weldingPower);
            Debug.Log($"Welding power set to: {currentPower}W");
        }

        /// <summary>
        /// Получить текущую мощность сварочного аппарата
        /// </summary>
        public float GetWeldingPower()
        {
            return currentPower;
        }

        /// <summary>
        /// Получить максимальную мощность сварочного аппарата
        /// </summary>
        public float GetMaxWeldingPower()
        {
            return weldingPower;
        }

        /// <summary>
        /// Получить температуру сварочного аппарата
        /// </summary>
        public float GetWeldingTemperature()
        {
            return temperature;
        }

        /// <summary>
        /// Получить прогресс сварки
        /// </summary>
        public float GetWeldProgress()
        {
            return weldProgress;
        }

        /// <summary>
        /// Получить тип сварочного аппарата
        /// </summary>
        public WeldingMachineType GetWeldingType()
        {
            return weldingType;
        }

        /// <summary>
        /// Получить информацию о сварочном аппарате
        /// </summary>
        public override string GetPartInfo()
        {
            string info = base.GetPartInfo();
            info += $"\nТип: {GetLocalizedWeldingType()}";
            info += $"\nМощность: {weldingPower}Вт";
            info += $"\nДальность: {weldingRange}м";
            info += $"\nСкорость: {weldingSpeed} м/с";
            info += $"\nПрочность шва: {weldStrength}Н";
            info += $"\nТемпература: {temperature:F1}°C";
            info += $"\nПрогресс: {weldProgress:P0}";
            info += $"\nСтатус: {(weldingActive ? "Активен" : "Неактивен")}";
            return info;
        }

        /// <summary>
        /// Получить локализованное название типа сварочного аппарата
        /// </summary>
        private string GetLocalizedWeldingType()
        {
            return weldingType switch
            {
                WeldingMachineType.Standard => "Стандартный",
                WeldingMachineType.Arc => "Дуговая сварка",
                WeldingMachineType.Spot => "Точечная сварка",
                WeldingMachineType.Industrial => "Промышленный",
                _ => weldingType.ToString()
            };
        }

        /// <summary>
        /// Получить стоимость сварочного аппарата
        /// </summary>
        public int GetPartCost()
        {
            return weldingType switch
            {
                WeldingMachineType.Standard => 300,
                WeldingMachineType.Arc => 500,
                WeldingMachineType.Spot => 250,
                WeldingMachineType.Industrial => 800,
                _ => 300
            };
        }

        /// <summary>
        /// Получить вес сварочного аппарата
        /// </summary>
        public float GetPartWeight()
        {
            return weldingType switch
            {
                WeldingMachineType.Standard => 4f,
                WeldingMachineType.Arc => 6f,
                WeldingMachineType.Spot => 3f,
                WeldingMachineType.Industrial => 8f,
                _ => 4f
            };
        }

        private void Update()
        {
            if (!isInitialized || !weldingActive) return;

            // Обновление температуры
            UpdateTemperature();

            // Обновление сварочного процесса
            UpdateWeldingProcess();

            // Обновление визуальных эффектов
            UpdateVisualEffects();
        }

        /// <summary>
        /// Обновление температуры сварочного аппарата
        /// </summary>
        private void UpdateTemperature()
        {
            if (currentPower > 0f)
            {
                // Нагрев при работе
                temperature += Time.deltaTime * (currentPower / weldingPower) * 15f;
                temperature = Mathf.Min(temperature, maxTemperature);
            }
            else
            {
                // Охлаждение при выключенном аппарате
                temperature -= Time.deltaTime * 3f;
                temperature = Mathf.Max(temperature, 20f);
            }
        }

        /// <summary>
        /// Обновление сварочного процесса
        /// </summary>
        private void UpdateWeldingProcess()
        {
            if (currentPower > 0f)
            {
                // Проверка попадания сварочной дуги
                if (UnityEngine.Physics.Raycast(transform.position, transform.forward, out weldingHit, weldingRange, weldableLayers))
                {
                    if (!isWelding)
                    {
                        // Начало сварки
                        isWelding = true;
                        currentWeldTarget = weldingHit.collider.gameObject;
                        weldProgress = 0f;
                        Debug.Log($"Started welding: {currentWeldTarget.name}");
                    }

                    // Обновление прогресса сварки
                    if (isWelding && currentWeldTarget == weldingHit.collider.gameObject)
                    {
                        weldProgress += Time.deltaTime * weldingSpeed * (currentPower / weldingPower);
                        weldProgress = Mathf.Clamp01(weldProgress);

                        // Завершение сварки
                        if (weldProgress >= 1f)
                        {
                            CompleteWeld();
                        }
                    }
                }
                else
                {
                    // Прерывание сварки
                    if (isWelding)
                    {
                        isWelding = false;
                        currentWeldTarget = null;
                        Debug.Log("Welding interrupted");
                    }
                }
            }
            else
            {
                // Остановка сварки при выключенном питании
                if (isWelding)
                {
                    isWelding = false;
                    currentWeldTarget = null;
                    Debug.Log("Welding stopped - no power");
                }
            }
        }

        /// <summary>
        /// Завершение сварки
        /// </summary>
        private void CompleteWeld()
        {
            if (currentWeldTarget != null)
            {
                // Здесь можно добавить логику создания соединения между деталями
                Debug.Log($"Welding completed: {currentWeldTarget.name}");
                
                // Сброс прогресса
                weldProgress = 0f;
                isWelding = false;
                currentWeldTarget = null;
            }
        }

        /// <summary>
        /// Обновление визуальных эффектов
        /// </summary>
        private void UpdateVisualEffects()
        {
            // Обновление света
            if (weldingLight != null)
            {
                if (currentPower > 0f)
                {
                    weldingLight.enabled = true;
                    weldingLight.intensity = (currentPower / weldingPower) * 2f;
                }
                else
                {
                    weldingLight.enabled = false;
                }
            }

            // Обновление системы частиц
            if (weldingParticles != null)
            {
                var emission = weldingParticles.emission;
                if (isWelding && currentPower > 0f)
                {
                    emission.rateOverTime = (currentPower / weldingPower) * 100f;
                }
                else
                {
                    emission.rateOverTime = 0f;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!weldingActive) return;

            // Отображение сварочной дуги в редакторе
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, transform.forward * weldingRange);
            
            // Отображение зоны действия
            Gizmos.color = Color.Lerp(Color.blue, Color.white, currentPower / weldingPower);
            Gizmos.DrawWireSphere(transform.position, 0.1f);
            
            // Отображение прогресса сварки
            if (isWelding)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(weldingHit.point, 0.05f);
            }
        }
    }
}
