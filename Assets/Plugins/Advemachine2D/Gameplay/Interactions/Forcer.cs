using Advemachine2D.Gameplay.Core;
using Advemachine2D.Gameplay.Runtime;
using UnityEngine;

namespace Advemachine2D.Interactions
{
    public class Forcer : MonoBehaviour
    {
        public enum Mode { Tosser, Bouncer }

        public Mode mode = Mode.Tosser;
        [Range(1, 20)] public float force = 10;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Movable movable = collision.GetComponent<Movable>();

            // Check Movable
            if (movable == null)
            {
                return;
            }

            // Check Bouncer
            if (mode == Mode.Bouncer)
            {
                if (movable.IsGrounded)
                {
                    return;
                }
            }

            // Add Force to movable
            movable.Force(force * Vector2.up);
        }
    }
}