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

        private TennisMovement m_TrackingObject;

        private float m_MaxSpeed = 14f;
        private float m_MinSpeed = 14f;
        private Vector2 m_BallDirection;
        private float m_BoundDistance = 2.1f;

        public TennisMovement Pedal1 { get; set; }
        public TennisMovement Pedal2 { get; set; }
        public AudioSource BallHitSound { get; set; }


        public override void Spawned()
        {
            BallHitSound = GetComponent<AudioSource>();
            m_TrackingObject = default;
        }

        public override void FixedUpdateNetwork()
        {
            if (m_BallDirection != Vector2.zero)
            {
                transform.Translate(m_BallDirection * Runner.DeltaTime);
            }

            CheckCollision();
        }

        private float CalculateBallSpeed()
        {
            return m_TrackingObject.SwipeSpeed.Remap(0,1f, m_MinSpeed, m_MaxSpeed);
        }

        public void CheckCollision()
        {
            bool touchingPedal1 = Pedal1 == null ? false : Vector3.Distance(Pedal1.GraphicsPos, transform.position) < m_BoundDistance;
            bool touchingPedal2 = Pedal2 == null? false: Vector3.Distance(Pedal2.GraphicsPos, transform.position) < m_BoundDistance;
            float x = 0f;

            if (!touchingPedal1 && !touchingPedal2)
                return;

            if (touchingPedal1)
            {
                print("Ball Collided with Master Pedal");
                if (m_TrackingObject == Pedal1)
                    return;
                
                m_TrackingObject = Pedal1;
                x = hitFactor(transform.position, Pedal1.GraphicsPos, Pedal1.PedalWidth);
            }

            if (touchingPedal2)
            {
                print("Ball Collided with Master Pedal");
                if (m_TrackingObject == Pedal2)
                    return;

                m_TrackingObject = Pedal2;
                x = hitFactor(transform.position, Pedal2.GraphicsPos, Pedal2.PedalWidth);
            }

            if (m_TrackingObject != null)
            {
                x = Mathf.Clamp(x, -0.65f, 0.65f);
                //print($"{m_TrackingObject.SwipeSpeed} {CalculateBallSpeed()}");
                m_BallDirection = new Vector2(x, -m_TrackingObject.ForwardDir).normalized * CalculateBallSpeed();
                BallHitSound.Play();
            }
        }

        float hitFactor(Vector2 ballPos, Vector2 racketPos, float racketWidth)
        {
            return (ballPos.x - racketPos.x) / racketWidth;
        }

        public void Reset(Vector3 resetPos)
        {
            m_TrackingObject = null;
            m_BallDirection = Vector2.zero;
            m_TrackingObject = default;
            transform.position = resetPos;
        }

    }

    public static class ExtensionMethods
    {

        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

    }
}



