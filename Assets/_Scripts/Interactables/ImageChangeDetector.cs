using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChangeDetector : MonoBehaviour
{
    public Image _image; // Image to check
    //public Sprite _referenceImage; // Used to compare with a specific sprite

    public Sprite _previousImage; // Used to compare with the previous
    public Sprite _ignoreNull;

    [SerializeField] private bool _isImageChanged;
    private bool _checkOnce;

    private void Awake()
    {
        _checkOnce = false;

        // Initialize previousImage with the current image
        if (_image != null)
        {
            _previousImage = _image.sprite;
        }

    }

    void Update()
    {
        //CheckImage(); // Can be used if we continously want to check 
        CheckImageTutorial();
    }

    void CheckImage()
    {
        // Check if the sprite has changed
        if (_image.sprite != _previousImage)
        {
            _isImageChanged = true;

            // Update previousImage to the new sprite
            _previousImage = _image.sprite;
        }
    }

    public void CheckImageTutorial() // Only used for the Guideline script
    {
        if ( _image.sprite != _ignoreNull) // Ignore if the image sprite is null
        {
            if (_image.sprite != _previousImage && !_checkOnce)
            {
                GuidelineManager.instance._isfoodInToolBar = true;
                _checkOnce = true;

                // Update previousImage to the new sprite
                _previousImage = _image.sprite;
            }
        }
    }

}
