/******************************************************************************
 * Logic for moving player.
 * 
 * Authors: Bill, Hamza, Max, Ryan
 *****************************************************************************/

using UnityEngine;
using MLAPI;

public class PlayerMovement : NetworkBehaviour
{
    // Grab value from playcontroller.playerstats later
    [SerializeField]
    private float moveSpeed = 7f;

    /// <summary>
    /// Direction to move the player towards.
    /// </summary>
    private Vector3 moveVector;

    /// <summary>
    /// Toggle to 
    /// </summary>
    public bool canMove = true;

    /// <summary>
    /// Rigidbody to move player.
    /// </summary>
    private Rigidbody2D rb;

    /// <summary>
    /// Mobile joystick.
    /// </summary>
    private FloatingJoystick stick;

    private bool isPaused = false;

    /// <summary>
    /// Start and grab references.
    /// </summary>
    void Start()
    {
        if (IsLocalPlayer)
        {
            rb = gameObject.GetComponent<Rigidbody2D>();

            try
            {
                stick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<FloatingJoystick>();
            }
            catch
            {
            }
        }
    }

    /// <summary>
    /// Normal update. Grab input.
    /// </summary>
    public void UpdateMovement()
    {
        if (canMove)
        {
            GrabInputPC();
#if UNITY_ANDROID
            GrabInputAndroid();
#endif
            if (moveVector.magnitude > 0 && isPaused)
            {
                AudioManager._instance.Play("Walking");
                isPaused = false;
            }
            else if (moveVector.magnitude <= 0 && !isPaused)
            {
                AudioManager._instance.Pause("Walking");
                isPaused = true;
            }
        }
    }

    /// <summary>
    /// PC input.
    /// </summary>
    void GrabInputPC()
    {
        moveVector = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        moveVector = Vector3.ClampMagnitude(moveVector, 1f);
    }

    /// <summary>
    /// Android input.
    /// </summary>
    void GrabInputAndroid()
    {
        moveVector = new Vector3(stick.Horizontal, stick.Vertical, 0);
        moveVector = Vector3.ClampMagnitude(moveVector, 1f);
    }

    /// <summary>
    /// Fixed update. Move player.
    /// </summary>
    public void UpdateFixedMovement()
    {
        rb.MovePosition(transform.position + moveVector * moveSpeed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Allow player to move.
    /// </summary>
    public void turnOnMove()
    {
        canMove = true;
    }

    /// <summary>
    /// Don't allow player to move.
    /// </summary>
    public void turnOffMove()
    {
        canMove = false;
        moveVector = Vector3.zero;
    }
}
