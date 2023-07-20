using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using Fusion;
/*using Photon.Realtime;
using Photon.Pun;
using Photon;*/
public class Ball : NetworkBehaviour
{
    public DOTweenAnimation[] animations; 
    public float hitForce;
    Vector2 startingPosition;
    public GameObject ballHitEffect;
    public bool isApplyingForce, isHit;
    
    public Rigidbody2D rb { get; private set; }
    private AudioSource ballHitSound;
    private TrailRenderer trailRenderer;
    private List<Coroutine> m_BallCorotine = new List<Coroutine>();

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

    public override void FixedUpdateNetwork()
    {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, 15f);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            BounceAnimation();

            if (collision.gameObject.TryGetComponent(out TennisMovement tennisRacket))
            {
                rb.AddForce(tennisRacket.paddleDragDirection * hitForce, ForceMode2D.Force);
            }
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
    
    public void SetToDefaultPos()
    {
        ballHitEffect.SetActive(false);
        isHit = false;
        isApplyingForce = false;
        print("Ball Set To default");
        trailRenderer.enabled = false;
    }

}



