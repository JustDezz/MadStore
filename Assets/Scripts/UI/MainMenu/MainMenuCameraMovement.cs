using PathCreation;
using UnityEngine;

public class MainMenuCameraMovement : MonoBehaviour
{
    [Min(0)] [SerializeField] private float speed = 5f;
    [SerializeField] private PathCreator path;
    [SerializeField] private Transform targetToLook;
    private float distanceTraveled = 0;

    private void Update()
    {
        distanceTraveled += speed * Time.deltaTime;
        transform.position = path.path.GetPointAtDistance(distanceTraveled);
        transform.LookAt(targetToLook);
    }
}
