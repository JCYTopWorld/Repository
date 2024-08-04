using UnityEngine;

public class ShapeGenerator
{
    public MinMax elevationMinMax;
    private INoiseFilter[] noiseFilters;

    private ShapeSettings settings;

    public void UpdateSettings(ShapeSettings settings)
    {
        this.settings = settings;
        noiseFilters = new INoiseFilter[settings.noiseLayers.Length];
        for (var i = 0; i < noiseFilters.Length; i++)
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);
        elevationMinMax = new MinMax();
    }

    public float CalculateUnscaledElevation(Vector3 pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        float elevation = 0;

        if (noiseFilters.Length > 0)
        {
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (settings.noiseLayers[0].enabled) elevation = firstLayerValue;
        }

        for (var i = 1; i < noiseFilters.Length; i++)
            if (settings.noiseLayers[i].enabled)
            {
                var mask = settings.noiseLayers[i].useFirstLayerAsMask ? firstLayerValue : 1;
                elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }

        elevationMinMax.AddValue(elevation);
        return elevation;
    }

    public float GetScaledElevation(float unscaledElevation)
    {
        var elevation = Mathf.Max(0, unscaledElevation);
        elevation = settings.planetRadius * (1 + elevation);
        return elevation;
    }
}