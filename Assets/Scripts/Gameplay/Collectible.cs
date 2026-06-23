using UnityEngine;
using EduQuest.Core;

namespace EduQuest.Gameplay
{
    /// <summary>
    /// Item yang bisa dikumpulkan pemain sambil menjelajah ("poin pengetahuan").
    /// Menambah skor lalu menghilang. Colliders harus Is Trigger.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Collectible : MonoBehaviour
    {
        [Tooltip("Skor yang didapat saat dikumpulkan")]
        public int scoreValue = 5;

        [Tooltip("Tag pemain")]
        public string playerTag = "Player";

        [Tooltip("Efek partikel opsional saat dikumpulkan")]
        public GameObject collectEffect;

        private bool collected;

        private void Reset()
        {
            var col = GetComponent<Collider>();
            if (col) col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (collected || !other.CompareTag(playerTag)) return;
            collected = true;

            GameManager.Instance?.AddScore(scoreValue);

            if (collectEffect != null)
                Instantiate(collectEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
