using UnityEngine;
using ScrapArchitect.Physics;

namespace ScrapArchitect.Parts
{
    public class Connection : PartBase
    {
        [Header("Connection Settings")]
        public ConnectionType connectionType = ConnectionType.Fixed;
        public float maxForce = 1000f;
        public float maxTorque = 500f;
        public bool canBreak = true;
        public float breakForce = 2000f;
        public float breakTorque = 1000f;
        
        [Header("Joint Settings")]
        public Vector3 axis = Vector3.up;
        public float springForce = 100f;
        public float damperForce = 10f;
        public float targetPosition = 0f;
        public float targetVelocity = 0f;
        
        [Header("Visual Settings")]
        public Material normalMaterial;
        public Material stressedMaterial;
        public Material brokenMaterial;
        public GameObject stressEffect;
        public GameObject breakEffect;
        
        private Joint joint;
        private bool isStressed = false;
        private bool isBroken = false;
        private float currentForce = 0f;
        private float currentTorque = 0f;
        
        public enum ConnectionType
        {
            Fixed,      // Жесткое соединение
            Hinge,      // Шарнир
            Spring,     // Пружина
            Slider,     // Ползунок
            Configurable // Настраиваемое
        }
        
        protected override void InitializePart()
        {
            base.InitializePart();
            
            // Настройка параметров детали
            partID = "connection_" + connectionType.ToString().ToLower();
            partName = GetConnectionName(connectionType);
            description = GetConnectionDescription(connectionType);
            unlockLevel = GetUnlockLevel(connectionType);
            cost = GetCost(connectionType);
            
            // Настройка физических свойств
            mass = GetMass(connectionType);
            health = GetHealth(connectionType);
            strength = GetStrength(connectionType);
            
            Debug.Log($"Connection initialized: {partName} (Type: {connectionType})");
        }
        
        public override void OnConnect(PartController otherPart)
        {
            base.OnConnect(otherPart);
            CreateJoint(otherPart);
        }
        
        public override void OnDisconnect(PartController otherPart)
        {
            base.OnDisconnect(otherPart);
            DestroyJoint();
        }
        
        private void CreateJoint(PartController otherPart)
        {
            if (joint != null)
            {
                DestroyJoint();
            }
            
            switch (connectionType)
            {
                case ConnectionType.Fixed:
                    CreateFixedJoint(otherPart);
                    break;
                case ConnectionType.Hinge:
                    CreateHingeJoint(otherPart);
                    break;
                case ConnectionType.Spring:
                    CreateSpringJoint(otherPart);
                    break;
                case ConnectionType.Slider:
                    CreateSliderJoint(otherPart);
                    break;
                case ConnectionType.Configurable:
                    CreateConfigurableJoint(otherPart);
                    break;
            }
            
            if (joint != null)
            {
                joint.breakForce = breakForce;
                joint.breakTorque = breakTorque;
                joint.enableCollision = false;
            }
        }
        
        private void CreateFixedJoint(PartController otherPart)
        {
            FixedJoint fixedJoint = gameObject.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = otherPart.GetComponent<Rigidbody>();
            joint = fixedJoint;
        }
        
        private void CreateHingeJoint(PartController otherPart)
        {
            HingeJoint hingeJoint = gameObject.AddComponent<HingeJoint>();
            hingeJoint.connectedBody = otherPart.GetComponent<Rigidbody>();
            hingeJoint.axis = axis;
            
            // Настройка мотора
            JointMotor motor = hingeJoint.motor;
            motor.force = maxForce;
            motor.targetVelocity = targetVelocity;
            motor.freeSpin = false;
            hingeJoint.motor = motor;
            hingeJoint.useMotor = false; // По умолчанию выключен
            
            joint = hingeJoint;
        }
        
        private void CreateSpringJoint(PartController otherPart)
        {
            SpringJoint springJoint = gameObject.AddComponent<SpringJoint>();
            springJoint.connectedBody = otherPart.GetComponent<Rigidbody>();
            springJoint.spring = springForce;
            springJoint.damper = damperForce;
            springJoint.autoConfigureConnectedAnchor = false;
            springJoint.anchor = Vector3.zero;
            springJoint.connectedAnchor = Vector3.zero;
            
            joint = springJoint;
        }
        
        private void CreateSliderJoint(PartController otherPart)
        {
            ConfigurableJoint configJoint = gameObject.AddComponent<ConfigurableJoint>();
            configJoint.connectedBody = otherPart.GetComponent<Rigidbody>();
            
            // Настройка как слайдер
            configJoint.xMotion = ConfigurableJointMotion.Limited;
            configJoint.yMotion = ConfigurableJointMotion.Locked;
            configJoint.zMotion = ConfigurableJointMotion.Locked;
            
            SoftJointLimit limit = new SoftJointLimit();
            limit.limit = 5f;
            configJoint.linearLimit = limit;
            
            joint = configJoint;
        }
        
        private void CreateConfigurableJoint(PartController otherPart)
        {
            ConfigurableJoint configJoint = gameObject.AddComponent<ConfigurableJoint>();
            configJoint.connectedBody = otherPart.GetComponent<Rigidbody>();
            
            // Полностью настраиваемое соединение
            configJoint.xMotion = ConfigurableJointMotion.Limited;
            configJoint.yMotion = ConfigurableJointMotion.Limited;
            configJoint.zMotion = ConfigurableJointMotion.Limited;
            configJoint.angularXMotion = ConfigurableJointMotion.Limited;
            configJoint.angularYMotion = ConfigurableJointMotion.Limited;
            configJoint.angularZMotion = ConfigurableJointMotion.Limited;
            
            joint = configJoint;
        }
        
        private void DestroyJoint()
        {
            if (joint != null)
            {
                DestroyImmediate(joint);
                joint = null;
            }
        }
        
        private void Update()
        {
            if (joint != null && canBreak)
            {
                UpdateJointStress();
            }
        }
        
        private void UpdateJointStress()
        {
            // Проверка нагрузки на соединение
            currentForce = joint.currentForce.magnitude;
            currentTorque = joint.currentTorque.magnitude;
            
            float stressLevel = Mathf.Max(currentForce / maxForce, currentTorque / maxTorque);
            
            if (stressLevel > 0.8f && !isStressed)
            {
                SetStressed(true);
            }
            else if (stressLevel < 0.6f && isStressed)
            {
                SetStressed(false);
            }
            
            if (stressLevel > 1.0f && !isBroken)
            {
                BreakConnection();
            }
        }
        
        private void SetStressed(bool stressed)
        {
            isStressed = stressed;
            
            if (stressed)
            {
                if (stressedMaterial != null)
                {
                    GetComponent<Renderer>().material = stressedMaterial;
                }
                
                if (stressEffect != null)
                {
                    stressEffect.SetActive(true);
                }
                
                // Воспроизвести звук напряжения
                PlaySound("stress");
            }
            else
            {
                if (normalMaterial != null)
                {
                    GetComponent<Renderer>().material = normalMaterial;
                }
                
                if (stressEffect != null)
                {
                    stressEffect.SetActive(false);
                }
            }
        }
        
        private void BreakConnection()
        {
            isBroken = true;
            
            if (brokenMaterial != null)
            {
                GetComponent<Renderer>().material = brokenMaterial;
            }
            
            if (breakEffect != null)
            {
                Instantiate(breakEffect, transform.position, transform.rotation);
            }
            
            // Воспроизвести звук поломки
            PlaySound("break");
            
            // Уничтожить соединение
            DestroyJoint();
            
            // Уведомить о поломке
            OnPartDamaged(health);
        }
        
        public void ToggleMotor(bool enabled)
        {
            if (joint is HingeJoint hingeJoint)
            {
                hingeJoint.useMotor = enabled;
            }
        }
        
        public void SetMotorVelocity(float velocity)
        {
            if (joint is HingeJoint hingeJoint)
            {
                JointMotor motor = hingeJoint.motor;
                motor.targetVelocity = velocity;
                hingeJoint.motor = motor;
            }
        }
        
        public float GetCurrentForce()
        {
            return currentForce;
        }
        
        public float GetCurrentTorque()
        {
            return currentTorque;
        }
        
        public float GetStressLevel()
        {
            return Mathf.Max(currentForce / maxForce, currentTorque / maxTorque);
        }
        
        public bool IsStressed()
        {
            return isStressed;
        }
        
        public bool IsBroken()
        {
            return isBroken;
        }
        
        private string GetConnectionName(ConnectionType type)
        {
            switch (type)
            {
                case ConnectionType.Fixed: return "Жесткое соединение";
                case ConnectionType.Hinge: return "Шарнир";
                case ConnectionType.Spring: return "Пружина";
                case ConnectionType.Slider: return "Ползунок";
                case ConnectionType.Configurable: return "Универсальное соединение";
                default: return "Соединение";
            }
        }
        
        private string GetConnectionDescription(ConnectionType type)
        {
            switch (type)
            {
                case ConnectionType.Fixed: return "Жесткое соединение двух деталей";
                case ConnectionType.Hinge: return "Шарнирное соединение с возможностью вращения";
                case ConnectionType.Spring: return "Пружинное соединение с упругостью";
                case ConnectionType.Slider: return "Соединение с возможностью скольжения";
                case ConnectionType.Configurable: return "Универсальное настраиваемое соединение";
                default: return "Базовое соединение";
            }
        }
        
        private int GetUnlockLevel(ConnectionType type)
        {
            switch (type)
            {
                case ConnectionType.Fixed: return 1;
                case ConnectionType.Hinge: return 2;
                case ConnectionType.Spring: return 3;
                case ConnectionType.Slider: return 4;
                case ConnectionType.Configurable: return 5;
                default: return 1;
            }
        }
        
        private int GetCost(ConnectionType type)
        {
            switch (type)
            {
                case ConnectionType.Fixed: return 10;
                case ConnectionType.Hinge: return 25;
                case ConnectionType.Spring: return 40;
                case ConnectionType.Slider: return 60;
                case ConnectionType.Configurable: return 100;
                default: return 10;
            }
        }
        
        private float GetMass(ConnectionType type)
        {
            switch (type)
            {
                case ConnectionType.Fixed: return 0.5f;
                case ConnectionType.Hinge: return 1.0f;
                case ConnectionType.Spring: return 0.8f;
                case ConnectionType.Slider: return 1.2f;
                case ConnectionType.Configurable: return 2.0f;
                default: return 1.0f;
            }
        }
        
        private float GetHealth(ConnectionType type)
        {
            switch (type)
            {
                case ConnectionType.Fixed: return 100f;
                case ConnectionType.Hinge: return 80f;
                case ConnectionType.Spring: return 60f;
                case ConnectionType.Slider: return 90f;
                case ConnectionType.Configurable: return 120f;
                default: return 100f;
            }
        }
        
        private float GetStrength(ConnectionType type)
        {
            switch (type)
            {
                case ConnectionType.Fixed: return 100f;
                case ConnectionType.Hinge: return 80f;
                case ConnectionType.Spring: return 60f;
                case ConnectionType.Slider: return 90f;
                case ConnectionType.Configurable: return 120f;
                default: return 100f;
            }
        }
    }
}
