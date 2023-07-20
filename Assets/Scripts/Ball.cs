using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using Fusion;
using UnityEngine.XR;

/*using Photon.Realtime;
using Photon.Pun;
using Photon;*/
public class Ball : NetworkBehaviour
{
    public DOTweenAnimation[] animations; 
    Vector2 startingPosition;
    public GameObject ballHitEffect;
    
    public Rigidbody2D rb { get; private set; }
    private AudioSource ballHitSound;
    private TrailRenderer trailRenderer;
    private List<Coroutine> m_BallCorotine = new List<Coroutine>();

    [SerializeField] private SpriteRenderer[] ballVisuals;
    
    [SerializeField] private float translationSpeed = 10f;
    [SerializeField] private float sideWaySpeed = 5f;
    [SerializeField] private float swingStartTime;
    
    
    [SerializeField] private Transform trackingObject;

    private Coroutine swingCoroutineHandler;

    private Stopwatch stopWatch;
    
    public override void Spawned()
    {
        ballHitEffect = transform.GetChild(0).gameObject;
        ballHitSound = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();

    }

    public void BounceAnimation()
    {
        foreach (var item in animations)
        {
            item.DOPlay();
        }
    }
    
    public void Toggle(bool status)
    {
        foreach (var item in ballVisuals)
        {
            item.enabled = status;
        }
    }

    [Networked] TickTimer timer { get; set; }
    public bool canSwing = false;
    public bool canEnter = false;
    
    public override void FixedUpdateNetwork()
    {
        if (trackingObject != null)
        {
            if (!canEnter)
            {
                swingCoroutineHandler = StartCoroutine(SwingEnableTIme());
                canEnter = true;
            }
            
            Vector3 finalDirection = trackingObject.up.normalized * translationSpeed;
            
            if (canSwing)
            {
                if (trackingObject.TryGetComponent(out TennisMovement tennis))
                {
                    if (tennis.MoveState == TennisMovement.MovementState.MovingRight)
                    {
                        finalDirection.x  = sideWaySpeed;
                    }
                    else if (tennis.MoveState == TennisMovement.MovementState.MovingLeft)
                    {
                        finalDirection.x  = -sideWaySpeed;
                    }
                }

                tennis.MoveState = TennisMovement.MovementState.NoMovement;
                StopCoroutine(swingCoroutineHandler);
            }
            
            transform.Translate(finalDirection * Runner.DeltaTime, Space.World);
        }
    }

    IEnumerator SwingEnableTIme()
    {
        yield return new WaitForSeconds(0.5f);
        print("Swing Enabled");
        canSwing = true;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            canSwing = false;
            canEnter = false;
            Reset();
            BounceAnimation();
            trackingObject = collision.transform;
        }
    }

    public void Bounce()
    {
        //m_BallCorotine.Clear();
        //m_BallCorotine.Add(StartCoroutine(BallBounce()));
        
        //IEnumerator BallBounce()
        //{
        //    float delay = Random.Range(0.1f, 0.25f);
        //    yield return new WaitForSeconds(delay);
        //    //  IntiateHitEffect();
        //    ballHitEffect.SetActive(true);
        //    yield return new WaitWhile(() => ballHitSound.isPlaying);
        //    ballHitSound.Play();
        //}
    }

    public void Reset(Transform standingPosition = null)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;
        trackingObject = null;
        canSwing = false;
        
        // reset Trail Renderer

        if (standingPosition != null)
        {
            Toggle(false);
            StartCoroutine(ResetPosition());
        }

        IEnumerator ResetPosition()
        {
            yield return new WaitForEndOfFrame();
            transform.position = standingPosition.position;
            Toggle(true);
        }
    }

}



