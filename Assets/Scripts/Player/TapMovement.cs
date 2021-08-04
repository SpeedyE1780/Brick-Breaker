using UnityEngine;

public class TapMovement : Movement
{
    [SerializeField] private float sensitivity = 8;
    [SerializeField] private float maxHorizontal = 8;
    private float halfWidth;

    protected override void Start()
    {
        base.Start();
        halfWidth = Screen.width * 0.5f;
        sensitivity = PlayerPrefs.GetFloat(PlayerPrefsKeys.TapSensitivity, sensitivity);
    }

    protected override void MovePlayer()
    {
        //Move left/right if player taps left/right
        if (Input.mousePosition.x < halfWidth)
            myTransform.Translate(Vector3.left * sensitivity * Time.unscaledDeltaTime);
        else
            myTransform.Translate(Vector3.right * sensitivity * Time.unscaledDeltaTime);

        LimitHorizontal();
    }

    void LimitHorizontal() //Prevents player from going out of bounds
    {
        float xPosition = myTransform.position.x;
        if (Mathf.Abs(xPosition) > maxHorizontal)
        {
            Vector3 position = myTransform.position;
            position.x = maxHorizontal * Mathf.Sign(xPosition);
            myTransform.position = position;
        }
    }

    protected override int MovementType() => 1;
}