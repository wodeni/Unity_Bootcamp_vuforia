using UnityEngine;
using System.Collections;
using Vuforia;
using System;

public class cube_lookat_script : MonoBehaviour, IVirtualButtonEventHandler {

    public GameObject wand;
    public GameObject translate_button, rotation_button, scale_button;
    public bool translate, rotate, scale;
    public Vector3 tempPos;
    public Vector3 tempAngle;

	// Use this for initialization
	void Start () {
        //wand = GameObject.Find("wand");
        //tempRotation = wand.transform.rotation;
        translate_button = GameObject.Find("Translate");
        rotation_button = GameObject.Find("Rotate");
        scale_button = GameObject.Find("Scale");
        translate_button.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
        rotation_button.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
        translate_button.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
    }

    // Update is called once per frame
    void Update () {
        if (translate)
        {
            transform.position += wand.transform.position - tempPos;
            tempPos = wand.transform.position;
        }
        else if (rotate)
        {
            Vector3 angle_difference = wand.transform.eulerAngles - tempAngle;
            transform.localRotation = Quaternion.FromToRotation(transform.eulerAngles, transform.eulerAngles + angle_difference);
        }
        else if (scale)
        {

        }
	}

    public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
    {
        tempPos = wand.transform.position;
        tempAngle = wand.transform.eulerAngles;

        switch (vb.VirtualButtonName) {
            case "Translate":
                translate = true;
                rotate = false;
                scale = false;
                break;
            case "Rotate":
                rotate = true;
                translate = false;
                scale = false;
                break;
            case "Scale":
                scale = true;
                translate = false;
                rotate = false;
                break;
        }
    }

    public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
    {
    }
}
