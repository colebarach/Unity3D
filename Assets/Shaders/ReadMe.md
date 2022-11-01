# Shader Package
This package contains general purpose HLSL code. The code used is derived from a variety of open-source algorithms, which has been adapted for use with the package.
## Fresnel
Implementation of Schlick's Fresnel approximation, used for performing realistic reflections in an efficient manner. Further optimization may be performed by replacing the power function, although this will decrease readability.
Sources:
-https://en.wikipedia.org/wiki/Schlick%27s_approximation
## Interpolation
Library of interpolation functions. Includes the following.
- Linear Interpolation
- Cubic Hermite Spline Interpolation
- Quintic Hermite Spline Interpolation
- Squared Easing In/Out
All functions allow overriding for 2D and 3D vectors.
Sources:
- https://en.wikipedia.org/wiki/Hermite_interpolation
- https://en.wikipedia.org/wiki/Polynomial_interpolation
## White Noise
Library of white noise and pseudo-random number functions
Sources:
- https://www.ronja-tutorials.com/post/024-white-noise/
## Value Noise
Library of value noise functions, for usage when a perlin-like noise function is required.
Sources:
- https://www.ronja-tutorials.com/post/025-value-noise/