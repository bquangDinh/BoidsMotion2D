using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Slider cohensionSlider;
    public Slider seperationSlider;
    public Slider alignmentSlider;

    public Toggle viewVelocityToggle;
    public Toggle viewVelocityComponentsToggle;
    public Toggle enableSingleBoidToggle;

    // Start is called before the first frame update
    void Start()
    {
        cohensionSlider.value = Constants.cohensionFactor;
        seperationSlider.value = Constants.seperationFactor;
        alignmentSlider.value = Constants.alignmentFactor;

        viewVelocityToggle.isOn = Constants.viewVelocityVector;
        viewVelocityComponentsToggle.isOn = Constants.viewVelocityComponentsVector;
        enableSingleBoidToggle.isOn = Constants.enableSingleBoidViewing;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CohensionUpdateCallback(float value)
    {
        Constants.cohensionFactor = value;
    }

    public void SeperationUpdateCallback(float value)
    {
        Constants.seperationFactor = value;
    }

    public void AlignmentUpdateCallback(float value)
    {
        Constants.alignmentFactor = value;
    }

    public void ViewVelocityToggleCallback(bool value)
    {
        Constants.viewVelocityVector = value;
    }

    public void ViewVelocityComponentsCallback(bool value)
    {
        Constants.viewVelocityComponentsVector = value;
    }

    public void EnableSingleBoidViewing(bool value)
    {
        Constants.enableSingleBoidViewing = value;

        if(value == true)
        {
            Constants.singleBoidID = Random.Range(0, Constants.numberOfBoids - 1);
        }
        else
        {
            Constants.singleBoidID = -1;
        }
    }
}
