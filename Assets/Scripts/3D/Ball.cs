using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Tennis.Perspective
{
    public class Ball : NetworkBehaviour
    {

        public float maxVelocity;

        private Rigidbody m_Rigidbody;
        public TennisMovement currentPedal;

        public override void Spawned()
        {
            m_Rigidbody = GetComponent<Rigidbody>(); 
            
            TestingPush();
        }


        public override void FixedUpdateNetwork()
        {
            if (!Object.HasStateAuthority)
                return;

            if (currentPedal != null)
            {
                m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, maxVelocity);
            }
        }

        public void SetVelocity(TennisMovement pedal)
        {
            if (currentPedal == pedal)
                return;

            currentPedal = pedal;
            m_Rigidbody.velocity = Vector2.zero;

            float x = hitFactor(transform.position, pedal.transform.position, pedal.GetComponent<Collider>().bounds.size.x);
            x = Mathf.Clamp(x, -0.5f, 0.5f);

            float y = pedal.forwardDir * -1; // Reverse it
            Vector3 m_BallDirection = new Vector3(x, y, 0).normalized;

            m_Rigidbody.velocity = m_BallDirection * maxVelocity;
        }

        float hitFactor(Vector3 ballPos, Vector3 racketPos, float racketWidth)
        {
            return (ballPos.x - racketPos.x) / racketWidth;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Bounds"))
            {
                currentPedal = null;
                m_Rigidbody.velocity = Vector3.zero;
                transform.position = FusionManager.Instance.ClientPedal.ResetPoint.localPosition;

                TestingPush();
            }
        }

        
        private void TestingPush()
        {
            StartCoroutine(DelayExe());
            
            IEnumerator DelayExe()
            {
                yield return new WaitForSeconds(1);
                currentPedal = FusionManager.Instance.ClientPedal;
                m_Rigidbody.velocity = Vector3.up * currentPedal.forwardDir * maxVelocity;
            }
        }

    }
}
