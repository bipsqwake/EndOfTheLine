using UnityEngine;

//This class is awful, but it need to be
//(will rewrite later)
public class TrainPart : MonoBehaviour
{
    [SerializeField] private SpriteRenderer cartView;
    [SerializeField] private Locomotive locomotive;
    [SerializeField] private CarriagePayload carriage;
    [SerializeField] private Cart cart;
    [SerializeField] private CarriageType carriageType;

    public Train train { get; set; }

    public SpriteRenderer GetCartView()
    {
        return cartView;
    }

    public Actor GetActor()
    {
        return CarriageType.LOCOMOTIVE.Equals(carriageType) ? locomotive : carriage;
    }

    public Cart GetCart()
    {
        return cart;
    }

    public CarriagePayload GetCarriagePayload()
    {
        return carriage;
    }

    public bool IsCarriageDestroyed()
    {
        return CarriageType.LOCOMOTIVE.Equals(carriageType) ? locomotive.IsDestroyed() : carriage.IsDestroyed();
    }

    public void SetPlayerControl(bool playerControl)
    {
        if (carriage != null)
        {
            carriage.SetPlayerControl(playerControl);   
        }
        if (locomotive != null)
        {
            locomotive.SetPlayerControl(playerControl);
        }
        if (cart != null)
        {
            cart.SetPlayerControl(playerControl);
        }
    }

    public CarriageType GetCarriageType()
    {
        return carriageType;
    }

    public int GetDamage()
    {
        if (CarriageType.LOCOMOTIVE.Equals(carriageType))
        {
            return locomotive.GetDamage();
        }
        if (carriage != null && !carriage.IsDestroyed())
        {
            return carriage.GetDamage();
        } else if (cart != null)
        {
            return cart.GetDamage();
        }
        return 0;
    }

    public float GetHealthPercent()
    {
        if (CarriageType.LOCOMOTIVE.Equals(carriageType))
        {
            return locomotive.GetHealthPercent();
        }
        if (carriage != null && !carriage.IsDestroyed())
        {
            return carriage.GetHealthPercent();
        } else if (cart != null)
        {
            return cart.GetHealthPercent();
        }
        return 0;
    }

}
