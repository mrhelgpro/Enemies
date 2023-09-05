using UnityEngine;

namespace Animachine.Runtime.Core
{
    public class Healthable : MonoBehaviour, IDamageable
    {
        [SerializeField] private float health;
        
        private IDetectable _detectable;

        // Unity Methods
        private void Awake()
        {
            _detectable = GetComponentInParent<IDetectable>();
        }
        
        // IDamageable
        public bool IsDamage { get; private set; }

        public void TakeDamage(Transform dealer, float value, float shockTime = 0.25f)
        {
            health -= value;
            Debug.Log(dealer.name);
            _detectable.Target = dealer;
            IsDamage = true;
            
            Invoke(nameof(ResumeShock), shockTime);
        }

        private void ResumeShock()
        {
            CancelInvoke(nameof(ResumeShock));
            IsDamage = false;
        }
    }
}