using UnityEngine;

// Dynamically set the size of the attached collider to cover all the children objects.

[RequireComponent(typeof(BoxCollider))]
public class DynamicColliderSize : MonoBehaviour
{
    [SerializeField] Transform boardVisual;

    void OnEnable()
    {
        ApplySize();
    }

    void ApplySize()
    {
        var boxCol = gameObject.GetComponent<BoxCollider>();
        if (boxCol == null)
        {
            boxCol = gameObject.AddComponent<BoxCollider>();
        }

        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        bool boundsInitialized = false;

        var allDescendants = boardVisual.GetComponentsInChildren<Transform>();
        foreach (Transform desc in allDescendants)
        {
            Renderer childRenderer = desc.GetComponent<Renderer>();
            if (childRenderer != null && childRenderer.enabled)
            {
                if (!boundsInitialized)
                {
                    bounds = new Bounds(childRenderer.bounds.center, childRenderer.bounds.size);
                    boundsInitialized = true;
                }
                else
                {
                    bounds.Encapsulate(childRenderer.bounds);
                }
            }
        }

        if (boundsInitialized)
        {
            boxCol.center = bounds.center - transform.position;
            boxCol.size = bounds.size;
        }
    }
}
