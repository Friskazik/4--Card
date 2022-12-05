Shader "Custom/SpriteShine(horisontal)"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" { }
        _Color ("Tint", Color) = (1, 1, 1, 1)

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
        [NoScaleOffset]_Texture0 ("Texture 0", 2D) = "white" { }
        _Texture0Scale ("Texture Scale", float) = 1
        _AnimationTanScaleH ("Animation Tan H", Float) = 1
        _AnimationTanScaleW ("Animation Tan W", Float) = 1
        _Speed("Speed", Float) = 0.1
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest LEqual
        Blend One One, One One

        Pass
        {
            Name "Default"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex: POSITION;
                float4 color: COLOR;
                float2 texcoord: TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex: SV_POSITION;
                fixed4 color: COLOR;
                half2 texcoord: TEXCOORD0;
                float4 worldPosition: TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
                float4 ase_texcoord2: TEXCOORD2;
            };

            uniform fixed4 _Color;
            // uniform fixed4 _TextureSampleAdd;
            uniform float4 _ClipRect;
            uniform sampler2D _MainTex;
            uniform sampler2D _Texture0;
            uniform float _Texture0Scale;
            uniform float _AnimationTanScaleH;
            uniform float _AnimationTanScaleW;
            uniform float _Speed;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = IN.vertex;
                float3 ase_worldPos = mul(unity_ObjectToWorld, IN.vertex).xyz;
                OUT.ase_texcoord2.xyz = ase_worldPos;


                //setting value to unused interpolator channels and avoid initialization warnings
                OUT.ase_texcoord2.w = 0;

                OUT.worldPosition.xyz += float3(0, 0, 0);
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = IN.texcoord;

                OUT.color = IN.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN): SV_Target
            {
                float flarePosition = _AnimationTanScaleH * tan(_Time.g * _Speed * _AnimationTanScaleW);
                float4 appendResult60 = (float4(0.0, flarePosition, 0.0, 0.0));
                float3 ase_worldPos = IN.ase_texcoord2.xyz;
                // float2 rotator58 = (appendResult60 + float4(ase_worldPos, 0.0)).xy - float2(0, 0);
                float cos58 = cos(-0.8);
                float sin58 = sin(-0.8);
                float2 rotator58 = mul(
                    ((appendResult60 + float4(ase_worldPos, 0.0)) * 0.15).xy - float2(0, 0),
                    float2x2(cos58, -sin58, sin58, cos58)
                ) + float2(0, 0);

                half4 color = ((tex2D(_Texture0, rotator58 * _Texture0Scale) * IN.color) * 1.59);
                fixed4 col = tex2D(_MainTex, IN.texcoord);
                col.a *= IN.color.a;
                color *= col.a;

                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

                #ifdef UNITY_UI_ALPHACLIP
                    clip(color.a - 0.001);
                #endif

                return color;
            }
            ENDCG

        }
    }
    CustomEditor "ASEMaterialInspector"
}
