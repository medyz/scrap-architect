using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using ScrapArchitect.Physics;
using ScrapArchitect.UI;

namespace ScrapArchitect.System
{
    /// <summary>
    /// Менеджер для работы с blueprint (сохранение/загрузка конструкций)
    /// </summary>
    public class BlueprintManager : MonoBehaviour
    {
        [Header("Blueprint Settings")]
        public string blueprintFolder = "Blueprints";
        public string fileExtension = ".json";
        public int maxBlueprints = 100;
        public bool autoSave = true;
        public float autoSaveInterval = 60f; // секунды
        
        [Header("UI References")]
        public GameObject blueprintPanel;
        public Transform blueprintListContent;
        public GameObject blueprintItemPrefab;
        
        [Header("Audio Settings")]
        public AudioClip saveSound;
        public AudioClip loadSound;
        public AudioClip deleteSound;
        public AudioClip errorSound;
        
        private List<VehicleBlueprint> blueprints = new List<VehicleBlueprint>();
        private VehicleBlueprint currentBlueprint;
        private float lastAutoSaveTime;
        private string blueprintPath;
        
        // Events
        public Action<VehicleBlueprint> OnBlueprintSaved;
        public Action<VehicleBlueprint> OnBlueprintLoaded;
        public Action<VehicleBlueprint> OnBlueprintDeleted;
        public Action<List<VehicleBlueprint>> OnBlueprintListUpdated;
        
        private void Start()
        {
            InitializeBlueprintManager();
            LoadAllBlueprints();
        }
        
        private void Update()
        {
            // Автосохранение
            if (autoSave && currentBlueprint != null)
            {
                if (Time.time - lastAutoSaveTime >= autoSaveInterval)
                {
                    AutoSaveCurrentBlueprint();
                }
            }
        }
        
        /// <summary>
        /// Инициализация менеджера blueprint
        /// </summary>
        private void InitializeBlueprintManager()
        {
            // Создать папку для blueprint
            blueprintPath = Path.Combine(Application.persistentDataPath, blueprintFolder);
            if (!Directory.Exists(blueprintPath))
            {
                Directory.CreateDirectory(blueprintPath);
            }
            
            Debug.Log($"BlueprintManager initialized. Path: {blueprintPath}");
        }
        
        /// <summary>
        /// Загрузить все blueprint
        /// </summary>
        private void LoadAllBlueprints()
        {
            blueprints.Clear();
            
            if (!Directory.Exists(blueprintPath))
            {
                return;
            }
            
            string[] files = Directory.GetFiles(blueprintPath, "*" + fileExtension);
            
            foreach (string file in files)
            {
                try
                {
                    VehicleBlueprint blueprint = LoadBlueprintFromFile(file);
                    if (blueprint != null && blueprint.IsValid())
                    {
                        blueprints.Add(blueprint);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to load blueprint from {file}: {e.Message}");
                }
            }
            
            // Сортировать по дате изменения
            blueprints.Sort((a, b) => b.lastModified.CompareTo(a.lastModified));
            
            OnBlueprintListUpdated?.Invoke(blueprints);
            Debug.Log($"Loaded {blueprints.Count} blueprints");
        }
        
        /// <summary>
        /// Сохранить текущую конструкцию как blueprint
        /// </summary>
        public bool SaveCurrentVehicle(string blueprintName, string description = "")
        {
            GameObject currentVehicle = FindCurrentVehicle();
            if (currentVehicle == null)
            {
                Debug.LogWarning("No vehicle found to save");
                PlayErrorSound();
                return false;
            }
            
            // Создать blueprint
            VehicleBlueprint blueprint = VehicleBlueprint.CreateFromVehicle(currentVehicle);
            blueprint.blueprintName = blueprintName;
            blueprint.description = description;
            blueprint.author = "Player"; // В будущем можно добавить систему пользователей
            
            // Сохранить в файл
            if (SaveBlueprintToFile(blueprint))
            {
                currentBlueprint = blueprint;
                blueprints.Add(blueprint);
                
                // Сортировать по дате
                blueprints.Sort((a, b) => b.lastModified.CompareTo(a.lastModified));
                
                OnBlueprintSaved?.Invoke(blueprint);
                OnBlueprintListUpdated?.Invoke(blueprints);
                
                PlaySaveSound();
                Debug.Log($"Blueprint saved: {blueprintName}");
                return true;
            }
            
            PlayErrorSound();
            return false;
        }
        
        /// <summary>
        /// Загрузить blueprint
        /// </summary>
        public GameObject LoadBlueprint(VehicleBlueprint blueprint, Vector3 position, Quaternion rotation)
        {
            if (blueprint == null || !blueprint.IsValid())
            {
                Debug.LogError("Invalid blueprint");
                PlayErrorSound();
                return null;
            }
            
            try
            {
                // Удалить текущую машину
                ClearCurrentVehicle();
                
                // Создать новую машину
                GameObject vehicle = blueprint.CreateVehicle(position, rotation);
                
                currentBlueprint = blueprint;
                OnBlueprintLoaded?.Invoke(blueprint);
                
                PlayLoadSound();
                Debug.Log($"Blueprint loaded: {blueprint.blueprintName}");
                
                return vehicle;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load blueprint: {e.Message}");
                PlayErrorSound();
                return null;
            }
        }
        
        /// <summary>
        /// Удалить blueprint
        /// </summary>
        public bool DeleteBlueprint(VehicleBlueprint blueprint)
        {
            if (blueprint == null)
            {
                return false;
            }
            
            try
            {
                string fileName = GetBlueprintFileName(blueprint);
                string filePath = Path.Combine(blueprintPath, fileName);
                
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                
                blueprints.Remove(blueprint);
                OnBlueprintDeleted?.Invoke(blueprint);
                OnBlueprintListUpdated?.Invoke(blueprints);
                
                PlayDeleteSound();
                Debug.Log($"Blueprint deleted: {blueprint.blueprintName}");
                
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to delete blueprint: {e.Message}");
                PlayErrorSound();
                return false;
            }
        }
        
        /// <summary>
        /// Найти текущую машину в сцене
        /// </summary>
        private GameObject FindCurrentVehicle()
        {
            // Ищем объект с PartController (первая деталь)
            PartController[] parts = FindObjectsOfType<PartController>();
            if (parts.Length > 0)
            {
                return parts[0].transform.root.gameObject;
            }
            
            return null;
        }
        
        /// <summary>
        /// Очистить текущую машину
        /// </summary>
        private void ClearCurrentVehicle()
        {
            GameObject currentVehicle = FindCurrentVehicle();
            if (currentVehicle != null)
            {
                DestroyImmediate(currentVehicle);
            }
        }
        
        /// <summary>
        /// Автосохранение текущего blueprint
        /// </summary>
        private void AutoSaveCurrentBlueprint()
        {
            if (currentBlueprint != null)
            {
                GameObject currentVehicle = FindCurrentVehicle();
                if (currentVehicle != null)
                {
                    // Обновить blueprint
                    VehicleBlueprint updatedBlueprint = VehicleBlueprint.CreateFromVehicle(currentVehicle);
                    updatedBlueprint.blueprintName = currentBlueprint.blueprintName;
                    updatedBlueprint.description = currentBlueprint.description;
                    updatedBlueprint.author = currentBlueprint.author;
                    updatedBlueprint.creationDate = currentBlueprint.creationDate;
                    
                    // Сохранить
                    if (SaveBlueprintToFile(updatedBlueprint))
                    {
                        currentBlueprint = updatedBlueprint;
                        lastAutoSaveTime = Time.time;
                        Debug.Log($"Auto-saved blueprint: {currentBlueprint.blueprintName}");
                    }
                }
            }
        }
        
        /// <summary>
        /// Сохранить blueprint в файл
        /// </summary>
        private bool SaveBlueprintToFile(VehicleBlueprint blueprint)
        {
            try
            {
                string fileName = GetBlueprintFileName(blueprint);
                string filePath = Path.Combine(blueprintPath, fileName);
                
                // Конвертировать в JSON
                string json = JsonUtility.ToJson(blueprint, true);
                
                // Записать в файл
                File.WriteAllText(filePath, json);
                
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save blueprint: {e.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Загрузить blueprint из файла
        /// </summary>
        private VehicleBlueprint LoadBlueprintFromFile(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                VehicleBlueprint blueprint = JsonUtility.FromJson<VehicleBlueprint>(json);
                return blueprint;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load blueprint from {filePath}: {e.Message}");
                return null;
            }
        }
        
        /// <summary>
        /// Получить имя файла для blueprint
        /// </summary>
        private string GetBlueprintFileName(VehicleBlueprint blueprint)
        {
            string safeName = MakeFileNameSafe(blueprint.blueprintName);
            return $"{safeName}_{blueprint.creationDate:yyyyMMdd_HHmmss}{fileExtension}";
        }
        
        /// <summary>
        /// Сделать имя файла безопасным
        /// </summary>
        private string MakeFileNameSafe(string fileName)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char c in invalidChars)
            {
                fileName = fileName.Replace(c, '_');
            }
            return fileName;
        }
        
        /// <summary>
        /// Получить все blueprint
        /// </summary>
        public List<VehicleBlueprint> GetAllBlueprints()
        {
            return new List<VehicleBlueprint>(blueprints);
        }
        
        /// <summary>
        /// Получить blueprint по имени
        /// </summary>
        public VehicleBlueprint GetBlueprintByName(string name)
        {
            return blueprints.Find(b => b.blueprintName == name);
        }
        
        /// <summary>
        /// Получить текущий blueprint
        /// </summary>
        public VehicleBlueprint GetCurrentBlueprint()
        {
            return currentBlueprint;
        }
        
        /// <summary>
        /// Проверить, есть ли несохраненные изменения
        /// </summary>
        public bool HasUnsavedChanges()
        {
            if (currentBlueprint == null)
            {
                return false;
            }
            
            GameObject currentVehicle = FindCurrentVehicle();
            if (currentVehicle == null)
            {
                return false;
            }
            
            // Простая проверка - можно улучшить
            return currentVehicle.name != currentBlueprint.blueprintName;
        }
        
        /// <summary>
        /// Экспортировать blueprint
        /// </summary>
        public bool ExportBlueprint(VehicleBlueprint blueprint, string exportPath)
        {
            try
            {
                string json = JsonUtility.ToJson(blueprint, true);
                File.WriteAllText(exportPath, json);
                
                Debug.Log($"Blueprint exported to: {exportPath}");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to export blueprint: {e.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Импортировать blueprint
        /// </summary>
        public VehicleBlueprint ImportBlueprint(string importPath)
        {
            try
            {
                VehicleBlueprint blueprint = LoadBlueprintFromFile(importPath);
                if (blueprint != null && blueprint.IsValid())
                {
                    // Добавить в список
                    blueprints.Add(blueprint);
                    blueprints.Sort((a, b) => b.lastModified.CompareTo(a.lastModified));
                    
                    OnBlueprintListUpdated?.Invoke(blueprints);
                    Debug.Log($"Blueprint imported: {blueprint.blueprintName}");
                    
                    return blueprint;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to import blueprint: {e.Message}");
            }
            
            return null;
        }
        
        /// <summary>
        /// Очистить все blueprint
        /// </summary>
        public void ClearAllBlueprints()
        {
            try
            {
                foreach (VehicleBlueprint blueprint in blueprints.ToArray())
                {
                    DeleteBlueprint(blueprint);
                }
                
                blueprints.Clear();
                currentBlueprint = null;
                OnBlueprintListUpdated?.Invoke(blueprints);
                
                Debug.Log("All blueprints cleared");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to clear blueprints: {e.Message}");
            }
        }
        
        #region Audio Methods
        
        private void PlaySaveSound()
        {
            if (saveSound != null)
            {
                AudioSource.PlayClipAtPoint(saveSound, Camera.main.transform.position);
            }
        }
        
        private void PlayLoadSound()
        {
            if (loadSound != null)
            {
                AudioSource.PlayClipAtPoint(loadSound, Camera.main.transform.position);
            }
        }
        
        private void PlayDeleteSound()
        {
            if (deleteSound != null)
            {
                AudioSource.PlayClipAtPoint(deleteSound, Camera.main.transform.position);
            }
        }
        
        private void PlayErrorSound()
        {
            if (errorSound != null)
            {
                AudioSource.PlayClipAtPoint(errorSound, Camera.main.transform.position);
            }
        }
        
        #endregion
        
        #region UI Methods
        
        /// <summary>
        /// Обновить UI список blueprint
        /// </summary>
        public void UpdateBlueprintListUI()
        {
            if (blueprintListContent == null || blueprintItemPrefab == null)
            {
                return;
            }
            
            // Очистить список
            foreach (Transform child in blueprintListContent)
            {
                Destroy(child.gameObject);
            }
            
            // Добавить элементы
            foreach (VehicleBlueprint blueprint in blueprints)
            {
                GameObject item = Instantiate(blueprintItemPrefab, blueprintListContent);
                BlueprintListItem listItem = item.GetComponent<BlueprintListItem>();
                
                if (listItem != null)
                {
                    listItem.Initialize(blueprint, this);
                }
            }
        }
        
        /// <summary>
        /// Показать панель blueprint
        /// </summary>
        public void ShowBlueprintPanel()
        {
            if (blueprintPanel != null)
            {
                blueprintPanel.SetActive(true);
                UpdateBlueprintListUI();
            }
        }
        
        /// <summary>
        /// Скрыть панель blueprint
        /// </summary>
        public void HideBlueprintPanel()
        {
            if (blueprintPanel != null)
            {
                blueprintPanel.SetActive(false);
            }
        }
        
        #endregion
        
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus && autoSave && currentBlueprint != null)
            {
                AutoSaveCurrentBlueprint();
            }
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus && autoSave && currentBlueprint != null)
            {
                AutoSaveCurrentBlueprint();
            }
        }
    }
}
