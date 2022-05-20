using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpinnerController : MonoBehaviour
{
    const int CIRCLE_DEGREE = 360;
    public int Radius;
    public bool Spin;
    public float Speed;
    public int MinTimes;
    public int MaxTiems;
    public AnimationCurve SpeedCurve;
    public int Index;
    public bool RandomIndex;
    public Sprite Background;
    public Sprite Pointer;
    public float PointerOffset;
    public Vector3 PointerRotation;
    public GameObject SpinButton;
    public bool UpdateOverFrame;

    public Piece[] Pieces;
    private List<PieceRenderer> pieceRenderers;

    private GameObject pieceContainer;

    private GameObject pointerGO;
    private SpriteRenderer pointerSpriteRenderer;

    private SpriteRenderer backgroundSpriteRenderer;

    private AudioSource audioSource;

    private bool isSpin;
    private float currentDegree;
    private float targetDegree;
    private float angle;


    void Start()
    {
        SetupDontDestroy();

        pieceRenderers = new List<PieceRenderer>();
        audioSource = GetComponent<AudioSource>();


        pieceContainer = new GameObject();
        pieceContainer.name = "Pieces";
        pieceContainer.transform.SetParent(transform, true);

        backgroundSpriteRenderer = GetComponent<SpriteRenderer>();
        backgroundSpriteRenderer.sortingOrder = -2;

        //CreateSprite(ref spinButtonGO, ref spinButtonSpriteRenderer, SpinButton, "Spin Button", 1);
        Instantiate(SpinButton, transform, true).name = "Spin Button";

        CreateSprite(ref pointerGO, ref pointerSpriteRenderer, Pointer, "Pointer", 1);
        pointerGO.transform.Translate(new Vector3(0, PointerOffset, 0), Space.Self);
        pointerGO.transform.rotation = Quaternion.Euler(PointerRotation);

        CreatePieces();
    }
    void Update()
    {
        UpdateBackground();
        SpinListener();
        Spinning();

    }

    void LoadLevel()
    {
        SceneManager.LoadScene("Level_" + Index);
    }

    void SetupDontDestroy()
    {
        if (GameObject.FindGameObjectsWithTag("GameController").Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);

    }
    void UpdateBackground()
    {
        backgroundSpriteRenderer.sprite = Background;

    }
    public void StartSpin()
    {
        Spin = true;
    }
    void CreateSprite(ref GameObject go, ref SpriteRenderer renderer, Sprite sprite, string name, int sortingOrder)
    {
        go = new GameObject();
        go.name = name;
        go.transform.SetParent(transform, true);
        renderer = go.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sortingOrder = sortingOrder;
    }
    void CreatePieces()
    {
        angle = CIRCLE_DEGREE / Pieces.Length;
        GameObject piece;
        PieceRenderer pieceRenderer;
        for (int i = 0; i < Pieces.Length; i++)
        {
            piece = new GameObject();
            piece.name = "Piece " + i;
            piece.transform.position = transform.position;
            piece.transform.rotation = Quaternion.identity;
            piece.transform.SetParent(pieceContainer.transform, true);
            Pieces[i].PieceRadius = Radius;
            Pieces[i].PieceAngle = angle;
            pieceRenderer = piece.AddComponent<PieceRenderer>();
            pieceRenderer.ItemPiece = Pieces[i];
            pieceRenderer.spinnerController = this;
            piece.transform.Rotate(new Vector3(0, 0, angle * i));
            pieceRenderers.Add(pieceRenderer);
        }
    }
    void SpinListener()
    {
        if (Spin)
        {
            Spin = false;
            isSpin = true;
            currentDegree = currentDegree % CIRCLE_DEGREE;
            if (RandomIndex)
            {
                Index = RandomPiece();
            }
            targetDegree = ((CIRCLE_DEGREE * Random.Range(MinTimes, MaxTiems) + angle * Index) - angle / 2);
            audioSource.Play();
            audioSource.loop = true;
        }
    }
    void Spinning()
    {
        if (isSpin)
        {
            pieceContainer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -currentDegree));
            currentDegree += Time.deltaTime * Speed + Speed * SpeedCurve.Evaluate(currentDegree / targetDegree);
            if (currentDegree >= targetDegree)
            {
                isSpin = false;
                Pieces[Index].Selected = true;
                audioSource.Stop();
                Debug.Log(Pieces[Index].PieceItem.Name + " X " + Pieces[Index].Quantity);
                LoadLevel();
            }
        }
    }
    int RandomPiece()
    {
        int totalRate = 0;
        int countRate = 0;
        int randomRate = 0;
        for (int i = 0; i < Pieces.Length; i++)
        {
            totalRate += Pieces[i].Rate;
        }
        randomRate = Random.Range(0, totalRate);
        for (int i = 0; i < Pieces.Length; i++)
        {
            countRate += Pieces[i].Rate;
            if (randomRate < countRate) return i;
        }
        return -1;
    }
}

