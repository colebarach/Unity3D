/*----------------------------------------------------------------------------------------------------------------------------------------------
  Fresnel Library
	
	Author -  Cole Barach
	Created - 2022.05.01
	Updated - 2022.05.05
	
	Function
		-Public collection of fresnel approximation functions
    Sources
        -https://en.wikipedia.org/wiki/Schlick%27s_approximation
----------------------------------------------------------------------------------------------------------------------------------------------*/

float SchlickFresnel(float cosineTheta, float normalReflectance) {
    return normalReflectance + (1-normalReflectance)*pow((1-cosineTheta),5);
}
float SchlickFresnel(float cosineTheta, float indexOfRefraction1, float indexOfRefraction2) {
    float normalReflectance = ((indexOfRefraction1-indexOfRefraction2)/(indexOfRefraction1+indexOfRefraction2))*((indexOfRefraction1-indexOfRefraction2)/(indexOfRefraction1+indexOfRefraction2));
    return normalReflectance + (1-normalReflectance)*pow((1-cosineTheta),5);
}

float SchlickFresnelPower(float3 normal, float3 viewDirection, float power) {
    float cosineTheta = saturate(dot(normalize(normal), normalize(viewDirection)));
    return pow(cosineTheta,power);
}
