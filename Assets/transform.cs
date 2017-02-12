using UnityEngine;
using Vuforia;
using System.Collections;

public class transform : MonoBehaviour, IVirtualButtonEventHandler {

    private GameObject rotate_button, translate_button, scale_button;
    Vector3 curPos, curScale;
    bool translate, rotate, scale;
    bool right_flag, expand_flag;

	// Use this for initialization
	void Start () {
        translate = rotate = scale = false;
        rotate_button = GameObject.Find("rotate_button");
        rotate_button.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
        translate_button = GameObject.Find("translate_button");
        translate_button.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this); 
        scale_button = GameObject.Find("scale_button");
        scale_button.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);

    }

    // Update is called once per frame
    void Update () {

        if (translate)
        {
            curPos = transform.position;
            if (curPos.x > 5)
                right_flag = false;
            if (curPos.x < -5)
                right_flag = true;

            if (right_flag)
                curPos.x += 0.01f;
            else
                curPos.x -= 0.01f;
            transform.position = curPos;
        }
        else if (rotate)
        {
            transform.Rotate(0, 20 * Time.deltaTime, 0);
        }
        else if (scale) {

            curScale = transform.localScale;
            if (curScale.x <= 0.1) 
                expand_flag = true;
            
            if (curScale.x >= 0.5)
                expand_flag = false;
            

            if (expand_flag)
            {
                curScale.x += Time.deltaTime / 10;
                curScale.y += Time.deltaTime / 10;
                curScale.z += Time.deltaTime / 10;
            }
            else
            {
                curScale.x -= Time.deltaTime / 10;
                curScale.y -= Time.deltaTime / 10;
                curScale.z -= Time.deltaTime / 10;
            }

            transform.localScale = curScale;
        }

    }

    public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
    {

        Debug.Log("Button pressed!" + vb.VirtualButtonName);


        switch (vb.VirtualButtonName)
        {

            case "translate_button":
                Debug.Log("translate: Button pressed!");

                rotate = scale = false;
                translate = true;
                break;
            case "rotate_button":
                Debug.Log("rotate: Button pressed!");

                translate = scale = false;
                rotate = true;
                break;
            case "scale_button":
                Debug.Log("scale: Button pressed!");

                scale = true;
                translate = rotate = false;
                break;
        }
    }

    public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
    {
        Debug.Log("Button released!");
    }
}
