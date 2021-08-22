using System.Collections;
using UnityEngine;

public class MegaBonus : Bonus
{
    [Range(0, 10)] [SerializeField] private float speedBonus;
    [Min(0)] [SerializeField] private int bonusScore;

    protected override void Start()
    {
        base.Start();
    }
    public override void Activate()
    {
        Player.Instance.Income("Mega Bonus", bonusScore);
        SoundManager.Instance.PlaySound("MegaBonus");
        UIManager.Instance.ActivateBonusLabel(bonusLabel.GetComponent<BonusLabel>(), duration);
        SpawnManager.Instance.RemoveObject(this.gameObject);
        Destroy(this.gameObject.GetComponentInChildren<MeshRenderer>());
        StartCoroutine(ActivateMegaBonus());
    }
    private IEnumerator ActivateMegaBonus()
    {
        float speedBonus = Player.Instance.SpeedBonus;
        Player.Instance.AddBonus(duration * 0.1f, Player.Instance.IncomeBonus, -speedBonus + 0.1f);
        Player.Instance.Immortality(duration + 0.5f);
        yield return new WaitForSeconds(duration * 0.1f);
        Player.Instance.AddBonus(duration * 0.9f, Player.Instance.IncomeBonus, speedBonus + this.speedBonus);
        yield return new WaitForSeconds(duration * 0.9f);
        Destroy(this.gameObject);
    }
}
