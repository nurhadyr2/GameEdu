using UnityEngine;

namespace EduQuest.Gameplay
{
    /// <summary>
    /// Kamera pengikut sederhana sebagai fallback bila belum memakai
    /// PlayerFollowCamera (Cinemachine) dari Starter Assets.
    /// Mengikuti target dengan offset tetap dan sedikit smoothing.
    /// </summary>
    public class SimpleFollowCamera : MonoBehaviour
    {
        [Tooltip("Target yang diikuti (biasanya pemain)")]
        public Transform target;

        [Tooltip("Posisi kamera relatif terhadap target")]
        public Vector3 offset = new Vector3(0f, 3f, -6f);

        [Tooltip("Seberapa cepat kamera mengikuti")]
        public float smoothSpeed = 8f;

        [Tooltip("Titik yang dilihat kamera di atas target")]
        public float lookHeight = 1.5f;

        private void LateUpdate()
        {
            if (target == null) return;

            Vector3 desired = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
            transform.LookAt(target.position + Vector3.up * lookHeight);
        }
    }
}
