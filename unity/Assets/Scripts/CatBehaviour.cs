using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBehaviour : MonoBehaviour
{
    public GameObject[] catNodes;

    public enum mood {
        sitting,
        moving,
        bored
    }

    public CatNode currentNode;

    public mood currentMood = mood.moving;


    public Animator animator;

    void Start()
    {
        catNodes = GameObject.FindGameObjectsWithTag("CatNode");
        moveToNewSpot();
    }
    void reshuffleNodes()
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < catNodes.Length; t++)
        {
            GameObject tmp = catNodes[t];
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
            CatNode tempNode = catNodes[i].GetComponent<CatNode>();
            Debug.Log(tempNode);
            Debug.Log(tempNode.canSit());
            if (tempNode.canSit())
            {
                toMoveTo = tempNode;
                Debug.Log("found spot");
                break;
            }
        }
        if (toMoveTo != null) {
            if (currentNode != null) { currentNode.getLeft(); }
            currentNode = toMoveTo;
            StartCoroutine(moveToSpot(currentNode));
            currentNode.getSatOn();
        }
        else { actBored(); };
    }

    IEnumerator moveToSpot(CatNode node)
    {
        animator.SetTrigger("run");
        while (transform.position != node.gameObject.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, node.gameObject.transform.position, 0.1f);
            if((transform.position - node.gameObject.transform.position).magnitude <= 0.1f)
            {
                transform.position = node.gameObject.transform.position;
            }
            yield return new WaitForEndOfFrame();
        }
        animator.SetTrigger("chill");
        currentMood = mood.sitting;
    }
    void actBored()
    {
        currentMood = mood.bored;
        //play animation
    }
    public void pictureTaken()
    {
        switch (currentMood)
        {
            case mood.bored:
                moveToNewSpot();
                break;
            case mood.sitting:
                moveToNewSpot();
                break;
            case mood.moving:
                break;
        }
    }
    //moveToNewRoom
}
