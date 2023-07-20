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

    private Vector2 initialPosition, finalPosition, directionPaddle;
    private float paddleAngle;
    public GameObject paddleHitEffect;
    GameObject currentBall;
    Vector2 currentforce;

    private Rigidbody2D m_Rigidbody;

    public enum MovementState { MovingRight, MovingLeft, NoMovement }

    public MovementState MoveState = MovementState.NoMovement;


    public override void Spawned()
    {
        startingPosition = transform.position;
        paddleHitEffect = transform.GetChild(0).gameObject;

        paddleDragDirection = transform.forward;
    }


    #region New Movement 

    public bool dragging = false;
    private Vector3 offset;

    public Vector3 initalPos;
    [Networked] public Vector3 paddleDragDirection { get; set; }


    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        if (dragging)
        {
            // Move object, taking into account original offset.
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            paddleDragDirection = (transform.position - initalPos).normalized;

            CheckPedalDirection();

            // m_Rigidbody.MovePosition(Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset);// Movement
        }
    }

    private void CheckPedalDirection()
    {
        Vector3 forDirection = Vector3.Cross(Vector3.up, paddleDragDirection);

        if (forDirection.z < 0)
        {
            MoveState = MovementState.MovingRight;
        }
        else if (forDirection.z > 0)
        {
            MoveState = MovementState.MovingLeft;
        }
        else
        {
            MoveState = MovementState.NoMovement;
        }
    }

    private void OnMouseDown()
    {
        initalPos = transform.position;
        paddleDragDirection = transform.up.normalized;

        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
    }

    private void OnDrawGizmos()
    {

        if (initalPos != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            // Gizmos.DrawLine(initalPos, transform.position);
            Gizmos.DrawRay(initalPos, paddleDragDirection);
        }
    }

    private void OnMouseUp()
    {
        initalPos = Vector2.zero;
        // Stop dragging.
        dragging = false;
    }

    public void Reset()
    {
        MoveState = MovementState.NoMovement;
    }

    #endregion

}
