using Advemachine2D.Gameplay.Core;
using UnityEngine;

namespace Advemachine2D.Gameplay.Input
{
    [AddComponentMenu("Advemachine2D/Input/InputDirectionController")]
    [RequireComponent(typeof(Inputable))]
    public class InputDirectionController : MonoBehaviour
    {
        public enum  Mode { Button, Touch, Both }
        public Mode mode;
        
        private Inputable _inputable;

        private void Awake()
        {
            _inputable = GetComponent<Inputable>();
        }

        private void Update()
        {
            // Button
            var horizontalButtonAxis = UnityEngine.Input.GetAxis("Horizontal");
            var verticalButtonAxis = UnityEngine.Input.GetAxis("Vertical");
            
            var inputDirectionButton = new Vector2(horizontalButtonAxis, verticalButtonAxis);
            
            // Mouse
            var horizontalMousePosition = Mathf.Sign(UnityEngine.Input.mousePosition.x - Screen.width / 2);
            var verticalMousePosition = Mathf.Sign(UnityEngine.Input.mousePosition.y - Screen.height / 2);
            
            var inputHorizontalMouse = UnityEngine.Input.GetMouseButton(0) ? horizontalMousePosition : 0;
            var inputVerticalMouse = UnityEngine.Input.GetMouseButton(0) ? verticalMousePosition : 0;

            var inputDirectionMouse = new Vector2(inputHorizontalMouse, inputVerticalMouse);

            // Result
            Vector2 inputHorizontal = Vector2.zero;
            
            if (mode == Mode.Button)
            {
                inputHorizontal = inputDirectionButton;
            }
            else if (mode == Mode.Touch)
            {
                inputHorizontal = inputDirectionMouse;
            }
            else if (mode == Mode.Both)
            {
                inputHorizontal = inputDirectionButton.magnitude > 0 ? inputDirectionButton : inputDirectionMouse;
            }

            _inputable.moveDirection = inputHorizontal;
        }
    }
}