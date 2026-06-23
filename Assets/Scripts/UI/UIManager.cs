using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EduQuest.Core;
using EduQuest.Data;

namespace EduQuest.UI
{
    /// <summary>
    /// Mengelola seluruh tampilan UI: HUD, panel quiz, feedback, dan result screen.
    /// Bersifat Singleton per-scene (tidak persist) supaya QuizManager mudah memanggilnya.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("HUD")]
        [Tooltip("Teks skor (kiri atas)")]
        public TMP_Text scoreText;
        [Tooltip("Teks / kumpulan ikon hati (kanan atas)")]
        public TMP_Text livesText;
        [Tooltip("Nama level (tengah atas)")]
        public TMP_Text levelNameText;
        [Tooltip("Progres stasiun, contoh: Stasiun 1 / 3 (di bawah nama level)")]
        public TMP_Text stationProgressText;
        [Tooltip("Prompt interaksi 'Tekan E' ")]
        public GameObject interactPrompt;

        [Header("Quiz Panel")]
        public GameObject quizPanel;
        public TMP_Text questionText;
        [Tooltip("4 tombol jawaban, urutan index 0 - 3")]
        public Button[] answerButtons = new Button[4];
        [Tooltip("Label teks pada masing-masing tombol jawaban")]
        public TMP_Text[] answerLabels = new TMP_Text[4];
        [Tooltip("Teks nomor soal, contoh: Soal 1 / 3")]
        public TMP_Text questionProgressText;

        [Header("Feedback Panel")]
        public GameObject feedbackPanel;
        public TMP_Text feedbackTitleText;       // BENAR / SALAH
        public TMP_Text feedbackExplanationText; // penjelasan singkat
        public Button feedbackNextButton;        // tombol Lanjut

        [Header("Result Screen")]
        public GameObject resultPanel;
        public TMP_Text resultScoreText;
        public TMP_Text resultCorrectText;
        public TMP_Text resultWrongText;
        public TMP_Text motivationText;          // pesan motivasi
        public Button nextLevelButton;
        public Button retryButton;
        public Button mainMenuButton;

        [Header("Warna Feedback")]
        public Color correctColor = new Color(0.2f, 0.8f, 0.2f);
        public Color wrongColor = new Color(0.85f, 0.2f, 0.2f);
        public Color defaultButtonColor = Color.white;

        [Header("Info Level")]
        [Tooltip("Nama level untuk ditampilkan di HUD, contoh: Level 1 - Jantung")]
        public string levelDisplayName = "Level";

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        private void Start()
        {
            // Sembunyikan semua panel di awal
            if (quizPanel) quizPanel.SetActive(false);
            if (feedbackPanel) feedbackPanel.SetActive(false);
            if (resultPanel) resultPanel.SetActive(false);
            if (interactPrompt) interactPrompt.SetActive(false);

            if (levelNameText) levelNameText.text = levelDisplayName;

            // Subscribe ke event GameManager supaya HUD update otomatis
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnStatsChanged += UpdateHUD;
                UpdateHUD();
            }
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnStatsChanged -= UpdateHUD;
        }

        // ----------------------- HUD -----------------------

        /// <summary>Perbarui tampilan skor dan nyawa di HUD.</summary>
        public void UpdateHUD()
        {
            var gm = GameManager.Instance;
            if (gm == null) return;

            if (scoreText) scoreText.text = $"Skor: {gm.Score}";

            if (livesText)
            {
                // Tampilkan nyawa sebagai ikon hati
                string hearts = "";
                for (int i = 0; i < gm.Lives; i++) hearts += "♥ "; // ♥
                livesText.text = hearts.Trim();
            }
        }

        /// <summary>Tampilkan / sembunyikan prompt "Tekan E untuk interaksi".</summary>
        public void ShowInteractPrompt(bool show)
        {
            if (interactPrompt) interactPrompt.SetActive(show);
        }

        /// <summary>Perbarui teks progres stasiun (berapa stasiun selesai dari total).</summary>
        public void UpdateStationProgress(int done, int total)
        {
            if (stationProgressText)
                stationProgressText.text = $"Stasiun: {done} / {total}";
        }

        // ----------------------- Quiz Panel -----------------------

        /// <summary>
        /// Tampilkan satu soal. Callback onAnswerSelected dipanggil dengan index jawaban yang dipilih.
        /// </summary>
        public void ShowQuestion(Question q, int currentIndex, int total, Action<int> onAnswerSelected)
        {
            if (feedbackPanel) feedbackPanel.SetActive(false);
            if (quizPanel) quizPanel.SetActive(true);

            if (questionText) questionText.text = q.questionText;
            if (questionProgressText) questionProgressText.text = $"Soal {currentIndex + 1} / {total}";

            for (int i = 0; i < answerButtons.Length; i++)
            {
                int captured = i; // hindari closure trap
                var btn = answerButtons[i];
                if (btn == null) continue;

                btn.interactable = true;
                SetButtonColor(btn, defaultButtonColor);

                if (i < answerLabels.Length && answerLabels[i] != null && q.options != null && i < q.options.Length)
                    answerLabels[i].text = q.options[i];

                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => onAnswerSelected?.Invoke(captured));
            }
        }

        /// <summary>Beri warna tombol: hijau untuk benar, merah untuk salah. Lalu nonaktifkan semua tombol.</summary>
        public void HighlightAnswer(int selectedIndex, int correctIndex)
        {
            for (int i = 0; i < answerButtons.Length; i++)
            {
                if (answerButtons[i] == null) continue;
                answerButtons[i].interactable = false;

                if (i == correctIndex)
                    SetButtonColor(answerButtons[i], correctColor);
                else if (i == selectedIndex)
                    SetButtonColor(answerButtons[i], wrongColor);
            }
        }

        private void SetButtonColor(Button btn, Color c)
        {
            var img = btn.GetComponent<Image>();
            if (img != null) img.color = c;
        }

        // ----------------------- Feedback Panel -----------------------

        /// <summary>Tampilkan panel feedback benar/salah beserta penjelasan.</summary>
        public void ShowFeedback(bool isCorrect, string explanation, Action onNext)
        {
            if (feedbackPanel) feedbackPanel.SetActive(true);

            if (feedbackTitleText)
            {
                feedbackTitleText.text = isCorrect ? "BENAR! ✔" : "SALAH ✖";
                feedbackTitleText.color = isCorrect ? correctColor : wrongColor;
            }

            if (feedbackExplanationText) feedbackExplanationText.text = explanation;

            if (feedbackNextButton)
            {
                feedbackNextButton.onClick.RemoveAllListeners();
                feedbackNextButton.onClick.AddListener(() => onNext?.Invoke());
            }
        }

        public void HideQuiz()
        {
            if (quizPanel) quizPanel.SetActive(false);
            if (feedbackPanel) feedbackPanel.SetActive(false);
        }

        // ----------------------- Result Screen -----------------------

        /// <summary>
        /// Tampilkan layar hasil di akhir level.
        /// </summary>
        public void ShowResult(bool levelPassed, Action onNextLevel, Action onRetry, Action onMainMenu)
        {
            var gm = GameManager.Instance;
            HideQuiz();
            if (resultPanel) resultPanel.SetActive(true);

            if (gm != null)
            {
                if (resultScoreText) resultScoreText.text = $"Skor Total: {gm.Score}";
                if (resultCorrectText) resultCorrectText.text = $"Benar: {gm.CorrectAnswers}";
                if (resultWrongText) resultWrongText.text = $"Salah: {gm.WrongAnswers}";
            }

            // Pesan motivasi
            if (motivationText)
            {
                if (!levelPassed)
                    motivationText.text = "Jangan menyerah! Coba lagi ya, kamu pasti bisa! 💪";
                else if (gm != null && gm.WrongAnswers == 0)
                    motivationText.text = "Luar biasa! Kamu menguasai materi ini dengan sempurna! 🌟";
                else
                    motivationText.text = "Kerja bagus! Terus belajar dan tingkatkan pemahamanmu! 👍";
            }

            // Atur tombol-tombol
            SetupResultButton(nextLevelButton, levelPassed, onNextLevel);
            SetupResultButton(retryButton, true, onRetry);
            SetupResultButton(mainMenuButton, true, onMainMenu);
        }

        private void SetupResultButton(Button btn, bool active, Action callback)
        {
            if (btn == null) return;
            btn.gameObject.SetActive(active);
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => callback?.Invoke());
        }
    }
}
