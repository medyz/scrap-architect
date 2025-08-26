using UnityEngine;

namespace ScrapArchitect.Parts
{
    /// <summary>
    /// Гусеницы - специальный тип движителей для бездорожья
    /// </summary>
    public class Track : PartBase
    {
        [Header("Track Settings")]
        public TrackType trackType = TrackType.Standard;
        public float trackSpeed = 10f;
        public float trackTorque = 1000f;
        public float trackFriction = 0.8f;
        public float trackGrip = 1.2f;

        [Header("Track Physics")]
        public float trackWidth = 0.3f;
        public float trackLength = 1.0f;
        public float trackHeight = 0.2f;
        public int trackSegments = 8;

        [Header("Track Materials")]
        public Material trackMaterial;
        public Material trackTreadMaterial;

        // Компоненты гусеницы
        private WheelCollider[] trackWheels;
        private Transform[] trackSegments;
        private bool isTrackActive = false;

        public enum TrackType
        {
            Standard,    // Стандартные гусеницы
            Heavy,       // Тяжелые гусеницы
            Light,       // Легкие гусеницы
            OffRoad      // Внедорожные гусеницы
        }

        protected override void InitializePart()
        {
            base.InitializePart();
            
            // Настройка типа гусениц
            ConfigureTrackType();
            
            // Создание компонентов гусеницы
            CreateTrackComponents();
            
            Debug.Log($"Track initialized: {trackType} type");
        }

        /// <summary>
        /// Специфичное действие для гусеницы
        /// </summary>
        protected override void OnPartSpecificAction()
        {
            // Логика специфичная для гусеницы
            if (isTrackActive)
            {
                UpdateTrackMovement();
                UpdateTrackVisuals();
            }
        }

        /// <summary>
        /// Настройка типа гусениц
        /// </summary>
        private void ConfigureTrackType()
        {
            switch (trackType)
            {
                case TrackType.Standard:
                    trackSpeed = 10f;
                    trackTorque = 1000f;
                    trackFriction = 0.8f;
                    trackGrip = 1.2f;
                    break;
                    
                case TrackType.Heavy:
                    trackSpeed = 6f;
                    trackTorque = 2000f;
                    trackFriction = 0.9f;
                    trackGrip = 1.5f;
                    trackWidth = 0.4f;
                    trackLength = 1.2f;
                    break;
                    
                case TrackType.Light:
                    trackSpeed = 15f;
                    trackTorque = 600f;
                    trackFriction = 0.7f;
                    trackGrip = 1.0f;
                    trackWidth = 0.2f;
                    trackLength = 0.8f;
                    break;
                    
                case TrackType.OffRoad:
                    trackSpeed = 8f;
                    trackTorque = 1500f;
                    trackFriction = 0.85f;
                    trackGrip = 1.8f;
                    trackWidth = 0.35f;
                    trackLength = 1.1f;
                    break;
            }
        }

        /// <summary>
        /// Создание компонентов гусеницы
        /// </summary>
        private void CreateTrackComponents()
        {
            // Создание колес гусеницы
            trackWheels = new WheelCollider[trackSegments];
            trackSegments = new Transform[trackSegments];

            for (int i = 0; i < trackSegments; i++)
            {
                // Создание колеса
                GameObject wheelObj = new GameObject($"TrackWheel_{i}");
                wheelObj.transform.SetParent(transform);
                wheelObj.transform.localPosition = new Vector3(0, 0, (i - trackSegments/2f) * trackLength / trackSegments);

                WheelCollider wheel = wheelObj.AddComponent<WheelCollider>();
                wheel.radius = trackHeight / 2f;
                wheel.wheelDampingRate = 1f;
                wheel.suspensionDistance = 0.1f;
                wheel.forceAppPointDistance = 0f;

                // Настройка подвески
                JointSpring suspension = wheel.suspensionSpring;
                suspension.spring = 35000f;
                suspension.damper = 4500f;
                suspension.targetPosition = 0.5f;
                wheel.suspensionSpring = suspension;

                // Настройка трения
                WheelFrictionCurve friction = wheel.forwardFriction;
                friction.extremumSlip = 2f;
                friction.extremumValue = 1f;
                friction.asymptoteSlip = 4f;
                friction.asymptoteValue = 0.5f;
                friction.stiffness = 1f;
                wheel.forwardFriction = friction;
                wheel.sidewaysFriction = friction;

                trackWheels[i] = wheel;

                // Создание визуального сегмента гусеницы
                GameObject segmentObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                segmentObj.name = $"TrackSegment_{i}";
                segmentObj.transform.SetParent(transform);
                segmentObj.transform.localPosition = wheelObj.transform.localPosition;
                segmentObj.transform.localScale = new Vector3(trackWidth, trackHeight, trackLength / trackSegments);

                // Применение материала
                if (trackMaterial != null)
                {
                    segmentObj.GetComponent<Renderer>().material = trackMaterial;
                }

                trackSegments[i] = segmentObj.transform;
            }
        }

        /// <summary>
        /// Активировать гусеницы
        /// </summary>
        public void ActivateTrack()
        {
            isTrackActive = true;
            Debug.Log($"Track activated: {trackType}");
        }

        /// <summary>
        /// Деактивировать гусеницы
        /// </summary>
        public void DeactivateTrack()
        {
            isTrackActive = false;
            
            // Остановить все колеса
            foreach (WheelCollider wheel in trackWheels)
            {
                if (wheel != null)
                {
                    wheel.motorTorque = 0f;
                    wheel.brakeTorque = float.MaxValue;
                }
            }
            
            Debug.Log($"Track deactivated: {trackType}");
        }

        /// <summary>
        /// Применить крутящий момент к гусеницам
        /// </summary>
        public void ApplyTorque(float torque)
        {
            if (!isTrackActive) return;

            float torquePerWheel = torque / trackWheels.Length;
            
            foreach (WheelCollider wheel in trackWheels)
            {
                if (wheel != null)
                {
                    wheel.motorTorque = torquePerWheel * trackTorque;
                }
            }
        }

        /// <summary>
        /// Применить торможение к гусеницам
        /// </summary>
        public void ApplyBrake(float brakeForce)
        {
            foreach (WheelCollider wheel in trackWheels)
            {
                if (wheel != null)
                {
                    wheel.brakeTorque = brakeForce;
                }
            }
        }

        /// <summary>
        /// Получить скорость гусениц
        /// </summary>
        public float GetTrackSpeed()
        {
            if (trackWheels.Length == 0) return 0f;

            float totalSpeed = 0f;
            int activeWheels = 0;

            foreach (WheelCollider wheel in trackWheels)
            {
                if (wheel != null && wheel.isGrounded)
                {
                    totalSpeed += wheel.rpm;
                    activeWheels++;
                }
            }

            return activeWheels > 0 ? totalSpeed / activeWheels : 0f;
        }

        /// <summary>
        /// Получить сцепление гусениц
        /// </summary>
        public float GetTrackGrip()
        {
            return trackGrip;
        }

        /// <summary>
        /// Получить тип гусениц
        /// </summary>
        public TrackType GetTrackType()
        {
            return trackType;
        }

        /// <summary>
        /// Получить информацию о гусеницах
        /// </summary>
        public override string GetPartInfo()
        {
            string info = base.GetPartInfo();
            info += $"\nТип: {GetLocalizedTrackType()}";
            info += $"\nСкорость: {trackSpeed}";
            info += $"\nКрутящий момент: {trackTorque}";
            info += $"\nСцепление: {trackGrip}";
            info += $"\nСегментов: {trackSegments}";
            return info;
        }

        /// <summary>
        /// Получить локализованное название типа гусениц
        /// </summary>
        private string GetLocalizedTrackType()
        {
            return trackType switch
            {
                TrackType.Standard => "Стандартные",
                TrackType.Heavy => "Тяжелые",
                TrackType.Light => "Легкие",
                TrackType.OffRoad => "Внедорожные",
                _ => trackType.ToString()
            };
        }

        /// <summary>
        /// Получить стоимость гусениц
        /// </summary>
        public int GetPartCost()
        {
            return trackType switch
            {
                TrackType.Standard => 800,
                TrackType.Heavy => 1200,
                TrackType.Light => 600,
                TrackType.OffRoad => 1000,
                _ => 800
            };
        }

        /// <summary>
        /// Получить вес гусениц
        /// </summary>
        public float GetPartWeight()
        {
            return trackType switch
            {
                TrackType.Standard => 50f,
                TrackType.Heavy => 80f,
                TrackType.Light => 30f,
                TrackType.OffRoad => 60f,
                _ => 50f
            };
        }

        private void OnDestroy()
        {
            // Очистка ресурсов
            if (trackWheels != null)
            {
                foreach (WheelCollider wheel in trackWheels)
                {
                    if (wheel != null)
                    {
                        DestroyImmediate(wheel.gameObject);
                    }
                }
            }
        }
    }
}
