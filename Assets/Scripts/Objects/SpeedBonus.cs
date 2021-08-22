using UnityEngine;

public class SpeedBonus : Bonus
{
    [Range(0, 1)] [SerializeField] private float speedBonus;
    protected override void Start()
    {
        base.Start();
    }
    public override void Activate()
    {
        Player.Instance.AddBonus(duration, speedBonus: speedBonus);
        SoundManager.Instance.PlaySound("SpeedBonus");
        UIManager.Instance.ActivateBonusLabel(bonusLabel.GetComponent<BonusLabel>(), duration);
        SpawnManager.Instance.RemoveObject(this.gameObject);
        Destroy(this.gameObject);
    }
}
