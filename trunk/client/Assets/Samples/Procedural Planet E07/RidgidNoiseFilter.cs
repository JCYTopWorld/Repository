using UnityEngine;

public class RidgidNoiseFilter : INoiseFilter
{
    private readonly Noise noise = new();

    private readonly NoiseSettings.RidgidNoiseSettings settings;

    public RidgidNoiseFilter(NoiseSettings.RidgidNoiseSettings settings)
    {
        this.settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        var frequency = settings.baseRoughness;
        float amplitude = 1;
        float weight = 1;

        for (var i = 0; i < settings.numLayers; i++)
        {
            var v = 1 - Mathf.Abs(noise.Evaluate(point * frequency + settings.centre));
            v *= v;
            v *= weight;
            weight = Mathf.Clamp01(v * settings.weightMultiplier);

            noiseValue += v * amplitude;
            frequency *= settings.roughness;
            amplitude *= settings.persistence;
        }

        noiseValue = noiseValue - settings.minValue;
        return noiseValue * settings.strength;
    }
}