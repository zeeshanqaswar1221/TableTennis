using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class wall : MonoBehaviour
{
    // public Transform rightBoundary; // Right boundary position of the half table
    // public Transform topBoundary; // Top boundary position of the half table
    // public Transform bottomBoundary; // Bottom boundary position of the half table

    private bool isDragging; // Flag to indicate if the tennis is being dragged
    private Vector2 touchOffset; // Offset between the touch position and the tennis object position
    private Vector2 startingPosition;
    private Vector2 touchStartPosition;
    private float touchStartTime;
    public float paddleHitSpeed;
    public float hitForce = 18f;

    private Vector2 initialPosition, finalPosition, directionPaddle;
    private float paddleAngle;

    private void OnEnable()
    {
        SetToDefaultPos();
    }
    private void Awake()
    {
        startingPosition = transform.position;

    }


    // void Update()
    // {

    //     if (Input.touchCount > 0)
    //     {
    //         Touch touch = Input.GetTouch(0);
    //         // if (!IsTouchWithinArea(touch.position))
    //         // {
    //         //     return;
    //         // }

    //         //  print($"touch{Input.to} ");



    //         SpeedCheck(touch);
    //         if (touch.phase == TouchPhase.Began)
    //         {
    //             Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
    //             Collider2D collider = Physics2D.OverlapPoint(touchPosition);

    //             if (collider != null && collider.gameObject == gameObject)
    //             {
    //                 isDragging = true;
    //                 touchOffset = (Vector2)transform.position - touchPosition;
    //                 initialPosition = transform.position;
    //             }
    //             else
    //             {

    //                 MoveToPosition(touchPosition);

    //             }
    //         }
    //         else if (touch.phase == TouchPhase.Moved && isDragging)
    //         {
    //             Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
    //             transform.position = touchPosition + touchOffset;


    //             // Clamp the tennis object's position within the table boundaries
    //             Vector2 clampedPosition = transform.position;
    //             //                clampedPosition.x = Mathf.Clamp(clampedPosition.x, leftBoundary.position.x, rightBoundary.position.x);
    //             //     clampedPosition.y = Mathf.Clamp(clampedPosition.y, bottomBoundary.position.y, topBoundary.position.y);
    //             transform.position = clampedPosition;

    //             finalPosition = transform.position;
    //             directionPaddle = (finalPosition - initialPosition).normalized;
    //             paddleAngle = Vector2.Angle(initialPosition, finalPosition);
    //             paddleAngle = ClampAngle(paddleAngle);
    //             //                print(paddleAngle);
    //             initialPosition = finalPosition;

    //         }
    //         else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
    //         {
    //             isDragging = false;

    //         }
    //     }
    // }

    private void MoveToPosition(Vector2 targetPosition)
    {
        // Clamp the target position within the table boundaries
        //  targetPosition.x = Mathf.Clamp(targetPosition.x, leftBoundary.position.x, rightBoundary.position.x);
        // targetPosition.y = Mathf.Clamp(targetPosition.y, bottomBoundary.position.y, topBoundary.position.y);


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
            // Store the starting position and time of the touch
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
            //            Debug.LogError(paddleHitSpeed);

            //            Debug.Log("Touch Speed: " + touchSpeed);
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            paddleHitSpeed = 0f; // Reset touch speed when touch ends
        }

    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag != "Ball") return;
        Ball ball = other.gameObject.GetComponent<Ball>();
        ball.rb.velocity = Vector2.zero;
        // if (transform.position.x > 2.3)
        // {
        //     ball.AddForceToBall(new Vector2(-paddleAngle, hitForce), ForceMode2D.Impulse);

        //     // ball.rb.AddForce(new Vector2(-5, hitForce), ForceMode2D.Impulse);
        //     return;
        // }
        // if (transform.position.x <= -2.4)
        // {
        //     ball.AddForceToBall(new Vector2(5, hitForce), ForceMode2D.Impulse);

        //     // ball.rb.AddForce(new Vector2(5, hitForce), ForceMode2D.Impulse);
        //     return;

        // }
        // // Debug.LogError(speedMultiplier);
        // if (directionPaddle.x >= 0)
        // {
        //     ball.AddForceToBall(new Vector2(paddleAngle, hitForce), ForceMode2D.Impulse);
        //     // ball.rb.AddForce(new Vector2(paddleAngle, hitForce), ForceMode2D.Impulse);
        // }
        // if (directionPaddle.x < 0)
        // {
        //     ball.AddForceToBall(new Vector2(-paddleAngle, hitForce), ForceMode2D.Impulse);

        //     // ball.rb.AddForce(new Vector2(-paddleAngle, hitForce), ForceMode2D.Impulse);
        // }
        //  print("Paddle Angle" + paddleAngle);
        //
        // ball.ApplyForce(Clampvalue(paddleHitSpeed * 3.5f), transform);
        //  print("Hit With Ball");
        //        print("Paddle Angle" + paddleAngle);


    }
}
