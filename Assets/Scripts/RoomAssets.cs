using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class RoomAssets : ScriptableObject
{
    public AudioClip[] audioClips;
    public ArtistAsset[] artists;
    public Material[] wallMaterials;
    public GameObject imagePrefab;
    public TMP_FontAsset[] fontAssets;
}
