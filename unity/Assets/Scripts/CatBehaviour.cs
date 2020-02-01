using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum mood
{
    sitting,
    moving,
    changingRoom,
    bored
}
public class CatBehaviour : MonoBehaviour
{
    private CatNode[] catNodes;
    private CatNode currentNode;

    public int currentRoom;
    public RoomNode[] rooms;

    public mood currentMood = mood.moving;


    public Animator animator;
    public Sprite blurryCat;

    private float movementTimer;
    private float movementTimeTarget = 100000;
    public Vector2 movementTimeRange;
    public Vector2 boredTimeRange;


    void Start()
    {
        catNodes = rooms[currentRoom].nodes;
        moveToNewSpot();
    }

    private void Update()
    {
        if (currentMood == mood.sitting)
        {
            movementTimer += 1;
            if (movementTimer >= movementTimeTarget)
            {
                float choice = Random.Range(0, 100);
                //40% chance to move
                if (choice >= 0 && choice < 40)
                {
                    moveToNewSpot();       
                }
                //20% chance to move to a new Room
                else if(choice >= 40 && choice < 60)
                {
                    chooseANewRoom();
                }
                //30% chance to act bored
                else if (choice >= 60 && choice < 90)
                {
                    actBored(); 
                }
                //20% chance to stay asleep
                else
                {
                    resetTimer();
                }
            }
        }
    }
    void reshuffleNodes()
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < catNodes.Length; t++)
        {
            CatNode tmp = catNodes[t];
            int r = Random.Range(t, catNodes.Length);
            catNodes[t] = catNodes[r];
            catNodes[r] = tmp;
        }
    }
    void moveToNewSpot()
    {
        currentMood = mood.moving;
        CatNode toMoveTo = null;
        reshuffleNodes();
        for (int i = 0; i < catNodes.Length; i++)
        {
            CatNode tempNode = catNodes[i];
            if (tempNode.canSit())
            {
                toMoveTo = tempNode;
                break;
            }
        }
        if (toMoveTo != null) {
            if (currentNode != null) { currentNode.getLeft(); }
            currentNode = toMoveTo;
            StopAllCoroutines();
            StartCoroutine(moveToSpot(currentNode));
            currentNode.getSatOn();
            resetTimer();
        }
    }
    void resetTimer()
    {
        movementTimer = 0;
        //roughly between 5 to 10 seconds
        movementTimeTarget = Random.Range(60 * movementTimeRange.x, 60 * movementTimeRange.y);
    }
    void flipSprite(Vector2 dist, float factor)
    {
        if (dist.x < 0)
        {
            transform.localScale = new Vector3(-factor, factor, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(factor, factor, transform.localScale.z);
        }
    }
    IEnumerator moveToSpot(CatNode node)
    {
        
        animator.ResetTrigger("chill");
        animator.SetTrigger("run");
        float scale = Mathf.Abs(transform.localScale.x);
        while (transform.position != node.floorPoint.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, node.floorPoint.transform.position, 0.1f);
            Vector2 dist = transform.position - node.floorPoint.transform.position;
            scale = Mathf.MoveTowards(scale, 1, 0.01f);
            flipSprite(dist, scale);
            if (dist.magnitude <= 0.1f)
            {
                transform.position = node.floorPoint.transform.position;
                animator.ResetTrigger("chill");
                animator.SetTrigger("jump");
            }
            yield return new WaitForEndOfFrame();
        }
        scale = Mathf.Abs(transform.localScale.x);
        while (transform.position != node.gameObject.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, node.gameObject.transform.position, 0.1f);
            scale = Mathf.MoveTowards(scale, node.scaleFactor, 0.01f);
            Vector2 dist = transform.position - node.gameObject.transform.position;
            flipSprite(dist, scale);
            if (dist.magnitude <= 0.1f)
            {
                transform.position = node.gameObject.transform.position;
                animator.SetTrigger("chill");
            }
            yield return new WaitForEndOfFrame();
        }
        currentMood = mood.sitting;
    }
    void actBored()
    {
        currentMood = mood.bored;
        StopAllCoroutines();
        StartCoroutine(beBored());
    }
    IEnumerator beBored()
    {
        animator.ResetTrigger("chill");
        animator.SetTrigger("bored");
        yield return new WaitForSeconds(Random.Range(boredTimeRange.x, boredTimeRange.y));
        animator.SetTrigger("chill");
        currentMood = mood.sitting;
    }
    public void pictureTaken()
    {
        switch (currentMood)
        {
            case mood.bored:
                chooseANewRoom();
                break;
            case mood.sitting:
                moveToNewSpot();
                break;
            case mood.moving:
                break;
        }
    }
    //moveToNewRoom
    public void chooseANewRoom()
    {
        currentMood = mood.changingRoom;
        int nextRoomIndex = currentRoom + 1;
        if (nextRoomIndex < rooms.Length && currentRoom != 0)
        {
            float choice = Random.Range(0, 1f);
            Debug.Log(choice);
            if (choice < 0.5f)
            {
                nextRoomIndex = currentRoom - 1;               
            }
        }else if(currentRoom == 0)
        {
            nextRoomIndex = currentRoom + 1;
        }else if(nextRoomIndex >= rooms.Length)
        {
            nextRoomIndex = currentRoom - 1;
        }
        StopAllCoroutines();
        StartCoroutine(moveToNewRoom(rooms[currentRoom], rooms[nextRoomIndex], nextRoomIndex - currentRoom));
    }
    IEnumerator moveToNewRoom(RoomNode currentRoom, RoomNode nextRoom, int dir)
    {
        // + is right
        // - is left
        Debug.Log(dir);
        Transform target = currentRoom.movementNodeLeft.transform;
        if(dir < 0)
        {
             target = currentRoom.movementNodeLeft.transform;
        }
        else
        {
            target = currentRoom.movementNodeRight.transform;
        }
        animator.ResetTrigger("chill");
        animator.SetTrigger("run");
        yield return moveToNode(target);
        animator.SetTrigger("jump");
        
        if (dir < 0)
        {
            target = nextRoom.movementNodeRight.transform;
        }
        else
        {
            target = nextRoom.movementNodeLeft.transform;
        }
        
        yield return moveToNode(target);
        this.currentRoom += dir;
        catNodes = rooms[this.currentRoom].nodes;
        animator.SetTrigger("chill");
        yield return new WaitForSeconds(0.5f);
        moveToNewSpot();
    }
    IEnumerator moveToNode(Transform target)
    {
        Debug.Log("moving to" + target);
        while (transform.position != target.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, 0.1f);
            Vector2 dist = transform.position - target.position;
            flipSprite(dist, 1);
            if (Mathf.Abs(dist.magnitude) <= 0.1f)
            {
                Debug.Log("done moving to" + target);
                transform.position = target.position;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
