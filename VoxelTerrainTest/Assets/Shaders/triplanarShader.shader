Shader "Custom/triplanarShader" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_xPlane ("Albedo wall 1 (RGB)", 2D) = "white" {}
		_yPlane ("Albedo floor (RGB)", 2D) = "white" {}
		_zPlane ("Albedo wall 2 (RGB)", 2D) = "white" {}

		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Scale ("Texture Scale", Float) = 0.1
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input 
		{
			float3 worldNormal;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		sampler2D _xPlane;
		sampler2D _yPlane;
		sampler2D _zPlane;
		float _Scale;

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			half3 blending = abs(IN.worldNormal);
			blending = normalize(max(blending, 0.00001));

			float b = (blending.x + blending.y + blending.z);
			blending /= half3(b, b, b);

			half4 xaxis = tex2D(_xPlane, IN.worldPos.yz * _Scale);
			half4 yaxis = tex2D(_yPlane, IN.worldPos.xz * _Scale);
			half4 zaxis = tex2D(_zPlane, IN.worldPos.xy * _Scale);

			half4 tex = (xaxis * blending.r + yaxis * blending.g + zaxis * blending.b);

			o.Albedo = tex.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
