using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening.Core;
using Fusion;

public class Ball : NetworkBehaviour
{
    public bool transformMovement;

    public DOTweenAnimation[] animations;


    public AudioSource BallHitSound { get; set; }
    private TrailRenderer trailRenderer;

    private NetworkRigidbody2D m_NetRigidbody;
    private bool canSwing = true;
    private Vector2 m_BallDirection;


    private Collider2D _hitCollider;
    private Collider2D _ballCollider;

    [SerializeField] private SpriteRenderer[] ballVisuals;

    public float ballSpeed = 30f;

    [SerializeField] private int trackingObject;

    public override void Spawned()
    {

        m_NetRigidbody = GetComponent<NetworkRigidbody2D>();

        if (!transformMovement)
        {

            if (Object.HasInputAuthority)
                m_NetRigidbody.InterpolationDataSource = InterpolationDataSources.Predicted;
        }


        _ballCollider = GetComponent<Collider2D>();
        BallHitSound = GetComponent<AudioSource>();
        trailRenderer = GetComponent<TrailRenderer>();
        trackingObject = default;
        //m_NetRigidbody.Rigidbody.velocity = new Vector2(0,1) * ballSpeed;
    }

    public float radius = 0.6f;

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority)
            return;

        //DetectCollisions();
        BallController();
    }

    private void DetectCollisions()
    {
        //_hitCollider = Runner.GetPhysicsScene2D().OverlapBox(transform.position, _ballCollider.bounds.size * 1f, 0, LayerMask.GetMask("Paddle"));
        //if (_hitCollider != default)
        //{
        //    if (_hitCollider.gameObject.TryGetComponent(out TennisMovement racket))
        //    {
        //        if (trackingObject == _hitCollider.transform) // means collided with same object so do nothing
        //            return;

        //        BallHitSound.Play();
        //        trackingObject = _hitCollider.transform;

        //        canSwing = true;
                
        //        if (!transformMovement)
        //        {
        //            m_NetRigidbody.Rigidbody.velocity = Vector2.zero;
        //        }

        //        float x = 0f;

        //        // if racket is not moving we have to set x to zero
        //        x = hitFactor(transform.position, racket.transform.position, _hitCollider.bounds.size.x);
        //        x = Mathf.Clamp(x, -0.5f, 0.5f);

        //        float y = racket.yDirectionParameter * -1;
        //        m_BallDirection = new Vector2(x, y).normalized;

        //        if (!transformMovement)
        //        {
        //            m_NetRigidbody.Rigidbody.velocity = m_BallDirection * ballSpeed;
        //        }

        //        BallHitSound.Play();
        //        racket.moveDirection = 0;
        //    }

        //}
    }


    private void BallController()
    {
        if (transformMovement)
        {
            transform.Translate(m_BallDirection * Runner.DeltaTime * ballSpeed);
        }
        //m_NetRigidbody.Rigidbody.MovePosition((Vector2)transform.position + m_BallDirection * Runner.DeltaTime * ballSpeed);

        if (!transformMovement)
        {
            if (m_NetRigidbody.Rigidbody.velocity.magnitude > ballSpeed)
                m_NetRigidbody.Rigidbody.velocity = Vector3.ClampMagnitude(m_NetRigidbody.Rigidbody.velocity, ballSpeed);
        }

        if (trackingObject != null)
        {
            // if (canSwing && (int)Mathf.Abs(transform.position.y) == 2)
            // {
            //     canSwing = false;
            //     ballHitSound.Play();
            // }

            //if (!canSwing)
            //    return;
            //var tennisMovement = trackingObject.GetComponent<TennisMovement>();
            //switch (tennisMovement.yDirectionParameter)
            //{
            //    case 1:
            //        if (transform.position.y > 0 && transform.position.y < 1.5)
            //        {

            //            m_Rigidbody.velocity += new Vector2(tennisMovement.moveDirection, 0);
            //            tennisMovement.moveDirection = 0;
            //        }
            //        break;
            //    case -1:
            //        if (transform.position.y < 0 && transform.position.y > -1.5)
            //        {
            //            m_Rigidbody.velocity += new Vector2(tennisMovement.moveDirection, 0);
            //            tennisMovement.moveDirection = 0;
            //        }
            //        break;
            //}
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(transform.position, 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out TennisMovement racket))
        {
            
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_ReceiveCollision(int padelId,float forwardDir, Vector3 pedalPosition ,Vector3 bound)
    {
        if (trackingObject == padelId) // means collided with same object so do nothing
            return;

       
        trackingObject = padelId;

        if (!transformMovement)
            m_NetRigidbody.Rigidbody.velocity = Vector2.zero;

        float x = hitFactor(transform.position, pedalPosition, bound.x);
        x = Mathf.Clamp(x, -0.5f, 0.5f);

        float y = forwardDir * -1; // Reverse it
        m_BallDirection = new Vector2(x, y).normalized;

        if (!transformMovement)
            m_NetRigidbody.Rigidbody.velocity = m_BallDirection * ballSpeed;

        BallHitSound.Play();
        //tennis.moveDirection = 0;
    }

    public void ReceiveCollision(int padelId, float forwardDir, Vector3 pedalPosition, Vector3 bound)
    {
        if (trackingObject == padelId) // means collided with same object so do nothing
            return;


        trackingObject = padelId;

        if (!transformMovement)
            m_NetRigidbody.Rigidbody.velocity = Vector2.zero;

        float x = hitFactor(transform.position, pedalPosition, bound.x);
        x = Mathf.Clamp(x, -0.5f, 0.5f);

        float y = forwardDir * -1; // Reverse it
        m_BallDirection = new Vector2(x, y).normalized;

        if (!transformMovement)
            m_NetRigidbody.Rigidbody.velocity = m_BallDirection * ballSpeed;

        BallHitSound.Play();
        //tennis.moveDirection = 0;
    }

    float hitFactor(Vector2 ballPos, Vector2 racketPos, float racketWidth)
    {
        return (ballPos.x - racketPos.x) / racketWidth;
    }




    public void Reset(Transform standingPosition = null)
    {
        m_BallDirection = Vector2.zero;
        canSwing = true;
        //m_NetRigidbody.Rigidbody.velocity = Vector3.zero;
        //m_NetRigidbody.Rigidbody.angularVelocity = 0f;
        trackingObject = default;

        // reset Trail Renderer

        if (standingPosition != null)
        {
            ToggleBallVisuals(false);
            StartCoroutine(ResetPosition());
        }

        IEnumerator ResetPosition()
        {
            yield return new WaitForEndOfFrame();
            transform.position = standingPosition.position;
            ToggleBallVisuals(true);
        }
    }

    private void ToggleBallVisuals(bool status)
    {
        foreach (var item in ballVisuals)
        {
            item.enabled = status;
        }
    }

}



