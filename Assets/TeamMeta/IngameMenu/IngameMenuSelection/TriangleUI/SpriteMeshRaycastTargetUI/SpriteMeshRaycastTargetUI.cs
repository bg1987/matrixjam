using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class SpriteMeshRaycastTargetUI : MonoBehaviour, ICanvasRaycastFilter
{
    Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        Vector2[] vertices = image.sprite.vertices;
        ushort[] triangles = image.sprite.triangles;

        //string verticesString = "";
        //foreach (var vertice in vertices)
        //    verticesString += vertice+" ";
        //Debug.Log(verticesString);

        Vector3 scale = Vector3.zero;
        scale = transform.lossyScale;

        var centerPosition = new Vector2(transform.position.x + (image.rectTransform.rect.xMin*scale.x), transform.position.y + image.rectTransform.rect.yMin * scale.y);

        var rectPivot = image.rectTransform.pivot;

        Vector2 centerPoint = new Vector3(-image.rectTransform.rect.width*rectPivot.x* scale.x,
                                     -image.rectTransform.rect.height * rectPivot.y * scale.y, 0);

        vertices[0] = AppendRectTransformationsToVertice(vertices[0]);
        vertices[1] = AppendRectTransformationsToVertice(vertices[1]);
        vertices[2] = AppendRectTransformationsToVertice(vertices[2]);

        vertices[0] = ApplyRotationToVertice(vertices[0], centerPoint);
        vertices[1] = ApplyRotationToVertice(vertices[1], centerPoint);
        vertices[2] = ApplyRotationToVertice(vertices[2], centerPoint);

        vertices[0] += centerPosition;
        vertices[1] += centerPosition;
        vertices[2] += centerPosition;

        var pointInTriangle = IsInTriangle(sp, vertices[0], vertices[1], vertices[2]);

        #if UNITY_EDITOR
        Color debugColor = pointInTriangle ? Color.green : Color.red;
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 start = vertices[i];
            Vector2 end = vertices[(i + 1) % vertices.Length];
            Debug.DrawLine(start, end, debugColor, 0.1f, false);
        }
        #endif

        return pointInTriangle;
    }
    private Vector2 ApplyRotationToVertice(Vector2 vertice, Vector2 center)
    {
        Vector3 scale = transform.lossyScale;
        var centerPosition = new Vector2(transform.position.x + (image.rectTransform.rect.xMin * scale.x), transform.position.y + image.rectTransform.rect.yMin * scale.y);

        Debug.DrawLine((Vector2)center + Vector2.up,  (Vector2)center + Vector2.down, Color.cyan, 1f, false);

        vertice += center;
        vertice = transform.rotation * vertice;
        vertice -= center;

        return vertice;
    }
    Vector2 AppendRectTransformationsToVertice(Vector2 vertice)
    {
        Vector3 scale = Vector3.zero;
        scale = transform.lossyScale;

        vertice.x /= image.sprite.texture.width;
        vertice.x *= image.rectTransform.rect.width;
        vertice.x *= scale.x;

        vertice.y /= image.sprite.texture.height;
        vertice.y *= image.rectTransform.rect.height;
        vertice.y *= scale.y;

        return vertice;
    }
    float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
    }
    bool IsInTriangle(Vector2 pt, Vector2 v1, Vector2 v2, Vector2 v3)
    {
        float d1, d2, d3;
        bool has_neg, has_pos;

        d1 = Sign(pt, v1, v2);
        d2 = Sign(pt, v2, v3);
        d3 = Sign(pt, v3, v1);

        has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
        has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

        return !(has_neg && has_pos);
    }
}
