using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class SettingsManager : MonoBehaviour
{
    [Header("Volumen")]    
    public Slider volumeSlider;
    public RawImage muteImage;

    [Header("Pantalla completa")]
    public Toggle fullscreenToggle;

    [Header("Calidad gráfica")]
    public TMP_Dropdown qualityDropdown;

    [Header("Resolución")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    private const string KEY_VOLUME = "volumenAudio";
    private const string KEY_FULLSCREEN = "fullScreen";
    private const string KEY_QUALITY = "numeroDeCalidad";
    private const string KEY_RESOLUTION = "numeroResolucion";

    void Start()
    {
        // --- Volumen ---
        float savedVolume = PlayerPrefs.GetFloat(KEY_VOLUME, 0.5f);
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;
        UpdateMuteIcon(savedVolume);

        // --- Pantalla completa ---
        bool savedFullscreen = PlayerPrefs.GetInt(KEY_FULLSCREEN, 1) == 1;
        Screen.fullScreen = savedFullscreen;
        fullscreenToggle.isOn = savedFullscreen;

        // --- Calidad gráfica ---
        int savedQuality = PlayerPrefs.GetInt(KEY_QUALITY, 5); // Predefinido a 5
        qualityDropdown.value = savedQuality;
        ApplyQuality(savedQuality);

        // --- Resolución ---
        resolutions = Screen.resolutions;

        // Filtrar resoluciones duplicadas
        resolutions = resolutions.GroupBy(r => new { r.width, r.height })
                                 .Select(g => g.Last())
                                 .ToArray();

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int defaultResolutionIndex = resolutions.Length - 1; // Predeterminado a la mayor resolución

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }

        resolutionDropdown.AddOptions(options);

        // Cargar resolución guardada o usar la mayor por defecto
        int savedResolution = PlayerPrefs.GetInt(KEY_RESOLUTION, defaultResolutionIndex);
        resolutionDropdown.value = Mathf.Clamp(savedResolution, 0, resolutions.Length - 1);
        resolutionDropdown.RefreshShownValue();

        // Aplicar la resolución inicial
        OnResolutionChange(resolutionDropdown.value);

        // Suscripción de eventos
        volumeSlider.onValueChanged.AddListener(OnVolumeChange);
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenChange);
        qualityDropdown.onValueChanged.AddListener(OnQualityChange);
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChange);
    }

    // --- Cambiar volumen ---
    private void OnVolumeChange(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat(KEY_VOLUME, value);
        UpdateMuteIcon(value);
    }

    private void UpdateMuteIcon(float value)
    {
        muteImage.enabled = value == 0f;
    }

    // --- Cambiar pantalla completa ---
    private void OnFullscreenChange(bool value)
    {
        Screen.fullScreen = value;
        PlayerPrefs.SetInt(KEY_FULLSCREEN, value ? 1 : 0);
    }

    // --- Cambiar calidad gráfica ---
    private void OnQualityChange(int value)
    {
        ApplyQuality(value);
        PlayerPrefs.SetInt(KEY_QUALITY, value);
    }

    private void ApplyQuality(int value)
    {
        QualitySettings.SetQualityLevel(value);
    }

    // --- Cambiar resolución ---
    private void OnResolutionChange(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt(KEY_RESOLUTION, index);
    }
}