using TMPro;
using UnityEngine;

public class PieceRenderer : MonoBehaviour
{
    public Piece ItemPiece;
    public SpinnerController spinnerController;

    private SpriteRenderer pieceSpriteRenderer;
    private Texture2D pieceTexture;
    private Sprite pieceSprite;

    private GameObject itemName;
    private TextMeshPro meshItemName;

    private GameObject itemQuantity;
    private TextMeshPro meshItemQuantity;

    private GameObject itemSprite;
    private SpriteRenderer itemSpriteRenderer;

    private bool isSelected;

    void Start()
    {

        SetupTexture();
        RenderPiece();

        CreateItemSprite(ref itemSprite, ref itemSpriteRenderer);

        CreateText(ref itemName, ref meshItemName, "Item Name", ItemPiece.NameColor, ItemPiece.PieceItem.Name, TextAlignmentOptions.Top, ItemPiece.NameSize, ItemPiece.NameWidth, ItemPiece.NameHeight);
        CreateText(ref itemQuantity, ref meshItemQuantity, "Item Quantity", ItemPiece.QuantityColor, ItemPiece.Quantity.ToString(), TextAlignmentOptions.Top, ItemPiece.QuantitySize, ItemPiece.QuantityWidth, ItemPiece.QuantityHeight);

        UpdateOffset(itemName);
        UpdateOffset(itemQuantity);
    }
    void Update()
    {
        UpdatePiece();
    }

    void RenderPiece()
    {
        if (ItemPiece.PieceAngle > 180)
        {
            RenderCircle();
        }
        else
        {
            RenderAnglePiece();

        }
    }
    void UpdatePiece()
    {
        if (isSelected != ItemPiece.Selected)
        {
            RenderPiece();
            isSelected = ItemPiece.Selected;
        }
        if (spinnerController.UpdateOverFrame)
        {
            UpdateSprite();

            UpdateText(ref itemName, ref meshItemName, "Item Name", ItemPiece.NameColor, ItemPiece.PieceItem.Name, TextAlignmentOptions.Top, ItemPiece.NameSize, ItemPiece.NameWidth, ItemPiece.NameHeight);
            UpdateText(ref itemQuantity, ref meshItemQuantity, "Item Quantity", ItemPiece.QuantityColor, ItemPiece.Quantity.ToString(), TextAlignmentOptions.Top, ItemPiece.QuantitySize, ItemPiece.QuantityWidth, ItemPiece.QuantityHeight);
            
            UpdateOffset(itemName);
            UpdateOffset(itemQuantity);
        }
    }

    void UpdateSprite()
    {
        itemSprite.transform.position = Vector3.zero;
        itemSprite.transform.Translate(new Vector3(0, ItemPiece.SpriteOffset + ItemPiece.PieceOffset, 0), Space.Self);
        itemSpriteRenderer.sprite = ItemPiece.PieceItem.Image;
        //itemSpriteRenderer.sprite = Sprite.Create(itemSpriteRenderer.sprite.texture, itemSpriteRenderer.sprite.rect, new Vector2(0.5f, 0));
        itemSprite.transform.localScale = new Vector3(ItemPiece.SpriteWidthScale, ItemPiece.SpriteHeightScale, 0);
    }

    void UpdateOffset(GameObject go)
    {
        go.transform.position = Vector3.zero;
        go.transform.Translate(new Vector3(0, ItemPiece.PieceOffset, 0), Space.Self);
    }

    void UpdateText(ref GameObject text, ref TextMeshPro textMesh, string name, Color color, string content, TextAlignmentOptions align, float fontSize, float width, float height)
    {
        textMesh.color = color;
        textMesh.text = content;
        textMesh.alignment = align;
        textMesh.rectTransform.sizeDelta = new Vector2(width, height);
        textMesh.fontSizeMin = fontSize / 2;
        textMesh.fontSizeMax = fontSize;
    }
    void CreateText(ref GameObject text, ref TextMeshPro textMesh, string name, Color color, string content, TextAlignmentOptions align, float fontSize, float width, float height)
    {
        text = new GameObject();
        textMesh = text.AddComponent<TextMeshPro>();

        text.transform.position = Vector3.zero;
        text.transform.Rotate(new Vector3(0, 0, -ItemPiece.PieceAngle / 2));
        text.name = name;

        textMesh.color = color;
        textMesh.text = content;
        textMesh.alignment = align;
        textMesh.rectTransform.sizeDelta = new Vector2(width, height);
        textMesh.fontSizeMin = fontSize / 2;
        textMesh.fontSizeMax = fontSize;
        textMesh.characterWidthAdjustment = 80f;

        textMesh.enableAutoSizing = true;
        text.transform.SetParent(transform, false);
    }

    void SetupTexture()
    {
        Vector2 pivot = new Vector2(0, 0.5f);
        if (ItemPiece.PieceAngle > 180)
        {
            pivot = new Vector2(0.5f, 0.5f);
            ItemPiece.PieceRadius *= 2;
            pieceTexture = new Texture2D(ItemPiece.PieceRadius, ItemPiece.PieceRadius);
            pieceSprite = Sprite.Create(pieceTexture, new Rect(0, 0, ItemPiece.PieceRadius - 1, ItemPiece.PieceRadius), pivot);
        }
        else
        {
            pieceTexture = new Texture2D(ItemPiece.PieceRadius, ItemPiece.PieceRadius * 2);
            pieceSprite = Sprite.Create(pieceTexture, new Rect(0, 0, ItemPiece.PieceRadius - 1, ItemPiece.PieceRadius * 2), pivot);
        }
        pieceSpriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        pieceSpriteRenderer.sprite = pieceSprite;
        pieceSpriteRenderer.sortingOrder = -1;
    }

    void CreateItemSprite(ref GameObject itemSprite, ref SpriteRenderer spriteRenderer)
    {
        itemSprite = new GameObject();
        itemSprite.name = "Item Sprite";
        spriteRenderer = itemSprite.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = ItemPiece.PieceItem.Image;
        //spriteRenderer.sprite = Sprite.Create(spriteRenderer.sprite.texture, spriteRenderer.sprite.rect, new Vector2(0.5f, 0));
        itemSprite.transform.localScale = new Vector3(ItemPiece.SpriteWidthScale, ItemPiece.SpriteHeightScale, 0);
        itemSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -ItemPiece.PieceAngle / 2));
        itemSprite.transform.Translate(new Vector3(0, ItemPiece.SpriteOffset + ItemPiece.PieceOffset, 0), Space.Self);

        itemSprite.transform.SetParent(transform, false);
    }

    void RenderCircle()
    {
        Texture2D currentTexture;
        if (ItemPiece.Selected)
        {
            currentTexture = ItemPiece.SelectedPieceTexture;
            pieceSpriteRenderer.color = ItemPiece.SelectedPieceColor;
        }
        else
        {
            currentTexture = ItemPiece.PieceTexture;
            pieceSpriteRenderer.color = ItemPiece.PieceColor;
        }
        for (int y = 0; y < pieceTexture.height; y++)
        {
            for (int x = 0; x < pieceTexture.width; x++)
            {
                if (IsInCircle(new Vector2(ItemPiece.PieceRadius / 2, ItemPiece.PieceRadius / 2), new Vector2(x, y), ItemPiece.PieceRadius / 2))
                {
                    pieceTexture.SetPixel(x, y, currentTexture.GetPixel(x, y));
                }
                else
                    pieceTexture.SetPixel(x, y, new Color(0, 0, 0, 0));
            }
        }
        pieceTexture.Apply();
    }
    void RenderAnglePiece()
    {
        Texture2D currentTexture;
        if (ItemPiece.Selected)
        {
            currentTexture = ItemPiece.SelectedPieceTexture;
            pieceSpriteRenderer.color = ItemPiece.SelectedPieceColor;
        }
        else
        {
            currentTexture = ItemPiece.PieceTexture;
            pieceSpriteRenderer.color = ItemPiece.PieceColor;
        }
        for (int y = 0; y < pieceTexture.height; y++)
        {
            for (int x = 0; x < pieceTexture.width; x++)
            {
                if (CheckPoint(x, y, ItemPiece.PieceRadius, ItemPiece.PieceAngle))
                {
                    pieceTexture.SetPixel(x, y, currentTexture.GetPixel(x, y));
                }
                else
                    pieceTexture.SetPixel(x, y, new Color(0, 0, 0, 0));
            }
        }
        pieceTexture.Apply();
    }
    bool CheckPoint(int x, int y, int radius, float angle)
    {
        Vector2 aPoint = new Vector2(0, 0);
        Vector2 mPoint = new Vector2(0, radius);
        Vector2 fPoint = new Vector2(x, y);

        float fAngle = Vector2.Angle(aPoint - mPoint, mPoint - fPoint);

        return fAngle <= angle && IsInCircle(mPoint, fPoint, radius);

    }
    bool IsInCircle(Vector2 mPoint, Vector2 fPoint, int radius)
    {
        return Vector2.Distance(mPoint, fPoint) <= radius;
    }
}
