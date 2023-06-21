Shader "YouTube/FresnelGlow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _DistortTex ("Distort Texture", 2D) = "white" {}
        _DistortIntensity("Distort Intensity", Range(0,10)) = 0

        _FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
        _FresnelIntensity("Fresnel Intensity", Range(0,10)) = 0
        _FresnelRamp("Fresnel Ramp", Range(0,10)) = 0

        _InvFresnelColor ("Inv Fresnel Color", Color) = (1,1,1,1)
        _InvFresnelIntensity("Inv Fresnel Intensity", Range(0,10)) = 0
        _InvFresnelRamp("Inv Fresnel Ramp", Range(0,10)) = 0

        [Toggle] NORMAL_MAP ("Normal Mapping", float) = 0
        _NormalMap ("Normal Map", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile __ NORMAL_MAP_ON
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float3 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
                float3 tangent : TEXCOORD3;
                float3 bitangent : TEXCOORD4;
            };

            sampler2D _MainTex, _NormalMap, _DistortTex;
            float4 _MainTex_ST, _FresnelColor, _InvFresnelColor;

            float _FresnelIntensity, _FresnelRamp, _DistortIntensity, _InvFresnelRamp, _InvFresnelIntensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);

                #if NORMAL_MAP_ON
                    o.tangent = UnityObjectToWorldDir(v.tangent);
                    o.bitangent = cross(o.tangent, o.normal);
                #endif

                o.viewDir = normalize(WorldSpaceViewDir(v.vertex));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float distort = tex2D(_DistortTex, i.uv + _Time.xx).r;

                float3 finalNormal = i.normal;
                #if NORMAL_MAP_ON
                    float3 normalMap = UnpackNormal(tex2D(_NormalMap, i.uv));
                    finalNormal = normalMap.r * i.tangent + normalMap.g * i.bitangent + normalMap.b * i.normal;
                #endif

                float fresnelAmount = 1 - max(0,dot(finalNormal, i.viewDir));
                fresnelAmount *= distort * _DistortIntensity;
                fresnelAmount = pow(fresnelAmount, _FresnelRamp) * _FresnelIntensity;
                float3 fresnelColor = fresnelAmount * _FresnelColor;
    
                float invfresnelAmount = max(0,dot(finalNormal, i.viewDir));
                invfresnelAmount *= distort * _DistortIntensity;
                invfresnelAmount = pow(invfresnelAmount, _InvFresnelRamp) * _InvFresnelIntensity;
                float3 invfresnelColor = invfresnelAmount * _InvFresnelColor;
                float3 finalColor = fresnelColor + invfresnelColor;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
}