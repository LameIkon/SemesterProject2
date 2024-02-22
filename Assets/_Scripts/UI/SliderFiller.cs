using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderFiller : MonoBehaviour
{
    [SerializeField] private FloatReference _maxFillAmount;
    [SerializeField] private FloatReference _fillAmount;

    [SerializeField] private Slider _slider;


    void Start() 
    {
        UpdateBar();
    }

    public void UpdateBar() 
    {
        _slider.value = _fillAmount.Value;
        _slider.maxValue = _maxFillAmount.Value;
    }

}
