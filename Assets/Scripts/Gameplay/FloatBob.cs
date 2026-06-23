using UnityEngine;

namespace EduQuest.Gameplay
{
    /// <summary>
    /// Membuat objek mengambang naik-turun perlahan (efek melayang).
    /// </summary>
    public class FloatBob : MonoBehaviour
    {
        [Tooltip("Jarak naik-turun")]
        public float amplitude = 0.25f;

        [Tooltip("Kecepatan ayunan")]
        public float frequency = 1f;

        private Vector3 startLocalPos;

        private void Start()
        {
            startLocalPos = transform.localPosition;
        }

        private void Update()
        {
            float y = Mathf.Sin(Time.time * frequency) * amplitude;
            transform.localPosition = startLocalPos + Vector3.up * y;
        }
    }
}
