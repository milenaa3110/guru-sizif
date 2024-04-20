using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : Consumable
{
    public override string GetConsumableName()
    {
        return "Pegazovo krilo";
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
        if (c.trackManager.speed != c.trackManager.maxSpeed)
        {
            c.trackManager.speed += 1;
        }

    }

    public override void Ended(CharacterInputController c)
    {
        base.Ended(c);

        if (c.trackManager.speed != c.trackManager.minSpeed)
        {
            c.trackManager.speed -= 1;
        }
    }
}
