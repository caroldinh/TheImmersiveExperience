using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class ArtistAsset : ScriptableObject
{
    public string artistName;
    public Sprite[] sprites;
    public Texture2D[] backgrounds;
}
