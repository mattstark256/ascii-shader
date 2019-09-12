using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ImageEffectCamera : MonoBehaviour
{
    [SerializeField]
    Material mat;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!mat)
        {
            Graphics.Blit(source, destination);
            return;
        }

        Graphics.Blit(source, destination, mat);
    }
}
