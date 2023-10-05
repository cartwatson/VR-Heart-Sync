using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(buttonAssetstore))]
public class Assetstorebutton : Editor
{
    
    public override void OnInspectorGUI()
    {

        
        
       
        buttonAssetstore myScript = (buttonAssetstore)target;

        Texture Tex= Resources.Load("Small") as Texture;
        Texture Tex1 = Resources.Load("Small1") as Texture;
        Texture Tex2 = Resources.Load("Small2") as Texture;
        Texture Tex3 = Resources.Load("Small3") as Texture;

        Asset("Low Poly Sports Characters", "95648", Tex, myScript);
        Asset("Fort Building", "130058",Tex1,myScript);
        Asset("Brick Town Cars", "96585", Tex2, myScript);
        Asset("Low Poly Trees V2", "97263", Tex3, myScript);


    }

    #region ASSET
    void Asset(string AssetText, string AssetLink, Texture AssetImage,buttonAssetstore myScript  )
    {
        
        //TEXT
        GUILayout.TextArea(AssetText);

        //IMAGE
        using (new GUILayout.HorizontalScope())
        {
            GUILayout.FlexibleSpace();
            GUILayout.Box(AssetImage, GUILayout.ExpandWidth(true), GUILayout.Width(AssetImage.width), GUILayout.Height(AssetImage.height));
            GUILayout.FlexibleSpace();
        }

        //BUTTON
        using (new GUILayout.HorizontalScope())
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("AssetStore", GUILayout.ExpandWidth(false), GUILayout.Width(AssetImage.width)))
            {
                myScript.m_Assetstorebutton(AssetLink);
            }
            GUILayout.FlexibleSpace();
        }
    }
    #endregion
}