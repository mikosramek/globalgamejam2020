using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Instagram : MonoBehaviour
{
    public Transform canvas;
    public GameObject PhotoPrefab;
    public GameObject catPrefab;

    public TextMeshProUGUI followCount;
    public TextMeshProUGUI postCountText;

    public RectTransform scrollViewContent;
    int catCount;
    int followerCount;
    private void Start()
    {
        catCount = 0;
        followerCount = 23;
    }

    public float rerange(float OldValue, float OldMin, float OldMax, float NewMin, float NewMax)
    {
        return (((OldValue - OldMin) * (NewMax - NewMin)) / (OldMax - OldMin)) + NewMin;
    }

    public void CreateAPhoto(List<CatBehaviour> cats, Vector2 cameraPos, Sprite roomSprite, Vector2 roomPos)
    {
        
        GameObject newPhoto = Instantiate(PhotoPrefab, Vector3.zero, Quaternion.identity);
        newPhoto.transform.SetParent(canvas);
        GameObject bg = newPhoto.transform.GetChild(1).transform.GetChild(0).gameObject;
        bg.GetComponent<RectTransform>().localPosition = new Vector3(-rerange(cameraPos.x - roomPos.x, -8, 8, -290, 290), -rerange(cameraPos.y - roomPos.y, -5, 5, -189, 189));
        bg.GetComponent<Image>().sprite = roomSprite;
        int likes = 0;
        foreach (var catB in cats)
        {
            GameObject cat = Instantiate(catPrefab, newPhoto.transform.GetChild(1));
            cat.GetComponent<Image>().sprite = catB.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

            cat.transform.GetChild(0).GetComponent<Image>().sprite = catB.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
            cat.transform.GetChild(0).gameObject.SetActive(catB.transform.GetChild(0).transform.GetChild(0).gameObject.activeSelf);
            Vector2 toyPos = catB.transform.GetChild(0).transform.GetChild(0).transform.localPosition;
            float range = 23;
            cat.transform.GetChild(0).localPosition = new Vector2(rerange(toyPos.x, -3, 3, -range, range), rerange(toyPos.y, -3, 3, -range, range));

            RectTransform catRect = cat.GetComponent<RectTransform>();
            catRect.localScale = catB.gameObject.transform.localScale;
            Vector2 catPos = catB.gameObject.transform.position;
            Vector3 catDif = cameraPos - catPos;
            Debug.Log(catDif);
            range = 2.5f;
            Debug.Log(-rerange(catDif.x, -range , range, -115, 115));
            Debug.Log(-rerange(catDif.y, -range, range, -115, 115));
            catRect.localPosition = new Vector3(-rerange(catDif.x, -range, range, -115, 115), -rerange(catDif.y, -range, range, -115, 115));
            
            switch (catB.currentMood)
            {
                case mood.bored:
                    likes += Random.Range(5000, 12938);
                    followerCount += Random.Range(10, 30);
                    break;
                case mood.moving:
                    likes += Random.Range(0, 50);
                    cat.GetComponent<Image>().sprite = catB.blurryCat;
                    followerCount += Random.Range(-10, 5);
                    break;
                case mood.sitting:
                    likes += Random.Range(300, 2000);
                    followerCount += Random.Range(0, 3);
                    break;
                case mood.changingRoom:
                    likes += Random.Range(50, 70);
                    cat.GetComponent<Image>().sprite = catB.blurryCat;
                    followerCount += Random.Range(2, 5);
                    break;
            }
        }
        newPhoto.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = ""+likes;
        RectTransform photo = newPhoto.GetComponent<RectTransform>();
        photo.localScale = Vector2.one;
        photo.anchoredPosition = new Vector2(-290 + 200 * (catCount % 4), -100 - 220 * Mathf.Floor(catCount/4));
        scrollViewContent.sizeDelta = new Vector2(scrollViewContent.sizeDelta.x, 225 + 225 * Mathf.Floor(catCount / 4));
        catCount++;
        followCount.text = "" + followerCount;
        postCountText.text = "" + catCount;
    }
}


