using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening.Core;
using Fusion;

namespace Tennis.Orthographic
{
    public class Ball : NetworkBehaviour
    {
        public bool transformMovement;

        public DOTweenAnimation[] animations;


        public AudioSource BallHitSound { get; set; }
        private TrailRenderer trailRenderer;

        private Rigidbody2D m_Rigidbody2d;
        private bool canSwing = true;
        private Vector2 m_BallDirection;


        private Collider2D _hitCollider;
        private Collider2D _ballCollider;

        [SerializeField] private SpriteRenderer[] ballVisuals;

        public float ballSpeed = 30f;

        [SerializeField] private TennisMovement trackingObject;

        public override void Spawned()
        {
            m_Rigidbody2d = GetComponent<Rigidbody2D>();

            _ballCollider = GetComponent<Collider2D>();
            BallHitSound = GetComponent<AudioSource>();
            trailRenderer = GetComponent<TrailRenderer>();
            trackingObject = default;
        }

        public float radius = 0.6f;

        public override void FixedUpdateNetwork()
        {
            if (!Object.HasStateAuthority)
                return;

            BallController();
        }


        private void BallController()
        {
            if (transformMovement)
            {
                transform.Translate(m_BallDirection * Runner.DeltaTime * ballSpeed);
            }
            else
            {
                //m_Rigidbody2d.MovePosition((Vector2)transform.position + m_BallDirection * Runner.DeltaTime * ballSpeed);
                if (m_Rigidbody2d.velocity.magnitude > ballSpeed)
                    m_Rigidbody2d.velocity = Vector3.ClampMagnitude(m_Rigidbody2d.velocity, ballSpeed);
            }
        }


        public void SetVelocity(TennisMovement pedal)
        {
            if (trackingObject == pedal) 
                return;

            trackingObject = pedal;

            if (!transformMovement)
            {
                m_Rigidbody2d.velocity = Vector2.zero;
            }

            float x = hitFactor(transform.position, pedal.transform.position, pedal.GetCollider2d.bounds.size.x);
            x = Mathf.Clamp(x, -0.5f, 0.5f);

            float y = pedal.ForwardDir * -1;
            m_BallDirection = new Vector2(x, y).normalized;

            if (!transformMovement)
            {
                m_Rigidbody2d.velocity = m_BallDirection * ballSpeed;
            }

            BallHitSound.Play();
        }

        float hitFactor(Vector2 ballPos, Vector2 racketPos, float racketWidth)
        {
            return (ballPos.x - racketPos.x) / racketWidth;
        }

        public void Reset(Transform standingPosition = null)
        {
            m_BallDirection = Vector2.zero;
            m_Rigidbody2d.velocity = Vector2.zero;
            trackingObject = default;

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
}



