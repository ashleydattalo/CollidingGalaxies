Shader "BWDiffuse" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "red" {}
		_bwBlend ("Black & White blend", Range (0, 1)) = 0
	}
	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform float _bwBlend;

			float4 frag(v2f_img i) : COLOR {
				float4 result;
				float4 c = tex2D(_MainTex, i.uv );
					
				//float lum = c.r*.3 + c.g*.59 + c.b*.11;
				//float3 bw = float3( lum, lum, lum ); 
				
				//float4 result = c;
				//result.rgb = lerp(c.rgb, bw, _bwBlend);


				if (c.r == 0 && c.g == 0 && c.b == 0) {
					result = float4(.2f, .2f, .2f, 1.0f);
				}
				else {
					result = float4(0.4f, 0.9f, 0.1f, 1.0f);
				}

				return result;
			}
			ENDCG
		}
	}
}