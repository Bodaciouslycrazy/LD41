Shader "Bodie/Screenspace" {
	Properties {
		_Color ("Color", Color) = (.9, .9, .9, 1)
		_Wrap ("Wrap", Float) = 20
		_ScreenTex ("Screen Tex", 2D) = "white" {}
	}
	
	SubShader {
		Lighting Off
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }

		CGPROGRAM
		
		#pragma surface surf Unlit alpha
		#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
			float4 screenPos;
		};

		sampler2D _ScreenTex;
		fixed4 _Color;
		float _Wrap;
		
		void surf( Input IN, inout SurfaceOutput o)
		{
			//o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			screenUV *=  float2(_Wrap, _Wrap);
			o.Albedo = tex2D(_ScreenTex, screenUV).rgb *_Color.rgb;
			o.Alpha = tex2D(_ScreenTex, screenUV).a * _Color.a;
		}

		half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten)
		{
			return half4(s.Albedo, s.Alpha);
		}
		
		ENDCG
	}
	FallBack "Diffuse"
}
