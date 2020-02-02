using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColliderLogic : MonoBehaviour
{
    public bool isCatInShot = false;
    public List<GameObject> currentCats;

    private void Start()
    {
        currentCats = new List<GameObject>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isCatInShot = true;
        if (!currentCats.Contains(collision.gameObject)){
            currentCats.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isCatInShot = false;
        if (currentCats.Contains(collision.gameObject))
        {
            currentCats.Remove(collision.gameObject);
        }
    }

    public List<GameObject> getCurrentCat()
    {
        return currentCats;
    }
}
