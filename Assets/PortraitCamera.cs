using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PortraitCamera : MonoBehaviour
{
    [SerializeField] Camera m_portraitCamera;
    [SerializeField] Image m_destinationTexture;
    [SerializeField] Transform m_characterSpawnLocation;
    [SerializeField] GameObject m_dummy;

    private Texture2D portraitTexture;
    private RenderTexture rt;
    public void TakePortrait(CharacterBase character)
    {
        PlaceCharacter(character);

        m_portraitCamera.aspect = 1f;

        if (rt == null)
        {
            rt = new RenderTexture(1024, 1024, 24);
        }
        m_portraitCamera.targetTexture = rt;
        RenderTexture.active = rt;
        m_portraitCamera.Render();
        StartCoroutine(SaveCameraView());
    }

    public IEnumerator SaveCameraView()
    {
        yield return new WaitForEndOfFrame();
        portraitTexture = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        RenderTexture.active = rt;
        portraitTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        RenderTexture.active = null;
        portraitTexture.Apply();
        //byte[] bytes = portraitTexture.EncodeToPNG();
        //File.WriteAllBytes(Application.dataPath + "/test.png", bytes);
        m_destinationTexture.sprite = Sprite.Create(portraitTexture, new Rect(0, 0, portraitTexture.width, portraitTexture.height), new Vector2(0.5f, 0.5f));
    }

    private void PlaceCharacter(CharacterBase character)
    {
        m_dummy.GetComponent<MeshFilter>().mesh = character.GetComponent<MeshFilter>().mesh;
        m_dummy.GetComponent<MeshRenderer>().material = character.GetComponent<MeshRenderer>().material;
    }
}