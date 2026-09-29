#ifndef WALL_REVEAL_INCLUDED
#define WALL_REVEAL_INCLUDED

// Filled in by WallRevealManager.cs every frame
float4 _RevealPoints[8]; // xy = circle centre (world), z = radius
float4 _RevealShape[8];  // x = inner radius (0-1 of radius), y = falloff power
float  _RevealCount;

// WallA/WallB = how open THIS wall is for character 0-3 and 4-7 (set per wall by the manager)
void WallReveal_float(float3 WorldPos, float MinAlpha, float4 WallA, float4 WallB, out float AlphaMultiplier)
{
    AlphaMultiplier = 1;
    float cut = 0;
    float strength[8] = { WallA.x, WallA.y, WallA.z, WallA.w, WallB.x, WallB.y, WallB.z, WallB.w };

    [unroll]
    for (int i = 0; i < 8; i++)
    {
        if (i < (int)_RevealCount)
        {
            float4 p = _RevealPoints[i];
            float t = saturate(distance(WorldPos.xy, p.xy) / max(p.z, 0.0001));
            float inner = _RevealShape[i].x;
            float r = 1 - saturate((t - inner) / max(1 - inner, 0.0001));
            r = pow(r, max(_RevealShape[i].y, 0.01));
            cut = max(cut, r * strength[i]);
        }
    }

    AlphaMultiplier = lerp(1, MinAlpha, cut);
}
#endif