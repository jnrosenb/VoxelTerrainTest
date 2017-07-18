Shader "Custom/triplanar2"
{

	Properties 
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTexWall2 ("Wall Side Texture (RGB)", 2D) = "surface" {}
		_MainTexWall1 ("Wall Front Texture (RGB)", 2D) = "surface" {} 
		_MainTexFloor ("Floor Texture", 2D) = "surface" {} 
		_Scale ("Texture Scale", Float) = 0.1
	}

	SubShader 
	{

		Tags { "RenderType"="Opaque" }

		CGPROGRAM
		#pragma surface surf Lambert

		struct Input 
		{
			float3 worldNormal;
			float3 worldPos;
		};

		sampler2D _MainTexWall1;
		sampler2D _MainTexWall2;
		sampler2D _MainTexFloor;
		float4 _Color;
		float _Scale;
		void surf (Input IN, inout SurfaceOutput o) 
		{
			float2 UV;
			fixed4 c;

			if(abs(IN.worldNormal.x) > 0.4) 
			{
				UV = IN.worldPos.yz; // side
				c = tex2D(_MainTexWall2, UV * _Scale); // use WALLSIDE texture
			} 
			else if(abs(IN.worldNormal.z) > 0.4) 
			{
				UV = IN.worldPos.xy; // front
				c = tex2D(_MainTexWall1, UV * _Scale); // use WALL texture
			} 
			else
			{
				UV = IN.worldPos.xz; // top
				c = tex2D(_MainTexFloor, UV * _Scale); // use FLR texture
			}

			o.Albedo = c.rgb * _Color;
		}

		ENDCG
	} 

	Fallback "VertexLit"
}