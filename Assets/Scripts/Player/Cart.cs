using UnityEngine;

public class Cart : MonoBehaviour
{
    private float tillNextIncome;
    private bool hasIncome = false;
    private Shelving currentShelving;

    [SerializeField] private PlayerRunning runner;
    private CartStacker cartStacker;

    private void Start()
    {
        tillNextIncome = 0;
        cartStacker = this.gameObject.GetComponent<CartStacker>();
    }
    private void FixedUpdate()
    {
        if (hasIncome)
        {
            if (tillNextIncome <= 0)
            {
                if (runner.Speed < runner.PlayerMinSpeed)
                {
                    tillNextIncome += Player.Instance.IncomeTimeRate;
                }
                else
                {
                    tillNextIncome += Player.Instance.IncomeTimeRate / (runner.Speed / Player.Instance.PlayerMaxSpeed);
                }
                Player.Instance.Income(currentShelving.onIcome(this.gameObject));
            }
            else
            {
                tillNextIncome -= Time.fixedDeltaTime;
            }
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Shelving":
                {
                    hasIncome = true;
                    currentShelving = other.gameObject.GetComponent<Shelving>();
                    break;
                }
            case "Security":
                {
                    if (!Player.Instance.Immortal)
                    {
                        Player.Instance.Crash();
                    }
                    other.GetComponent<Security>().Die();
                    break;
                }
            case "Customer":
                {
                    Customer customer = other.GetComponent<Customer>();
                    if (customer.HasCart)
                    {
                        Player.Instance.AddBonus(customer.BonusDuration, customer.BonusIncome, customer.BonusSpeed);
                        cartStacker.StackCart(customer.Cart, customer.BonusDuration);
                    }
                    Player.Instance.Income("Customer", customer.BonusScore);
                    customer.Die();
                    break;
                }
            case "Bonus":
                {
                    other.GetComponent<Bonus>().Activate();
                    break;
                }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Shelving"))
        {
            hasIncome = false;
            currentShelving = null;
        }
    }
}
