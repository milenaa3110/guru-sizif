using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : Consumable
{

    protected float speed = 0;
    public override string GetConsumableName()
    {
        return "SpeedUp";
    }

    public override ConsumableType GetConsumableType()
    {
        return ConsumableType.SPEED_UP;
    }

    public override int GetPrice()
    {
        return 750;
    }

    public override int GetPremiumCost()
    {
        return 0;
    }

    public override IEnumerator Started(CharacterInputController c)
    {
        yield return base.Started(c);
        this.speed = c.trackManager.speed;
        c.trackManager.speed = c.trackManager.maxSpeed;
        
    }

    public override void Ended(CharacterInputController c)
    {
        base.Ended(c);

        c.trackManager.speed = this.speed;
    }
}
