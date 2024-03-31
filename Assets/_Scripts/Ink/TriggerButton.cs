using UnityEngine;
using UnityEngine.UI;

public class ButtonTrigger : MonoBehaviour
{
    public Button targetButton;

    // Method to trigger the onClick event of the target button
    public void TriggerButtonClick()
    {
        targetButton.onClick.Invoke();
    }
}