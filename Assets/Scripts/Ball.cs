using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using DG.Tweening.Core;
using Fusion;

public class Ball : NetworkBehaviour
{
    public DOTweenAnimation[] animations;

    public GameObject ballHitEffect;

    public Rigidbody2D rb { get; private set; }
    private AudioSource ballHitSound;
    private TrailRenderer trailRenderer;

    private Rigidbody2D m_Rigidbody;


    [SerializeField] private SpriteRenderer[] ballVisuals;

    public float ballSpeed = 30f;

    [SerializeField] private Transform trackingObject;

    public override void Spawned()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        ballHitEffect = transform.GetChild(0).gameObject;
        ballHitSound = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        ballHitEffect = transform.GetChild(0).gameObject;
        ballHitSound = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
    }


    private void Start()
    {
        //m_Rigidbody.velocity = Vector2.up * ballSpeed;
    }

    public void BounceAnimation()
    {
        foreach (var item in animations)
        {
            item.DOPlay();
        }
    }

    public override void FixedUpdateNetwork()
    {
    }

    public bool canSwing = true;
    private void FixedUpdate()
    {
        // Limit Velocity
        if (m_Rigidbody.velocity.magnitude > ballSpeed)
            m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, ballSpeed);

        if (trackingObject != null)
        {
            // if (canSwing && (int)Mathf.Abs(transform.position.y) == 2)
            // {
            //     canSwing = false;
            //     ballHitSound.Play();
            // }

            if (!canSwing)
                return;
            var tennisMovement = trackingObject.GetComponent<TennisMovement>();
            switch (tennisMovement.yDirectionParameter)
            {
                case 1:
                    if (transform.position.y > 0 && transform.position.y < 1.5)
                    {

                        m_Rigidbody.velocity += new Vector2(tennisMovement.moveDirection, 0);
                        tennisMovement.moveDirection = 0;
                    }
                    break;
                case -1:
                    if (transform.position.y < 0 && transform.position.y > -1.5)
                    {
                        m_Rigidbody.velocity += new Vector2(tennisMovement.moveDirection, 0);
                        tennisMovement.moveDirection = 0;
                    }
                    break;
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out TennisMovement racket))
        {
            if (trackingObject == collision.transform) // means collided with same object so do nothing
                return;

            canSwing = true;
            trackingObject = collision.transform;
            m_Rigidbody.velocity = Vector2.zero;

            float x = 0f;

            // if racket is not moving we have to set x to zero
            x = hitFactor(transform.position, racket.transform.position, collision.collider.bounds.size.x);
            x = Mathf.Clamp(x, 0, 0.5f);

            float y = racket.yDirectionParameter * -1;
            Vector2 dir = new Vector2(x, y).normalized;
            m_Rigidbody.velocity = dir * ballSpeed;

            ballHitSound.Play();
            racket.moveDirection = 0;
            //BounceAnimation();
        }
    }

    float hitFactor(Vector2 ballPos, Vector2 racketPos, float racketWidth)
    {
        return (ballPos.x - racketPos.x) / racketWidth;
    }

    public void Reset(Transform standingPosition = null)
    {
        canSwing = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;
        trackingObject = null;

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



