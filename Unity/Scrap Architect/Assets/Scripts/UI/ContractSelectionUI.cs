using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using ScrapArchitect.Gameplay;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Экран выбора контрактов
    /// </summary>
    public class ContractSelectionUI : UIBase
    {
        [Header("Contract Selection Elements")]
        public Transform contractsContainer;
        public GameObject contractItemPrefab;
        public Button backButton;
        public Button refreshButton;
        
        [Header("Contract Details")]
        public GameObject contractDetailsPanel;
        public TextMeshProUGUI contractTitleText;
        public TextMeshProUGUI contractDescriptionText;
        public TextMeshProUGUI contractDifficultyText;
        public TextMeshProUGUI contractRewardText;
        public TextMeshProUGUI contractClientText;
        public TextMeshProUGUI contractObjectivesText;
        
        [Header("Contract Actions")]
        public Button acceptContractButton;
        public Button closeDetailsButton;
        
        [Header("Filter Elements")]
        public TMP_Dropdown difficultyFilter;
        public TMP_Dropdown typeFilter;
        public Button clearFiltersButton;
        
        [Header("Info Elements")]
        public TextMeshProUGUI availableContractsText;
        public TextMeshProUGUI activeContractsText;
        
        private List<Contract> availableContracts = new List<Contract>();
        private List<ContractItemUI> contractItems = new List<ContractItemUI>();
        private Contract selectedContract;
        
        private void Start()
        {
            SetupButtons();
            SetupFilters();
        }
        
        /// <summary>
        /// Настройка кнопок
        /// </summary>
        private void SetupButtons()
        {
            if (backButton != null)
            {
                backButton.onClick.AddListener(OnBackButtonClick);
            }
            
            if (refreshButton != null)
            {
                refreshButton.onClick.AddListener(OnRefreshButtonClick);
            }
            
            if (acceptContractButton != null)
            {
                acceptContractButton.onClick.AddListener(OnAcceptContractClick);
            }
            
            if (closeDetailsButton != null)
            {
                closeDetailsButton.onClick.AddListener(OnCloseDetailsClick);
            }
            
            if (clearFiltersButton != null)
            {
                clearFiltersButton.onClick.AddListener(OnClearFiltersClick);
            }
        }
        
        /// <summary>
        /// Настройка фильтров
        /// </summary>
        private void SetupFilters()
        {
            if (difficultyFilter != null)
            {
                difficultyFilter.ClearOptions();
                difficultyFilter.AddOptions(new List<string> { "Все сложности", "Легкие", "Средние", "Сложные", "Эксперт", "Мастер" });
                difficultyFilter.onValueChanged.AddListener(OnDifficultyFilterChanged);
            }
            
            if (typeFilter != null)
            {
                typeFilter.ClearOptions();
                typeFilter.AddOptions(new List<string> { "Все типы", "Доставка", "Сбор", "Гонка", "Перевозка", "Строительство", "Разрушение", "Исследование", "Спасение" });
                typeFilter.onValueChanged.AddListener(OnTypeFilterChanged);
            }
        }
        
        /// <summary>
        /// Вызывается при показе экрана выбора контрактов
        /// </summary>
        protected override void OnShow()
        {
            base.OnShow();
            
            LoadContracts();
            UpdateContractDisplay();
            HideContractDetails();
        }
        
        /// <summary>
        /// Загрузить контракты
        /// </summary>
        private void LoadContracts()
        {
            if (ContractManager.Instance != null)
            {
                availableContracts = ContractManager.Instance.GetAvailableContracts();
            }
            else
            {
                availableContracts = new List<Contract>();
            }
        }
        
        /// <summary>
        /// Обновить отображение контрактов
        /// </summary>
        private void UpdateContractDisplay()
        {
            ClearContractItems();
            
            if (contractsContainer == null || contractItemPrefab == null) return;
            
            foreach (Contract contract in availableContracts)
            {
                GameObject contractItemObj = Instantiate(contractItemPrefab, contractsContainer);
                ContractItemUI contractItem = contractItemObj.GetComponent<ContractItemUI>();
                
                if (contractItem != null)
                {
                    contractItem.Initialize(contract, this);
                    contractItems.Add(contractItem);
                }
            }
            
            UpdateInfoTexts();
        }
        
        /// <summary>
        /// Очистить элементы контрактов
        /// </summary>
        private void ClearContractItems()
        {
            foreach (ContractItemUI item in contractItems)
            {
                if (item != null)
                {
                    Destroy(item.gameObject);
                }
            }
            contractItems.Clear();
        }
        
        /// <summary>
        /// Обновить информационные тексты
        /// </summary>
        private void UpdateInfoTexts()
        {
            if (availableContractsText != null)
            {
                availableContractsText.text = $"Доступно контрактов: {availableContracts.Count}";
            }
            
            if (activeContractsText != null)
            {
                int activeCount = ContractManager.Instance != null ? ContractManager.Instance.GetActiveContracts().Count : 0;
                activeContractsText.text = $"Активных контрактов: {activeCount}";
            }
        }
        
        /// <summary>
        /// Показать детали контракта
        /// </summary>
        public void ShowContractDetails(Contract contract)
        {
            selectedContract = contract;
            
            if (contractDetailsPanel != null)
            {
                contractDetailsPanel.SetActive(true);
            }
            
            UpdateContractDetails();
        }
        
        /// <summary>
        /// Скрыть детали контракта
        /// </summary>
        public void HideContractDetails()
        {
            selectedContract = null;
            
            if (contractDetailsPanel != null)
            {
                contractDetailsPanel.SetActive(false);
            }
        }
        
        /// <summary>
        /// Обновить детали контракта
        /// </summary>
        private void UpdateContractDetails()
        {
            if (selectedContract == null) return;
            
            if (contractTitleText != null)
            {
                contractTitleText.text = selectedContract.title;
            }
            
            if (contractDescriptionText != null)
            {
                contractDescriptionText.text = selectedContract.description;
            }
            
            if (contractDifficultyText != null)
            {
                contractDifficultyText.text = $"Сложность: {GetDifficultyText(selectedContract.difficulty)}";
            }
            
            if (contractRewardText != null)
            {
                contractRewardText.text = $"Награда: {selectedContract.reward.scrapReward} скрапа, {selectedContract.reward.experienceReward} опыта";
            }
            
            if (contractClientText != null)
            {
                contractClientText.text = $"Клиент: {selectedContract.clientName}";
            }
            
            if (contractObjectivesText != null)
            {
                string objectivesText = "Цели:\n";
                foreach (var objective in selectedContract.objectives)
                {
                    objectivesText += $"• {objective.description}\n";
                }
                contractObjectivesText.text = objectivesText;
            }
            
            // Обновить состояние кнопки принятия
            if (acceptContractButton != null)
            {
                acceptContractButton.interactable = selectedContract.CanAccept();
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
        /// Обработчик кнопки "Назад"
        /// </summary>
        public override void OnBackButtonClick()
        {
            if (uiManager != null)
            {
                uiManager.PlayButtonClickSound();
                uiManager.ShowMainMenu();
            }
        }
        
        /// <summary>
        /// Обработчик кнопки "Обновить"
        /// </summary>
        public void OnRefreshButtonClick()
        {
            if (uiManager != null)
            {
                uiManager.PlayButtonClickSound();
                LoadContracts();
                UpdateContractDisplay();
            }
        }
        
        /// <summary>
        /// Обработчик кнопки "Принять контракт"
        /// </summary>
        public void OnAcceptContractClick()
        {
            if (selectedContract == null) return;
            
            if (uiManager != null)
            {
                uiManager.PlayButtonClickSound();
                
                if (ContractManager.Instance != null && ContractManager.Instance.AcceptContract(selectedContract))
                {
                    // Контракт принят, переходим к игре
                    uiManager.ShowGameplayUI();
                }
                else
                {
                    Debug.LogWarning("Не удалось принять контракт");
                }
            }
        }
        
        /// <summary>
        /// Обработчик кнопки "Закрыть детали"
        /// </summary>
        public void OnCloseDetailsClick()
        {
            if (uiManager != null)
            {
                uiManager.PlayButtonClickSound();
                HideContractDetails();
            }
        }
        
        /// <summary>
        /// Обработчик изменения фильтра сложности
        /// </summary>
        public void OnDifficultyFilterChanged(int value)
        {
            ApplyFilters();
        }
        
        /// <summary>
        /// Обработчик изменения фильтра типа
        /// </summary>
        public void OnTypeFilterChanged(int value)
        {
            ApplyFilters();
        }
        
        /// <summary>
        /// Обработчик кнопки "Очистить фильтры"
        /// </summary>
        public void OnClearFiltersClick()
        {
            if (uiManager != null)
            {
                uiManager.PlayButtonClickSound();
                
                if (difficultyFilter != null)
                {
                    difficultyFilter.value = 0;
                }
                
                if (typeFilter != null)
                {
                    typeFilter.value = 0;
                }
                
                ApplyFilters();
            }
        }
        
        /// <summary>
        /// Применить фильтры
        /// </summary>
        private void ApplyFilters()
        {
            LoadContracts();
            
            // Фильтр по сложности
            if (difficultyFilter != null && difficultyFilter.value > 0)
            {
                ContractDifficulty selectedDifficulty = (ContractDifficulty)(difficultyFilter.value - 1);
                availableContracts = availableContracts.Where(c => c.difficulty == selectedDifficulty).ToList();
            }
            
            // Фильтр по типу
            if (typeFilter != null && typeFilter.value > 0)
            {
                ContractType selectedType = (ContractType)(typeFilter.value - 1);
                availableContracts = availableContracts.Where(c => c.contractType == selectedType).ToList();
            }
            
            UpdateContractDisplay();
        }
        
        /// <summary>
        /// Обработка нажатия клавиши Escape
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (contractDetailsPanel != null && contractDetailsPanel.activeSelf)
                {
                    OnCloseDetailsClick();
                }
                else
                {
                    OnBackButtonClick();
                }
            }
        }
    }
}
