using UnityEngine;
using EduQuest.Core;

namespace EduQuest.Gameplay
{
    [RequireComponent(typeof(Collider))]
    public class Bomb : MonoBehaviour
    {
        [Header("Damage")]
        public int lifeLoss = 1;
        public string playerTag = "Player";

        [Header("Efek Ledakan")]
        public GameObject explosionEffect;
        public AudioClip explosionSound;
        [Range(0f, 1f)] public float explosionVolume = 1f;

        [Header("Knockback")]
        public float knockbackForce = 8f;
        public float knockbackUpward = 3f;
        public float knockbackDuration = 0.3f;

        private bool exploded;

        private void Reset()
        {
            var col = GetComponent<Collider>();
            if (col) col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (exploded || !other.CompareTag(playerTag)) return;
            exploded = true;
            Explode(other.gameObject);
        }

        private void Explode(GameObject player)
        {
            if (explosionEffect != null)
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            if (explosionSound != null)
                AudioSource.PlayClipAtPoint(explosionSound, transform.position, explosionVolume);

            var gm = GameManager.Instance;
            gm?.LoseLife(lifeLoss);

            ApplyKnockback(player);

            if (gm != null && gm.IsGameOver)
                LevelManager.Instance?.ForceGameOver();

            Destroy(gameObject);
        }

        private void ApplyKnockback(GameObject player)
        {
            Vector3 dir = player.transform.position - transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.0001f) dir = player.transform.forward * -1f;
            dir.Normalize();

            Vector3 velocity = dir * knockbackForce + Vector3.up * knockbackUpward;

            var rb = player.GetComponent<Rigidbody>();
            if (rb != null && !rb.isKinematic)
            {
                rb.AddForce(velocity, ForceMode.VelocityChange);
                return;
            }

            var cc = player.GetComponent<CharacterController>();
            if (cc != null)
            {
                var kb = player.GetComponent<BombKnockback>();
                if (kb == null) kb = player.AddComponent<BombKnockback>();
                kb.Begin(velocity, knockbackDuration);
            }
        }
    }
}
