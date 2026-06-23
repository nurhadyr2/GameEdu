using UnityEngine;
using UnityEngine.InputSystem; // Input System baru
using EduQuest.UI;

namespace EduQuest.Gameplay
{
    /// <summary>
    /// Mendeteksi input tombol "E" untuk berinteraksi dengan objek terdekat
    /// (mis. QuizStation). Juga mengatur enable/disable gerakan pemain
    /// (ThirdPersonController) saat quiz aktif.
    /// </summary>
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Referensi Kontroler Pemain")]
        [Tooltip("Drag komponen ThirdPersonController di sini (sebagai MonoBehaviour).")]
        public MonoBehaviour thirdPersonController;

        [Tooltip("Drag komponen StarterAssetsInputs di sini (opsional).")]
        public MonoBehaviour starterAssetsInputs;

        [Tooltip("Drag komponen PlayerInput di sini (opsional).")]
        public PlayerInput playerInput;

        // Objek yang sedang berada dalam jangkauan (null jika tidak ada)
        private IInteractable currentInteractable;

        private void Update()
        {
            // Jika quiz sedang berlangsung, abaikan input interaksi
            if (QuizManager.Instance != null && QuizManager.Instance.IsQuizActive)
                return;

            bool pressedE = Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame;

            if (pressedE && currentInteractable != null && currentInteractable.CanInteract)
            {
                currentInteractable.Interact();
            }
        }

        /// <summary>Set objek interaksi aktif (dipanggil dari QuizStation/TriggerZone).</summary>
        public void SetInteractable(IInteractable interactable)
        {
            currentInteractable = interactable;
        }

        /// <summary>Hapus objek interaksi jika cocok.</summary>
        public void ClearInteractable(IInteractable interactable)
        {
            if (ReferenceEquals(currentInteractable, interactable))
                currentInteractable = null;
        }

        /// <summary>Enable / disable gerakan & kamera pemain (dimatikan saat quiz aktif).</summary>
        public void SetMovementEnabled(bool enabled)
        {
            if (thirdPersonController != null) thirdPersonController.enabled = enabled;
            if (starterAssetsInputs != null) starterAssetsInputs.enabled = enabled;
            if (playerInput != null) playerInput.enabled = enabled;
        }
    }
}
