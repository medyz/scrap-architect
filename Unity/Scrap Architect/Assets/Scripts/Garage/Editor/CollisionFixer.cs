using UnityEngine;
using UnityEditor;
using System;

namespace ScrapArchitect.Garage.Editor
{
    public class CollisionFixer : EditorWindow
    {
        [MenuItem("Scrap Architect/Garage/Fix All Collisions in Simple Garage")]
        public static void FixAllCollisions()
        {
            Debug.Log("=== ИСПРАВЛЕНИЕ ВСЕХ КОЛЛИЗИЙ В SIMPLE GARAGE ===");
            
            // Исправляем пол
            FixFloorCollisions();
            
            // Исправляем стены
            FixWallCollisions();
            
            // Исправляем предметы
            FixObjectCollisions();
            
            // Исправляем мебель
            FixFurnitureCollisions();
            
            Debug.Log("=== ИСПРАВЛЕНИЕ КОЛЛИЗИЙ ЗАВЕРШЕНО ===");
        }
        
        static void FixFloorCollisions()
        {
            Debug.Log("🔧 Исправляем коллизии пола...");
            
            // Ищем пол
            GameObject floor = GameObject.Find("Floor");
            if (floor == null)
            {
                floor = GameObject.Find("Carpet");
            }
            
            if (floor != null)
            {
                // Добавляем или исправляем коллизию пола
                BoxCollider floorCollider = floor.GetComponent<BoxCollider>();
                if (floorCollider == null)
                {
                    floorCollider = floor.AddComponent<BoxCollider>();
                }
                
                // Настраиваем коллизию пола
                floorCollider.size = new Vector3(20f, 0.1f, 20f); // Большая площадь
                floorCollider.center = Vector3.zero;
                floorCollider.isTrigger = false;
                
                Debug.Log($"✅ Коллизия пола исправлена: {floor.name}");
            }
            else
            {
                Debug.LogWarning("⚠️ Пол не найден!");
            }
        }
        
        static void FixWallCollisions()
        {
            Debug.Log("🔧 Исправляем коллизии стен...");
            
            // Ищем все объекты, которые могут быть стенами
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            
            foreach (GameObject obj in allObjects)
            {
                // Проверяем, является ли объект стеной
                if (IsWall(obj))
                {
                    AddWallCollision(obj);
                }
            }
        }
        
        static bool IsWall(GameObject obj)
        {
            string name = obj.name.ToLower();
            
            // Проверяем по имени
            if (name.Contains("wall") || name.Contains("exterior") || 
                name.Contains("garage") || name.Contains("ceiling"))
            {
                return true;
            }
            
            // Проверяем по размеру (стены обычно высокие и тонкие)
            Vector3 scale = obj.transform.localScale;
            if (scale.y > 2f && (scale.x < 0.5f || scale.z < 0.5f))
            {
                return true;
            }
            
            return false;
        }
        
        static void AddWallCollision(GameObject wall)
        {
            BoxCollider wallCollider = wall.GetComponent<BoxCollider>();
            if (wallCollider == null)
            {
                wallCollider = wall.AddComponent<BoxCollider>();
            }
            
            // Настраиваем коллизию стены
            Renderer renderer = wall.GetComponent<Renderer>();
            if (renderer != null)
            {
                wallCollider.size = renderer.bounds.size;
                wallCollider.center = Vector3.zero;
            }
            else
            {
                // Если нет рендерера, используем размер объекта
                wallCollider.size = wall.transform.localScale;
                wallCollider.center = Vector3.zero;
            }
            
            wallCollider.isTrigger = false;
            
            Debug.Log($"✅ Коллизия стены добавлена: {wall.name}");
        }
        
        static void FixObjectCollisions()
        {
            Debug.Log("🔧 Исправляем коллизии предметов...");
            
            // Список предметов, которые должны иметь коллизии
            string[] objectNames = {
                "Table", "Shelf", "Locker", "Garage door", "Drilling machine",
                "Saw", "Bench Grinder", "Air conditioner", "Small stool",
                "Big shelf", "Large corner shelf", "3 shelves", "Opened locker",
                "Between locker", "Carpet"
            };
            
            foreach (string objName in objectNames)
            {
                GameObject obj = GameObject.Find(objName);
                if (obj != null)
                {
                    AddObjectCollision(obj);
                }
            }
            
            // Ищем все префабы Simple Garage
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.Contains("suitcase") || obj.name.Contains("Drill") ||
                    obj.name.Contains("Machinery") || obj.name.Contains("Socket") ||
                    obj.name.Contains("Hook") || obj.name.Contains("Clamp") ||
                    obj.name.Contains("Sponge") || obj.name.Contains("Hose"))
                {
                    AddObjectCollision(obj);
                }
            }
        }
        
        static void AddObjectCollision(GameObject obj)
        {
            BoxCollider objCollider = obj.GetComponent<BoxCollider>();
            if (objCollider == null)
            {
                objCollider = obj.AddComponent<BoxCollider>();
            }
            
            // Настраиваем коллизию объекта
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                objCollider.size = renderer.bounds.size;
                objCollider.center = Vector3.zero;
            }
            else
            {
                // Если нет рендерера, используем размер объекта
                objCollider.size = obj.transform.localScale;
                objCollider.center = Vector3.zero;
            }
            
            objCollider.isTrigger = false;
            
            Debug.Log($"✅ Коллизия предмета добавлена: {obj.name}");
        }
        
        static void FixFurnitureCollisions()
        {
            Debug.Log("🔧 Исправляем коллизии мебели...");
            
            // Ищем мебель по тегам и именам
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            
            foreach (GameObject obj in allObjects)
            {
                if (IsFurniture(obj))
                {
                    AddFurnitureCollision(obj);
                }
            }
        }
        
        static bool IsFurniture(GameObject obj)
        {
            string name = obj.name.ToLower();
            
            // Проверяем по имени
            if (name.Contains("table") || name.Contains("shelf") || 
                name.Contains("locker") || name.Contains("stool") ||
                name.Contains("bench") || name.Contains("cabinet"))
            {
                return true;
            }
            
            // Проверяем по размеру (мебель обычно среднего размера)
            Vector3 scale = obj.transform.localScale;
            if (scale.x > 0.5f && scale.y > 0.5f && scale.z > 0.5f &&
                scale.x < 5f && scale.y < 5f && scale.z < 5f)
            {
                return true;
            }
            
            return false;
        }
        
        static void AddFurnitureCollision(GameObject furniture)
        {
            BoxCollider furnitureCollider = furniture.GetComponent<BoxCollider>();
            if (furnitureCollider == null)
            {
                furnitureCollider = furniture.AddComponent<BoxCollider>();
            }
            
            // Настраиваем коллизию мебели
            Renderer renderer = furniture.GetComponent<Renderer>();
            if (renderer != null)
            {
                furnitureCollider.size = renderer.bounds.size;
                furnitureCollider.center = Vector3.zero;
            }
            else
            {
                // Если нет рендерера, используем размер объекта
                furnitureCollider.size = furniture.transform.localScale;
                furnitureCollider.center = Vector3.zero;
            }
            
            furnitureCollider.isTrigger = false;
            
            Debug.Log($"✅ Коллизия мебели добавлена: {furniture.name}");
        }
        
        [MenuItem("Scrap Architect/Garage/Check Missing Collisions")]
        public static void CheckMissingCollisions()
        {
            Debug.Log("=== ПРОВЕРКА ОТСУТСТВУЮЩИХ КОЛЛИЗИЙ ===");
            
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            int missingCollisions = 0;
            
            foreach (GameObject obj in allObjects)
            {
                // Пропускаем игрока, камеру и UI
                if (obj.name.Contains("Player") || obj.name.Contains("Camera") || 
                    obj.name.Contains("UI") || obj.name.Contains("Canvas"))
                {
                    continue;
                }
                
                // Проверяем, нужна ли коллизия
                if (NeedsCollision(obj) && obj.GetComponent<Collider>() == null)
                {
                    Debug.LogWarning($"❌ Отсутствует коллизия: {obj.name}");
                    missingCollisions++;
                }
            }
            
            if (missingCollisions == 0)
            {
                Debug.Log("✅ Все объекты имеют коллизии!");
            }
            else
            {
                Debug.LogWarning($"⚠️ Найдено {missingCollisions} объектов без коллизий");
            }
        }
        
        static bool NeedsCollision(GameObject obj)
        {
            // Объекты, которые должны иметь коллизии
            string name = obj.name.ToLower();
            
            if (name.Contains("floor") || name.Contains("carpet") ||
                name.Contains("wall") || name.Contains("exterior") ||
                name.Contains("table") || name.Contains("shelf") ||
                name.Contains("locker") || name.Contains("door") ||
                name.Contains("machine") || name.Contains("tool") ||
                name.Contains("suitcase") || name.Contains("drill") ||
                name.Contains("furniture") || name.Contains("stool"))
            {
                return true;
            }
            
            // Проверяем по размеру (большие объекты должны иметь коллизии)
            Vector3 scale = obj.transform.localScale;
            if (scale.x > 1f || scale.y > 1f || scale.z > 1f)
            {
                return true;
            }
            
            return false;
        }
        
        [MenuItem("Scrap Architect/Garage/Remove All Collisions")]
        public static void RemoveAllCollisions()
        {
            Debug.Log("=== УДАЛЕНИЕ ВСЕХ КОЛЛИЗИЙ ===");
            
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            int removedCollisions = 0;
            
            foreach (GameObject obj in allObjects)
            {
                // Пропускаем игрока
                if (obj.name.Contains("Player"))
                {
                    continue;
                }
                
                Collider[] colliders = obj.GetComponents<Collider>();
                foreach (Collider collider in colliders)
                {
                    DestroyImmediate(collider);
                    removedCollisions++;
                }
            }
            
            Debug.Log($"✅ Удалено {removedCollisions} коллизий");
        }
    }
}
