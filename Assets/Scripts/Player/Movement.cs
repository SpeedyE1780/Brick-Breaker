using System.Collections;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    [SerializeField] private float scaleSpeed = 2;
    protected Transform myTransform;
    protected int movementType;
    private Vector3 initialScale;
    private float scaleDuration;

    private void Awake()
    {
        //Get movement type and disable movement
        movementType = PlayerPrefs.GetInt(PlayerPrefsKeys.MovementType, 0);
        enabled = false;

        if (movementType != MovementType()) //Destroy if movement type is different
            DestroyImmediate(this);
    }
    protected virtual void Start()
    {
        myTransform = transform;
        initialScale = myTransform.localScale;
        scaleDuration = 0;
    }

    private void OnEnable()
    {
        EventManager.EDoubleSize += ActivateDoubleSize;
        EventManager.EGameEnded += DisableMovement;
    }

    private void OnDisable()
    {
        EventManager.EDoubleSize -= ActivateDoubleSize;
        EventManager.EGameEnded -= DisableMovement;
    }

    private void ActivateDoubleSize(float duration)
    {
        if (scaleDuration <= 0) //Start couroutine if not active
            StartCoroutine(DoubleSize());

        scaleDuration += duration; //Set/extend scale duration
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
            MovePlayer();
    }

    void DisableMovement() => enabled = false;

    protected abstract void MovePlayer();
    protected abstract int MovementType();

    IEnumerator DoubleSize()
    {
        Vector3 doubleScale = initialScale;
        doubleScale.x *= 2;

        yield return StartCoroutine(ScalePlayer(doubleScale)); //Scale up player

        while (scaleDuration > 0)
        {
            scaleDuration -= Time.deltaTime;
            yield return null;
        }

        scaleDuration = 0; //Reset to 0 to prevent shorter duration for next time

        yield return StartCoroutine(ScalePlayer(initialScale)); //Scale down player
    }

    IEnumerator ScalePlayer(Vector3 target)
    {
        while (transform.localScale != target)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, target, scaleSpeed * Time.deltaTime);
            yield return null;
        }
    }
}