using UnityEngine;
using UnityEditor;
using System;

namespace ScrapArchitect.Garage.Editor
{
    public class PlayerPositionFixer : EditorWindow
    {
        [MenuItem("Scrap Architect/Garage/EMERGENCY: Fix Player Under Ground")]
        public static void FixPlayerUnderGround()
        {
            Debug.Log("🚨 ЭКСТРЕННОЕ ИСПРАВЛЕНИЕ: Персонаж под землей!");
            
            // Находим игрока
            GameObject player = GameObject.Find("Player");
            if (player == null)
            {
                Debug.LogError("❌ Игрок не найден! Создаем нового игрока...");
                CreateNewPlayer();
                return;
            }
            
            // Находим пол гаража
            GameObject floor = FindGarageFloor();
            
            if (floor != null)
            {
                // Вычисляем правильную позицию игрока
                Vector3 floorPosition = floor.transform.position;
                Vector3 floorSize = GetFloorSize(floor);
                
                // Позиция игрока должна быть выше пола
                Vector3 newPlayerPosition = new Vector3(
                    floorPosition.x,
                    floorPosition.y + floorSize.y + 1f, // 1 метр выше пола
                    floorPosition.z
                );
                
                // Устанавливаем позицию игрока
                player.transform.position = newPlayerPosition;
                
                // Исправляем CharacterController
                CharacterController controller = player.GetComponent<CharacterController>();
                if (controller != null)
                {
                    controller.center = new Vector3(0, 1f, 0);
                    controller.height = 2f;
                    controller.radius = 0.5f;
                }
                
                // Исправляем камеру
                Camera playerCamera = player.GetComponentInChildren<Camera>();
                if (playerCamera != null)
                {
                    playerCamera.transform.localPosition = new Vector3(0, 1.8f, 0);
                }
                
                Debug.Log($"✅ Персонаж перемещен на позицию: {newPlayerPosition}");
                Debug.Log($"✅ Позиция пола: {floorPosition}, Размер пола: {floorSize}");
            }
            else
            {
                // Если пол не найден, используем стандартную позицию
                player.transform.position = new Vector3(0, 2f, 0);
                Debug.LogWarning("⚠️ Пол не найден! Используем стандартную позицию (0, 2, 0)");
            }
            
            // Выбираем игрока в иерархии
            Selection.activeGameObject = player;
            
            Debug.Log("✅ Исправление завершено! Персонаж должен быть на полу.");
        }
        
        static GameObject FindGarageFloor()
        {
            // Ищем пол по разным именам
            GameObject floor = GameObject.Find("Floor");
            if (floor != null) return floor;
            
            floor = GameObject.Find("Carpet");
            if (floor != null) return floor;
            
            floor = GameObject.Find("Ground");
            if (floor != null) return floor;
            
            // Ищем по тегу
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Ground");
            if (taggedObjects.Length > 0) return taggedObjects[0];
            
            // Ищем по имени, содержащему "floor"
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.ToLower().Contains("floor") || 
                    obj.name.ToLower().Contains("carpet") ||
                    obj.name.ToLower().Contains("ground"))
                {
                    return obj;
                }
            }
            
            return null;
        }
        
        static Vector3 GetFloorSize(GameObject floor)
        {
            // Пытаемся получить размер из рендерера
            Renderer renderer = floor.GetComponent<Renderer>();
            if (renderer != null)
            {
                return renderer.bounds.size;
            }
            
            // Пытаемся получить размер из коллайдера
            Collider collider = floor.GetComponent<Collider>();
            if (collider != null)
            {
                return collider.bounds.size;
            }
            
            // Используем размер объекта
            return floor.transform.localScale;
        }
        
        static void CreateNewPlayer()
        {
            Debug.Log("🔧 Создаем нового игрока...");
            
            // Создаем игрока
            GameObject player = new GameObject("Player");
            player.transform.position = new Vector3(0, 2f, 0);
            
            // Добавляем CharacterController
            CharacterController controller = player.AddComponent<CharacterController>();
            controller.center = new Vector3(0, 1f, 0);
            controller.height = 2f;
            controller.radius = 0.5f;
            controller.slopeLimit = 45f;
            controller.stepOffset = 0.3f;
            controller.skinWidth = 0.08f;
            controller.minMoveDistance = 0.001f;
            
            // Создаем камеру
            GameObject cameraObj = new GameObject("PlayerCamera");
            cameraObj.transform.SetParent(player.transform);
            cameraObj.transform.localPosition = new Vector3(0, 1.8f, 0);
            
            Camera camera = cameraObj.AddComponent<Camera>();
            camera.tag = "MainCamera";
            camera.fieldOfView = 60f;
            camera.nearClipPlane = 0.1f;
            camera.farClipPlane = 1000f;
            
            // Добавляем GarageManager
            GarageManager garageManager = player.AddComponent<GarageManager>();
            garageManager.playerCamera = camera;
            
            // Блокируем курсор
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            Debug.Log("✅ Новый игрок создан!");
            Selection.activeGameObject = player;
        }
        
        [MenuItem("Scrap Architect/Garage/Find and Show Floor Position")]
        public static void FindAndShowFloorPosition()
        {
            Debug.Log("🔍 Поиск позиции пола...");
            
            GameObject floor = FindGarageFloor();
            if (floor != null)
            {
                Vector3 floorPosition = floor.transform.position;
                Vector3 floorSize = GetFloorSize(floor);
                
                Debug.Log($"✅ Пол найден: {floor.name}");
                Debug.Log($"📍 Позиция пола: {floorPosition}");
                Debug.Log($"📏 Размер пола: {floorSize}");
                Debug.Log($"🔝 Верхняя точка пола: {floorPosition.y + floorSize.y}");
                
                // Выбираем пол в иерархии
                Selection.activeGameObject = floor;
            }
            else
            {
                Debug.LogWarning("⚠️ Пол не найден!");
            }
        }
        
        [MenuItem("Scrap Architect/Garage/Show Player Position")]
        public static void ShowPlayerPosition()
        {
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                Vector3 playerPosition = player.transform.position;
                Debug.Log($"📍 Позиция игрока: {playerPosition}");
                
                CharacterController controller = player.GetComponent<CharacterController>();
                if (controller != null)
                {
                    Debug.Log($"🎯 CharacterController.center: {controller.center}");
                    Debug.Log($"📏 CharacterController.height: {controller.height}");
                    Debug.Log($"🔵 CharacterController.radius: {controller.radius}");
                }
                
                Camera camera = player.GetComponentInChildren<Camera>();
                if (camera != null)
                {
                    Debug.Log($"📷 Позиция камеры: {camera.transform.position}");
                    Debug.Log($"📷 Локальная позиция камеры: {camera.transform.localPosition}");
                }
                
                Selection.activeGameObject = player;
            }
            else
            {
                Debug.LogError("❌ Игрок не найден!");
            }
        }
        
        [MenuItem("Scrap Architect/Garage/Set Player to Garage Center")]
        public static void SetPlayerToGarageCenter()
        {
            Debug.Log("🎯 Устанавливаем игрока в центр гаража...");
            
            GameObject player = GameObject.Find("Player");
            if (player == null)
            {
                Debug.LogError("❌ Игрок не найден!");
                return;
            }
            
            // Устанавливаем игрока в центр гаража на высоте 2 метра
            player.transform.position = new Vector3(0, 2f, 0);
            
            // Исправляем CharacterController
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.center = new Vector3(0, 1f, 0);
                controller.height = 2f;
                controller.radius = 0.5f;
            }
            
            // Исправляем камеру
            Camera playerCamera = player.GetComponentInChildren<Camera>();
            if (playerCamera != null)
            {
                playerCamera.transform.localPosition = new Vector3(0, 1.8f, 0);
            }
            
            Debug.Log("✅ Игрок установлен в центр гаража на высоте 2 метра");
            Selection.activeGameObject = player;
        }
        
        [MenuItem("Scrap Architect/Garage/Reset Player to Safe Position")]
        public static void ResetPlayerToSafePosition()
        {
            Debug.Log("🛡️ Сброс игрока в безопасную позицию...");
            
            GameObject player = GameObject.Find("Player");
            if (player == null)
            {
                Debug.LogError("❌ Игрок не найден!");
                return;
            }
            
            // Безопасная позиция - высоко над гаражом
            player.transform.position = new Vector3(0, 10f, 0);
            
            // Исправляем CharacterController
            CharacterController controller = player.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.center = new Vector3(0, 1f, 0);
                controller.height = 2f;
                controller.radius = 0.5f;
            }
            
            // Исправляем камеру
            Camera playerCamera = player.GetComponentInChildren<Camera>();
            if (playerCamera != null)
            {
                playerCamera.transform.localPosition = new Vector3(0, 1.8f, 0);
            }
            
            Debug.Log("✅ Игрок сброшен в безопасную позицию (0, 10, 0)");
            Selection.activeGameObject = player;
        }
    }
}
