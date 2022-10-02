using UnityEngine;

namespace DaftAppleGames.Buildings
{
    /// <summary>
    /// A simple Editor context script to search and replace Game Objects / renderes of 3D Forge prefab instances.
    /// WARNING: Use on prefab variants ONLY! Do not run against prefabs, or you might have to re-install 3D forge assets
    /// to recover any changes.
    /// </summary>
    [ExecuteInEditMode]
    public class InteriorSwapper : MonoBehaviour
    {
        // Component config
        [Header("Config")]
        [Tooltip("Pick the top level Game Object to process. If you leave this empty, the game object containing the component will be used.")]
        public GameObject parentGameObject;
        [Tooltip("Check this to delete the old Game Objects. If unchecked, they will be set to inactive.")]
        public bool destroySourceGameObjects = false;

        [Header("Game Objects")]
        [Tooltip("Drag all Game Objects here, that you want to replace. e.g. everything from Prefabs > Walls > wall_01")]
        public GameObject[] sourcePartsList;
        [Tooltip("Drag all Game Objects here, that you want to use.. e.g. everything from Prefabs > Walls > wall_07. MAKE SURE THEY ARE IN THE SAME ORDER / POSITION AS SOURCE!")]
        public GameObject[] targetPartsList;

        /// <summary>
        /// Iterate over all sources and find renderers to replace by comparing names
        /// If found, new instances are instantiated, placed over the original and the
        /// original either destoryed or inactivated.
        /// </summary>
        public void ReplaceParts()
        {
            // Search for all mesh renderes. We'll parse these to find matches for each source
            MeshRenderer[] allRenderers = parentGameObject.GetComponentsInChildren<MeshRenderer>();

            // Take each source game object
            for (int index = 0; index < sourcePartsList.Length; index++)
            {
                foreach (MeshRenderer renderer in allRenderers)
                {
                    if (renderer.gameObject.name.Equals(sourcePartsList[index].name))
                    {
                        // Instantiate a new target part
                        GameObject newGameObject = Instantiate(targetPartsList[index]);

                        // Position the target part over the source
                        newGameObject.transform.position = renderer.gameObject.transform.position;
                        newGameObject.transform.rotation = renderer.gameObject.transform.rotation;
                        newGameObject.transform.SetParent(renderer.gameObject.transform.parent);

                        // Rename, to remove the "clone" part and allow reversion
                        newGameObject.name = targetPartsList[index].name;

                        // Delete or disable the source
                        if (destroySourceGameObjects)
                        {
                            DestroyImmediate(renderer.gameObject, true);
                        }
                        else
                        {
                            renderer.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Swaps the source and target parts
        /// </summary>
        public void SwapPartsLists()
        {
            for (int index = 0; index < sourcePartsList.Length; index++)
            {
                GameObject temp = sourcePartsList[index];
                sourcePartsList[index] = targetPartsList[index];
                targetPartsList[index] = temp;
            }
        }
    }
}