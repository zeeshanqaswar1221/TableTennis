using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;
/*using Photon.Pun;*/
using Fusion;
using Unity.VisualScripting;

public class TennisMovement : NetworkBehaviour
{
    public SpriteRenderer interpolator;
    private bool isDragging; // Flag to indicate if the tennis is being dragged
    private Vector2 touchOffset; // Offset between the touch position and the tennis object position
    private Vector2 startingPosition;
    private Vector2 touchStartPosition;
    private float touchStartTime;
    public float paddleHitSpeed;
    public float hitForce = 12f;

    public Vector2 initialPosition, finalPosition, directionPaddle;
    private float paddleAngle;
    public GameObject paddleHitEffect;
    GameObject currentBall;
    Vector2 currentforce;

    private Rigidbody2D m_Rigidbody;

    public override void Spawned()
    {
        startingPosition = transform.position;
        m_Rigidbody = GetComponent<Rigidbody2D>();
        paddleHitEffect = transform.GetChild(0).gameObject;
        SetToDefaultPos();
    }


    #region New Movement 

    public bool dragging = false;
    private Vector3 offset;

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        if (dragging)
        {
            // Move object, taking into account original offset.
            //transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            m_Rigidbody.MovePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset);
        }
    }

    private void OnMouseDown()
    {
        // Record the difference between the objects centre, and the clicked point on the camera plane.
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
    }

    private void OnMouseUp()
    {
        // Stop dragging.
        dragging = false;
    }

    #endregion

    //public override void FixedUpdateNetwork()
    //{
    //    if (!Object.HasInputAuthority)
    //        return;

    //    PlayerMovement();
    //}

    private void PlayerMovement()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (!IsTouchWithinArea(touch.position))
                return;

            SpeedCheck(touch);
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                Collider2D collider = Physics2D.OverlapPoint(touchPosition);

                if (collider != null && collider.gameObject == gameObject)
                {
                    isDragging = true;
                    touchOffset = (Vector2)transform.position - touchPosition;
                    initialPosition = transform.position;
                }
                else
                {

                    MoveToPosition(touchPosition);

                }
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                transform.position = touchPosition + touchOffset;


                // Clamp the tennis object's position within the table boundaries
                Vector2 clampedPosition = transform.position;
                transform.position = clampedPosition;

                finalPosition = transform.position;
                directionPaddle = (finalPosition - initialPosition).normalized;
                paddleAngle = Vector2.Angle(initialPosition, finalPosition);
                paddleAngle = ClampAngle(paddleAngle);
                //                print(paddleAngle);
                initialPosition = finalPosition;

            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;

            }
        }
    }

    private void MoveToPosition(Vector2 targetPosition)
    {
        // Calculate the distance between the current position and the target position
        float distance = Vector2.Distance(transform.position, targetPosition);

        // Move the sprite to the target position using DOTween
        transform.DOMove(targetPosition, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            isDragging = true;
            touchOffset = (Vector2)transform.position - targetPosition;

        });
    }
    
    public void SetToDefaultPos()
    {
        transform.position = startingPosition;
    }
    
    public void SpeedCheck(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            touchStartPosition = touch.position;
            touchStartTime = Time.time;
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            // Calculate the distance traveled and the touch duration
            Vector2 touchCurrentPosition = touch.position;
            float touchCurrentTime = Time.time;

            float touchDuration = touchCurrentTime - touchStartTime;
            float touchDistance = Vector2.Distance(touchStartPosition, touchCurrentPosition);

            // Calculate touch speed (distance divided by duration)
            paddleHitSpeed = touchDistance / touchDuration;
            paddleHitSpeed = paddleHitSpeed / 20;

        }
        else if (touch.phase == TouchPhase.Ended)
        {
            paddleHitSpeed = 0f; // Reset touch speed when touch ends
        }

    }

    bool once = false;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag != "Ball") return;
        
        Rigidbody2D ball = PongGameController.Instance.gameBall.GetComponent<Rigidbody2D>();

        once = true; 
        //ball.velocity = Vector2.zero;

        //if (transform.position.x > 2.3)
        //{
        //    ball.AddForce(new Vector2(-paddleAngle, hitForce), ForceMode2D.Impulse);
        //    return;
        //}
        //if (transform.position.x <= -2.4)
        //{
        //    ball.AddForce(new Vector2(5, hitForce), ForceMode2D.Impulse);
        //}
        //if (directionPaddle.x >= 0)
        //{
        //    ball.AddForce(new Vector2(paddleAngle, hitForce), ForceMode2D.Impulse);
        //}
        //if (directionPaddle.x < 0)
        //{
        //    ball.AddForce(new Vector2(-paddleAngle, hitForce), ForceMode2D.Impulse);
        //}


    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.tag != "Ball") return;
    //     
    //     PongGameController.Instance.gameBall.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    //     
    //     if (Object.HasInputAuthority)
    //     {
    //         if (transform.position.x > 2.3)
    //         {
    //             PongGameController.Instance.gameBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(-paddleAngle, hitForce), ForceMode2D.Impulse);
    //             return;
    //         }
    //         if (transform.position.x <= -2.4)
    //         {
    //             PongGameController.Instance.gameBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(5, hitForce), ForceMode2D.Impulse);
    //         }
    //         if (directionPaddle.x >= 0)
    //         {
    //             PongGameController.Instance.gameBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(paddleAngle, hitForce), ForceMode2D.Impulse);
    //         }
    //         if (directionPaddle.x < 0)
    //         {
    //             PongGameController.Instance.gameBall.GetComponent<Rigidbody2D>().AddForce(new Vector2(-paddleAngle, hitForce), ForceMode2D.Impulse);
    //         }
    //     }
    // }
    
    

    float ClampAngle(float value, float min = 0, float max = 18)
    {
        value = Mathf.Clamp(value, min, max);
        return value;
    }
    
    bool IsTouchWithinArea(Vector2 touchPosition, float screenHight = 0.40f)
    {
        float minX, maxX, minY, maxY;
        
        if (Object.HasInputAuthority)
        {
            minX = 0;                  // Left edge of the screen
            maxX = Screen.width;       // Right edge of the screen
            minY = 0;                  // Bottom edge of the screen
            maxY = Screen.height * screenHight; // 50% from the bottom of the screen
                                                // Check if touch position is within the defined touch area
        }
        else
        {
            minX = 0;                         // Left edge of the screen
            maxX = Screen.width;              // Right edge of the screen
            minY = Screen.height / 1.55f;         // Top edge of the upper half of the screen
            maxY = Screen.height;
            minX = 0;                  // Left edge of the screen
            maxX = Screen.width;       // Right edge of the screen
            minY = 0;                  // Bottom edge of the screen
            maxY = Screen.height * screenHight;
        }
        return (touchPosition.x >= minX && touchPosition.x <= maxX && touchPosition.y >= minY && touchPosition.y <= maxY);
    }

}
