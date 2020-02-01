using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBehaviour : MonoBehaviour
{

    public Camera mainCam;
    public float cameraRadius = 10;
    public List<GameObject> cameraIndicators;
    public CameraColliderLogic cameraCollider;

    public RoomNode[] roomNodes;
    public int currentRoom = 0;

    public Instagram insta;

    public AnimationCurve movementCurve;


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
    
    void Update()
    {
        Vector2 mosPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mosPos;

        if (Input.GetButtonDown("Fire1"))
        {
            GameObject cat = cameraCollider.getCurrentCat();
            if(cat != null)
            {
                CatBehaviour currentCat = cat.GetComponent<CatBehaviour>();
                takePhoto(currentCat);
                currentCat.pictureTaken();
            }
        }
    }

    public void ChangeRoom(int direction)
    {
        if(currentRoom + direction != -1 && currentRoom + direction < roomNodes.Length)
        {
            currentRoom += direction;
            StartCoroutine(moveCamera());
        }
    }
    IEnumerator moveCamera()
    {
        Vector3 target = roomNodes[currentRoom].transform.position;
        target.z = mainCam.transform.position.z;
        Vector3 start = mainCam.transform.position;
        bool isLerping = true;
        float t = 0;
        float duration = 1;
        while (isLerping)
        {
            t += Time.deltaTime;
            float s = t / duration;
            mainCam.transform.position = Vector3.Lerp(start, target, movementCurve.Evaluate(s));
            if (s >= 1.0f)
            {
                isLerping = false;
            }
            yield return new WaitForEndOfFrame();
        }       
    }
    void takePhoto(CatBehaviour cat)
    {
        //cat scale -> facing direction
        float catDir = cat.gameObject.transform.localScale.x;
        //cat position relative to 0 of room
        Vector2 catPos = cat.gameObject.transform.position;
        //their sprite from the child
        Sprite catSprite = cat.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        //camera position relative to 0 of room
        Vector2 cameraPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        //which room
        Sprite roomSprite = roomNodes[currentRoom].roomSprite;
        //cat mood
        mood catMood = cat.currentMood;
        //Hand off the data
        insta.CreateAPhoto(catDir, catPos, catSprite, cameraPos, roomSprite, catMood, cat.blurryCat);
    }
}
