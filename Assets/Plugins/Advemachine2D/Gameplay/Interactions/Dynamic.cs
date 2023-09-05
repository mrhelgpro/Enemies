using System;
using UnityEngine;

namespace Advemachine2D.Gameplay.Runtime.Interactions
{
    [Tooltip("This class as a marker for moving surface. Inherit all classes that are movable objects.")]
    public class Dynamic : MonoBehaviour
    {
        private void Awake()
        {
            // Check Collider
            Collider2D colliderComponent = GetComponent<Collider2D>();
            
            if (colliderComponent == null)
            {
                Debug.LogWarning(gameObject.name + " - Collider is not found");
                return;
            }
            
            // Check Resources
            PhysicsMaterial2D frictionMaterial = Resources.Load<PhysicsMaterial2D>("Physic2D/Friction 2D");
            
            if (frictionMaterial == null)
            {
                Debug.LogWarning(gameObject.name + " - Resource (Path:Physic2D/Friction2D) is not found");
                return;
            }
            
            colliderComponent.sharedMaterial = frictionMaterial;
        }
    }
}