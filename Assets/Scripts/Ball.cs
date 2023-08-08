using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening.Core;
using Fusion;

namespace Tennis.Orthographic
{
    public class Ball : NetworkBehaviour, IReset
    {
        public LayerMask collisionLayer;
        float defaultSpeed = 10f;

        private Rigidbody2D m_Rigidbody2d;

        public AudioSource BallHitSound { get; set; }

        public override void Spawned()
        {
            m_Rigidbody2d = GetComponent<Rigidbody2D>();
            BallHitSound = GetComponent<AudioSource>();
            m_TrackingObject = default;
        }

        public override void FixedUpdateNetwork()
        {
            //float speedMultiplier = m_Rigidbody2d.velocity.magnitude / 50f;
            //Vector2 curentVel = m_Rigidbody2d.velocity.normalized;
            //curentVel.y = Mathf.Abs(curentVel.y);
            //m_Rigidbody2d.velocity = curentVel * 10f;
            m_Rigidbody2d.velocity = m_Rigidbody2d.velocity.normalized * BallSpeed();
        }


        private TennisMovement m_TrackingObject;
        private float startingPosition;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out TennisMovement tMovement))
            {
                if (m_TrackingObject == tMovement)
                {
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


        private const float Speed = 10f;
        private const float MaxSpeed = 12f;
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
            ballResetGameTime = Time.timeSinceLevelLoad;
            m_Rigidbody2d.angularVelocity= 0f;
            m_Rigidbody2d.velocity = Vector2.zero;
            m_TrackingObject = null;
            transform.position = resetPos;
        }

    }

   
}



