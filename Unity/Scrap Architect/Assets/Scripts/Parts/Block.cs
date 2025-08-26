using UnityEngine;
using ScrapArchitect.Physics;

namespace ScrapArchitect.Parts
{
    /// <summary>
    /// Базовый строительный блок - основа для всех конструкций
    /// </summary>
    public class Block : PartBase
    {
        [Header("Block Settings")]
        public BlockType blockType = BlockType.Wood;
        public float strength = 50f;
        public bool isStructural = true;
        
        [Header("Block Visual")]
        public Color woodColor = new Color(0.6f, 0.4f, 0.2f);
        public Color metalColor = new Color(0.7f, 0.7f, 0.7f);
        public Color plasticColor = new Color(0.2f, 0.6f, 0.8f);
        
        private Renderer rend;
        
        private void Start()
        {
            rend = GetComponent<Renderer>();
            InitializeBlock();
        }
        
        /// <summary>
        /// Инициализация блока
        /// </summary>
        private void InitializeBlock()
        {
            // Устанавливаем тип детали
            partType = PartType.Block;
            
            // Настраиваем параметры в зависимости от типа блока
            SetupBlockProperties();
            
            // Устанавливаем цвет в зависимости от типа
            SetBlockColor();
            
            Debug.Log($"Block {partName} ({blockType}) initialized");
        }
        
        /// <summary>
        /// Настройка свойств блока в зависимости от типа
        /// </summary>
        private void SetupBlockProperties()
        {
            switch (blockType)
            {
                case BlockType.Wood:
                    mass = 0.5f;
                    maxHealth = 30f;
                    strength = 30f;
                    cost = 5f;
                    unlockLevel = 1;
                    break;
                    
                case BlockType.Metal:
                    mass = 2f;
                    maxHealth = 80f;
                    strength = 80f;
                    cost = 15f;
                    unlockLevel = 2;
                    break;
                    
                case BlockType.Plastic:
                    mass = 0.3f;
                    maxHealth = 20f;
                    strength = 20f;
                    cost = 3f;
                    unlockLevel = 1;
                    break;
                    
                case BlockType.Stone:
                    mass = 3f;
                    maxHealth = 100f;
                    strength = 100f;
                    cost = 20f;
                    unlockLevel = 3;
                    break;
            }
            
            // Обновляем текущее здоровье
            currentHealth = maxHealth;
        }
        
        /// <summary>
        /// Установка цвета блока
        /// </summary>
        private void SetBlockColor()
        {
            if (rend != null)
            {
                Color blockColor = Color.white;
                
                switch (blockType)
                {
                    case BlockType.Wood:
                        blockColor = woodColor;
                        break;
                    case BlockType.Metal:
                        blockColor = metalColor;
                        break;
                    case BlockType.Plastic:
                        blockColor = plasticColor;
                        break;
                    case BlockType.Stone:
                        blockColor = Color.gray;
                        break;
                }
                
                rend.material.color = blockColor;
            }
        }
        
        /// <summary>
        /// Специфичная логика блока при соединении
        /// </summary>
        protected override void OnPartSpecificAction()
        {
            // Блоки не имеют специальной логики при соединении
            // Они просто служат строительными элементами
        }
        
        /// <summary>
        /// Получение информации о блоке
        /// </summary>
        public override string GetPartInfo()
        {
            string baseInfo = base.GetPartInfo();
            string blockInfo = $"\nBlock Type: {blockType}\nStrength: {strength}\nStructural: {isStructural}";
            
            return baseInfo + blockInfo;
        }
        
        /// <summary>
        /// Проверка прочности блока
        /// </summary>
        public bool IsStrongEnough(float load)
        {
            return load <= strength;
        }
        
        /// <summary>
        /// Получение урона с учетом типа блока
        /// </summary>
        public override void TakeDamage(float damage)
        {
            // Разные типы блоков по-разному воспринимают урон
            float actualDamage = damage;
            
            switch (blockType)
            {
                case BlockType.Metal:
                    actualDamage *= 0.7f; // Металл более устойчив
                    break;
                case BlockType.Stone:
                    actualDamage *= 0.5f; // Камень очень устойчив
                    break;
                case BlockType.Plastic:
                    actualDamage *= 1.3f; // Пластик менее устойчив
                    break;
            }
            
            base.TakeDamage(actualDamage);
        }
    }
    
    /// <summary>
    /// Типы блоков
    /// </summary>
    public enum BlockType
    {
        Wood,    // Деревянный блок - легкий, дешевый
        Metal,   // Металлический блок - прочный, тяжелый
        Plastic, // Пластиковый блок - очень легкий, хрупкий
        Stone    // Каменный блок - очень прочный, очень тяжелый
    }
}
