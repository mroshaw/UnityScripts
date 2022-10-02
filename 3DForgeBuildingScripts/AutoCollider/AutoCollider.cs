using System;
using UnityEngine;
namespace DaftAppleGames.Buildings
{
    /// <summary>
    /// Automatically add Colliders to buildings and props
    /// </summary>
    [ExecuteInEditMode]
    public class AutoCollider : MonoBehaviour
    {
        // For debugging / testing
        [Header("Debug")]
        public bool reportOnly = true;
        public bool detailedReport = true;
        public bool onlyShowCreated = false;

        [SerializeField]
        private int meshProcessed;
        [SerializeField]
        private int meshWithName;
        [SerializeField]
        private int existingCollider;
        [SerializeField]
        private int collidersCreated;

        /// <summary>
        /// Reset processing counters
        /// </summary>
        private void ResetCounter()
        {
            meshProcessed = 0;
            meshWithName = 0;
            existingCollider = 0;
            collidersCreated = 0;
        }

        /// <summary>
        /// Add missing colliders to crates
        /// </summary>
        public void AddCollidersToCrates()
        {
            // Add missing Crate colliders
            AddBoxColliders("crate");
        }

        /// <summary>
        /// Add missing colliders to barrels
        /// </summary>
        public void AddCollidersToBarrels()
        {
            // Add missing Barrel colliders
            AddCapsuleColliders("barrel");
        }

        /// <summary>
        /// Add missing Box Colliders
        /// </summary>
        /// <param name="namePart"></param>
        private void AddBoxColliders(string namePart)
        {
            AddColliders(namePart, typeof(BoxCollider));
        }

        /// <summary>
        /// Add missing Capsule colliders
        /// </summary>
        /// <param name="namePart"></param>
        private void AddCapsuleColliders(string namePart)
        {
            AddColliders(namePart, typeof(CapsuleCollider));
        }

        /// <summary>
        /// Add given Collider type to objects with namePart in name
        /// </summary>
        /// <param name="namePart"></param>
        /// <param name="colliderType"></param>
        private void AddColliders(string namePart, Type colliderType)
        {
            // Reset counts
            ResetCounter();

            // Debug header
            Debug.Log($"Finding missing Colliders...");
            Debug.Log($"Namepart to find: {namePart}");
            Debug.Log($"Collider type to create: {colliderType.ToString()}");

            MeshFilter[] objects = FindObjectsOfType<MeshFilter>();
            string namePartLower = namePart.ToLower();

            foreach (MeshFilter currentMesh in objects)
            {
                // Inc processed count
                meshProcessed++;

                GameObject currentGameObject = currentMesh.gameObject;
                if (currentGameObject.name.ToLower().Contains(namePartLower))
                {
                    // Inc name count
                    meshWithName++;

                    if (detailedReport && !onlyShowCreated)
                    {
                        Debug.Log($"Processing: {currentGameObject.name}");
                    }

                    // Check to see if Collider is already present
                    if (currentGameObject.GetComponent<Collider>())
                    {
                        // Inc existing count
                        existingCollider++;
                        if (detailedReport && !onlyShowCreated)
                        {
                            Debug.Log($"Already has a Collider: {currentGameObject.name}");
                        }
                    }
                    else
                    {
                        if (!reportOnly)
                        {
                            currentGameObject.AddComponent(colliderType);
                        }
                        // Inc added count
                        collidersCreated++;
                        if (detailedReport || onlyShowCreated)
                        {
                            Debug.Log($"Added new {colliderType.ToString()} to: {currentGameObject.name}");
                        }
                    }
                }
                else
                {
                    if (detailedReport && !onlyShowCreated)
                    {
                        Debug.Log($"Ignoring as no name match: {currentGameObject.name}");
                    }
                }
            }

            // Output counters
            Debug.Log($"Total mesh processed: {meshProcessed}");
            Debug.Log($"Matching names: {meshWithName}");
            Debug.Log($"Existing colliders: {existingCollider}");
            Debug.Log($"New colliders: {collidersCreated}");
        }
    }
}