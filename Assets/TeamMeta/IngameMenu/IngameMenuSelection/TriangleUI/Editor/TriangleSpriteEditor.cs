using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(TriangleSpriteUI))]
public class TriangleSpriteEditor : Editor
{
    TriangleSpriteUI triangleSpriteUI;
    Vector2[] editorVertices = new Vector2[3];

    Task updateSpriteDelayTask;
    static float handleSize = 3;

    private void OnEnable()
    {
        triangleSpriteUI = target as TriangleSpriteUI;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //edge = EditorGUILayout.ObjectField(edge, typeof(Edge), allowSceneObjects: true) as Edge;

        var headerSkin = GUI.skin.label;
        headerSkin.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("Editor", headerSkin);
        handleSize = EditorGUILayout.FloatField("HandleSize",handleSize);


    }
    public void OnSceneGUI()
    {
        if (EditorApplication.isPlaying ==false || !SceneManager.GetActiveScene().isLoaded)
        {
            return;
        }
        if (triangleSpriteUI == null)
            return;
        if (triangleSpriteUI.Image==null)
        {
            return;
        }
        Handles.color = Color.blue;

        var image = triangleSpriteUI.Image;
        var transform = triangleSpriteUI.Image.transform;

        var vertices = triangleSpriteUI.ScreenVertices;
        var centerLocalPosition = CalculateCenter();
        var centerWorldPosition = new Vector2(transform.position.x + (image.rectTransform.rect.xMin * transform.lossyScale.x), transform.position.y + image.rectTransform.rect.yMin * transform.lossyScale.y);

        for (int i = 0; i < 3; i++)
        {
            Vector2 handlePosition = AppendRectTransformationsToVertice(vertices[i]);

            handlePosition = ApplyRotationToVertice(handlePosition, centerLocalPosition);
            
            handlePosition += centerWorldPosition;

            Vector2 point = Handles.FreeMoveHandle(handlePosition, transform.rotation, handleSize, Vector3.zero, Handles.ConeHandleCap);

            if (point != handlePosition)
            {

                editorVertices[0] = triangleSpriteUI.A;
                editorVertices[1] = triangleSpriteUI.B;
                editorVertices[2] = triangleSpriteUI.C;

                point -= centerWorldPosition;
                point = UndoRotationToVertice(point,centerLocalPosition);

                point = BoundPointToRect(point);

                point = UndoRectTransformationsToVertice(point);
                editorVertices[i] = point;

                //Delay UpdateSprite by one frame because otherwise sprite.OverrideGeometry will throw an error
                //That error comes when calling it from OnSceneGUI or OnGUI
                if (updateSpriteDelayTask== null)
                    updateSpriteDelayTask = UpdateSpriteDelayRoutine(editorVertices[0], editorVertices[1], editorVertices[2]);
            }
        }
    }
    Vector2 BoundPointToRect(Vector2 point)
    {
        var image = triangleSpriteUI.Image;
        var transform = triangleSpriteUI.Image.transform;

        float rightBound = image.rectTransform.rect.width * transform.lossyScale.x;
        float leftBound = 0;
        point.x = Mathf.Clamp(point.x, leftBound, rightBound);

        float topBound = (int)image.rectTransform.rect.height * transform.lossyScale.y;
        float bottomBound = 0;
        point.y = Mathf.Clamp(point.y, bottomBound, topBound);

        return point;
    }
    async Task UpdateSpriteDelayRoutine(Vector2 a, Vector2 b, Vector2 c)
    {
        await Task.Delay(1);

        Undo.RegisterCompleteObjectUndo(triangleSpriteUI, "Update sprite");
        //Undo.RegisterCompleteObjectUndo(triangleSpriteUI.Image.sprite, "Sprite");

        triangleSpriteUI.UpdateSprite(a, b, c);

        updateSpriteDelayTask = null;
    }
    private Vector2 ApplyRotationToVertice(Vector2 vertice, Vector2 center)
    {
        vertice += center;
        vertice = triangleSpriteUI.transform.rotation * vertice;
        vertice -= center;

        return vertice;
    }
    private Vector2 UndoRotationToVertice(Vector2 vertice, Vector2 center)
    {
        vertice += center;
        vertice = Quaternion.Inverse( triangleSpriteUI.transform.rotation) * vertice;
        vertice -= center;

        return vertice;
    }
    Vector2 UndoRectTransformationsToVertice(Vector2 vertice)
    {
        var image = triangleSpriteUI.Image;
        var transform = image.rectTransform;

        vertice.x /= transform.lossyScale.x;
        vertice.x /= image.rectTransform.rect.width;

        vertice.y /= transform.lossyScale.y;
        vertice.y /= image.rectTransform.rect.height;

        return vertice;
    }
    Vector2 AppendRectTransformationsToVertice(Vector2 vertice)
    {
        var image = triangleSpriteUI.Image;
        var transform = image.rectTransform;

        vertice.x /= image.sprite.texture.width;
        vertice.x *= image.rectTransform.rect.width;
        vertice.x *= transform.lossyScale.x;

        vertice.y /= image.sprite.texture.height;
        vertice.y *= image.rectTransform.rect.height;
        vertice.y *= transform.lossyScale.y;

        return vertice;
    }
    private Vector2 CalculateCenter()
    {
        var image = triangleSpriteUI.Image;
        var transform = image.transform;
        var rectPivot = image.rectTransform.pivot;
        Vector2 centerPoint = new Vector3(-image.rectTransform.rect.width * rectPivot.x * transform.lossyScale.x,
                                          -image.rectTransform.rect.height * rectPivot.y * transform.lossyScale.y, 0);

        return centerPoint;
    }
}
