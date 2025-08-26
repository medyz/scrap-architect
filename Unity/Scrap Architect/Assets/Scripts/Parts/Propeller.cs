using UnityEngine;

namespace ScrapArchitect.Parts
{
    /// <summary>
    /// Пропеллер - воздушный движитель для летательных аппаратов
    /// </summary>
    public class Propeller : PartBase
    {
        [Header("Propeller Settings")]
        public PropellerType propellerType = PropellerType.Standard;
        public float propellerSpeed = 1000f;
        public float propellerThrust = 500f;
        public float propellerEfficiency = 0.8f;
        public float propellerDiameter = 0.5f;

        [Header("Propeller Physics")]
        public float propellerBlades = 3;
        public float propellerPitch = 15f;
        public float propellerRPM = 0f;
        public float maxRPM = 3000f;

        [Header("Propeller Visual")]
        public Transform propellerBlade;
        public Material propellerMaterial;
        public bool enablePropellerEffect = true;

        // Компоненты пропеллера
        private Rigidbody propellerRigidbody;
        private bool isPropellerActive = false;
        private float currentRPM = 0f;
        private Vector3 thrustDirection;

        public enum PropellerType
        {
            Standard,    // Стандартный пропеллер
            HighSpeed,   // Высокоскоростной пропеллер
            HeavyLift,   // Тяжелый подъемный пропеллер
            Ducted       // Кольцевой пропеллер
        }

        protected override void InitializePart()
        {
            base.InitializePart();
            
            // Настройка типа пропеллера
            ConfigurePropellerType();
            
            // Создание компонентов пропеллера
            CreatePropellerComponents();
            
            Debug.Log($"Propeller initialized: {propellerType} type");
        }

        /// <summary>
        /// Настройка типа пропеллера
        /// </summary>
        private void ConfigurePropellerType()
        {
            switch (propellerType)
            {
                case PropellerType.Standard:
                    propellerSpeed = 1000f;
                    propellerThrust = 500f;
                    propellerEfficiency = 0.8f;
                    propellerDiameter = 0.5f;
                    propellerBlades = 3;
                    propellerPitch = 15f;
                    maxRPM = 3000f;
                    break;
                    
                case PropellerType.HighSpeed:
                    propellerSpeed = 1500f;
                    propellerThrust = 400f;
                    propellerEfficiency = 0.9f;
                    propellerDiameter = 0.4f;
                    propellerBlades = 4;
                    propellerPitch = 20f;
                    maxRPM = 4000f;
                    break;
                    
                case PropellerType.HeavyLift:
                    propellerSpeed = 800f;
                    propellerThrust = 800f;
                    propellerEfficiency = 0.7f;
                    propellerDiameter = 0.8f;
                    propellerBlades = 2;
                    propellerPitch = 12f;
                    maxRPM = 2000f;
                    break;
                    
                case PropellerType.Ducted:
                    propellerSpeed = 1200f;
                    propellerThrust = 600f;
                    propellerEfficiency = 0.85f;
                    propellerDiameter = 0.6f;
                    propellerBlades = 5;
                    propellerPitch = 18f;
                    maxRPM = 3500f;
                    break;
            }
        }

        /// <summary>
        /// Создание компонентов пропеллера
        /// </summary>
        private void CreatePropellerComponents()
        {
            // Создание лопастей пропеллера
            if (propellerBlade == null)
            {
                CreatePropellerBlades();
            }

            // Настройка направления тяги
            thrustDirection = transform.forward;

            // Применение материала
            if (propellerMaterial != null && propellerBlade != null)
            {
                Renderer[] renderers = propellerBlade.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    renderer.material = propellerMaterial;
                }
            }
        }

        /// <summary>
        /// Создание лопастей пропеллера
        /// </summary>
        private void CreatePropellerBlades()
        {
            GameObject bladeParent = new GameObject("PropellerBlades");
            bladeParent.transform.SetParent(transform);
            bladeParent.transform.localPosition = Vector3.zero;

            for (int i = 0; i < propellerBlades; i++)
            {
                GameObject blade = GameObject.CreatePrimitive(PrimitiveType.Cube);
                blade.name = $"Blade_{i}";
                blade.transform.SetParent(bladeParent.transform);
                
                // Позиционирование лопасти
                float angle = (360f / propellerBlades) * i;
                blade.transform.localRotation = Quaternion.Euler(0, angle, propellerPitch);
                blade.transform.localPosition = Vector3.zero;
                
                // Размеры лопасти
                float bladeLength = propellerDiameter * 0.4f;
                float bladeWidth = propellerDiameter * 0.1f;
                float bladeThickness = propellerDiameter * 0.02f;
                blade.transform.localScale = new Vector3(bladeWidth, bladeThickness, bladeLength);
                
                // Смещение от центра
                blade.transform.localPosition = new Vector3(0, 0, bladeLength * 0.3f);
            }

            propellerBlade = bladeParent.transform;
        }

        /// <summary>
        /// Активировать пропеллер
        /// </summary>
        public void ActivatePropeller()
        {
            isPropellerActive = true;
            Debug.Log($"Propeller activated: {propellerType}");
        }

        /// <summary>
        /// Деактивировать пропеллер
        /// </summary>
        public void DeactivatePropeller()
        {
            isPropellerActive = false;
            currentRPM = 0f;
            Debug.Log($"Propeller deactivated: {propellerType}");
        }

        /// <summary>
        /// Установить скорость пропеллера
        /// </summary>
        public void SetPropellerSpeed(float speed)
        {
            if (!isPropellerActive) return;

            propellerRPM = Mathf.Clamp(speed, 0f, maxRPM);
        }

        /// <summary>
        /// Получить тягу пропеллера
        /// </summary>
        public Vector3 GetPropellerThrust()
        {
            if (!isPropellerActive || propellerRPM <= 0f) return Vector3.zero;

            // Расчет тяги на основе RPM и эффективности
            float thrustForce = (propellerRPM / maxRPM) * propellerThrust * propellerEfficiency;
            return thrustDirection * thrustForce;
        }

        /// <summary>
        /// Применить тягу к объекту
        /// </summary>
        public void ApplyThrust(Rigidbody targetRigidbody)
        {
            if (!isPropellerActive) return;

            Vector3 thrust = GetPropellerThrust();
            if (thrust.magnitude > 0f)
            {
                targetRigidbody.AddForce(thrust, ForceMode.Force);
            }
        }

        /// <summary>
        /// Получить текущий RPM пропеллера
        /// </summary>
        public float GetCurrentRPM()
        {
            return currentRPM;
        }

        /// <summary>
        /// Получить максимальный RPM пропеллера
        /// </summary>
        public float GetMaxRPM()
        {
            return maxRPM;
        }

        /// <summary>
        /// Получить тип пропеллера
        /// </summary>
        public PropellerType GetPropellerType()
        {
            return propellerType;
        }

        /// <summary>
        /// Получить информацию о пропеллере
        /// </summary>
        public override string GetPartInfo()
        {
            string info = base.GetPartInfo();
            info += $"\nТип: {GetLocalizedPropellerType()}";
            info += $"\nДиаметр: {propellerDiameter}m";
            info += $"\nЛопастей: {propellerBlades}";
            info += $"\nМакс. RPM: {maxRPM}";
            info += $"\nТяга: {propellerThrust}N";
            info += $"\nЭффективность: {propellerEfficiency:P0}";
            return info;
        }

        /// <summary>
        /// Получить локализованное название типа пропеллера
        /// </summary>
        private string GetLocalizedPropellerType()
        {
            return propellerType switch
            {
                PropellerType.Standard => "Стандартный",
                PropellerType.HighSpeed => "Высокоскоростной",
                PropellerType.HeavyLift => "Тяжелый подъемный",
                PropellerType.Ducted => "Кольцевой",
                _ => propellerType.ToString()
            };
        }

        /// <summary>
        /// Получить стоимость пропеллера
        /// </summary>
        public int GetPartCost()
        {
            return propellerType switch
            {
                PropellerType.Standard => 600,
                PropellerType.HighSpeed => 900,
                PropellerType.HeavyLift => 1200,
                PropellerType.Ducted => 1000,
                _ => 600
            };
        }

        /// <summary>
        /// Получить вес пропеллера
        /// </summary>
        public float GetPartWeight()
        {
            return propellerType switch
            {
                PropellerType.Standard => 15f,
                PropellerType.HighSpeed => 12f,
                PropellerType.HeavyLift => 25f,
                PropellerType.Ducted => 20f,
                _ => 15f
            };
        }

        private void Update()
        {
            if (isPropellerActive && propellerBlade != null)
            {
                // Плавное изменение RPM
                currentRPM = Mathf.Lerp(currentRPM, propellerRPM, Time.deltaTime * 2f);
                
                // Вращение лопастей
                float rotationSpeed = (currentRPM / 60f) * 360f; // RPM в градусы в секунду
                propellerBlade.Rotate(0, 0, rotationSpeed * Time.deltaTime);
                
                // Визуальные эффекты (опционально)
                if (enablePropellerEffect && currentRPM > 100f)
                {
                    // Здесь можно добавить эффекты размытия или частиц
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Отображение направления тяги в редакторе
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, thrustDirection * 2f);
            
            // Отображение диаметра пропеллера
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, propellerDiameter * 0.5f);
        }
    }
}
