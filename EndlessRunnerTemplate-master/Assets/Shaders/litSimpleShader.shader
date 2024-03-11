Shader"Mobile/SimpleLit"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

#include "Lighting.cginc"

struct appdata
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float2 uv : TEXCOORD0;
    float3 worldPos : TEXCOORD1;
    float3 worldNormal : TEXCOORD2;
    float4 vertex : SV_POSITION;
                UNITY_FOG_COORDS(3)
};

sampler2D _MainTex;
float4 _MainTex_ST;
float4 _Color;

v2f vert(appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
    o.worldNormal = UnityObjectToWorldNormal(v.vertex.xyz);
    UNITY_TRANSFER_FOG(o, o.vertex);
    return o;
}
            
fixed4 frag(v2f i) : SV_Target
{
                // Sample the texture color
    fixed4 col = tex2D(_MainTex, i.uv) * _Color;

                // Simple per-pixel lighting
    float3 ambientLight = UNITY_LIGHTMODEL_AMBIENT.xyz;
    float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
    float3 normal = normalize(i.worldNormal);
    float ndotl = max(0, dot(normal, lightDir));
    float3 diffuse = _LightColor0.rgb * ndotl;

                // Apply lighting
    col.rgb *= (ambientLight + diffuse);
    UNITY_APPLY_FOG(i.fogCoord, col);
    return col;
}
            ENDCG
        }
    }
FallBack "Diffuse"
}
