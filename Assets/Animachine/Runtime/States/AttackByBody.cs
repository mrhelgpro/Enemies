using Animachine.Behaviours;
using UnityEngine;

namespace Animachine.Runtime.States
{
    public class AttackByBody : MonoBehaviour, IAttack
    {
        [SerializeField] private GameObject prefab;
        
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
            Debug.Log("ATTACK BY BODY");
        }

        public void OnUpdate()
        {

        }

        public void OnExit()
        {

        }
    }
}