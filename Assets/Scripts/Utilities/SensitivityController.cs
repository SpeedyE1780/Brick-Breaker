using UnityEngine;
using UnityEngine.UI;

public class SensitivityController : MonoBehaviour
{
    [SerializeField] private Slider sensitivity;

    //Set value based on player prefs and saves it when value changes
    private void Awake() => sensitivity.value = PlayerPrefs.GetFloat(PlayerPrefsKeys.TapSensitivity, 8);
    public void ChangeSensitivity(float value) => PlayerPrefs.SetFloat(PlayerPrefsKeys.TapSensitivity, value);
}
