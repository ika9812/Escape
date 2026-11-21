
Shader "Custom/Outline"
{

    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _Threshold ("Threshold", Range(0.001, 1)) = 0.05
        _OutlineWidth ("Outline Width", Range(1, 5)) = 1
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            ZTest Always
            ZWrite Off
            Cull Off

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float4 _MainTex_TexelSize;
            float4 _OutlineColor;
            float _Threshold;
            float _OutlineWidth;

            struct Attributes
            {
                uint vertexID : SV_VertexID;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;
                 
                output.positionCS =
                    GetFullScreenTriangleVertexPosition(
                        input.vertexID
                    );

                output.uv =
                    GetFullScreenTriangleTexCoord(
                        input.vertexID
                    );

                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float2 uv = input.uv;

                float2 offset =
                    _MainTex_TexelSize.xy * _OutlineWidth;

                float depthCenter =
                    SampleSceneDepth(uv);

                float depthRight =
                    SampleSceneDepth(uv + float2(offset.x, 0));

                float depthLeft =
                    SampleSceneDepth(uv - float2(offset.x, 0));

                float depthUp =
                    SampleSceneDepth(uv + float2(0, offset.y));

                float depthDown =
                    SampleSceneDepth(uv - float2(0, offset.y));

                float depthDiff =
                    abs(depthCenter - depthRight) +
                    abs(depthCenter - depthLeft) +
                    abs(depthCenter - depthUp) +
                    abs(depthCenter - depthDown);

                float edge =
                    step(_Threshold, depthDiff);

                half4 color =
                    SAMPLE_TEXTURE2D(
                        _MainTex,
                        sampler_MainTex,
                        uv
                    );

                color.rgb = lerp(
                    color.rgb,
                    _OutlineColor.rgb,
                    edge * _OutlineColor.a
                );

                return color;
            }

            ENDHLSL
        }
    }
}
