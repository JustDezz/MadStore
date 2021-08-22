using DG.Tweening;
using UnityEngine;

public class PlayerMovementMobile : MonoBehaviour
{
    private Vector2 touchStartPosition;
    private float touchStartTime;
    private bool isOnLeft = true;
    private bool canTurn = true;
    [Min(0)] [SerializeField] private float turnTime = 0.2f;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    {
                        touchStartPosition = touch.position;
                        touchStartTime = Time.time;
                        break;
                    }
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    {
                        if (canTurn)
                        {
                            Vector2 direction = touch.position - touchStartPosition;
                            if (direction.magnitude < 100 || Time.time - touchStartTime > 1f)
                            {
                                break;
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
                        break;
                    }
            }
        }
    }
}
