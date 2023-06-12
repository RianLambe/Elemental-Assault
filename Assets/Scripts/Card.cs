using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Card : ScriptableObject
{
    public int value;
    public enum elementTypes {Fire, Earth, Ice, Water, Wind}
    public elementTypes elements;
    public Sprite elementImage;
    public Sprite background;

    public List<elementTypes> BeatenBy;
}
