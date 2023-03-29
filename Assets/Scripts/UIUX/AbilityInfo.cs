using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Ability", menuName ="Scriptable Objects/Ability Info")]
public class AbilityInfo : ScriptableObject{
    //Ability name used on card
    public string abilityName;
    //Ability name used in internal data (i.e. inside player)
    public string internalName;
    //Cost of the ability in points
    public int pointCost;
    //Displayed on the ability card itself
    public string shortDesc;
    //How many of this ability should the shop start with?
    public int startingStock;
    //Displayed on the ability's "info" page
    [TextAreaAttribute] 
    public string longDesc;
    //image which appears on the ability card
    public Sprite spr;
}
