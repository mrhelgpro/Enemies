using Animachine.Behaviours;
using Animachine.Runtime.Core;
using UnityEngine;

namespace Animachine.Runtime.States
{
    public class AttackWithWeapons : MonoBehaviour, IAttack
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform weapon;
        
        // Buffer
        private Transform _thisTransform;
        private IDetectable _detectable;
        private INavigation _navigation;

        private void Awake()
        {
            _thisTransform = transform;
            _navigation = GetComponentInParent<INavigation>();
            _detectable = GetComponentInParent<IDetectable>();
        }
        
        public void OnEnter()
        {
            if(prefab == null || weapon == null) return;
            
            var attackInstance = Instantiate(prefab, weapon.position, Quaternion.identity);

            Shell shell = attackInstance.GetComponent<Shell>();
            shell.Construct(_detectable.Target);
        }

        public void OnUpdate() { }

        public void OnExit() { }
        
        public void Hit()
        {
            
        }
    }
}