using UnityEngine;
using TMPro;
using EduQuest.Core;

namespace EduQuest.UI
{
    /// <summary>
    /// Layar akhir (EndScreen) setelah semua level selesai / game over.
    /// Menampilkan skor total dan kondisi menang/kalah sesuai GDD.
    /// </summary>
    public class EndScreenController : MonoBehaviour
    {
        [Header("Teks")]
        public TMP_Text titleText;
        public TMP_Text scoreText;
        public TMP_Text statsText;
        public TMP_Text messageText;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 1f;

            var gm = GameManager.Instance;
            if (gm == null) return;

            bool win = !gm.IsGameOver; // kalah jika nyawa habis

            if (titleText) titleText.text = win ? "SELAMAT! 🎉" : "GAME OVER";
            if (scoreText) scoreText.text = $"Skor Akhir: {gm.Score}";
            if (statsText) statsText.text = $"Benar: {gm.CorrectAnswers}   |   Salah: {gm.WrongAnswers}";

            if (messageText)
            {
                messageText.text = win
                    ? "Kamu berhasil menjelajahi seluruh sistem organ manusia!\nTeruslah belajar dan jaga kesehatanmu! 💪"
                    : "Nyawamu habis. Jangan menyerah, coba lagi ya! 💙";
            }
        }

        /// <summary>Tombol Main Lagi: reset & kembali ke menu.</summary>
        public void OnPlayAgain()
        {
            GameManager.Instance?.ResetGame();
            SceneLoader.Instance?.LoadMainMenu();
        }

        public void OnMainMenu()
        {
            SceneLoader.Instance?.LoadMainMenu();
        }

        public void OnQuit()
        {
            Application.Quit();
        }
    }
}
