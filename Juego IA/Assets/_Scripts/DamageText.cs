using TMPro;
using UnityEngine;

/// <summary>
/// Text that shows the damage dealth and dissapears while going up
/// </summary>
public class DamageText : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update ()
    {   
        // Hasn't dissapeared already
        if(textMesh.alpha > 0)
        {   
            // Reduce alpha and make it go up
            textMesh.alpha -= Time.deltaTime * 0.4f;
            transform.position += Vector3.up * Time.deltaTime * 40;
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
