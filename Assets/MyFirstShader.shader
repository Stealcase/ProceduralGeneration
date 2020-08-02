Shader "Benjamin/MyFirstShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _color ("Color", Color) = (1,1,1,1)
        _colorEmission ("Color", Color) = (1,1,1,1)
        _colorNormal ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
            CGPROGRAM
            #pragma surface surf Lambert
            
            struct Input
            {
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _color;
            fixed4 _colorEmission;
            fixed4 _colorNormal;
            
            void surf (Input IN, inout SurfaceOutput o)
            {
            
            o.Albedo.rgb = _colorEmission.rgb;
            o.Emission = _color.rgb;
            o.Normal = _colorNormal.rgb;
            }
            ENDCG
        }
    }
