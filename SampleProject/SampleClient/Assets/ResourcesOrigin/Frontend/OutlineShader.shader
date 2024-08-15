Shader "Custom/ScaledThinVisibleOutlineCircle"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineThickness ("Outline Thickness", Range(0,0.1)) = 0.02
        _ScaleFactor ("Scale Factor", Range(0.01, 10)) = 1.0
    }
    SubShader
    {
        Tags {"Queue" = "Transparent"}
        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _OutlineThickness;
            float4 _OutlineColor;
            float _ScaleFactor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = (v.uv * 2.0 - 1.0) / _ScaleFactor; // Shift UVs from [0,1] to [-1,1] range and scale them
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float dist = length(i.uv);
                
                // Draw only the outline
                float outline = smoothstep(0.5 - _OutlineThickness, 0.5, dist) - smoothstep(0.5, 0.5 + _OutlineThickness, dist);

                return _OutlineColor * outline;
            }
            ENDCG
        }
    }
}
