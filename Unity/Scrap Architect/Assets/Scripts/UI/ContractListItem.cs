using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ScrapArchitect.Gameplay;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// UI элемент для отображения контракта в списке
    /// </summary>
    public class ContractListItem : MonoBehaviour
    {
        [Header("UI References")]
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI difficultyText;
        public TextMeshProUGUI rewardText;
        public TextMeshProUGUI timeText;
        public Image difficultyIcon;
        public Image typeIcon;
        public Button acceptButton;
        public Button cancelButton;
        public Button infoButton;
        public Slider progressSlider;
        
        [Header("Visual Settings")]
        public Color easyColor = Color.green;
        public Color mediumColor = Color.yellow;
        public Color hardColor = Color.orange;
        public Color expertColor = Color.red;
        public Color masterColor = Color.purple;
        
        public Color normalColor = Color.white;
        public Color selectedColor = Color.cyan;
        public Color hoverColor = Color.lightBlue;
        
        [Header("Audio Settings")]
        public AudioClip buttonClickSound;
        public AudioClip hoverSound;
        
        private Contract contract;
        private ContractManager contractManager;
        private bool isSelected = false;
        
        private void Start()
        {
            SetupButtons();
        }
        
        public void Initialize(Contract contract, ContractManager manager)
        {
            this.contract = contract;
            this.contractManager = manager;
            
            UpdateUI();
        }
        
        private void SetupButtons()
        {
            if (acceptButton != null)
            {
                acceptButton.onClick.AddListener(OnAcceptButtonClicked);
            }
            
            if (cancelButton != null)
            {
                cancelButton.onClick.AddListener(OnCancelButtonClicked);
            }
            
            if (infoButton != null)
            {
                infoButton.onClick.AddListener(OnInfoButtonClicked);
            }
        }
        
        private void Update()
        {
            if (contract != null && contract.status == ContractStatus.Active)
            {
                UpdateProgress();
                UpdateTimeRemaining();
            }
        }
        
        private void UpdateUI()
        {
            if (contract == null)
            {
                return;
            }
            
            if (titleText != null)
            {
                titleText.text = contract.title;
            }
            
            if (descriptionText != null)
            {
                descriptionText.text = contract.description;
            }
            
            if (difficultyText != null)
            {
                difficultyText.text = GetDifficultyText(contract.difficulty);
                difficultyText.color = GetDifficultyColor(contract.difficulty);
            }
            
            if (rewardText != null)
            {
                rewardText.text = $"Награда: {contract.reward.scrapReward} 💰 {contract.reward.experienceReward} ⭐";
            }
            
            if (timeText != null)
            {
                UpdateTimeRemaining();
            }
            
            if (progressSlider != null)
            {
                UpdateProgress();
            }
            
            UpdateButtonStates();
            UpdateVisualState();
        }
        
        private void UpdateProgress()
        {
            if (progressSlider != null && contract != null)
            {
                float progress = contract.GetProgress();
                progressSlider.value = progress;
                
                // Изменить цвет слайдера в зависимости от прогресса
                Image fillImage = progressSlider.fillRect.GetComponent<Image>();
                if (fillImage != null)
                {
                    if (progress >= 1f)
                    {
                        fillImage.color = Color.green;
                    }
                    else if (progress >= 0.5f)
                    {
                        fillImage.color = Color.yellow;
                    }
                    else
                    {
                        fillImage.color = Color.red;
                    }
                }
            }
        }
        
        private void UpdateTimeRemaining()
        {
            if (timeText != null && contract != null)
            {
                if (contract.status == ContractStatus.Active && contract.timeLimit > 0)
                {
                    float remaining = contract.GetRemainingTime();
                    if (remaining >= 0)
                    {
                        int minutes = Mathf.FloorToInt(remaining / 60f);
                        int seconds = Mathf.FloorToInt(remaining % 60f);
                        timeText.text = $"Время: {minutes:00}:{seconds:00}";
                        
                        // Изменить цвет в зависимости от оставшегося времени
                        if (remaining < 30f)
                        {
                            timeText.color = Color.red;
                        }
                        else if (remaining < 60f)
                        {
                            timeText.color = Color.yellow;
                        }
                        else
                        {
                            timeText.color = Color.white;
                        }
                    }
                    else
                    {
                        timeText.text = "Время: ∞";
                        timeText.color = Color.white;
                    }
                }
                else
                {
                    timeText.text = "";
                }
            }
        }
        
        private void UpdateButtonStates()
        {
            if (acceptButton != null)
            {
                acceptButton.gameObject.SetActive(contract.status == ContractStatus.Available);
                acceptButton.interactable = contract.CanAccept();
            }
            
            if (cancelButton != null)
            {
                cancelButton.gameObject.SetActive(contract.status == ContractStatus.Active);
            }
            
            if (infoButton != null)
            {
                infoButton.gameObject.SetActive(true);
            }
        }
        
        private void UpdateVisualState()
        {
            Image backgroundImage = GetComponent<Image>();
            if (backgroundImage != null)
            {
                if (isSelected)
                {
                    backgroundImage.color = selectedColor;
                }
                else
                {
                    backgroundImage.color = normalColor;
                }
            }
        }
        
        private string GetDifficultyText(ContractDifficulty difficulty)
        {
            return difficulty switch
            {
                ContractDifficulty.Easy => "Легкий",
                ContractDifficulty.Medium => "Средний",
                ContractDifficulty.Hard => "Сложный",
                ContractDifficulty.Expert => "Эксперт",
                ContractDifficulty.Master => "Мастер",
                _ => "Неизвестно"
            };
        }
        
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
        
        private void OnAcceptButtonClicked()
        {
            if (contractManager != null && contract != null)
            {
                PlayButtonClickSound();
                
                bool success = contractManager.AcceptContract(contract);
                if (success)
                {
                    Debug.Log($"Contract accepted: {contract.title}");
                    // Обновить UI после принятия контракта
                    UpdateUI();
                }
                else
                {
                    Debug.LogWarning("Failed to accept contract");
                }
            }
        }
        
        private void OnCancelButtonClicked()
        {
            if (contractManager != null && contract != null)
            {
                PlayButtonClickSound();
                
                bool success = contractManager.CancelContract(contract);
                if (success)
                {
                    Debug.Log($"Contract cancelled: {contract.title}");
                    // Обновить UI после отмены контракта
                    UpdateUI();
                }
                else
                {
                    Debug.LogWarning("Failed to cancel contract");
                }
            }
        }
        
        private void OnInfoButtonClicked()
        {
            if (contract != null)
            {
                PlayButtonClickSound();
                
                ShowContractInfo();
            }
        }
        
        private void ShowContractInfo()
        {
            if (contract == null)
            {
                return;
            }
            
            string info = contract.GetContractInfo();
            
            // Добавить информацию о целях
            info += "\n\nЦели:\n";
            foreach (var objective in contract.objectives)
            {
                string status = objective.isCompleted ? "✅" : "⏳";
                info += $"{status} {objective.description}\n";
                if (objective.type == ContractObjective.ObjectiveType.CollectItems || 
                    objective.type == ContractObjective.ObjectiveType.DestroyObjects)
                {
                    info += $"   Прогресс: {objective.currentValue}/{objective.targetValue}\n";
                }
            }
            
            Debug.Log($"Contract Info:\n{info}");
        }
        
        public void SetSelected(bool selected)
        {
            isSelected = selected;
            UpdateVisualState();
        }
        
        public void OnPointerEnter()
        {
            if (!isSelected)
            {
                Image backgroundImage = GetComponent<Image>();
                if (backgroundImage != null)
                {
                    backgroundImage.color = hoverColor;
                }
                
                PlayHoverSound();
            }
        }
        
        public void OnPointerExit()
        {
            if (!isSelected)
            {
                Image backgroundImage = GetComponent<Image>();
                if (backgroundImage != null)
                {
                    backgroundImage.color = normalColor;
                }
            }
        }
        
        private void PlayButtonClickSound()
        {
            if (buttonClickSound != null)
            {
                AudioSource.PlayClipAtPoint(buttonClickSound, Camera.main.transform.position);
            }
        }
        
        private void PlayHoverSound()
        {
            if (hoverSound != null)
            {
                AudioSource.PlayClipAtPoint(hoverSound, Camera.main.transform.position);
            }
        }
        
        public Contract GetContract()
        {
            return contract;
        }
        
        public bool IsSelected()
        {
            return isSelected;
        }
        
        public void RefreshInfo()
        {
            UpdateUI();
        }
        
        private void OnDestroy()
        {
            if (acceptButton != null)
            {
                acceptButton.onClick.RemoveListener(OnAcceptButtonClicked);
            }
            
            if (cancelButton != null)
            {
                cancelButton.onClick.RemoveListener(OnCancelButtonClicked);
            }
            
            if (infoButton != null)
            {
                infoButton.onClick.RemoveListener(OnInfoButtonClicked);
            }
        }
    }
}
