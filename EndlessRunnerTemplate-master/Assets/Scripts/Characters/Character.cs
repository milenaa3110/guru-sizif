using UnityEngine;
using System;

public class Character : MonoBehaviour
{
    public string characterName;
    public int cost;
	public int premiumCost;
	public GameObject truck;
    public GameObject stone;
	public CharacterAccessories[] accessories;

    public Animator animator;
	public Sprite icon;

	[Header("Sound")]
	public AudioClip jumpSound;
	public AudioClip hitSound;
	public AudioClip deathSound;

    public void SetupAccesory(int accessory)
    {
        for (int i = 0; i < accessories.Length; ++i)
        {
            accessories[i].gameObject.SetActive(i == PlayerData.instance.usedAccessory);
        }
        truck.gameObject.SetActive(false);
        stone.gameObject.SetActive(false);
    }
}
