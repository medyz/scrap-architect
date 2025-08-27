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
            
            // Создаем менеджер меню
            CreateMenuManager();
            
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
        
        [MenuItem("Scrap Architect/Garage/Fix Purple Materials")]
        public static void FixPurpleMaterials()
        {
            Debug.Log("Исправляем фиолетовые материалы...");
            
            // Принудительно обновляем все материалы
            CreateMaterials();
            
            // Принудительно обновляем AssetDatabase
            AssetDatabase.Refresh();
            
            // Ждем завершения компиляции
            EditorApplication.delayCall += () => {
                AssetDatabase.Refresh();
                Debug.Log("Материалы исправлены! Проверьте сцену.");
            };
        }
        
        [MenuItem("Scrap Architect/Garage/Create Simple Materials")]
        public static void CreateSimpleMaterials()
        {
            Debug.Log("Создаем простые материалы...");
            
            // Создаем папку для материалов если её нет
            if (!AssetDatabase.IsValidFolder("Assets/Materials/Garage"))
            {
                AssetDatabase.CreateFolder("Assets/Materials", "Garage");
            }
            
            // Создаем простые материалы с базовым шейдером
            CreateSimpleMaterial("WallMaterial", new Color(0.8f, 0.7f, 0.6f), "Assets/Materials/Garage/WallMaterial.mat");
            CreateSimpleMaterial("FloorMaterial", new Color(0.6f, 0.5f, 0.4f), "Assets/Materials/Garage/FloorMaterial.mat");
            CreateSimpleMaterial("CeilingMaterial", new Color(0.9f, 0.85f, 0.8f), "Assets/Materials/Garage/CeilingMaterial.mat");
            CreateSimpleMaterial("MetalMaterial", new Color(0.7f, 0.7f, 0.7f), "Assets/Materials/Garage/MetalMaterial.mat");
            CreateSimpleMaterial("WoodMaterial", new Color(0.5f, 0.3f, 0.2f), "Assets/Materials/Garage/WoodMaterial.mat");
            CreateSimpleMaterial("BlueprintMaterial", new Color(0.95f, 0.95f, 0.9f), "Assets/Materials/Garage/BlueprintMaterial.mat");
            CreateSimpleMaterial("OilStainMaterial", new Color(0.2f, 0.2f, 0.1f), "Assets/Materials/Garage/OilStainMaterial.mat");
            CreateSimpleMaterial("ToolMaterial", new Color(0.4f, 0.4f, 0.4f), "Assets/Materials/Garage/ToolMaterial.mat");
            
            AssetDatabase.Refresh();
            Debug.Log("Простые материалы созданы!");
        }
        
        [MenuItem("Scrap Architect/Garage/EMERGENCY: Fix All Purple Objects")]
        public static void EmergencyFixPurpleObjects()
        {
            Debug.Log("ЭКСТРЕННОЕ исправление фиолетовых объектов...");
            
            // Находим все объекты в сцене
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            
            foreach (GameObject obj in allObjects)
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null && renderer.material != null)
                {
                    // Проверяем, фиолетовый ли материал
                    if (renderer.material.color.r > 0.9f && renderer.material.color.b > 0.9f && renderer.material.color.g < 0.1f)
                    {
                        Debug.Log($"Исправляем фиолетовый объект: {obj.name}");
                        
                        // Создаем новый материал прямо на объекте
                        Material newMaterial = new Material(Shader.Find("Diffuse"));
                        if (newMaterial == null)
                        {
                            newMaterial = new Material(Shader.Find("Unlit/Color"));
                        }
                        if (newMaterial == null)
                        {
                            newMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
                        }
                        
                        // Устанавливаем цвет в зависимости от имени объекта
                        if (obj.name.Contains("Wall") || obj.name.Contains("GarageStructure"))
                        {
                            newMaterial.color = new Color(0.8f, 0.7f, 0.6f); // Бежевый
                        }
                        else if (obj.name.Contains("Floor"))
                        {
                            newMaterial.color = new Color(0.6f, 0.5f, 0.4f); // Коричневый
                        }
                        else if (obj.name.Contains("Ceiling"))
                        {
                            newMaterial.color = new Color(0.9f, 0.85f, 0.8f); // Светло-бежевый
                        }
                        else if (obj.name.Contains("Workbench") || obj.name.Contains("Wood"))
                        {
                            newMaterial.color = new Color(0.5f, 0.3f, 0.2f); // Темно-коричневый
                        }
                        else if (obj.name.Contains("Computer") || obj.name.Contains("Safe") || obj.name.Contains("Metal"))
                        {
                            newMaterial.color = new Color(0.7f, 0.7f, 0.7f); // Серый
                        }
                        else if (obj.name.Contains("Blueprint"))
                        {
                            newMaterial.color = new Color(0.95f, 0.95f, 0.9f); // Светло-бежевый
                        }
                        else if (obj.name.Contains("Oil") || obj.name.Contains("Stain"))
                        {
                            newMaterial.color = new Color(0.2f, 0.2f, 0.1f); // Темно-коричневый
                        }
                        else if (obj.name.Contains("Tool"))
                        {
                            newMaterial.color = new Color(0.4f, 0.4f, 0.4f); // Серый
                        }
                        else
                        {
                            newMaterial.color = new Color(0.6f, 0.6f, 0.6f); // Нейтральный серый
                        }
                        
                        // Применяем материал
                        renderer.material = newMaterial;
                    }
                }
            }
            
            Debug.Log("ЭКСТРЕННОЕ исправление завершено!");
        }
        
        [MenuItem("Scrap Architect/Garage/REBUILD: Create Garage with Working Materials")]
        public static void RebuildGarageWithWorkingMaterials()
        {
            Debug.Log("ПЕРЕСБОРКА гаража с рабочими материалами...");
            
            // Удаляем все существующие объекты кроме Player и UI
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (!obj.name.Contains("Player") && !obj.name.Contains("GarageUI") && !obj.name.Contains("Canvas"))
                {
                    DestroyImmediate(obj);
                }
            }
            
            // Создаем материалы с простыми шейдерами
            CreateSimpleMaterials();
            
            // Создаем структуру гаража с материалами прямо на объектах
            CreateGarageStructureWithDirectMaterials();
            
            // Создаем функциональные зоны
            CreateFunctionalZonesWithDirectMaterials();
            
            // Создаем детали интерьера
            CreateInteriorDetailsWithDirectMaterials();
            
            // Создаем освещение
            CreateLighting();
            
            Debug.Log("Гараж пересобран с рабочими материалами!");
        }
        
        [MenuItem("Scrap Architect/Garage/Add Interactive Zones")]
        public static void AddInteractiveZones()
        {
            Debug.Log("Добавляем интерактивные зоны...");
            
            // Добавляем GarageZone к функциональным объектам
            AddZoneToObject("WorkbenchZone", "Верстак", "Нажмите E для доступа к контрактам", ZoneType.Workbench);
            AddZoneToObject("ComputerZone", "Компьютер", "Нажмите E для Steam Workshop", ZoneType.Computer);
            AddZoneToObject("BulletinBoardZone", "Доска объявлений", "Нажмите E для просмотра контрактов", ZoneType.BulletinBoard);
            AddZoneToObject("SafeZone", "Сейф", "Нажмите E для магазина деталей", ZoneType.Safe);
            AddZoneToObject("DoorZone", "Дверь", "Нажмите E для выхода на полигон", ZoneType.Door);
            
            Debug.Log("Интерактивные зоны добавлены!");
        }
        
        static void AddZoneToObject(string objectName, string zoneName, string interactionText, ZoneType zoneType)
        {
            GameObject obj = GameObject.Find(objectName);
            if (obj != null)
            {
                // Добавляем GarageZone компонент
                GarageZone zone = obj.AddComponent<GarageZone>();
                zone.Initialize(zoneName, interactionText, zoneType);
                
                // Настраиваем параметры зоны
                zone.interactionRange = 3f;
                
                Debug.Log($"Добавлена зона {zoneName} к объекту {objectName}");
            }
            else
            {
                Debug.LogWarning($"Объект {objectName} не найден!");
            }
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
        
        static void CreateSimpleMaterial(string name, Color color, string path)
        {
            // Удаляем существующий материал если есть
            if (AssetDatabase.LoadAssetAtPath<Material>(path) != null)
            {
                AssetDatabase.DeleteAsset(path);
            }
            
            // Создаем новый материал с базовым шейдером
            Material material = new Material(Shader.Find("Diffuse"));
            if (material == null)
            {
                // Если Diffuse не найден, пробуем Unlit/Color
                material = new Material(Shader.Find("Unlit/Color"));
            }
            if (material == null)
            {
                // Если и это не работает, создаем без шейдера
                material = new Material(Shader.Find("Hidden/InternalErrorShader"));
                Debug.LogWarning($"Используем fallback шейдер для {name}");
            }
            
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
        
        static void CreateMenuManager()
        {
            // Создаем менеджер меню
            GameObject menuManagerObj = new GameObject("GarageMenuManager");
            GarageMenuManager menuManager = menuManagerObj.AddComponent<GarageMenuManager>();
            
            // Подключаем к GarageManager
            GarageManager garageManager = FindObjectOfType<GarageManager>();
            if (garageManager != null)
            {
                garageManager.menuManager = menuManager;
            }
            
            Debug.Log("Менеджер меню создан и подключен!");
        }
        
        static void CreateGarageStructureWithDirectMaterials()
        {
            // Создаем структуру гаража с материалами прямо на объектах
            GameObject garageStructure = new GameObject("GarageStructure");
            
            // Пол
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "Floor";
            floor.transform.SetParent(garageStructure.transform);
            floor.transform.localScale = new Vector3(2f, 1f, 1.5f);
            
            Material floorMaterial = new Material(Shader.Find("Diffuse"));
            if (floorMaterial == null) floorMaterial = new Material(Shader.Find("Unlit/Color"));
            floorMaterial.color = new Color(0.6f, 0.5f, 0.4f);
            floor.GetComponent<Renderer>().material = floorMaterial;
            
            // Стены
            CreateWall("BackWall", new Vector3(0, 2, -7.5f), new Vector3(20, 4, 0.2f), garageStructure.transform);
            CreateWall("FrontWall", new Vector3(0, 2, 7.5f), new Vector3(20, 4, 0.2f), garageStructure.transform);
            CreateWall("LeftWall", new Vector3(-10, 2, 0), new Vector3(0.2f, 4, 15), garageStructure.transform);
            CreateWall("RightWall", new Vector3(10, 2, 0), new Vector3(0.2f, 4, 15), garageStructure.transform);
            
            // Потолок
            GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ceiling.name = "Ceiling";
            ceiling.transform.SetParent(garageStructure.transform);
            ceiling.transform.position = new Vector3(0, 4, 0);
            ceiling.transform.localScale = new Vector3(2f, 1f, 1.5f);
            ceiling.transform.rotation = Quaternion.Euler(180, 0, 0);
            
            Material ceilingMaterial = new Material(Shader.Find("Diffuse"));
            if (ceilingMaterial == null) ceilingMaterial = new Material(Shader.Find("Unlit/Color"));
            ceilingMaterial.color = new Color(0.9f, 0.85f, 0.8f);
            ceiling.GetComponent<Renderer>().material = ceilingMaterial;
        }
        
        static void CreateWall(string name, Vector3 position, Vector3 scale, Transform parent)
        {
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = name;
            wall.transform.SetParent(parent);
            wall.transform.position = position;
            wall.transform.localScale = scale;
            
            Material wallMaterial = new Material(Shader.Find("Diffuse"));
            if (wallMaterial == null) wallMaterial = new Material(Shader.Find("Unlit/Color"));
            wallMaterial.color = new Color(0.8f, 0.7f, 0.6f);
            wall.GetComponent<Renderer>().material = wallMaterial;
        }
        
        static void CreateFunctionalZonesWithDirectMaterials()
        {
            // Верстак
            CreateWorkbenchWithDirectMaterial(new Vector3(-3f, 0f, -2f));
            
            // Компьютер
            CreateComputerWithDirectMaterial(new Vector3(3f, 0f, -2f));
            
            // Доска объявлений
            CreateBulletinBoardWithDirectMaterial(new Vector3(-8f, 0f, 0f));
            
            // Сейф
            CreateSafeWithDirectMaterial(new Vector3(8f, 0f, 0f));
            
            // Дверь
            CreateDoorWithDirectMaterial(new Vector3(0f, 0f, 7f));
        }
        
        static void CreateWorkbenchWithDirectMaterial(Vector3 position)
        {
            GameObject workbench = new GameObject("WorkbenchZone");
            workbench.transform.position = position;
            
            // Поверхность верстака
            GameObject surface = GameObject.CreatePrimitive(PrimitiveType.Cube);
            surface.name = "Surface";
            surface.transform.SetParent(workbench.transform);
            surface.transform.localPosition = Vector3.up * 0.9f;
            surface.transform.localScale = new Vector3(2f, 0.1f, 1f);
            
            Material woodMaterial = new Material(Shader.Find("Diffuse"));
            if (woodMaterial == null) woodMaterial = new Material(Shader.Find("Unlit/Color"));
            woodMaterial.color = new Color(0.5f, 0.3f, 0.2f);
            surface.GetComponent<Renderer>().material = woodMaterial;
            
            // Ножки
            for (int i = 0; i < 4; i++)
            {
                GameObject leg = GameObject.CreatePrimitive(PrimitiveType.Cube);
                leg.name = $"Leg{i + 1}";
                leg.transform.SetParent(workbench.transform);
                leg.transform.localPosition = new Vector3((i % 2 == 0 ? -0.8f : 0.8f), 0.45f, (i < 2 ? -0.4f : 0.4f));
                leg.transform.localScale = new Vector3(0.1f, 0.9f, 0.1f);
                leg.GetComponent<Renderer>().material = woodMaterial;
            }
        }
        
        static void CreateComputerWithDirectMaterial(Vector3 position)
        {
            GameObject computer = new GameObject("ComputerZone");
            computer.transform.position = position;
            
            // Монитор
            GameObject monitor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            monitor.name = "Monitor";
            monitor.transform.SetParent(computer.transform);
            monitor.transform.localPosition = Vector3.up * 1.2f;
            monitor.transform.localScale = new Vector3(0.8f, 0.6f, 0.1f);
            
            Material metalMaterial = new Material(Shader.Find("Diffuse"));
            if (metalMaterial == null) metalMaterial = new Material(Shader.Find("Unlit/Color"));
            metalMaterial.color = new Color(0.7f, 0.7f, 0.7f);
            monitor.GetComponent<Renderer>().material = metalMaterial;
            
            // Клавиатура
            GameObject keyboard = GameObject.CreatePrimitive(PrimitiveType.Cube);
            keyboard.name = "Keyboard";
            keyboard.transform.SetParent(computer.transform);
            keyboard.transform.localPosition = Vector3.up * 0.8f;
            keyboard.transform.localScale = new Vector3(0.6f, 0.05f, 0.2f);
            keyboard.GetComponent<Renderer>().material = metalMaterial;
        }
        
        static void CreateBulletinBoardWithDirectMaterial(Vector3 position)
        {
            GameObject board = new GameObject("BulletinBoardZone");
            board.transform.position = position;
            
            // Доска
            GameObject mainBoard = GameObject.CreatePrimitive(PrimitiveType.Cube);
            mainBoard.name = "MainBoard";
            mainBoard.transform.SetParent(board.transform);
            mainBoard.transform.localPosition = Vector3.up * 1.5f;
            mainBoard.transform.localScale = new Vector3(1.5f, 1f, 0.05f);
            
            Material woodMaterial = new Material(Shader.Find("Diffuse"));
            if (woodMaterial == null) woodMaterial = new Material(Shader.Find("Unlit/Color"));
            woodMaterial.color = new Color(0.5f, 0.3f, 0.2f);
            mainBoard.GetComponent<Renderer>().material = woodMaterial;
        }
        
        static void CreateSafeWithDirectMaterial(Vector3 position)
        {
            GameObject safe = new GameObject("SafeZone");
            safe.transform.position = position;
            
            // Корпус сейфа
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
            body.name = "Body";
            body.transform.SetParent(safe.transform);
            body.transform.localPosition = Vector3.up * 0.5f;
            body.transform.localScale = new Vector3(0.8f, 1f, 0.6f);
            
            Material metalMaterial = new Material(Shader.Find("Diffuse"));
            if (metalMaterial == null) metalMaterial = new Material(Shader.Find("Unlit/Color"));
            metalMaterial.color = new Color(0.7f, 0.7f, 0.7f);
            body.GetComponent<Renderer>().material = metalMaterial;
        }
        
        static void CreateDoorWithDirectMaterial(Vector3 position)
        {
            GameObject door = new GameObject("DoorZone");
            door.transform.position = position;
            
            // Дверное полотно
            GameObject doorPanel = GameObject.CreatePrimitive(PrimitiveType.Cube);
            doorPanel.name = "DoorPanel";
            doorPanel.transform.SetParent(door.transform);
            doorPanel.transform.localPosition = Vector3.up * 1.5f;
            doorPanel.transform.localScale = new Vector3(1f, 2.8f, 0.05f);
            
            Material woodMaterial = new Material(Shader.Find("Diffuse"));
            if (woodMaterial == null) woodMaterial = new Material(Shader.Find("Unlit/Color"));
            woodMaterial.color = new Color(0.5f, 0.3f, 0.2f);
            doorPanel.GetComponent<Renderer>().material = woodMaterial;
        }
        
        static void CreateInteriorDetailsWithDirectMaterials()
        {
            // Полка с инструментами
            CreateToolShelfWithDirectMaterial(new Vector3(-6f, 0f, -5f));
            
            // Чертежи на стене
            CreateBlueprintWallWithDirectMaterial(new Vector3(-9f, 0f, 0f));
            
            // Инструменты на верстаке
            CreateWorkbenchToolsWithDirectMaterial(new Vector3(-3f, 0f, -2f));
            
            // Детали на полу
            CreateFloorDetailsWithDirectMaterial(new Vector3(0f, 0f, 0f));
        }
        
        static void CreateToolShelfWithDirectMaterial(Vector3 position)
        {
            GameObject shelf = new GameObject("ToolShelf");
            shelf.transform.position = position;
            
            // Полка
            GameObject mainShelf = GameObject.CreatePrimitive(PrimitiveType.Cube);
            mainShelf.name = "MainShelf";
            mainShelf.transform.SetParent(shelf.transform);
            mainShelf.transform.localPosition = Vector3.up * 1.8f;
            mainShelf.transform.localScale = new Vector3(2f, 0.1f, 0.6f);
            
            Material woodMaterial = new Material(Shader.Find("Diffuse"));
            if (woodMaterial == null) woodMaterial = new Material(Shader.Find("Unlit/Color"));
            woodMaterial.color = new Color(0.5f, 0.3f, 0.2f);
            mainShelf.GetComponent<Renderer>().material = woodMaterial;
            
            // Инструменты
            Material toolMaterial = new Material(Shader.Find("Diffuse"));
            if (toolMaterial == null) toolMaterial = new Material(Shader.Find("Unlit/Color"));
            toolMaterial.color = new Color(0.4f, 0.4f, 0.4f);
            
            GameObject hammer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            hammer.name = "Hammer";
            hammer.transform.SetParent(shelf.transform);
            hammer.transform.localPosition = new Vector3(0.3f, 1.85f, 0);
            hammer.transform.localScale = new Vector3(0.3f, 0.05f, 0.1f);
            hammer.GetComponent<Renderer>().material = toolMaterial;
        }
        
        static void CreateBlueprintWallWithDirectMaterial(Vector3 position)
        {
            GameObject blueprintWall = new GameObject("BlueprintWall");
            blueprintWall.transform.position = position;
            
            // Чертеж
            GameObject blueprint = GameObject.CreatePrimitive(PrimitiveType.Cube);
            blueprint.name = "Blueprint";
            blueprint.transform.SetParent(blueprintWall.transform);
            blueprint.transform.localPosition = new Vector3(0.5f, 2f, 0);
            blueprint.transform.localScale = new Vector3(0.8f, 0.6f, 0.02f);
            
            Material blueprintMaterial = new Material(Shader.Find("Diffuse"));
            if (blueprintMaterial == null) blueprintMaterial = new Material(Shader.Find("Unlit/Color"));
            blueprintMaterial.color = new Color(0.95f, 0.95f, 0.9f);
            blueprint.GetComponent<Renderer>().material = blueprintMaterial;
        }
        
        static void CreateWorkbenchToolsWithDirectMaterial(Vector3 position)
        {
            GameObject tools = new GameObject("WorkbenchTools");
            tools.transform.position = position;
            
            // Молоток на верстаке
            GameObject hammer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            hammer.name = "Hammer";
            hammer.transform.SetParent(tools.transform);
            hammer.transform.localPosition = new Vector3(0.3f, 0.95f, 0);
            hammer.transform.localScale = new Vector3(0.2f, 0.05f, 0.05f);
            
            Material toolMaterial = new Material(Shader.Find("Diffuse"));
            if (toolMaterial == null) toolMaterial = new Material(Shader.Find("Unlit/Color"));
            toolMaterial.color = new Color(0.4f, 0.4f, 0.4f);
            hammer.GetComponent<Renderer>().material = toolMaterial;
        }
        
        static void CreateFloorDetailsWithDirectMaterial(Vector3 position)
        {
            GameObject floorDetails = new GameObject("FloorDetails");
            floorDetails.transform.position = position;
            
            // Пятно масла
            GameObject oilStain = GameObject.CreatePrimitive(PrimitiveType.Cube);
            oilStain.name = "OilStain";
            oilStain.transform.SetParent(floorDetails.transform);
            oilStain.transform.localPosition = new Vector3(2f, 0.01f, 0);
            oilStain.transform.localScale = new Vector3(0.5f, 0.01f, 0.3f);
            
            Material oilMaterial = new Material(Shader.Find("Diffuse"));
            if (oilMaterial == null) oilMaterial = new Material(Shader.Find("Unlit/Color"));
            oilMaterial.color = new Color(0.2f, 0.2f, 0.1f);
            oilStain.GetComponent<Renderer>().material = oilMaterial;
        }
    }
}
