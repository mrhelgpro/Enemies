using Animachine.Scripts.Core;
using Animachine.Scripts.Interfaces;
using Animachine.Scripts.Utilities;
using UnityEngine;

namespace Animachine.Scripts.States
{
    public class AttackWithWeapons : AnimachineState
    {
        [SerializeField] private GameObject prefab;
        
        // Buffer
        private Transform _weapon;
        private Transform _thisTransform;
        private IDetectable _detectable;
        private INavigation _navigation;
        
        protected override void OnEnter()
        {
            _navigation = ThisTransform.GetComponentInParent<INavigation>();
            _detectable = ThisTransform.GetComponentInParent<IDetectable>();

            _weapon = ThisTransform; // FIXED IT!!!!!!
            
            if(prefab == null || _weapon == null) return;
            
            var attackInstance = Instantiate(prefab, _weapon.position, Quaternion.identity);

            var shell = attackInstance.GetComponent<Shell>();
            shell.Construct(_detectable.Target);
        } 
    }
}