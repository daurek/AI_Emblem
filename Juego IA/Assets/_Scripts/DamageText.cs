using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{

    private TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update ()
    {   
        if(textMesh.alpha > 0)
        {
            textMesh.alpha -= Time.deltaTime * 0.4f;
            transform.position += Vector3.up * Time.deltaTime * 40;
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
