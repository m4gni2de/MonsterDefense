using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//make a new equipable for each buff/nerf that a monster will have, including abilities. Different slots will correspond to different things. 1 is Equip One, 2 is Equip 2. Eventually, this class will be remade to accomodate Abilities and other items
public class EquippableItem
{
    

    public void Equip(Monster monster, int slot)
    {
        

        var equipDict = GameManager.Instance.items.allEquipmentDict;

       

        if (slot == 1)
        {
            if (equipDict.ContainsKey(monster.info.equip1Name))
            {
                monster.info.equip1 = equipDict[monster.info.equip1Name];
                Equipment equip = monster.info.equip1;



                if (equip.hpBonus != 0)
                    monster.info.HP.AddModifier(new StatModifier(equip.hpBonus, StatModType.Flat, this));
                if (equip.atkBonus != 0)
                    monster.info.Attack.AddModifier(new StatModifier(equip.atkBonus, StatModType.Flat, this));
                if (equip.defBonus != 0)
                    monster.info.Defense.AddModifier(new StatModifier(equip.defBonus, StatModType.Flat, this));
                if (equip.speedBonus != 0)
                    monster.info.Speed.AddModifier(new StatModifier(equip.speedBonus, StatModType.Flat, this));

                if (equip.hpPercentBonus != 0)
                    monster.info.HP.AddModifier(new StatModifier(equip.hpPercentBonus, StatModType.PercentMult, this));
                if (equip.atkPercentBonus != 0)
                    monster.info.Attack.AddModifier(new StatModifier(equip.atkPercentBonus, StatModType.PercentMult, this));
                if (equip.defPercentBonus != 0)
                    monster.info.Defense.AddModifier(new StatModifier(equip.defPercentBonus, StatModType.PercentMult, this));
                if (equip.spePercentBonus != 0)
                    monster.info.Speed.AddModifier(new StatModifier(equip.spePercentBonus, StatModType.PercentMult, this));

                //if the equipment item is Type protected, check and make sure the types match. If they do, apply bonuses
                if (equip.typeMoveReq == monster.info.attack1.type)
                {

                    if (equip.atkPowerBonus != 0)
                        monster.info.attack1.Power.AddModifier(new StatModifier(equip.atkPowerBonus, StatModType.Flat, this));
                    if (equip.atkRangeBonus != 0)
                        monster.info.attack1.Range.AddModifier(new StatModifier(equip.atkRangeBonus, StatModType.Flat, this));
                    if (equip.atkTimeBonus != 0)
                        monster.info.attack1.AttackTime.AddModifier(new StatModifier(equip.atkTimeBonus, StatModType.Flat, this));
                    if (equip.critChanceBonus != 0)
                        monster.info.attack1.CritChance.AddModifier(new StatModifier(equip.critChanceBonus, StatModType.Flat, this));
                    if (equip.critModBonus != 0)
                        monster.info.attack1.CritMod.AddModifier(new StatModifier(equip.critModBonus, StatModType.Flat, this));

                    if (equip.atkPowerPercentBonus != 0)
                        monster.info.attack1.Power.AddModifier(new StatModifier(equip.atkPowerPercentBonus, StatModType.PercentMult, this));
                    if (equip.atkTimePercentBonus != 0)
                        monster.info.attack1.AttackTime.AddModifier(new StatModifier(equip.atkTimePercentBonus, StatModType.PercentMult, this));


                }

                if (equip.typeMoveReq == monster.info.attack2.type)
                {
                    if (equip.atkPowerBonus != 0)
                        monster.info.attack2.Power.AddModifier(new StatModifier(equip.atkPowerBonus, StatModType.Flat, this));
                    if (equip.atkRangeBonus != 0)
                        monster.info.attack2.Range.AddModifier(new StatModifier(equip.atkRangeBonus, StatModType.Flat, this));
                    if (equip.atkTimeBonus != 0)
                        monster.info.attack2.AttackTime.AddModifier(new StatModifier(equip.atkTimeBonus, StatModType.Flat, this));
                    if (equip.critChanceBonus != 0)
                        monster.info.attack2.CritChance.AddModifier(new StatModifier(equip.critChanceBonus, StatModType.Flat, this));
                    if (equip.critModBonus != 0)
                        monster.info.attack2.CritMod.AddModifier(new StatModifier(equip.critModBonus, StatModType.Flat, this));

                    if (equip.atkPowerPercentBonus != 0)
                        monster.info.attack2.Power.AddModifier(new StatModifier(equip.atkPowerPercentBonus, StatModType.PercentMult, this));
                    if (equip.atkTimePercentBonus != 0)
                        monster.info.attack2.AttackTime.AddModifier(new StatModifier(equip.atkTimePercentBonus, StatModType.PercentMult, this));

                }

            }
        }

        if (slot == 2)
        {
            if (equipDict.ContainsKey(monster.info.equip2Name))
            {
                monster.info.equip2 = equipDict[monster.info.equip2Name];
                Equipment equip = monster.info.equip2;





                if (equip.hpBonus != 0)
                    monster.info.HP.AddModifier(new StatModifier(equip.hpBonus, StatModType.Flat, this));
                if (equip.atkBonus != 0)
                    monster.info.Attack.AddModifier(new StatModifier(equip.atkBonus, StatModType.Flat, this));
                if (equip.defBonus != 0)
                    monster.info.Defense.AddModifier(new StatModifier(equip.defBonus, StatModType.Flat, this));
                if (equip.speedBonus != 0)
                    monster.info.Speed.AddModifier(new StatModifier(equip.speedBonus, StatModType.Flat, this));

                if (equip.hpPercentBonus != 0)
                    monster.info.HP.AddModifier(new StatModifier(equip.hpPercentBonus, StatModType.PercentMult, this));
                if (equip.atkPercentBonus != 0)
                    monster.info.Attack.AddModifier(new StatModifier(equip.atkPercentBonus, StatModType.PercentMult, this));
                if (equip.defPercentBonus != 0)
                    monster.info.Defense.AddModifier(new StatModifier(equip.defPercentBonus, StatModType.PercentMult, this));
                if (equip.spePercentBonus != 0)
                    monster.info.Speed.AddModifier(new StatModifier(equip.spePercentBonus, StatModType.PercentMult, this));

                //if the equipment item is Type protected, check and make sure the types match. If they do, apply bonuses
                if (equip.typeMoveReq == monster.info.attack1.type)
                {

                    if (equip.atkPowerBonus != 0)
                        monster.info.attack1.Power.AddModifier(new StatModifier(equip.atkPowerBonus, StatModType.Flat, this));
                    if (equip.atkRangeBonus != 0)
                        monster.info.attack1.Range.AddModifier(new StatModifier(equip.atkRangeBonus, StatModType.Flat, this));
                    if (equip.atkTimeBonus != 0)
                        monster.info.attack1.AttackTime.AddModifier(new StatModifier(equip.atkTimeBonus, StatModType.Flat, this));
                    if (equip.critChanceBonus != 0)
                        monster.info.attack1.CritChance.AddModifier(new StatModifier(equip.critChanceBonus, StatModType.Flat, this));
                    if (equip.critModBonus != 0)
                        monster.info.attack1.CritMod.AddModifier(new StatModifier(equip.critModBonus, StatModType.Flat, this));

                    if (equip.atkPowerPercentBonus != 0)
                        monster.info.attack1.Power.AddModifier(new StatModifier(equip.atkPowerPercentBonus, StatModType.PercentMult, this));
                    if (equip.atkTimePercentBonus != 0)
                        monster.info.attack1.AttackTime.AddModifier(new StatModifier(equip.atkTimePercentBonus, StatModType.PercentMult, this));


                }

                if (equip.typeMoveReq == monster.info.attack2.type)
                {
                    if (equip.atkPowerBonus != 0)
                        monster.info.attack2.Power.AddModifier(new StatModifier(equip.atkPowerBonus, StatModType.Flat, this));
                    if (equip.atkRangeBonus != 0)
                        monster.info.attack2.Range.AddModifier(new StatModifier(equip.atkRangeBonus, StatModType.Flat, this));
                    if (equip.atkTimeBonus != 0)
                        monster.info.attack2.AttackTime.AddModifier(new StatModifier(equip.atkTimeBonus, StatModType.Flat, this));
                    if (equip.critChanceBonus != 0)
                        monster.info.attack2.CritChance.AddModifier(new StatModifier(equip.critChanceBonus, StatModType.Flat, this));
                    if (equip.critModBonus != 0)
                        monster.info.attack2.CritMod.AddModifier(new StatModifier(equip.critModBonus, StatModType.Flat, this));

                    if (equip.atkPowerPercentBonus != 0)
                        monster.info.attack2.Power.AddModifier(new StatModifier(equip.atkPowerPercentBonus, StatModType.PercentMult, this));
                    if (equip.atkTimePercentBonus != 0)
                        monster.info.attack2.AttackTime.AddModifier(new StatModifier(equip.atkTimePercentBonus, StatModType.PercentMult, this));

                }

            }
        }
    }
           

       
       

    //invoke this to remove all of the stat boosts when unequipping an equipment item
    public void Unequip(Monster monster)
    {
        monster.info.HP.RemoveAllModifiersFromSource(this);
        monster.info.Attack.RemoveAllModifiersFromSource(this);
        monster.info.Defense.RemoveAllModifiersFromSource(this);
        monster.info.Speed.RemoveAllModifiersFromSource(this);
        monster.info.Precision.RemoveAllModifiersFromSource(this);

        monster.info.attack1.Power.RemoveAllModifiersFromSource(this);
        monster.info.attack1.Range.RemoveAllModifiersFromSource(this);
        monster.info.attack1.AttackSpeed.RemoveAllModifiersFromSource(this);
        monster.info.attack1.AttackTime.RemoveAllModifiersFromSource(this);
        monster.info.attack1.CritChance.RemoveAllModifiersFromSource(this);
        monster.info.attack1.CritMod.RemoveAllModifiersFromSource(this);
        monster.info.attack1.EffectChance.RemoveAllModifiersFromSource(this);

        monster.info.attack2.Power.RemoveAllModifiersFromSource(this);
        monster.info.attack2.Range.RemoveAllModifiersFromSource(this);
        monster.info.attack2.AttackSpeed.RemoveAllModifiersFromSource(this);
        monster.info.attack2.AttackTime.RemoveAllModifiersFromSource(this);
        monster.info.attack2.CritChance.RemoveAllModifiersFromSource(this);
        monster.info.attack2.CritMod.RemoveAllModifiersFromSource(this);
        monster.info.attack2.EffectChance.RemoveAllModifiersFromSource(this);


    }
}



