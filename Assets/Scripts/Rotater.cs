using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotater : MonoBehaviour
{
    [SerializeField]
    private InputField angleinput;
    [SerializeField]
    private Image image;


    private Texture2D originalTexture;

    private void Start()
    {
        originalTexture = Resources.Load("Smiley") as Texture2D;
        image.sprite = Sprite.Create(originalTexture, new Rect(0.0f, 0.0f, originalTexture.width, originalTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
    

    public void OnRotateClick()
    {
        Texture2D rotatedTexture = new Texture2D(originalTexture.width, originalTexture.height);

        float angle = float.Parse(angleinput.text);

        Color32[] pix3 = RotateImage(originalTexture, (Mathf.PI/180*angle));
        rotatedTexture.SetPixels32(pix3);
        rotatedTexture.Apply();
        image.sprite = Sprite.Create(rotatedTexture, new Rect(0.0f, 0.0f, rotatedTexture.width, rotatedTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
         
    }

    static Color32[] RotateImage(Texture2D originTexture, float angleInRadians)
    {
        /*
        Implemented based on rotation matrix
        reference: https://en.wikipedia.org/wiki/Rotation_matrix

        from rotation matrix:
        x' = xcos(a)-ysin(a)
        y' = xsin(a)+ycos(a)

        we are deriving the reverse of it
        
        x = x'cos(a)+y'sin(a)
        y = y'cos(a)-x'sin(a) 
        */

        Color32[] originalImageArray = originTexture.GetPixels32();
        double sn = Mathf.Sin(angleInRadians);
        double cs = Mathf.Cos(angleInRadians);
         
        Color32[] rotatedArray = originTexture.GetPixels32();
        int W = originTexture.width;
        int H = originTexture.height;
        int xc = W/2;
        int yc = H/2;
        
        for (int j=0; j<H; j++)
        {
            for (int i=0; i<W; i++)
            {
                rotatedArray[j*W+i] = new Color32(0,0,0,0);
                int x = (int)(cs*(i-xc)+sn*(j-yc)+xc);
                int y = (int)(-sn*(i-xc)+cs*(j-yc)+yc);
                /*
                xc and yc is subtracted and added to the final value becuse
                rotation is done with respect to center.
                */
                if ((x>-1) && (x<W) &&(y>-1) && (y<H))
                { 
                    rotatedArray[j*W+i]=originalImageArray[y*W+x];
                }
             }
         }
         return rotatedArray;
     }
}
