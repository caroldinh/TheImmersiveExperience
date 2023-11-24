using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meta.WitAi;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class PosterRandomizer : MonoBehaviour
{

    public Canvas posterCanvas;
    public Material lightFont;
    public Material darkFont;
    public GameObject posterPrefab;

    private List<GameObject> posters = new List<GameObject>();
    private ArtistAsset artist;
    private string title;
    private RectTransform posterCanvasRect;
    
    // Start is called before the first frame update
    void Start()
    {
        posterCanvasRect = posterCanvas.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPosters(ArtistAsset newArtist, string newTitle)
    {
        foreach (GameObject poster in posters)
        {
           poster.DestroySafely(); 
        }
        posters.Clear();
        artist = newArtist;
        title = newTitle;
        InstantiatePoster(gameObject, true);
        for (int i = 0; i < Random.Range(5, 15); i++)
        {
            GameObject newPoster = Instantiate(posterPrefab, posterCanvas.transform);
            InstantiatePoster(newPoster, false);
            posters.Add(newPoster);
        }
    }

    public void InstantiatePoster(GameObject newPoster, bool isMainPoster = true)
    {
        TextMeshProUGUI posterTitle = newPoster.GetComponentInChildren<TextMeshProUGUI>();
        Image posterBackground = newPoster.GetComponentInChildren<Image>();
        RectTransform parentRect = posterBackground.transform.parent.GetComponent<RectTransform>();

        if (!isMainPoster)
        {
            //parentRect.sizeDelta = new Vector2(Random.Range(5, 10), Random.Range(5, 10));
            //parentRect.transform.position = new Vector3(Random.Range(0, 10), Random.Range(0, 10), 1);
            //parentRect.transform.rotation = new Quaternion(0, 0, Random.Range(-10, 10), 0);
            RectTransform newPosterRect = newPoster.GetComponent<RectTransform>();
            newPosterRect.localPosition = new Vector2(Random.Range(0, 10),
                Random.Range(0, 10));
            newPosterRect.sizeDelta = new Vector2(Random.Range(5, 10), Random.Range(5, 10));
        }

        else
        {
            posterBackground.sprite = artist.backgrounds[Random.Range(0, artist.backgrounds.Length)];
            float newPosterScale = 1f;
            if (posterBackground.sprite.bounds.size.x < parentRect.rect.width)
            {
                newPosterScale *= (parentRect.rect.width / posterBackground.sprite.bounds.size.x);
            }
            if (posterBackground.sprite.bounds.size.y < parentRect.rect.height)
            {
                newPosterScale *= (parentRect.rect.height / posterBackground.sprite.bounds.size.y);
            }
            posterBackground.rectTransform.sizeDelta = new Vector2(
                newPosterScale * posterBackground.sprite.bounds.size.x,
                newPosterScale * posterBackground.sprite.bounds.size.y);
        }
        posterTitle.text = title;

        Color[] SampleImagePixels(Texture2D texture)
        {
            if (texture.height > 1.5 * texture.width)
            {
                return texture.GetPixels(0, 0, texture.width, texture.height / 2);
            }
            else
            {
                return texture.GetPixels();
            }
        }

        Color[] texturePixels = SampleImagePixels(posterBackground.sprite.texture);
        
        float avgBrightness = 0;
        foreach (Color pix in  texturePixels)
        {
            avgBrightness += (pix.r + pix.b + pix.g) / 3;
        }
        avgBrightness /= (posterBackground.sprite.texture.width * posterBackground.sprite.texture.height);
        if (avgBrightness > 0.5)
        {
            posterTitle.fontMaterial = darkFont;
        }
        else
        {
            posterTitle.fontMaterial = lightFont;
        }
    }
}
