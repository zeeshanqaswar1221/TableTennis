using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Fusion;
using DG.Tweening;

namespace Tennis.Orthographic
{
    public class Ball : NetworkBehaviour, IReset
    {
        [SerializeField] private GameObject BallGraphics;
        [SerializeField] private DOTweenAnimation bounceAnimation,shadowAnimation;
        private Collider2D m_Collider;

        private Rigidbody2D m_Rigidbody2d;

        public AudioSource BallHitSound { get; set; }

        public override void Spawned()
        {
            m_Rigidbody2d = GetComponent<Rigidbody2D>();
            BallHitSound = GetComponent<AudioSource>();
            m_Collider = GetComponent<Collider2D>();
            m_TrackingObject = default;
        }

        public override void FixedUpdateNetwork()
        {
            if (Runner.IsForward)
            {
                m_Rigidbody2d.velocity = m_Rigidbody2d.velocity.normalized * BallSpeed();
            }


            //m_Rigidbody2d.velocity = m_Rigidbody2d.velocity.normalized * BallSpeed();
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            //if (collision.gameObject.TryGetComponent(out TennisMovement tMovement))
            //{
            //    m_Rigidbody2d.velocity = new Vector2(0f, collision.transform.position.y); 
            //}
        }

        private TennisMovement m_TrackingObject;
        private float startingPosition;

        private bool bounceCompleted = false;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!bounceCompleted)
            {
                bounceCompleted = true;
                BallHitSound.Play();
                bounceAnimation.DORestart();
                shadowAnimation.DORestart();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out TennisMovement tMovement))
            {
                if (m_TrackingObject != tMovement)
                {
                    bounceCompleted = false;
                    BallGraphics.transform.localScale = Vector3.one * 1.2f;
                    BallHitSound.Play();
                    ballResetGameTime = Time.timeSinceLevelLoad;
                    //startingPosition = transform.position.magnitude;
                    //print($"Distance to Cover {}");
                    m_TrackingObject = tMovement;
                }
            }

            //if (collision.gameObject.TryGetComponent(out TennisMovement racket))
            //{
            //    if (m_TrackingObject != racket)
            //    {
            //        m_TrackingObject = racket;
            //        float x = Mathf.Clamp(hitFactor(transform.position, racket.transform.position, collision.collider.bounds.size.x), -0.65f, 0.65f);
            //        m_Rigidbody2d.velocity = new Vector2(x, racket.transform.position.normalized.y * -1f);
            //        BallHitSound.Play();
            //    }
            //}

            //float hitFactor(Vector2 ballPos, Vector2 racketPos, float racketWidth)
            //{
            //    return (ballPos.x - racketPos.x) / racketWidth;
            //}
        }


        private const float Speed = 8f;
        private const float MaxSpeed = 14f;
        private const float MinTime = 5f;
        private const float MaxTime = 6f;
        private float ballResetGameTime;

        private float BallSpeed()
        {
            var time = Time.timeSinceLevelLoad - ballResetGameTime;
            return time switch
            {
                < MinTime => Speed,
                >= MaxTime => MaxSpeed,
                _ => time.Remap(MinTime, MaxTime, Speed, MaxSpeed)
            };
        }


        public void Reset(Vector3 resetPos)
        {
            StartCoroutine(ResetRoutine());

            IEnumerator ResetRoutine()
            {
                transform.position = Vector3.one * 999f;
                m_Rigidbody2d.velocity = Vector2.zero;
                m_Rigidbody2d.angularVelocity = 0f;
                m_Collider.enabled = false;
                BallGraphics.SetActive(false);

                ballResetGameTime = Time.timeSinceLevelLoad;
                m_TrackingObject = null;

                transform.position = resetPos;
                yield return new WaitForSeconds(0.4f);

                BallGraphics.transform.localScale = Vector3.one;
                BallGraphics.SetActive(true);

                yield return new WaitForSeconds(0.1f);
                m_Collider.enabled = true;

            }
        }

    }

   
}



