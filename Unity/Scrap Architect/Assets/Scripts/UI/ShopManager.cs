using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using ScrapArchitect.Economy;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Менеджер магазина - управляет отображением и покупкой деталей
    /// </summary>
    public class ShopManager : MonoBehaviour
    {
        [Header("Shop UI References")]
        public GameObject shopPanel;
        public Transform shopContent;
        public GameObject shopItemPrefab;
        public TextMeshProUGUI scrapText;
        public Button closeButton;
        public Button refreshButton;
        public Button randomSaleButton;
        
        [Header("Category Tabs")]
        public Button blocksTab;
        public Button wheelsTab;
        public Button motorsTab;
        public Button jointsTab;
        public Button seatsTab;
        public Button toolsTab;
        public Button allTab;
        
        [Header("Visual Settings")]
        public Color normalPriceColor = Color.white;
        public Color salePriceColor = Color.red;
        public Color lockedItemColor = Color.gray;
        public Color unlockedItemColor = Color.green;
        
        [Header("Audio Settings")]
        public AudioClip tabSwitchSound;
        public AudioClip refreshSound;
        
        private EconomyManager economyManager;
        private List<ShopItem> shopItems = new List<ShopItem>();
        private string currentCategory = "All";
        
        private void Start()
        {
            InitializeShop();
        }
        
        /// <summary>
        /// Инициализация магазина
        /// </summary>
        private void InitializeShop()
        {
            economyManager = FindObjectOfType<EconomyManager>();
            if (economyManager == null)
            {
                Debug.LogError("EconomyManager not found!");
                return;
            }
            
            SetupButtons();
            SetupCategoryTabs();
            
            // Подписаться на события экономики
            economyManager.OnScrapChanged += UpdateScrapDisplay;
            economyManager.OnItemUnlocked += OnItemUnlocked;
            economyManager.OnSaleStarted += OnSaleStarted;
            economyManager.OnSaleEnded += OnSaleEnded;
            
            UpdateScrapDisplay(economyManager.GetCurrentScrap());
            Debug.Log("Shop initialized");
        }
        
        /// <summary>
        /// Настройка кнопок
        /// </summary>
        private void SetupButtons()
        {
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(CloseShop);
            }
            
            if (refreshButton != null)
            {
                refreshButton.onClick.AddListener(RefreshShop);
            }
            
            if (randomSaleButton != null)
            {
                randomSaleButton.onClick.AddListener(TryRandomSale);
            }
        }
        
        /// <summary>
        /// Настройка вкладок категорий
        /// </summary>
        private void SetupCategoryTabs()
        {
            if (blocksTab != null)
            {
                blocksTab.onClick.AddListener(() => SwitchCategory("Blocks"));
            }
            
            if (wheelsTab != null)
            {
                wheelsTab.onClick.AddListener(() => SwitchCategory("Wheels"));
            }
            
            if (motorsTab != null)
            {
                motorsTab.onClick.AddListener(() => SwitchCategory("Motors"));
            }
            
            if (jointsTab != null)
            {
                jointsTab.onClick.AddListener(() => SwitchCategory("Joints"));
            }
            
            if (seatsTab != null)
            {
                seatsTab.onClick.AddListener(() => SwitchCategory("Seats"));
            }
            
            if (toolsTab != null)
            {
                toolsTab.onClick.AddListener(() => SwitchCategory("Tools"));
            }
            
            if (allTab != null)
            {
                allTab.onClick.AddListener(() => SwitchCategory("All"));
            }
        }
        
        /// <summary>
        /// Показать магазин
        /// </summary>
        public void ShowShop()
        {
            if (shopPanel != null)
            {
                shopPanel.SetActive(true);
                RefreshShop();
            }
        }
        
        /// <summary>
        /// Скрыть магазин
        /// </summary>
        public void CloseShop()
        {
            if (shopPanel != null)
            {
                shopPanel.SetActive(false);
            }
        }
        
        /// <summary>
        /// Обновить магазин
        /// </summary>
        public void RefreshShop()
        {
            if (economyManager == null) return;
            
            ClearShopItems();
            PopulateShopItems();
            PlayRefreshSound();
            Debug.Log("Shop refreshed");
        }
        
        /// <summary>
        /// Очистить элементы магазина
        /// </summary>
        private void ClearShopItems()
        {
            foreach (ShopItem item in shopItems)
            {
                if (item != null)
                {
                    Destroy(item.gameObject);
                }
            }
            shopItems.Clear();
        }
        
        /// <summary>
        /// Заполнить магазин предметами
        /// </summary>
        private void PopulateShopItems()
        {
            if (shopContent == null || shopItemPrefab == null) return;
            
            List<string> availableItems = economyManager.GetAvailableItems();
            
            foreach (string itemID in availableItems)
            {
                if (ShouldShowItem(itemID))
                {
                    CreateShopItem(itemID);
                }
            }
        }
        
        /// <summary>
        /// Проверить, нужно ли показывать предмет
        /// </summary>
        private bool ShouldShowItem(string itemID)
        {
            if (currentCategory == "All") return true;
            
            string category = GetItemCategory(itemID);
            return category == currentCategory;
        }
        
        /// <summary>
        /// Получить категорию предмета
        /// </summary>
        private string GetItemCategory(string itemID)
        {
            if (itemID.Contains("Block")) return "Blocks";
            if (itemID.Contains("Wheel")) return "Wheels";
            if (itemID.Contains("Motor")) return "Motors";
            if (itemID.Contains("Joint")) return "Joints";
            if (itemID.Contains("Seat")) return "Seats";
            if (itemID.Contains("Tool") || itemID.Contains("Hammer") || itemID.Contains("Wrench") || 
                itemID.Contains("Drill") || itemID.Contains("Welder") || itemID.Contains("Magnet") || 
                itemID.Contains("Vacuum") || itemID.Contains("Sprayer"))
                return "Tools";
            
            return "Other";
        }
        
        /// <summary>
        /// Создать элемент магазина
        /// </summary>
        private void CreateShopItem(string itemID)
        {
            GameObject itemObject = Instantiate(shopItemPrefab, shopContent);
            ShopItem shopItem = itemObject.GetComponent<ShopItem>();
            
            if (shopItem != null)
            {
                shopItem.Initialize(itemID, economyManager);
                shopItems.Add(shopItem);
            }
        }
        
        /// <summary>
        /// Переключить категорию
        /// </summary>
        private void SwitchCategory(string category)
        {
            currentCategory = category;
            RefreshShop();
            PlayTabSwitchSound();
            Debug.Log($"Switched to category: {category}");
        }
        
        /// <summary>
        /// Попробовать случайную распродажу
        /// </summary>
        private void TryRandomSale()
        {
            if (economyManager != null)
            {
                economyManager.TryRandomSale();
            }
        }
        
        /// <summary>
        /// Обновить отображение скрапа
        /// </summary>
        private void UpdateScrapDisplay(int scrap)
        {
            if (scrapText != null)
            {
                scrapText.text = $"💰 {scrap}";
            }
        }
        
        /// <summary>
        /// Обработчик разблокировки предмета
        /// </summary>
        private void OnItemUnlocked(string itemID)
        {
            RefreshShop();
        }
        
        /// <summary>
        /// Обработчик начала распродажи
        /// </summary>
        private void OnSaleStarted(string itemID, float discount)
        {
            RefreshShop();
        }
        
        /// <summary>
        /// Обработчик окончания распродажи
        /// </summary>
        private void OnSaleEnded(string itemID)
        {
            RefreshShop();
        }
        
        /// <summary>
        /// Получить информацию о магазине
        /// </summary>
        public string GetShopInfo()
        {
            if (economyManager == null) return "Shop not initialized";
            
            int totalItems = economyManager.GetAvailableItems().Count;
            int unlockedItems = economyManager.GetUnlockedItems().Count;
            int currentScrap = economyManager.GetCurrentScrap();
            
            string info = $"Shop Information:\n";
            info += $"Current Scrap: {currentScrap}\n";
            info += $"Total Items: {totalItems}\n";
            info += $"Unlocked Items: {unlockedItems}\n";
            info += $"Current Category: {currentCategory}\n";
            
            return info;
        }
        
        /// <summary>
        /// Фильтровать предметы по цене
        /// </summary>
        public void FilterByPrice(int maxPrice)
        {
            foreach (ShopItem item in shopItems)
            {
                if (item != null)
                {
                    float price = economyManager.GetItemPrice(item.GetItemID());
                    item.gameObject.SetActive(price <= maxPrice);
                }
            }
        }
        
        /// <summary>
        /// Показать только предметы со скидкой
        /// </summary>
        public void ShowOnlySaleItems()
        {
            foreach (ShopItem item in shopItems)
            {
                if (item != null)
                {
                    // Проверить, есть ли скидка (это можно сделать через EconomyManager)
                    item.gameObject.SetActive(true); // Пока что показываем все
                }
            }
        }
        
        /// <summary>
        /// Показать все предметы
        /// </summary>
        public void ShowAllItems()
        {
            foreach (ShopItem item in shopItems)
            {
                if (item != null)
                {
                    item.gameObject.SetActive(true);
                }
            }
        }
        
        #region Audio Methods
        
        private void PlayTabSwitchSound()
        {
            if (tabSwitchSound != null)
            {
                AudioSource.PlayClipAtPoint(tabSwitchSound, Camera.main.transform.position);
            }
        }
        
        private void PlayRefreshSound()
        {
            if (refreshSound != null)
            {
                AudioSource.PlayClipAtPoint(refreshSound, Camera.main.transform.position);
            }
        }
        
        #endregion
        
        private void OnDestroy()
        {
            if (economyManager != null)
            {
                economyManager.OnScrapChanged -= UpdateScrapDisplay;
                economyManager.OnItemUnlocked -= OnItemUnlocked;
                economyManager.OnSaleStarted -= OnSaleStarted;
                economyManager.OnSaleEnded -= OnSaleEnded;
            }
        }
    }
}
