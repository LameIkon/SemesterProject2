using UnityEngine;
using UnityEngine.UI;

public class SceneFadeManager : MonoBehaviour
{
    public static SceneFadeManager _Instance;

    [SerializeField] private Image _fadeOutImage;
    [Range(0.1f, 10f), SerializeField] private float _fadeOutSpeed = 5;
    [Range(0.1f, 10f), SerializeField] private float _fadeInSpeed = 5;
    [SerializeField] private Color _fadeOutStartColor;
    
    public bool _IsFadingOut { get; private set; }
    public bool _IsFadingIn { get; private set; }
    
    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        } 
        _fadeOutStartColor.a = 0f;
    }

    private void Update()
    {
        if (_IsFadingOut)
        {
            if (_fadeOutImage.color.a < 1f)
            {
                _fadeOutStartColor.a += Time.deltaTime * _fadeOutSpeed;
                _fadeOutImage.color = _fadeOutStartColor;
            } 
            else
            {
                _IsFadingOut = false;
            }
        }

        if (_IsFadingIn)
        {
            if (_fadeOutImage.color.a > 0f)
            {
                _fadeOutStartColor.a -= Time.deltaTime * _fadeInSpeed;
                _fadeOutImage.color = _fadeOutStartColor;
            }
            else
            {
                _IsFadingIn = false;
            }
        }
    }

    public void StartFadeOut()
    {
        _fadeOutImage.color = _fadeOutStartColor;
        _IsFadingOut = true;
    }

    public void StartFadeIn()
    {
        if (_fadeOutImage.color.a >= 1f)
        {
            _fadeOutImage.color = _fadeOutStartColor;
            _IsFadingIn = true;
        }
    }
}
