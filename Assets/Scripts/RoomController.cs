using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meta.WitAi;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random=UnityEngine.Random;
public class RoomController : MonoBehaviour
{

    enum AnimationType { GLIDE = 0, FADE = 1 }
    public RoomAssets roomAssetManager;
    public Canvas[] wallCanvases;
    public AudioSource[] musicSources;
    public float transitionSpeed = 0.01f;
    public PosterRandomizer PosterRandomizer;
    public TextMeshProUGUI Score;
    public GameObject[] roomProps;
    public InputActionReference inputMap;
    
    private ArtistAsset _currentArtist;
    private AnimationType _currAnimationType;
    private TMP_FontAsset _currFont;
    private string _exhibitionTitle;
    private List<GameObject> _wallImages = new List<GameObject>();
    private int _roomCount = -1;
    
    // Start is called before the first frame update
    void Start()
    {
        inputMap.action.Enable();
        GenerateRoom();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputMap.action.IsPressed())
        {
            Debug.Log("Is pressed");
        }

        if (inputMap.action.triggered)
        {
            Debug.Log("triggered");
        }
        foreach (GameObject image in _wallImages)
        {
            RectTransform imageTransform = image.GetComponent<RectTransform>();
            Image imageComponent = image.GetComponent<Image>();
            imageTransform.anchoredPosition += new Vector2(transitionSpeed, 0);
            if (imageTransform.anchoredPosition.x >
                image.transform.parent.GetComponent<RectTransform>().rect.width)
            {
                imageTransform.anchoredPosition = Vector2.zero - new Vector2(imageComponent.sprite.bounds.size.x * 2, 0);
            }
        }
    }

    public void GenerateRoom()
    {
        _roomCount++;
        foreach (GameObject wallImage in _wallImages)
        {
           wallImage.DestroySafely(); 
        }
        _wallImages.Clear();
        _currentArtist = roomAssetManager.artists[Random.Range(0, roomAssetManager.artists.Length)];
        _currFont = roomAssetManager.fontAssets[Random.Range(0, roomAssetManager.fontAssets.Length)];
        _exhibitionTitle = generateExhibitionTitle();
        PosterRandomizer.ResetPosters(_currentArtist, _exhibitionTitle, _currFont);
        AudioClip clip = roomAssetManager.audioClips[Random.Range(0, roomAssetManager.audioClips.Length)];
        foreach (AudioSource musicSource in musicSources)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material = roomAssetManager.wallMaterials[Random.Range(0, roomAssetManager.wallMaterials.Length)];
        renderer.material.SetTexture("_BaseTexture", _currentArtist.backgrounds[Random.Range(0, _currentArtist.backgrounds.Length)].texture);

        _currAnimationType = (AnimationType) Random.Range(0, 1);
        foreach (Canvas wallCanvas in wallCanvases)
        {
            int numImages = Random.Range(5, 12);
            float wallCanvasWidth = wallCanvas.GetComponent<RectTransform>().rect.width;
            for (int i = 0; i < numImages; i++)
            {
                GameObject newImage = Instantiate(roomAssetManager.imagePrefab, wallCanvas.transform);
                
                // Set image for new sprite
                newImage.GetComponent<Image>().sprite =
                    _currentArtist.sprites[Random.Range(0, _currentArtist.sprites.Length)];

                RectTransform newRectTransform = newImage.GetComponent<RectTransform>();
                newRectTransform.anchoredPosition =
                    new Vector2(((float)i / numImages) * wallCanvasWidth, 0);
                
                _wallImages.Add(newImage);
            }
        }
        foreach (GameObject prop in roomProps)
        {
            if (Random.Range(0, 3) > 0)
            {
                prop.SetActive(true);
            }
            else
            {
                prop.SetActive(false);
            }
        }
        Score.text = _roomCount.ToString();
    }

    public ArtistAsset getCurrentArtist()
    {
        return _currentArtist;
    }

    public int GetRoomCount()
    {
        return _roomCount;
    }

    private string generateExhibitionTitle()
    {
        string[] structures =
        {
            "[OPENER][ARTIST NAME]: The [ADJECTIVE] [EXPERIENCE]",
            "[OPENER][ARTIST NAME]",
            "[ARTIST NAME] [MODIFIER]",
            "[ARTIST NAME] [MODIFIER]: The [ADJECTIVE] [EXPERIENCE]",
            "The [ADJECTIVE] [ARTIST NAME] [EXPERIENCE]",
            "The [ARTIST NAME] [ADJECTIVE] [EXPERIENCE]",
            "The [ADJECTIVE] [ARTIST NAME] [MODIFIER] [EXPERIENCE]"
        };
        string[] opener =
        {
            " ", "Experience ", "Imagine ", "Beyond ", "Through the Eyes of ", "Re-Imagine ", "Relive ", "Marvelous ", "Fantastic ",
            "Visions Of ", "Rethink ", "Re-imagine ", "Journey Into ", "Discover ", "Uncover ", "Exclusively ", "Reinventing ", 
        };
        string[] adjective =
        {
            "Immersive", "Interactive", "Multisensory", "4D", "Revised", "Modernized", "Original", "Cinematic"
        };
        string[] experience =
        {
            "Immersive", "Chamber", "Exhibition", "Exhibit", "Gallery", "Experience", "Tour", "Cinema"
        };
        string[] modifier =
        {
            "Re-imagined", "Examined", "Alive", "Unlocked", "Uncovered", "Revived", "On Display", "Revealed", "Today", "Exclusive", "Original"
        };

        string getArtistName()
        {
            if (Random.Range(0, 2) == 0)
            {
                return _currentArtist.firstName + " " + _currentArtist.lastName;
            }

            return _currentArtist.lastName;
        }
        string title = structures[Random.Range(0, structures.Length)];
        title = title.Replace("[OPENER]", opener[Random.Range(0, opener.Length)]);
        title = title.Replace("[ARTIST NAME]", getArtistName());
        title = title.Replace("[ADJECTIVE]", adjective[Random.Range(0, adjective.Length)]);
        title = title.Replace("[MODIFIER]", modifier[Random.Range(0, modifier.Length)]);
        title = title.Replace("[EXPERIENCE]", experience[Random.Range(0, experience.Length)]);
        return title;
    }
}
