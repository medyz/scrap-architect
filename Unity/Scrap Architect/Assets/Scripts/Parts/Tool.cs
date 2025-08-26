using UnityEngine;
using ScrapArchitect.Physics;

namespace ScrapArchitect.Parts
{
    public class Tool : PartBase
    {
        [Header("Tool Settings")]
        public ToolType toolType = ToolType.Hammer;
        public float power = 100f;
        public float range = 2f;
        public float cooldown = 1f;
        public bool isActive = false;
        public bool requiresFuel = false;
        public float fuelConsumption = 1f;
        
        [Header("Action Settings")]
        public bool canBreak = true;
        public bool canBuild = false;
        public bool canRepair = false;
        public bool canCollect = false;
        public LayerMask targetLayers = -1;
        
        [Header("Visual Settings")]
        public GameObject toolModel;
        public GameObject activeEffect;
        public GameObject impactEffect;
        public Material normalMaterial;
        public Material activeMaterial;
        public new Material damagedMaterial;
        
        [Header("Audio Settings")]
        public AudioClip activateSound;
        public AudioClip deactivateSound;
        public AudioClip useSound;
        public AudioClip impactSound;
        
        private float lastUseTime = 0f;
        private bool isActivated = false;
        private float currentFuel = 100f;
        private Collider[] nearbyObjects;
        
        public enum ToolType
        {
            Hammer,     // Молоток
            Wrench,     // Гаечный ключ
            Screwdriver, // Отвертка
            Drill,      // Дрель
            Welder,     // Сварка
            Magnet,     // Магнит
            Vacuum,     // Пылесос
            Sprayer     // Краскопульт
        }
        
        protected override void InitializePart()
        {
            base.InitializePart();
            
            // Настройка параметров детали
            partID = "tool_" + toolType.ToString().ToLower();
            partName = GetToolName(toolType);
            description = GetToolDescription(toolType);
            unlockLevel = GetUnlockLevel(toolType);
            cost = GetCost(toolType);
            
            // Настройка физических свойств
            mass = GetMass(toolType);
            currentHealth = GetHealth(toolType);
            maxHealth = GetHealth(toolType);
            
            // Настройка возможностей инструмента
            SetupToolCapabilities();
            
            Debug.Log($"Tool initialized: {partName} (Type: {toolType})");
        }

        /// <summary>
        /// Специфичное действие для инструмента
        /// </summary>
        protected override void OnPartSpecificAction()
        {
            // Логика специфичная для инструмента
            if (isActive)
            {
                // Логика инструмента обрабатывается в методах Activate(), Deactivate() и Use()
                // Дополнительная логика может быть добавлена здесь при необходимости
            }
        }
        
        private void SetupToolCapabilities()
        {
            switch (toolType)
            {
                case ToolType.Hammer:
                    canBreak = true;
                    canBuild = false;
                    canRepair = true;
                    canCollect = false;
                    break;
                    
                case ToolType.Wrench:
                    canBreak = false;
                    canBuild = true;
                    canRepair = true;
                    canCollect = false;
                    break;
                    
                case ToolType.Screwdriver:
                    canBreak = false;
                    canBuild = true;
                    canRepair = true;
                    canCollect = false;
                    break;
                    
                case ToolType.Drill:
                    canBreak = true;
                    canBuild = true;
                    canRepair = false;
                    canCollect = false;
                    break;
                    
                case ToolType.Welder:
                    canBreak = false;
                    canBuild = true;
                    canRepair = true;
                    canCollect = false;
                    break;
                    
                case ToolType.Magnet:
                    canBreak = false;
                    canBuild = false;
                    canRepair = false;
                    canCollect = true;
                    break;
                    
                case ToolType.Vacuum:
                    canBreak = false;
                    canBuild = false;
                    canRepair = false;
                    canCollect = true;
                    break;
                    
                case ToolType.Sprayer:
                    canBreak = false;
                    canBuild = false;
                    canRepair = true;
                    canCollect = false;
                    break;
            }
        }
        
        public void Activate()
        {
            if (!isActive || (requiresFuel && currentFuel <= 0))
            {
                return;
            }
            
            isActivated = true;
            
            // Визуальные эффекты
            if (activeEffect != null)
            {
                activeEffect.SetActive(true);
            }
            
            if (activeMaterial != null)
            {
                GetComponent<Renderer>().material = activeMaterial;
            }
            
            // Звуковой эффект
            PlaySound("activate");
            
            Debug.Log($"Tool activated: {partName}");
        }
        
        public void Deactivate()
        {
            if (!isActivated)
            {
                return;
            }
            
            isActivated = false;
            
            // Визуальные эффекты
            if (activeEffect != null)
            {
                activeEffect.SetActive(false);
            }
            
            if (normalMaterial != null)
            {
                GetComponent<Renderer>().material = normalMaterial;
            }
            
            // Звуковой эффект
            PlaySound("deactivate");
            
            Debug.Log($"Tool deactivated: {partName}");
        }
        
        public void Use(Vector3 targetPosition)
        {
            if (!isActivated || Time.time - lastUseTime < cooldown)
            {
                return;
            }
            
            if (requiresFuel && currentFuel <= 0)
            {
                Deactivate();
                return;
            }
            
            lastUseTime = Time.time;
            
            // Потребление топлива
            if (requiresFuel)
            {
                currentFuel -= fuelConsumption;
            }
            
            // Поиск объектов в радиусе действия
            nearbyObjects = Physics.OverlapSphere(targetPosition, range, targetLayers);
            
            foreach (Collider obj in nearbyObjects)
            {
                ProcessTarget(obj.gameObject, targetPosition);
            }
            
            // Визуальные эффекты использования
            if (impactEffect != null)
            {
                Instantiate(impactEffect, targetPosition, Quaternion.identity);
            }
            
            // Звуковой эффект
            PlaySound("use");
            
            Debug.Log($"Tool used: {partName} at {targetPosition}");
        }
        
        private void ProcessTarget(GameObject target, Vector3 position)
        {
            PartController partController = target.GetComponent<PartController>();
            
            if (partController != null)
            {
                if (canBreak)
                {
                    // Ломать детали
                    partController.TakeDamage(power);
                }
                
                if (canBuild)
                {
                    // Строить/соединять детали
                    // Логика строительства будет реализована позже
                }
                
                if (canRepair)
                {
                    // Ремонтировать детали
                    partController.Repair(power);
                }
                
                if (canCollect)
                {
                    // Собирать объекты
                    CollectObject(target);
                }
            }
            else
            {
                // Обработка обычных объектов
                if (canBreak)
                {
                    Destroy(target);
                }
                
                if (canCollect)
                {
                    CollectObject(target);
                }
            }
        }
        
        private void CollectObject(GameObject obj)
        {
            // Логика сбора объектов
            // Будет интегрирована с системой инвентаря
            
            if (obj.CompareTag("Collectible"))
            {
                // Добавить в инвентарь
                Debug.Log($"Collected object: {obj.name}");
                
                // Воспроизвести звук сбора
                PlaySound("impact");
                
                // Уничтожить объект
                Destroy(obj);
            }
        }
        
        public void Refuel(float amount)
        {
            if (!requiresFuel)
            {
                return;
            }
            
            currentFuel = Mathf.Min(currentFuel + amount, 100f);
            Debug.Log($"Tool refueled: {partName} - {currentFuel}%");
        }
        
        public void Repair(float amount)
        {
            health = Mathf.Min(health + amount, GetHealth(toolType));
            
            if (health >= GetHealth(toolType) * 0.8f && damagedMaterial != null)
            {
                GetComponent<Renderer>().material = normalMaterial;
            }
            
            Debug.Log($"Tool repaired: {partName} - {health}%");
        }
        
        public bool IsActivated()
        {
            return isActivated;
        }
        
        public bool CanUse()
        {
            return isActivated && Time.time - lastUseTime >= cooldown && 
                   (!requiresFuel || currentFuel > 0);
        }
        
        public float GetFuelLevel()
        {
            return requiresFuel ? currentFuel : 100f;
        }
        
        public float GetCooldownProgress()
        {
            if (Time.time - lastUseTime >= cooldown)
            {
                return 1f;
            }
            
            return (Time.time - lastUseTime) / cooldown;
        }
        
        public bool CanBreak()
        {
            return canBreak;
        }
        
        public bool CanBuild()
        {
            return canBuild;
        }
        
        public bool CanRepair()
        {
            return canRepair;
        }
        
        public bool CanCollect()
        {
            return canCollect;
        }
        
        private string GetToolName(ToolType type)
        {
            switch (type)
            {
                case ToolType.Hammer: return "Молоток";
                case ToolType.Wrench: return "Гаечный ключ";
                case ToolType.Screwdriver: return "Отвертка";
                case ToolType.Drill: return "Дрель";
                case ToolType.Welder: return "Сварка";
                case ToolType.Magnet: return "Магнит";
                case ToolType.Vacuum: return "Пылесос";
                case ToolType.Sprayer: return "Краскопульт";
                default: return "Инструмент";
            }
        }
        
        private string GetToolDescription(ToolType type)
        {
            switch (type)
            {
                case ToolType.Hammer: return "Молоток для разрушения и ремонта";
                case ToolType.Wrench: return "Гаечный ключ для сборки и ремонта";
                case ToolType.Screwdriver: return "Отвертка для точной работы";
                case ToolType.Drill: return "Дрель для сверления отверстий";
                case ToolType.Welder: return "Сварка для соединения металла";
                case ToolType.Magnet: return "Магнит для сбора металлических предметов";
                case ToolType.Vacuum: return "Пылесос для сбора мусора";
                case ToolType.Sprayer: return "Краскопульт для покраски";
                default: return "Базовый инструмент";
            }
        }
        
        private int GetUnlockLevel(ToolType type)
        {
            switch (type)
            {
                case ToolType.Hammer: return 1;
                case ToolType.Wrench: return 1;
                case ToolType.Screwdriver: return 2;
                case ToolType.Drill: return 3;
                case ToolType.Welder: return 4;
                case ToolType.Magnet: return 2;
                case ToolType.Vacuum: return 3;
                case ToolType.Sprayer: return 4;
                default: return 1;
            }
        }
        
        private int GetCost(ToolType type)
        {
            switch (type)
            {
                case ToolType.Hammer: return 20;
                case ToolType.Wrench: return 30;
                case ToolType.Screwdriver: return 25;
                case ToolType.Drill: return 80;
                case ToolType.Welder: return 150;
                case ToolType.Magnet: return 60;
                case ToolType.Vacuum: return 100;
                case ToolType.Sprayer: return 120;
                default: return 50;
            }
        }
        
        private float GetMass(ToolType type)
        {
            switch (type)
            {
                case ToolType.Hammer: return 2f;
                case ToolType.Wrench: return 1.5f;
                case ToolType.Screwdriver: return 0.5f;
                case ToolType.Drill: return 3f;
                case ToolType.Welder: return 5f;
                case ToolType.Magnet: return 1f;
                case ToolType.Vacuum: return 4f;
                case ToolType.Sprayer: return 2.5f;
                default: return 2f;
            }
        }
        
        private float GetHealth(ToolType type)
        {
            switch (type)
            {
                case ToolType.Hammer: return 150f;
                case ToolType.Wrench: return 100f;
                case ToolType.Screwdriver: return 80f;
                case ToolType.Drill: return 120f;
                case ToolType.Welder: return 200f;
                case ToolType.Magnet: return 90f;
                case ToolType.Vacuum: return 150f;
                case ToolType.Sprayer: return 130f;
                default: return 100f;
            }
        }
        
        private float GetStrength(ToolType type)
        {
            switch (type)
            {
                case ToolType.Hammer: return 120f;
                case ToolType.Wrench: return 80f;
                case ToolType.Screwdriver: return 60f;
                case ToolType.Drill: return 100f;
                case ToolType.Welder: return 150f;
                case ToolType.Magnet: return 70f;
                case ToolType.Vacuum: return 110f;
                case ToolType.Sprayer: return 90f;
                default: return 80f;
            }
        }
    }
}
