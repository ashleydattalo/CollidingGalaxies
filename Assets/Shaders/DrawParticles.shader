Shader "DrawParticles"
{
	SubShader 
	{
		Pass 
		{
			Blend SrcAlpha one

			CGPROGRAM
			#pragma target 5.0
			
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			// Particle's data
			struct Particle
			{
				float3 position;
				float3 velocity;
				float3 color;
				float mass;
			};
			
			// Pixel shader input
			struct PS_INPUT
			{
				float4 position : SV_POSITION;
				float4 color : COLOR;
			};
			
			// Particle's data, shared with the compute shader
			StructuredBuffer<Particle> particleBuffer;
			
			// Properties variables
			uniform float4 _ColorLow;
			uniform float4 _ColorHigh;
			uniform float _HighSpeedValue;
			
			// Vertex shader
			PS_INPUT vert(uint vertex_id : SV_VertexID, uint instance_id : SV_InstanceID)
			{
				PS_INPUT o = (PS_INPUT)0;

				// Color
				o.color = float4(particleBuffer[instance_id].color, 1.0f);

				// Position	
				o.position = UnityObjectToClipPos(float4(particleBuffer[instance_id].position, 1.0f));

				//o.position.xyz += 5.0f;

				return o;
			}

			// Pixel shader
			float4 frag(PS_INPUT i) : COLOR
			{
				return i.color;
				//return float4(1.0f, 0.1f, 0.1f, 1.0f);
			}
			
			ENDCG
		}
	}

	Fallback Off
}
