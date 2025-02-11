using UnityEngine;

public class scr_cook : MonoBehaviour
{
    public int leftValue = 0;
    public int rightValue = 0; // The adjustable integer value
    public GameObject knob_left;
    public GameObject knob_right;
    public GameObject fire_left; // The GameObject to enable/disable based on currentValue
    public GameObject fire_right;

    public bool sel_side = false;

    void Update()
    {
        GameObject targetObject = null;
        GameObject objectToToggle = null;

        switch (sel_side)
        {
            case false:
                objectToToggle = fire_left;
                targetObject = knob_left;
                // Adjust the value with arrow keys
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    leftValue = Mathf.Min(leftValue + 1, 100); // Increase value, max 100
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    leftValue = Mathf.Max(leftValue - 1, 0); // Decrease value, min 0
                }

                // Rotate the GameObject based on the value
                float rotationY = (leftValue / 100f) * 360f; // Map value to 0-360 degrees
                targetObject.transform.localRotation = Quaternion.Euler(0, rotationY, 0);

                // Enable or disable the object based on currentValue
                if (leftValue > 10)
                {
                    objectToToggle.SetActive(true); // Enable the GameObject
                }
                else
                {
                    objectToToggle.SetActive(false); // Disable the GameObject
                }

                break;
            case true:
                objectToToggle = fire_right;
                targetObject = knob_right;

                // Adjust the value with arrow keys
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    rightValue = Mathf.Min(rightValue + 1, 100); // Increase value, max 100
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    rightValue = Mathf.Max(rightValue - 1, 0); // Decrease value, min 0
                }

                // Rotate the GameObject based on the value
                float rotationY_ = (rightValue / 100f) * 360f; // Map value to 0-360 degrees
                targetObject.transform.localRotation = Quaternion.Euler(0, rotationY_, 0);

                // Enable or disable the object based on currentValue
                if (rightValue > 10)
                {
                    objectToToggle.SetActive(true); // Enable the GameObject
                }
                else
                {
                    objectToToggle.SetActive(false); // Disable the GameObject
                }
                break;
                
        }

        
    }
}