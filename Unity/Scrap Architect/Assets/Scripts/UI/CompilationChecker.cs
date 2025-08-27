using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Скрипт для проверки ошибок компиляции
    /// </summary>
    public class CompilationChecker : MonoBehaviour
    {
        [MenuItem("Scrap Architect/Check Compilation")]
        public static void CheckCompilation()
        {
            Debug.Log("=== Compilation Checker ===");
            
            // Проверяем статус компиляции
            if (EditorApplication.isCompiling)
            {
                Debug.LogWarning("⚠️ Unity is still compiling... Please wait.");
                return;
            }
            
            // Проверяем наличие ошибок
            var logs = File.ReadAllLines(Application.dataPath + "/../Logs/Packages-Update.log");
            bool hasErrors = false;
            
            foreach (var line in logs)
            {
                if (line.Contains("error CS"))
                {
                    Debug.LogError($"❌ Compilation Error: {line}");
                    hasErrors = true;
                }
            }
            
            if (!hasErrors)
            {
                Debug.Log("✅ No compilation errors found!");
            }
            
            Debug.Log("=== End Compilation Check ===");
        }
        
        [MenuItem("Scrap Architect/Force Recompile")]
        public static void ForceRecompile()
        {
            Debug.Log("🔄 Forcing recompile...");
            AssetDatabase.Refresh();
            EditorApplication.ExecuteMenuItem("Assets/Reimport All");
        }
    }
}
