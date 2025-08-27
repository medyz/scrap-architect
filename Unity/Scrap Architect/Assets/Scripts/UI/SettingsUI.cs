using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace ScrapArchitect.UI
{
    /// <summary>
    /// Экран настроек
    /// </summary>
    public class SettingsUI : UIBase
    {
        [Header("Graphics Settings")]
        public TMP_Dropdown qualityDropdown;
        public TMP_Dropdown resolutionDropdown;
        public Toggle fullscreenToggle;
        public Slider brightnessSlider;
        public Slider contrastSlider;
        
        [Header("Audio Settings")]
        public Slider masterVolumeSlider;
        public Slider musicVolumeSlider;
        public Slider sfxVolumeSlider;
        public Toggle muteToggle;
        
        [Header("Gameplay Settings")]
        public Toggle tutorialToggle;
        public Toggle autoSaveToggle;
        public TMP_Dropdown languageDropdown;
        public Slider mouseSensitivitySlider;
        
        [Header("Buttons")]
        public Button applyButton;
        public Button resetButton;
        public Button backButton;
        
        [Header("Settings")]
        public float defaultBrightness = 1f;
        public float defaultContrast = 1f;
        public float defaultMasterVolume = 1f;
        public float defaultMusicVolume = 0.8f;
        public float defaultSfxVolume = 1f;
        public float defaultMouseSensitivity = 1f;
        
        private bool settingsChanged = false;
        
        private void Start()
        {
            SetupButtons();
            LoadSettings();
        }
        
        /// <summary>
        /// Настройка кнопок
        /// </summary>
        private void SetupButtons()
        {
            if (applyButton != null)
            {
                applyButton.onClick.AddListener(OnApplyButtonClick);
            }
            
            if (resetButton != null)
            {
                resetButton.onClick.AddListener(OnResetButtonClick);
            }
            
            if (backButton != null)
            {
                backButton.onClick.AddListener(OnBackButtonClick);
            }
        }
        
        /// <summary>
        /// Загрузить настройки
        /// </summary>
        private void LoadSettings()
        {
            LoadGraphicsSettings();
            LoadAudioSettings();
            LoadGameplaySettings();
            
            settingsChanged = false;
            UpdateApplyButton();
        }
        
        /// <summary>
        /// Загрузить графические настройки
        /// </summary>
        private void LoadGraphicsSettings()
        {
            // Качество
            if (qualityDropdown != null)
            {
                qualityDropdown.ClearOptions();
                qualityDropdown.AddOptions(new List<string> { "Низкое", "Среднее", "Высокое", "Ультра" });
                qualityDropdown.value = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());
                qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
            }
            
            // Разрешение
            if (resolutionDropdown != null)
            {
                SetupResolutionDropdown();
                resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", 0);
                resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
            }
            
            // Полноэкранный режим
            if (fullscreenToggle != null)
            {
                fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0) == 1;
                fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
            }
            
            // Яркость
            if (brightnessSlider != null)
            {
                brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", defaultBrightness);
                brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
            }
            
            // Контрастность
            if (contrastSlider != null)
            {
                contrastSlider.value = PlayerPrefs.GetFloat("Contrast", defaultContrast);
                contrastSlider.onValueChanged.AddListener(OnContrastChanged);
            }
        }
        
        /// <summary>
        /// Настройка выпадающего списка разрешений
        /// </summary>
        private void SetupResolutionDropdown()
        {
            resolutionDropdown.ClearOptions();
            
            Resolution[] resolutions = Screen.resolutions;
            List<string> options = new List<string>();
            
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = $"{resolutions[i].width} x {resolutions[i].height}";
                options.Add(option);
            }
            
            resolutionDropdown.AddOptions(options);
        }
        
        /// <summary>
        /// Загрузить аудио настройки
        /// </summary>
        private void LoadAudioSettings()
        {
            // Общая громкость
            if (masterVolumeSlider != null)
            {
                masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", defaultMasterVolume);
                masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            }
            
            // Громкость музыки
            if (musicVolumeSlider != null)
            {
                musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", defaultMusicVolume);
                musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            }
            
            // Громкость звуковых эффектов
            if (sfxVolumeSlider != null)
            {
                sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", defaultSfxVolume);
                sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            }
            
            // Без звука
            if (muteToggle != null)
            {
                muteToggle.isOn = PlayerPrefs.GetInt("Mute", 0) == 1;
                muteToggle.onValueChanged.AddListener(OnMuteChanged);
            }
        }
        
        /// <summary>
        /// Загрузить игровые настройки
        /// </summary>
        private void LoadGameplaySettings()
        {
            // Туториал
            if (tutorialToggle != null)
            {
                tutorialToggle.isOn = PlayerPrefs.GetInt("TutorialEnabled", 1) == 1;
                tutorialToggle.onValueChanged.AddListener(OnTutorialChanged);
            }
            
            // Автосохранение
            if (autoSaveToggle != null)
            {
                autoSaveToggle.isOn = PlayerPrefs.GetInt("AutoSave", 1) == 1;
                autoSaveToggle.onValueChanged.AddListener(OnAutoSaveChanged);
            }
            
            // Язык
            if (languageDropdown != null)
            {
                languageDropdown.ClearOptions();
                languageDropdown.AddOptions(new List<string> { "Русский", "English" });
                languageDropdown.value = PlayerPrefs.GetInt("Language", 0);
                languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
            }
            
            // Чувствительность мыши
            if (mouseSensitivitySlider != null)
            {
                mouseSensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity", defaultMouseSensitivity);
                mouseSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivityChanged);
            }
        }
        
        /// <summary>
        /// Применить настройки
        /// </summary>
        public void ApplySettings()
        {
            ApplyGraphicsSettings();
            ApplyAudioSettings();
            ApplyGameplaySettings();
            
            PlayerPrefs.Save();
            settingsChanged = false;
            UpdateApplyButton();
        }
        
        /// <summary>
        /// Применить графические настройки
        /// </summary>
        private void ApplyGraphicsSettings()
        {
            if (qualityDropdown != null)
            {
                QualitySettings.SetQualityLevel(qualityDropdown.value);
                PlayerPrefs.SetInt("QualityLevel", qualityDropdown.value);
            }
            
            if (resolutionDropdown != null)
            {
                Resolution[] resolutions = Screen.resolutions;
                if (resolutionDropdown.value < resolutions.Length)
                {
                    Resolution resolution = resolutions[resolutionDropdown.value];
                    Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
                    PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
                }
            }
            
            if (fullscreenToggle != null)
            {
                Screen.fullScreen = fullscreenToggle.isOn;
                PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
            }
            
            if (brightnessSlider != null)
            {
                PlayerPrefs.SetFloat("Brightness", brightnessSlider.value);
            }
            
            if (contrastSlider != null)
            {
                PlayerPrefs.SetFloat("Contrast", contrastSlider.value);
            }
        }
        
        /// <summary>
        /// Применить аудио настройки
        /// </summary>
        private void ApplyAudioSettings()
        {
            if (masterVolumeSlider != null)
            {
                PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
            }
            
            if (musicVolumeSlider != null)
            {
                PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
            }
            
            if (sfxVolumeSlider != null)
            {
                PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
            }
            
            if (muteToggle != null)
            {
                PlayerPrefs.SetInt("Mute", muteToggle.isOn ? 1 : 0);
            }
        }
        
        /// <summary>
        /// Применить игровые настройки
        /// </summary>
        private void ApplyGameplaySettings()
        {
            if (tutorialToggle != null)
            {
                PlayerPrefs.SetInt("TutorialEnabled", tutorialToggle.isOn ? 1 : 0);
            }
            
            if (autoSaveToggle != null)
            {
                PlayerPrefs.SetInt("AutoSave", autoSaveToggle.isOn ? 1 : 0);
            }
            
            if (languageDropdown != null)
            {
                PlayerPrefs.SetInt("Language", languageDropdown.value);
            }
            
            if (mouseSensitivitySlider != null)
            {
                PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivitySlider.value);
            }
        }
        
        /// <summary>
        /// Сбросить настройки
        /// </summary>
        public void ResetSettings()
        {
            if (qualityDropdown != null) qualityDropdown.value = 2; // Высокое качество
            if (resolutionDropdown != null) resolutionDropdown.value = 0;
            if (fullscreenToggle != null) fullscreenToggle.isOn = true;
            if (brightnessSlider != null) brightnessSlider.value = defaultBrightness;
            if (contrastSlider != null) contrastSlider.value = defaultContrast;
            
            if (masterVolumeSlider != null) masterVolumeSlider.value = defaultMasterVolume;
            if (musicVolumeSlider != null) musicVolumeSlider.value = defaultMusicVolume;
            if (sfxVolumeSlider != null) sfxVolumeSlider.value = defaultSfxVolume;
            if (muteToggle != null) muteToggle.isOn = false;
            
            if (tutorialToggle != null) tutorialToggle.isOn = true;
            if (autoSaveToggle != null) autoSaveToggle.isOn = true;
            if (languageDropdown != null) languageDropdown.value = 0;
            if (mouseSensitivitySlider != null) mouseSensitivitySlider.value = defaultMouseSensitivity;
            
            settingsChanged = true;
            UpdateApplyButton();
        }
        
        /// <summary>
        /// Обновить состояние кнопки "Применить"
        /// </summary>
        private void UpdateApplyButton()
        {
            if (applyButton != null)
            {
                applyButton.interactable = settingsChanged;
            }
        }
        
        /// <summary>
        /// Отметить, что настройки изменились
        /// </summary>
        private void MarkSettingsChanged()
        {
            settingsChanged = true;
            UpdateApplyButton();
        }
        
        #region Event Handlers
        
        private void OnQualityChanged(int value) => MarkSettingsChanged();
        private void OnResolutionChanged(int value) => MarkSettingsChanged();
        private void OnFullscreenChanged(bool value) => MarkSettingsChanged();
        private void OnBrightnessChanged(float value) => MarkSettingsChanged();
        private void OnContrastChanged(float value) => MarkSettingsChanged();
        private void OnMasterVolumeChanged(float value) => MarkSettingsChanged();
        private void OnMusicVolumeChanged(float value) => MarkSettingsChanged();
        private void OnSFXVolumeChanged(float value) => MarkSettingsChanged();
        private void OnMuteChanged(bool value) => MarkSettingsChanged();
        private void OnTutorialChanged(bool value) => MarkSettingsChanged();
        private void OnAutoSaveChanged(bool value) => MarkSettingsChanged();
        private void OnLanguageChanged(int value) => MarkSettingsChanged();
        private void OnMouseSensitivityChanged(float value) => MarkSettingsChanged();
        
        #endregion
        
        #region Button Handlers
        
        /// <summary>
        /// Обработчик кнопки "Применить"
        /// </summary>
        public void OnApplyButtonClick()
        {
            if (uiManager != null)
            {
                uiManager.PlayButtonClickSound();
                ApplySettings();
            }
        }
        
        /// <summary>
        /// Обработчик кнопки "Сбросить"
        /// </summary>
        public void OnResetButtonClick()
        {
            if (uiManager != null)
            {
                uiManager.PlayButtonClickSound();
                ResetSettings();
            }
        }
        
        /// <summary>
        /// Обработчик кнопки "Назад"
        /// </summary>
        public override void OnBackButtonClick()
        {
            if (uiManager != null)
            {
                uiManager.PlayButtonClickSound();
                
                if (settingsChanged)
                {
                    // Показать диалог подтверждения
                    Debug.Log("Настройки не сохранены. Применить изменения?");
                    // TODO: Добавить диалог подтверждения
                }
                
                uiManager.GoBack();
            }
        }
        
        #endregion
        
        /// <summary>
        /// Обработка нажатия клавиши Escape
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnBackButtonClick();
            }
        }
    }
}
