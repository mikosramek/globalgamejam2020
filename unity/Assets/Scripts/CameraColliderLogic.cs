using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColliderLogic : MonoBehaviour
{
    public bool isCatInShot = false;
    public GameObject currentCat;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isCatInShot = true;
        currentCat = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isCatInShot = false;
        currentCat = null;
    }

    public GameObject getCurrentCat()
    {
        return currentCat;
    }
}
