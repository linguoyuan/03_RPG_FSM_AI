using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{

    public float maxDis = 15;

    public float minDis = 1;

    public float minAngle = 0;
    public float maxAngle = 0;

    private Camera _camera;

    public Transform Axis;

    float dis = 0;
    float disFlag = 0;
    float deta = 0f;
    Vector3 axisLocalEulerAngles;
    Vector3 cameraCtlLocalEulerAngles;

    private void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
    }
    public void Update()
    {
        dis = Vector3.Distance(_camera.transform.localPosition, transform.localPosition);
        disFlag = Input.GetAxis("Mouse ScrollWheel");
        if (dis < minDis)
        {
            if (disFlag < 0)//已到达最近距离，只允许往后拉
            {
                _camera.transform.localPosition += new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel"));
            }
        }           
        else if (dis > maxDis)
        {
            if (disFlag > 0)//已到达最远距离，只允许往前推
            {
                _camera.transform.localPosition += new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel"));
            }
        }
        else
        {
            //镜头远近
            _camera.transform.localPosition += new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel"));
        }

        //右键旋转视野
        if (Input.GetMouseButton(1))
        {
            
            SetRotate();
        }

    }

    private void SetRotate()
    {
        cameraCtlLocalEulerAngles = transform.localEulerAngles;
        //y轴方向的旋转，也就是水平（左右）方向旋转---CameraControl
        transform.localEulerAngles = new Vector3(cameraCtlLocalEulerAngles.x, cameraCtlLocalEulerAngles.y + Input.GetAxis("Mouse X"), cameraCtlLocalEulerAngles.z);

        deta = Input.GetAxis("Mouse Y");
        axisLocalEulerAngles = transform.localEulerAngles;
        RotateClampX(Axis.transform, deta, minAngle, maxAngle, axisLocalEulerAngles.y, axisLocalEulerAngles.z);


    }

    private void RotateClampX(Transform t, float degree, float min, float max, float y, float z)
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
