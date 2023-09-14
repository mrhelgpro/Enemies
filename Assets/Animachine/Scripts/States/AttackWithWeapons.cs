using Animachine.Scripts.Core;
using Animachine.Scripts.Interactions;
using Animachine.Scripts.Interfaces;
using UnityEngine;

namespace Animachine.Scripts.States
{
    public class AttackWithWeapons : AnimachineState
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform weapon;
        
        // Buffer
        private Transform _thisTransform;
        private IDetectable _detectable;
        
        protected override void OnEnter()
        {
            _detectable = ThisTransform.GetComponentInParent<IDetectable>();

            weapon = weapon == null ? ThisTransform : weapon;
            
            if(prefab == null || weapon == null) return;
            
            var attackInstance = Instantiate(prefab, weapon.position, Quaternion.identity);

            var shell = attackInstance.GetComponent<Shell>();
            shell.Construct(_detectable.Target);
        } 
    }
}