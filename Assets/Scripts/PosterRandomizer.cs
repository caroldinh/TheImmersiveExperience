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
    public Texture2D[] posterTextures;
    
    private List<GameObject> posters = new List<GameObject>();
    private List<Material> posterMaterials = new List<Material>();
    private ArtistAsset artist;
    private TMP_FontAsset fontAsset;
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

    public void ResetPosters(ArtistAsset newArtist, string newTitle, TMP_FontAsset newFont)
    {
        foreach (GameObject poster in posters)
        {
           poster.DestroySafely(); 
        }
        posters.Clear();
        foreach (Material mat in posterMaterials)
        {
            mat.DestroySafely();
        }
        posterMaterials.Clear();
        artist = newArtist;
        title = newTitle;
        fontAsset = newFont;
        InstantiatePoster(gameObject);
        int numPosters = Random.Range(3, 8);
        for (int i = 0; i < numPosters; i++)
        {
            GameObject newPoster = Instantiate(posterPrefab, posterCanvas.transform);
            InstantiatePoster(newPoster, (float) i / numPosters);
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

    public void InstantiatePoster(GameObject newPoster, float posterX = -1f)
    {
        TextMeshProUGUI posterTitle = newPoster.GetComponentInChildren<TextMeshProUGUI>();
        Image posterBackground = newPoster.GetComponentInChildren<Image>();
        RectTransform parentRect = newPoster.GetComponent<RectTransform>();
        Material newPosterMaterial = Instantiate(posterBackground.material);
        newPosterMaterial.SetTexture("_PosterTexture", posterTextures[Random.Range(0, posterTextures.Length)]);
        newPosterMaterial.SetTexture("_MainTexture", artist.backgrounds[Random.Range(0, artist.backgrounds.Length)].texture);
        posterBackground.material = newPosterMaterial;
        posterMaterials.Append(newPosterMaterial);

        if (posterX >= 0)
        {
            float xPos = posterX * posterCanvasRect.rect.width + Random.Range(-1, 1) - 1f;
            parentRect.sizeDelta = new Vector2(Random.Range(5, 10), Random.Range(5, 10));
            parentRect.anchoredPosition = new Vector2(xPos,
                Random.Range(0, posterCanvasRect.rect.height - parentRect.rect.height));
            parentRect.localRotation = Quaternion.Euler(0, 0, Random.Range(-10, 10));

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
        posterTitle.font = fontAsset;
        posterTitle.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, 0.5f);
        posterTitle.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.5f);
        if (avgBrightness > 0.5)
        {
            posterTitle.fontMaterial.SetColor(ShaderUtilities.ID_FaceColor, Color.black);
            posterTitle.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.white);
        }
        else
        {
            posterTitle.fontMaterial.SetColor(ShaderUtilities.ID_FaceColor, Color.white);
            posterTitle.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
        }
    }
}
