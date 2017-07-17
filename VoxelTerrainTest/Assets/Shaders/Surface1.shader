// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//Attempt at making a shader for the procedurally generated terrain:

Shader "Custom/Surface1" 
{
	Properties 
	{
		_MainTex1 ("Base1 (RGB)", 2D) = "white" {}
		_MainTex2 ("Base2 (RGB)", 2D) = "white" {}
		_MainTex3 ("Base3 (RGB)", 2D) = "white" {}
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Pass 
		{
			CGPROGRAM

				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
			
				struct v2f 
				{
					float4 pos : SV_POSITION;
					float2 uvs_Tex : TEXCOORD0;
					float3 normals : NORMAL;
				};
			
				float4 _MainTex_ST;

				//Vertex shader:
				v2f vert(appdata_base v) 
				{
					v2f o;

					o.pos = UnityObjectToClipPos(v.vertex);
					//o.uvs_Tex = TRANSFORM_TEX(v.texcoord, _MainTex);
					o.uvs_Tex = v.texcoord;
					o.normals = v.normal;

					return o;
				}
			
				sampler2D _MainTex1;
				sampler2D _MainTex2;
				sampler2D _MainTex3;

				//Fragment shader:
				float4 frag(v2f IN) : COLOR 
				{
					half4 colorA = half4(0, 1, 0, 1); //tex2D (_MainTex1, IN.uvs_Tex);
					half4 colorB = half4(1, 0, 0, 1); //tex2D (_MainTex2, IN.uvs_Tex);

					float x = dot(IN.normals, float3(0,1,0)); 
					x = (x + 1) * 0.5;
					float3 colorF = lerp(colorA, colorB, x);

					return float4(colorF.rgb, 1);
				}

			ENDCG
		}
	}
}