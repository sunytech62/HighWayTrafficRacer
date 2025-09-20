// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "KokoZone/Car Stunt/Car Body"
{
	Properties
	{
		_Color("Base Color", Color) = (0.6415094,0.1482734,0.1482734,1)
		_MaskColor("Mask Color", Color) = (0.6743306,1,0.4858491,1)
		_MainTex("Albedo", 2D) = "white" {}
		_DamageAlbedo("DamageAlbedo", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
		_AlbedoBoost("Albedo Boost", Range( 1 , 5)) = 1
		_DamageLevel("DamageLevel", Range( 0 , 1)) = 1
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Glossiness("Smoothness", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma only_renderers d3d9 d3d11_9x d3d11 glcore gles gles3 metal vulkan 
		#pragma surface surf Standard keepalpha nolightmap  nodynlightmap nodirlightmap nofog noforwardadd 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _MainTex;
		uniform half4 _MainTex_ST;
		uniform sampler2D _DamageAlbedo;
		uniform half4 _DamageAlbedo_ST;
		uniform half _DamageLevel;
		uniform half4 _Color;
		uniform half4 _MaskColor;
		uniform sampler2D _Mask;
		uniform half4 _Mask_ST;
		uniform half _AlbedoBoost;
		uniform half _Metallic;
		uniform half _Glossiness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float2 uv_DamageAlbedo = i.uv_texcoord * _DamageAlbedo_ST.xy + _DamageAlbedo_ST.zw;
			half4 tex2DNode41 = tex2D( _DamageAlbedo, uv_DamageAlbedo );
			half4 lerpResult55 = lerp( tex2D( _MainTex, uv_MainTex ) , tex2DNode41 , ( tex2DNode41.a * _DamageLevel ));
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			half4 lerpResult16 = lerp( _Color , _MaskColor , ( lerpResult55 * tex2D( _Mask, uv_Mask ).a ));
			o.Albedo = ( ( lerpResult55 * lerpResult16 ) * _AlbedoBoost ).rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
}
/*ASEBEGIN
Version=18912
-1920;0;1920;1019;3545.581;474.1967;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;56;-2548.366,193.481;Inherit;False;Property;_DamageLevel;DamageLevel;6;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;41;-2560.61,-54.27505;Inherit;True;Property;_DamageAlbedo;DamageAlbedo;3;0;Create;True;0;0;0;False;0;False;-1;2c6536772776dd84f872779990273bfc;c4437f0c07125dc44ab9c7bb4073f017;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-2247.046,95.51122;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-2542.906,-383.1534;Inherit;True;Property;_MainTex;Albedo;2;0;Create;False;0;0;0;False;0;False;-1;eb6f05e4b01b80d47ab7a9146b84ec73;dfe3f1ecb394d144380b356940a14d28;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-1868.864,153.6386;Inherit;True;Property;_Mask;Mask;4;0;Create;True;0;0;0;False;0;False;-1;a77fbe525b11cf440871bd4a29d55600;a77fbe525b11cf440871bd4a29d55600;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;55;-2085.965,-87.91034;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-1341.248,0.2059865;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;14;-1891.262,613.7407;Inherit;False;Property;_MaskColor;Mask Color;1;0;Create;True;0;0;0;False;0;False;0.6743306,1,0.4858491,1;1,0.3757962,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;15;-1848.562,400.0408;Inherit;False;Property;_Color;Base Color;0;0;Create;False;0;0;0;False;0;False;0.6415094,0.1482734,0.1482734,1;0.3195877,1,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;16;-1252.427,426.2947;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-833.83,-93.07578;Inherit;False;Property;_AlbedoBoost;Albedo Boost;5;0;Create;True;0;0;0;False;0;False;1;1;1;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-770.041,-381.3711;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-482.39,-357.0979;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-823.2178,154.4605;Inherit;False;Property;_Glossiness;Smoothness;8;0;Create;False;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-833.9023,23.17267;Inherit;False;Property;_Metallic;Metallic;7;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;203,16;Half;False;True;-1;2;;0;0;Standard;KokoZone/Car Stunt/Car Body;False;False;False;False;False;False;True;True;True;True;False;True;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;False;0;False;Opaque;;Geometry;All;8;d3d9;d3d11_9x;d3d11;glcore;gles;gles3;metal;vulkan;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;58;0;41;4
WireConnection;58;1;56;0
WireConnection;55;0;4;0
WireConnection;55;1;41;0
WireConnection;55;2;58;0
WireConnection;12;0;55;0
WireConnection;12;1;2;4
WireConnection;16;0;15;0
WireConnection;16;1;14;0
WireConnection;16;2;12;0
WireConnection;28;0;55;0
WireConnection;28;1;16;0
WireConnection;34;0;28;0
WireConnection;34;1;33;0
WireConnection;0;0;34;0
WireConnection;0;3;30;0
WireConnection;0;4;32;0
ASEEND*/
//CHKSM=7428DB86E27570B5C779E4ED2747E4175F082663