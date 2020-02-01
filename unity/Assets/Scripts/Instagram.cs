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
    public void CreateAPhoto(float catDir, Vector2 catPos, Sprite catSprite, Vector2 cameraPos, Sprite roomSprite, mood catMood, Sprite blurryCat)
    {
        GameObject newPhoto = Instantiate(PhotoPrefab, new Vector3(200 * catCount, 0), Quaternion.identity);
        newPhoto.transform.SetParent(canvas);
        GameObject bg = newPhoto.transform.GetChild(0).transform.GetChild(0).gameObject;
        GameObject cat = newPhoto.transform.GetChild(0).transform.GetChild(1).gameObject;

        cat.GetComponent<Image>().sprite = catSprite;
        cat.GetComponent<RectTransform>().localScale = new Vector3(catDir, 1);
        cat.GetComponent<RectTransform>().localPosition = (cameraPos - catPos) * -30;
        bg.GetComponent<RectTransform>().localPosition = (cameraPos) * -25;
        bg.GetComponent<Image>().sprite = roomSprite;
        int likes = 0;
        switch (catMood)
        {
            case mood.bored:
                likes = Random.Range(5000, 12938);
                break;
            case mood.moving:
                likes = Random.Range(0, 150);
                cat.GetComponent<Image>().sprite = blurryCat;
                break;
            case mood.sitting:
                likes = Random.Range(300, 2000);
                break;
        }
        newPhoto.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ""+likes;

        catCount++;
    }
}


