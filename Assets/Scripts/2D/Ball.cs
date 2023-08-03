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


        public LayerMask collisionLayer;
        public AudioSource BallHitSound { get; set; }
        private TrailRenderer trailRenderer;

        private NetworkRigidbody2D m_Rigidbody;
        private Vector2 m_BallDirection;


        [SerializeField] private SpriteRenderer[] ballVisuals;

        public float ballSpeed = 30f;

        [SerializeField] private TennisMovement trackingObject;

        public override void Spawned()
        {
            m_Rigidbody = GetComponent<NetworkRigidbody2D>();
            BallHitSound = GetComponent<AudioSource>();
            trailRenderer = GetComponent<TrailRenderer>();
            trackingObject = default;
        }

        public override void FixedUpdateNetwork()
        {
            m_Rigidbody.Rigidbody.velocity = Vector2.ClampMagnitude(m_Rigidbody.ReadVelocity(), ballSpeed);
            CheckCollision();
        }

        public void CheckCollision()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.6f, collisionLayer);
            if (colliders.Length > 0)
            {
                if (colliders[0].gameObject.TryGetComponent<TennisMovement>(out TennisMovement pedal))
                {
                    SetVelocity(pedal);
                }
            }
        }


        public void SetVelocity(TennisMovement pedal)
        {
            if (trackingObject == pedal) 
                return;

            trackingObject = pedal;

            float x = hitFactor(transform.position, pedal.transform.position, pedal.GetCollider2d.bounds.size.x);
            x = Mathf.Clamp(x, -0.5f, 0.5f);

            float y = pedal.ForwardDir * -1;
            m_Rigidbody.Rigidbody.velocity = new Vector2(x, y).normalized * ballSpeed;

            BallHitSound.Play();
        }

        float hitFactor(Vector2 ballPos, Vector2 racketPos, float racketWidth)
        {
            return (ballPos.x - racketPos.x) / racketWidth;
        }

        public void Reset(Transform standingPosition = null)
        {
            m_Rigidbody.Rigidbody.centerOfMass = Vector2.zero;
            m_Rigidbody.Rigidbody.velocity = Vector2.zero;
            m_Rigidbody.Rigidbody.angularVelocity = 0f;

            trackingObject = default;

            m_Rigidbody.Rigidbody.MovePosition(standingPosition.position);
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



