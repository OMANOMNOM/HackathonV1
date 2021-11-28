using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using TMPro;

public class AlterPost : MonoBehaviour
{
    public PostProcessingData prd;
    public TextMeshProUGUI RadiatedText;
    private Volume v;
    private Bloom b;
    private Vignette vg;
    // Start is called before the first frame update
    void Start()
    {
        v = GetComponent<Volume>();
        v.profile.TryGet(out b);
        v.profile.TryGet(out vg);
    }

    public void Radiated()
    {
        RadiatedText.gameObject.SetActive(true);
        b.scatter.value = 0.1f;
        vg.color.value = Color.green;
        vg.intensity.value = 0.6f;
        vg.smoothness.value = 0.6f;
    }

    public void NotRadiated()
    {
        RadiatedText.gameObject.SetActive(false);
        b.scatter.value = 0.1f;
        vg.color.value = Color.white;
        vg.intensity.value = 0f;
        vg.smoothness.value = 0.01f;
    }
}
