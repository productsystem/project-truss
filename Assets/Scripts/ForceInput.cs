using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ForceInput : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button applyButton;

    private JointPlacer jointPlacer;
    private GameObject selectedJoint;

    void Start()
    {
        jointPlacer = FindObjectOfType<JointPlacer>();
        gameObject.SetActive(false);
        applyButton.onClick.AddListener(OnApplyClicked);
    }

    public void Show(Vector2 screenPos, GameObject joint)
    {
        selectedJoint = joint;
        transform.position = screenPos;
        inputField.text = "";
        gameObject.SetActive(true);
    }

    void OnApplyClicked()
    {
        if (selectedJoint == null) return;

        string text = inputField.text.Trim();
        if (TryParseVector(text, out Vector2 force))
        {
            jointPlacer.DrawForceArrow(selectedJoint, force.normalized);
            jointPlacer.canPlace = true;
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Invalid input! Use format: (x,y)");
        }
    }

    bool TryParseVector(string input, out Vector2 result)
    {
        result = Vector2.zero;
        input = input.Replace("(", "").Replace(")", "").Trim();
        string[] parts = input.Split(',');
        if (parts.Length != 2) return false;

        bool parsedX = float.TryParse(parts[0], out float x);
        bool parsedY = float.TryParse(parts[1], out float y);

        if (parsedX && parsedY)
        {
            result = new Vector2(x, y);
            return true;
        }
        return false;
    }
}
