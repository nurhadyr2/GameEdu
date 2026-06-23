using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EduQuest.Core
{
    /// <summary>
    /// Mengatur perpindahan scene beserta transisi fade in/out sederhana.
    /// Urutan scene: MainMenu > Level1_Jantung > Level2_ParuParu > Level3_Otak > EndScreen
    /// PENTING: daftarkan semua scene tersebut di Build Settings (File > Build Settings).
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }

        [Header("Transisi Fade")]
        [Tooltip("CanvasGroup atau Image hitam full-screen untuk efek fade. Boleh dikosongkan (langsung pindah tanpa fade).")]
        public CanvasGroup fadeCanvasGroup;

        [Tooltip("Durasi fade (detik)")]
        public float fadeDuration = 0.5f;

        // Urutan scene game. Pastikan nama persis sama dengan nama file scene di Build Settings.
        public static readonly string[] SceneOrder =
        {
            "MainMenu",
            "Level1_Jantung",
            "Level2_ParuParu",
            "Level3_Otak",
            "EndScreen"
        };

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Pastikan layar mulai dalam keadaan terang (alpha 0)
            if (fadeCanvasGroup != null)
            {
                fadeCanvasGroup.alpha = 0f;
                fadeCanvasGroup.blocksRaycasts = false;
            }
        }

        /// <summary>Muat scene berdasarkan nama, dengan transisi fade.</summary>
        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneRoutine(sceneName));
        }

        /// <summary>Muat scene berikutnya sesuai urutan SceneOrder.</summary>
        public void LoadNextScene()
        {
            string current = SceneManager.GetActiveScene().name;
            int index = System.Array.IndexOf(SceneOrder, current);

            if (index >= 0 && index < SceneOrder.Length - 1)
            {
                LoadScene(SceneOrder[index + 1]);
            }
            else
            {
                // Sudah di scene terakhir, kembali ke EndScreen sebagai fallback
                LoadScene("EndScreen");
            }
        }

        public void LoadMainMenu() => LoadScene("MainMenu");

        /// <summary>Muat ulang scene yang sedang aktif (untuk tombol Retry).</summary>
        public void ReloadCurrentScene()
        {
            LoadScene(SceneManager.GetActiveScene().name);
        }

        private IEnumerator LoadSceneRoutine(string sceneName)
        {
            // Fade out (layar menjadi gelap)
            yield return Fade(1f);

            // Muat scene secara asinkron
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            while (op != null && !op.isDone)
                yield return null;

            // Fade in (layar terang kembali)
            yield return Fade(0f);
        }

        private IEnumerator Fade(float targetAlpha)
        {
            if (fadeCanvasGroup == null)
                yield break;

            fadeCanvasGroup.blocksRaycasts = true;
            float startAlpha = fadeCanvasGroup.alpha;
            float time = 0f;

            while (time < fadeDuration)
            {
                time += Time.unscaledDeltaTime; // unscaled supaya tetap jalan walau Time.timeScale = 0
                fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
                yield return null;
            }

            fadeCanvasGroup.alpha = targetAlpha;
            fadeCanvasGroup.blocksRaycasts = targetAlpha > 0.9f;
        }
    }
}
