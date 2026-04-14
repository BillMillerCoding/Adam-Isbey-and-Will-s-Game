using UnityEngine;

/// <summary>
/// Lives on the boss (same object as the Animator).
/// The animation event calls SpawnLazer() which instantiates the prefab at the head transform.
/// </summary>
public class LazerSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject lazerPrefab;

    [Header("Spawn Point")]
    [Tooltip("Drag an empty child GameObject positioned at the boss's head.")]
    [SerializeField] private Transform headTransform;

    /// <summary>
    /// Called by an Animation Event on the lazer attack clip.
    /// </summary>
    public void SpawnLazer()
    {
        if (lazerPrefab == null)
        {
            Debug.LogWarning("[LazerSpawner] No lazer prefab assigned.");
            return;
        }

        if (headTransform == null)
        {
            Debug.LogWarning("[LazerSpawner] No head transform assigned.");
            return;
        }

        Instantiate(lazerPrefab, headTransform.position, Quaternion.identity)
            .GetComponent<LazerController>()?.Init(headTransform);
    }
}
