using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
    private readonly Noise noise = new();

    private readonly NoiseSettings.SimpleNoiseSettings settings;

    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings)
    {
        this.settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        var frequency = settings.baseRoughness;
        float amplitude = 1;

        for (var i = 0; i < settings.numLayers; i++)
        {
            var v = noise.Evaluate(point * frequency + settings.centre);
            noiseValue += (v + 1) * .5f * amplitude;
            frequency *= settings.roughness;
            amplitude *= settings.persistence;
        }

        noiseValue = noiseValue - settings.minValue;
        return noiseValue * settings.strength;
    }
}