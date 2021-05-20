using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TriangleSpriteUI : MonoBehaviour
{
    Image image;
    public Image Image { get => image; }
    [SerializeField] bool useSpriteMesh = false;

    [SerializeField] Vector2 a = new Vector2(0, 0);
    public Vector2 A { get => a;}
        
    [SerializeField] Vector2 b = new Vector2(0.5f, 1);
    public Vector2 B { get => b;}

    [SerializeField] Vector2 c = new Vector2(1f, 0);
    public Vector2 C { get => c;}

    Vector2[] screenVertices = new Vector2[3] { new Vector2(0, 0), new Vector2(0.5f, 1), new Vector2(1, 0) };
    public Vector2[] ScreenVertices { get=> screenVertices;}

    Texture2D texture2D;
    ushort[] triangles = new ushort[3];

    Vector2 aValidate = new Vector2(0, 0);
    Vector2 bValidate = new Vector2(0.5f, 1);
    Vector2 cValidate = new Vector2(1f, 0);
    private Material material;
    private bool isAwake = false;
    private Coroutine updateSpriteDelayRoutine;

    // Start is called before the first frame update
    void Awake()
    {
        image = GetComponent<Image>();
        texture2D = new Texture2D(64, 64);

        for (int y = 0; y < texture2D.height; y++)
        {
            for (int x = 0; x < texture2D.width; x++)
            {
                Color color = Color.white;
                texture2D.SetPixel(x, y, color);
            }
        }
        texture2D.Apply();

        image.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero, 1, 1, meshType: SpriteMeshType.Tight);
        image.useSpriteMesh = useSpriteMesh;
        material =new Material( image.material);
        image.material = material;
        //image.alphaHitTestMinimumThreshold = 0.5f;
        isAwake = true;
    }
    private void Start()
    {
        UpdateSprite(a, b, c);
    }
    Vector2 LocalToScreenVertex(Vector2 vertex, Sprite sprite)
    {
        vertex.x *= sprite.texture.width;
        vertex.y *= sprite.texture.height;

        return vertex;
    }
    public void UpdateSprite(Vector2 a, Vector2 b, Vector2 c)
    {
        a.x = Mathf.Clamp01(a.x);
        a.y = Mathf.Clamp01(a.y);
        b.x = Mathf.Clamp01(b.x);
        b.y = Mathf.Clamp01(b.y);
        c.x = Mathf.Clamp01(c.x);
        c.y = Mathf.Clamp01(c.y);
        

        Sprite sprite = image.sprite;

        this.a = a;
        this.b = b;
        this.c = c;

        image.material.SetVector("_A", a);
        image.material.SetVector("_B", b);
        image.material.SetVector("_C", c);
        image.material.SetVector("_Scale", transform.lossyScale);

        screenVertices[0] = LocalToScreenVertex(a, sprite);
        screenVertices[1] = LocalToScreenVertex(b, sprite);
        screenVertices[2] = LocalToScreenVertex(c, sprite);

        aValidate = a;
        bValidate = b;
        cValidate = c;

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        sprite.OverrideGeometry(screenVertices, triangles);
        texture2D = sprite.texture;

        //After OverrideGeometry useSpriteMesh effect isn't updated on screen. Toggle off on makes it work properly
        image.useSpriteMesh = !image.useSpriteMesh;
        image.useSpriteMesh = !image.useSpriteMesh;

    }
    private void OnValidate()
    {
        if (!isAwake)
            return;
        //if (Application.isPlaying || SceneManager.GetActiveScene().isLoaded==false)
        //    return;

        if (a != aValidate || b != bValidate || c != cValidate)
        {
            if (updateSpriteDelayRoutine!=null)
                StopCoroutine(updateSpriteDelayRoutine);
            updateSpriteDelayRoutine = StartCoroutine(UpdateSpriteDelayRoutine(a,b,c));
            //UpdateSprite(a, b, c);
        }
    }
    IEnumerator UpdateSpriteDelayRoutine(Vector2 a, Vector2 b, Vector2 c)
    {
        yield return null;
        UpdateSprite(a, b, c);
        updateSpriteDelayRoutine = null;
    }
}
