using UnityEngine;
using TMPro;

public class NameInputHandler : MonoBehaviour
{
    public TMP_InputField InputField;
    public NameTextureGenerator Generator;

    void Start()
    {
        // onEndEdit fires when the user presses Enter or clicks away
        InputField.onEndEdit.AddListener(OnNameSubmitted);
    }

    void OnDestroy()
    {
        InputField.onEndEdit.RemoveListener(OnNameSubmitted);
    }

    void OnNameSubmitted(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return;
        Generator.Capture(name);
    }
}
