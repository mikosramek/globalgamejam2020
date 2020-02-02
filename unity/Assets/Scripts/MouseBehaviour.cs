using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBehaviour : MonoBehaviour
{

    public Camera mainCam;
    public List<GameObject> cameraIndicators;
    public CameraColliderLogic cameraCollider;

    public RoomNode[] roomNodes;
    public int currentRoom = 0;

    public Instagram insta;

    public AnimationCurve movementCurve;


    public GameObject instagramOverlay, menuOverlay, rounderOverOverlay;

    public Animator cameraIndicator;

    
    
    void Update()
    {
        Vector2 mosPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mosPos;

        if (Input.GetButtonDown("Fire1") && !instagramOverlay.activeSelf && !menuOverlay.activeSelf && !rounderOverOverlay.activeSelf)
        {
            List<GameObject> cats = cameraCollider.getCurrentCat();
            if(cats.Count > 0)
            {
                List<CatBehaviour> catBehavs = new List<CatBehaviour>();
                foreach(var cat in cats)
                {
                    CatBehaviour catBehav = cat.GetComponent<CatBehaviour>();
                    catBehavs.Add(catBehav);
                }
                takePhoto(catBehavs);
                foreach (var cat in catBehavs)
                {
                    cat.pictureTaken();
                }
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
    void takePhoto(List<CatBehaviour> cats)
    {
        cameraIndicator.SetTrigger("snap");
        //camera position relative to 0 of room
        Vector2 cameraPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        //which room
        Sprite roomSprite = roomNodes[currentRoom].roomSprite;
        //cat mood
        //room pos
        Vector2 roomPos = roomNodes[currentRoom].transform.position;
        //Hand off the data
        insta.CreateAPhoto(cats, cameraPos, roomSprite, roomPos);
    }
}
