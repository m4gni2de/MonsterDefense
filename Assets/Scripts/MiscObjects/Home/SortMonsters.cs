using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum SortMode
{
    Index,
    Level,
    Name,
    Attack,
    Defense,
    Speed,
    Evasion,
    Rank,
    Cost,
    Stamina,
    Precision,
    EnGen,
    CoinGen,
    KOCount,
    Critical,
}

public enum SortOrder
{
    Ascending,
    Descending,
}

public class SortMonsters : MonoBehaviour
{
    //list of all of the monsters converted to a MonsterSort object
    public List<MonsterSort> toSort = new List<MonsterSort>();
    //how the monsters will be sorted
    public SortMode sortMode;
    public SortOrder sortOrder;

    public YourHome infoPanel;

    //values to be used to switch between ascending and descending order
    public int greaterThanValue, lessThanValue;
    public Button ascendBtn, descendBtn;
    public TMP_Text sortModeText;

    // Start is called before the first frame update
    void Start()
    {
        greaterThanValue = 1;
        lessThanValue = -1;
    }

    // Update is called once per frame
    void Update()
    {

    }


    //call this to sort the monsters
    public void Sort(SortMode m)
    {

        var monsters = GameManager.Instance.GetComponent<YourMonsters>().yourMonstersComplete;

        sortMode = m;
        

        toSort.Clear();


        //convert each of your monsters to a MonsterSort, so they can be sorted based on their parameters
        foreach (KeyValuePair<int, Monster> monster in monsters)
        {
            MonsterSort sort = new MonsterSort();
            sort.monster = monster.Value;
            toSort.Add(sort);
            PlayerPrefs.SetString("SortMode", sortMode.ToString());

            //if (toSort.Count >= monsters.Count)
            //{
            //    SortMethod();
            //    GameManager.Instance.GetComponent<YourMonsters>().GetYourMonsters();
            //    infoPanel.LoadMonsters();
            //    return;
            //}
           

        }


        SortMethod();
        GameManager.Instance.GetComponent<YourMonsters>().GetYourMonsters();
        infoPanel.LoadMonsters();
    }


    public void SortMethod()
    {
        int n = 0;
        //once each monster has become a MonsterSort, then actually do the sorting

        
        foreach (MonsterSort sort in toSort)
        {
            if (sortMode == SortMode.Level)
            {
                //compare each monster's level, with x and y being the monster's compared
                toSort.Sort(delegate (MonsterSort x, MonsterSort y)
                {

                    if (x.monster.info.level == y.monster.info.level) return 0;
                    else if (x.monster.info.level < y.monster.info.level) return greaterThanValue;
                    else if (x.monster.info.level > y.monster.info.level) return lessThanValue;
                    else return x.monster.info.level.CompareTo(y.monster.info.level);
                });
            }

            else if (sortMode == SortMode.Attack)
            {

                //compare each monster's level, with x and y being the monster's compared
                toSort.Sort(delegate (MonsterSort x, MonsterSort y)
                {
                    
                    if (x.monster.info.Attack.Value == y.monster.info.Attack.Value) return 0;
                    else if (x.monster.info.Attack.Value < y.monster.info.Attack.Value) return greaterThanValue;
                    else if (x.monster.info.Attack.Value > y.monster.info.Attack.Value) return lessThanValue;
                    else return x.monster.info.Attack.Value.CompareTo(y.monster.info.Attack.Value);
                });
            }

            else if (sortMode == SortMode.Defense)
            {

                //compare each monster's level, with x and y being the monster's compared
                toSort.Sort(delegate (MonsterSort x, MonsterSort y)
                {

                    if (x.monster.info.Defense.Value == y.monster.info.Defense.Value) return 0;
                    else if (x.monster.info.Defense.Value < y.monster.info.Defense.Value) return greaterThanValue;
                    else if (x.monster.info.Defense.Value > y.monster.info.Defense.Value) return lessThanValue;
                    else return x.monster.info.Defense.Value.CompareTo(y.monster.info.Defense.Value);
                });
            }

            else if (sortMode == SortMode.Precision)
            {

                //compare each monster's level, with x and y being the monster's compared
                toSort.Sort(delegate (MonsterSort x, MonsterSort y)
                {

                    if (x.monster.info.Precision.Value == y.monster.info.Precision.Value) return 0;
                    else if (x.monster.info.Precision.Value < y.monster.info.Precision.Value) return greaterThanValue;
                    else if (x.monster.info.Precision.Value > y.monster.info.Precision.Value) return lessThanValue;
                    else return x.monster.info.Precision.Value.CompareTo(y.monster.info.Precision.Value);
                });
            }
            else if (sortMode == SortMode.Speed)
            {

                //compare each monster's level, with x and y being the monster's compared
                toSort.Sort(delegate (MonsterSort x, MonsterSort y)
                {

                    if (x.monster.info.Speed.Value == y.monster.info.Speed.Value) return 0;
                    else if (x.monster.info.Speed.Value < y.monster.info.Speed.Value) return greaterThanValue;
                    else if (x.monster.info.Speed.Value > y.monster.info.Speed.Value) return lessThanValue;
                    else return x.monster.info.Speed.Value.CompareTo(y.monster.info.Speed.Value);
                });
            }
            else if (sortMode == SortMode.Rank)
            {

                //compare each monster's level, with x and y being the monster's compared
                toSort.Sort(delegate (MonsterSort x, MonsterSort y)
                {

                    if (x.monster.info.monsterRank == y.monster.info.monsterRank) return 0;
                    else if (x.monster.info.monsterRank < y.monster.info.monsterRank) return greaterThanValue;
                    else if (x.monster.info.monsterRank > y.monster.info.monsterRank) return lessThanValue;
                    else return x.monster.info.monsterRank.CompareTo(y.monster.info.monsterRank);
                });
            }
            else if (sortMode == SortMode.Evasion)
            {

                //compare each monster's level, with x and y being the monster's compared
                toSort.Sort(delegate (MonsterSort x, MonsterSort y)
                {

                    if (x.monster.info.evasionBase == y.monster.info.evasionBase) return 0;
                    else if (x.monster.info.evasionBase < y.monster.info.evasionBase) return greaterThanValue;
                    else if (x.monster.info.evasionBase > y.monster.info.evasionBase) return lessThanValue;
                    else return x.monster.info.evasionBase.CompareTo(y.monster.info.evasionBase);
                });
            }
            else if (sortMode == SortMode.CoinGen)
            {

                //compare each monster's level, with x and y being the monster's compared
                toSort.Sort(delegate (MonsterSort x, MonsterSort y)
                {

                    if (x.monster.info.CoinGeneration.Value == y.monster.info.CoinGeneration.Value) return 0;
                    else if (x.monster.info.CoinGeneration.Value < y.monster.info.CoinGeneration.Value) return greaterThanValue;
                    else if (x.monster.info.CoinGeneration.Value > y.monster.info.CoinGeneration.Value) return lessThanValue;
                    else return x.monster.info.CoinGeneration.Value.CompareTo(y.monster.info.CoinGeneration.Value);
                });
            }
            else if (sortMode == SortMode.Cost)
            {

                //compare each monster's level, with x and y being the monster's compared
                toSort.Sort(delegate (MonsterSort x, MonsterSort y)
                {

                    if (x.monster.info.EnergyCost.Value == y.monster.info.EnergyCost.Value) return 0;
                    else if (x.monster.info.EnergyCost.Value < y.monster.info.EnergyCost.Value) return greaterThanValue;
                    else if (x.monster.info.EnergyCost.Value > y.monster.info.EnergyCost.Value) return lessThanValue;
                    else return x.monster.info.EnergyCost.Value.CompareTo(y.monster.info.EnergyCost.Value);
                });
            }
            else if (sortMode == SortMode.EnGen)
            {

                //compare each monster's level, with x and y being the monster's compared
                toSort.Sort(delegate (MonsterSort x, MonsterSort y)
                {

                    if (x.monster.info.EnergyGeneration.Value == y.monster.info.EnergyGeneration.Value) return 0;
                    else if (x.monster.info.EnergyGeneration.Value < y.monster.info.EnergyGeneration.Value) return greaterThanValue;
                    else if (x.monster.info.EnergyGeneration.Value > y.monster.info.EnergyGeneration.Value) return lessThanValue;
                    else return x.monster.info.EnergyGeneration.Value.CompareTo(y.monster.info.EnergyGeneration.Value);
                });
            }
            else if (sortMode == SortMode.Name)
            {

                //compare each monster's level, with x and y being the monster's compared
                toSort.Sort(delegate (MonsterSort x, MonsterSort y)
                {
                    return x.SortByNameAscending(x.monster.info.name, y.monster.info.name);
                });
            }
            else if (sortMode == SortMode.KOCount)
            {

                //compare each monster's level, with x and y being the monster's compared
                toSort.Sort(delegate (MonsterSort x, MonsterSort y)
                {
                    if (x.monster.info.koCount == y.monster.info.koCount) return 0;
                    else if (x.monster.info.koCount < y.monster.info.koCount) return greaterThanValue;
                    else if (x.monster.info.koCount > y.monster.info.koCount) return lessThanValue;
                    else return x.monster.info.koCount.CompareTo(y.monster.info.koCount);
                });
            }
            else if (sortMode == SortMode.Stamina)
            {

                //compare each monster's level, with x and y being the monster's compared
                toSort.Sort(delegate (MonsterSort x, MonsterSort y)
                {
                    if (x.monster.info.critBase == y.monster.info.critBase) return 0;
                    else if (x.monster.info.critBase < y.monster.info.critBase) return greaterThanValue;
                    else if (x.monster.info.critBase > y.monster.info.critBase) return lessThanValue;
                    else return x.monster.info.critBase.CompareTo(y.monster.info.critBase);
                });
            }
            else if (sortMode == SortMode.Critical)
            {

                //compare each monster's level, with x and y being the monster's compared
                toSort.Sort(delegate (MonsterSort x, MonsterSort y)
                {
                    if (x.monster.info.Stamina.Value == y.monster.info.Stamina.Value) return 0;
                    else if (x.monster.info.Stamina.Value < y.monster.info.Stamina.Value) return greaterThanValue;
                    else if (x.monster.info.Stamina.Value > y.monster.info.Stamina.Value) return lessThanValue;
                    else return x.monster.info.Stamina.Value.CompareTo(y.monster.info.Stamina.Value);
                });
            }


            toSort[n].monster.info.index = n + 1;
            toSort[n].monster.SaveMonsterToken();
            
            n += 1;


            }
    }







    /////***************BUTTONS FOR SORTING*********************************///
    
    public void LevelSort()
    {
        for (int i = 0; i < 2; i++)
        {

            Sort(SortMode.Level);
        }

        gameObject.SetActive(false);
    }

    public void AttackSort()
    {
        for (int i = 0; i < 2; i++)
        {

            Sort(SortMode.Attack);
        }

        gameObject.SetActive(false);
    }

    public void DefenseSort()
    {
        for (int i = 0; i < 2; i++)
        {

            Sort(SortMode.Defense);
        }

        gameObject.SetActive(false);
    }

    public void PrecisionSort()
    {
        for (int i = 0; i < 2; i++)
        {

            Sort(SortMode.Precision);
        }

        gameObject.SetActive(false);
    }

    public void SpeedSort()
    {
        for (int i = 0; i < 2; i++)
        {

            Sort(SortMode.Speed);
        }

        gameObject.SetActive(false);
    }

    public void RankSort()
    {
        for (int i = 0; i < 2; i++)
        {

            Sort(SortMode.Rank);
        }

        gameObject.SetActive(false);
    }

    public void EvasionSort()
    {
        for (int i = 0; i < 2; i++)
        {

            Sort(SortMode.Evasion);
        }

        gameObject.SetActive(false);
    }

    public void CoinGenSort()
    {
        for (int i = 0; i < 2; i++)
        {

            Sort(SortMode.CoinGen);
        }

        gameObject.SetActive(false);
    }

    public void CostSort()
    {
        for (int i = 0; i < 2; i++)
        {

            Sort(SortMode.Cost);
        }

        gameObject.SetActive(false);
    }

    public void EnGenSort()
    {
        for (int i = 0; i < 2; i++)
        {

            Sort(SortMode.EnGen);
        }

        gameObject.SetActive(false);
    }

    public void NameSort()
    {
        for (int i = 0; i < 2; i++)
        {

            Sort(SortMode.Name);
        }

        gameObject.SetActive(false);
    }

    public void KOCountSort()
    {
        for (int i = 0; i < 2; i++)
        {

            Sort(SortMode.KOCount);
        }

        gameObject.SetActive(false);
    }

    public void StaminaSort()
    {
        for (int i = 0; i < 2; i++)
        {

            Sort(SortMode.Stamina);
        }

        gameObject.SetActive(false);
    }

    public void CriticalSort()
    {
        for (int i = 0; i < 2; i++)
        {

            Sort(SortMode.Critical);
        }

        gameObject.SetActive(false);
    }



    //button to sort monsters in ascending order
    public void AscendingOrder()
    {
        greaterThanValue = 1;
        lessThanValue = -1;

        ascendBtn.interactable = false;
        descendBtn.interactable = true;

        sortOrder = SortOrder.Ascending;
    }

    //button to sort monsters in ascending order
    public void DescendingOrder()
    {
        greaterThanValue = -1;
        lessThanValue = 1;

        ascendBtn.interactable = true;
        descendBtn.interactable = false;

        sortOrder = SortOrder.Descending;
    }

}

public class MonsterSort : IEquatable<MonsterSort>, IComparable<MonsterSort>
{
    public Monster monster;

    public override string ToString()
    {
        return monster.name;
    }
    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        MonsterSort objAsPart = obj as MonsterSort;
        if (objAsPart == null) return false;
        else return Equals(objAsPart);
    }
    public int SortByNameAscending(string name1, string name2)
    {

        return name1.CompareTo(name2);
    }

    // Default comparer for Part type.
    public int CompareTo(MonsterSort comparePart)
    {
       
        // A null value means that this object is greater.
        if (comparePart == null)
            return 1;

        else
            return 0;
    }
    public override int GetHashCode()
    {
        return monster.GetHashCode();
    }
    public bool Equals(MonsterSort other)
    {
        if (other == null) return false;
        return true;
    }
    // Should also override == and != operators.
}
