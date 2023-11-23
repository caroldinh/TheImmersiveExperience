using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meta.WitAi;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random=UnityEngine.Random;
public class RoomController : MonoBehaviour
{

    enum AnimationType { GLIDE = 0, FADE = 1 }
    public RoomAssets roomAssetManager;
    public Canvas[] wallCanvases;
    public AudioSource musicSource;
    private ArtistAsset _currentArtist;
    private AnimationType _currAnimationType;
    private List<GameObject> _wallImages = new List<GameObject>();
    public float transitionSpeed = 0.01f;
    private int _roomCount = -1;

    // Start is called before the first frame update
    void Start()
    {
        GenerateRoom();
    }

    // Update is called once per frame
    void Update()
    {
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
        Debug.Log(_currentArtist.name);
        musicSource.clip = roomAssetManager.audioClips[Random.Range(0, roomAssetManager.audioClips.Length)];
        musicSource.Play();
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material = roomAssetManager.wallMaterials[Random.Range(0, roomAssetManager.wallMaterials.Length)];
        renderer.material.SetTexture("_BaseTexture", _currentArtist.backgrounds[Random.Range(0, _currentArtist.backgrounds.Length)]);

        _currAnimationType = (AnimationType) Random.Range(0, 1);
        foreach (Canvas wallCanvas in wallCanvases)
        {
            for (int i = 0; i < Random.Range(3, 10); i++)
            {
                GameObject newImage = Instantiate(roomAssetManager.imagePrefab, wallCanvas.transform);
                
                // Set image for new sprite
                newImage.GetComponent<Image>().sprite =
                    _currentArtist.sprites[Random.Range(0, _currentArtist.sprites.Length)];

                RectTransform newRectTransform = newImage.GetComponent<RectTransform>();
                newRectTransform.anchoredPosition =
                    new Vector2(Random.Range(0, wallCanvas.GetComponent<RectTransform>().rect.width), 0);
                
                _wallImages.Add(newImage);
            }
        }
    }

    public int GetRoomCount()
    {
        return _roomCount;
    }
}
