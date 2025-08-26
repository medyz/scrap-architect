using UnityEngine;

namespace ScrapArchitect.Parts
{
    /// <summary>
    /// Лазерный резак - продвинутый инструмент для точной резки материалов
    /// </summary>
    public class LaserCutter : PartBase
    {
        [Header("Laser Settings")]
        public LaserCutterType cutterType = LaserCutterType.Standard;
        public float laserPower = 200f;
        public float laserRange = 5f;
        public float laserAccuracy = 0.01f;
        public float cuttingSpeed = 1f;
        public bool laserActive = false;

        [Header("Laser Physics")]
        public LayerMask cuttableLayers = -1;
        public Transform laserEmitter;
        public Transform laserBeam;
        public Material laserMaterial;
        public bool enableLaserEffect = true;
        public ParticleSystem laserParticles;
        public LineRenderer laserLine;

        [Header("Laser Output")]
        public float currentPower = 0f;
        public float temperature = 20f;
        public float maxTemperature = 200f;
        public bool isCutting = false;

        // Компоненты лазера
        private RaycastHit laserHit;
        private bool isInitialized = false;
        private float lastCutTime = 0f;

        public enum LaserCutterType
        {
            Standard,    // Стандартный лазерный резак
            Precision,   // Точный лазерный резак
            Industrial,  // Промышленный лазерный резак
            Experimental // Экспериментальный лазерный резак
        }

        protected override void InitializePart()
        {
            base.InitializePart();
            
            // Настройка типа лазера
            ConfigureLaserType();
            
            // Создание компонентов лазера
            CreateLaserComponents();
            
            Debug.Log($"Laser cutter initialized: {cutterType} type");
        }

        /// <summary>
        /// Настройка типа лазера
        /// </summary>
        private void ConfigureLaserType()
        {
            switch (cutterType)
            {
                case LaserCutterType.Standard:
                    laserPower = 200f;
                    laserRange = 5f;
                    laserAccuracy = 0.01f;
                    cuttingSpeed = 1f;
                    maxTemperature = 200f;
                    break;
                    
                case LaserCutterType.Precision:
                    laserPower = 150f;
                    laserRange = 8f;
                    laserAccuracy = 0.005f;
                    cuttingSpeed = 0.5f;
                    maxTemperature = 150f;
                    break;
                    
                case LaserCutterType.Industrial:
                    laserPower = 400f;
                    laserRange = 3f;
                    laserAccuracy = 0.02f;
                    cuttingSpeed = 2f;
                    maxTemperature = 300f;
                    break;
                    
                case LaserCutterType.Experimental:
                    laserPower = 600f;
                    laserRange = 10f;
                    laserAccuracy = 0.001f;
                    cuttingSpeed = 1.5f;
                    maxTemperature = 500f;
                    break;
            }
        }

        /// <summary>
        /// Создание компонентов лазера
        /// </summary>
        private void CreateLaserComponents()
        {
            // Создание излучателя лазера
            if (laserEmitter == null)
            {
                CreateLaserEmitter();
            }

            // Создание луча лазера
            if (laserBeam == null)
            {
                CreateLaserBeam();
            }

            // Создание LineRenderer для лазерного луча
            if (laserLine == null)
            {
                CreateLaserLine();
            }

            // Применение материала
            if (laserMaterial != null)
            {
                if (laserEmitter != null)
                {
                    laserEmitter.GetComponent<Renderer>().material = laserMaterial;
                }
                if (laserBeam != null)
                {
                    laserBeam.GetComponent<Renderer>().material = laserMaterial;
                }
            }

            // Создание системы частиц для лазера
            if (enableLaserEffect && laserParticles == null)
            {
                CreateLaserParticles();
            }

            isInitialized = true;
        }

        /// <summary>
        /// Создание излучателя лазера
        /// </summary>
        private void CreateLaserEmitter()
        {
            GameObject emitter = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            emitter.name = "LaserEmitter";
            emitter.transform.SetParent(transform);
            emitter.transform.localPosition = Vector3.zero;
            emitter.transform.localRotation = Quaternion.identity;
            emitter.transform.localScale = new Vector3(0.1f, 0.2f, 0.1f);

            // Удаление коллайдера у излучателя
            DestroyImmediate(emitter.GetComponent<Collider>());

            laserEmitter = emitter.transform;
        }

        /// <summary>
        /// Создание луча лазера
        /// </summary>
        private void CreateLaserBeam()
        {
            GameObject beam = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            beam.name = "LaserBeam";
            beam.transform.SetParent(transform);
            beam.transform.localPosition = new Vector3(0, 0, laserRange * 0.5f);
            beam.transform.localRotation = Quaternion.identity;
            beam.transform.localScale = new Vector3(0.005f, laserRange, 0.005f);

            // Удаление коллайдера у луча
            DestroyImmediate(beam.GetComponent<Collider>());

            laserBeam = beam.transform;
        }

        /// <summary>
        /// Создание LineRenderer для лазерного луча
        /// </summary>
        private void CreateLaserLine()
        {
            laserLine = gameObject.AddComponent<LineRenderer>();
            laserLine.material = laserMaterial;
            laserLine.startWidth = 0.01f;
            laserLine.endWidth = 0.01f;
            laserLine.positionCount = 2;
            laserLine.useWorldSpace = true;
            laserLine.enabled = false;
        }

        /// <summary>
        /// Создание системы частиц для лазера
        /// </summary>
        private void CreateLaserParticles()
        {
            GameObject particleObj = new GameObject("LaserParticles");
            particleObj.transform.SetParent(transform);
            particleObj.transform.localPosition = Vector3.zero;

            laserParticles = particleObj.AddComponent<ParticleSystem>();
            
            // Настройка системы частиц
            var main = laserParticles.main;
            main.startLifetime = 0.2f;
            main.startSpeed = 5f;
            main.startSize = 0.01f;
            main.maxParticles = 100;
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = laserParticles.emission;
            emission.rateOverTime = 0f; // Начинаем с 0

            var shape = laserParticles.shape;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 5f;
            shape.radius = 0.02f;

            var colorOverLifetime = laserParticles.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.orange, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
            colorOverLifetime.color = gradient;
        }

        /// <summary>
        /// Активировать лазер
        /// </summary>
        public void ActivateLaser()
        {
            laserActive = true;
            Debug.Log($"Laser cutter activated: {cutterType}");
        }

        /// <summary>
        /// Деактивировать лазер
        /// </summary>
        public void DeactivateLaser()
        {
            laserActive = false;
            currentPower = 0f;
            isCutting = false;
            
            // Отключение визуальных эффектов
            if (laserLine != null)
            {
                laserLine.enabled = false;
            }
            
            Debug.Log($"Laser cutter deactivated: {cutterType}");
        }

        /// <summary>
        /// Установить мощность лазера
        /// </summary>
        public void SetLaserPower(float power)
        {
            if (!laserActive) return;
            
            currentPower = Mathf.Clamp(power, 0f, laserPower);
            Debug.Log($"Laser power set to: {currentPower}W");
        }

        /// <summary>
        /// Получить текущую мощность лазера
        /// </summary>
        public float GetLaserPower()
        {
            return currentPower;
        }

        /// <summary>
        /// Получить максимальную мощность лазера
        /// </summary>
        public float GetMaxLaserPower()
        {
            return laserPower;
        }

        /// <summary>
        /// Получить температуру лазера
        /// </summary>
        public float GetLaserTemperature()
        {
            return temperature;
        }

        /// <summary>
        /// Получить тип лазера
        /// </summary>
        public LaserCutterType GetCutterType()
        {
            return cutterType;
        }

        /// <summary>
        /// Получить информацию о лазере
        /// </summary>
        public override string GetPartInfo()
        {
            string info = base.GetPartInfo();
            info += $"\nТип: {GetLocalizedCutterType()}";
            info += $"\nМощность: {laserPower}Вт";
            info += $"\nДальность: {laserRange}м";
            info += $"\nТочность: {laserAccuracy}мм";
            info += $"\nСкорость: {cuttingSpeed} м/с";
            info += $"\nТемпература: {temperature:F1}°C";
            info += $"\nСтатус: {(laserActive ? "Активен" : "Неактивен")}";
            return info;
        }

        /// <summary>
        /// Получить локализованное название типа лазера
        /// </summary>
        private string GetLocalizedCutterType()
        {
            return cutterType switch
            {
                LaserCutterType.Standard => "Стандартный",
                LaserCutterType.Precision => "Точный",
                LaserCutterType.Industrial => "Промышленный",
                LaserCutterType.Experimental => "Экспериментальный",
                _ => cutterType.ToString()
            };
        }

        /// <summary>
        /// Получить стоимость лазера
        /// </summary>
        public int GetPartCost()
        {
            return cutterType switch
            {
                LaserCutterType.Standard => 250,
                LaserCutterType.Precision => 400,
                LaserCutterType.Industrial => 600,
                LaserCutterType.Experimental => 800,
                _ => 250
            };
        }

        /// <summary>
        /// Получить вес лазера
        /// </summary>
        public float GetPartWeight()
        {
            return cutterType switch
            {
                LaserCutterType.Standard => 3f,
                LaserCutterType.Precision => 2.5f,
                LaserCutterType.Industrial => 5f,
                LaserCutterType.Experimental => 4f,
                _ => 3f
            };
        }

        private void Update()
        {
            if (!isInitialized || !laserActive) return;

            // Обновление температуры
            UpdateTemperature();

            // Обновление лазерного луча
            UpdateLaserBeam();

            // Обновление системы частиц
            UpdateParticles();

            // Проверка возможности резки
            CheckCutting();
        }

        /// <summary>
        /// Обновление температуры лазера
        /// </summary>
        private void UpdateTemperature()
        {
            if (currentPower > 0f)
            {
                // Нагрев при работе
                temperature += Time.deltaTime * (currentPower / laserPower) * 10f;
                temperature = Mathf.Min(temperature, maxTemperature);
            }
            else
            {
                // Охлаждение при выключенном лазере
                temperature -= Time.deltaTime * 5f;
                temperature = Mathf.Max(temperature, 20f);
            }
        }

        /// <summary>
        /// Обновление лазерного луча
        /// </summary>
        private void UpdateLaserBeam()
        {
            if (laserLine != null && currentPower > 0f)
            {
                laserLine.enabled = true;
                
                Vector3 startPos = transform.position;
                Vector3 endPos = startPos + transform.forward * laserRange;
                
                // Проверка попадания лазера
                if (Physics.Raycast(startPos, transform.forward, out laserHit, laserRange, cuttableLayers))
                {
                    endPos = laserHit.point;
                    isCutting = true;
                }
                else
                {
                    isCutting = false;
                }
                
                laserLine.SetPosition(0, startPos);
                laserLine.SetPosition(1, endPos);
                
                // Цвет лазера в зависимости от мощности
                Color laserColor = Color.Lerp(Color.red, Color.orange, currentPower / laserPower);
                laserLine.startColor = laserColor;
                laserLine.endColor = laserColor;
            }
            else if (laserLine != null)
            {
                laserLine.enabled = false;
            }
        }

        /// <summary>
        /// Обновление системы частиц
        /// </summary>
        private void UpdateParticles()
        {
            if (laserParticles != null)
            {
                var emission = laserParticles.emission;
                if (currentPower > 0f)
                {
                    emission.rateOverTime = (currentPower / laserPower) * 50f;
                }
                else
                {
                    emission.rateOverTime = 0f;
                }
            }
        }

        /// <summary>
        /// Проверка возможности резки
        /// </summary>
        private void CheckCutting()
        {
            if (isCutting && currentPower > laserPower * 0.5f)
            {
                if (Time.time - lastCutTime >= 1f / cuttingSpeed)
                {
                    // Здесь можно добавить логику резки объектов
                    Debug.Log($"Cutting at point: {laserHit.point}");
                    lastCutTime = Time.time;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!laserActive) return;

            // Отображение лазерного луча в редакторе
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * laserRange);
            
            // Отображение зоны действия
            Gizmos.color = Color.Lerp(Color.red, Color.orange, currentPower / laserPower);
            Gizmos.DrawWireSphere(transform.position, 0.1f);
        }
    }
}
