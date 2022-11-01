/*----------------------------------------------------------------------------------------------------------------------------------------------
  White Noise Library
	
	Author -  Cole Barach
	Created - 2022.05.01
	Updated - 2022.05.05
	
	Function
		-Public collection of shader white noise functions
    Sources
        -https://www.ronja-tutorials.com/post/024-white-noise/
----------------------------------------------------------------------------------------------------------------------------------------------*/

#ifndef WhiteNoise
#define WhiteNoise

#include "Assets/Shaders/Interpolation.cginc"

static float3 vectorAlpha = float3(12.989,78.233,37.719);
static float3 vectorBeta  = float3(39.346,11.135,83.155);
static float3 vectorGamma = float3(73.156,52.235,09.151);

float RandomScalarFromVectors(float3 vectorA, float3 vectorB) {
    float3 vectorClamped = sin(vectorA);       //Clamp input to prevent overflow
    float random = dot(vectorClamped,vectorB); //Get Pseudorandom Dot Product
    random = frac(sin(random) * 143758.5453);  //Take fractional part of scaled pseudorandom
    return random;
}

float3 WhiteNoise3DTo3D(float3 vectorA) {
    return float3(  RandomScalarFromVectors(vectorA,vectorAlpha),
                    RandomScalarFromVectors(vectorA,vectorBeta),
                    RandomScalarFromVectors(vectorA,vectorGamma));
}
float2 WhiteNoise3DTo2D(float3 vectorA) {
    return float2(  RandomScalarFromVectors(vectorA,vectorAlpha),
                    RandomScalarFromVectors(vectorA,vectorBeta));
}
float  WhiteNoise3DTo1D(float3 vectorA) {
    return RandomScalarFromVectors(vectorA,vectorAlpha);
}
float3 WhiteNoise2DTo3D(float2 vectorA) {
    return float3(  RandomScalarFromVectors(float3(vectorA.x,vectorA.y,0),vectorAlpha),
                    RandomScalarFromVectors(float3(vectorA.x,vectorA.y,0),vectorBeta),
                    RandomScalarFromVectors(float3(vectorA.x,vectorA.y,0),vectorGamma));
}
float2 WhiteNoise2DTo2D(float2 vectorA) {
    return float2(  RandomScalarFromVectors(float3(vectorA.x,vectorA.y,0),vectorAlpha),
                    RandomScalarFromVectors(float3(vectorA.x,vectorA.y,0),vectorBeta));
}
float  WhiteNoise2DTo1D(float2 vectorA) {
    return RandomScalarFromVectors(float3(vectorA.x,vectorA.y,0),vectorAlpha);
}
float3 WhiteNoise1DTo3D(float A) {
    return float3(  RandomScalarFromVectors(float3(A,0,0),vectorAlpha),
                    RandomScalarFromVectors(float3(A,0,0),vectorBeta),
                    RandomScalarFromVectors(float3(A,0,0),vectorGamma));
}
float2 WhiteNoise1DTo2D(float A) {
    return float2(  RandomScalarFromVectors(float3(A,0,0),vectorAlpha),
                    RandomScalarFromVectors(float3(A,0,0),vectorBeta));
}
float  WhiteNoise1DTo1D(float A) {
    return RandomScalarFromVectors(float3(A,0,0),vectorAlpha);
}

#endif