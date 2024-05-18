using System;
using System.Collections;
using UnityEngine;

public class SlowDown : Consumable
{
    public override string GetConsumableName()
    {
        return "Paunova frula";
    }

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

    private float currentSpeed = -1f;

    public override IEnumerator Started(CharacterInputController c)
    {
        yield return base.Started(c);
        currentSpeed = c.trackManager.speed;
        c.trackManager.speed -= 2;

    }

    public override void Ended(CharacterInputController c)
    {
        base.Ended(c);

        c.trackManager.speed = currentSpeed;
    }
}
