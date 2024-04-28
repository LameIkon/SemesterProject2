using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChangeDetector : MonoBehaviour
{
    public List<Image> _image; // Image to check
    public Sprite _referenceImage; // Used to compare with a specific sprite

    public List<Sprite> _previousImage; // Used to compare with the previous
    public Sprite _ignoreNull;

    [SerializeField] private bool _isImageChanged;
    private bool _checkOnce;

    private void Awake()
    {
        _checkOnce = false;

        // Initialize previousImage with the current image
        _previousImage = new List<Sprite>();
        foreach (Image image in _image)
        if (_image != null)
        {
            _previousImage.Add(image.sprite);
        }

    }

    void Update()
    {
        //CheckImage(); // Can be used if we continously want to check 
        CheckImage();
    }

    //void CheckImage()
    //{
    //    // Check if the sprite has changed
    //    if (_image.sprite != _previousImage)
    //    {
    //        _isImageChanged = true;

    //        // Update previousImage to the new sprite
    //        _previousImage = _image.sprite;
    //    }
    //}

    public void CheckImage() 
    {
        for (int i = 0; i < _image.Count; i++)
        {
            Image image = _image[i];
            Sprite previousImage = _previousImage[i];

            // Check if the sprite has changed
            if (image.sprite != previousImage && image.sprite != _ignoreNull)
            {
                GuidelineManager.instance._isfoodInToolBar = true;
                _checkOnce = true;

                // Update previousImage to the new sprite
                _previousImage[i] = image.sprite;
            }

            // if the image matches a certain image
            if (image.sprite == _referenceImage)
            {
                GuidelineManager.instance._isfoodInToolBar = true;
            }
        }       
    }
}
