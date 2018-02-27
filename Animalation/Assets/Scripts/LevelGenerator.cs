using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class LevelGenerator : MonoBehaviour {

    public static LevelGenerator Instance;
    public Texture2D map;
	public ColorToPrefab[] colorMappings;
    public GameObject flagPoint;

    public static void SetTextureImporterFormat(Texture2D texture, bool isReadable)
    {
        if (null == texture) return;

        string assetPath = AssetDatabase.GetAssetPath(texture);
        var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (tImporter != null)
        {
            tImporter.textureType = TextureImporterType.Default;

            tImporter.isReadable = isReadable;

            AssetDatabase.ImportAsset(assetPath);
            AssetDatabase.Refresh();
        }
    }

    // Use this for initialization
    void Awake () {
        SetTextureImporterFormat(map, true);
        Instance = this;
		GenerateLevel ();		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void GenerateLevel() {
		//foreach(ColorToPrefab colorMapping in colorMappings) {
		//	//Debug.Log("Colormapping " + colorMapping.color);
		//}

		for(int x = 0; x < map.width; x++) {
			for(int y = 0; y < map.height; y++) {
				GenerateTile(x,y);
			}
		}
	}

	void GenerateTile(int x, int y) {
		Color pixelColor = map.GetPixel(x,y);
		if(pixelColor.a == 0) {
			// Pixel transparent
			return;
		}

		foreach(ColorToPrefab colorMapping in colorMappings) {
			if (colorMapping.color.Equals(pixelColor)) {
				//Debug.Log("Doing stuff");
				Vector2 position = new Vector2(x - map.width/2, y - map.height/2);
				GameObject t = Instantiate(colorMapping.prefab, position, Quaternion.identity);
				t.transform.parent = this.gameObject.transform;

                if(t.tag == "Flag")
                {
                    flagPoint = t;
                }
				// TODO: Server action here
			}
		}
		//Debug.Log(pixelColor);
	}

    public T[] getAllTilesWithComponents<T>()
    {
        return transform.GetComponentsInChildren<T>();
    }

}
