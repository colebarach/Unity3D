/*----------------------------------------------------------------------------------------------------------------------------------------------
  Value Noise Library
	
	Author -  Cole Barach
	Created - 2022.05.01
	Updated - 2022.05.05
	
	Function
		-Public collection of value noise functions
    Sources
        -https://www.ronja-tutorials.com/post/025-value-noise/
----------------------------------------------------------------------------------------------------------------------------------------------*/

#ifndef ValueNoise
#define ValueNoise

#include "Assets/Shaders/WhiteNoise.cginc"

float ValueNoise3DTo1D(float3 vectorA) {
    float3 gridVector = floor(vectorA);
    float3 localVector = frac(vectorA);
    float3 interpolatedVector = SquaredEase(localVector);

    float gridXNoise[2];
    [unroll]
    for(int x = 0; x <= 1; x++) {
        float gridYNoise[2];
        [unroll]
        for(int y = 0; y <= 1; y++) {
            float gridZNoise[2];
            [unroll]
            for(int z = 0; z <= 1; z++) {
                gridZNoise[z] = WhiteNoise3DTo1D(gridVector+float3(x,y,z));
            }
            gridYNoise[y] = LinearInterpolate(interpolatedVector.z,gridZNoise[0],gridZNoise[1]);
        }
        gridXNoise[x] = LinearInterpolate(interpolatedVector.y,gridYNoise[0],gridYNoise[1]);
    }
    return LinearInterpolate(interpolatedVector.x,gridXNoise[0],gridXNoise[1]);
}
float3 ValueNoise3DTo3D(float3 vectorA) {
    float3 gridVector = floor(vectorA);
    float3 localVector = frac(vectorA);
    float3 interpolatedVector = SquaredEase(localVector);

    float3 gridXNoise[2];
    [unroll]
    for(int x = 0; x <= 1; x++) {
        float3 gridYNoise[2];
        [unroll]
        for(int y = 0; y <= 1; y++) {
            float3 gridZNoise[2];
            [unroll]
            for(int z = 0; z <= 1; z++) {
                gridZNoise[z] = WhiteNoise3DTo3D(gridVector+float3(x,y,z));
            }
            gridYNoise[y] = LinearInterpolate(interpolatedVector.z,gridZNoise[0],gridZNoise[1]);
        }
        gridXNoise[x] = LinearInterpolate(interpolatedVector.y,gridYNoise[0],gridYNoise[1]);
    }
    return LinearInterpolate(interpolatedVector.x,gridXNoise[0],gridXNoise[1]);
}
float ValueNoise2DTo1D(float2 vectorA) {
    float2 gridVector = floor(vectorA);
    float2 localVector = frac(vectorA);
    float2 interpolatedVector = SquaredEase(localVector);

    float gridXNoise[2];
    [unroll]
    for(int x = 0; x <= 1; x++) {
        float gridYNoise[2];
        [unroll]
        for(int y = 0; y <= 1; y++) {
            gridYNoise[y] = WhiteNoise2DTo1D(gridVector+float2(x,y));
        }
        gridXNoise[x] = LinearInterpolate(interpolatedVector.y,gridYNoise[0],gridYNoise[1]);
    }
    return LinearInterpolate(interpolatedVector.x,gridXNoise[0],gridXNoise[1]);
}

#endif