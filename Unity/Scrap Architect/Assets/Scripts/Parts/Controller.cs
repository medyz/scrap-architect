using UnityEngine;
using System.Collections.Generic;

namespace ScrapArchitect.Parts
{
    /// <summary>
    /// Контроллер - электронный компонент для управления другими устройствами
    /// </summary>
    public class Controller : PartBase
    {
        [Header("Controller Settings")]
        public ControllerType controllerType = ControllerType.Basic;
        public int controllerInputs = 4;
        public int controllerOutputs = 4;
        public float controllerProcessingSpeed = 10f;
        public bool controllerActive = false;

        [Header("Controller Logic")]
        public LogicType logicType = LogicType.AND;
        public float thresholdValue = 0.5f;
        public bool invertOutput = false;
        public float delayTime = 0.1f;

        [Header("Controller Visual")]
        public Transform controllerBody;
        public Transform[] inputLights;
        public Transform[] outputLights;
        public Material controllerMaterial;
        public Material activeMaterial;
        public bool enableControllerEffect = true;
        public ParticleSystem controllerParticles;

        [Header("Controller Connections")]
        public List<Sensor> connectedSensors = new List<Sensor>();
        public List<PartBase> controlledDevices = new List<PartBase>();

        // Компоненты контроллера
        private bool[] inputStates;
        private bool[] outputStates;
        private float[] inputValues;
        private float[] outputValues;
        private float lastProcessTime = 0f;
        private bool isInitialized = false;

        public enum ControllerType
        {
            Basic,      // Базовый контроллер
            Advanced,   // Продвинутый контроллер
            Programmable, // Программируемый контроллер
            Logic       // Логический контроллер
        }

        public enum LogicType
        {
            AND,        // Логическое И
            OR,         // Логическое ИЛИ
            NOT,        // Логическое НЕ
            XOR,        // Исключающее ИЛИ
            NAND,       // И-НЕ
            NOR,        // ИЛИ-НЕ
            Threshold,  // Пороговое значение
            Timer       // Таймер
        }

        protected override void InitializePart()
        {
            base.InitializePart();
            
            // Настройка типа контроллера
            ConfigureControllerType();
            
            // Создание компонентов контроллера
            CreateControllerComponents();
            
            Debug.Log($"Controller initialized: {controllerType} type");
        }

        /// <summary>
        /// Настройка типа контроллера
        /// </summary>
        private void ConfigureControllerType()
        {
            switch (controllerType)
            {
                case ControllerType.Basic:
                    controllerInputs = 4;
                    controllerOutputs = 4;
                    controllerProcessingSpeed = 10f;
                    logicType = LogicType.AND;
                    break;
                    
                case ControllerType.Advanced:
                    controllerInputs = 8;
                    controllerOutputs = 8;
                    controllerProcessingSpeed = 20f;
                    logicType = LogicType.OR;
                    break;
                    
                case ControllerType.Programmable:
                    controllerInputs = 12;
                    controllerOutputs = 12;
                    controllerProcessingSpeed = 50f;
                    logicType = LogicType.Threshold;
                    break;
                    
                case ControllerType.Logic:
                    controllerInputs = 6;
                    controllerOutputs = 6;
                    controllerProcessingSpeed = 15f;
                    logicType = LogicType.XOR;
                    break;
            }
        }

        /// <summary>
        /// Создание компонентов контроллера
        /// </summary>
        private void CreateControllerComponents()
        {
            // Создание корпуса контроллера
            if (controllerBody == null)
            {
                CreateControllerBody();
            }

            // Создание индикаторов входов
            CreateInputLights();

            // Создание индикаторов выходов
            CreateOutputLights();

            // Инициализация массивов состояний
            inputStates = new bool[controllerInputs];
            outputStates = new bool[controllerOutputs];
            inputValues = new float[controllerInputs];
            outputValues = new float[controllerOutputs];

            // Применение материала
            if (controllerMaterial != null)
            {
                if (controllerBody != null)
                {
                    controllerBody.GetComponent<Renderer>().material = controllerMaterial;
                }
            }

            // Создание системы частиц для контроллера
            if (enableControllerEffect && controllerParticles == null)
            {
                CreateControllerParticles();
            }

            isInitialized = true;
        }

        /// <summary>
        /// Создание корпуса контроллера
        /// </summary>
        private void CreateControllerBody()
        {
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
            body.name = "ControllerBody";
            body.transform.SetParent(transform);
            body.transform.localPosition = Vector3.zero;
            body.transform.localRotation = Quaternion.identity;
            body.transform.localScale = new Vector3(0.3f, 0.2f, 0.4f);

            // Удаление коллайдера у корпуса
            DestroyImmediate(body.GetComponent<Collider>());

            controllerBody = body.transform;
        }

        /// <summary>
        /// Создание индикаторов входов
        /// </summary>
        private void CreateInputLights()
        {
            inputLights = new Transform[controllerInputs];
            
            for (int i = 0; i < controllerInputs; i++)
            {
                GameObject light = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                light.name = $"InputLight_{i}";
                light.transform.SetParent(transform);
                
                // Позиционирование индикаторов
                float xPos = -0.1f + (i % 4) * 0.05f;
                float yPos = 0.15f - (i / 4) * 0.05f;
                light.transform.localPosition = new Vector3(xPos, yPos, -0.2f);
                light.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

                // Удаление коллайдера
                DestroyImmediate(light.GetComponent<Collider>());

                // Применение материала
                if (controllerMaterial != null)
                {
                    light.GetComponent<Renderer>().material = controllerMaterial;
                }

                inputLights[i] = light.transform;
            }
        }

        /// <summary>
        /// Создание индикаторов выходов
        /// </summary>
        private void CreateOutputLights()
        {
            outputLights = new Transform[controllerOutputs];
            
            for (int i = 0; i < controllerOutputs; i++)
            {
                GameObject light = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                light.name = $"OutputLight_{i}";
                light.transform.SetParent(transform);
                
                // Позиционирование индикаторов
                float xPos = -0.1f + (i % 4) * 0.05f;
                float yPos = 0.15f - (i / 4) * 0.05f;
                light.transform.localPosition = new Vector3(xPos, yPos, 0.2f);
                light.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);

                // Удаление коллайдера
                DestroyImmediate(light.GetComponent<Collider>());

                // Применение материала
                if (controllerMaterial != null)
                {
                    light.GetComponent<Renderer>().material = controllerMaterial;
                }

                outputLights[i] = light.transform;
            }
        }

        /// <summary>
        /// Создание системы частиц для контроллера
        /// </summary>
        private void CreateControllerParticles()
        {
            GameObject particleObj = new GameObject("ControllerParticles");
            particleObj.transform.SetParent(transform);
            particleObj.transform.localPosition = new Vector3(0, 0.1f, 0);

            controllerParticles = particleObj.AddComponent<ParticleSystem>();
            
            // Настройка системы частиц
            var main = controllerParticles.main;
            main.startLifetime = 0.5f;
            main.startSpeed = 1f;
            main.startSize = 0.01f;
            main.maxParticles = 30;
            main.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = controllerParticles.emission;
            emission.rateOverTime = 0f; // Начинаем с 0

            var shape = controllerParticles.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 0.1f;

            var colorOverLifetime = controllerParticles.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.blue, 0.0f), new GradientColorKey(Color.cyan, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );
            colorOverLifetime.color = gradient;
        }

        /// <summary>
        /// Активировать контроллер
        /// </summary>
        public void ActivateController()
        {
            controllerActive = true;
            Debug.Log($"Controller activated: {controllerType}");
        }

        /// <summary>
        /// Деактивировать контроллер
        /// </summary>
        public void DeactivateController()
        {
            controllerActive = false;
            
            // Сброс всех выходов
            for (int i = 0; i < controllerOutputs; i++)
            {
                outputStates[i] = false;
                outputValues[i] = 0f;
            }
            
            Debug.Log($"Controller deactivated: {controllerType}");
        }

        /// <summary>
        /// Подключить датчик к входу
        /// </summary>
        public void ConnectSensor(Sensor sensor, int inputIndex)
        {
            if (inputIndex >= 0 && inputIndex < controllerInputs)
            {
                if (!connectedSensors.Contains(sensor))
                {
                    connectedSensors.Add(sensor);
                }
                Debug.Log($"Sensor connected to input {inputIndex}");
            }
        }

        /// <summary>
        /// Отключить датчик
        /// </summary>
        public void DisconnectSensor(Sensor sensor)
        {
            if (connectedSensors.Contains(sensor))
            {
                connectedSensors.Remove(sensor);
                Debug.Log("Sensor disconnected");
            }
        }

        /// <summary>
        /// Подключить устройство к выходу
        /// </summary>
        public void ConnectDevice(PartBase device, int outputIndex)
        {
            if (outputIndex >= 0 && outputIndex < controllerOutputs)
            {
                if (!controlledDevices.Contains(device))
                {
                    controlledDevices.Add(device);
                }
                Debug.Log($"Device connected to output {outputIndex}");
            }
        }

        /// <summary>
        /// Отключить устройство
        /// </summary>
        public void DisconnectDevice(PartBase device)
        {
            if (controlledDevices.Contains(device))
            {
                controlledDevices.Remove(device);
                Debug.Log("Device disconnected");
            }
        }

        /// <summary>
        /// Получить состояние входа
        /// </summary>
        public bool GetInputState(int inputIndex)
        {
            if (inputIndex >= 0 && inputIndex < controllerInputs)
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
            if (inputIndex >= 0 && inputIndex < controllerInputs)
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
            if (outputIndex >= 0 && outputIndex < controllerOutputs)
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
            if (outputIndex >= 0 && outputIndex < controllerOutputs)
            {
                return outputValues[outputIndex];
            }
            return 0f;
        }

        /// <summary>
        /// Установить логику контроллера
        /// </summary>
        public void SetLogicType(LogicType newLogicType)
        {
            logicType = newLogicType;
            Debug.Log($"Controller logic changed to: {logicType}");
        }

        /// <summary>
        /// Получить тип контроллера
        /// </summary>
        public ControllerType GetControllerType()
        {
            return controllerType;
        }

        /// <summary>
        /// Получить информацию о контроллере
        /// </summary>
        public override string GetPartInfo()
        {
            string info = base.GetPartInfo();
            info += $"\nТип: {GetLocalizedControllerType()}";
            info += $"\nВходы: {controllerInputs}";
            info += $"\nВыходы: {controllerOutputs}";
            info += $"\nСкорость: {controllerProcessingSpeed} Гц";
            info += $"\nЛогика: {GetLocalizedLogicType()}";
            info += $"\nСтатус: {(controllerActive ? "Активен" : "Неактивен")}";
            return info;
        }

        /// <summary>
        /// Получить локализованное название типа контроллера
        /// </summary>
        private string GetLocalizedControllerType()
        {
            return controllerType switch
            {
                ControllerType.Basic => "Базовый",
                ControllerType.Advanced => "Продвинутый",
                ControllerType.Programmable => "Программируемый",
                ControllerType.Logic => "Логический",
                _ => controllerType.ToString()
            };
        }

        /// <summary>
        /// Получить локализованное название типа логики
        /// </summary>
        private string GetLocalizedLogicType()
        {
            return logicType switch
            {
                LogicType.AND => "И",
                LogicType.OR => "ИЛИ",
                LogicType.NOT => "НЕ",
                LogicType.XOR => "Исключающее ИЛИ",
                LogicType.NAND => "И-НЕ",
                LogicType.NOR => "ИЛИ-НЕ",
                LogicType.Threshold => "Порог",
                LogicType.Timer => "Таймер",
                _ => logicType.ToString()
            };
        }

        /// <summary>
        /// Получить стоимость контроллера
        /// </summary>
        public int GetPartCost()
        {
            return controllerType switch
            {
                ControllerType.Basic => 300,
                ControllerType.Advanced => 600,
                ControllerType.Programmable => 1000,
                ControllerType.Logic => 500,
                _ => 300
            };
        }

        /// <summary>
        /// Получить вес контроллера
        /// </summary>
        public float GetPartWeight()
        {
            return controllerType switch
            {
                ControllerType.Basic => 5f,
                ControllerType.Advanced => 8f,
                ControllerType.Programmable => 12f,
                ControllerType.Logic => 7f,
                _ => 5f
            };
        }

        private void Update()
        {
            if (!isInitialized || !controllerActive) return;

            // Обновление контроллера с заданной частотой
            if (Time.time - lastProcessTime >= 1f / controllerProcessingSpeed)
            {
                ProcessInputs();
                ProcessLogic();
                ProcessOutputs();
                lastProcessTime = Time.time;
            }

            // Обновление визуальных индикаторов
            UpdateVisualIndicators();

            // Обновление системы частиц
            if (controllerParticles != null)
            {
                var emission = controllerParticles.emission;
                if (controllerActive)
                {
                    emission.rateOverTime = 10f;
                }
                else
                {
                    emission.rateOverTime = 0f;
                }
            }
        }

        /// <summary>
        /// Обработка входов
        /// </summary>
        private void ProcessInputs()
        {
            // Сброс входов
            for (int i = 0; i < controllerInputs; i++)
            {
                inputStates[i] = false;
                inputValues[i] = 0f;
            }

            // Чтение данных с датчиков
            for (int i = 0; i < connectedSensors.Count && i < controllerInputs; i++)
            {
                Sensor sensor = connectedSensors[i];
                if (sensor != null && sensor.sensorActive)
                {
                    inputValues[i] = sensor.GetNormalizedValue();
                    inputStates[i] = sensor.IsTriggered();
                }
            }
        }

        /// <summary>
        /// Обработка логики
        /// </summary>
        private void ProcessLogic()
        {
            bool result = false;
            float outputValue = 0f;

            switch (logicType)
            {
                case LogicType.AND:
                    result = true;
                    for (int i = 0; i < controllerInputs; i++)
                    {
                        if (!inputStates[i])
                        {
                            result = false;
                            break;
                        }
                    }
                    break;

                case LogicType.OR:
                    result = false;
                    for (int i = 0; i < controllerInputs; i++)
                    {
                        if (inputStates[i])
                        {
                            result = true;
                            break;
                        }
                    }
                    break;

                case LogicType.NOT:
                    result = !inputStates[0];
                    break;

                case LogicType.XOR:
                    int trueCount = 0;
                    for (int i = 0; i < controllerInputs; i++)
                    {
                        if (inputStates[i]) trueCount++;
                    }
                    result = trueCount == 1;
                    break;

                case LogicType.NAND:
                    result = true;
                    for (int i = 0; i < controllerInputs; i++)
                    {
                        if (!inputStates[i])
                        {
                            result = false;
                            break;
                        }
                    }
                    result = !result;
                    break;

                case LogicType.NOR:
                    result = true;
                    for (int i = 0; i < controllerInputs; i++)
                    {
                        if (inputStates[i])
                        {
                            result = false;
                            break;
                        }
                    }
                    break;

                case LogicType.Threshold:
                    float sum = 0f;
                    for (int i = 0; i < controllerInputs; i++)
                    {
                        sum += inputValues[i];
                    }
                    float average = sum / controllerInputs;
                    result = average >= thresholdValue;
                    outputValue = average;
                    break;

                case LogicType.Timer:
                    // Простая реализация таймера
                    if (inputStates[0])
                    {
                        outputValue = Mathf.PingPong(Time.time, delayTime) / delayTime;
                        result = outputValue > 0.5f;
                    }
                    else
                    {
                        outputValue = 0f;
                        result = false;
                    }
                    break;
            }

            // Применение инверсии
            if (invertOutput)
            {
                result = !result;
                outputValue = 1f - outputValue;
            }

            // Установка выходов
            for (int i = 0; i < controllerOutputs; i++)
            {
                outputStates[i] = result;
                outputValues[i] = outputValue;
            }
        }

        /// <summary>
        /// Обработка выходов
        /// </summary>
        private void ProcessOutputs()
        {
            // Управление подключенными устройствами
            for (int i = 0; i < controlledDevices.Count && i < controllerOutputs; i++)
            {
                PartBase device = controlledDevices[i];
                if (device != null)
                {
                    // Пример управления различными типами устройств
                    if (device is Motor motor)
                    {
                        if (outputStates[i])
                        {
                            motor.SetMotorPower(outputValues[i]);
                        }
                        else
                        {
                            motor.SetMotorPower(0f);
                        }
                    }
                    else if (device is PneumaticCylinder cylinder)
                    {
                        if (outputStates[i])
                        {
                            cylinder.ExtendCylinder();
                        }
                        else
                        {
                            cylinder.RetractCylinder();
                        }
                    }
                    // Добавить другие типы устройств по необходимости
                }
            }
        }

        /// <summary>
        /// Обновление визуальных индикаторов
        /// </summary>
        private void UpdateVisualIndicators()
        {
            // Обновление индикаторов входов
            for (int i = 0; i < inputLights.Length && i < controllerInputs; i++)
            {
                if (inputLights[i] != null)
                {
                    Renderer renderer = inputLights[i].GetComponent<Renderer>();
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

            // Обновление индикаторов выходов
            for (int i = 0; i < outputLights.Length && i < controllerOutputs; i++)
            {
                if (outputLights[i] != null)
                {
                    Renderer renderer = outputLights[i].GetComponent<Renderer>();
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
        }

        private void OnDrawGizmosSelected()
        {
            if (!controllerActive) return;

            // Отображение подключений в редакторе
            Gizmos.color = Color.yellow;
            
            // Подключения к датчикам
            foreach (Sensor sensor in connectedSensors)
            {
                if (sensor != null)
                {
                    Gizmos.DrawLine(transform.position, sensor.transform.position);
                }
            }

            // Подключения к устройствам
            foreach (PartBase device in controlledDevices)
            {
                if (device != null)
                {
                    Gizmos.DrawLine(transform.position, device.transform.position);
                }
            }
        }
    }
}
