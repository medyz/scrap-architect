using UnityEngine;
using UnityEditor;
using System;

namespace ScrapArchitect.Garage.Editor
{
    public class MaterialCreator : EditorWindow
    {
        [MenuItem("Scrap Architect/Garage/Create Safe Materials")]
        public static void CreateSafeMaterials()
        {
            Debug.Log("Создаем безопасные материалы...");
            
            // Создаем папку для материалов если её нет
            string materialsPath = "Assets/Simple Garage/Materials";
            if (!AssetDatabase.IsValidFolder(materialsPath))
            {
                AssetDatabase.CreateFolder("Assets/Simple Garage", "Materials");
            }
            
            // Создаем материалы
            CreateMaterial("ComputerMaterial", new Color(0.7f, 0.7f, 0.7f));
            CreateMaterial("SafeMaterial", new Color(0.7f, 0.7f, 0.7f));
            CreateMaterial("BoardMaterial", new Color(0.5f, 0.3f, 0.2f));
            
            AssetDatabase.Refresh();
            Debug.Log("Безопасные материалы созданы!");
        }
        
        static void CreateMaterial(string name, Color color)
        {
            string materialPath = $"Assets/Simple Garage/Materials/{name}.mat";
            
            // Проверяем, существует ли уже материал
            Material existingMaterial = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            if (existingMaterial != null)
            {
                // Обновляем существующий материал
                existingMaterial.color = color;
                EditorUtility.SetDirty(existingMaterial);
                Debug.Log($"Материал {name} обновлен");
                return;
            }
            
            // Создаем новый материал
            Material material = new Material(Shader.Find("Standard"));
            if (material == null)
            {
                material = new Material(Shader.Find("Diffuse"));
            }
            if (material == null)
            {
                material = new Material(Shader.Find("Unlit/Color"));
            }
            
            material.name = name;
            material.color = color;
            
            // Сохраняем материал как ассет
            AssetDatabase.CreateAsset(material, materialPath);
            Debug.Log($"Материал {name} создан");
        }
        
        [MenuItem("Scrap Architect/Garage/Cleanup Materials")]
        public static void CleanupMaterials()
        {
            Debug.Log("Очищаем утечки материалов...");
            
            // Принудительно очищаем память
            Resources.UnloadUnusedAssets();
            GC.Collect();
            
            Debug.Log("Очистка материалов завершена!");
        }
    }
}
