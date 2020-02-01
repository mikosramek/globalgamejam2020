using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBehaviour : MonoBehaviour
{

    public Camera mainCam;
    public float cameraRadius = 10;
    public List<GameObject> cameraIndicators;
    public CameraColliderLogic cameraCollider;

    
    void Start()
    {
        if(cameraIndicators != null && cameraIndicators.Count == 4)
        {
            cameraIndicators[0].transform.localPosition = new Vector2(-cameraRadius, -cameraRadius);
            cameraIndicators[1].transform.localPosition = new Vector2(cameraRadius, -cameraRadius);
            cameraIndicators[2].transform.localPosition = new Vector2(cameraRadius, cameraRadius);
            cameraIndicators[3].transform.localPosition = new Vector2(-cameraRadius, cameraRadius);
        }
        if(cameraCollider != null) { cameraCollider.gameObject.transform.localScale = new Vector2(cameraRadius*2, cameraRadius*2); }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mosPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mosPos;

        if (Input.GetButtonDown("Fire1"))
        {
            CatBehaviour currentCat = cameraCollider.getCurrentCat().GetComponent<CatBehaviour>();
            currentCat.pictureTaken();
        }
    }
}
