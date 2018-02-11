﻿Shader "Custom/Outline" 
{
	Properties 
	{
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Color("Main Color", Color)= (0.5,0.5,0.5,1)
		_OutlineColour("Outline colour", Color) = (0,0,0,1)
		_OutlineWidth("Outline width", Range(1.0,20)) = 1.01
		
	}
	CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	struct v2f
	{
		float4 pos : POSITION;
		float3 normal : NORMAL;
	};
	
	float4 _OutlineColour;
	float4 _OutlineWidth;

	v2f vert(appdata v)
	{
		v.vertex.xyz *= _OutlineWidth;

		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		return o;
	}
		ENDCG

	
	SubShader 
	{
			Tags{"Queue"= "Transparent"}
		Pass	//Outline
		{
			ZWrite Off
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			half4 frag(v2f i ): COLOR
			{
				return _OutlineColour;
			}


			ENDCG
				
		}
		Pass	//Normals
		{
			ZWrite On
				
			Material
			{
				Diffuse[_Color]
				Ambient[_Color]
			}
			Lighting On

			SetTexture[_MainTex]
			{
				ConstantColor[_Color]
			}

			SetTexture[_MainTex]
			{
				Combine previous * primary DOUBLE
			}

		}
	}
	
}
