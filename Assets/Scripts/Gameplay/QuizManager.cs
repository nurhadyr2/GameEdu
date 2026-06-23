using System;
using UnityEngine;
using EduQuest.Core;
using EduQuest.Data;
using EduQuest.UI;

namespace EduQuest.Gameplay
{
    /// <summary>
    /// Menangani tampilan & pemeriksaan SATU soal quiz (mode stasiun eksplorasi).
    /// Saat quiz aktif, gerakan pemain dimatikan. Setelah selesai, hasil benar/salah
    /// dilaporkan balik ke pemanggil (QuizStation) lewat callback.
    /// </summary>
    public class QuizManager : MonoBehaviour
    {
        public static QuizManager Instance { get; private set; }

        [Header("Referensi")]
        [Tooltip("Untuk menonaktifkan gerakan pemain saat quiz aktif")]
        public PlayerInteraction playerInteraction;

        public bool IsQuizActive { get; private set; }

        private Question currentQuestion;
        private Action<bool> onComplete;
        private bool answered;

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else Instance = this;
        }

        /// <summary>
        /// Tampilkan satu soal. stationIndex & total dipakai untuk teks "Stasiun x / y".
        /// onComplete(true/false) dipanggil setelah pemain menutup feedback.
        /// </summary>
        public void ShowSingleQuestion(Question q, int stationIndex, int total, Action<bool> onComplete)
        {
            if (IsQuizActive || q == null) return;

            IsQuizActive = true;
            currentQuestion = q;
            this.onComplete = onComplete;
            answered = false;

            // Matikan gerakan pemain + tampilkan kursor untuk klik UI
            if (playerInteraction != null) playerInteraction.SetMovementEnabled(false);
            SetCursor(true);

            UIManager.Instance?.ShowInteractPrompt(false);
            UIManager.Instance?.ShowQuestion(q, stationIndex, total, OnAnswerSelected);
        }

        private void OnAnswerSelected(int selectedIndex)
        {
            if (answered) return;
            answered = true;

            bool isCorrect = selectedIndex == currentQuestion.correctIndex;

            // Skor / nyawa
            if (isCorrect) GameManager.Instance?.RegisterCorrectAnswer(); // +10
            else GameManager.Instance?.RegisterWrongAnswer();             // -1 nyawa

            UIManager.Instance?.HighlightAnswer(selectedIndex, currentQuestion.correctIndex);
            UIManager.Instance?.ShowFeedback(isCorrect, currentQuestion.explanation, () => Finish(isCorrect));
        }

        private void Finish(bool isCorrect)
        {
            UIManager.Instance?.HideQuiz();

            IsQuizActive = false;
            if (playerInteraction != null) playerInteraction.SetMovementEnabled(true);
            SetCursor(false);

            var cb = onComplete;
            onComplete = null;
            cb?.Invoke(isCorrect);
        }

        private void SetCursor(bool unlock)
        {
            Cursor.lockState = unlock ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = unlock;
        }
    }
}
