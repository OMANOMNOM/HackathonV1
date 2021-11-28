using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class AlterPost : MonoBehaviour
{
    public PostProcessingData prd;
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
        b.scatter.value = 0.1f;
        vg.color.value = Color.green;
        vg.intensity.value = 0.271f;
        vg.smoothness.value = 0.58f;
    }

    public void NotRadiated()
    {
        b.scatter.value = 0.1f;
        vg.color.value = Color.white;
        vg.intensity.value = 0f;
        vg.smoothness.value = 0.01f;
    }
}
