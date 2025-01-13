using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace KlutterTools {

#if !KLUTTER_TOOLS_HAS_HDRP

[ScriptedImporter(0, "cube")]
class CubeLutImporter : ScriptedImporter
{
    #region ScriptedImporter implementation

    public override void OnImportAsset(AssetImportContext ctx)
    {
        var (size, table) = ParseCubeFile(ctx.assetPath);

        var tex = new Texture3D(size, size, size, TextureFormat.RGBAHalf, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.anisoLevel = 0;
        tex.SetPixels(table);
        tex.Apply();

        ctx.AddObjectToAsset("3D Lookup Texture", tex);
        ctx.SetMainObject(tex);
    }

    #endregion

    #region Parser implementation

    static bool CheckIfNumber(string word)
      => '0' <= word[0] && word[0] <= '9';

    static Color ParseColor(ReadOnlySpan<string> words)
      => new Color(float.Parse(words[0]),
                   float.Parse(words[1]),
                   float.Parse(words[2]));

    (int size, Color[] table) ParseCubeFile(string filePath)
    {
        var (size, table) = (0, new List<Color>());
        foreach (var line in File.ReadLines(filePath))
        {
            var words = line.Split();
            if (words.Length == 0 || words[0].Length == 0)
                continue;
            else if (words[0] == "LUT_3D_SIZE")
                size = int.Parse(words[1]);
            else if (CheckIfNumber(words[0]))
                table.Add(ParseColor(words));
        }
        return (size, table.ToArray());
    }

    #endregion
}

#endif

} // namespace KlutterTools
