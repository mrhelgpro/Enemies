using Cinemachine;
using UnityEngine;

namespace Cinemachine2D.Runtime
{
    [AddComponentMenu("Cinemachine2D/Cinemachine Volume Collider2D")]
    public class CinemachineVolumeCollider2D : MonoBehaviour
    {
        public enum Mode { FieldOfView, SwitchCamera }
        public Mode mode = Mode.FieldOfView;
        
        public float fieldOfView = 20;
        public float rate = 1;
        public float time = 1;
        public CinemachineVirtualCamera cinemachineVirtualCamera;

        private CinemachineGameplayController2D _cinemachineGameplayController;

        private void OnValidate()
        {
            // Check <Collider> Component
            var checkCollider = GetComponent<Collider2D>();
            if (checkCollider == null)
            {
                Debug.LogWarning(gameObject.name + " - <Collider2D> is not found");
                return;
            }

            checkCollider.isTrigger = true;
            
            // Check <CinemachineVirtualCamera> Component
            if (mode is Mode.SwitchCamera && cinemachineVirtualCamera == null)
            {
                Debug.LogWarning(gameObject.name + " - <CinemachineVirtualCamera> is not found");
            }
        }

        private void Awake()
        {
            _cinemachineGameplayController = FindObjectOfType<CinemachineGameplayController2D>();
        }

        private bool CheckTriggeredCollider(Collider2D triggerCollider)
        {
            if (_cinemachineGameplayController.Followable == null) return false;
            
            var followable = _cinemachineGameplayController.Followable.gameObject;
            return followable == triggerCollider.gameObject;
        }

        private void OnTriggerEnter2D(Collider2D triggerCollider)
        {
            // Check Trigger Collider
            if(CheckTriggeredCollider(triggerCollider) == false) return;

            // Call by mode
            switch (mode)
            {
                case Mode.FieldOfView:
                    OnFieldOfView();
                    break;
                case Mode.SwitchCamera:
                    OnSwitchCamera();
                    break;
            }
        }
    
        private void OnTriggerExit2D(Collider2D triggerCollider)
        {
            if(CheckTriggeredCollider(triggerCollider) == false) return;
            _cinemachineGameplayController.ResetParameters(rate);

            if (mode is Mode.SwitchCamera)
            {
                cinemachineVirtualCamera.Priority = CameraPriority.None;
            }
        }

        private void OnFieldOfView()
        {
            _cinemachineGameplayController.SetFieldOfView(fieldOfView, rate);
        }
        
        private void OnSwitchCamera()
        {
            cinemachineVirtualCamera.Priority = CameraPriority.Gameplay;
            _cinemachineGameplayController.SetPriority(CameraPriority.None, time);
        }
    }
}