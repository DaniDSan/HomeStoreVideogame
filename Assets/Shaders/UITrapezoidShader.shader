Shader "UI/TrapezoidShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TopWidth ("Top Width", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _TopWidth;

            v2f vert (appdata_t v)
            {
                v2f o;
                float aspect = 1.0; // Ajustar si es necesario

                // Deformar los vértices superiores
                if (v.vertex.y > 0)
                {
                    float scale = lerp(1.0, _TopWidth, v.vertex.y);
                    v.vertex.x *= scale;
                }

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
