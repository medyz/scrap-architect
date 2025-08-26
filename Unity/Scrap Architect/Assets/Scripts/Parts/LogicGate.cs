using UnityEngine;

namespace ScrapArchitect.Parts
{
    /// <summary>
    /// Логический элемент - базовый компонент для создания цифровых схем
    /// </summary>
    public class LogicGate : PartBase
    {
        [Header("Logic Gate Settings")]
        public LogicGateType gateType = LogicGateType.AND;
        public int gateInputs = 2;
        public int gateOutputs = 1;
        public float gateDelay = 0.01f;
        public bool gateActive = false;

        [Header("Logic Gate Visual")]
        public Transform gateBody;
        public Transform[] inputPins;
        public Transform[] outputPins;
        public Material gateMaterial;
        public Material activeMaterial;
        public bool enableGateEffect = true;
        public ParticleSystem gateParticles;

        [Header("Logic Gate State")]
        public bool[] inputStates;
        public bool[] outputStates;
        public float[] inputValues;
        public float[] outputValues;

        // Компоненты логического элемента
        private bool[] previousInputStates;
        private float lastUpdateTime = 0f;
        private bool isInitialized = false;

        public enum LogicGateType
        {
            AND,        // Логическое И
            OR,         // Логическое ИЛИ
            NOT,        // Логическое НЕ
            NAND,       // И-НЕ
            NOR,        // ИЛИ-НЕ
            XOR,        // Исключающее ИЛИ
            XNOR,       // Исключающее ИЛИ-НЕ
            Buffer      // Буфер
        }

        protected override void InitializePart()
        {
            base.InitializePart();
            
            // Настройка типа логического элемента
            ConfigureGateType();
            
            // Создание компонентов логического элемента
            CreateGateComponents();
            
            Debug.Log($"Logic gate initialized: {gateType} type");
        }

        /// <summary>
        /// Специфичное действие для логического элемента
        /// </summary>
        protected override void OnPartSpecificAction()
        {
            // Логика специфичная для логического элемента
            if (gateActive)
            {
                ProcessLogic();
                UpdateVisualIndicators();
            }
        }

        /// <summary>
        /// Настройка типа логического элемента
        /// </summary>
        private void ConfigureGateType()
        {
            switch (gateType)
            {
                case LogicGateType.AND:
                    gateInputs = 2;
                    gateOutputs = 1;
                    gateDelay = 0.01f;
                    break;
                    
                case LogicGateType.OR:
                    gateInputs = 2;
                    gateOutputs = 1;
                    gateDelay = 0.01f;
                    break;
                    
                case LogicGateType.NOT:
                    gateInputs = 1;
                    gateOutputs = 1;
                    gateDelay = 0.005f;
                    break;
                    
                case LogicGateType.NAND:
                    gateInputs = 2;
                    gateOutputs = 1;
                    gateDelay = 0.01f;
                    break;
                    
                case LogicGateType.NOR:
                    gateInputs = 2;
                    gateOutputs = 1;
                    gateDelay = 0.01f;
                    break;
                    
                case LogicGateType.XOR:
                    gateInputs = 2;
                    gateOutputs = 1;
                    gateDelay = 0.015f;
                    break;
                    
                case LogicGateType.XNOR:
                    gateInputs = 2;
                    gateOutputs = 1;
                    gateDelay = 0.015f;
                    break;
                    
                case LogicGateType.Buffer:
                    gateInputs = 1;
                    gateOutputs = 1;
                    gateDelay = 0.005f;
                    break;
            }
        }

        /// <summary>
        /// Создание компонентов логического элемента
        /// </summary>
        private void CreateGateComponents()
        {
            // Создание корпуса логического элемента
            if (gateBody == null)
            {
                CreateGateBody();
            }

            // Создание входных контактов
            CreateInputPins();

            // Создание выходных контактов
            CreateOutputPins();

            // Инициализация массивов состояний
            inputStates = new bool[gateInputs];
            outputStates = new bool[gateOutputs];
            inputValues = new float[gateInputs];
            outputValues = new float[gateOutputs];
            previousInputStates = new bool[gateInputs];

            // Применение материала
            if (gateMaterial != null)
            {
                if (gateBody != null)
                {
                    gateBody.GetComponent<Renderer>().material = gateMaterial;
                }
            }

            // Создание системы частиц для логического элемента
            if (enableGateEffect && gateParticles == null)
            {
                CreateGateParticles();
            }

            isInitialized = true;
        }

        /// <summary>
        /// Создание корпуса логического элемента
        /// </summary>
        private void CreateGateBody()
        {
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
            body.name = "LogicGateBody";
            body.transform.SetParent(transform);
            body.transform.localPosition = Vector3.zero;
            body.transform.localRotation = Quaternion.identity;
            body.transform.localScale = new Vector3(0.2f, 0.15f, 0.25f);

            // Удаление коллайдера у корпуса
            DestroyImmediate(body.GetComponent<Collider>());

            gateBody = body.transform;
        }

        /// <summary>
        /// Создание входных контактов
        /// </summary>
        private void CreateInputPins()
        {
            inputPins = new Transform[gateInputs];
            
            for (int i = 0; i < gateInputs; i++)
            {
                GameObject pin = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                pin.name = $"InputPin_{i}";
                pin.transform.SetParent(transform);
                
                // Позиционирование контактов
                float yPos = 0.05f - (i * 0.03f);
                pin.transform.localPosition = new Vector3(-0.15f, yPos, 0);
                pin.transform.localRotation = Quaternion.Euler(0, 0, 90);
                pin.transform.localScale = new Vector3(0.01f, 0.02f, 0.01f);

                // Удаление коллайдера
                DestroyImmediate(pin.GetComponent<Collider>());

                // Применение материала
                if (gateMaterial != null)
                {
                    pin.GetComponent<Renderer>().material = gateMaterial;
                }

                inputPins[i] = pin.transform;
            }
        }

        /// <summary>
        /// Создание выходных контактов
        /// </summary>
        private void CreateOutputPins()
        {
            outputPins = new Transform[gateOutputs];
            
            for (int i = 0; i < gateOutputs; i++)
            {
                GameObject pin = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                pin.name = $"OutputPin_{i}";
                pin.transform.SetParent(transform);
                
                // Позиционирование контактов
                float yPos = 0.05f - (i * 0.03f);
                pin.transform.localPosition = new Vector3(0.15f, yPos, 0);
                pin.transform.localRotation = Quaternion.Euler(0, 0, 90);
                pin.transform.localScale = new Vector3(0.01f, 0.02f, 0.01f);

                // Удаление коллайдера
                DestroyImmediate(pin.GetComponent<Collider>());

                // Применение материала
                if (gateMaterial != null)
                {
                    pin.GetComponent<Renderer>().material = gateMaterial;
                }

                outputPins[i] = pin.transform;
            }
        }

        /// <summary>
        /// Создание системы частиц для логического элемента
        /// </summary>
        private void CreateGateParticles()
        {
            GameObject particleObj = new GameObject("GateParticles");
            particleObj.transform.SetParent(transform);
            particleObj.transform.localPosition = new Vector3(0, 0.05f, 0);

            gateParticles = particleObj.AddComponent<ParticleSystem>();
            
            // Настройка системы частиц
            var main = gateParticles.main;
            main.startLifetime = 0.2f;
            main.startSpeed = 0.5f;
            main.startSize = 0.005f;
            main.maxParticles = 20;
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = gateParticles.emission;
            emission.rateOverTime = 0f; // Начинаем с 0

            var shape = gateParticles.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 0.05f;

            var colorOverLifetime = gateParticles.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.gray, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
            colorOverLifetime.color = gradient;
        }

        /// <summary>
        /// Активировать логический элемент
        /// </summary>
        public void ActivateGate()
        {
            gateActive = true;
            Debug.Log($"Logic gate activated: {gateType}");
        }

        /// <summary>
        /// Деактивировать логический элемент
        /// </summary>
        public void DeactivateGate()
        {
            gateActive = false;
            
            // Сброс всех выходов
            for (int i = 0; i < gateOutputs; i++)
            {
                outputStates[i] = false;
                outputValues[i] = 0f;
            }
            
            Debug.Log($"Logic gate deactivated: {gateType}");
        }

        /// <summary>
        /// Установить состояние входа
        /// </summary>
        public void SetInput(int inputIndex, bool state, float value = 0f)
        {
            if (inputIndex >= 0 && inputIndex < gateInputs)
            {
                inputStates[inputIndex] = state;
                inputValues[inputIndex] = value;
            }
        }

        /// <summary>
        /// Получить состояние входа
        /// </summary>
        public bool GetInputState(int inputIndex)
        {
            if (inputIndex >= 0 && inputIndex < gateInputs)
            {
                return inputStates[inputIndex];
            }
            return false;
        }

        /// <summary>
        /// Получить значение входа
        /// </summary>
        public float GetInputValue(int inputIndex)
        {
            if (inputIndex >= 0 && inputIndex < gateInputs)
            {
                return inputValues[inputIndex];
            }
            return 0f;
        }

        /// <summary>
        /// Получить состояние выхода
        /// </summary>
        public bool GetOutputState(int outputIndex)
        {
            if (outputIndex >= 0 && outputIndex < gateOutputs)
            {
                return outputStates[outputIndex];
            }
            return false;
        }

        /// <summary>
        /// Получить значение выхода
        /// </summary>
        public float GetOutputValue(int outputIndex)
        {
            if (outputIndex >= 0 && outputIndex < gateOutputs)
            {
                return outputValues[outputIndex];
            }
            return 0f;
        }

        /// <summary>
        /// Получить тип логического элемента
        /// </summary>
        public LogicGateType GetGateType()
        {
            return gateType;
        }

        /// <summary>
        /// Получить информацию о логическом элементе
        /// </summary>
        public override string GetPartInfo()
        {
            string info = base.GetPartInfo();
            info += $"\nТип: {GetLocalizedGateType()}";
            info += $"\nВходы: {gateInputs}";
            info += $"\nВыходы: {gateOutputs}";
            info += $"\nЗадержка: {gateDelay * 1000:F1} мс";
            info += $"\nСтатус: {(gateActive ? "Активен" : "Неактивен")}";
            return info;
        }

        /// <summary>
        /// Получить локализованное название типа логического элемента
        /// </summary>
        private string GetLocalizedGateType()
        {
            return gateType switch
            {
                LogicGateType.AND => "И",
                LogicGateType.OR => "ИЛИ",
                LogicGateType.NOT => "НЕ",
                LogicGateType.NAND => "И-НЕ",
                LogicGateType.NOR => "ИЛИ-НЕ",
                LogicGateType.XOR => "Исключающее ИЛИ",
                LogicGateType.XNOR => "Исключающее ИЛИ-НЕ",
                LogicGateType.Buffer => "Буфер",
                _ => gateType.ToString()
            };
        }

        /// <summary>
        /// Получить стоимость логического элемента
        /// </summary>
        public int GetPartCost()
        {
            return gateType switch
            {
                LogicGateType.AND => 50,
                LogicGateType.OR => 50,
                LogicGateType.NOT => 30,
                LogicGateType.NAND => 60,
                LogicGateType.NOR => 60,
                LogicGateType.XOR => 80,
                LogicGateType.XNOR => 80,
                LogicGateType.Buffer => 20,
                _ => 50
            };
        }

        /// <summary>
        /// Получить вес логического элемента
        /// </summary>
        public float GetPartWeight()
        {
            return gateType switch
            {
                LogicGateType.AND => 1f,
                LogicGateType.OR => 1f,
                LogicGateType.NOT => 0.8f,
                LogicGateType.NAND => 1.2f,
                LogicGateType.NOR => 1.2f,
                LogicGateType.XOR => 1.5f,
                LogicGateType.XNOR => 1.5f,
                LogicGateType.Buffer => 0.5f,
                _ => 1f
            };
        }

        private void Update()
        {
            if (!isInitialized || !gateActive) return;

            // Обновление логического элемента с учетом задержки
            if (Time.time - lastUpdateTime >= gateDelay)
            {
                ProcessLogic();
                lastUpdateTime = Time.time;
            }

            // Обновление визуальных индикаторов
            UpdateVisualIndicators();

            // Обновление системы частиц
            if (gateParticles != null)
            {
                var emission = gateParticles.emission;
                if (gateActive && HasOutputChanged())
                {
                    emission.rateOverTime = 15f;
                }
                else
                {
                    emission.rateOverTime = 0f;
                }
            }
        }

        /// <summary>
        /// Обработка логики
        /// </summary>
        private void ProcessLogic()
        {
            bool result = false;

            switch (gateType)
            {
                case LogicGateType.AND:
                    result = true;
                    for (int i = 0; i < gateInputs; i++)
                    {
                        if (!inputStates[i])
                        {
                            result = false;
                            break;
                        }
                    }
                    break;

                case LogicGateType.OR:
                    result = false;
                    for (int i = 0; i < gateInputs; i++)
                    {
                        if (inputStates[i])
                        {
                            result = true;
                            break;
                        }
                    }
                    break;

                case LogicGateType.NOT:
                    result = !inputStates[0];
                    break;

                case LogicGateType.NAND:
                    result = true;
                    for (int i = 0; i < gateInputs; i++)
                    {
                        if (!inputStates[i])
                        {
                            result = false;
                            break;
                        }
                    }
                    result = !result;
                    break;

                case LogicGateType.NOR:
                    result = true;
                    for (int i = 0; i < gateInputs; i++)
                    {
                        if (inputStates[i])
                        {
                            result = false;
                            break;
                        }
                    }
                    break;

                case LogicGateType.XOR:
                    int trueCount = 0;
                    for (int i = 0; i < gateInputs; i++)
                    {
                        if (inputStates[i]) trueCount++;
                    }
                    result = trueCount == 1;
                    break;

                case LogicGateType.XNOR:
                    trueCount = 0;
                    for (int i = 0; i < gateInputs; i++)
                    {
                        if (inputStates[i]) trueCount++;
                    }
                    result = trueCount != 1;
                    break;

                case LogicGateType.Buffer:
                    result = inputStates[0];
                    break;
            }

            // Установка выходов
            for (int i = 0; i < gateOutputs; i++)
            {
                outputStates[i] = result;
                outputValues[i] = result ? 1f : 0f;
            }

            // Сохранение предыдущих состояний входов
            for (int i = 0; i < gateInputs; i++)
            {
                previousInputStates[i] = inputStates[i];
            }
        }

        /// <summary>
        /// Проверка изменения выходов
        /// </summary>
        private bool HasOutputChanged()
        {
            for (int i = 0; i < gateInputs; i++)
            {
                if (inputStates[i] != previousInputStates[i])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Обновление визуальных индикаторов
        /// </summary>
        private void UpdateVisualIndicators()
        {
            // Обновление входных контактов
            for (int i = 0; i < inputPins.Length && i < gateInputs; i++)
            {
                if (inputPins[i] != null)
                {
                    Renderer renderer = inputPins[i].GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        if (inputStates[i])
                        {
                            renderer.material.color = Color.green;
                        }
                        else
                        {
                            renderer.material.color = Color.red;
                        }
                    }
                }
            }

            // Обновление выходных контактов
            for (int i = 0; i < outputPins.Length && i < gateOutputs; i++)
            {
                if (outputPins[i] != null)
                {
                    Renderer renderer = outputPins[i].GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        if (outputStates[i])
                        {
                            renderer.material.color = Color.blue;
                        }
                        else
                        {
                            renderer.material.color = Color.gray;
                        }
                    }
                }
            }

            // Обновление корпуса
            if (gateBody != null)
            {
                Renderer bodyRenderer = gateBody.GetComponent<Renderer>();
                if (bodyRenderer != null)
                {
                    if (gateActive)
                    {
                        bodyRenderer.material.color = Color.Lerp(Color.white, Color.yellow, 0.3f);
                    }
                    else
                    {
                        bodyRenderer.material.color = Color.gray;
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!gateActive) return;

            // Отображение логики в редакторе
            Gizmos.color = Color.yellow;
            
            // Входы
            for (int i = 0; i < gateInputs; i++)
            {
                Vector3 inputPos = transform.position + new Vector3(-0.15f, 0.05f - (i * 0.03f), 0);
                Gizmos.color = inputStates[i] ? Color.green : Color.red;
                Gizmos.DrawWireSphere(inputPos, 0.02f);
            }

            // Выходы
            for (int i = 0; i < gateOutputs; i++)
            {
                Vector3 outputPos = transform.position + new Vector3(0.15f, 0.05f - (i * 0.03f), 0);
                Gizmos.color = outputStates[i] ? Color.blue : Color.gray;
                Gizmos.DrawWireSphere(outputPos, 0.02f);
            }
        }
    }
}
