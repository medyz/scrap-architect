using UnityEngine;

namespace ScrapArchitect.Parts
{
    /// <summary>
    /// Гидравлический цилиндр - устройство для создания мощного линейного движения
    /// </summary>
    public class HydraulicCylinder : PartBase
    {
        [Header("Hydraulic Settings")]
        public HydraulicCylinderType cylinderType = HydraulicCylinderType.Standard;
        public float cylinderForce = 5000f;
        public float cylinderSpeed = 1f;
        public float cylinderStroke = 2f;
        public float cylinderPressure = 100f;

        [Header("Hydraulic Physics")]
        public float cylinderDiameter = 0.2f;
        public float cylinderLength = 1f;
        public float cylinderExtension = 0f;
        public float maxExtension = 2f;

        [Header("Hydraulic Visual")]
        public Transform cylinderBody;
        public Transform cylinderRod;
        public Material cylinderMaterial;
        public bool enableHydraulicEffect = true;
        public ParticleSystem hydraulicParticles;

        // Компоненты цилиндра
        private bool isCylinderActive = false;
        private bool isExtending = false;
        private bool isRetracting = false;
        private float targetExtension = 0f;
        private Vector3 startPosition;
        private Vector3 endPosition;

        public enum HydraulicCylinderType
        {
            Standard,    // Стандартный гидравлический цилиндр
            Heavy,       // Тяжелый гидравлический цилиндр
            Fast,        // Быстрый гидравлический цилиндр
            Precise      // Точный гидравлический цилиндр
        }

        protected override void InitializePart()
        {
            base.InitializePart();
            
            // Настройка типа цилиндра
            ConfigureCylinderType();
            
            // Создание компонентов цилиндра
            CreateCylinderComponents();
            
            Debug.Log($"Hydraulic cylinder initialized: {cylinderType} type");
        }

        /// <summary>
        /// Специфичное действие для гидравлического цилиндра
        /// </summary>
        protected override void OnPartSpecificAction()
        {
            // Логика специфичная для гидравлического цилиндра
            if (isCylinderActive)
            {
                ProcessCylinderLogic();
                UpdateVisualEffects();
            }
        }

        /// <summary>
        /// Настройка типа цилиндра
        /// </summary>
        private void ConfigureCylinderType()
        {
            switch (cylinderType)
            {
                case HydraulicCylinderType.Standard:
                    cylinderForce = 5000f;
                    cylinderSpeed = 1f;
                    cylinderStroke = 2f;
                    cylinderPressure = 100f;
                    cylinderDiameter = 0.2f;
                    cylinderLength = 1f;
                    maxExtension = 2f;
                    break;
                    
                case HydraulicCylinderType.Heavy:
                    cylinderForce = 10000f;
                    cylinderSpeed = 0.5f;
                    cylinderStroke = 3f;
                    cylinderPressure = 150f;
                    cylinderDiameter = 0.3f;
                    cylinderLength = 1.5f;
                    maxExtension = 3f;
                    break;
                    
                case HydraulicCylinderType.Fast:
                    cylinderForce = 3000f;
                    cylinderSpeed = 2f;
                    cylinderStroke = 1.5f;
                    cylinderPressure = 80f;
                    cylinderDiameter = 0.15f;
                    cylinderLength = 0.8f;
                    maxExtension = 1.5f;
                    break;
                    
                case HydraulicCylinderType.Precise:
                    cylinderForce = 2000f;
                    cylinderSpeed = 0.8f;
                    cylinderStroke = 1f;
                    cylinderPressure = 60f;
                    cylinderDiameter = 0.12f;
                    cylinderLength = 0.6f;
                    maxExtension = 1f;
                    break;
            }
        }

        /// <summary>
        /// Создание компонентов цилиндра
        /// </summary>
        private void CreateCylinderComponents()
        {
            // Создание корпуса цилиндра
            if (cylinderBody == null)
            {
                CreateCylinderBody();
            }

            // Создание штока цилиндра
            if (cylinderRod == null)
            {
                CreateCylinderRod();
            }

            // Настройка позиций
            startPosition = transform.position;
            endPosition = transform.position + transform.forward * maxExtension;

            // Применение материала
            if (cylinderMaterial != null)
            {
                if (cylinderBody != null)
                {
                    cylinderBody.GetComponent<Renderer>().material = cylinderMaterial;
                }
                if (cylinderRod != null)
                {
                    cylinderRod.GetComponent<Renderer>().material = cylinderMaterial;
                }
            }

            // Создание системы частиц для гидравлики
            if (enableHydraulicEffect && hydraulicParticles == null)
            {
                CreateHydraulicParticles();
            }
        }

        /// <summary>
        /// Создание корпуса цилиндра
        /// </summary>
        private void CreateCylinderBody()
        {
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            body.name = "HydraulicCylinderBody";
            body.transform.SetParent(transform);
            body.transform.localPosition = Vector3.zero;
            body.transform.localRotation = Quaternion.identity;
            body.transform.localScale = new Vector3(cylinderDiameter, cylinderLength, cylinderDiameter);

            // Удаление коллайдера у корпуса
            DestroyImmediate(body.GetComponent<Collider>());

            cylinderBody = body.transform;
        }

        /// <summary>
        /// Создание штока цилиндра
        /// </summary>
        private void CreateCylinderRod()
        {
            GameObject rod = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            rod.name = "HydraulicCylinderRod";
            rod.transform.SetParent(transform);
            rod.transform.localPosition = new Vector3(0, 0, cylinderLength * 0.5f);
            rod.transform.localRotation = Quaternion.identity;
            rod.transform.localScale = new Vector3(cylinderDiameter * 0.7f, cylinderLength * 0.4f, cylinderDiameter * 0.7f);

            // Удаление коллайдера у штока
            DestroyImmediate(rod.GetComponent<Collider>());

            cylinderRod = rod.transform;
        }

        /// <summary>
        /// Создание системы частиц для гидравлики
        /// </summary>
        private void CreateHydraulicParticles()
        {
            GameObject particleObj = new GameObject("HydraulicParticles");
            particleObj.transform.SetParent(transform);
            particleObj.transform.localPosition = new Vector3(0, 0, cylinderLength * 0.8f);

            hydraulicParticles = particleObj.AddComponent<ParticleSystem>();
            
            // Настройка системы частиц
            var main = hydraulicParticles.main;
            main.startLifetime = 0.4f;
            main.startSpeed = 3f;
            main.startSize = 0.03f;
            main.maxParticles = 80;
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = hydraulicParticles.emission;
            emission.rateOverTime = 0f; // Начинаем с 0

            var shape = hydraulicParticles.shape;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = cylinderDiameter * 0.4f;

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
        /// Активировать цилиндр
        /// </summary>
        public void ActivateCylinder()
        {
            isCylinderActive = true;
            Debug.Log($"Hydraulic cylinder activated: {cylinderType}");
        }

        /// <summary>
        /// Деактивировать цилиндр
        /// </summary>
        public void DeactivateCylinder()
        {
            isCylinderActive = false;
            isExtending = false;
            isRetracting = false;
            Debug.Log($"Hydraulic cylinder deactivated: {cylinderType}");
        }

        /// <summary>
        /// Выдвинуть шток
        /// </summary>
        public void ExtendCylinder()
        {
            if (!isCylinderActive) return;

            isExtending = true;
            isRetracting = false;
            targetExtension = maxExtension;
            Debug.Log($"Hydraulic cylinder extending to {maxExtension}");
        }

        /// <summary>
        /// Втянуть шток
        /// </summary>
        public void RetractCylinder()
        {
            if (!isCylinderActive) return;

            isRetracting = true;
            isExtending = false;
            targetExtension = 0f;
            Debug.Log("Hydraulic cylinder retracting to 0");
        }

        /// <summary>
        /// Установить позицию штока
        /// </summary>
        public void SetCylinderPosition(float position)
        {
            if (!isCylinderActive) return;

            targetExtension = Mathf.Clamp(position, 0f, maxExtension);
            isExtending = targetExtension > cylinderExtension;
            isRetracting = targetExtension < cylinderExtension;
        }

        /// <summary>
        /// Получить текущую позицию штока
        /// </summary>
        public float GetCylinderPosition()
        {
            return cylinderExtension;
        }

        /// <summary>
        /// Получить максимальную позицию штока
        /// </summary>
        public float GetMaxCylinderPosition()
        {
            return maxExtension;
        }

        /// <summary>
        /// Получить силу цилиндра
        /// </summary>
        public float GetCylinderForce()
        {
            return cylinderForce * (cylinderPressure / 100f);
        }

        /// <summary>
        /// Получить тип цилиндра
        /// </summary>
        public HydraulicCylinderType GetCylinderType()
        {
            return cylinderType;
        }

        /// <summary>
        /// Получить информацию о цилиндре
        /// </summary>
        public override string GetPartInfo()
        {
            string info = base.GetPartInfo();
            info += $"\nТип: {GetLocalizedCylinderType()}";
            info += $"\nСила: {cylinderForce}N";
            info += $"\nСкорость: {cylinderSpeed} м/с";
            info += $"\nХод: {cylinderStroke}м";
            info += $"\nДавление: {cylinderPressure} бар";
            info += $"\nПозиция: {cylinderExtension:F2}/{maxExtension}";
            return info;
        }

        /// <summary>
        /// Получить локализованное название типа цилиндра
        /// </summary>
        private string GetLocalizedCylinderType()
        {
            return cylinderType switch
            {
                HydraulicCylinderType.Standard => "Стандартный",
                HydraulicCylinderType.Heavy => "Тяжелый",
                HydraulicCylinderType.Fast => "Быстрый",
                HydraulicCylinderType.Precise => "Точный",
                _ => cylinderType.ToString()
            };
        }

        /// <summary>
        /// Получить стоимость цилиндра
        /// </summary>
        public int GetPartCost()
        {
            return cylinderType switch
            {
                HydraulicCylinderType.Standard => 700,
                HydraulicCylinderType.Heavy => 1200,
                HydraulicCylinderType.Fast => 900,
                HydraulicCylinderType.Precise => 800,
                _ => 700
            };
        }

        /// <summary>
        /// Получить вес цилиндра
        /// </summary>
        public float GetPartWeight()
        {
            return cylinderType switch
            {
                HydraulicCylinderType.Standard => 25f,
                HydraulicCylinderType.Heavy => 45f,
                HydraulicCylinderType.Fast => 18f,
                HydraulicCylinderType.Precise => 15f,
                _ => 25f
            };
        }

        private void Update()
        {
            if (isCylinderActive)
            {
                // Обновление позиции штока
                if (isExtending || isRetracting)
                {
                    float direction = isExtending ? 1f : -1f;
                    cylinderExtension += direction * cylinderSpeed * Time.deltaTime;
                    cylinderExtension = Mathf.Clamp(cylinderExtension, 0f, maxExtension);

                    // Проверка достижения цели
                    if ((isExtending && cylinderExtension >= targetExtension) ||
                        (isRetracting && cylinderExtension <= targetExtension))
                    {
                        isExtending = false;
                        isRetracting = false;
                        cylinderExtension = targetExtension;
                    }

                    // Обновление позиции штока
                    if (cylinderRod != null)
                    {
                        Vector3 newPosition = new Vector3(0, 0, cylinderLength * 0.5f + cylinderExtension);
                        cylinderRod.localPosition = newPosition;
                    }
                }

                // Обновление системы частиц
                if (hydraulicParticles != null)
                {
                    var emission = hydraulicParticles.emission;
                    if (isExtending || isRetracting)
                    {
                        emission.rateOverTime = 25f;
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
            // Отображение направления движения в редакторе
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * maxExtension);
            
            // Отображение текущей позиции
            Gizmos.color = Color.green;
            Vector3 currentPos = transform.position + transform.forward * cylinderExtension;
            Gizmos.DrawWireSphere(currentPos, cylinderDiameter * 0.5f);
        }

        /// <summary>
        /// Обработка логики цилиндра
        /// </summary>
        private void ProcessCylinderLogic()
        {
            // Обработка движения цилиндра
            if (isExtending || isRetracting)
            {
                float direction = isExtending ? 1f : -1f;
                cylinderExtension += direction * cylinderSpeed * Time.deltaTime;
                cylinderExtension = Mathf.Clamp(cylinderExtension, 0f, maxExtension);

                // Проверка достижения цели
                if ((isExtending && cylinderExtension >= targetExtension) ||
                    (isRetracting && cylinderExtension <= targetExtension))
                {
                    isExtending = false;
                    isRetracting = false;
                    cylinderExtension = targetExtension;
                }

                // Обновление позиции штока
                if (cylinderRod != null)
                {
                    Vector3 newPosition = new Vector3(0, 0, cylinderLength * 0.5f + cylinderExtension);
                    cylinderRod.localPosition = newPosition;
                }
            }
        }

        /// <summary>
        /// Обновление визуальных эффектов
        /// </summary>
        private void UpdateVisualEffects()
        {
            // Обновление системы частиц
            if (hydraulicParticles != null)
            {
                var emission = hydraulicParticles.emission;
                if (isExtending || isRetracting)
                {
                    emission.rateOverTime = 25f;
                }
                else
                {
                    emission.rateOverTime = 0f;
                }
            }

            // Обновление цвета в зависимости от состояния
            if (cylinderBody != null && cylinderRod != null)
            {
                Color activeColor = isCylinderActive ? Color.green : Color.gray;
                cylinderBody.GetComponent<Renderer>().material.color = activeColor;
                cylinderRod.GetComponent<Renderer>().material.color = activeColor;
            }
        }
    }
}
