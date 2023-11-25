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
    public TextMeshProUGUI tagOnline;

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
        for (int i = 0; i < Random.Range(5, 10); i++)
        {
            GameObject newPoster = Instantiate(posterPrefab, posterCanvas.transform);
            InstantiatePoster(newPoster, false);
            posters.Add(newPoster);
        }

        string GenerateHandle(String title)
        {
            string[] experience =
            {
                "Immersive", "Chamber", "Exhibition", "Exhibit", "Gallery", "Experience", "Tour", "Cinema"
            };
            foreach (string word in experience)
            {
                if (title.IndexOf(word) > 0)
                {
                    return word;
                }
            }
            return "Experience";
        }
        string[] separators = { "", ".", "_", "-" };
        string ending = GenerateHandle(newTitle);
        string separator = separators[Random.Range(0, separators.Length)];
        string hashtag = newTitle.Replace(" ", "");
        if (hashtag.IndexOf(":") > 0)
        {
            hashtag = hashtag.Substring(0, hashtag.IndexOf(":"));
        }
        tagOnline.text = "";
        tagOnline.text += "@" + newArtist.firstName + separator + newArtist.lastName + separator + ending;
        tagOnline.text += "\n#" + hashtag;
    }

    public void InstantiatePoster(GameObject newPoster, bool isMainPoster = true)
    {
        TextMeshProUGUI posterTitle = newPoster.GetComponentInChildren<TextMeshProUGUI>();
        Image posterBackground = newPoster.GetComponentInChildren<Image>();
        RectTransform parentRect = newPoster.GetComponent<RectTransform>();
        posterBackground.sprite = artist.backgrounds[Random.Range(0, artist.backgrounds.Length)];

        if (!isMainPoster)
        {
            parentRect.sizeDelta = new Vector2(Random.Range(5, 10), Random.Range(5, 10));
            parentRect.anchoredPosition = new Vector2(Random.Range(0, posterCanvasRect.rect.width - parentRect.rect.width),
                Random.Range(0, posterCanvasRect.rect.height - parentRect.rect.height));
            parentRect.localRotation = Quaternion.Euler(0, 0, Random.Range(-10, 10));
            posterTitle.fontSize = Random.Range(0.3f, 1f);

            string[] posterTextTemplates =
            {
                "[TITLE]",
                "VISIT [TITLE]",
                "VISIT [TITLE] TODAY",
                "[TITLE] ON DISPLAY NOW",
                "TICKETS ON SALE FOR [TITLE]",
                "ONCE IN A LIFETIME EXPERIENCE: [TITLE]",
                "IMMERSE YOURSELF: [TITLE]",
                "BUY TICKETS TO [TITLE] NOW",
            };
            posterTitle.text = posterTextTemplates[Random.Range(0, posterTextTemplates.Length)].Replace("[TITLE]", title);
        }

        else
        {
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
            posterTitle.text = title;
        }

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
