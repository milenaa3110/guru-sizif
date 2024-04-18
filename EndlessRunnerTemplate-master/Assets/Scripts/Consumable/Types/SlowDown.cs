using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDown : Consumable
{
    public override string GetConsumableName()
    {
        return "SlowDown";
    }
    
    protected float speed = 0;
    
    public override ConsumableType GetConsumableType()
    {
        return ConsumableType.SLOW_DOWN;
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
        c.trackManager.speed= c.trackManager.minSpeed;
        
    }

    public override void Ended(CharacterInputController c)
    {
        base.Ended(c);
        c.trackManager.speed = this.speed;
        
    }
}
