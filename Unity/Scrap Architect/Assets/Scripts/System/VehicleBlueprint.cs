using UnityEngine;
using System.Collections.Generic;
using System;
using ScrapArchitect.Physics;
using ScrapArchitect.Parts;

namespace ScrapArchitect.System
{
    /// <summary>
    /// Структура данных для сохранения информации о детали
    /// </summary>
    [Serializable]
    public class PartData
    {
        public string partID;
        public string partName;
        public string partType;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public float health;
        public float mass;
        public Dictionary<string, object> customProperties;
        
        public PartData()
        {
            customProperties = new Dictionary<string, object>();
        }
    }
    
    /// <summary>
    /// Структура данных для сохранения соединений между деталями
    /// </summary>
    [Serializable]
    public class ConnectionData
    {
        public int part1Index;
        public int part2Index;
        public string connectionType;
        public Vector3 connectionPoint1;
        public Vector3 connectionPoint2;
        public Dictionary<string, object> jointProperties;
        
        public ConnectionData()
        {
            jointProperties = new Dictionary<string, object>();
        }
    }
    
    /// <summary>
    /// Основной класс для сохранения и загрузки конструкций
    /// </summary>
    [Serializable]
    public class VehicleBlueprint
    {
        [Header("Blueprint Info")]
        public string blueprintName = "New Vehicle";
        public string description = "";
        public string author = "";
        public DateTime creationDate;
        public DateTime lastModified;
        public string version = "1.0";
        
        [Header("Vehicle Data")]
        public List<PartData> parts = new List<PartData>();
        public List<ConnectionData> connections = new List<ConnectionData>();
        public Vector3 centerOfMass;
        public float totalMass;
        public int totalParts;
        
        [Header("Metadata")]
        public Dictionary<string, object> metadata = new Dictionary<string, object>();
        public List<string> tags = new List<string>();
        public float rating;
        public int downloadCount;
        
        public VehicleBlueprint()
        {
            creationDate = DateTime.Now;
            lastModified = DateTime.Now;
        }
        
        /// <summary>
        /// Создать blueprint из текущей конструкции
        /// </summary>
        public static VehicleBlueprint CreateFromVehicle(GameObject vehicleRoot)
        {
            VehicleBlueprint blueprint = new VehicleBlueprint();
            blueprint.blueprintName = vehicleRoot.name;
            
            // Получить все детали
            PartController[] partControllers = vehicleRoot.GetComponentsInChildren<PartController>();
            
            foreach (PartController part in partControllers)
            {
                PartData partData = CreatePartData(part);
                blueprint.parts.Add(partData);
            }
            
            // Получить все соединения
            blueprint.connections = GetConnectionsData(partControllers);
            
            // Вычислить общие характеристики
            CalculateVehicleStats(blueprint, partControllers);
            
            blueprint.lastModified = DateTime.Now;
            return blueprint;
        }
        
        /// <summary>
        /// Создать данные детали из PartController
        /// </summary>
        private static PartData CreatePartData(PartController part)
        {
            PartData partData = new PartData();
            
            partData.partID = part.partName;
            partData.partName = part.partName;
            partData.partType = part.partType.ToString();
            partData.position = part.transform.position;
            partData.rotation = part.transform.rotation;
            partData.scale = part.transform.localScale;
            partData.health = part.currentHealth;
            partData.mass = part.mass;
            
            // Сохранить специфичные свойства детали
            SavePartSpecificProperties(part, partData);
            
            return partData;
        }
        
        /// <summary>
        /// Сохранить специфичные свойства детали
        /// </summary>
        private static void SavePartSpecificProperties(PartController part, PartData partData)
        {
            // Сохранить свойства в зависимости от типа детали
            switch (part.partType)
            {
                case PartType.Block:
                    Block block = part.GetComponent<Block>();
                    if (block != null)
                    {
                        partData.customProperties["blockType"] = block.blockType.ToString();
                        partData.customProperties["strength"] = block.strength;
                    }
                    break;
                    
                case PartType.Wheel:
                    Wheel wheel = part.GetComponent<Wheel>();
                    if (wheel != null)
                    {
                        partData.customProperties["wheelType"] = wheel.wheelType.ToString();
                        partData.customProperties["radius"] = wheel.wheelRadius;
                        partData.customProperties["width"] = wheel.wheelWidth;
                    }
                    break;
                    
                case PartType.Motor:
                    Motor motor = part.GetComponent<Motor>();
                    if (motor != null)
                    {
                        partData.customProperties["motorType"] = motor.motorType.ToString();
                        partData.customProperties["power"] = motor.power;
                        partData.customProperties["fuelLevel"] = motor.currentFuel;
                    }
                    break;
                    
                case PartType.Seat:
                    DriverSeat seat = part.GetComponent<DriverSeat>();
                    if (seat != null)
                    {
                        partData.customProperties["seatType"] = seat.seatType.ToString();
                        partData.customProperties["hasSteeringWheel"] = seat.hasSteeringWheel;
                    }
                    break;
                    
                case PartType.Tool:
                    Tool tool = part.GetComponent<Tool>();
                    if (tool != null)
                    {
                        partData.customProperties["toolType"] = tool.toolType.ToString();
                        partData.customProperties["power"] = tool.power;
                    }
                    break;
            }
            
            // Сохранить snap-points
            SnapPoint[] snapPoints = part.GetSnapPoints();
            if (snapPoints.Length > 0)
            {
                List<Dictionary<string, object>> snapPointsData = new List<Dictionary<string, object>>();
                
                foreach (SnapPoint snapPoint in snapPoints)
                {
                    Dictionary<string, object> snapData = new Dictionary<string, object>
                    {
                        ["type"] = snapPoint.snapType.ToString(),
                        ["direction"] = snapPoint.GetSnapDirection(),
                        ["radius"] = snapPoint.GetSnapRadius(),
                        ["isOccupied"] = snapPoint.IsOccupied()
                    };
                    
                    snapPointsData.Add(snapData);
                }
                
                partData.customProperties["snapPoints"] = snapPointsData;
            }
        }
        
        /// <summary>
        /// Получить данные соединений
        /// </summary>
        private static List<ConnectionData> GetConnectionsData(PartController[] parts)
        {
            List<ConnectionData> connections = new List<ConnectionData>();
            
            for (int i = 0; i < parts.Length; i++)
            {
                PartController part = parts[i];
                SnapPoint[] snapPoints = part.GetSnapPoints();
                
                foreach (SnapPoint snapPoint in snapPoints)
                {
                    if (snapPoint.IsOccupied())
                    {
                        SnapPoint connectedSnapPoint = snapPoint.GetConnectedSnapPoint();
                        if (connectedSnapPoint != null)
                        {
                            PartController connectedPart = connectedSnapPoint.GetParentPart();
                            int connectedPartIndex = Array.IndexOf(parts, connectedPart);
                            
                            if (connectedPartIndex > i) // Избежать дублирования
                            {
                                ConnectionData connectionData = new ConnectionData
                                {
                                    part1Index = i,
                                    part2Index = connectedPartIndex,
                                    connectionType = "SnapPoint",
                                    connectionPoint1 = snapPoint.GetSnapPosition(),
                                    connectionPoint2 = connectedSnapPoint.GetSnapPosition()
                                };
                                
                                connections.Add(connectionData);
                            }
                        }
                    }
                }
            }
            
            return connections;
        }
        
        /// <summary>
        /// Вычислить общие характеристики машины
        /// </summary>
        private static void CalculateVehicleStats(VehicleBlueprint blueprint, PartController[] parts)
        {
            blueprint.totalParts = parts.Length;
            blueprint.totalMass = 0f;
            Vector3 centerOfMass = Vector3.zero;
            
            foreach (PartController part in parts)
            {
                blueprint.totalMass += part.mass;
                centerOfMass += part.transform.position * part.mass;
            }
            
            if (blueprint.totalMass > 0)
            {
                blueprint.centerOfMass = centerOfMass / blueprint.totalMass;
            }
        }
        
        /// <summary>
        /// Воссоздать машину из blueprint
        /// </summary>
        public GameObject CreateVehicle(Vector3 position, Quaternion rotation)
        {
            GameObject vehicleRoot = new GameObject(blueprintName);
            vehicleRoot.transform.position = position;
            vehicleRoot.transform.rotation = rotation;
            
            // Создать все детали
            List<PartController> createdParts = new List<PartController>();
            
            foreach (PartData partData in parts)
            {
                GameObject partObject = CreatePartFromData(partData, vehicleRoot.transform);
                if (partObject != null)
                {
                    PartController partController = partObject.GetComponent<PartController>();
                    if (partController != null)
                    {
                        createdParts.Add(partController);
                    }
                }
            }
            
            // Восстановить соединения
            RestoreConnections(createdParts);
            
            return vehicleRoot;
        }
        
        /// <summary>
        /// Создать деталь из данных
        /// </summary>
        private GameObject CreatePartFromData(PartData partData, Transform parent)
        {
            // Создать базовый объект
            GameObject partObject = new GameObject(partData.partName);
            partObject.transform.SetParent(parent);
            partObject.transform.position = partData.position;
            partObject.transform.rotation = partData.rotation;
            partObject.transform.localScale = partData.scale;
            
            // Добавить базовые компоненты
            Rigidbody rb = partObject.AddComponent<Rigidbody>();
            BoxCollider col = partObject.AddComponent<BoxCollider>();
            MeshRenderer renderer = partObject.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = partObject.AddComponent<MeshFilter>();
            
            // Создать базовую геометрию
            CreateBasicGeometry(partObject, partData.partType);
            
            // Добавить PartController
            PartController partController = partObject.AddComponent<PartController>();
            partController.partName = partData.partName;
            partController.partType = ParsePartType(partData.partType);
            partController.mass = partData.mass;
            partController.currentHealth = partData.health;
            
            // Восстановить специфичные свойства
            RestorePartSpecificProperties(partObject, partData);
            
            return partObject;
        }
        
        /// <summary>
        /// Создать базовую геометрию для детали
        /// </summary>
        private void CreateBasicGeometry(GameObject partObject, string partType)
        {
            MeshFilter meshFilter = partObject.GetComponent<MeshFilter>();
            MeshRenderer renderer = partObject.GetComponent<MeshRenderer>();
            
            // Создать базовый куб
            Mesh mesh = new Mesh();
            mesh.vertices = new Vector3[]
            {
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f)
            };
            
            mesh.triangles = new int[]
            {
                0, 2, 1, 0, 3, 2, // Front
                1, 6, 5, 1, 2, 6, // Right
                5, 7, 4, 5, 6, 7, // Back
                4, 3, 0, 4, 7, 3, // Left
                3, 6, 2, 3, 7, 6, // Top
                0, 5, 4, 0, 1, 5  // Bottom
            };
            
            mesh.RecalculateNormals();
            meshFilter.mesh = mesh;
            
            // Создать материал
            Material material = new Material(Shader.Find("Standard"));
            material.color = GetColorForPartType(partType);
            renderer.material = material;
        }
        
        /// <summary>
        /// Получить цвет для типа детали
        /// </summary>
        private Color GetColorForPartType(string partType)
        {
            switch (partType.ToLower())
            {
                case "block": return Color.gray;
                case "wheel": return Color.black;
                case "motor": return Color.red;
                case "seat": return Color.blue;
                case "tool": return Color.yellow;
                case "connection": return Color.green;
                default: return Color.white;
            }
        }
        
        /// <summary>
        /// Парсить тип детали из строки
        /// </summary>
        private PartType ParsePartType(string partTypeString)
        {
            if (Enum.TryParse(partTypeString, out PartType partType))
            {
                return partType;
            }
            return PartType.Block;
        }
        
        /// <summary>
        /// Восстановить специфичные свойства детали
        /// </summary>
        private void RestorePartSpecificProperties(GameObject partObject, PartData partData)
        {
            PartController partController = partObject.GetComponent<PartController>();
            
            // Восстановить snap-points
            if (partData.customProperties.ContainsKey("snapPoints"))
            {
                List<Dictionary<string, object>> snapPointsData = 
                    partData.customProperties["snapPoints"] as List<Dictionary<string, object>>;
                
                if (snapPointsData != null)
                {
                    foreach (Dictionary<string, object> snapData in snapPointsData)
                    {
                        GameObject snapPointObj = new GameObject("SnapPoint");
                        snapPointObj.transform.SetParent(partObject.transform);
                        
                        SnapPoint snapPoint = snapPointObj.AddComponent<SnapPoint>();
                        
                        if (snapData.ContainsKey("type"))
                        {
                            string typeString = snapData["type"].ToString();
                            if (Enum.TryParse(typeString, out SnapPoint.SnapPointType snapType))
                            {
                                snapPoint.snapType = snapType;
                            }
                        }
                        
                        if (snapData.ContainsKey("direction"))
                        {
                            snapPoint.snapDirection = (Vector3)snapData["direction"];
                        }
                        
                        if (snapData.ContainsKey("radius"))
                        {
                            snapPoint.snapRadius = Convert.ToSingle(snapData["radius"]);
                        }
                    }
                }
            }
            
            // Восстановить специфичные компоненты
            switch (partController.partType)
            {
                case PartType.Block:
                    RestoreBlockProperties(partObject, partData);
                    break;
                case PartType.Wheel:
                    RestoreWheelProperties(partObject, partData);
                    break;
                case PartType.Motor:
                    RestoreMotorProperties(partObject, partData);
                    break;
                case PartType.Seat:
                    RestoreSeatProperties(partObject, partData);
                    break;
                case PartType.Tool:
                    RestoreToolProperties(partObject, partData);
                    break;
            }
        }
        
        /// <summary>
        /// Восстановить свойства блока
        /// </summary>
        private void RestoreBlockProperties(GameObject partObject, PartData partData)
        {
            Block block = partObject.AddComponent<Block>();
            
            if (partData.customProperties.ContainsKey("blockType"))
            {
                string blockTypeString = partData.customProperties["blockType"].ToString();
                if (Enum.TryParse(blockTypeString, out BlockType blockType))
                {
                    block.blockType = blockType;
                }
            }
        }
        
        /// <summary>
        /// Восстановить свойства колеса
        /// </summary>
        private void RestoreWheelProperties(GameObject partObject, PartData partData)
        {
            Wheel wheel = partObject.AddComponent<Wheel>();
            
            if (partData.customProperties.ContainsKey("wheelType"))
            {
                string wheelTypeString = partData.customProperties["wheelType"].ToString();
                if (Enum.TryParse(wheelTypeString, out WheelType wheelType))
                {
                    wheel.wheelType = wheelType;
                }
            }
        }
        
        /// <summary>
        /// Восстановить свойства двигателя
        /// </summary>
        private void RestoreMotorProperties(GameObject partObject, PartData partData)
        {
            Motor motor = partObject.AddComponent<Motor>();
            
            if (partData.customProperties.ContainsKey("motorType"))
            {
                string motorTypeString = partData.customProperties["motorType"].ToString();
                if (Enum.TryParse(motorTypeString, out MotorType motorType))
                {
                    motor.motorType = motorType;
                }
            }
        }
        
        /// <summary>
        /// Восстановить свойства сиденья
        /// </summary>
        private void RestoreSeatProperties(GameObject partObject, PartData partData)
        {
            DriverSeat seat = partObject.AddComponent<DriverSeat>();
            
            if (partData.customProperties.ContainsKey("seatType"))
            {
                string seatTypeString = partData.customProperties["seatType"].ToString();
                if (Enum.TryParse(seatTypeString, out SeatType seatType))
                {
                    seat.seatType = seatType;
                }
            }
        }
        
        /// <summary>
        /// Восстановить свойства инструмента
        /// </summary>
        private void RestoreToolProperties(GameObject partObject, PartData partData)
        {
            Tool tool = partObject.AddComponent<Tool>();
            
            if (partData.customProperties.ContainsKey("toolType"))
            {
                string toolTypeString = partData.customProperties["toolType"].ToString();
                if (Enum.TryParse(toolTypeString, out ToolType toolType))
                {
                    tool.toolType = toolType;
                }
            }
        }
        
        /// <summary>
        /// Восстановить соединения между деталями
        /// </summary>
        private void RestoreConnections(List<PartController> createdParts)
        {
            foreach (ConnectionData connectionData in connections)
            {
                if (connectionData.part1Index < createdParts.Count && 
                    connectionData.part2Index < createdParts.Count)
                {
                    PartController part1 = createdParts[connectionData.part1Index];
                    PartController part2 = createdParts[connectionData.part2Index];
                    
                    // Восстановить соединение через snap-points
                    RestoreSnapPointConnection(part1, part2, connectionData);
                }
            }
        }
        
        /// <summary>
        /// Восстановить соединение через snap-points
        /// </summary>
        private void RestoreSnapPointConnection(PartController part1, PartController part2, ConnectionData connectionData)
        {
            SnapPoint[] snapPoints1 = part1.GetSnapPoints();
            SnapPoint[] snapPoints2 = part2.GetSnapPoints();
            
            // Найти ближайшие snap-points
            SnapPoint bestSnap1 = null;
            SnapPoint bestSnap2 = null;
            float bestDistance = float.MaxValue;
            
            foreach (SnapPoint snap1 in snapPoints1)
            {
                foreach (SnapPoint snap2 in snapPoints2)
                {
                    float distance = Vector3.Distance(snap1.GetSnapPosition(), snap2.GetSnapPosition());
                    if (distance < bestDistance && snap1.CanSnapTo(snap2))
                    {
                        bestDistance = distance;
                        bestSnap1 = snap1;
                        bestSnap2 = snap2;
                    }
                }
            }
            
            // Выполнить соединение
            if (bestSnap1 != null && bestSnap2 != null)
            {
                bestSnap1.TrySnapTo(bestSnap2);
            }
        }
        
        /// <summary>
        /// Получить информацию о blueprint
        /// </summary>
        public string GetBlueprintInfo()
        {
            return $"Name: {blueprintName}\n" +
                   $"Author: {author}\n" +
                   $"Parts: {totalParts}\n" +
                   $"Mass: {totalMass:F1}\n" +
                   $"Connections: {connections.Count}\n" +
                   $"Created: {creationDate:yyyy-MM-dd HH:mm}\n" +
                   $"Modified: {lastModified:yyyy-MM-dd HH:mm}";
        }
        
        /// <summary>
        /// Проверить валидность blueprint
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(blueprintName) && 
                   parts.Count > 0 && 
                   !string.IsNullOrEmpty(version);
        }
    }
}
