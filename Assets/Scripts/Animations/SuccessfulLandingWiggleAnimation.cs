using System.Collections;
using TMPro;
using UnityEngine;

public class SuccessfulLandingTextWiggleAnimation : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    void OnEnable()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh != null)
        {
            StartCoroutine(WiggleLetters());
        }
    }

    IEnumerator WiggleLetters()
    {
        while (true)
        {
            textMesh.ForceMeshUpdate();
            var textInfo = textMesh.textInfo;

            // Wiggle each letter
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                int vertIndex = charInfo.vertexIndex;
                int matIndex = charInfo.materialReferenceIndex;
                Vector3[] verts = textInfo.meshInfo[matIndex].vertices;

                // Smooth wave movement (side to side + up down)
                float time = Time.time + i * 0.2f;  // Offset each letter
                Vector3 wiggle = new Vector3(
                    Mathf.Sin(time * 3f) * 2f,      // Side to side
                    Mathf.Cos(time * 2.5f) * 2f,    // Up and down
                    0f
                );

                // Apply to all 4 corners
                verts[vertIndex + 0] += wiggle;
                verts[vertIndex + 1] += wiggle;
                verts[vertIndex + 2] += wiggle;
                verts[vertIndex + 3] += wiggle;
            }

            // Update the mesh
            textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);

            yield return null;  // Update every frame for smoothness
        }
    }
}