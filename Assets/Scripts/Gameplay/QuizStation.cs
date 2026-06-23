using UnityEngine;
using EduQuest.Data;
using EduQuest.UI;

namespace EduQuest.Gameplay
{
    /// <summary>
    /// Satu stasiun quiz yang tersebar di ruangan. Pemain mendekat lalu menekan E
    /// untuk menjawab SATU soal. Setelah dijawab, stasiun dinonaktifkan dan
    /// melapor ke LevelManager.
    /// Collider pada objek ini WAJIB Is Trigger.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class QuizStation : MonoBehaviour, IInteractable
    {
        [Header("Data")]
        public QuizData quizData;

        [Tooltip("Index soal yang dipakai stasiun ini")]
        public int questionIndex;

        [Tooltip("Nomor stasiun untuk ditampilkan (1-based)")]
        public int stationNumber = 1;

        [Tooltip("Total stasiun di level")]
        public int totalStations = 3;

        [Header("Visual")]
        [Tooltip("Renderer yang warnanya diubah saat selesai (mis. bola organ di stasiun)")]
        public Renderer glowRenderer;
        public Color completedColor = new Color(0.2f, 1f, 0.3f);
        [Tooltip("Objek yang dimatikan saat stasiun selesai (mis. cahaya/penanda)")]
        public GameObject activeIndicator;

        [Header("Pengaturan")]
        public string playerTag = "Player";

        private bool answered;

        public bool CanInteract => !answered && quizData != null;

        private void OnTriggerEnter(Collider other)
        {
            if (answered || !other.CompareTag(playerTag)) return;

            var pi = other.GetComponentInParent<PlayerInteraction>();
            if (pi != null) pi.SetInteractable(this);

            UIManager.Instance?.ShowInteractPrompt(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(playerTag)) return;

            var pi = other.GetComponentInParent<PlayerInteraction>();
            if (pi != null) pi.ClearInteractable(this);

            UIManager.Instance?.ShowInteractPrompt(false);
        }

        /// <summary>Dipanggil PlayerInteraction saat pemain menekan E di dekat stasiun.</summary>
        public void Interact()
        {
            if (!CanInteract) return;

            UIManager.Instance?.ShowInteractPrompt(false);

            int idx = Mathf.Clamp(questionIndex, 0, quizData.questions.Count - 1);
            Question q = quizData.questions[idx];

            QuizManager.Instance?.ShowSingleQuestion(q, stationNumber - 1, totalStations, OnAnswered);
        }

        private void OnAnswered(bool isCorrect)
        {
            answered = true;

            // Ubah visual menjadi "selesai"
            if (glowRenderer != null)
            {
                var mat = glowRenderer.material; // instance, aman diubah saat runtime
                mat.color = completedColor;
                if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", completedColor);
                if (mat.HasProperty("_EmissionColor")) mat.SetColor("_EmissionColor", completedColor);
            }
            if (activeIndicator != null) activeIndicator.SetActive(false);

            LevelManager.Instance?.HandleStationComplete(isCorrect);
        }
    }
}
