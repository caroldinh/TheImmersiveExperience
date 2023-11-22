using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random=UnityEngine.Random;
public class RoomController : MonoBehaviour
{

    public RoomAssets roomAssetManager;
    public Canvas[] wallCanvases;
    public AudioSource musicSource;
    private ArtistAsset _currentArtist;
    private List<GameObject> _wallImages = new List<GameObject>();
    public float transitionSpeed = 0.01f;

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
            //image.transform.position += new Vector3(transitionSpeed, 0, 0);
            RectTransform imageTransform = image.GetComponent<RectTransform>();
            Image imageComponent = image.GetComponent<Image>();
            imageTransform.anchoredPosition += new Vector2(transitionSpeed, 0);
            if (imageTransform.anchoredPosition.x + (2 * imageComponent.sprite.bounds.size.x) > 
                image.transform.parent.GetComponent<RectTransform>().rect.width)
            {
                imageTransform.anchoredPosition = Vector2.zero;
            }
        }
    }

    void GenerateRoom()
    {
        _wallImages.Clear();
        _currentArtist = roomAssetManager.artists[Random.Range(0, roomAssetManager.artists.Length)];
        musicSource.clip = roomAssetManager.audioClips[Random.Range(0, roomAssetManager.audioClips.Length)];
        musicSource.Play();
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
}
