Shader "Morvaly/LineDrawerShader" {
    Properties {
        _col1 ("col1", Color) = (0.5,0.5,0.5,1)
        _col2 ("col2", Color) = (0.5,0.5,0.5,1)
        _appear ("appear", Float ) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        Pass {

            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
			#pragma target 3.0

            uniform float4 _col1;
            uniform float4 _col2;
            uniform float _appear;

            float BrightnessNearDrawer( float u , float appear ){
	            float f = u-appear;
	            return 1.0+4.0*exp(-10000.0*f*f);
            }
            
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                return o;
            }

            float4 frag(VertexOutput i) : COLOR {

                float4 colorLerpMesh = lerp(_col1,_col2,i.uv0.r);
                float brightnessNearDrawer = BrightnessNearDrawer( i.uv0.r , _appear );
                float4 blackColor = float4(0,0,0,0);

                float3 finalColor = (colorLerpMesh*step(i.uv0.r,_appear)+blackColor*step(_appear,i.uv0.r)).rgb;
                finalColor *= brightnessNearDrawer;
				float4 finalRGBA = fixed4(finalColor,1);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
