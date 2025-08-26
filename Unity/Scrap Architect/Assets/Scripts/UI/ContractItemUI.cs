using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ScrapArchitect.Gameplay;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Элемент контракта в списке
    /// </summary>
    public class ContractItemUI : MonoBehaviour
    {
        [Header("Contract Item Elements")]
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI difficultyText;
        public TextMeshProUGUI rewardText;
        public TextMeshProUGUI typeText;
        
        [Header("Visual Elements")]
        public Image backgroundImage;
        public Image difficultyIcon;
        public Image typeIcon;
        public Button selectButton;
        
        [Header("Difficulty Colors")]
        public Color easyColor = Color.green;
        public Color mediumColor = Color.yellow;
        public Color hardColor = Color.red;
        public Color expertColor = Color.magenta;
        public Color masterColor = Color.black;
        
        [Header("Type Icons")]
        public Sprite deliveryIcon;
        public Sprite collectionIcon;
        public Sprite racingIcon;
        public Sprite transportIcon;
        public Sprite constructionIcon;
        public Sprite destructionIcon;
        public Sprite explorationIcon;
        public Sprite rescueIcon;
        
        private Contract contract;
        private ContractSelectionUI parentUI;
        
        private void Start()
        {
            SetupButton();
        }
        
        /// <summary>
        /// Инициализация элемента контракта
        /// </summary>
        public void Initialize(Contract contractData, ContractSelectionUI parent)
        {
            contract = contractData;
            parentUI = parent;
            
            UpdateDisplay();
        }
        
        /// <summary>
        /// Настройка кнопки
        /// </summary>
        private void SetupButton()
        {
            if (selectButton != null)
            {
                selectButton.onClick.AddListener(OnSelectButtonClick);
            }
        }
        
        /// <summary>
        /// Обновить отображение
        /// </summary>
        private void UpdateDisplay()
        {
            if (contract == null) return;
            
            // Заголовок
            if (titleText != null)
            {
                titleText.text = contract.title;
            }
            
            // Описание
            if (descriptionText != null)
            {
                descriptionText.text = contract.description;
            }
            
            // Сложность
            if (difficultyText != null)
            {
                difficultyText.text = GetDifficultyText(contract.difficulty);
            }
            
            // Награда
            if (rewardText != null)
            {
                rewardText.text = $"{contract.reward.scrapReward} скрапа";
            }
            
            // Тип
            if (typeText != null)
            {
                typeText.text = GetTypeText(contract.contractType);
            }
            
            // Цвет сложности
            if (backgroundImage != null)
            {
                backgroundImage.color = GetDifficultyColor(contract.difficulty);
            }
            
            // Иконка типа
            if (typeIcon != null)
            {
                typeIcon.sprite = GetTypeIcon(contract.contractType);
            }
        }
        
        /// <summary>
        /// Получить текст сложности
        /// </summary>
        private string GetDifficultyText(ContractDifficulty difficulty)
        {
            return difficulty switch
            {
                ContractDifficulty.Easy => "Легкая",
                ContractDifficulty.Medium => "Средняя",
                ContractDifficulty.Hard => "Сложная",
                ContractDifficulty.Expert => "Эксперт",
                ContractDifficulty.Master => "Мастер",
                _ => "Неизвестно"
            };
        }
        
        /// <summary>
        /// Получить текст типа
        /// </summary>
        private string GetTypeText(ContractType type)
        {
            return type switch
            {
                ContractType.Delivery => "Доставка",
                ContractType.Collection => "Сбор",
                ContractType.Racing => "Гонка",
                ContractType.Transport => "Перевозка",
                ContractType.Construction => "Строительство",
                ContractType.Destruction => "Разрушение",
                ContractType.Exploration => "Исследование",
                ContractType.Rescue => "Спасение",
                _ => "Неизвестно"
            };
        }
        
        /// <summary>
        /// Получить цвет сложности
        /// </summary>
        private Color GetDifficultyColor(ContractDifficulty difficulty)
        {
            return difficulty switch
            {
                ContractDifficulty.Easy => easyColor,
                ContractDifficulty.Medium => mediumColor,
                ContractDifficulty.Hard => hardColor,
                ContractDifficulty.Expert => expertColor,
                ContractDifficulty.Master => masterColor,
                _ => Color.white
            };
        }
        
        /// <summary>
        /// Получить иконку типа
        /// </summary>
        private Sprite GetTypeIcon(ContractType type)
        {
            return type switch
            {
                ContractType.Delivery => deliveryIcon,
                ContractType.Collection => collectionIcon,
                ContractType.Racing => racingIcon,
                ContractType.Transport => transportIcon,
                ContractType.Construction => constructionIcon,
                ContractType.Destruction => destructionIcon,
                ContractType.Exploration => explorationIcon,
                ContractType.Rescue => rescueIcon,
                _ => null
            };
        }
        
        /// <summary>
        /// Обработчик нажатия на элемент контракта
        /// </summary>
        public void OnSelectButtonClick()
        {
            if (parentUI != null && contract != null)
            {
                parentUI.ShowContractDetails(contract);
            }
        }
        
        /// <summary>
        /// Получить контракт
        /// </summary>
        public Contract GetContract()
        {
            return contract;
        }
        
        /// <summary>
        /// Проверить, доступен ли контракт
        /// </summary>
        public bool IsAvailable()
        {
            return contract != null && contract.CanAccept();
        }
        
        /// <summary>
        /// Обновить состояние доступности
        /// </summary>
        public void UpdateAvailability()
        {
            if (selectButton != null)
            {
                selectButton.interactable = IsAvailable();
            }
            
            // Изменить прозрачность, если контракт недоступен
            if (backgroundImage != null)
            {
                Color color = backgroundImage.color;
                color.a = IsAvailable() ? 1f : 0.5f;
                backgroundImage.color = color;
            }
        }
    }
}
