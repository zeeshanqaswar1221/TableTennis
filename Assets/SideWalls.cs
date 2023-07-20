using UnityEngine;

public class SideWalls : MonoBehaviour
{
    [SerializeField] private Transform resetBallPosition;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.gameObject.TryGetComponent<Ball>(out var ball)) return;
        
        ball.SetToDefaultPos(resetBallPosition.position);
    }

}
