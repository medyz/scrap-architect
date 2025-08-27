using UnityEngine;
using UnityEditor;

namespace ScrapArchitect.Garage.Editor
{
    public class PlayerDebugger : EditorWindow
    {
        [MenuItem("Scrap Architect/Garage/Debug Player Movement")]
        public static void DebugPlayerMovement()
        {
            Debug.Log("=== ДИАГНОСТИКА ПЕРСОНАЖА ===");
            
            // Находим игрока
            GameObject player = GameObject.Find("Player");
            if (player == null)
            {
                Debug.LogError("❌ Игрок не найден!");
                return;
            }
            
            Debug.Log($"✅ Игрок найден: {player.name}");
            
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
                Debug.Log($"✅ CharacterController найден: Height={controller.height}, Radius={controller.radius}");
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
            controller.center = Vector3.zero;
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
