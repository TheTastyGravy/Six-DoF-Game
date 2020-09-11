using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[DisallowMultipleComponent, RequireComponent(typeof(Slider))]
public class PlayerPrefsSlider : MonoBehaviour
{
    // This script requiers the slider object have:
    //      -A Text object named 'Text'
    //      -A InputField object named 'InputField'
    //
    // This script also adds its own listeners to the appropriate events

    Slider slider;
    InputField field;

    public string playerPrefsKey;
    // Used to set the label attached to the slider
    [SerializeField] string labelText = "Label";

    
    // Called when a value in the inspector in changed
    void OnValidate()
    {
        // Update label in editor
        transform.Find("Text").GetComponent<Text>().text = labelText;
    }

    void Start()
    {
        // Get components
        slider = GetComponent<Slider>();
        field = transform.Find("InputField").GetComponent<InputField>();

        // Add listeners to slider and input field events
        slider.onValueChanged.AddListener(new UnityAction<float>(SliderChange));
        field.onEndEdit.AddListener(new UnityAction<string>(FieldChange));
        
        // Set slider to current player pref
        slider.value = PlayerPrefs.GetFloat(playerPrefsKey, 1f);
    }


    public void SliderChange(float newValue)
    {
        // Set input field to display current value, without invoking the event
        field.SetTextWithoutNotify(newValue.ToString("F2"));
        // Update player prefs, rounding to 2 decimals
        PlayerPrefs.SetFloat(playerPrefsKey, Mathf.Round(newValue * 100f) / 100f);
    }
    public void FieldChange(string newValue)
    {
        // Update the slider, calling SliderChange()
        slider.value = float.Parse(newValue);
    }
}