using UnityEngine;

namespace ScrapArchitect.Parts
{
    /// <summary>
    /// Колесо - движущая сила транспортных средств
    /// </summary>
    public class Wheel : PartBase
    {
        [Header("Wheel Settings")]
        public WheelType wheelType = WheelType.Small;
        public float torque = 100f;
        public float maxSpeed = 10f;
        public bool isMotorized = false;
        public bool isSteering = false;
        
        [Header("Wheel Physics")]
        public float wheelRadius = 0.5f;
        public float wheelWidth = 0.2f;
        public float friction = 1f;
        public float suspensionHeight = 0.3f;
        
        [Header("Wheel Visual")]
        public Color tireColor = Color.black;
        public Color rimColor = Color.gray;
        
        // Компоненты колеса
        private WheelCollider wheelCollider;
        private GameObject wheelMesh;
        
        private void Start()
        {
            InitializeWheel();
        }
        
        /// <summary>
        /// Инициализация колеса
        /// </summary>
        private void InitializeWheel()
        {
            // Устанавливаем тип детали
            partType = PartType.Wheel;
            
            // Настраиваем параметры в зависимости от типа колеса
            SetupWheelProperties();
            
            // Создаем визуальную модель колеса
            CreateWheelMesh();
            
            // Создаем WheelCollider для физики
            CreateWheelCollider();
            
            Debug.Log($"Wheel {partName} ({wheelType}) initialized");
        }
        
        /// <summary>
        /// Настройка свойств колеса в зависимости от типа
        /// </summary>
        private void SetupWheelProperties()
        {
            switch (wheelType)
            {
                case WheelType.Small:
                    mass = 1f;
                    maxHealth = 40f;
                    torque = 50f;
                    maxSpeed = 8f;
                    wheelRadius = 0.3f;
                    cost = 8f;
                    unlockLevel = 1;
                    break;
                    
                case WheelType.Medium:
                    mass = 2f;
                    maxHealth = 60f;
                    torque = 100f;
                    maxSpeed = 12f;
                    wheelRadius = 0.5f;
                    cost = 15f;
                    unlockLevel = 2;
                    break;
                    
                case WheelType.Large:
                    mass = 4f;
                    maxHealth = 80f;
                    torque = 200f;
                    maxSpeed = 15f;
                    wheelRadius = 0.8f;
                    cost = 25f;
                    unlockLevel = 3;
                    break;
                    
                case WheelType.OffRoad:
                    mass = 3f;
                    maxHealth = 70f;
                    torque = 150f;
                    maxSpeed = 10f;
                    wheelRadius = 0.6f;
                    friction = 1.5f;
                    cost = 20f;
                    unlockLevel = 2;
                    break;
            }
            
            // Обновляем текущее здоровье
            currentHealth = maxHealth;
        }
        
        /// <summary>
        /// Создание визуальной модели колеса
        /// </summary>
        private void CreateWheelMesh()
        {
            // Создаем дочерний объект для меша колеса
            wheelMesh = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            wheelMesh.name = "WheelMesh";
            wheelMesh.transform.SetParent(transform);
            wheelMesh.transform.localPosition = Vector3.zero;
            wheelMesh.transform.localRotation = Quaternion.Euler(90, 0, 0);
            wheelMesh.transform.localScale = new Vector3(wheelRadius * 2, wheelWidth, wheelRadius * 2);
            
            // Настраиваем материал колеса
            Renderer wheelRenderer = wheelMesh.GetComponent<Renderer>();
            if (wheelRenderer != null)
            {
                Material wheelMaterial = new Material(Shader.Find("Standard"));
                wheelMaterial.color = tireColor;
                wheelRenderer.material = wheelMaterial;
            }
            
            // Удаляем коллайдер с меша (физика будет через WheelCollider)
            DestroyImmediate(wheelMesh.GetComponent<Collider>());
        }
        
        /// <summary>
        /// Создание WheelCollider для физики колеса
        /// </summary>
        private void CreateWheelCollider()
        {
            wheelCollider = gameObject.AddComponent<WheelCollider>();
            
            // Настройка WheelCollider
            wheelCollider.radius = wheelRadius;
            wheelCollider.wheelDampingRate = 1f;
            wheelCollider.suspensionDistance = suspensionHeight;
            wheelCollider.forceAppPointDistance = 0f;
            
            // Настройка подвески
            JointSpring suspension = wheelCollider.suspensionSpring;
            suspension.spring = 35000f;
            suspension.damper = 4500f;
            suspension.targetPosition = 0.5f;
            wheelCollider.suspensionSpring = suspension;
            
            // Настройка трения
            WheelFrictionCurve forwardFriction = wheelCollider.forwardFriction;
            forwardFriction.extremumSlip = 2f;
            forwardFriction.extremumValue = 1f;
            forwardFriction.asymptoteSlip = 4f;
            forwardFriction.asymptoteValue = 0.5f;
            forwardFriction.stiffness = 1f;
            wheelCollider.forwardFriction = forwardFriction;
            
            WheelFrictionCurve sidewaysFriction = wheelCollider.sidewaysFriction;
            sidewaysFriction.extremumSlip = 2f;
            sidewaysFriction.extremumValue = 1f;
            sidewaysFriction.asymptoteSlip = 4f;
            sidewaysFriction.asymptoteValue = 0.5f;
            sidewaysFriction.stiffness = 1f;
            wheelCollider.sidewaysFriction = sidewaysFriction;
        }
        
        /// <summary>
        /// Специфичная логика колеса при соединении
        /// </summary>
        protected override void OnPartSpecificAction()
        {
            // При соединении колеса проверяем, можно ли его использовать
            if (isMotorized)
            {
                Debug.Log($"Motorized wheel {partName} connected - ready for movement");
            }
        }
        
        /// <summary>
        /// Применение крутящего момента к колесу
        /// </summary>
        public void ApplyTorque(float motorTorque)
        {
            if (wheelCollider != null && isMotorized)
            {
                // Ограничиваем крутящий момент
                float limitedTorque = Mathf.Clamp(motorTorque, -torque, torque);
                wheelCollider.motorTorque = limitedTorque;
            }
        }
        
        /// <summary>
        /// Установка угла поворота для рулевых колес
        /// </summary>
        public void SetSteeringAngle(float angle)
        {
            if (wheelCollider != null && isSteering)
            {
                wheelCollider.steerAngle = angle;
            }
        }
        
        /// <summary>
        /// Получение скорости вращения колеса
        /// </summary>
        public float GetWheelRPM()
        {
            if (wheelCollider != null)
            {
                return wheelCollider.rpm;
            }
            return 0f;
        }
        
        /// <summary>
        /// Получение информации о колесе
        /// </summary>
        public override string GetPartInfo()
        {
            string baseInfo = base.GetPartInfo();
            string wheelInfo = $"\nWheel Type: {wheelType}\nTorque: {torque}\nMax Speed: {maxSpeed}\nMotorized: {isMotorized}\nSteering: {isSteering}";
            
            return baseInfo + wheelInfo;
        }
        
        /// <summary>
        /// Получение урона с учетом типа колеса
        /// </summary>
        public override void TakeDamage(float damage)
        {
            // Колеса более уязвимы к урону
            float actualDamage = damage * 1.2f;
            
            switch (wheelType)
            {
                case WheelType.OffRoad:
                    actualDamage *= 0.8f; // Внедорожные колеса более устойчивы
                    break;
                case WheelType.Large:
                    actualDamage *= 0.9f; // Большие колеса немного устойчивее
                    break;
            }
            
            base.TakeDamage(actualDamage);
        }
        
        /// <summary>
        /// Обновление визуальной модели колеса
        /// </summary>
        private void Update()
        {
            if (wheelCollider != null && wheelMesh != null)
            {
                // Обновляем позицию и вращение визуальной модели
                Vector3 position;
                Quaternion rotation;
                wheelCollider.GetWorldPose(out position, out rotation);
                
                wheelMesh.transform.position = position;
                wheelMesh.transform.rotation = rotation;
            }
        }
    }
    
    /// <summary>
    /// Типы колес
    /// </summary>
    public enum WheelType
    {
        Small,     // Маленькое колесо - маневренное, слабое
        Medium,    // Среднее колесо - сбалансированное
        Large,     // Большое колесо - мощное, медленное
        OffRoad    // Внедорожное колесо - проходимое, универсальное
    }
}
