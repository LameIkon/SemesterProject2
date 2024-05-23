using UnityEngine;

public class GuideLineSingleton : MonoBehaviour
{
    public static GuideLineSingleton _Instance;

    private void Awake()
    {
        // Ensure only 1 singleton of this script
        if (_Instance != null && _Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _Instance = this;
        }
    }
}
