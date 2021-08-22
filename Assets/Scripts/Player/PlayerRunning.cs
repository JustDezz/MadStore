using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunning : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> speedParticles;
    private float playerSpeed;
    public float Speed { get { return playerSpeed * Player.Instance.SpeedBonus; } }
    public float PlayerMaxSpeed { get { return Player.Instance.PlayerMaxSpeed; } }
    public float PlayerMinSpeed { get { return Player.Instance.PlayerMinSpeed; } }
    public float TimeToMaxSpeed { get { return Player.Instance.TimeToMaxSpeed; } }
    public float SpeedBonus { get { return Player.Instance.SpeedBonus; } }

    private void Start()
    {
        playerSpeed = Player.Instance.PlayerMinSpeed;
        DOTween.To(x => playerSpeed = x, Player.Instance.PlayerMinSpeed,
            Player.Instance.PlayerMaxSpeed, Player.Instance.TimeToMaxSpeed).SetEase(Ease.InOutSine);
        TimeScaleToggler.OnTimeScaled += OnTimeScaled;
    }
    private void FixedUpdate()
    {
        if (SpeedBonus > 1)
        {
            foreach (ParticleSystem particle in speedParticles)
            {
#pragma warning disable CS0618 // Тип или член устарел
                particle.emissionRate = Mathf.Lerp(100, 250, Speed / PlayerMaxSpeed / 2);
#pragma warning restore CS0618 // Тип или член устарел
            }
        }
        else
        {
            foreach (ParticleSystem particle in speedParticles)
            {
#pragma warning disable CS0618 // Тип или член устарел
                particle.emissionRate = 0;
#pragma warning restore CS0618 // Тип или член устарел
            }
        }
        transform.position += Vector3.forward * Time.fixedDeltaTime * playerSpeed * Player.Instance.SpeedBonus;
    }
    private void OnTimeScaled(bool isNormal)
    {
        if (isNormal)
        {
            SoundManager.Instance.PlaySound("PlayerRun");
        }
        else
        {
            SoundManager.Instance.StopSound("PlayerRun");
        }
    }
    private void OnDestroy()
    {
        SoundManager.Instance.StopSound("PlayerRun");
    }
}
