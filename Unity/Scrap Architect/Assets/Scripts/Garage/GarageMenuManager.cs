using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ScrapArchitect.Garage
{
    public class GarageMenuManager : MonoBehaviour
    {
        [Header("Menu Panels")]
        public GameObject workbenchMenu;
        public GameObject computerMenu;
        public GameObject bulletinBoardMenu;
        public GameObject safeMenu;
        public GameObject doorMenu;
        
        [Header("UI Elements")]
        public GameObject menuCanvas;
        public Button closeButton;
        
        [Header("Menu Content")]
        public Transform menuContentParent;
        
        // Private variables
        private GameObject currentMenu = null;
        private bool isMenuOpen = false;
        
        void Start()
        {
            InitializeMenus();
            SetupCloseButton();
        }
        
        void InitializeMenus()
        {
            // Создаем Canvas для меню если его нет
            if (menuCanvas == null)
            {
                CreateMenuCanvas();
            }
            
            // Создаем меню если их нет
            if (workbenchMenu == null) CreateWorkbenchMenu();
            if (computerMenu == null) CreateComputerMenu();
            if (bulletinBoardMenu == null) CreateBulletinBoardMenu();
            if (safeMenu == null) CreateSafeMenu();
            if (doorMenu == null) CreateDoorMenu();
            
            // Скрываем все меню по умолчанию
            HideAllMenus();
        }
        
        void CreateMenuCanvas()
        {
            menuCanvas = new GameObject("MenuCanvas");
            Canvas canvas = menuCanvas.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 10; // Поверх всего
            
            menuCanvas.AddComponent<CanvasScaler>();
            menuCanvas.AddComponent<GraphicRaycaster>();
            
            // Создаем панель для контента меню
            GameObject contentPanel = new GameObject("MenuContentPanel");
            contentPanel.transform.SetParent(menuCanvas.transform, false);
            
            Image panelImage = contentPanel.AddComponent<Image>();
            panelImage.color = new Color(0, 0, 0, 0.9f);
            
            RectTransform panelRect = contentPanel.GetComponent<RectTransform>();
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.one;
            panelRect.sizeDelta = Vector2.zero;
            panelRect.anchoredPosition = Vector2.zero;
            
            menuContentParent = contentPanel.transform;
        }
        
        void CreateWorkbenchMenu()
        {
            workbenchMenu = CreateMenuPanel("WorkbenchMenu", "Верстак - Контракты");
            
            // Добавляем кнопки контрактов
            AddMenuButton(workbenchMenu, "Контракт 1: Простой механизм", () => Debug.Log("Открыт контракт 1"));
            AddMenuButton(workbenchMenu, "Контракт 2: Сложная система", () => Debug.Log("Открыт контракт 2"));
            AddMenuButton(workbenchMenu, "Контракт 3: Экспериментальный", () => Debug.Log("Открыт контракт 3"));
            AddMenuButton(workbenchMenu, "Создать новый проект", () => Debug.Log("Создание нового проекта"));
        }
        
        void CreateComputerMenu()
        {
            computerMenu = CreateMenuPanel("ComputerMenu", "Компьютер - Steam Workshop");
            
            // Добавляем кнопки Steam Workshop
            AddMenuButton(computerMenu, "Популярные проекты", () => Debug.Log("Популярные проекты"));
            AddMenuButton(computerMenu, "Мои загрузки", () => Debug.Log("Мои загрузки"));
            AddMenuButton(computerMenu, "Загрузить проект", () => Debug.Log("Загрузка проекта"));
            AddMenuButton(computerMenu, "Поделиться проектом", () => Debug.Log("Поделиться проектом"));
        }
        
        void CreateBulletinBoardMenu()
        {
            bulletinBoardMenu = CreateMenuPanel("BulletinBoardMenu", "Доска объявлений");
            
            // Добавляем список контрактов
            AddMenuButton(bulletinBoardMenu, "Активные контракты", () => Debug.Log("Активные контракты"));
            AddMenuButton(bulletinBoardMenu, "Завершенные проекты", () => Debug.Log("Завершенные проекты"));
            AddMenuButton(bulletinBoardMenu, "Рейтинг мастеров", () => Debug.Log("Рейтинг мастеров"));
            AddMenuButton(bulletinBoardMenu, "Новости Workshop", () => Debug.Log("Новости Workshop"));
        }
        
        void CreateSafeMenu()
        {
            safeMenu = CreateMenuPanel("SafeMenu", "Сейф - Магазин деталей");
            
            // Добавляем категории деталей
            AddMenuButton(safeMenu, "Механические детали", () => Debug.Log("Механические детали"));
            AddMenuButton(safeMenu, "Электронные компоненты", () => Debug.Log("Электронные компоненты"));
            AddMenuButton(safeMenu, "Пневматические системы", () => Debug.Log("Пневматические системы"));
            AddMenuButton(safeMenu, "Гидравлические системы", () => Debug.Log("Гидравлические системы"));
            AddMenuButton(safeMenu, "Инструменты", () => Debug.Log("Инструменты"));
        }
        
        void CreateDoorMenu()
        {
            doorMenu = CreateMenuPanel("DoorMenu", "Выход на полигон");
            
            // Добавляем опции выхода
            AddMenuButton(doorMenu, "Тестовый полигон", () => Debug.Log("Переход на тестовый полигон"));
            AddMenuButton(doorMenu, "Полигон испытаний", () => Debug.Log("Переход на полигон испытаний"));
            AddMenuButton(doorMenu, "Тренировочная зона", () => Debug.Log("Переход в тренировочную зону"));
            AddMenuButton(doorMenu, "Отмена", () => CloseCurrentMenu());
        }
        
        GameObject CreateMenuPanel(string name, string title)
        {
            GameObject menuPanel = new GameObject(name);
            menuPanel.transform.SetParent(menuContentParent, false);
            
            // Создаем фон панели
            Image panelImage = menuPanel.AddComponent<Image>();
            panelImage.color = new Color(0.1f, 0.1f, 0.1f, 0.95f);
            
            RectTransform panelRect = menuPanel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.1f, 0.1f);
            panelRect.anchorMax = new Vector2(0.9f, 0.9f);
            panelRect.sizeDelta = Vector2.zero;
            panelRect.anchoredPosition = Vector2.zero;
            
            // Создаем заголовок
            GameObject titleObj = new GameObject("Title");
            titleObj.transform.SetParent(menuPanel.transform, false);
            
            Text titleText = titleObj.AddComponent<Text>();
            titleText.text = title;
            titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            titleText.fontSize = 24;
            titleText.color = Color.white;
            titleText.alignment = TextAnchor.MiddleCenter;
            
            RectTransform titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0, 0.8f);
            titleRect.anchorMax = new Vector2(1, 1);
            titleRect.sizeDelta = Vector2.zero;
            titleRect.anchoredPosition = Vector2.zero;
            
            // Создаем контейнер для кнопок
            GameObject buttonContainer = new GameObject("ButtonContainer");
            buttonContainer.transform.SetParent(menuPanel.transform, false);
            
            RectTransform containerRect = buttonContainer.GetComponent<RectTransform>();
            containerRect.anchorMin = new Vector2(0.1f, 0.1f);
            containerRect.anchorMax = new Vector2(0.9f, 0.8f);
            containerRect.sizeDelta = Vector2.zero;
            containerRect.anchoredPosition = Vector2.zero;
            
            // Добавляем VerticalLayoutGroup для кнопок
            VerticalLayoutGroup layoutGroup = buttonContainer.AddComponent<VerticalLayoutGroup>();
            layoutGroup.spacing = 10;
            layoutGroup.padding = new RectOffset(20, 20, 20, 20);
            layoutGroup.childControlHeight = false;
            layoutGroup.childControlWidth = true;
            layoutGroup.childForceExpandHeight = false;
            layoutGroup.childForceExpandWidth = true;
            
            return menuPanel;
        }
        
        void AddMenuButton(GameObject menuPanel, string text, System.Action onClick)
        {
            GameObject buttonObj = new GameObject("Button_" + text.Replace(" ", "_"));
            buttonObj.transform.SetParent(menuPanel.transform.Find("ButtonContainer"), false);
            
            // Создаем фон кнопки
            Image buttonImage = buttonObj.AddComponent<Image>();
            buttonImage.color = new Color(0.2f, 0.2f, 0.2f, 1f);
            
            // Добавляем Button компонент
            Button button = buttonObj.AddComponent<Button>();
            button.onClick.AddListener(() => onClick?.Invoke());
            
            // Создаем текст кнопки
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform, false);
            
            Text buttonText = textObj.AddComponent<Text>();
            buttonText.text = text;
            buttonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            buttonText.fontSize = 16;
            buttonText.color = Color.white;
            buttonText.alignment = TextAnchor.MiddleCenter;
            
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            textRect.anchoredPosition = Vector2.zero;
            
            // Настраиваем размер кнопки
            RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
            buttonRect.sizeDelta = new Vector2(0, 50);
        }
        
        void SetupCloseButton()
        {
            if (closeButton == null)
            {
                // Создаем кнопку закрытия
                GameObject closeButtonObj = new GameObject("CloseButton");
                closeButtonObj.transform.SetParent(menuContentParent, false);
                
                Image closeImage = closeButtonObj.AddComponent<Image>();
                closeImage.color = new Color(0.8f, 0.2f, 0.2f, 1f);
                
                closeButton = closeButtonObj.AddComponent<Button>();
                closeButton.onClick.AddListener(CloseCurrentMenu);
                
                RectTransform closeRect = closeButtonObj.GetComponent<RectTransform>();
                closeRect.anchorMin = new Vector2(0.9f, 0.9f);
                closeRect.anchorMax = new Vector2(0.98f, 0.98f);
                closeRect.sizeDelta = Vector2.zero;
                closeRect.anchoredPosition = Vector2.zero;
                
                // Текст кнопки
                GameObject closeTextObj = new GameObject("CloseText");
                closeTextObj.transform.SetParent(closeButtonObj.transform, false);
                
                Text closeText = closeTextObj.AddComponent<Text>();
                closeText.text = "X";
                closeText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                closeText.fontSize = 20;
                closeText.color = Color.white;
                closeText.alignment = TextAnchor.MiddleCenter;
                
                RectTransform closeTextRect = closeTextObj.GetComponent<RectTransform>();
                closeTextRect.anchorMin = Vector2.zero;
                closeTextRect.anchorMax = Vector2.one;
                closeTextRect.sizeDelta = Vector2.zero;
                closeTextRect.anchoredPosition = Vector2.zero;
            }
        }
        
        public void OpenMenu(ZoneType zoneType)
        {
            CloseCurrentMenu();
            
            switch (zoneType)
            {
                case ZoneType.Workbench:
                    currentMenu = workbenchMenu;
                    break;
                case ZoneType.Computer:
                    currentMenu = computerMenu;
                    break;
                case ZoneType.BulletinBoard:
                    currentMenu = bulletinBoardMenu;
                    break;
                case ZoneType.Safe:
                    currentMenu = safeMenu;
                    break;
                case ZoneType.Door:
                    currentMenu = doorMenu;
                    break;
            }
            
            if (currentMenu != null)
            {
                currentMenu.SetActive(true);
                isMenuOpen = true;
                
                // Находим GarageManager и блокируем движение
                GarageManager garageManager = FindObjectOfType<GarageManager>();
                if (garageManager != null)
                {
                    garageManager.SetInteractionMode(true);
                }
            }
        }
        
        public void CloseCurrentMenu()
        {
            if (currentMenu != null)
            {
                currentMenu.SetActive(false);
                currentMenu = null;
                isMenuOpen = false;
                
                // Находим GarageManager и разблокируем движение
                GarageManager garageManager = FindObjectOfType<GarageManager>();
                if (garageManager != null)
                {
                    garageManager.SetInteractionMode(false);
                }
            }
        }
        
        void HideAllMenus()
        {
            if (workbenchMenu != null) workbenchMenu.SetActive(false);
            if (computerMenu != null) computerMenu.SetActive(false);
            if (bulletinBoardMenu != null) bulletinBoardMenu.SetActive(false);
            if (safeMenu != null) safeMenu.SetActive(false);
            if (doorMenu != null) doorMenu.SetActive(false);
        }
        
        public bool IsMenuOpen()
        {
            return isMenuOpen;
        }
    }
}
