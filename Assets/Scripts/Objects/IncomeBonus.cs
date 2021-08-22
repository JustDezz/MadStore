using UnityEngine;

public class IncomeBonus : Bonus
{
    [Min(0)] [SerializeField] private int incomeBonus;
    protected override void Start()
    {
        base.Start();
    }
    public override void Activate()
    {
        Player.Instance.AddBonus(duration, incomeBonus);
        SoundManager.Instance.PlaySound("IncomeBonus");
        UIManager.Instance.ActivateBonusLabel(bonusLabel.GetComponent<BonusLabel>(), duration);
        SpawnManager.Instance.RemoveObject(this.gameObject);
        Destroy(this.gameObject);
    }
}
