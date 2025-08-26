using UnityEngine;
using System;
using System.Collections.Generic;

namespace ScrapArchitect.Economy
{
    /// <summary>
    /// Менеджер экономики - управляет валютой и покупками
    /// </summary>
    public class EconomyManager : MonoBehaviour
    {
        [Header("Economy Settings")]
        public int startingScrap = 1000;
        public int maxScrap = 999999;
        public bool enableInfiniteMoney = false;
        
        [Header("Shop Settings")]
        public float discountMultiplier = 0.8f;
        public bool enableSales = true;
        public float saleChance = 0.1f; // 10% шанс скидки
        public float saleDuration = 300f; // 5 минут
        
        [Header("Audio Settings")]
        public AudioClip purchaseSound;
        public AudioClip insufficientFundsSound;
        public AudioClip saleStartSound;
        public AudioClip saleEndSound;
        
        private int currentScrap;
        private Dictionary<string, float> itemPrices = new Dictionary<string, float>();
        private Dictionary<string, bool> itemUnlocked = new Dictionary<string, bool>();
        private Dictionary<string, float> activeSales = new Dictionary<string, float>();
        
        // Events
        public Action<int> OnScrapChanged;
        public Action<string, int> OnItemPurchased;
        public Action<string> OnItemUnlocked;
        public Action<string, float> OnSaleStarted;
        public Action<string> OnSaleEnded;
        
        private void Start()
        {
            InitializeEconomy();
        }
        
        private void Update()
        {
            UpdateSales();
        }
        
        /// <summary>
        /// Инициализация экономики
        /// </summary>
        private void InitializeEconomy()
        {
            currentScrap = startingScrap;
            
            // Инициализировать базовые цены деталей
            InitializeBasePrices();
            
            // Разблокировать базовые детали
            UnlockBasicParts();
            
            OnScrapChanged?.Invoke(currentScrap);
            Debug.Log($"Economy initialized with {currentScrap} scrap");
        }
        
        /// <summary>
        /// Инициализация базовых цен
        /// </summary>
        private void InitializeBasePrices()
        {
            // Блоки
            itemPrices["WoodBlock"] = 50;
            itemPrices["MetalBlock"] = 100;
            itemPrices["PlasticBlock"] = 75;
            itemPrices["StoneBlock"] = 80;
            
            // Колеса
            itemPrices["SmallWheel"] = 150;
            itemPrices["MediumWheel"] = 250;
            itemPrices["LargeWheel"] = 400;
            itemPrices["OffRoadWheel"] = 500;
            
            // Двигатели
            itemPrices["ElectricMotor"] = 300;
            itemPrices["GasolineMotor"] = 500;
            itemPrices["DieselMotor"] = 800;
            itemPrices["JetMotor"] = 1500;
            
            // Соединения
            itemPrices["FixedJoint"] = 25;
            itemPrices["HingeJoint"] = 50;
            itemPrices["SpringJoint"] = 75;
            itemPrices["SliderJoint"] = 100;
            itemPrices["ConfigurableJoint"] = 150;
            
            // Сиденья
            itemPrices["BasicSeat"] = 200;
            itemPrices["ComfortSeat"] = 400;
            itemPrices["RacingSeat"] = 600;
            itemPrices["LuxurySeat"] = 1000;
            itemPrices["IndustrialSeat"] = 800;
            
            // Инструменты
            itemPrices["Hammer"] = 100;
            itemPrices["Wrench"] = 150;
            itemPrices["Screwdriver"] = 75;
            itemPrices["Drill"] = 300;
            itemPrices["Welder"] = 500;
            itemPrices["Magnet"] = 200;
            itemPrices["Vacuum"] = 400;
            itemPrices["Sprayer"] = 250;
        }
        
        /// <summary>
        /// Разблокировать базовые детали
        /// </summary>
        private void UnlockBasicParts()
        {
            string[] basicParts = {
                "WoodBlock", "SmallWheel", "ElectricMotor", 
                "FixedJoint", "BasicSeat", "Hammer"
            };
            
            foreach (string part in basicParts)
            {
                itemUnlocked[part] = true;
                OnItemUnlocked?.Invoke(part);
            }
        }
        
        /// <summary>
        /// Получить текущий баланс скрапа
        /// </summary>
        public int GetCurrentScrap()
        {
            return currentScrap;
        }
        
        /// <summary>
        /// Добавить скрап
        /// </summary>
        public bool AddScrap(int amount)
        {
            if (amount <= 0) return false;
            
            int newAmount = Mathf.Min(currentScrap + amount, maxScrap);
            int actualAdded = newAmount - currentScrap;
            
            if (actualAdded > 0)
            {
                currentScrap = newAmount;
                OnScrapChanged?.Invoke(currentScrap);
                Debug.Log($"Added {actualAdded} scrap. New balance: {currentScrap}");
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Потратить скрап
        /// </summary>
        public bool SpendScrap(int amount)
        {
            if (amount <= 0) return false;
            if (enableInfiniteMoney) return true;
            
            if (currentScrap >= amount)
            {
                currentScrap -= amount;
                OnScrapChanged?.Invoke(currentScrap);
                Debug.Log($"Spent {amount} scrap. New balance: {currentScrap}");
                return true;
            }
            else
            {
                PlayInsufficientFundsSound();
                Debug.LogWarning($"Insufficient funds. Need {amount}, have {currentScrap}");
                return false;
            }
        }
        
        /// <summary>
        /// Получить цену предмета
        /// </summary>
        public float GetItemPrice(string itemID)
        {
            if (!itemPrices.ContainsKey(itemID))
            {
                Debug.LogWarning($"Price not found for item: {itemID}");
                return 0f;
            }
            
            float basePrice = itemPrices[itemID];
            
            // Применить скидку, если есть активная распродажа
            if (activeSales.ContainsKey(itemID))
            {
                return basePrice * discountMultiplier;
            }
            
            return basePrice;
        }
        
        /// <summary>
        /// Проверить, разблокирован ли предмет
        /// </summary>
        public bool IsItemUnlocked(string itemID)
        {
            return itemUnlocked.ContainsKey(itemID) && itemUnlocked[itemID];
        }
        
        /// <summary>
        /// Купить предмет
        /// </summary>
        public bool PurchaseItem(string itemID)
        {
            if (!itemPrices.ContainsKey(itemID))
            {
                Debug.LogWarning($"Cannot purchase unknown item: {itemID}");
                return false;
            }
            
            if (IsItemUnlocked(itemID))
            {
                Debug.LogWarning($"Item already unlocked: {itemID}");
                return false;
            }
            
            float price = GetItemPrice(itemID);
            int priceInt = Mathf.RoundToInt(price);
            
            if (SpendScrap(priceInt))
            {
                itemUnlocked[itemID] = true;
                OnItemPurchased?.Invoke(itemID, priceInt);
                OnItemUnlocked?.Invoke(itemID);
                
                PlayPurchaseSound();
                Debug.Log($"Purchased {itemID} for {priceInt} scrap");
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Разблокировать предмет (без траты денег)
        /// </summary>
        public void UnlockItem(string itemID)
        {
            if (!itemUnlocked.ContainsKey(itemID))
            {
                itemUnlocked[itemID] = false;
            }
            
            if (!itemUnlocked[itemID])
            {
                itemUnlocked[itemID] = true;
                OnItemUnlocked?.Invoke(itemID);
                Debug.Log($"Unlocked item: {itemID}");
            }
        }
        
        /// <summary>
        /// Заблокировать предмет
        /// </summary>
        public void LockItem(string itemID)
        {
            if (itemUnlocked.ContainsKey(itemID))
            {
                itemUnlocked[itemID] = false;
                Debug.Log($"Locked item: {itemID}");
            }
        }
        
        /// <summary>
        /// Начать распродажу для предмета
        /// </summary>
        public void StartSale(string itemID)
        {
            if (!itemPrices.ContainsKey(itemID))
            {
                Debug.LogWarning($"Cannot start sale for unknown item: {itemID}");
                return;
            }
            
            if (!enableSales) return;
            
            activeSales[itemID] = Time.time + saleDuration;
            OnSaleStarted?.Invoke(itemID, discountMultiplier);
            PlaySaleStartSound();
            Debug.Log($"Sale started for {itemID}");
        }
        
        /// <summary>
        /// Остановить распродажу для предмета
        /// </summary>
        public void EndSale(string itemID)
        {
            if (activeSales.ContainsKey(itemID))
            {
                activeSales.Remove(itemID);
                OnSaleEnded?.Invoke(itemID);
                PlaySaleEndSound();
                Debug.Log($"Sale ended for {itemID}");
            }
        }
        
        /// <summary>
        /// Обновить распродажи
        /// </summary>
        private void UpdateSales()
        {
            List<string> expiredSales = new List<string>();
            
            foreach (var sale in activeSales)
            {
                if (Time.time >= sale.Value)
                {
                    expiredSales.Add(sale.Key);
                }
            }
            
            foreach (string itemID in expiredSales)
            {
                EndSale(itemID);
            }
        }
        
        /// <summary>
        /// Случайно начать распродажу
        /// </summary>
        public void TryRandomSale()
        {
            if (!enableSales) return;
            
            if (UnityEngine.Random.Range(0f, 1f) < saleChance)
            {
                List<string> availableItems = new List<string>();
                
                foreach (var item in itemPrices)
                {
                    if (!IsItemUnlocked(item.Key) && !activeSales.ContainsKey(item.Key))
                    {
                        availableItems.Add(item.Key);
                    }
                }
                
                if (availableItems.Count > 0)
                {
                    string randomItem = availableItems[UnityEngine.Random.Range(0, availableItems.Count)];
                    StartSale(randomItem);
                }
            }
        }
        
        /// <summary>
        /// Получить информацию о предмете
        /// </summary>
        public string GetItemInfo(string itemID)
        {
            if (!itemPrices.ContainsKey(itemID))
            {
                return $"Unknown item: {itemID}";
            }
            
            float basePrice = itemPrices[itemID];
            float currentPrice = GetItemPrice(itemID);
            bool isUnlocked = IsItemUnlocked(itemID);
            bool hasSale = activeSales.ContainsKey(itemID);
            
            string info = $"Item: {itemID}\n";
            info += $"Base Price: {basePrice} scrap\n";
            info += $"Current Price: {currentPrice} scrap\n";
            info += $"Status: {(isUnlocked ? "Unlocked" : "Locked")}\n";
            
            if (hasSale)
            {
                float timeLeft = activeSales[itemID] - Time.time;
                info += $"Sale: Active ({timeLeft:F0}s remaining)\n";
            }
            
            return info;
        }
        
        /// <summary>
        /// Получить все доступные предметы
        /// </summary>
        public List<string> GetAvailableItems()
        {
            List<string> items = new List<string>();
            
            foreach (var item in itemPrices)
            {
                if (!IsItemUnlocked(item.Key))
                {
                    items.Add(item.Key);
                }
            }
            
            return items;
        }
        
        /// <summary>
        /// Получить все разблокированные предметы
        /// </summary>
        public List<string> GetUnlockedItems()
        {
            List<string> items = new List<string>();
            
            foreach (var item in itemUnlocked)
            {
                if (item.Value)
                {
                    items.Add(item.Key);
                }
            }
            
            return items;
        }
        
        /// <summary>
        /// Сбросить прогресс (для тестирования)
        /// </summary>
        public void ResetProgress()
        {
            currentScrap = startingScrap;
            itemUnlocked.Clear();
            activeSales.Clear();
            
            InitializeBasePrices();
            UnlockBasicParts();
            
            OnScrapChanged?.Invoke(currentScrap);
            Debug.Log("Economy progress reset");
        }
        
        /// <summary>
        /// Установить бесконечные деньги (для тестирования)
        /// </summary>
        public void SetInfiniteMoney(bool enabled)
        {
            enableInfiniteMoney = enabled;
            Debug.Log($"Infinite money: {(enabled ? "enabled" : "disabled")}");
        }
        
        #region Audio Methods
        
        private void PlayPurchaseSound()
        {
            if (purchaseSound != null)
            {
                AudioSource.PlayClipAtPoint(purchaseSound, Camera.main.transform.position);
            }
        }
        
        private void PlayInsufficientFundsSound()
        {
            if (insufficientFundsSound != null)
            {
                AudioSource.PlayClipAtPoint(insufficientFundsSound, Camera.main.transform.position);
            }
        }
        
        private void PlaySaleStartSound()
        {
            if (saleStartSound != null)
            {
                AudioSource.PlayClipAtPoint(saleStartSound, Camera.main.transform.position);
            }
        }
        
        private void PlaySaleEndSound()
        {
            if (saleEndSound != null)
            {
                AudioSource.PlayClipAtPoint(saleEndSound, Camera.main.transform.position);
            }
        }
        
        #endregion
    }
}
