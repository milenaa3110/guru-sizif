using UnityEngine;
using System;
using System.Collections;

public class ExtraLife : Consumable
{
    protected const int k_MaxLives = 3;
    protected const int k_CoinValue = 10;

    public override string GetConsumableName()
    {
        return "Ambrozija";
    }

    public override ConsumableType GetConsumableType()
    {
        return ConsumableType.EXTRALIFE;
    }

    public override int GetPrice()
    {
        return 2000;
    }

	public override int GetPremiumCost()
	{
		return 5;
	}

    public override bool CanBeUsed(CharacterInputController c)
    {
        if (c.currentLife == c.maxLife)
            return false;

        return true;
    }

    public override IEnumerator Started(CharacterInputController c)
    {
        Debug.Log("ExtraLife.Started");
        yield return base.Started(c);
        if (this == null)
        {
            Debug.Log("ExtraLife object has been destroyed");
            yield break;
        }
        Debug.Log($"ExtraLife.Started: {c.currentLife}");
        if (c.currentLife < k_MaxLives)
        {
            c.currentLife += 1;
            Debug.Log($"ExtraLife.Started: {c.currentLife}");
        }
        else
            c.coins += k_CoinValue;
    }
}
