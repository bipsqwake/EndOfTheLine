using UnityEngine;

public class TrainPart : MonoBehaviour
{
    [SerializeField] private SpriteRenderer cartView;

    public Train train { get; set; }

    public SpriteRenderer GetCartView()
    {
        return cartView;
    }

}
