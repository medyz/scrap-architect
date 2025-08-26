using UnityEngine;

namespace ScrapArchitect.Parts
{
    /// <summary>
    /// Пневмоцилиндр - устройство для создания линейного движения
    /// </summary>
    public class PneumaticCylinder : PartBase
    {
        [Header("Pneumatic Settings")]
        public PneumaticType cylinderType = PneumaticType.Standard;
        public float cylinderForce = 1000f;
        public float cylinderSpeed = 2f;
        public float cylinderStroke = 1f;
        public float cylinderPressure = 1f;

        [Header("Pneumatic Physics")]
        public float cylinderDiameter = 0.1f;
        public float cylinderLength = 0.5f;
        public float cylinderExtension = 0f;
        public float maxExtension = 1f;

        [Header("Pneumatic Visual")]
        public Transform cylinderBody;
        public Transform cylinderRod;
        public Material cylinderMaterial;
        public bool enablePneumaticEffect = true;
        public ParticleSystem pressureParticles;

        // Компоненты цилиндра
        private bool isCylinderActive = false;
        private bool isExtending = false;
        private bool isRetracting = false;
        private float targetExtension = 0f;
        private Vector3 startPosition;
        private Vector3 endPosition;

        public enum PneumaticType
        {
            Standard,    // Стандартный пневмоцилиндр
            Heavy,       // Тяжелый пневмоцилиндр
            Fast,        // Быстрый пневмоцилиндр
            Precise      // Точный пневмоцилиндр
        }

        protected override void InitializePart()
        {
            base.InitializePart();
            
            // Настройка типа цилиндра
            ConfigureCylinderType();
            
            // Создание компонентов цилиндра
            CreateCylinderComponents();
            
            Debug.Log($"Pneumatic cylinder initialized: {cylinderType} type");
        }

        /// <summary>
        /// Настройка типа цилиндра
        /// </summary>
        private void ConfigureCylinderType()
        {
            switch (cylinderType)
            {
                case PneumaticType.Standard:
                    cylinderForce = 1000f;
                    cylinderSpeed = 2f;
                    cylinderStroke = 1f;
                    cylinderPressure = 1f;
                    cylinderDiameter = 0.1f;
                    cylinderLength = 0.5f;
                    maxExtension = 1f;
                    break;
                    
                case PneumaticType.Heavy:
                    cylinderForce = 2000f;
                    cylinderSpeed = 1f;
                    cylinderStroke = 1.5f;
                    cylinderPressure = 1.5f;
                    cylinderDiameter = 0.15f;
                    cylinderLength = 0.7f;
                    maxExtension = 1.5f;
                    break;
                    
                case PneumaticType.Fast:
                    cylinderForce = 800f;
                    cylinderSpeed = 4f;
                    cylinderStroke = 0.8f;
                    cylinderPressure = 0.8f;
                    cylinderDiameter = 0.08f;
                    cylinderLength = 0.4f;
                    maxExtension = 0.8f;
                    break;
                    
                case PneumaticType.Precise:
                    cylinderForce = 600f;
                    cylinderSpeed = 1.5f;
                    cylinderStroke = 0.5f;
                    cylinderPressure = 0.6f;
                    cylinderDiameter = 0.06f;
                    cylinderLength = 0.3f;
                    maxExtension = 0.5f;
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

            // Создание системы частиц для давления
            if (enablePneumaticEffect && pressureParticles == null)
            {
                CreatePressureParticles();
            }
        }

        /// <summary>
        /// Создание корпуса цилиндра
        /// </summary>
        private void CreateCylinderBody()
        {
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            body.name = "CylinderBody";
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
            rod.name = "CylinderRod";
            rod.transform.SetParent(transform);
            rod.transform.localPosition = new Vector3(0, 0, cylinderLength * 0.5f);
            rod.transform.localRotation = Quaternion.identity;
            rod.transform.localScale = new Vector3(cylinderDiameter * 0.6f, cylinderLength * 0.3f, cylinderDiameter * 0.6f);

            // Удаление коллайдера у штока
            DestroyImmediate(rod.GetComponent<Collider>());

            cylinderRod = rod.transform;
        }

        /// <summary>
        /// Создание системы частиц для давления
        /// </summary>
        private void CreatePressureParticles()
        {
            GameObject particleObj = new GameObject("PressureParticles");
            particleObj.transform.SetParent(transform);
            particleObj.transform.localPosition = new Vector3(0, 0, cylinderLength * 0.8f);

            pressureParticles = particleObj.AddComponent<ParticleSystem>();
            
            // Настройка системы частиц
            var main = pressureParticles.main;
            main.startLifetime = 0.3f;
            main.startSpeed = 2f;
            main.startSize = 0.02f;
            main.maxParticles = 50;
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = pressureParticles.emission;
            emission.rateOverTime = 0f; // Начинаем с 0

            var shape = pressureParticles.shape;
            shape.shapeType = ParticleSystemShapeType.Circle;
            shape.radius = cylinderDiameter * 0.3f;

            var colorOverLifetime = pressureParticles.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.blue, 0.0f), new GradientColorKey(Color.cyan, 1.0f) },
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
            Debug.Log($"Pneumatic cylinder activated: {cylinderType}");
        }

        /// <summary>
        /// Деактивировать цилиндр
        /// </summary>
        public void DeactivateCylinder()
        {
            isCylinderActive = false;
            isExtending = false;
            isRetracting = false;
            Debug.Log($"Pneumatic cylinder deactivated: {cylinderType}");
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
            Debug.Log($"Cylinder extending to {maxExtension}");
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
            Debug.Log("Cylinder retracting to 0");
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
            return cylinderForce * cylinderPressure;
        }

        /// <summary>
        /// Получить тип цилиндра
        /// </summary>
        public PneumaticType GetCylinderType()
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
            info += $"\nДавление: {cylinderPressure}";
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
                PneumaticType.Standard => "Стандартный",
                PneumaticType.Heavy => "Тяжелый",
                PneumaticType.Fast => "Быстрый",
                PneumaticType.Precise => "Точный",
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
                PneumaticType.Standard => 400,
                PneumaticType.Heavy => 800,
                PneumaticType.Fast => 600,
                PneumaticType.Precise => 500,
                _ => 400
            };
        }

        /// <summary>
        /// Получить вес цилиндра
        /// </summary>
        public float GetPartWeight()
        {
            return cylinderType switch
            {
                PneumaticType.Standard => 8f,
                PneumaticType.Heavy => 15f,
                PneumaticType.Fast => 6f,
                PneumaticType.Precise => 5f,
                _ => 8f
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
                if (pressureParticles != null)
                {
                    var emission = pressureParticles.emission;
                    if (isExtending || isRetracting)
                    {
                        emission.rateOverTime = 20f;
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
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, transform.forward * maxExtension);
            
            // Отображение текущей позиции
            Gizmos.color = Color.green;
            Vector3 currentPos = transform.position + transform.forward * cylinderExtension;
            Gizmos.DrawWireSphere(currentPos, cylinderDiameter * 0.5f);
        }
    }
}
