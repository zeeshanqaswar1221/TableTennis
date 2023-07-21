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

    public bool dragging = false;
    private Vector3 offset;
    private Vector3 m_InitalPos;

    public int moveDirection = 0; // 1 means right -1 means left

    private Rigidbody2D m_Rigidbody;
    public Vector3 paddleDragDirection { get; set; }

    public float yDirectionParameter;

    public override void Spawned()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        paddleDragDirection = transform.forward;
        yDirectionParameter = transform.position.y / Mathf.Abs(transform.position.y);
    }

    private void Awake()
    {
        //m_Rigidbody = GetComponent<Rigidbody2D>();
        //paddleDragDirection = transform.forward;
        //yDirectionParameter = transform.position.y / Mathf.Abs(transform.position.y);
    }


    #region New Movement

    public float minDragRadius = 1f;
    public override void FixedUpdateNetwork()
    {
        TennisController();
    }

    private void FixedUpdate()
    {
        //TennisController();
    }

    private void TennisController()
    {
        if (dragging)
        {
            // Move object, taking into account original offset.
            //transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            paddleDragDirection = transform.position - m_InitalPos;
            //print(paddleDragDirection.magnitude);
            if (paddleDragDirection.magnitude > minDragRadius)
            {
                Vector3 forDirection = Vector3.Cross(Vector3.up, paddleDragDirection.normalized);
                moveDirection = forDirection.z < 0 ? 1 : forDirection.z > 0 ? -1 : 0;
            }
            else
            {
                moveDirection = 0;
            }

            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            if (yDirectionParameter > 0)
            {
                newPos.y = Mathf.Clamp(newPos.y, 3, 1000);
            }
            else
            {
                newPos.y = Mathf.Clamp(newPos.y, -1000, -3);
            }

            m_Rigidbody.MovePosition(newPos);// Movement
        }
    }

    private void OnMouseDown()
    {
        moveDirection = 0;
        m_InitalPos = transform.position;
        paddleDragDirection = transform.up.normalized;

        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
    }

    private void OnDrawGizmos()
    {

        if (m_InitalPos != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            // Gizmos.DrawLine(initalPos, transform.position);
            Gizmos.DrawRay(m_InitalPos, paddleDragDirection);
        }
    }

    private void OnMouseUp()
    {
        moveDirection = 0;
        m_InitalPos = Vector2.zero;
        dragging = false; // Stop dragging.
    }


    #endregion

}
