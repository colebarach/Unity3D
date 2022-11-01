/*----------------------------------------------------------------------------------------------------------------------------------------------
  Interpolation Library
	
	Author -  Cole Barach
	Created - 2022.05.01
	Updated - 2022.05.05
	
	Function
		-Public collection of interpolation functions
    Sources
        -Interpolation - https://en.wikipedia.org/wiki/Hermite_interpolation
                         https://en.wikipedia.org/wiki/Polynomial_interpolation
----------------------------------------------------------------------------------------------------------------------------------------------*/

#ifndef Interpolation
#define Interpolation

float LinearInterpolate(float x, float a, float b) {
    return (b-a)*x + a;
}
float2 LinearInterpolate(float2 x, float a, float b) {
    return float2(LinearInterpolate(x.x,a,b),LinearInterpolate(x.y,a,b));
}
float3 LinearInterpolate(float3 x, float a, float b) {
    return float3(LinearInterpolate(x.x,a,b),LinearInterpolate(x.y,a,b),LinearInterpolate(x.z,a,b));
}
float3 LinearInterpolate(float3 x, float3 a, float3 b) {
    return float3(LinearInterpolate(x.x,a.x,b.x),LinearInterpolate(x.y,a.y,b.y),LinearInterpolate(x.z,a.z,b.z));
}
float CubicHermiteSpline(float x, float a, float b) {
    return (b-a)*(3*x*x - 2*x*x*x) + a;
}
float2 CubicHermiteSpline(float2 x, float a, float b) {
    return float2(CubicHermiteSpline(x.x,a,b),CubicHermiteSpline(x.y,a,b));
}
float3 CubicHermiteSpline(float3 x, float a, float b) {
    return float3(CubicHermiteSpline(x.x,a,b),CubicHermiteSpline(x.y,a,b),CubicHermiteSpline(x.z,a,b));
}
float QuinticHermiteSpline(float x, float a, float b) {
    return (b-a)*(6*x*x*x*x*x - 15*x*x*x*x + 10*x*x*x) + a;
}
float2 QuinticHermiteSpline(float2 x, float a, float b) {
    return float2(QuinticHermiteSpline(x.x,a,b),QuinticHermiteSpline(x.y,a,b));
}
float3 QuinticHermiteSpline(float3 x, float a, float b) {
    return float3(QuinticHermiteSpline(x.x,a,b),QuinticHermiteSpline(x.y,a,b),QuinticHermiteSpline(x.z,a,b));
}
float SquaredEaseIn(float x) {
    return x*x;
}
float SquaredEaseOut(float x) {
    return 1-SquaredEaseIn(1-x);
}
float SquaredEase(float x) {
    return LinearInterpolate(x,SquaredEaseIn(x),SquaredEaseOut(x));
}
float2 SquaredEase(float2 x) {
    return float2(SquaredEase(x.x),SquaredEase(x.y));
}
float3 SquaredEase(float3 x) {
    return float3(SquaredEase(x.x),SquaredEase(x.y),SquaredEase(x.z));
}

#endif