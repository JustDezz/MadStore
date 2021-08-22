using DG.Tweening;
using UnityEngine;

public class PlayerMovementPC : MonoBehaviour
{
    private Vector2 touchStartPosition;
    private float touchStartTime;
    private bool isOnLeft = true;
    private bool canTurn = true;
    [Min(0)] [SerializeField] private float turnTime = 0.2f;

    public void OnMouseDown()
    {
        touchStartPosition = Input.mousePosition;
        touchStartTime = Time.time;
    }

    public void OnMouseUp()
    {
        if (canTurn)
        {
            Vector2 direction = Input.mousePosition - (Vector3)touchStartPosition;
            if (direction.magnitude < 100 || Time.time - touchStartTime > 1f)
            {
                return;
            }
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            if (angle > 30 && angle < 150 && isOnLeft)
            {
                canTurn = false;
                this.transform.DOLocalMoveX(1.5f, turnTime).OnComplete(() => canTurn = true);
                isOnLeft = false;
            }
            else if (angle < -30 && angle > -150 && !isOnLeft)
            {
                canTurn = false;
                this.transform.DOLocalMoveX(-1.5f, turnTime).OnComplete(() => canTurn = true);
                isOnLeft = true;
            }
        }
    }
}
