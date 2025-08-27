using UnityEngine;
using UnityEditor;

namespace ScrapArchitect.Garage.Editor
{
    public class PlayerDebugger : EditorWindow
    {
        [MenuItem("Scrap Architect/Garage/Fix Player Position (Walking on Ceiling)")]
        public static void FixPlayerPosition()
        {
            Debug.Log("=== ИСПРАВЛЕНИЕ ПОЗИЦИИ ПЕРСОНАЖА ===");
            
            // Находим игрока
            GameObject player = GameObject.Find("Player");
            if (player == null)
            {
                Debug.LogError("❌ Игрок не найден!");
                Debug.Log("💡 Создаем игрока...");
                CreatePlayerInCurrentScene();
                return;
            }
            
            // Исправляем позицию игрока
            player.transform.position = new Vector3(0, 1f, 0);
            Debug.Log("✅ Позиция игрока исправлена: (0, 1, 0)");
            
            // Исправляем CharacterController
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller == null)
            {
                controller = player.AddComponent<CharacterController>();
            }
            
            // Правильные настройки для ходьбы по полу
            controller.height = 2f;
            controller.radius = 0.5f;
            controller.center = new Vector3(0, 1f, 0); // Центр на уровне груди
            controller.slopeLimit = 45f;
            controller.stepOffset = 0.3f;
            controller.skinWidth = 0.08f;
            controller.minMoveDistance = 0.001f;
            
            Debug.Log("✅ CharacterController исправлен:");
            Debug.Log($"   - Height: {controller.height}");
            Debug.Log($"   - Radius: {controller.radius}");
            Debug.Log($"   - Center: {controller.center}");
            
            // Исправляем камеру
            Camera camera = player.GetComponentInChildren<Camera>();
            if (camera != null)
            {
                camera.transform.localPosition = new Vector3(0, 1.8f, 0);
                camera.transform.localRotation = Quaternion.identity;
                Debug.Log("✅ Позиция камеры исправлена: (0, 1.8, 0)");
            }
            
            // Проверяем GarageManager
            GarageManager garageManager = player.GetComponent<GarageManager>();
            if (garageManager != null)
            {
                garageManager.playerCamera = camera;
                Debug.Log("✅ GarageManager подключен к камере");
            }
            
            // Блокируем курсор
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            Debug.Log("=== ИСПРАВЛЕНИЕ ЗАВЕРШЕНО ===");
            Debug.Log("🎮 Теперь персонаж должен ходить по полу!");
        }
        
        [MenuItem("Scrap Architect/Garage/Create Player in Current Scene")]
        public static void CreatePlayerInCurrentScene()
        {
            Debug.Log("=== СОЗДАНИЕ ИГРОКА В ТЕКУЩЕЙ СЦЕНЕ ===");
            
            // Проверяем, есть ли уже игрок
            GameObject existingPlayer = GameObject.Find("Player");
            if (existingPlayer != null)
            {
                Debug.LogWarning("⚠️ Игрок уже существует в сцене!");
                return;
            }
            
            // Создаем игрока
            GameObject player = new GameObject("Player");
            player.transform.position = new Vector3(0, 1f, 0);
            
            // Добавляем CharacterController с правильными настройками
            CharacterController controller = player.AddComponent<CharacterController>();
            controller.height = 2f;
            controller.radius = 0.5f;
            controller.center = new Vector3(0, 1f, 0); // Центр на уровне груди
            controller.slopeLimit = 45f;
            controller.stepOffset = 0.3f;
            controller.skinWidth = 0.08f;
            controller.minMoveDistance = 0.001f;
            
            // Добавляем камеру
            GameObject camera = new GameObject("PlayerCamera");
            camera.transform.SetParent(player.transform);
            camera.transform.localPosition = new Vector3(0, 1.8f, 0);
            camera.transform.localRotation = Quaternion.identity;
            
            Camera cam = camera.AddComponent<Camera>();
            cam.fieldOfView = 60f;
            cam.nearClipPlane = 0.1f;
            cam.farClipPlane = 1000f;
            cam.tag = "MainCamera";
            
            // Добавляем GarageManager
            GarageManager garageManager = player.AddComponent<GarageManager>();
            garageManager.playerCamera = cam;
            garageManager.walkSpeed = 5f;
            garageManager.runSpeed = 8f;
            garageManager.mouseSensitivity = 2f;
            
            // Блокируем курсор
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            // Выбираем игрока в иерархии
            Selection.activeGameObject = player;
            
            Debug.Log("✅ Игрок успешно создан в текущей сцене!");
            Debug.Log("✅ CharacterController добавлен с правильными настройками");
            Debug.Log("✅ Камера создана и настроена");
            Debug.Log("✅ GarageManager добавлен");
            Debug.Log("✅ Курсор заблокирован");
            Debug.Log("=== СОЗДАНИЕ ЗАВЕРШЕНО ===");
        }
        
        [MenuItem("Scrap Architect/Garage/Debug Player Movement")]
        public static void DebugPlayerMovement()
        {
            Debug.Log("=== ДИАГНОСТИКА ПЕРСОНАЖА ===");
            
            // Находим игрока
            GameObject player = GameObject.Find("Player");
            if (player == null)
            {
                Debug.LogError("❌ Игрок не найден!");
                Debug.Log("💡 Используйте 'Create Player in Current Scene' для создания игрока");
                return;
            }
            
            Debug.Log($"✅ Игрок найден: {player.name}");
            Debug.Log($"✅ Позиция игрока: {player.transform.position}");
            
            // Проверяем CharacterController
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller == null)
            {
                Debug.LogError("❌ CharacterController не найден!");
                controller = player.AddComponent<CharacterController>();
                Debug.Log("✅ CharacterController добавлен");
            }
            else
            {
                Debug.Log($"✅ CharacterController найден:");
                Debug.Log($"   - Height: {controller.height}");
                Debug.Log($"   - Radius: {controller.radius}");
                Debug.Log($"   - Center: {controller.center}");
                Debug.Log($"   - Position: {controller.transform.position}");
            }
            
            // Проверяем GarageManager
            GarageManager garageManager = player.GetComponent<GarageManager>();
            if (garageManager == null)
            {
                Debug.LogError("❌ GarageManager не найден!");
                garageManager = player.AddComponent<GarageManager>();
                Debug.Log("✅ GarageManager добавлен");
            }
            else
            {
                Debug.Log($"✅ GarageManager найден: WalkSpeed={garageManager.walkSpeed}, RunSpeed={garageManager.runSpeed}");
            }
            
            // Проверяем камеру
            Camera camera = player.GetComponentInChildren<Camera>();
            if (camera == null)
            {
                Debug.LogError("❌ Камера не найдена!");
                CreatePlayerCamera(player);
            }
            else
            {
                Debug.Log($"✅ Камера найдена: {camera.name}");
                Debug.Log($"✅ Позиция камеры: {camera.transform.localPosition}");
                garageManager.playerCamera = camera;
            }
            
            // Проверяем курсор
            Debug.Log($"Курсор заблокирован: {Cursor.lockState == CursorLockMode.Locked}");
            Debug.Log($"Курсор видим: {Cursor.visible}");
            
            Debug.Log("=== ДИАГНОСТИКА ЗАВЕРШЕНА ===");
        }
        
        [MenuItem("Scrap Architect/Garage/Fix Player Movement")]
        public static void FixPlayerMovement()
        {
            Debug.Log("=== ИСПРАВЛЕНИЕ ПЕРСОНАЖА ===");
            
            // Находим игрока
            GameObject player = GameObject.Find("Player");
            if (player == null)
            {
                Debug.LogError("❌ Игрок не найден!");
                Debug.Log("💡 Создаем игрока...");
                CreatePlayerInCurrentScene();
                return;
            }
            
            // Исправляем CharacterController
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller == null)
            {
                controller = player.AddComponent<CharacterController>();
            }
            controller.height = 2f;
            controller.radius = 0.5f;
            controller.center = new Vector3(0, 1f, 0);
            Debug.Log("✅ CharacterController исправлен");
            
            // Исправляем GarageManager
            GarageManager garageManager = player.GetComponent<GarageManager>();
            if (garageManager == null)
            {
                garageManager = player.AddComponent<GarageManager>();
            }
            garageManager.walkSpeed = 5f;
            garageManager.runSpeed = 8f;
            garageManager.mouseSensitivity = 2f;
            Debug.Log("✅ GarageManager исправлен");
            
            // Исправляем камеру
            Camera camera = player.GetComponentInChildren<Camera>();
            if (camera == null)
            {
                CreatePlayerCamera(player);
            }
            else
            {
                camera.tag = "MainCamera";
                garageManager.playerCamera = camera;
                Debug.Log("✅ Камера исправлена");
            }
            
            // Исправляем курсор
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Debug.Log("✅ Курсор исправлен");
            
            Debug.Log("=== ИСПРАВЛЕНИЕ ЗАВЕРШЕНО ===");
        }
        
        [MenuItem("Scrap Architect/Garage/Unlock Cursor")]
        public static void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("✅ Курсор разблокирован");
        }
        
        [MenuItem("Scrap Architect/Garage/Lock Cursor")]
        public static void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Debug.Log("✅ Курсор заблокирован");
        }
        
        static void CreatePlayerCamera(GameObject player)
        {
            GameObject cameraObj = new GameObject("PlayerCamera");
            cameraObj.transform.SetParent(player.transform);
            cameraObj.transform.localPosition = new Vector3(0, 1.8f, 0);
            cameraObj.transform.localRotation = Quaternion.identity;
            
            Camera camera = cameraObj.AddComponent<Camera>();
            camera.fieldOfView = 60f;
            camera.nearClipPlane = 0.1f;
            camera.farClipPlane = 1000f;
            camera.tag = "MainCamera";
            
            Debug.Log("✅ Камера создана");
        }
        
        [MenuItem("Scrap Architect/Garage/Test Input")]
        public static void TestInput()
        {
            Debug.Log("=== ТЕСТ УПРАВЛЕНИЯ ===");
            Debug.Log($"Horizontal: {Input.GetAxis("Horizontal")}");
            Debug.Log($"Vertical: {Input.GetAxis("Vertical")}");
            Debug.Log($"Mouse X: {Input.GetAxis("Mouse X")}");
            Debug.Log($"Mouse Y: {Input.GetAxis("Mouse Y")}");
            Debug.Log($"W pressed: {Input.GetKey(KeyCode.W)}");
            Debug.Log($"A pressed: {Input.GetKey(KeyCode.A)}");
            Debug.Log($"S pressed: {Input.GetKey(KeyCode.S)}");
            Debug.Log($"D pressed: {Input.GetKey(KeyCode.D)}");
            Debug.Log("=== ТЕСТ ЗАВЕРШЕН ===");
        }
    }
}
