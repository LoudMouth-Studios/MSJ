#ifndef WALL_REVEAL_INCLUDED
#define WALL_REVEAL_INCLUDED

// Filled in by WallRevealManager.cs every frame
float4 _RevealPoints[8]; // xy = circle centre (world), z = radius, w = strength 0-1
float4 _RevealShape[8];  // x = inner radius (0-1 of radius), y = falloff power
float  _RevealCount;

void WallReveal_float(float3 WorldPos, float MinAlpha, out float AlphaMultiplier)
{
    float cut = 0;
    int count = (int)_RevealCount;
    for (int i = 0; i < 8; i++)
    {
        if (i >= count) break;
        float4 p = _RevealPoints[i];
        float t = saturate(distance(WorldPos.xy, p.xy) / max(p.z, 0.0001));
        float inner = _RevealShape[i].x;
        float r = 1 - saturate((t - inner) / max(1 - inner, 0.0001));
        r = pow(r, max(_RevealShape[i].y, 0.01));
        cut = max(cut, r * p.w);
    }
    AlphaMultiplier = lerp(1, MinAlpha, cut);
}
#endif