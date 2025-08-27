using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace ScrapArchitect.Garage.Editor
{
    public class SimpleGarageIntegrator : EditorWindow
    {
        [MenuItem("Scrap Architect/Garage/Integrate Simple Garage")]
        public static void IntegrateSimpleGarage()
        {
            Debug.Log("Интегрируем Simple Garage в наш проект...");
            
            // Загружаем готовую сцену
            LoadSimpleGarageScene();
            
            // Добавляем игрока
            AddPlayerToScene();
            
            // Создаем недостающие объекты
            CreateMissingObjects();
            
            // Добавляем интерактивные зоны
            AddInteractiveZones();
            
            // Создаем UI
            CreateUI();
            
            // Создаем менеджер меню
            CreateMenuManager();
            
            Debug.Log("Simple Garage успешно интегрирован!");
        }
        
        static void LoadSimpleGarageScene()
        {
            // Загружаем готовую сцену
            string scenePath = "Assets/Simple Garage/Scenes/Garage Scene.unity";
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath) != null)
            {
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                Debug.Log("Сцена Simple Garage загружена");
            }
            else
            {
                Debug.LogError("Сцена Simple Garage не найдена!");
                return;
            }
        }
        
        static void AddPlayerToScene()
        {
            // Проверяем, есть ли уже игрок
            GameObject existingPlayer = GameObject.Find("Player");
            if (existingPlayer != null)
            {
                Debug.Log("Игрок уже существует в сцене");
                return;
            }
            
            // Создаем игрока
            GameObject player = new GameObject("Player");
            player.transform.position = new Vector3(0, 1f, 0);
            
            // Добавляем CharacterController
            CharacterController controller = player.AddComponent<CharacterController>();
            controller.height = 2f;
            controller.radius = 0.5f;
            
            // Добавляем камеру
            GameObject camera = new GameObject("PlayerCamera");
            camera.transform.SetParent(player.transform);
            camera.transform.localPosition = new Vector3(0, 1.8f, 0);
            
            Camera cam = camera.AddComponent<Camera>();
            cam.fieldOfView = 60f;
            cam.nearClipPlane = 0.1f;
            cam.farClipPlane = 1000f;
            
            // Добавляем GarageManager
            GarageManager garageManager = player.AddComponent<GarageManager>();
            garageManager.playerCamera = cam;
            
            // Устанавливаем как главную камеру
            camera.tag = "MainCamera";
            
            Debug.Log("Игрок добавлен в сцену");
        }
        
        static void CreateMissingObjects()
        {
            // Создаем компьютер
            CreateComputer();
            
            // Создаем сейф
            CreateSafe();
            
            // Создаем доску объявлений
            CreateBulletinBoard();
            
            Debug.Log("Недостающие объекты созданы");
        }
        
        static void CreateComputer()
        {
            // Проверяем, есть ли уже компьютер
            if (GameObject.Find("ComputerZone") != null)
            {
                Debug.Log("Компьютер уже существует");
                return;
            }
            
            // Создаем компьютер
            GameObject computer = new GameObject("ComputerZone");
            computer.transform.position = new Vector3(3f, 0f, -2f);
            
            // Монитор
            GameObject monitor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            monitor.name = "Monitor";
            monitor.transform.SetParent(computer.transform);
            monitor.transform.localPosition = Vector3.up * 1.2f;
            monitor.transform.localScale = new Vector3(0.8f, 0.6f, 0.1f);
            
            // Клавиатура
            GameObject keyboard = GameObject.CreatePrimitive(PrimitiveType.Cube);
            keyboard.name = "Keyboard";
            keyboard.transform.SetParent(computer.transform);
            keyboard.transform.localPosition = Vector3.up * 0.8f;
            keyboard.transform.localScale = new Vector3(0.6f, 0.05f, 0.2f);
            
            // Материал для компьютера
            Material computerMaterial = new Material(Shader.Find("Diffuse"));
            if (computerMaterial == null) computerMaterial = new Material(Shader.Find("Unlit/Color"));
            computerMaterial.color = new Color(0.7f, 0.7f, 0.7f);
            
            monitor.GetComponent<Renderer>().material = computerMaterial;
            keyboard.GetComponent<Renderer>().material = computerMaterial;
            
            Debug.Log("Компьютер создан");
        }
        
        static void CreateSafe()
        {
            // Проверяем, есть ли уже сейф
            if (GameObject.Find("SafeZone") != null)
            {
                Debug.Log("Сейф уже существует");
                return;
            }
            
            // Создаем сейф
            GameObject safe = new GameObject("SafeZone");
            safe.transform.position = new Vector3(8f, 0f, 0f);
            
            // Корпус сейфа
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
            body.name = "Body";
            body.transform.SetParent(safe.transform);
            body.transform.localPosition = Vector3.up * 0.5f;
            body.transform.localScale = new Vector3(0.8f, 1f, 0.6f);
            
            // Материал для сейфа
            Material safeMaterial = new Material(Shader.Find("Diffuse"));
            if (safeMaterial == null) safeMaterial = new Material(Shader.Find("Unlit/Color"));
            safeMaterial.color = new Color(0.7f, 0.7f, 0.7f);
            
            body.GetComponent<Renderer>().material = safeMaterial;
            
            Debug.Log("Сейф создан");
        }
        
        static void CreateBulletinBoard()
        {
            // Проверяем, есть ли уже доска
            if (GameObject.Find("BulletinBoardZone") != null)
            {
                Debug.Log("Доска объявлений уже существует");
                return;
            }
            
            // Создаем доску объявлений
            GameObject board = new GameObject("BulletinBoardZone");
            board.transform.position = new Vector3(-8f, 0f, 0f);
            
            // Доска
            GameObject mainBoard = GameObject.CreatePrimitive(PrimitiveType.Cube);
            mainBoard.name = "MainBoard";
            mainBoard.transform.SetParent(board.transform);
            mainBoard.transform.localPosition = Vector3.up * 1.5f;
            mainBoard.transform.localScale = new Vector3(1.5f, 1f, 0.05f);
            
            // Материал для доски
            Material boardMaterial = new Material(Shader.Find("Diffuse"));
            if (boardMaterial == null) boardMaterial = new Material(Shader.Find("Unlit/Color"));
            boardMaterial.color = new Color(0.5f, 0.3f, 0.2f);
            
            mainBoard.GetComponent<Renderer>().material = boardMaterial;
            
            Debug.Log("Доска объявлений создана");
        }
        
        static void AddInteractiveZones()
        {
            // Находим существующие объекты и добавляем к ним зоны
            AddZoneToExistingObject("Table", "Верстак", "Нажмите E для доступа к контрактам", ZoneType.Workbench);
            AddZoneToExistingObject("ComputerZone", "Компьютер", "Нажмите E для Steam Workshop", ZoneType.Computer);
            AddZoneToExistingObject("BulletinBoardZone", "Доска объявлений", "Нажмите E для просмотра контрактов", ZoneType.BulletinBoard);
            AddZoneToExistingObject("SafeZone", "Сейф", "Нажмите E для магазина деталей", ZoneType.Safe);
            AddZoneToExistingObject("Garage door", "Дверь", "Нажмите E для выхода на полигон", ZoneType.Door);
            
            Debug.Log("Интерактивные зоны добавлены");
        }
        
        static void AddZoneToExistingObject(string objectName, string zoneName, string interactionText, ZoneType zoneType)
        {
            GameObject obj = GameObject.Find(objectName);
            if (obj != null)
            {
                // Проверяем, есть ли уже GarageZone
                if (obj.GetComponent<GarageZone>() == null)
                {
                    // Добавляем GarageZone компонент
                    GarageZone zone = obj.AddComponent<GarageZone>();
                    zone.Initialize(zoneName, interactionText, zoneType);
                    zone.interactionRange = 3f;
                    
                    Debug.Log($"Добавлена зона {zoneName} к объекту {objectName}");
                }
                else
                {
                    Debug.Log($"Зона уже существует на объекте {objectName}");
                }
            }
            else
            {
                Debug.LogWarning($"Объект {objectName} не найден!");
            }
        }
        
        static void CreateUI()
        {
            // Проверяем, есть ли уже UI
            if (GameObject.Find("GarageUI") != null)
            {
                Debug.Log("UI уже существует");
                return;
            }
            
            // Создаем Canvas
            GameObject canvas = new GameObject("GarageUI");
            Canvas canvasComponent = canvas.AddComponent<Canvas>();
            canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvas.AddComponent<UnityEngine.UI.GraphicRaycaster>();
            
            // Создаем панель взаимодействия
            GameObject interactionPanel = new GameObject("InteractionPanel");
            interactionPanel.transform.SetParent(canvas.transform, false);
            
            UnityEngine.UI.Image panelImage = interactionPanel.AddComponent<UnityEngine.UI.Image>();
            panelImage.color = new Color(0, 0, 0, 0.8f);
            
            RectTransform panelRect = interactionPanel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0);
            panelRect.anchorMax = new Vector2(0.5f, 0);
            panelRect.pivot = new Vector2(0.5f, 0);
            panelRect.sizeDelta = new Vector2(400, 60);
            panelRect.anchoredPosition = new Vector2(0, 50);
            
            // Создаем текст взаимодействия
            GameObject interactionText = new GameObject("InteractionText");
            interactionText.transform.SetParent(interactionPanel.transform, false);
            
            UnityEngine.UI.Text textComponent = interactionText.AddComponent<UnityEngine.UI.Text>();
            textComponent.text = "Нажмите E для взаимодействия";
            textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            textComponent.fontSize = 18;
            textComponent.color = Color.white;
            textComponent.alignment = TextAnchor.MiddleCenter;
            
            RectTransform textRect = interactionText.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            textRect.anchoredPosition = Vector2.zero;
            
            // Скрываем панель по умолчанию
            interactionPanel.SetActive(false);
            
            // Находим GarageManager и подключаем UI
            GarageManager garageManager = FindObjectOfType<GarageManager>();
            if (garageManager != null)
            {
                garageManager.interactionUI = interactionPanel;
                garageManager.interactionText = textComponent;
            }
            
            Debug.Log("UI создан");
        }
        
        static void CreateMenuManager()
        {
            // Проверяем, есть ли уже менеджер меню
            if (FindObjectOfType<GarageMenuManager>() != null)
            {
                Debug.Log("Менеджер меню уже существует");
                return;
            }
            
            // Создаем менеджер меню
            GameObject menuManagerObj = new GameObject("GarageMenuManager");
            GarageMenuManager menuManager = menuManagerObj.AddComponent<GarageMenuManager>();
            
            // Подключаем к GarageManager
            GarageManager garageManager = FindObjectOfType<GarageManager>();
            if (garageManager != null)
            {
                garageManager.menuManager = menuManager;
            }
            
            Debug.Log("Менеджер меню создан и подключен");
        }
        
        [MenuItem("Scrap Architect/Garage/Add Player to Simple Garage")]
        public static void AddPlayerOnly()
        {
            AddPlayerToScene();
            CreateUI();
            CreateMenuManager();
            Debug.Log("Игрок добавлен в Simple Garage!");
        }
        
        [MenuItem("Scrap Architect/Garage/Add Missing Objects to Simple Garage")]
        public static void AddMissingObjectsOnly()
        {
            CreateMissingObjects();
            AddInteractiveZones();
            Debug.Log("Недостающие объекты добавлены в Simple Garage!");
        }
    }
}
