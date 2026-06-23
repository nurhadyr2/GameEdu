using UnityEngine;

namespace EduQuest.Gameplay
{
    /// <summary>
    /// Membuat objek (mis. label teks 3D) selalu menghadap kamera.
    /// </summary>
    public class Billboard : MonoBehaviour
    {
        private Camera cam;

        private void LateUpdate()
        {
            if (cam == null) cam = Camera.main;
            if (cam == null) return;

            // Arahkan +Z objek menjauh dari kamera agar teks terbaca normal
            transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        }
    }
}
