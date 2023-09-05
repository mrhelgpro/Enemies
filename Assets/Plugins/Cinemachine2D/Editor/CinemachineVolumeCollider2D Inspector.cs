using Cinemachine;
using Cinemachine2D.Runtime;
using UnityEditor;

namespace Cinemachine2D.Editor
{
    [CustomEditor(typeof(CinemachineVolumeCollider2D))]
    [CanEditMultipleObjects]
    public class CinemachineVolumeCollider2DInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var thisTarget = (CinemachineVolumeCollider2D)target;
            
            thisTarget.gameObject.layer = UnityEngine.LayerMask.NameToLayer("Ignore Raycast");
            
            thisTarget.mode = (CinemachineVolumeCollider2D.Mode)EditorGUILayout.EnumPopup("Mode", thisTarget.mode);
            
            switch (thisTarget.mode)
            {
                case CinemachineVolumeCollider2D.Mode.FieldOfView:
                    thisTarget.fieldOfView = EditorGUILayout.Slider("Field Of View", thisTarget.fieldOfView, 10, 80);
                    thisTarget.rate = EditorGUILayout.Slider("Rate", thisTarget.rate, 0.1f, 1);
                    break;
                case CinemachineVolumeCollider2D.Mode.SwitchCamera:
                    thisTarget.cinemachineVirtualCamera = EditorGUILayout.ObjectField("Virtual Camera", thisTarget.cinemachineVirtualCamera, typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
                    thisTarget.time = EditorGUILayout.Slider("Time", thisTarget.time, 0.0f, 3);
                    break;
            }
        }
    }
}
