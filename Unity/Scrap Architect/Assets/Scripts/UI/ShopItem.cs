using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ScrapArchitect.Economy;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// UI элемент для отображения предмета в магазине
    /// </summary>
    public class ShopItem : MonoBehaviour
    {
        [Header("UI References")]
        public TextMeshProUGUI itemNameText;
        public TextMeshProUGUI priceText;
        public TextMeshProUGUI descriptionText;
        public Image itemIcon;
        public Button purchaseButton;
        public Button infoButton;
        public GameObject saleBadge;
        public TextMeshProUGUI saleText;
        public Slider saleTimerSlider;
        
        [Header("Visual Settings")]
        public Color normalPriceColor = Color.white;
        public Color salePriceColor = Color.red;
        public Color affordableColor = Color.green;
        public Color expensiveColor = Color.red;
        public Color lockedColor = Color.gray;
        
        [Header("Audio Settings")]
        public AudioClip buttonClickSound;
        public AudioClip hoverSound;
        
        private string itemID;
        private EconomyManager economyManager;
        private bool hasSale = false;
        private float saleEndTime = 0f;
        
        private void Start()
        {
            SetupButtons();
        }
        
        private void Update()
        {
            UpdateSaleTimer();
        }
        
        public void Initialize(string itemID, EconomyManager manager)
        {
            this.itemID = itemID;
            this.economyManager = manager;
            
            UpdateUI();
        }
        
        private void SetupButtons()
        {
            if (purchaseButton != null)
            {
                purchaseButton.onClick.AddListener(OnPurchaseButtonClicked);
            }
            
            if (infoButton != null)
            {
                infoButton.onClick.AddListener(OnInfoButtonClicked);
            }
        }
        
        private void UpdateUI()
        {
            if (economyManager == null) return;
            
            // Обновить название
            if (itemNameText != null)
            {
                itemNameText.text = GetDisplayName(itemID);
            }
            
            // Обновить цену
            UpdatePriceDisplay();
            
            // Обновить описание
            if (descriptionText != null)
            {
                descriptionText.text = GetItemDescription(itemID);
            }
            
            // Обновить иконку
            UpdateItemIcon();
            
            // Обновить состояние кнопки покупки
            UpdatePurchaseButton();
            
            // Обновить состояние распродажи
            UpdateSaleDisplay();
        }
        
        private void UpdatePriceDisplay()
        {
            if (priceText == null || economyManager == null) return;
            
            float basePrice = economyManager.GetItemPrice(itemID);
            float currentPrice = economyManager.GetItemPrice(itemID);
            int currentScrap = economyManager.GetCurrentScrap();
            
            // Определить, есть ли скидка
            hasSale = currentPrice < basePrice;
            
            if (hasSale)
            {
                priceText.text = $"<s>{basePrice}</s> {currentPrice} 💰";
                priceText.color = salePriceColor;
            }
            else
            {
                priceText.text = $"{currentPrice} 💰";
                priceText.color = normalPriceColor;
            }
            
            // Изменить цвет в зависимости от доступности
            if (currentPrice <= currentScrap)
            {
                priceText.color = affordableColor;
            }
            else
            {
                priceText.color = expensiveColor;
            }
        }
        
        private void UpdateItemIcon()
        {
            if (itemIcon == null) return;
            
            // Здесь можно загрузить иконку предмета
            // Пока что используем цвет в зависимости от типа
            Color iconColor = GetItemColor(itemID);
            itemIcon.color = iconColor;
        }
        
        private void UpdatePurchaseButton()
        {
            if (purchaseButton == null || economyManager == null) return;
            
            float price = economyManager.GetItemPrice(itemID);
            int currentScrap = economyManager.GetCurrentScrap();
            bool canAfford = price <= currentScrap;
            
            purchaseButton.interactable = canAfford;
            
            // Изменить цвет кнопки
            Image buttonImage = purchaseButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = canAfford ? affordableColor : expensiveColor;
            }
        }
        
        private void UpdateSaleDisplay()
        {
            if (saleBadge == null) return;
            
            saleBadge.SetActive(hasSale);
            
            if (hasSale && saleText != null)
            {
                saleText.text = "SALE!";
            }
        }
        
        private void UpdateSaleTimer()
        {
            if (!hasSale || saleTimerSlider == null) return;
            
            if (Time.time < saleEndTime)
            {
                float timeLeft = saleEndTime - Time.time;
                float totalTime = 300f; // 5 минут
                saleTimerSlider.value = timeLeft / totalTime;
            }
            else
            {
                // Распродажа закончилась
                hasSale = false;
                UpdateUI();
            }
        }
        
        private void OnPurchaseButtonClicked()
        {
            if (economyManager == null) return;
            
            PlayButtonClickSound();
            
            bool success = economyManager.PurchaseItem(itemID);
            if (success)
            {
                Debug.Log($"Successfully purchased {itemID}");
                // Предмет куплен, обновить UI
                UpdateUI();
            }
            else
            {
                Debug.LogWarning($"Failed to purchase {itemID}");
            }
        }
        
        private void OnInfoButtonClicked()
        {
            if (economyManager == null) return;
            
            PlayButtonClickSound();
            
            string info = economyManager.GetItemInfo(itemID);
            Debug.Log($"Item Info:\n{info}");
        }
        
        private string GetDisplayName(string itemID)
        {
            return itemID switch
            {
                "WoodBlock" => "Деревянный блок",
                "MetalBlock" => "Металлический блок",
                "PlasticBlock" => "Пластиковый блок",
                "StoneBlock" => "Каменный блок",
                
                "SmallWheel" => "Маленькое колесо",
                "MediumWheel" => "Среднее колесо",
                "LargeWheel" => "Большое колесо",
                "OffRoadWheel" => "Внедорожное колесо",
                
                "ElectricMotor" => "Электродвигатель",
                "GasolineMotor" => "Бензиновый двигатель",
                "DieselMotor" => "Дизельный двигатель",
                "JetMotor" => "Реактивный двигатель",
                
                "FixedJoint" => "Жесткое соединение",
                "HingeJoint" => "Шарнирное соединение",
                "SpringJoint" => "Пружинное соединение",
                "SliderJoint" => "Ползунковое соединение",
                "ConfigurableJoint" => "Настраиваемое соединение",
                
                "BasicSeat" => "Базовое сиденье",
                "ComfortSeat" => "Комфортное сиденье",
                "RacingSeat" => "Гоночное сиденье",
                "LuxurySeat" => "Люксовое сиденье",
                "IndustrialSeat" => "Промышленное сиденье",
                
                "Hammer" => "Молоток",
                "Wrench" => "Гаечный ключ",
                "Screwdriver" => "Отвертка",
                "Drill" => "Дрель",
                "Welder" => "Сварщик",
                "Magnet" => "Магнит",
                "Vacuum" => "Пылесос",
                "Sprayer" => "Распылитель",
                
                _ => itemID
            };
        }
        
        private string GetItemDescription(string itemID)
        {
            return itemID switch
            {
                "WoodBlock" => "Прочный деревянный блок для строительства",
                "MetalBlock" => "Прочный металлический блок",
                "PlasticBlock" => "Легкий пластиковый блок",
                "StoneBlock" => "Тяжелый каменный блок",
                
                "SmallWheel" => "Маленькое колесо для легких конструкций",
                "MediumWheel" => "Универсальное среднее колесо",
                "LargeWheel" => "Большое колесо для тяжелых машин",
                "OffRoadWheel" => "Внедорожное колесо с хорошим сцеплением",
                
                "ElectricMotor" => "Экологичный электродвигатель",
                "GasolineMotor" => "Мощный бензиновый двигатель",
                "DieselMotor" => "Экономичный дизельный двигатель",
                "JetMotor" => "Сверхмощный реактивный двигатель",
                
                "FixedJoint" => "Прочное жесткое соединение",
                "HingeJoint" => "Гибкое шарнирное соединение",
                "SpringJoint" => "Эластичное пружинное соединение",
                "SliderJoint" => "Подвижное ползунковое соединение",
                "ConfigurableJoint" => "Универсальное настраиваемое соединение",
                
                "BasicSeat" => "Простое сиденье водителя",
                "ComfortSeat" => "Удобное сиденье с подушками",
                "RacingSeat" => "Спортивное сиденье для гонок",
                "LuxurySeat" => "Роскошное сиденье премиум-класса",
                "IndustrialSeat" => "Прочное промышленное сиденье",
                
                "Hammer" => "Инструмент для разрушения и строительства",
                "Wrench" => "Инструмент для закручивания гаек",
                "Screwdriver" => "Инструмент для работы с винтами",
                "Drill" => "Мощная дрель для сверления",
                "Welder" => "Инструмент для сварки металла",
                "Magnet" => "Магнит для сбора металлических предметов",
                "Vacuum" => "Пылесос для уборки мусора",
                "Sprayer" => "Распылитель для покраски",
                
                _ => "Описание недоступно"
            };
        }
        
        private Color GetItemColor(string itemID)
        {
            if (itemID.Contains("Block"))
            {
                if (itemID.Contains("Wood")) return new Color(0.6f, 0.4f, 0.2f); // Коричневый
                if (itemID.Contains("Metal")) return Color.gray;
                if (itemID.Contains("Plastic")) return Color.cyan;
                if (itemID.Contains("Stone")) return Color.gray;
            }
            else if (itemID.Contains("Wheel"))
            {
                return Color.black;
            }
            else if (itemID.Contains("Motor"))
            {
                if (itemID.Contains("Electric")) return Color.green;
                if (itemID.Contains("Gasoline")) return Color.red;
                if (itemID.Contains("Diesel")) return Color.blue;
                if (itemID.Contains("Jet")) return Color.magenta;
            }
            else if (itemID.Contains("Joint"))
            {
                return Color.yellow;
            }
            else if (itemID.Contains("Seat"))
            {
                return Color.blue;
            }
            else if (itemID.Contains("Tool") || itemID.Contains("Hammer") || itemID.Contains("Wrench") ||
                     itemID.Contains("Drill") || itemID.Contains("Welder") || itemID.Contains("Magnet") ||
                     itemID.Contains("Vacuum") || itemID.Contains("Sprayer"))
            {
                return Color.orange;
            }
            
            return Color.white;
        }
        
        public void OnPointerEnter()
        {
            PlayHoverSound();
        }
        
        public void OnPointerExit()
        {
            // Можно добавить эффекты при уходе курсора
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
        
        public string GetItemID()
        {
            return itemID;
        }
        
        public bool HasSale()
        {
            return hasSale;
        }
        
        public void SetSaleEndTime(float endTime)
        {
            saleEndTime = endTime;
            hasSale = true;
            UpdateUI();
        }
        
        private void OnDestroy()
        {
            if (purchaseButton != null)
            {
                purchaseButton.onClick.RemoveListener(OnPurchaseButtonClicked);
            }
            
            if (infoButton != null)
            {
                infoButton.onClick.RemoveListener(OnInfoButtonClicked);
            }
        }
    }
}
