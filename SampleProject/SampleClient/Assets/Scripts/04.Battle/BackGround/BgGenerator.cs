using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;
using Color = UnityEngine.Color;

public class BgGenerator : MonoBehaviour
{
    public GameObject bgPrefab; // Assign the bg0 prefab in the Inspector
    public int rowNumber = 3; // Set the number of rows
    public GameObject bg0;
    public int columnNumber = 3; // Set the number of columns
    public float tileWidth = 10f; // Width of each tile
    public float tileHeight = 10f; // Height of each tile

    private List<BackgroundUnit> backgroundUnitList = new List<BackgroundUnit>();
    [SerializeField] private BgManager bgManager = null;

    public void Init()
    {
        if (bg0 != null)
        {
            bg0.SetActive(true);
        }
        GenerateBackgroundTiles();
        ConnectToBgManager();
        DeactivateOriginalBg();
    }

    void GenerateBackgroundTiles()
    {
        for (int row = 0; row < rowNumber; row++)
        {
            for (int col = 0; col < columnNumber; col++)
            {
                // Calculate the position for the new tile
                Vector3 position = new Vector3(col * tileWidth, row * tileHeight, 0);

                // Instantiate a new tile
                GameObject newTile = Instantiate(bgPrefab, position, Quaternion.identity); //no rotation
                newTile.transform.parent = transform; // Set the new tile as a child of the BgGenerator

                // Initialize the BackgroundUnit component
                BackgroundUnit bgUnit = newTile.GetComponent<BackgroundUnit>();
                if (bgUnit != null)
                {
                    bgUnit.RowNumber = rowNumber;
                    bgUnit.ColumnNumber = columnNumber;
                    bgUnit.Init(row * columnNumber + col);
                    //pass info

                    // Add the new tile to the list
                    backgroundUnitList.Add(bgUnit);
                }

                //ModifyTileAppearance(newTile, row * columnNumber + col);
            }
        }
    }

    void ConnectToBgManager()
    {
        
        if (bgManager != null)
        {
            bgManager.SetBackgroundUnitList(backgroundUnitList);
        }
    }

    void DeactivateOriginalBg()
    {
        if (bg0 != null)
        {
            bg0.SetActive(false);
        }
    }

    void ModifyTileAppearance(GameObject tile, int index)
    {
        SpriteRenderer spriteRenderer = tile.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.Log("sprite renderer null");
        }

        spriteRenderer.color = new Color(1,1,1,1);
        //just for test

        Texture2D texture = spriteRenderer.sprite.texture;

        if (spriteRenderer == null)
        {
            Debug.Log("texture null");
        }

        Texture2D newTexture = new Texture2D(texture.width, texture.height); //we need new texture to write

        newTexture.SetPixels(texture.GetPixels()); //copy pixels from original texture

        Color indexColor = Color.white;
        char c = index.ToString()[0]; // Convert index to character

        int startX = 400; // Starting x position for the character (adjusted for larger texture)
        int startY = 400; // Starting y position (adjusted for larger texture)
        int pixelSize = 30; // Size of each "pixel" of the character in the texture

        for (int x = 0; x < 3; x++) // Adjust these loops to match your digit patterns
        {
            for (int y = 0; y < 5; y++)
            {
                if (ShouldSetPixel(c, x, y))
                {
                    int invertedY = 4 - y; //upsidedown
                    // Draw a larger "pixel" to make the digit more visible
                    for (int i = 0; i < pixelSize; i++)
                    {
                        for (int j = 0; j < pixelSize; j++)
                        {
                            newTexture.SetPixel(startX + x * pixelSize + i, startY + invertedY * pixelSize + j, indexColor);
                        }
                    }
                }
            }
        }

        newTexture.Apply();

        // Assign the modified texture to the SpriteRenderer
        spriteRenderer.sprite = Sprite.Create(newTexture, spriteRenderer.sprite.rect, new Vector3(0.5f, 0.5f,1)); //0.5 is pivot




    }

    bool ShouldSetPixel(int c, int x, int y)
    {
        // Simple representation of digits as 5x7 pixel patterns
        // Add more patterns as needed for different characters
        string[] digits = {
            "111101101101111", // 0
            "010110010010111", // 1
            "111001111100111", // 2
            "111001111001111", // 3
            "101101111001001", // 4
            "111100111001111", // 5
            "111100111101111", // 6
            "111001001001001", // 7
            "111101111101111", // 8
            "111101111001111"  // 9
        };

        if (c >= '0' && c <= '8')
        {
            int index = c - '0';
          
            return digits[index][x + y * 3] == '1';
        }
        return false;

    }
}
