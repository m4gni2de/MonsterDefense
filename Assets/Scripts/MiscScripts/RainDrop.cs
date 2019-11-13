using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainDrop : MonoBehaviour
{
    public Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator RainDropAnimation()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[i];

            yield return new WaitForSeconds(.005f);

            if (i >= sprites.Length - 1)
            {
                Destroy(gameObject);
            }
        }
    }
}
