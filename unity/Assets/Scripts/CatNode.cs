using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatNode : MonoBehaviour
{
    private bool isSat = false;
    [HideInInspector]
    public GameObject floorPoint;
    public float scaleFactor;
    private void Awake()
    {
        floorPoint = transform.GetChild(0).gameObject;
    }
    public bool canSit()
    {
        return !isSat;
    }
    public void getSatOn()
    {
        isSat = true;
    }
    public void getLeft()
    {
        isSat = false;
    }
}
