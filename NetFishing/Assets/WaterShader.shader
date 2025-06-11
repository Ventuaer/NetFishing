Shader "Custom/WaterEdgeFoam"
{
    Properties
    {
        _MainTex ("Water Texture", 2D) = "white" {}
        _FoamTex ("Foam Texture", 2D) = "white" {}
        _FoamColor ("Foam Color", Color) = (1, 1, 1, 1)
        _FoamIntensity ("Foam Intensity", Range(0, 1)) = 0.5
        _EdgeWidth ("Edge Width", Range(0.1, 1.0)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard alpha:fade

        sampler2D _MainTex;
        sampler2D _FoamTex;
        fixed4 _FoamColor;
        float _FoamIntensity;
        float _EdgeWidth;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // Base water texture
            fixed4 water = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = water.rgb;

            // Calculate foam at edges
            float3 worldNormal = normalize(cross(ddx(IN.worldPos), ddy(IN.worldPos)));
            float edgeFactor = smoothstep(0.0, _EdgeWidth, abs(worldNormal.y));
            fixed4 foam = tex2D(_FoamTex, IN.uv_MainTex) * edgeFactor * _FoamIntensity;

            // Blend foam with water
            o.Albedo = lerp(o.Albedo, _FoamColor.rgb, edgeFactor);
            o.Alpha = water.a + foam.a;
        }
        ENDCG
    }
    FallBack "Transparent/Diffuse"
}
