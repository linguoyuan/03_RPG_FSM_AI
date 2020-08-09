using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject obj;
    float angle = 0;
    float deta = 0f;
    void Start()
    {
        Debug.Log(obj.transform.eulerAngles);
    }

    // Update is called once per frame
    void Update()
    {
        deta = Input.GetAxis("Mouse Y");
        Vector3 eulerAngles = transform.localEulerAngles;
        //transform.localEulerAngles = new Vector3(eulerAngles.x - Input.GetAxis("Mouse Y"), eulerAngles.y, eulerAngles.z);
        //transform.localEulerAngles = new Vector3(deta - Input.GetAxis("Mouse Y"), eulerAngles.y, eulerAngles.z);

        //angle = transform.localEulerAngles.x - Input.GetAxis("Mouse Y");
        //Debug.Log("angle = " + angle);

        //if (angle > 60)
        //    transform.eulerAngles = new Vector3(60, transform.localEulerAngles.y, transform.localEulerAngles.z);
        //if (angle < -60)
        //    transform.eulerAngles = new Vector3(-60, transform.localEulerAngles.y, transform.localEulerAngles.z);
        RotateClampX(obj.transform, deta, -30, 60, eulerAngles.y, eulerAngles.z);
    }


    public void RotateClampX(Transform t, float degree, float min, float max, float y, float z)
    {
        degree = (t.localEulerAngles.x - degree);
        if (degree > 180f)
        {
            degree -= 360f;
        }

        degree = Mathf.Clamp(degree, min, max);
        //t.localEulerAngles = t.localEulerAngles.SetX(degree);
        t.transform.localEulerAngles = new Vector3(degree, y, z);
    }

}
