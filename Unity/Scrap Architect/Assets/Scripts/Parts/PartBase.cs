using UnityEngine;
using System;
using ScrapArchitect.Physics;

namespace ScrapArchitect.Parts
{
    /// <summary>
    /// Базовый класс для всех деталей в игре
    /// </summary>
    public abstract class PartBase : PartController
    {
        [Header("Part Base Settings")]
        public string partID = "";
        public string description = "";
        public int unlockLevel = 1;
        public float cost = 10f;
        public bool isUnlocked = true;
        
        [Header("Visual")]
        public Material defaultMaterial;
        public Material selectedMaterial;
        public Material damagedMaterial;
        
        [Header("Audio")]
        public AudioClip connectSound;
        public AudioClip disconnectSound;
        public AudioClip damageSound;
        public AudioClip destroySound;
        public AudioClip stressSound;
        public AudioClip breakSound;
        public AudioClip activateSound;
        public AudioClip deactivateSound;
        public AudioClip useSound;
        public AudioClip impactSound;
        public AudioClip seatSound;
        public AudioClip engineStartSound;
        public AudioClip hornSound;
        
        [Header("Effects")]
        public GameObject connectEffect;
        public GameObject damageEffect;
        public GameObject destroyEffect;
        
        // Audio source
        private AudioSource audioSource;
        
        protected override void Awake()
        {
            base.Awake();
            InitializePartBase();
        }
        
        /// <summary>
        /// Инициализация базовой детали
        /// </summary>
        private void InitializePartBase()
        {
            // Добавляем AudioSource если его нет
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            
            // Настройка AudioSource
            audioSource.playOnAwake = false;
            audioSource.volume = 0.5f;
            
            // Подписываемся на события
            OnPartConnected += OnPartConnectedHandler;
            OnPartDisconnected += OnPartDisconnectedHandler;
            OnPartDestroyed += OnPartDestroyedHandler;
            
            // Устанавливаем материал по умолчанию
            if (defaultMaterial != null && rend != null)
            {
                rend.material = defaultMaterial;
            }
            
            Debug.Log($"PartBase {partName} initialized");
        }
        
        /// <summary>
        /// Абстрактный метод для специфичной логики детали
        /// </summary>
        protected abstract void OnPartSpecificAction();
        
        /// <summary>
        /// Проверка возможности разблокировки
        /// </summary>
        public bool CanUnlock(int playerLevel)
        {
            return playerLevel >= unlockLevel;
        }
        
        /// <summary>
        /// Разблокировка детали
        /// </summary>
        public void Unlock()
        {
            if (!isUnlocked)
            {
                isUnlocked = true;
                Debug.Log($"Part {partName} unlocked!");
            }
        }
        
        /// <summary>
        /// Блокировка детали
        /// </summary>
        public void Lock()
        {
            isUnlocked = false;
            Debug.Log($"Part {partName} locked!");
        }
        
        /// <summary>
        /// Обработчик соединения деталей
        /// </summary>
        private void OnPartConnectedHandler(PartController thisPart, PartController otherPart)
        {
            // Воспроизводим звук соединения
            if (connectSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(connectSound);
            }
            
            // Создаем эффект соединения
            if (connectEffect != null)
            {
                Instantiate(connectEffect, transform.position, transform.rotation);
            }
            
            // Вызываем специфичную логику
            OnPartSpecificAction();
        }
        
        /// <summary>
        /// Обработчик отсоединения деталей
        /// </summary>
        private void OnPartDisconnectedHandler(PartController thisPart, PartController otherPart)
        {
            // Воспроизводим звук отсоединения
            if (disconnectSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(disconnectSound);
            }
        }
        
        /// <summary>
        /// Обработчик уничтожения детали
        /// </summary>
        private void OnPartDestroyedHandler(PartController part)
        {
            // Воспроизводим звук уничтожения
            if (destroySound != null && audioSource != null)
            {
                audioSource.PlayOneShot(destroySound);
            }
            
            // Создаем эффект уничтожения
            if (destroyEffect != null)
            {
                Instantiate(destroyEffect, transform.position, transform.rotation);
            }
        }
        
        /// <summary>
        /// Переопределенный метод получения урона
        /// </summary>
        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            
            // Воспроизводим звук урона
            if (damageSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(damageSound);
            }
            
            // Создаем эффект урона
            if (damageEffect != null)
            {
                Instantiate(damageEffect, transform.position, transform.rotation);
            }
            
            // Изменяем материал при повреждении
            if (damagedMaterial != null && rend != null && currentHealth < maxHealth * 0.5f)
            {
                rend.material = damagedMaterial;
            }
        }
        
        /// <summary>
        /// Переопределенный метод выбора
        /// </summary>
        public override void Select()
        {
            base.Select();
            
            // Изменяем материал при выборе
            if (selectedMaterial != null && rend != null)
            {
                rend.material = selectedMaterial;
            }
        }
        
        /// <summary>
        /// Переопределенный метод отмены выбора
        /// </summary>
        public override void Deselect()
        {
            base.Deselect();
            
            // Возвращаем материал по умолчанию
            if (defaultMaterial != null && rend != null)
            {
                rend.material = defaultMaterial;
            }
        }

        /// <summary>
        /// Воспроизвести звук по имени
        /// </summary>
        protected void PlaySound(string soundName)
        {
            if (audioSource == null) return;

            AudioClip clipToPlay = soundName switch
            {
                "connect" => connectSound,
                "disconnect" => disconnectSound,
                "destroy" => destroySound,
                "damage" => damageSound,
                "stress" => stressSound,
                "break" => breakSound,
                "activate" => activateSound,
                "deactivate" => deactivateSound,
                "use" => useSound,
                "impact" => impactSound,
                "seat" => seatSound,
                "engine_start" => engineStartSound,
                "horn" => hornSound,
                _ => null
            };

            if (clipToPlay != null)
            {
                audioSource.PlayOneShot(clipToPlay);
            }
        }
        
        /// <summary>
        /// Получение полной информации о детали
        /// </summary>
        public override string GetPartInfo()
        {
            string baseInfo = base.GetPartInfo();
            string additionalInfo = $"\nID: {partID}\nCost: {cost}\nUnlock Level: {unlockLevel}\nUnlocked: {isUnlocked}";
            
            return baseInfo + additionalInfo;
        }
        
        /// <summary>
        /// Сохранение данных детали
        /// </summary>
        public virtual PartData SavePartData()
        {
            return new PartData
            {
                partID = partID,
                partName = partName,
                position = transform.position,
                rotation = transform.rotation,
                currentHealth = currentHealth,
                isUnlocked = isUnlocked
            };
        }
        
        /// <summary>
        /// Загрузка данных детали
        /// </summary>
        public virtual void LoadPartData(PartData data)
        {
            if (data != null)
            {
                partID = data.partID;
                partName = data.partName;
                transform.position = data.position;
                transform.rotation = data.rotation;
                currentHealth = data.currentHealth;
                isUnlocked = data.isUnlocked;
            }
        }
    }
    
    /// <summary>
    /// Данные для сохранения детали
    /// </summary>
    [Serializable]
    public class PartData
    {
        public string partID;
        public string partName;
        public Vector3 position;
        public Quaternion rotation;
        public float currentHealth;
        public bool isUnlocked;
    }
}
