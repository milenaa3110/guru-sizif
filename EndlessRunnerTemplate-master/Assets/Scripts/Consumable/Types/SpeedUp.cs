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
    private float currentSpeed;

    public override IEnumerator Started(CharacterInputController c)
    {
        yield return base.Started(c);
        currentSpeed = c.trackManager.speed;
        c.trackManager.speed +=2;

    }

    public override void Ended(CharacterInputController c)
    {
        base.Ended(c);

        c.trackManager.speed = currentSpeed;
    }
}
