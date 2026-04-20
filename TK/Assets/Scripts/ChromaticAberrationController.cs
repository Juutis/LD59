using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChromaticAberrationController : MonoBehaviour
{
    public static ChromaticAberrationController Instance;
    public Volume volume;
    private ChromaticAberration chromatic;

    private float triggered = -1;
    private float duration = 0.8f;
    private float intensity = 1.0f;

    void Awake() {
        Instance = this;
    }

    void Start()
    {
        if (volume.profile.TryGet(out chromatic))
        {
            chromatic.intensity.overrideState = true;
        }
    }

    void Update()
    {
        if (triggered > 0 && triggered > Time.time - duration) {
            var t = (Time.time - triggered) / duration;
            var value = Mathf.Lerp(intensity, 0.0f, t);
            chromatic.intensity.value = value;
        } else {
            chromatic.intensity.value = 0.0f;
        }
    }

    public void Trigger(float intensity = 1.0f) {
        this.intensity = intensity;
        triggered = Time.time;
    }
}
