using UnityEngine;

public class SpawnBricks : MonoBehaviour
{
    [SerializeField] private int maxColumns = 11;
    [SerializeField] private int bricksAmount = 22;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private Vector3 startPosition;

    /// <summary>
    /// Place bricks in a grid layout
    /// </summary>
    public void PlaceBricks(bool usePool = true)
    {
        DeleteChildren(usePool);
        int row;
        int column;
        for (int brickIndex = 0; brickIndex < bricksAmount; brickIndex++)
        {
            GameObject brick = usePool ? PoolManager.Instance.GetPooledObject(PoolID.Brick) : Instantiate(brickPrefab);
            brick.transform.SetParent(transform);
            brick.name = $"Brick_{brickIndex}";
            brick.transform.SetParent(transform);

            row = Mathf.FloorToInt(brickIndex / maxColumns);
            column = brickIndex % maxColumns;
            brick.transform.position = startPosition + Vector3.right * column + Vector3.down * row;
        }
    }

    public void AddBricks() => bricksAmount = Mathf.Clamp(bricksAmount + maxColumns, 0, maxColumns * 7);

    /// <summary>
    /// Delete all spawned bricks
    /// </summary>
    void DeleteChildren(bool usePool)
    {
        Transform myTransform = transform;
        while (myTransform.childCount > 0)
            if (usePool)
                PoolManager.Instance.AddToPool(PoolID.Brick, myTransform.GetChild(0).gameObject);
            else
                DestroyImmediate(myTransform.GetChild(0).gameObject);

    }
}