using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] private List<Carriage> carriages;
    private Locomotive locomotive;

    public void Awake()
    {
        float position = 0;
        foreach (var carriage in carriages)
        {
            Carriage instantiated = Instantiate(carriage, transform, false);
            float halfLength = instantiated.GetComponentInChildren<SpriteRenderer>().size.x / 2;
            position += halfLength;
            instantiated.transform.localPosition = Vector3.left * position;
            position += halfLength;
            if (locomotive == null && instantiated.GetComponent<Locomotive>() != null)
            {
                locomotive = instantiated.GetComponent<Locomotive>();
            }
        }
    }

    public Locomotive GetLocomotive()
    {
        return locomotive;
    }
}
