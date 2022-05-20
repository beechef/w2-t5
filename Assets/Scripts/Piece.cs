using TMPro;
using UnityEngine;

[System.Serializable]
public class Piece
{
    [HideInInspector]
    public int PieceRadius;
    [HideInInspector]
    public float PieceAngle;

    [Header("Piece Settings")]
    [SerializeField]
    public int Rate;
    public float PieceOffset;
    public Color PieceColor;
    public Texture2D PieceTexture;
    public Color SelectedPieceColor;
    public Texture2D SelectedPieceTexture;
    public int Quantity;
    public bool Selected;
    [Header("Name Settings")]
    public float NameSize;
    public Color NameColor;
    public float NameWidth;
    public float NameHeight;
    [Header("Quantity Settings")]
    public float QuantitySize;
    public Color QuantityColor;
    public float QuantityWidth;
    public float QuantityHeight;
    [Header("Sprite Settings")]
    public float SpriteWidthScale;
    public float SpriteHeightScale;
    public float SpriteOffset;

    public Item PieceItem;

}
