using UnityEngine;
using UnityEngine.UI;
using EduQuest.Core;

namespace EduQuest.UI
{
    /// <summary>
    /// Mengontrol Main Menu sesuai GDD: Play, Settings, About, Exit.
    /// Juga menampilkan panel Settings (volume) dan About.
    /// </summary>
    public class MainMenuController : MonoBehaviour
    {
        [Header("Panel")]
        public GameObject mainPanel;
        public GameObject settingsPanel;
        public GameObject aboutPanel;

        [Header("Settings")]
        [Tooltip("Slider volume master (0 - 1)")]
        public Slider volumeSlider;

        [Header("Scene Tujuan")]
        public string firstLevelScene = "Level1_Jantung";

        private const string VolumeKey = "EduQuest_Volume";

        private void Start()
        {
            ShowMain();

            // Muat & terapkan volume tersimpan
            float vol = PlayerPrefs.GetFloat(VolumeKey, 1f);
            AudioListener.volume = vol;
            if (volumeSlider != null)
            {
                volumeSlider.value = vol;
                volumeSlider.onValueChanged.AddListener(SetVolume);
            }

            // Pastikan kursor terlihat di menu
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Reset waktu jika datang dari pause
            Time.timeScale = 1f;
        }

        // ---- Tombol utama ----

        /// <summary>Tombol Play: reset data game lalu masuk level pertama.</summary>
        public void OnPlay()
        {
            GameManager.Instance?.ResetGame();
            if (SceneLoader.Instance != null)
                SceneLoader.Instance.LoadScene(firstLevelScene);
        }

        public void OnExit()
        {
            Debug.Log("Keluar dari game.");
            Application.Quit();
            // Catatan: Application.Quit tidak berefek di Editor maupun WebGL.
        }

        // ---- Navigasi panel ----

        public void ShowMain()
        {
            SetPanels(main: true, settings: false, about: false);
        }

        public void ShowSettings()
        {
            SetPanels(main: false, settings: true, about: false);
        }

        public void ShowAbout()
        {
            SetPanels(main: false, settings: false, about: true);
        }

        private void SetPanels(bool main, bool settings, bool about)
        {
            if (mainPanel) mainPanel.SetActive(main);
            if (settingsPanel) settingsPanel.SetActive(settings);
            if (aboutPanel) aboutPanel.SetActive(about);
        }

        // ---- Settings ----

        public void SetVolume(float value)
        {
            AudioListener.volume = value;
            PlayerPrefs.SetFloat(VolumeKey, value);
        }
    }
}
