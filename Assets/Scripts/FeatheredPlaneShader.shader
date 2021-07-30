Shader "Unlit/FeatheredPlaneShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_TexTintColor("Texture Tint Color", Color) = (1,1,1,1)
		_PlaneColor("Plane Color", Color) = (1,1,1,1)
	}
		SubShader
	{
		   Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		   LOD 100
		   Blend SrcAlpha OneMinusSrcAlpha
		   ZWrite Off
			Pass
			{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			struct appdata
			{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
			float3 uv2 : TEXCOORD1;
			};
			struct v2f
			{
			float2 uv : TEXCOORD0;
			float3 uv2 : TEXCOORD1;
			float4 vertex : SV_POSITION;
			};
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _TexTintColor;
			fixed4 _PlaneColor;
			float _ShortestUVMapping;
			v2f vert(appdata v)
			{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			o.uv2 = v.uv2;
			return o;
			}
			fixed4 frag(v2f i) : SV_Target
			{
			fixed4 col = tex2D(_MainTex, i.uv);
			col = lerp(_PlaneColor, col, col.a);
			col.a *= 1 - smoothstep(1, _ShortestUVMapping, i.uv2.x);
			return col;
			}
			ENDCG
		}
	}
}
