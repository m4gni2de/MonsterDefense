using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnEnemy()
    {
        var random = Random.Range(1, GameManager.Instance.monstersData.monstersByIdDict.Count + 1);
        var byId = GameManager.Instance.monstersData.monstersByIdDict;
        var monsters = GameManager.Instance.monstersData.monstersAllDict;
        

        //picks a random number. then translates that number to the Monsters by Id Dictionary. Then takes that number, and summons a prefab based on the name of the matching key
        if (byId.ContainsKey(random))
        {
            string species = byId[random];

            if (monsters.ContainsKey(species))
            {

                var enemyMonster = Instantiate(monsters[species].monsterPrefab, transform.position, Quaternion.identity);
                enemyMonster.transform.position = spawnPoint.transform.position;
                enemyMonster.GetComponent<Monster>().isEnemy = true;
                enemyMonster.gameObject.tag = "Enemy";
                enemyMonster.gameObject.name = "Enemy " + enemyMonster.GetComponent<Monster>().info.species;
                enemyMonster.transform.localScale = new Vector3(1.8f, 1.8f, 1.0f);
            }
        }


        //var enemyMonster = Instantiate(enemy, transform.position, Quaternion.identity);
        ////x.transform.SetParent(GetComponentInParent<MapTemplate>().gameObject.transform, false);
        //enemyMonster.transform.position = spawnPoint.transform.position;
        //enemyMonster.GetComponent<Monster>().isEnemy = true;
        //enemyMonster.gameObject.tag = "Enemy";
        //enemyMonster.gameObject.name = "Enemy " + enemyMonster.GetComponent<Monster>().info.species;
        ////x.transform.localScale = new Vector2(x.transform.localScale.x * .85f, x.transform.localScale.y * .85f);
    }



 
}
