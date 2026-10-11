using UnityEngine;

public class SessionNodeContainer : MonoBehaviour
{
    [SerializeField] private GameObject sessionNodePrefab;
    [SerializeField] private int nodeCount = 4;
    [SerializeField] private bool isHorizontal = true;
    [SerializeField] private float spacing = 1.5f;
    [SerializeField] private float padding = 0.5f;

    private void Start()
    {
        BuildNodes();
    }

    private void BuildNodes()
    {
        Vector3 parentWorldScale = transform.lossyScale;

        // Cancel out whatever scale this container (the wide rectangle)
        // has, so every node ends up at the PREFAB's own original scale
        // in world space, regardless of the container's size.
        Vector3 compensatedScale = new Vector3(
            sessionNodePrefab.transform.localScale.x / parentWorldScale.x,
            sessionNodePrefab.transform.localScale.y / parentWorldScale.y,
            sessionNodePrefab.transform.localScale.z / parentWorldScale.z
        );

        for (int i = 0; i < nodeCount; i++)
        {
            GameObject nodeInstance = Instantiate(sessionNodePrefab, transform);

            float offset = (i - (nodeCount - 1) / 2f) * spacing;
            float paddedOffset = offset + Mathf.Sign(offset) * padding;

            Vector3 localPosition = isHorizontal
                ? new Vector3(paddedOffset, 0f, 0f)
                : new Vector3(0f, paddedOffset, 0f);

            nodeInstance.transform.localPosition = localPosition;
            nodeInstance.transform.localScale = compensatedScale;
        }
    }
}