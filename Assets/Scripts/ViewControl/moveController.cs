using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARPGSimpleDemo.Character;

public class moveController : MonoBehaviour
{
    public float speed = 1;              //人物移动速度
    public float rotatinDamping = 0.5f;    //人物旋转的速度
    public float mouse1RotateDamping = 4;
    private Vector3 moveDirection = Vector3.zero;
    public float jumpSpeed = 1.0F;
    public float gravity = 20.0F;
    private int direction = 0;
    private string aniState;
    private int currentDirection = 0;
    private int last = 0;

    public bool cameraIsRotate = true;      //判断相机是否跟随人物旋转（点击鼠标左键可观看角色）


    private float h1;                //点击鼠标右键，存储鼠标X方向位移
    private float h2;                //点击鼠标左键，存储鼠标X方向位移

    float currentOnClickMouse1AngleY = 0;     //鼠标右击时人物当前的Y轴度数
    float currentCameraAngleY = 0;           //鼠标左击时相机当前的Y轴度数


    public GameObject cam;                  //人物后面的相机
    private CharacterController characterContro;   //角色控制器组件
    private CharacterAnimation chaAnimation;

    void Start()
    {
        characterContro = this.GetComponent<CharacterController>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        chaAnimation = this.GetComponent<CharacterAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        rotate();
        mouseControllerRotation();

    }
    /// <summary>
    /// 向前向后移动 跳跃
    /// </summary>
    void Move()
    {
        if (characterContro.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("跳跃");
                moveDirection.y = jumpSpeed;
            }                
        }
        moveDirection.y -= gravity * Time.deltaTime;
        characterContro.Move(moveDirection);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            direction = 1;
            aniState = "Walk_F";
            if (Input.GetKey(KeyCode.LeftShift))
            {
                direction = 5;
                aniState = "Run_F";
                characterContro.Move(transform.forward * 3 * speed * Time.deltaTime);
            }
            else
            {
                characterContro.Move(transform.forward * speed * Time.deltaTime);
            }          
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            characterContro.Move(-transform.forward * speed * Time.deltaTime);
            direction = 2;
            aniState = "Walk_B";
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            characterContro.Move(-transform.right * speed * Time.deltaTime);
            direction = 3;
            aniState = "Jog_Left";
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            characterContro.Move(transform.right * speed * Time.deltaTime);
            direction = 4;
            aniState = "Jog_Right";
        }
        else
        {
            direction = 0;
            aniState = "Pose_Idle";
        }
        currentDirection = direction;

        if (currentDirection != last)//解决动画不能有过度时间问题，但依然有bug
        {
            chaAnimation.PlayAniByNameLoop(aniState);
            last = currentDirection;
        }
        #region 废弃
        /*
        if (characterContro.isGrounded)
        {
            //这种办法，当人物自身有旋转时，会影响自己Input.GetAxis，导致左右动画不能播放
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }
        moveDirection.y -= gravity * Time.deltaTime;

        if (moveDirection.z > 0)
        {
            direction = 1;          
            aniState = "Walk_F";
        }
        else if (moveDirection.z < 0)
        {
            direction = 2;
            aniState = "Walk_B";
        }
        else if (moveDirection.x < 0)
        {
            direction = 3;
            aniState = "Jog_Left";
        }
        else if (moveDirection.x > 0)
        {
            direction = 4;
            aniState = "Jog_Right";
        }
        else
        {
            direction = 0;
            aniState = "Pose_Idle";
        }
        currentDirection = direction;

        if (currentDirection != last)
        {
            animation.PlayAniByNameLoop(aniState);
            last = currentDirection;
        }
        characterContro.Move(moveDirection * Time.deltaTime);
        */
        #endregion
    }
    /// <summary>
    /// 按左右旋转
    /// </summary>
    void rotate()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * rotatinDamping);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.up * rotatinDamping);
        }
    }
    /// <summary>
    /// 鼠标控制旋转
    /// </summary>
    void mouseControllerRotation()
    {
        //右键
        if (Input.GetMouseButtonDown(1))
        {
            currentOnClickMouse1AngleY = transform.eulerAngles.y;
            h1 = 0;
        }
        if (Input.GetMouseButton(1))
        {

            h1 += Input.GetAxis("Mouse X") * mouse1RotateDamping;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, h1 + currentOnClickMouse1AngleY, transform.eulerAngles.z);

        }

        //左键
        if (Input.GetMouseButtonDown(0))
        {
            currentCameraAngleY = cam.transform.eulerAngles.y;
            h2 = 0;
        }
        if (Input.GetMouseButton(0))
        {
            // float currentOnClickMouse1Angle = transform.eulerAngles.y;
            cameraIsRotate = false;
            h2 += Input.GetAxis("Mouse X") * mouse1RotateDamping;
            cam.transform.eulerAngles = new Vector3(cam.transform.eulerAngles.x, h2 + currentCameraAngleY, cam.transform.eulerAngles.z);

        }
        else
        {
            cameraIsRotate = true;
        }
    }
}