using UnityEngine;

namespace ScrapArchitect.Parts
{
    /// <summary>
    /// Дрель - продвинутый инструмент для сверления отверстий в материалах
    /// </summary>
    public class Drill : PartBase
    {
        [Header("Drill Settings")]
        public DrillType drillType = DrillType.Standard;
        public float drillPower = 150f;
        public float drillSpeed = 1000f;
        public float drillTorque = 50f;
        public float drillRange = 1f;
        public bool drillActive = false;

        [Header("Drill Physics")]
        public LayerMask drillableLayers = -1;
        public Transform drillBit;
        public Transform drillBody;
        public Material drillMaterial;
        public bool enableDrillEffect = true;
        public ParticleSystem drillParticles;
        public AudioSource drillSound;

        [Header("Drill Output")]
        public float currentPower = 0f;
        public float currentSpeed = 0f;
        public float temperature = 20f;
        public float maxTemperature = 150f;
        public bool isDrilling = false;
        public float drillProgress = 0f;

        // Компоненты дрели
        private RaycastHit drillHit;
        private bool isInitialized = false;
        private float lastDrillTime = 0f;
        private GameObject currentDrillTarget = null;
        private float drillBitRotation = 0f;

        public enum DrillType
        {
            Standard,    // Стандартная дрель
            Impact,      // Ударная дрель
            Precision,   // Точная дрель
            Industrial   // Промышленная дрель
        }

        protected override void InitializePart()
        {
            base.InitializePart();
            
            // Настройка типа дрели
            ConfigureDrillType();
            
            // Создание компонентов дрели
            CreateDrillComponents();
            
            Debug.Log($"Drill initialized: {drillType} type");
        }

        /// <summary>
        /// Настройка типа дрели
        /// </summary>
        private void ConfigureDrillType()
        {
            switch (drillType)
            {
                case DrillType.Standard:
                    drillPower = 150f;
                    drillSpeed = 1000f;
                    drillTorque = 50f;
                    drillRange = 1f;
                    maxTemperature = 150f;
                    break;
                    
                case DrillType.Impact:
                    drillPower = 200f;
                    drillSpeed = 800f;
                    drillTorque = 80f;
                    drillRange = 1.2f;
                    maxTemperature = 180f;
                    break;
                    
                case DrillType.Precision:
                    drillPower = 100f;
                    drillSpeed = 2000f;
                    drillTorque = 30f;
                    drillRange = 0.8f;
                    maxTemperature = 120f;
                    break;
                    
                case DrillType.Industrial:
                    drillPower = 300f;
                    drillSpeed = 600f;
                    drillTorque = 120f;
                    drillRange = 1.5f;
                    maxTemperature = 200f;
                    break;
            }
        }

        /// <summary>
        /// Создание компонентов дрели
        /// </summary>
        private void CreateDrillComponents()
        {
            // Создание корпуса дрели
            if (drillBody == null)
            {
                CreateDrillBody();
            }

            // Создание сверла
            if (drillBit == null)
            {
                CreateDrillBit();
            }

            // Создание звука дрели
            if (drillSound == null)
            {
                CreateDrillSound();
            }

            // Применение материала
            if (drillMaterial != null)
            {
                if (drillBody != null)
                {
                    drillBody.GetComponent<Renderer>().material = drillMaterial;
                }
                if (drillBit != null)
                {
                    drillBit.GetComponent<Renderer>().material = drillMaterial;
                }
            }

            // Создание системы частиц для дрели
            if (enableDrillEffect && drillParticles == null)
            {
                CreateDrillParticles();
            }

            isInitialized = true;
        }

        /// <summary>
        /// Создание корпуса дрели
        /// </summary>
        private void CreateDrillBody()
        {
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            body.name = "DrillBody";
            body.transform.SetParent(transform);
            body.transform.localPosition = Vector3.zero;
            body.transform.localRotation = Quaternion.identity;
            body.transform.localScale = new Vector3(0.08f, 0.4f, 0.08f);

            // Удаление коллайдера у корпуса
            DestroyImmediate(body.GetComponent<Collider>());

            drillBody = body.transform;
        }

        /// <summary>
        /// Создание сверла
        /// </summary>
        private void CreateDrillBit()
        {
            GameObject bit = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            bit.name = "DrillBit";
            bit.transform.SetParent(transform);
            bit.transform.localPosition = new Vector3(0, 0, 0.3f);
            bit.transform.localRotation = Quaternion.identity;
            bit.transform.localScale = new Vector3(0.02f, 0.2f, 0.02f);

            // Удаление коллайдера у сверла
            DestroyImmediate(bit.GetComponent<Collider>());

            drillBit = bit.transform;
        }

        /// <summary>
        /// Создание звука дрели
        /// </summary>
        private void CreateDrillSound()
        {
            drillSound = gameObject.AddComponent<AudioSource>();
            drillSound.playOnAwake = false;
            drillSound.loop = true;
            drillSound.volume = 0.5f;
            drillSound.pitch = 1f;
        }

        /// <summary>
        /// Создание системы частиц для дрели
        /// </summary>
        private void CreateDrillParticles()
        {
            GameObject particleObj = new GameObject("DrillParticles");
            particleObj.transform.SetParent(transform);
            particleObj.transform.localPosition = new Vector3(0, 0, 0.4f);

            drillParticles = particleObj.AddComponent<ParticleSystem>();
            
            // Настройка системы частиц
            var main = drillParticles.main;
            main.startLifetime = 0.3f;
            main.startSpeed = 2f;
            main.startSize = 0.01f;
            main.maxParticles = 80;
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = drillParticles.emission;
            emission.rateOverTime = 0f; // Начинаем с 0

            var shape = drillParticles.shape;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 15f;
            shape.radius = 0.02f;

            var colorOverLifetime = drillParticles.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.gray, 0.0f), new GradientColorKey(Color.white, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
            colorOverLifetime.color = gradient;
        }

        /// <summary>
        /// Активировать дрель
        /// </summary>
        public void ActivateDrill()
        {
            drillActive = true;
            Debug.Log($"Drill activated: {drillType}");
        }

        /// <summary>
        /// Деактивировать дрель
        /// </summary>
        public void DeactivateDrill()
        {
            drillActive = false;
            currentPower = 0f;
            currentSpeed = 0f;
            isDrilling = false;
            drillProgress = 0f;
            currentDrillTarget = null;
            
            // Остановка звука
            if (drillSound != null)
            {
                drillSound.Stop();
            }
            
            Debug.Log($"Drill deactivated: {drillType}");
        }

        /// <summary>
        /// Установить мощность дрели
        /// </summary>
        public void SetDrillPower(float power)
        {
            if (!drillActive) return;
            
            currentPower = Mathf.Clamp(power, 0f, drillPower);
            currentSpeed = (currentPower / drillPower) * drillSpeed;
            Debug.Log($"Drill power set to: {currentPower}W, speed: {currentSpeed} RPM");
        }

        /// <summary>
        /// Получить текущую мощность дрели
        /// </summary>
        public float GetDrillPower()
        {
            return currentPower;
        }

        /// <summary>
        /// Получить максимальную мощность дрели
        /// </summary>
        public float GetMaxDrillPower()
        {
            return drillPower;
        }

        /// <summary>
        /// Получить текущую скорость дрели
        /// </summary>
        public float GetDrillSpeed()
        {
            return currentSpeed;
        }

        /// <summary>
        /// Получить температуру дрели
        /// </summary>
        public float GetDrillTemperature()
        {
            return temperature;
        }

        /// <summary>
        /// Получить прогресс сверления
        /// </summary>
        public float GetDrillProgress()
        {
            return drillProgress;
        }

        /// <summary>
        /// Получить тип дрели
        /// </summary>
        public DrillType GetDrillType()
        {
            return drillType;
        }

        /// <summary>
        /// Получить информацию о дрели
        /// </summary>
        public override string GetPartInfo()
        {
            string info = base.GetPartInfo();
            info += $"\nТип: {GetLocalizedDrillType()}";
            info += $"\nМощность: {drillPower}Вт";
            info += $"\nСкорость: {drillSpeed} об/мин";
            info += $"\nКрутящий момент: {drillTorque} Н⋅м";
            info += $"\nДальность: {drillRange}м";
            info += $"\nТемпература: {temperature:F1}°C";
            info += $"\nПрогресс: {drillProgress:P0}";
            info += $"\nСтатус: {(drillActive ? "Активна" : "Неактивна")}";
            return info;
        }

        /// <summary>
        /// Получить локализованное название типа дрели
        /// </summary>
        private string GetLocalizedDrillType()
        {
            return drillType switch
            {
                DrillType.Standard => "Стандартная",
                DrillType.Impact => "Ударная",
                DrillType.Precision => "Точная",
                DrillType.Industrial => "Промышленная",
                _ => drillType.ToString()
            };
        }

        /// <summary>
        /// Получить стоимость дрели
        /// </summary>
        public int GetPartCost()
        {
            return drillType switch
            {
                DrillType.Standard => 180,
                DrillType.Impact => 250,
                DrillType.Precision => 300,
                DrillType.Industrial => 400,
                _ => 180
            };
        }

        /// <summary>
        /// Получить вес дрели
        /// </summary>
        public float GetPartWeight()
        {
            return drillType switch
            {
                DrillType.Standard => 2f,
                DrillType.Impact => 3f,
                DrillType.Precision => 1.5f,
                DrillType.Industrial => 4f,
                _ => 2f
            };
        }

        private void Update()
        {
            if (!isInitialized || !drillActive) return;

            // Обновление температуры
            UpdateTemperature();

            // Обновление вращения сверла
            UpdateDrillRotation();

            // Обновление процесса сверления
            UpdateDrillingProcess();

            // Обновление визуальных эффектов
            UpdateVisualEffects();

            // Обновление звука
            UpdateDrillSound();
        }

        /// <summary>
        /// Обновление температуры дрели
        /// </summary>
        private void UpdateTemperature()
        {
            if (currentPower > 0f)
            {
                // Нагрев при работе
                temperature += Time.deltaTime * (currentPower / drillPower) * 8f;
                temperature = Mathf.Min(temperature, maxTemperature);
            }
            else
            {
                // Охлаждение при выключенной дрели
                temperature -= Time.deltaTime * 2f;
                temperature = Mathf.Max(temperature, 20f);
            }
        }

        /// <summary>
        /// Обновление вращения сверла
        /// </summary>
        private void UpdateDrillRotation()
        {
            if (drillBit != null && currentSpeed > 0f)
            {
                drillBitRotation += currentSpeed * Time.deltaTime * 6f; // 6 градусов в секунду на 1 RPM
                drillBit.localRotation = Quaternion.Euler(0, 0, drillBitRotation);
            }
        }

        /// <summary>
        /// Обновление процесса сверления
        /// </summary>
        private void UpdateDrillingProcess()
        {
            if (currentPower > 0f)
            {
                // Проверка попадания сверла
                if (Physics.Raycast(transform.position, transform.forward, out drillHit, drillRange, drillableLayers))
                {
                    if (!isDrilling)
                    {
                        // Начало сверления
                        isDrilling = true;
                        currentDrillTarget = drillHit.collider.gameObject;
                        drillProgress = 0f;
                        Debug.Log($"Started drilling: {currentDrillTarget.name}");
                    }

                    // Обновление прогресса сверления
                    if (isDrilling && currentDrillTarget == drillHit.collider.gameObject)
                    {
                        float drillEfficiency = (currentPower / drillPower) * (currentSpeed / drillSpeed);
                        drillProgress += Time.deltaTime * drillEfficiency * 0.5f; // Скорость сверления
                        drillProgress = Mathf.Clamp01(drillProgress);

                        // Завершение сверления
                        if (drillProgress >= 1f)
                        {
                            CompleteDrilling();
                        }
                    }
                }
                else
                {
                    // Прерывание сверления
                    if (isDrilling)
                    {
                        isDrilling = false;
                        currentDrillTarget = null;
                        Debug.Log("Drilling interrupted");
                    }
                }
            }
            else
            {
                // Остановка сверления при выключенном питании
                if (isDrilling)
                {
                    isDrilling = false;
                    currentDrillTarget = null;
                    Debug.Log("Drilling stopped - no power");
                }
            }
        }

        /// <summary>
        /// Завершение сверления
        /// </summary>
        private void CompleteDrilling()
        {
            if (currentDrillTarget != null)
            {
                // Здесь можно добавить логику создания отверстия в объекте
                Debug.Log($"Drilling completed: {currentDrillTarget.name}");
                
                // Сброс прогресса
                drillProgress = 0f;
                isDrilling = false;
                currentDrillTarget = null;
            }
        }

        /// <summary>
        /// Обновление визуальных эффектов
        /// </summary>
        private void UpdateVisualEffects()
        {
            // Обновление системы частиц
            if (drillParticles != null)
            {
                var emission = drillParticles.emission;
                if (isDrilling && currentPower > 0f)
                {
                    emission.rateOverTime = (currentPower / drillPower) * 60f;
                }
                else
                {
                    emission.rateOverTime = 0f;
                }
            }
        }

        /// <summary>
        /// Обновление звука дрели
        /// </summary>
        private void UpdateDrillSound()
        {
            if (drillSound != null)
            {
                if (currentPower > 0f)
                {
                    if (!drillSound.isPlaying)
                    {
                        drillSound.Play();
                    }
                    
                    // Настройка звука в зависимости от мощности
                    drillSound.volume = (currentPower / drillPower) * 0.5f;
                    drillSound.pitch = 0.8f + (currentSpeed / drillSpeed) * 0.4f;
                }
                else
                {
                    if (drillSound.isPlaying)
                    {
                        drillSound.Stop();
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!drillActive) return;

            // Отображение направления сверления в редакторе
            Gizmos.color = Color.gray;
            Gizmos.DrawRay(transform.position, transform.forward * drillRange);
            
            // Отображение зоны действия
            Gizmos.color = Color.Lerp(Color.gray, Color.white, currentPower / drillPower);
            Gizmos.DrawWireSphere(transform.position, 0.1f);
            
            // Отображение прогресса сверления
            if (isDrilling)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(drillHit.point, 0.03f);
            }
        }
    }
}
