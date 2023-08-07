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
        //public TennisGraphics ballGraphics;
        public LayerMask collisionLayer;


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
            Vector2 curentVel = m_Rigidbody2d.velocity.normalized;
            //curentVel.y = Mathf.Abs(curentVel.y);
            m_Rigidbody2d.velocity = curentVel * 10f;
        }


        private TennisMovement m_TrackingObject;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out TennisMovement tMovement))
            {
                if (m_TrackingObject == tMovement)
                {
                    BallHitSound.Play();
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


        public void Reset(Vector3 resetPos)
        {
            m_Rigidbody2d.angularVelocity= 0f;
            m_Rigidbody2d.velocity = Vector2.zero;
            m_TrackingObject = null;
            transform.position = resetPos;
        }

    }

   
}



