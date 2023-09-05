using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Cinemachine2D.Runtime
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    [RequireComponent(typeof(CinemachineConfiner2D))]
    [AddComponentMenu("Cinemachine2D/Cinemachine Gameplay Controller 2D")]
    public class CinemachineGameplayController2D : MonoBehaviour
    {
        //Get Values
        public Transform Followable => _thisVirtualCamera.Follow;
        
        // Buffer
        private float _fieldOfView;
        private Coroutine _coroutineFieldOfView;
        private CinemachineBrain _cinemachineBrain;
        private CinemachineVirtualCamera _thisVirtualCamera;
        private CinemachineFramingTransposer _framingTransposerComponent;
        private CinemachineConfiner2D _cinemachineConfiner2D;
        
        private void Awake()
        {
            _cinemachineBrain = FindObjectOfType<CinemachineBrain>();
            _thisVirtualCamera = GetComponent<CinemachineVirtualCamera>();
            _framingTransposerComponent = _thisVirtualCamera.AddCinemachineComponent<CinemachineFramingTransposer>();
            _cinemachineConfiner2D = GetComponent<CinemachineConfiner2D>();
            
            if (_cinemachineConfiner2D.m_BoundingShape2D) // REDESIGN IT, REMOVE TO METHOD!!!!!!!!!!!!!!!!!!
            {
                _cinemachineConfiner2D.m_BoundingShape2D.isTrigger = true;
                _cinemachineConfiner2D.m_BoundingShape2D.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            }

            // Set Default Parameters
            if (_thisVirtualCamera.Follow == null)
            {
                _thisVirtualCamera.Follow = GameObject.FindGameObjectWithTag("Player")?.transform;
            }

            _thisVirtualCamera.m_Lens.FieldOfView = _fieldOfView = 40;
            _framingTransposerComponent.m_TrackedObjectOffset = Vector3.zero;
            _framingTransposerComponent.m_LookaheadTime = 0;
            _framingTransposerComponent.m_LookaheadSmoothing = 0;
            _framingTransposerComponent.m_XDamping = 1.25f;
            _framingTransposerComponent.m_YDamping = 2.0f;
            _framingTransposerComponent.m_ZDamping = 0;
            _framingTransposerComponent.m_TargetMovementOnly = true;
            _framingTransposerComponent.m_ScreenX = 0.5f;
            _framingTransposerComponent.m_ScreenY = 0.65f;
            _framingTransposerComponent.m_CameraDistance = 25.0f;
            _framingTransposerComponent.m_DeadZoneWidth = 0.0f;
            _framingTransposerComponent.m_DeadZoneHeight = 0.0f;
            _framingTransposerComponent.m_DeadZoneDepth = 0;
            _framingTransposerComponent.m_UnlimitedSoftZone = false;
            _framingTransposerComponent.m_SoftZoneWidth = 0.2f;
            _framingTransposerComponent.m_SoftZoneHeight = 0.35f;
            _framingTransposerComponent.m_BiasX = 0.0f;
            _framingTransposerComponent.m_BiasY = -0.3f;
            _framingTransposerComponent.m_CenterOnActivate = true;
        }

        private void OnDisable() => CheckCoroutine();

        public void SetAsDefaultFollowable(Transform target) => _thisVirtualCamera.Follow = target;
        
        public void ResetParameters(float rate = 1)
        {
            _thisVirtualCamera.Priority = CameraPriority.Gameplay;
            ChangeFieldOfView(_fieldOfView, rate);
        }
        
        public void SetPriority(int value, float time = 1)
        {
            _cinemachineBrain.m_DefaultBlend.m_Time = time;
            _thisVirtualCamera.Priority = value;
        }
        
        public void SetFieldOfView(float value, float rate = 1) => ChangeFieldOfView(value, rate);

        // Private Methods
        private void ChangeFieldOfView(float value, float rate)
        {
            if(Mathf.Approximately(value, _thisVirtualCamera.m_Lens.FieldOfView)) return;

            CheckCoroutine();
            
            _coroutineFieldOfView = StartCoroutine(CoroutineFieldOfView(value, rate));
        }

        private IEnumerator CoroutineFieldOfView(float value, float rate)
        {
            var initFieldOfView = _thisVirtualCamera.m_Lens.FieldOfView;
            var scale = 0f;
            
            while (scale < 1f)
            {
                scale += Time.deltaTime * rate;
                var currentFieldOfView = Mathf.Lerp(initFieldOfView, value, scale);
                _thisVirtualCamera.m_Lens.FieldOfView = currentFieldOfView;
                yield return null;
            }
            
            _coroutineFieldOfView = null;
        }

        private void CheckCoroutine()
        {
            if (_coroutineFieldOfView != null)
            {
                StopCoroutine(_coroutineFieldOfView);
            }
        }
    }
}
