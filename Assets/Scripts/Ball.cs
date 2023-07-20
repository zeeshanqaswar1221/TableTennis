using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
/*using Photon.Realtime;
using Photon.Pun;
using Photon;*/
public class Ball : MonoBehaviour
{
    public float hitForce;
    Vector2 startingPosition;
    public GameObject ballHitEffect;
    public bool isApplyingForce, isHit;
    
    public Rigidbody2D rb { get; private set; }
    private AudioSource ballHitSound;
    private TrailRenderer trailRenderer;
    private List<Coroutine> m_BallCorotine = new List<Coroutine>();
    
    private void Awake()
    {
        ballHitEffect = transform.GetChild(0).gameObject;
        ballHitSound = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Master") || collision.gameObject.CompareTag("Client"))
        {
            Vector3 dir = transform.position - collision.transform.position;
            rb.AddForce(dir.normalized * hitForce, ForceMode2D.Impulse);
            //if (collision.gameObject.TryGetComponent<TennisMovement>(out TennisMovement tennisRacket))
            //{
            //    Vector3 dir = transform.position - collision.transform.position;

            //    if (tennisRacket.dragging)
            //    {
            //        rb.AddForce(dir.normalized * hitForce, ForceMode2D.Impulse);
            //    }
            //    else
            //    {
            //        rb.AddForce(dir.normalized * (hitForce * 0.5f), ForceMode2D.Impulse);
            //    }
            //}
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



