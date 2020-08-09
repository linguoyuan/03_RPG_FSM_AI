using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    private moveController mc;      //获取人物控制组件
    Transform target;         //相机跟随的目标位置

    public float rotationDamping = 2;         //相机跟随人物的旋转速度
    public float zoomSpeed = 4;                //鼠标滚轮滑动速度

    private float h1;                      //点击鼠标右键，存储鼠标Y方向位移
    private float distance = 0;     //相机和目标的距离
                                    //private float height = 1f;       //相机和目标的高度
                                    //private float heightDamping = 1;
                                    // Vector3 offsetPosition;

    // Use this for initialization
    void Start()
    {

        target = GameObject.FindGameObjectWithTag("Player").transform;
        mc = target.gameObject.GetComponent<moveController>();
        distance = Vector3.Distance(new Vector3(0, 0, target.position.z), new Vector3(0, 0, transform.position.z));

        //offsetPosition = target.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = target.position - offsetPosition;

        flowTarget();
        zoomView();
        UpAndDownView();

    }

    /// <summary>
    /// 相机跟随人物移动旋转
    /// </summary>
    void flowTarget()
    {

        float wantedRotationAngle = target.eulerAngles.y;    //要达到的旋转角度
        //float wantedHeight = target.position.y + height;     //要达到的高度
        float currentRotationAngle = transform.eulerAngles.y; //当前的旋转角度
        float currentHeight = transform.position.y;           //当前的高度
        if (mc.cameraIsRotate)
        {
            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
        }
        // currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);    //由当前高度达到要达到的高度
        Quaternion currentRotation = Quaternion.Euler(transform.eulerAngles.x, currentRotationAngle, 0);
        // float currentRotation=1;   //防止主角回头摄像机发生旋转，  这里不用

        Vector3 ca = target.position - currentRotation * Vector3.forward * distance;     //tt是相机的位置      
                                                                                         // transform.position = target.position-currentRotation * Vector3.forward * distance;

        transform.position = new Vector3(ca.x, transform.position.y, ca.z);      //最后得到的相机位置
        transform.rotation = currentRotation;                                   //最后得到相机的旋转角度
                                                                                // transform.LookAt(target.position);

    }

    /// <summary>
    /// 滚轮控制缩放
    /// </summary>
    void zoomView()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        distance -= scrollWheel;
        if (distance > 5.6f)
        {
            distance = 5.6f;
        }
        if (distance < 0.9f)
        {
            distance = 0.9f;
        }
    }
    /// <summary>
    /// 摄像头上下视角
    /// </summary>
    void UpAndDownView()
    {


        if (Input.GetMouseButton(1))
        {
            h1 = Input.GetAxis("Mouse Y") * rotationDamping;
            Vector3 originalPosition = transform.position;
            Quaternion originalRotation = transform.rotation;
            transform.RotateAround(target.position, -target.right, h1);     //决定因素position和rotation
            float x = transform.eulerAngles.x;
            if (x < -10 || x > 80)
            {
                transform.position = originalPosition;
                transform.rotation = originalRotation;
            }

        }
    }
}