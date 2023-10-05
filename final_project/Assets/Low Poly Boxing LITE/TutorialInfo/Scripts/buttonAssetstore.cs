using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class buttonAssetstore : MonoBehaviour {
   
  
   

    public void m_Assetstorebutton (string val) {
        UnityEditorInternal.AssetStore.Open("content/"+val);
    }
    
}
