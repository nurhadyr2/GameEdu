using UnityEngine;
using UnityEngine.InputSystem; // Input System baru
using EduQuest.Core;
using EduQuest.Gameplay;

namespace EduQuest.UI
{
    /// <summary>
    /// Pause Menu sesuai GDD: Resume, Restart, Main Menu.
    /// Tombol Esc untuk toggle. Tidak bisa pause saat quiz sedang aktif.
    /// </summary>
    public class PauseMenu : MonoBehaviour
    {
        [Header("Panel Pause")]
        public GameObject pausePanel;

        public bool IsPaused { get; private set; }

        private void Start()
        {
            if (pausePanel) pausePanel.SetActive(false);
        }

        private void Update()
        {
            // Deteksi tombol Esc (Input System baru)
            bool escPressed = Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;

            // Jangan boleh pause saat quiz berlangsung
            bool quizActive = QuizManager.Instance != null && QuizManager.Instance.IsQuizActive;

            if (escPressed && !quizActive)
            {
                if (IsPaused) Resume();
                else Pause();
            }
        }

        public void Pause()
        {
            IsPaused = true;
            if (pausePanel) pausePanel.SetActive(true);
            Time.timeScale = 0f; // hentikan game
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void Resume()
        {
            IsPaused = false;
            if (pausePanel) pausePanel.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void Restart()
        {
            Time.timeScale = 1f;
            SceneLoader.Instance?.ReloadCurrentScene();
        }

        public void GoToMainMenu()
        {
            Time.timeScale = 1f;
            SceneLoader.Instance?.LoadMainMenu();
        }
    }
}
