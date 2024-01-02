using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;


public class BallUserControl : MonoBehaviour
{
    private Ball ball;
    private Vector3 move;
    private Vector3 camForward;
    private bool jump;
    private bool canJump;
    public bool allowJump;
    public Joystick joystick;
    float hInput = 0f;
    float vInput = 0f;
    float finalhInput = 0f;
    float finalvInput = 0f;
    private PlayerPrefsController playerPrefsController;
    private LevelChanger levelChanger;
    Goal goal;

    private void Awake()
    {
        ball = GetComponent<Ball>();
        allowJump = true;
    }

    private void Start() {
        playerPrefsController = FindObjectOfType<PlayerPrefsController>();
        levelChanger = FindObjectOfType<LevelChanger>();
        goal = FindObjectOfType<Goal> ();
    }

    private void Update()
    {
        //  if DeviceType.Handheld

        //if (!playerPrefsController.pauseLock)
        //{
            //hInput = CrossPlatformInputManager.GetAxis("Horizontal");
            //vInput = CrossPlatformInputManager.GetAxis("Vertical");
            //if (vInput >= .3f || vInput <= -.3f)
            //{
            //    finalvInput = vInput;
            //}
            //else
            //{
            //    finalvInput = 0f;
            //}
            //if (hInput >= .3f || hInput <= -.3f)
            //{
            //    finalhInput = hInput;
            //}
            //else
            //{
            //    finalhInput = 0f;
            //}
            //Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            //if (CrossPlatformInputManager.GetAxisRaw("Horizontal") == 0 && !playerPrefsController.isPaused)
            //{
            //    ball.SlowBallx();
            //}
            //if (CrossPlatformInputManager.GetAxisRaw("Vertical") == 0 && !playerPrefsController.isPaused)
            //{
            //    ball.SlowBallz();
            //}
            //move = ((finalvInput * Vector3.forward) + (finalhInput * Vector3.right)).normalized;
        //}

            //  if DeviceType.Desktop

            hInput = Input.GetAxis("Horizontal");
            vInput = Input.GetAxis("Vertical");
            Jump = Input.GetButtonDown("Jump");
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                ball.SlowBallx();
            }
            if (Input.GetAxisRaw("Vertical") == 0)
            {
                ball.SlowBallz();
            }
            move = ((vInput * Vector3.forward) + (hInput * Vector3.right)).normalized;


            if (Input.GetButtonDown("Fire1") && !playerPrefsController.isPaused && !goal.betweenLevel)
            {
                playerPrefsController.Pause();
            }
            else if (Input.GetButtonDown("Fire1") && playerPrefsController.isPaused && !FindObjectOfType<OptionsSceneController>() && !FindObjectOfType<LevelSceneController>())
            {
                playerPrefsController.UnPause();
            }

            if (Jump)
            {
                CheckCanJump();
                if (CanJump)
                {
                    ball.BallJump();
                }
            }
    }


    private void FixedUpdate()
    {
            if (!playerPrefsController.pauseLock)
        {
        ball.Move(move);
        }
    }

    private bool CheckCanJump()
    {
            if (allowJump && Physics.Raycast(transform.position, -Vector3.up, 0.6f))
            {
                CanJump = true;
            }
            else
            {
                CanJump = false;
            }
            return CanJump;
    }

    public bool CanJump
    {
        get { return canJump; }
        set { canJump = value; }
    }

    public bool Jump
    {
        get { return jump; }
        set { jump = value; }
    }

}
