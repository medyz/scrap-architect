using UnityEngine;

namespace ScrapArchitect.Parts
{
    /// <summary>
    /// Гидравлический клапан - устройство для управления потоком жидкости
    /// </summary>
    public class HydraulicValve : PartBase
    {
        [Header("Valve Settings")]
        public HydraulicValveType valveType = HydraulicValveType.Standard;
        public float valveFlow = 20f;
        public float valvePressure = 100f;
        public float valveEfficiency = 0.9f;
        public float valveResponse = 0.5f;

        [Header("Valve Physics")]
        public float valveOpening = 0f;
        public float maxOpening = 1f;
        public float valveTemperature = 20f;
        public float maxTemperature = 80f;

        [Header("Valve Visual")]
        public Transform valveBody;
        public Transform valveHandle;
        public Material valveMaterial;
        public bool enableValveEffect = true;
        public ParticleSystem flowParticles;

        // Компоненты клапана
        private bool isValveActive = false;
        private bool isOpening = false;
        private bool isClosing = false;
        private float targetOpening = 0f;
        private float currentFlow = 0f;

        public enum HydraulicValveType
        {
            Standard,    // Стандартный гидравлический клапан
            HighFlow,    // Высокопроизводительный клапан
            Precise,     // Точный клапан
            Pressure     // Напорный клапан
        }

        protected override void InitializePart()
        {
            base.InitializePart();
            
            // Настройка типа клапана
            ConfigureValveType();
            
            // Создание компонентов клапана
            CreateValveComponents();
            
            Debug.Log($"Hydraulic valve initialized: {valveType} type");
        }

        /// <summary>
        /// Настройка типа клапана
        /// </summary>
        private void ConfigureValveType()
        {
            switch (valveType)
            {
                case HydraulicValveType.Standard:
                    valveFlow = 20f;
                    valvePressure = 100f;
                    valveEfficiency = 0.9f;
                    valveResponse = 0.5f;
                    maxTemperature = 80f;
                    break;
                    
                case HydraulicValveType.HighFlow:
                    valveFlow = 40f;
                    valvePressure = 80f;
                    valveEfficiency = 0.85f;
                    valveResponse = 0.3f;
                    maxTemperature = 90f;
                    break;
                    
                case HydraulicValveType.Precise:
                    valveFlow = 10f;
                    valvePressure = 120f;
                    valveEfficiency = 0.95f;
                    valveResponse = 0.8f;
                    maxTemperature = 70f;
                    break;
                    
                case HydraulicValveType.Pressure:
                    valveFlow = 15f;
                    valvePressure = 200f;
                    valveEfficiency = 0.8f;
                    valveResponse = 0.4f;
                    maxTemperature = 100f;
                    break;
            }
        }

        /// <summary>
        /// Создание компонентов клапана
        /// </summary>
        private void CreateValveComponents()
        {
            // Создание корпуса клапана
            if (valveBody == null)
            {
                CreateValveBody();
            }

            // Создание ручки клапана
            if (valveHandle == null)
            {
                CreateValveHandle();
            }

            // Применение материала
            if (valveMaterial != null)
            {
                if (valveBody != null)
                {
                    valveBody.GetComponent<Renderer>().material = valveMaterial;
                }
                if (valveHandle != null)
                {
                    valveHandle.GetComponent<Renderer>().material = valveMaterial;
                }
            }

            // Создание системы частиц для потока
            if (enableValveEffect && flowParticles == null)
            {
                CreateFlowParticles();
            }
        }

        /// <summary>
        /// Создание корпуса клапана
        /// </summary>
        private void CreateValveBody()
        {
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            body.name = "ValveBody";
            body.transform.SetParent(transform);
            body.transform.localPosition = Vector3.zero;
            body.transform.localRotation = Quaternion.identity;
            body.transform.localScale = new Vector3(0.15f, 0.2f, 0.15f);

            // Удаление коллайдера у корпуса
            DestroyImmediate(body.GetComponent<Collider>());

            valveBody = body.transform;
        }

        /// <summary>
        /// Создание ручки клапана
        /// </summary>
        private void CreateValveHandle()
        {
            GameObject handle = GameObject.CreatePrimitive(PrimitiveType.Cube);
            handle.name = "ValveHandle";
            handle.transform.SetParent(transform);
            handle.transform.localPosition = new Vector3(0, 0.15f, 0);
            handle.transform.localRotation = Quaternion.identity;
            handle.transform.localScale = new Vector3(0.1f, 0.05f, 0.1f);

            // Удаление коллайдера у ручки
            DestroyImmediate(handle.GetComponent<Collider>());

            valveHandle = handle.transform;
        }

        /// <summary>
        /// Создание системы частиц для потока
        /// </summary>
        private void CreateFlowParticles()
        {
            GameObject particleObj = new GameObject("FlowParticles");
            particleObj.transform.SetParent(transform);
            particleObj.transform.localPosition = new Vector3(0, 0, 0.1f);

            flowParticles = particleObj.AddComponent<ParticleSystem>();
            
            // Настройка системы частиц
            var main = flowParticles.main;
            main.startLifetime = 0.6f;
            main.startSpeed = 4f;
            main.startSize = 0.02f;
            main.maxParticles = 60;
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = flowParticles.emission;
            emission.rateOverTime = 0f; // Начинаем с 0

            var shape = flowParticles.shape;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = 0.05f;

            var colorOverLifetime = flowParticles.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.orange, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
            colorOverLifetime.color = gradient;
        }

        /// <summary>
        /// Активировать клапан
        /// </summary>
        public void ActivateValve()
        {
            isValveActive = true;
            Debug.Log($"Hydraulic valve activated: {valveType}");
        }

        /// <summary>
        /// Деактивировать клапан
        /// </summary>
        public void DeactivateValve()
        {
            isValveActive = false;
            isOpening = false;
            isClosing = false;
            targetOpening = 0f;
            valveOpening = 0f;
            currentFlow = 0f;
            Debug.Log($"Hydraulic valve deactivated: {valveType}");
        }

        /// <summary>
        /// Открыть клапан
        /// </summary>
        public void OpenValve()
        {
            if (!isValveActive) return;

            isOpening = true;
            isClosing = false;
            targetOpening = maxOpening;
            Debug.Log($"Valve opening to {maxOpening}");
        }

        /// <summary>
        /// Закрыть клапан
        /// </summary>
        public void CloseValve()
        {
            if (!isValveActive) return;

            isClosing = true;
            isOpening = false;
            targetOpening = 0f;
            Debug.Log("Valve closing to 0");
        }

        /// <summary>
        /// Установить открытие клапана
        /// </summary>
        public void SetValveOpening(float opening)
        {
            if (!isValveActive) return;

            targetOpening = Mathf.Clamp(opening, 0f, maxOpening);
            isOpening = targetOpening > valveOpening;
            isClosing = targetOpening < valveOpening;
        }

        /// <summary>
        /// Получить текущее открытие клапана
        /// </summary>
        public float GetValveOpening()
        {
            return valveOpening;
        }

        /// <summary>
        /// Получить максимальное открытие клапана
        /// </summary>
        public float GetMaxValveOpening()
        {
            return maxOpening;
        }

        /// <summary>
        /// Получить текущий поток через клапан
        /// </summary>
        public float GetCurrentFlow()
        {
            return currentFlow;
        }

        /// <summary>
        /// Получить эффективность клапана
        /// </summary>
        public float GetValveEfficiency()
        {
            // Учет температуры на эффективность
            float tempFactor = 1f - (valveTemperature / maxTemperature) * 0.2f;
            return valveEfficiency * tempFactor;
        }

        /// <summary>
        /// Получить тип клапана
        /// </summary>
        public HydraulicValveType GetValveType()
        {
            return valveType;
        }

        /// <summary>
        /// Получить информацию о клапане
        /// </summary>
        public override string GetPartInfo()
        {
            string info = base.GetPartInfo();
            info += $"\nТип: {GetLocalizedValveType()}";
            info += $"\nПоток: {valveFlow} л/мин";
            info += $"\nДавление: {valvePressure} бар";
            info += $"\nЭффективность: {valveEfficiency:P0}";
            info += $"\nОткрытие: {valveOpening:F2}/{maxOpening}";
            info += $"\nТемпература: {valveTemperature:F1}°C";
            return info;
        }

        /// <summary>
        /// Получить локализованное название типа клапана
        /// </summary>
        private string GetLocalizedValveType()
        {
            return valveType switch
            {
                HydraulicValveType.Standard => "Стандартный",
                HydraulicValveType.HighFlow => "Высокопроизводительный",
                HydraulicValveType.Precise => "Точный",
                HydraulicValveType.Pressure => "Напорный",
                _ => valveType.ToString()
            };
        }

        /// <summary>
        /// Получить стоимость клапана
        /// </summary>
        public override int GetPartCost()
        {
            return valveType switch
            {
                HydraulicValveType.Standard => 200,
                HydraulicValveType.HighFlow => 400,
                HydraulicValveType.Precise => 350,
                HydraulicValveType.Pressure => 500,
                _ => 200
            };
        }

        /// <summary>
        /// Получить вес клапана
        /// </summary>
        public override float GetPartWeight()
        {
            return valveType switch
            {
                HydraulicValveType.Standard => 5f,
                HydraulicValveType.HighFlow => 8f,
                HydraulicValveType.Precise => 6f,
                HydraulicValveType.Pressure => 10f,
                _ => 5f
            };
        }

        private void Update()
        {
            if (isValveActive)
            {
                // Обновление открытия клапана
                if (isOpening || isClosing)
                {
                    float direction = isOpening ? 1f : -1f;
                    valveOpening += direction * valveResponse * Time.deltaTime;
                    valveOpening = Mathf.Clamp(valveOpening, 0f, maxOpening);

                    // Проверка достижения цели
                    if ((isOpening && valveOpening >= targetOpening) ||
                        (isClosing && valveOpening <= targetOpening))
                    {
                        isOpening = false;
                        isClosing = false;
                        valveOpening = targetOpening;
                    }

                    // Обновление позиции ручки
                    if (valveHandle != null)
                    {
                        float handleAngle = valveOpening * 90f; // Поворот на 90 градусов
                        valveHandle.localRotation = Quaternion.Euler(0, 0, handleAngle);
                    }
                }

                // Расчет текущего потока
                currentFlow = valveOpening * valveFlow * GetValveEfficiency();

                // Нагрев клапана при работе
                if (currentFlow > 0f)
                {
                    valveTemperature += Time.deltaTime * (currentFlow / valveFlow) * 2f;
                    valveTemperature = Mathf.Min(valveTemperature, maxTemperature);
                }
                else
                {
                    // Охлаждение при выключенном клапане
                    valveTemperature -= Time.deltaTime * 1f;
                    valveTemperature = Mathf.Max(valveTemperature, 20f);
                }

                // Обновление системы частиц
                if (flowParticles != null)
                {
                    var emission = flowParticles.emission;
                    if (currentFlow > 0f)
                    {
                        emission.rateOverTime = (currentFlow / valveFlow) * 30f;
                    }
                    else
                    {
                        emission.rateOverTime = 0f;
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Отображение направления потока в редакторе
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * 0.3f);
            
            // Отображение открытия клапана
            Gizmos.color = Color.Lerp(Color.red, Color.green, valveOpening / maxOpening);
            Gizmos.DrawWireSphere(transform.position, 0.1f);
        }
    }
}
