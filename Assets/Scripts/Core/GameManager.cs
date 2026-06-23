using System.Collections.Generic;
using UnityEngine;

namespace EduQuest.Core
{
    /// <summary>
    /// GameManager bersifat Singleton dan persist antar scene (DontDestroyOnLoad).
    /// Bertugas menyimpan skor, nyawa, dan progres level yang sudah diselesaikan.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        // Instance tunggal yang bisa diakses dari mana saja
        public static GameManager Instance { get; private set; }

        [Header("Pengaturan Awal")]
        [Tooltip("Jumlah nyawa default saat game dimulai")]
        public int startingLives = 3;

        // Data runtime
        public int Score { get; private set; }
        public int Lives { get; private set; }

        // Statistik jawaban (untuk result screen)
        public int CorrectAnswers { get; private set; }
        public int WrongAnswers { get; private set; }

        // Daftar nama level yang sudah diselesaikan
        private readonly HashSet<string> completedLevels = new HashSet<string>();

        // Event sederhana supaya UI bisa update otomatis saat skor / nyawa berubah
        public System.Action OnStatsChanged;

        private void Awake()
        {
            // Pola Singleton: kalau sudah ada instance lain, hancurkan duplikat
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            ResetGame();
        }

        /// <summary>Reset seluruh data game ke kondisi awal (dipakai saat New Game / Retry total).</summary>
        public void ResetGame()
        {
            Score = 0;
            Lives = startingLives;
            CorrectAnswers = 0;
            WrongAnswers = 0;
            completedLevels.Clear();
            OnStatsChanged?.Invoke();
        }

        /// <summary>Tambah skor sebesar amount.</summary>
        public void AddScore(int amount)
        {
            Score += amount;
            OnStatsChanged?.Invoke();
        }

        /// <summary>Catat jawaban benar (+10 skor) dan update statistik.</summary>
        public void RegisterCorrectAnswer()
        {
            CorrectAnswers++;
            AddScore(10);
        }

        /// <summary>Catat jawaban salah (kurangi 1 nyawa) dan update statistik.</summary>
        public void RegisterWrongAnswer()
        {
            WrongAnswers++;
            Lives = Mathf.Max(0, Lives - 1);
            OnStatsChanged?.Invoke();
        }

        /// <summary>Bonus +20 jika seluruh soal dalam satu level dijawab benar.</summary>
        public void AddPerfectBonus()
        {
            AddScore(20);
        }

        /// <summary>Tandai sebuah level sudah selesai.</summary>
        public void MarkLevelCompleted(string levelName)
        {
            if (!string.IsNullOrEmpty(levelName))
                completedLevels.Add(levelName);
        }

        public bool IsLevelCompleted(string levelName) => completedLevels.Contains(levelName);

        /// <summary>Kondisi kalah: nyawa habis.</summary>
        public bool IsGameOver => Lives <= 0;

        /// <summary>Kondisi menang dipanggil saat semua level target selesai (dicek oleh SceneLoader/UI).</summary>
        public int CompletedLevelCount => completedLevels.Count;
    }
}
