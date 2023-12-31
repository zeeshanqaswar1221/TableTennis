using System.Collections;
using Fusion;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tennis.Orthographic
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BallController : NetworkBehaviour,IReset
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] SpriteRenderer _spriteRenderer;
        private const float Speed = 10f;
        private const float MaxSpeed = 12f;
        private const float MinTime = 5f;
        private const float MaxTime = 6f;
        
        private Vector2 _cachedVelocity;
        private float ballResetGameTime;
        private bool ballStarted = false;

        public override void Spawned()
        {
            base.Spawned();
            _rb ??= GetComponent<Rigidbody2D>();
        }
        
        public void ResetBall(float startDelay = 1f)
        {
            _spriteRenderer.enabled = false;
            _cachedVelocity = _rb.velocity;
            
            _rb.velocity = Vector2.zero;
            transform.position = Vector3.zero;
            _spriteRenderer.enabled = true;

            //if (startDelay > 0f)
            //    StartCoroutine(StartBall(startDelay));
        }


        public void ResetBall(Vector3 newPos)
        {
            _spriteRenderer.enabled = false;
            _cachedVelocity = _rb.velocity;

            _rb.velocity = Vector2.zero;
            transform.position = newPos;
        }

        public void HideBall()
        {
            _rb.velocity = Vector2.zero;
            transform.position = new Vector3(99999f,99999f);
            _spriteRenderer.enabled = false;
        }

        public override void FixedUpdateNetwork()
        {
            if (_rb.velocity == Vector2.zero) return;
            
            var vel = _rb.velocity.normalized;
            if (Mathf.Abs(vel.x) < 0.25f)
                vel.x = 0.25f * vel.x < 0 ? -1f : 1f;

            _rb.velocity = vel * BallSpeed();
        }

        public IEnumerator StartBall(float delay)
        {
            yield return new WaitForEndOfFrame();
            _spriteRenderer.enabled = true;
            yield return new WaitForSecondsRealtime(delay);

            ballResetGameTime = Time.timeSinceLevelLoad;
            
            if (_cachedVelocity == Vector2.zero)
            {
                var yVel = Random.Range(.2f, .8f);
                if (Random.value > 0.5f)
                    yVel *= -1f;

                _cachedVelocity = new Vector2(0f, yVel);
            }

            var xVel = Random.Range(.1f, .9f);
            if (Random.value > 0.5f)
                xVel *= -1f;

            var startVel = new Vector2(xVel, _cachedVelocity.y);

            _rb.velocity = startVel.normalized * BallSpeed();
        }

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

        #region CUSTOM FUNCTION

        TennisMovement previousRacket = default;
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<TennisMovement>(out TennisMovement newRacket))
            {
                if (previousRacket != newRacket)
                {
                    if (!ballStarted)
                    {
                        ballStarted = true;

                        ballResetGameTime = Time.timeSinceLevelLoad;

                        if (_cachedVelocity == Vector2.zero)
                        {
                            var yVel = Random.Range(.2f, .8f);
                            if (Random.value > 0.5f)
                                yVel *= -1f;

                            _cachedVelocity = new Vector2(0f, yVel);
                        }

                        var xVel = Random.Range(.1f, .9f);
                        if (Random.value > 0.5f)
                            xVel *= -1f;

                        var startVel = new Vector2(xVel, _cachedVelocity.y);

                        _rb.velocity = startVel.normalized * BallSpeed();
                    }
                }

            }
        }

        public void Reset(Vector3 resetPos)
        {
            ballStarted = false;
            HideBall();
            ResetBall(resetPos);
            _spriteRenderer.enabled = true;
        }
        #endregion

    }
}

public static class FloatExtensions
{
    public static float Remap(this float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }

    //format a float (in seconds) to a readable string
    public static string FormatTime(this float timeInSeconds, bool lessDecimals = false)
    {
        var minutes = (int)timeInSeconds / 60;
        var seconds = (int)timeInSeconds - 60 * minutes;
        var milliseconds = (int)(1000 * (timeInSeconds - minutes * 60 - seconds));

        if (lessDecimals)
            milliseconds /= 100;

        return $"{minutes:0}:{seconds:00}.{milliseconds:0}";
    }
}