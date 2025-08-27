using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace ScrapArchitect.Garage.Editor
{
    public class GarageSetupTool : EditorWindow
    {
        [MenuItem("Scrap Architect/Garage/Create Garage Hub")]
        public static void CreateGarageHub()
        {
            // Создаем новую сцену
            CreateNewScene();
            
            // Создаем материалы
            CreateMaterials();
            
            // Создаем структуру гаража
            CreateGarageStructure();
            
            // Создаем игрока
            CreatePlayer();
            
            // Создаем освещение
            CreateLighting();
            
            // Создаем функциональные зоны
            CreateFunctionalZones();
            
            // Создаем UI
            CreateUI();
            
            Debug.Log("Гараж успешно создан! Теперь можно настроить детали.");
        }
        
        [MenuItem("Scrap Architect/Garage/Add Interior Details")]
        public static void AddInteriorDetails()
        {
            // Создаем материалы если их нет
            CreateMaterials();
            
            // Загружаем материалы
            Material woodMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Garage/WoodMaterial.mat");
            Material metalMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Garage/MetalMaterial.mat");
            
            if (woodMaterial == null || metalMaterial == null)
            {
                Debug.LogError("Материалы не найдены! Сначала создайте гараж.");
                return;
            }
            
            // Создаем детали интерьера
            CreateInteriorDetails(woodMaterial, metalMaterial);
            
            Debug.Log("Детали интерьера добавлены!");
        }
        
        static void CreateNewScene()
        {
            // Создаем новую сцену
            EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            
            // Сохраняем сцену
            string scenePath = "Assets/Scenes/GarageHub.unity";
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), scenePath);
        }
        
        static void CreateMaterials()
        {
            // Создаем папку для материалов если её нет
            if (!AssetDatabase.IsValidFolder("Assets/Materials/Garage"))
            {
                AssetDatabase.CreateFolder("Assets/Materials", "Garage");
            }
            
            // Создаем материалы
            CreateMaterial("WallMaterial", new Color(0.8f, 0.7f, 0.6f), "Assets/Materials/Garage/WallMaterial.mat");
            CreateMaterial("FloorMaterial", new Color(0.6f, 0.5f, 0.4f), "Assets/Materials/Garage/FloorMaterial.mat");
            CreateMaterial("CeilingMaterial", new Color(0.9f, 0.85f, 0.8f), "Assets/Materials/Garage/CeilingMaterial.mat");
            CreateMaterial("MetalMaterial", new Color(0.7f, 0.7f, 0.7f), "Assets/Materials/Garage/MetalMaterial.mat");
            CreateMaterial("WoodMaterial", new Color(0.5f, 0.3f, 0.2f), "Assets/Materials/Garage/WoodMaterial.mat");
            
            // Дополнительные материалы для деталей
            CreateMaterial("BlueprintMaterial", new Color(0.95f, 0.95f, 0.9f), "Assets/Materials/Garage/BlueprintMaterial.mat");
            CreateMaterial("OilStainMaterial", new Color(0.2f, 0.2f, 0.1f), "Assets/Materials/Garage/OilStainMaterial.mat");
            CreateMaterial("ToolMaterial", new Color(0.4f, 0.4f, 0.4f), "Assets/Materials/Garage/ToolMaterial.mat");
            
            AssetDatabase.Refresh();
        }
        
        static void CreateMaterial(string name, Color color, string path)
        {
            Material material = new Material(Shader.Find("Standard"));
            material.name = name;
            material.color = color;
            AssetDatabase.CreateAsset(material, path);
        }
        
        static void CreateGarageStructure()
        {
            // Загружаем материалы
            Material wallMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Garage/WallMaterial.mat");
            Material floorMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Garage/FloorMaterial.mat");
            Material ceilingMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Garage/CeilingMaterial.mat");
            
            // Создаем структуру гаража (просторный: 20x15x4 метра)
            GameObject garageStructure = GarageObjectCreator.CreateGarageStructure(
                Vector3.zero, 
                new Vector3(20f, 4f, 15f), 
                wallMaterial, 
                floorMaterial, 
                ceilingMaterial
            );
            
            garageStructure.name = "GarageStructure";
        }
        
        static void CreatePlayer()
        {
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
        }
        
        static void CreateLighting()
        {
            // Создаем основное освещение
            GameObject mainLight = new GameObject("MainLight");
            mainLight.transform.position = new Vector3(0, 8f, 0);
            mainLight.transform.rotation = Quaternion.Euler(45f, 0f, 0f);
            
            Light light = mainLight.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1f;
            light.color = Color.white;
            light.shadows = LightShadows.Soft;
            
            // Создаем дополнительные источники света
            CreatePointLight("Light1", new Vector3(-5f, 3f, -5f), Color.white, 2f);
            CreatePointLight("Light2", new Vector3(5f, 3f, -5f), Color.white, 2f);
            CreatePointLight("Light3", new Vector3(-5f, 3f, 5f), Color.white, 2f);
            CreatePointLight("Light4", new Vector3(5f, 3f, 5f), Color.white, 2f);
        }
        
        static void CreatePointLight(string name, Vector3 position, Color color, float intensity)
        {
            GameObject lightObj = new GameObject(name);
            lightObj.transform.position = position;
            
            Light light = lightObj.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = color;
            light.intensity = intensity;
            light.range = 10f;
        }
        
        static void CreateFunctionalZones()
        {
            // Загружаем материалы
            Material woodMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Garage/WoodMaterial.mat");
            Material metalMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Garage/MetalMaterial.mat");
            
            // Создаем функциональные зоны
            GameObject workbench = GarageObjectCreator.CreateWorkbench(new Vector3(-3f, 0f, -2f), woodMaterial);
            workbench.name = "WorkbenchZone";
            
            GameObject computer = GarageObjectCreator.CreateComputer(new Vector3(3f, 0f, -2f), metalMaterial);
            computer.name = "ComputerZone";
            
            GameObject bulletinBoard = GarageObjectCreator.CreateBulletinBoard(new Vector3(-8f, 0f, 0f), woodMaterial);
            bulletinBoard.name = "BulletinBoardZone";
            
            GameObject safe = GarageObjectCreator.CreateSafe(new Vector3(8f, 0f, 0f), metalMaterial);
            safe.name = "SafeZone";
            
            GameObject door = GarageObjectCreator.CreateDoor(new Vector3(0f, 0f, 7f), woodMaterial);
            door.name = "DoorZone";
            
            // Создаем детали интерьера
            CreateInteriorDetails(woodMaterial, metalMaterial);
        }
        
        static void CreateInteriorDetails(Material woodMaterial, Material metalMaterial)
        {
            // Загружаем дополнительные материалы
            Material blueprintMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Garage/BlueprintMaterial.mat");
            Material oilStainMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Garage/OilStainMaterial.mat");
            Material toolMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Garage/ToolMaterial.mat");
            
            // Полка с инструментами
            GameObject toolShelf = GarageObjectCreator.CreateToolShelf(new Vector3(-6f, 0f, -5f), woodMaterial);
            toolShelf.name = "ToolShelf";
            
            // Чертежи на стене
            GameObject blueprintWall = GarageObjectCreator.CreateBlueprintWall(new Vector3(-9f, 0f, 0f), blueprintMaterial);
            blueprintWall.name = "BlueprintWall";
            
            // Инструменты на верстаке
            GameObject workbenchTools = GarageObjectCreator.CreateWorkbenchTools(new Vector3(-3f, 0f, -2f), toolMaterial);
            workbenchTools.name = "WorkbenchTools";
            
            // Детали на полу
            GameObject floorDetails = GarageObjectCreator.CreateFloorDetails(new Vector3(0f, 0f, 0f), oilStainMaterial);
            floorDetails.name = "FloorDetails";
            
            // Светильник
            GameObject lightFixture = GarageObjectCreator.CreateLightFixture(new Vector3(0f, 0f, 0f), metalMaterial);
            lightFixture.name = "LightFixture";
        }
        
        static void CreateUI()
        {
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
        }
    }
}
