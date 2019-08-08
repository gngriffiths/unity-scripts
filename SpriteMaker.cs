using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * SpriteMaker as copied from the tutorial by Board To Bits on YouTube: https://www.youtube.com/watch?v=cIIaKdlZ4Cw&list=PL5KbKbJ6Gf9-1VAsllNBn175nF4fqnBCF&index=1
 * SpriteMaker can be used for combining multiple sprites together and/or adding color to each sprite layer.
 */

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteMaker : MonoBehaviour
{
    [SerializeField] public Texture2D[] textureArray;
    [SerializeField] public Color[] colorArray;

    SpriteRenderer rend;
    Texture2D tex;

    void Start()
    {
        tex = MakeTexture(textureArray, colorArray);
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = MakeSprite(tex);    // Assign the new sprite to the sprite in Unity
    }

    public Texture2D MakeTexture(Texture2D[] layers, Color[] layerColors)
    {
        // Bug check: If no textures are loaded 
        if(layers.Length == 0)
        {
            Debug.LogError("No image layer information in array");
            return Texture2D.whiteTexture;
        }

        Texture2D newTexture = new Texture2D(layers[0].width, layers[0].height);     // Create a texture

        // Array to store the destination texture's pixels
        Color[] colorArray = new Color[newTexture.width * newTexture.height];

        // Array of colors derived from the source texture
        Color[][] adjustedLayers = new Color[layers.Length][];

        // Populate array with cropped or expanded layer arrays
        for (int i = 0; i < layers.Length; i++)
        {
            if (i == 0 || layers[i].width == newTexture.width && layers[i].height == newTexture.height) 
            {
                adjustedLayers[i] = layers[i].GetPixels(); 
            }
            else
            {                
                int getX, getWidth, setX, setWidth;

                getX = (layers[i].width > newTexture.width) ? (layers[i].width - newTexture.width) / 2 : 0;
                getWidth = (layers[i].width > newTexture.width) ? newTexture.width : layers[i].width;
                setX = (layers[i].width < newTexture.width) ? (newTexture.width - layers[i].width) / 2 : 0;
                setWidth = (layers[i].width < newTexture.width) ? layers[i].width : newTexture.width;

                int getY, getHeight, setY, setHeight;

                getY = (layers[i].height > newTexture.height) ? (layers[i].height - newTexture.height) / 2 : 0;
                getHeight = (layers[i].height > newTexture.height) ? newTexture.height : layers[i].height;
                setY = (layers[i].height < newTexture.height) ? (newTexture.height - layers[i].height) / 2 : 0;
                setHeight = (layers[i].height < newTexture.height) ? layers[i].height : newTexture.height;

                Color[] getPixels = layers[i].GetPixels(getX, getY, getWidth, getHeight);
                if(layers[i].width >= newTexture.width && layers[i].height >= newTexture.height)
                {
                    adjustedLayers[i] = getPixels;
                }
                else
                {
                    Texture2D sizedLayer = ClearTexture(newTexture.width, newTexture.height);
                    sizedLayer.SetPixels(setX, setY, setWidth, setHeight, getPixels);
                    adjustedLayers[i] = sizedLayer.GetPixels();
                }

            }
        }

        // Set each color layer to alpha% if it isn't already
        for (int i = 0; i < layerColors.Length; i++)
        {
            if(layerColors[i].a < 1)
            {
                layerColors[i] = new Color(layerColors[i].r, layerColors[i].g, layerColors[i].b, 1f);
            }
        }


        // Iterate through each pixel, copying source index to the destination index
        for (int x = 0; x < newTexture.width; x++)
        {
            for (int y = 0; y < newTexture.height; y++)
            {
                int pixelIndex = x + (y * newTexture.width);
                for (int i = 0; i < layers.Length; i++)
                {                    
                    Color srcPixel = adjustedLayers[i][pixelIndex];

                    // Apply layer color if necessary
                    if(srcPixel.r != 0 && srcPixel.a != 0 && i < layerColors.Length)
                    {
                        srcPixel = ApplyColorToPixel(srcPixel, layerColors[i]);
                    }

                    // Normal blending based on alpha
                    if(srcPixel.a == 1)
                    {
                        colorArray[pixelIndex] = srcPixel;
                    }
                    else if (srcPixel.a > 0)
                    {
                        colorArray[pixelIndex] = NormalBlend(colorArray[pixelIndex], srcPixel);
                    }
                    
                }

            }
        }
        newTexture.SetPixels(colorArray);
        newTexture.Apply();

        newTexture.wrapMode = TextureWrapMode.Clamp; // Texture should clamp so that edges are clean
        //newTexture.filterMode = FilterMode.Point;  // Point mode for pixels that a true colors
        return newTexture;
    }

    public Sprite MakeSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);   // Create a sprite from the new texture
    }

    private Color NormalBlend(Color dest, Color src)
    {
        float srcAlpha = src.a;
        float destAlpha = (1 - srcAlpha) * dest.a; // destAlpha (bottom layer) needs to be normalized otherwise the alph number will be greater than 1
        Color destLayer = dest * destAlpha;
        Color srcLayer = src * srcAlpha;    // Insure colors stay between 0 and 1
        return destLayer + srcLayer;    // Return both layers on top of each other
    }

    private Color ApplyColorToPixel(Color pixel, Color applyColor)
    {
        if (pixel.r == 1)
        {
            return applyColor;
        }
        return pixel * applyColor;
    }

    Texture2D ClearTexture(int width, int height)
    {
        Texture2D clearTexture = new Texture2D(width, height);
        Color[] clearPixels = new Color[width * height];
        clearTexture.SetPixels(clearPixels);
        return clearTexture;
    }
}
