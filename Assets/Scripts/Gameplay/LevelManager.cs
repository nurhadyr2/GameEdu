using UnityEngine;
using EduQuest.Core;
using EduQuest.Data;
using EduQuest.UI;

namespace EduQuest.Gameplay
{
    /// <summary>
    /// Mengoordinasi satu level berbasis eksplorasi:
    /// melacak berapa stasiun quiz yang sudah diselesaikan, menghitung bonus
    /// "semua benar", dan menampilkan result screen saat semua stasiun selesai.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [Header("Data Level")]
        public QuizData quizData;

        [Tooltip("Jumlah stasiun quiz di level ini")]
        public int totalStations = 3;

        private int answeredStations;
        private int correctStations;
        private bool levelEnded;

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else Instance = this;
        }

        private void Start()
        {
            UIManager.Instance?.UpdateStationProgress(0, totalStations);
        }

        /// <summary>Dipanggil oleh QuizStation setelah pemain menjawab soal stasiun itu.</summary>
        public void HandleStationComplete(bool isCorrect)
        {
            if (levelEnded) return;

            answeredStations++;
            if (isCorrect) correctStations++;

            UIManager.Instance?.UpdateStationProgress(answeredStations, totalStations);

            // Kalah jika nyawa habis
            if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            {
                EndLevel(false);
                return;
            }

            // Semua stasiun selesai -> level beres
            if (answeredStations >= totalStations)
            {
                if (correctStations >= totalStations)
                    GameManager.Instance?.AddPerfectBonus(); // bonus +20
                EndLevel(true);
            }
        }

        private void EndLevel(bool passed)
        {
            levelEnded = true;

            var gm = GameManager.Instance;
            bool reallyPassed = passed && (gm == null || !gm.IsGameOver);

            if (reallyPassed && gm != null && quizData != null)
                gm.MarkLevelCompleted(quizData.organName);

            // Tampilkan kursor untuk klik result
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            UIManager.Instance?.ShowResult(
                reallyPassed,
                onNextLevel: () => SceneLoader.Instance?.LoadNextScene(),
                onRetry: () => SceneLoader.Instance?.ReloadCurrentScene(),
                onMainMenu: () => SceneLoader.Instance?.LoadMainMenu());
        }
    }
}
