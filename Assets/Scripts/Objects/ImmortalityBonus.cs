public class ImmortalityBonus : Bonus
{
    protected override void Start()
    {
        base.Start();
    }
    public override void Activate()
    {
        Player.Instance.Immortality(duration);
        SoundManager.Instance.PlaySound("ImmortalityBonus");
        UIManager.Instance.ActivateBonusLabel(bonusLabel.GetComponent<BonusLabel>(), duration);
        SpawnManager.Instance.RemoveObject(this.gameObject);
        Destroy(this.gameObject);
    }
}
