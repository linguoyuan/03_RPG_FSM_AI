using UnityEngine;
using System.Collections;

namespace ARPGSimpleDemo.Character
{
    /// <summary>
    /// 角色马达，控制角色的行动
    /// 有两种方式控制角色移动，可以用代码的方式，也可以用动画本身的移动，这里使用代码的方式，代码的方式需要调好速度，不然容易有滑步现象
    /// 角色动画的播放可以用动画状态机，也可以直接使用动画Animation组件进行播放,这里采用状态机的方式，能更好对动画融合过渡
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMotor : MonoBehaviour
    {
        public float speed = 6.0F;
        public float jumpSpeed = 8.0F;
        public float gravity = 20.0F;
        private Vector3 moveDirection = Vector3.zero;
        CharacterController controller;
        private Animator animator;
        private int walk = Animator.StringToHash("walk");
        private int walkBack = Animator.StringToHash("walkBack");
        private int left = Animator.StringToHash("left");
        private int right = Animator.StringToHash("right");

        //角色控制器
        private CharacterAnimation chAnimation = null;

        private int direction = 0;


        private float x = 0.0f;
        private float y = 0.0f;
        float xSpeed = 1f;
        float ySpeed = 10f;
        float yMinLimit = -20;
        float yMaxLimit = 80;

        public void Start()
        {
            controller = GetComponent<CharacterController>();
            chAnimation = GetComponent<CharacterAnimation>();
            animator = GetComponent<Animator>();
            //Cursor.lockState = CursorLockMode.Locked;



            var angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;
        }
        private void FixedUpdate()
        {
            if (controller.isGrounded)
            {
                
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= speed;
                if (Input.GetButton("Jump"))
                    moveDirection.y = jumpSpeed;
            }
            moveDirection.y -= gravity * Time.deltaTime;

            //Debug.Log("moveDirection.x = " + moveDirection.x);
            if (moveDirection.z > 0)
            {
                direction = 1;
            }
            else if (moveDirection.z < 0)
            {
                direction = 2;
            }
            else if (moveDirection.x < 0)
            {
                direction = 3;
            }
            else if (moveDirection.x > 0)
            {
                direction = 4;
            }
            else
            {
                direction = 0;
            }

            switch (direction)
            {
                case 1:
                    animator.SetBool(walk, true);
                    animator.SetBool(walkBack, false);
                    animator.SetBool(left, false);
                    animator.SetBool(right, false);
                    break;
                case 2:
                    animator.SetBool(walk, false);
                    animator.SetBool(walkBack, true);
                    animator.SetBool(left, false);
                    animator.SetBool(right, false);
                    break;
                case 3:
                    animator.SetBool(walk, false);
                    animator.SetBool(walkBack, false);
                    animator.SetBool(left, true);
                    animator.SetBool(right, false);
                    break;
                case 4:
                    animator.SetBool(walk, false);
                    animator.SetBool(walkBack, false);
                    animator.SetBool(left, false);
                    animator.SetBool(right, true);

                    break;
                case 0:
                    animator.SetBool(walk, false);
                    animator.SetBool(walkBack, false);
                    animator.SetBool(left, false);
                    animator.SetBool(right, false);
                    break;
                default:
                    break;
            }

            controller.Move(moveDirection * Time.deltaTime);

            if (Input.GetMouseButton(1))
            {
                y -= Input.GetAxis("Horizontal") * ySpeed * Time.deltaTime;
                var rotation = Quaternion.Euler(0, y, 0);
                //transform.rotation = rotation;
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.5f);
            }

        }
    }
}
