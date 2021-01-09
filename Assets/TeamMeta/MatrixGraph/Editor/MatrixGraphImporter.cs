using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using System.IO;

[ScriptedImporter(1, "matrixgraph")]
public class MatrixGraphImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        Texture2D icon = CreateIcon();
        icon.SetPixel(1, 1, Color.black);
        TextAsset subAsset = new TextAsset(File.ReadAllText(ctx.assetPath));
        ctx.AddObjectToAsset("text", subAsset, icon);
        ctx.SetMainObject(subAsset);
    }
    Texture2D CreateIcon()
    {
        const int width = 12;
        const int height = 14;
        int[,] mPixelsMap = new int[width, height]
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,1,1,1,1,0,0,0,0,1,1,1,1,0},
            {0,1,1,1,1,1,0,0,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,0,1,1,1,1,0,1,1,1,0},
            {0,1,1,1,0,0,1,1,0,0,1,1,1,0},
            {0,1,1,1,0,0,0,0,0,0,1,1,1,0},
            {0,1,1,1,0,0,0,0,0,0,1,1,1,0},
            {0,1,1,1,0,0,0,0,0,0,1,1,1,0},
            {0,1,1,1,0,0,0,0,0,0,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0}
        };
        var icon = new Texture2D(height, width);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = Color.black;
                if (mPixelsMap[x, y] == 0)
                    color.a = 0;
                icon.SetPixel(height-1 - y, width-1 - x, color);
            }
        }

        return icon;
    }
}
