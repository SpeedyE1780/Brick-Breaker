using UnityEngine;
using UnityEngine.UI;

public class MovementButtonController : MonoBehaviour
{
    [SerializeField] private int movementType;
    [SerializeField] private Button button;

    //Interactable if not selected & change movement type and saves it
    private void Awake() => button.interactable = PlayerPrefs.GetInt(PlayerPrefsKeys.MovementType, 0) != movementType;

    public void ChangeMovementType()
    {
        PlayerPrefs.SetInt(PlayerPrefsKeys.MovementType, movementType);
        button.interactable = false;
    }
}
