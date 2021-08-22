using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	private Camera mainCamera;
	[SerializeField] private PlayerRunning runner;

	[Range(0, 1)] [SerializeField] private float adjustmentCoefficient = 0.1f;

	[Header("Max speed")]
	[SerializeField] private float maxSpeedFOV;
	[SerializeField] private float maxSpeedY;
	[SerializeField] private float maxSpeedZ;
	[SerializeField] private float maxSpeedRotationX;

	[Header("Min speed")]
	[SerializeField] private float minSpeedFOV;
	[SerializeField] private float minSpeedY;
	[SerializeField] private float minSpeedZ;
	[SerializeField] private float minSpeedRotationX;

	private float lerpCoefficient;

	private void Awake()
	{
		mainCamera = this.gameObject.GetComponent<Camera>();
	}

	private void FixedUpdate()
	{
		lerpCoefficient = runner.Speed / runner.PlayerMaxSpeed / 5;
		mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, Mathf.Lerp(minSpeedFOV, maxSpeedFOV, lerpCoefficient), adjustmentCoefficient);
		this.gameObject.transform.localPosition = new Vector3(0,
				Mathf.Lerp(transform.localPosition.y, Mathf.Lerp(minSpeedY, maxSpeedY, lerpCoefficient), adjustmentCoefficient),
				Mathf.Lerp(transform.localPosition.z, Mathf.Lerp(minSpeedZ, maxSpeedZ, lerpCoefficient), adjustmentCoefficient));
		this.gameObject.transform.localRotation = Quaternion.Euler(Vector3.right *
			Mathf.Lerp(transform.localRotation.eulerAngles.x, Mathf.Lerp(minSpeedRotationX, maxSpeedRotationX, lerpCoefficient), adjustmentCoefficient));
	}
}
