using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class RoomAssets : ScriptableObject
{
    public AudioClip[] audioClips;
    public ArtistAsset[] artists;
    public Material[] wallShaders;
    public GameObject imagePrefab;
}
