using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class DefencePlan
{
    public float Position {get;}
    public float Cost {get;}

    public float Impact {get; set;}

    public int CartNum {get; set;}

    public ICollection<ShieldActivation> ShieldActivations {get;}

    public DefencePlan(float position, float cost)
    {
        this.Position = position;
        this.Cost = cost;
        this.ShieldActivations = new ReadOnlyCollection<ShieldActivation>(new List<ShieldActivation>());
    }

    public DefencePlan(float position, float cost, List<ShieldActivation> shieldActivations)
    {
        this.Position = position;
        this.Cost = cost;
        this.ShieldActivations = new ReadOnlyCollection<ShieldActivation>(shieldActivations);
    }

    public float GetScore(float impactWeigth, float costWeigth)
    {
        return Mathf.Pow(impactWeigth * Impact, 2) + Mathf.Pow(costWeigth * Cost, 2);
    }

    public void ReduceShieldDelay(float reduce)
    {
        foreach (var sa in ShieldActivations)
        {
            sa.ReduceDelay(reduce);
        }
    }

    public override string ToString()
    {
        string shieldActivationsString = "";
        foreach (var activation in ShieldActivations)
        {
            shieldActivationsString += "[" + activation.ToString() + "]";
        }
        return "Position: " + Position + "; Cost: " + Cost + "; Impact: " + Impact + "; CartNum " + CartNum + "; Shields " + shieldActivationsString;
    }

    public class ShieldActivation
    {
        public AIArmorCart Cart {get;}
        public float Delay {get; private set;}

        public bool Activated {get; private set;}

        public ShieldActivation(AIArmorCart cart, float delay)
        {
            this.Cart = cart;
            this.Delay = delay;
            this.Activated = false;
        }

        public void ReduceDelay(float reduce)
        {
            Delay -= reduce;
        }

        public override string ToString()
        {
            return "Cart " + Cart.carriagePrefab.name + "; delay " + Delay;
        }

        public void Activate()
        {
            this.Activated = true;
        }
    }
}
