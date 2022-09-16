Shader "Render Overrides/VHS Image Effect" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        
        _ScrollSpeed      ("Scroll Speed", float)      = 0.1
        
        _BarScale         ("Bar Scale", Vector)        = (0.85,0,0,1)
        _BarOctaves       ("Bar Octaves", int)         = 8
        _BarLacunarity    ("Bar Lacunarity", float)    = 1.2
        
        _SubbarScale      ("Subbar Scale", Vector)     = (32,0,0,0.25)
        _SubbarOctaves    ("Subbar Octaves", int)      = 4
        _SubbarLacunarity ("Subbar Lacunarity", float) = 1

        [HDR]_GrainColor  ("Grain Color", color)       = (1,1,1,1)
        
        _GrainScale       ("Grain Scale", Vector)      = (114,64,12,0.4)
        _GrainOctaves     ("Grain Octaves", int)       = 8
        _GrainLacunarity  ("Grain Lacunarity", float)  = 2
        _GrainOffset      ("Grain Offset", float)      = -0.75

        _RedOffset        ("RedOffset", Vector)        = (0.01,0,    0,0)
        _GreenOffset      ("GreenOffset", Vector)      = (0   ,0.005,0,0)
        _BlueOffset       ("BlueOffset", Vector)       = (0.01,0.005,0,0)
    }
    SubShader {
        Cull Off ZWrite Off ZTest Always
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float  _ScrollSpeed;
            float4 _BarScale;
            int    _BarOctaves;
            float  _BarLacunarity;
            float4 _SubbarScale;
            int    _SubbarOctaves;
            float  _SubbarLacunarity;
            fixed4 _GrainColor;
            float4 _GrainScale;
            int    _GrainOctaves;
            float  _GrainLacunarity;
            float  _GrainOffset;
            float2 _RedOffset;
            float2 _GreenOffset;
            float2 _BlueOffset;

            //---------------------------------------------------------------TEMPORARY ALGORITHM--REPLACE THIS
            //Source https://www.ronja-tutorials.com/post/024-white-noise/
            float Random3DTo1D(float3 coordinates, float3 seed = float3(12.9898, 78.233, 37.719)) {
                float3 clampedCoordinates = sin(coordinates);
                float scalar = dot(clampedCoordinates, seed);
                scalar = frac(sin(scalar)*143758.5453);
                return scalar;
            }
            float Random3DTo3D(float3 coordinates) {
                float3 Vector = float3(Random3DTo1D(coordinates, float3(12.989, 78.233, 37.719)),
                                       Random3DTo1D(coordinates, float3(39.346, 11.135, 83.155)),
                                       Random3DTo1D(coordinates, float3(73.156, 52.235, 09.151)));
                return Vector;
            }
            float Random1DTo1D(float coordinates, float seed = 12.9898) {
                float scalar = sin(coordinates + seed);
                scalar = frac(scalar*143758.5453);
                return scalar;
            }
            float easeIn(float interpolator){
                return interpolator * interpolator;
            }
            float easeOut(float interpolator){
                return 1 - easeIn(1 - interpolator);
            }
            float easeInOut(float interpolator){
                float easeInValue = easeIn(interpolator);
                float easeOutValue = easeOut(interpolator);
                return lerp(easeInValue, easeOutValue, interpolator);
            }
            float GradientNoise1D(float value){
                float fraction = frac(value);
                float interpolator = easeInOut(fraction);

                float previousCellInclination = Random1DTo1D(floor(value)) * 2 - 1;
                float previousCellLinePoint = previousCellInclination * fraction;

                float nextCellInclination = Random1DTo1D(ceil(value)) * 2 - 1;
                float nextCellLinePoint = nextCellInclination * (fraction - 1);

                return lerp(previousCellLinePoint, nextCellLinePoint, interpolator);
            }
            float GradientNoise2D(float2 value){
                float upperLeftCell = Random3DTo1D(float3(floor(value.x), ceil(value.y),0));
                float upperRightCell = Random3DTo1D(float3(ceil(value.x), ceil(value.y),0));
                float lowerLeftCell = Random3DTo1D(float3(floor(value.x), floor(value.y),0));
                float lowerRightCell = Random3DTo1D(float3(ceil(value.x), floor(value.y),0));

                float interpolatorX = easeInOut(frac(value.x));
                float interpolatorY = easeInOut(frac(value.y));

                float upperCells = lerp(upperLeftCell, upperRightCell, interpolatorX);
                float lowerCells = lerp(lowerLeftCell, lowerRightCell, interpolatorX);

                float noise = lerp(lowerCells, upperCells, interpolatorY);
                return noise;
            }
            float GradientNoise3D(float3 value){
                float interpolatorX = easeInOut(frac(value.x));
                float interpolatorY = easeInOut(frac(value.y));
                float interpolatorZ = easeInOut(frac(value.z));

                float cellNoiseZ[2];
                for(int z=0;z<=1;z++){
                    float cellNoiseY[2];
                    for(int y=0;y<=1;y++){
                        float cellNoiseX[2];
                        for(int x=0;x<=1;x++){
                            float3 cell = floor(value) + float3(x, y, z);
                            cellNoiseX[x] = Random3DTo3D(cell);
                        }
                        cellNoiseY[y] = lerp(cellNoiseX[0], cellNoiseX[1], interpolatorX);
                    }
                    cellNoiseZ[z] = lerp(cellNoiseY[0], cellNoiseY[1], interpolatorY);
                }
                float noise = lerp(cellNoiseZ[0], cellNoiseZ[1], interpolatorZ);
                return noise;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 color = tex2D(_MainTex, i.uv);
                for(int w = 0; w < 3; w++) {
                    float2 uv = i.uv;
                    fixed4 aberration = fixed4(0,0,0,0);
                    switch(w) {
                        case 0:
                            uv += _RedOffset;
                            aberration = fixed4(1,0,0,0);
                            break;
                        case 1:
                            uv += _GreenOffset;
                            aberration = fixed4(0,1,0,0);
                            break;
                        case 2:
                            uv += _BlueOffset;
                            aberration = fixed4(0,0,1,0);
                            break;
                    }
                    float scroll = (uv.y+_Time.y*_ScrollSpeed)*_BarScale;
                    float grain = _GrainOffset;
                    for(int x = 0; x < _BarOctaves; x++) {
                        grain += GradientNoise1D(scroll*(x+1))*_BarScale.w/pow(_BarLacunarity,x);
                    }
                    for(int z = 0; z < _SubbarOctaves; z++) {
                        grain += GradientNoise1D(scroll*_SubbarScale.x*(z+1))*_SubbarScale.w/pow(_SubbarLacunarity,z);
                    }
                    for(int y = 0; y < _GrainOctaves; y++) {
                        grain += GradientNoise3D(float3(uv.x,scroll,_Time.y)*_GrainScale*(y+1))*_GrainScale.w/pow(_GrainLacunarity,y);
                    }
                    grain = clamp(grain,0,1);
                    grain *= _GrainColor.a;
                    color += color*aberration*(1-grain) + _GrainColor*aberration*grain;
                }
                return color;
            }
            ENDCG
        }
    }
}