using UnityEngine;

namespace EduQuest.Gameplay
{
    /// <summary>
    /// Memutar objek secara perlahan. Dipakai untuk model organ 3D agar menarik perhatian.
    /// </summary>
    public class Rotator : MonoBehaviour
    {
        [Tooltip("Kecepatan rotasi (derajat per detik) untuk tiap sumbu")]
        public Vector3 rotationSpeed = new Vector3(0f, 30f, 0f);

        private void Update()
        {
            transform.Rotate(rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}
