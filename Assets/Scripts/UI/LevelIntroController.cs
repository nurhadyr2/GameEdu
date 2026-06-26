using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarterAssets;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace EduQuest.UI
{
    public class LevelIntroController : MonoBehaviour
    {
        [Header("Panel Intro")]
        public GameObject introPanel;

        [Header("Konten Materi")]
        public TMP_Text titleText;
        [TextArea(3, 10)]
        public string judul = "Stage 1: Jantung";

        public TMP_Text bodyText;
        [TextArea(5, 20)]
        public string materi =
            "Jantung adalah organ berotot yang memompa darah ke seluruh tubuh...";

        [Header("Opsi")]
        public bool pauseGameplay = true;
        public bool showCursorDuringIntro = true;
        public bool disablePlayerDuringIntro = true;

        private StarterAssetsInputs _starterInputs;
        private bool _starterCursorLockedBefore;
#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif

        private void Start()
        {
            ShowIntro();
        }

        public void ShowIntro()
        {
            if (titleText != null) titleText.text = judul;
            if (bodyText != null) bodyText.text = materi;

            if (introPanel != null) introPanel.SetActive(true);

            if (pauseGameplay) Time.timeScale = 0f;

            if (disablePlayerDuringIntro)
            {
                _starterInputs = FindObjectOfType<StarterAssetsInputs>();
                if (_starterInputs != null)
                {
                    _starterCursorLockedBefore = _starterInputs.cursorLocked;
                    _starterInputs.cursorLocked = false;
#if ENABLE_INPUT_SYSTEM
                    _playerInput = _starterInputs.GetComponent<PlayerInput>();
                    if (_playerInput != null) _playerInput.enabled = false;
#endif
                }
            }

            if (showCursorDuringIntro)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void OnStartLevel()
        {
            if (introPanel != null) introPanel.SetActive(false);

            if (pauseGameplay) Time.timeScale = 1f;

            if (_starterInputs != null)
            {
                _starterInputs.cursorLocked = _starterCursorLockedBefore;
#if ENABLE_INPUT_SYSTEM
                if (_playerInput != null) _playerInput.enabled = true;
#endif
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
