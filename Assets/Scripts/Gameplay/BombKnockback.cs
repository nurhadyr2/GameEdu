using UnityEngine;

namespace EduQuest.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public class BombKnockback : MonoBehaviour
    {
        private CharacterController _cc;
        private Vector3 _velocity;
        private float _timeLeft;

        public void Begin(Vector3 velocity, float duration)
        {
            _cc = GetComponent<CharacterController>();
            _velocity = velocity;
            _timeLeft = Mathf.Max(0.01f, duration);
            enabled = true;
        }

        private void Update()
        {
            if (_timeLeft <= 0f || _cc == null)
            {
                Destroy(this);
                return;
            }

            _cc.Move(_velocity * Time.deltaTime);
            _velocity = Vector3.Lerp(_velocity, Vector3.zero, Time.deltaTime * 5f);
            _timeLeft -= Time.deltaTime;
        }
    }
}
