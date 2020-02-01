using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Instagram : MonoBehaviour
{
    public Transform canvas;
    public GameObject PhotoPrefab;
    int catCount;
    private void Start()
    {
        catCount = 0;
    }

    public float rerange(float OldValue, float OldMin, float OldMax, float NewMin, float NewMax)
    {
        return (((OldValue - OldMin) * (NewMax - NewMin)) / (OldMax - OldMin)) + NewMin;
    }

    public void CreateAPhoto(float catDir, Vector2 catPos, Sprite catSprite, Vector2 cameraPos, Sprite roomSprite, mood catMood, Sprite blurryCat)
    {
        GameObject newPhoto = Instantiate(PhotoPrefab, Vector3.zero, Quaternion.identity);
        newPhoto.transform.SetParent(canvas);
        GameObject bg = newPhoto.transform.GetChild(1).transform.GetChild(0).gameObject;
        GameObject cat = newPhoto.transform.GetChild(1).transform.GetChild(1).gameObject;

        cat.GetComponent<Image>().sprite = catSprite;
        RectTransform catRect = cat.GetComponent<RectTransform>();
        catRect.localScale = new Vector3(catRect.localScale.x * catDir, catRect.localScale.y);
        Vector3 catDif = cameraPos - catPos;
        catRect.localPosition = new Vector3(-rerange(catDif.x, -5, 5, -115, 115), -rerange(catDif.y, -5, 5, -115, 115));
        bg.GetComponent<RectTransform>().localPosition = new Vector3(-rerange(cameraPos.x, -9, 9, -420, 420), -rerange(cameraPos.y, -5, 5, -205, 205));
        bg.GetComponent<Image>().sprite = roomSprite;
        int likes = 0;
        switch (catMood)
        {
            case mood.bored:
                likes = Random.Range(5000, 12938);
                break;
            case mood.moving:
                likes = Random.Range(0, 50);
                cat.GetComponent<Image>().sprite = blurryCat;
                break;
            case mood.sitting:
                likes = Random.Range(300, 2000);
                break;
        }
        newPhoto.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = ""+likes;

        catCount++;
    }
}


