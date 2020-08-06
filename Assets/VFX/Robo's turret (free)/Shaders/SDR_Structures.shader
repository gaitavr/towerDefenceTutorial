// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Robo shaders/Structures"
{
	Properties
	{
		_Paintedcolor("Painted color", Color) = (0.5566038,0.1444019,0.1444019,0)
		_Tint("Tint", Color) = (0.6981132,0.5566645,0.4511392,0)
		_Brightness("Brightness", Range( 0 , 2)) = 1
		[HDR]_Emissivecolor("Emissive color", Color) = (0,0,0,0)
		[Toggle]_Fresnel("Fresnel", Float) = 0
		[HDR]_Fresnelcolor("Fresnel color", Color) = (1,0.0235849,0.0235849,0)
		_Fresnelfalloff("Fresnel falloff", Range( 0 , 5)) = 1
		_Desaturation("Desaturation", Range( 0 , 1)) = 0
		[Toggle]_Desaturationaffectsemission("Desaturation affects emission", Float) = 1
		_Flickeringintensity("Flickering intensity", Range( 1 , 20)) = 0
		[Toggle]_Continuousflickering("Continuous flickering", Float) = 0
		_Minimumsmoothness("Minimum smoothness", Range( 0 , 0.9)) = 0
		_Minimumsmoothnesspainted("Minimum smoothness (painted)", Range( 0 , 0.9)) = 0
		_Albedo("Albedo", 2D) = "white" {}
		_Paintedmask("Painted mask", 2D) = "black" {}
		_Tintmask("Tint mask", 2D) = "black" {}
		_Normal("Normal", 2D) = "bump" {}
		_MetalSmoothness("Metal Smoothness", 2D) = "gray" {}
		_Emissive("Emissive", 2D) = "black" {}
		_Flickermask("Flicker mask", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			INTERNAL_DATA
			float3 worldNormal;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float _Brightness;
		uniform sampler2D _Paintedmask;
		uniform float4 _Paintedmask_ST;
		uniform float4 _Paintedcolor;
		uniform float4 _Tint;
		uniform sampler2D _Tintmask;
		uniform float4 _Tintmask_ST;
		uniform sampler2D _Emissive;
		uniform float4 _Emissive_ST;
		uniform float4 _Emissivecolor;
		uniform float _Desaturation;
		uniform float _Fresnel;
		uniform float _Desaturationaffectsemission;
		uniform float _Continuousflickering;
		uniform float _Flickeringintensity;
		uniform sampler2D _Flickermask;
		uniform float4 _Flickermask_ST;
		uniform float4 _Fresnelcolor;
		uniform float _Fresnelfalloff;
		uniform sampler2D _MetalSmoothness;
		uniform float4 _MetalSmoothness_ST;
		uniform float _Minimumsmoothness;
		uniform float _Minimumsmoothnesspainted;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			float3 tex2DNode4 = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			o.Normal = tex2DNode4;
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 tex2DNode1 = tex2D( _Albedo, uv_Albedo );
			float2 uv_Paintedmask = i.uv_texcoord * _Paintedmask_ST.xy + _Paintedmask_ST.zw;
			float4 tex2DNode12 = tex2D( _Paintedmask, uv_Paintedmask );
			float4 lerpResult96 = lerp( tex2DNode1 , ( tex2DNode1 * _Brightness ) , ( 1.0 - tex2DNode12.r ));
			float4 lerpResult14 = lerp( lerpResult96 , ( _Paintedcolor * lerpResult96 ) , tex2DNode12.r);
			float4 blendOpSrc43 = _Paintedcolor;
			float4 blendOpDest43 = _Tint;
			float2 uv_Tintmask = i.uv_texcoord * _Tintmask_ST.xy + _Tintmask_ST.zw;
			float4 lerpResult29 = lerp( lerpResult14 , ( ( saturate( (( blendOpDest43 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpDest43 - 0.5 ) ) * ( 1.0 - blendOpSrc43 ) ) : ( 2.0 * blendOpDest43 * blendOpSrc43 ) ) )) * lerpResult96 ) , tex2D( _Tintmask, uv_Tintmask ));
			float2 uv_Emissive = i.uv_texcoord * _Emissive_ST.xy + _Emissive_ST.zw;
			float4 tex2DNode16 = tex2D( _Emissive, uv_Emissive );
			float4 lerpResult21 = lerp( lerpResult29 , ( tex2DNode16 * _Emissivecolor ) , tex2DNode16.r);
			float3 desaturateInitialColor50 = lerpResult21.rgb;
			float desaturateDot50 = dot( desaturateInitialColor50, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar50 = lerp( desaturateInitialColor50, desaturateDot50.xxx, _Desaturation );
			o.Albedo = desaturateVar50;
			float2 uv_Flickermask = i.uv_texcoord * _Flickermask_ST.xy + _Flickermask_ST.zw;
			float4 lerpResult68 = lerp( _Emissivecolor , ( _Emissivecolor * max( ( lerp(max( _SinTime.z , 0.0 ),abs( _SinTime.z ),_Continuousflickering) * _Flickeringintensity ) , 1.0 ) ) , tex2D( _Flickermask, uv_Flickermask ));
			float4 temp_output_18_0 = ( tex2DNode16 * lerpResult68 );
			float3 desaturateInitialColor55 = temp_output_18_0.rgb;
			float desaturateDot55 = dot( desaturateInitialColor55, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar55 = lerp( desaturateInitialColor55, desaturateDot55.xxx, _Desaturation );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_tangentToWorldFast = float3x3(ase_worldTangent.x,ase_worldBitangent.x,ase_worldNormal.x,ase_worldTangent.y,ase_worldBitangent.y,ase_worldNormal.y,ase_worldTangent.z,ase_worldBitangent.z,ase_worldNormal.z);
			float fresnelNdotV79 = dot( mul(ase_tangentToWorldFast,tex2DNode4), ase_worldViewDir );
			float fresnelNode79 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV79, _Fresnelfalloff ) );
			float4 lerpResult92 = lerp( lerp(temp_output_18_0,float4( desaturateVar55 , 0.0 ),_Desaturationaffectsemission) , _Fresnelcolor , min( fresnelNode79 , 1.0 ));
			o.Emission = lerp(lerp(temp_output_18_0,float4( desaturateVar55 , 0.0 ),_Desaturationaffectsemission),lerpResult92,_Fresnel).rgb;
			float2 uv_MetalSmoothness = i.uv_texcoord * _MetalSmoothness_ST.xy + _MetalSmoothness_ST.zw;
			float4 tex2DNode3 = tex2D( _MetalSmoothness, uv_MetalSmoothness );
			float4 appendResult11 = (float4(tex2DNode3.r , tex2DNode3.g , tex2DNode3.b , 0.0));
			o.Metallic = appendResult11.x;
			float lerpResult24 = lerp( max( tex2DNode3.a , _Minimumsmoothness ) , max( tex2DNode3.a , _Minimumsmoothnesspainted ) , tex2DNode12.r);
			o.Smoothness = lerpResult24;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16301
2567;148;1906;1010;375.1068;589.1562;1.984901;True;True
Node;AmplifyShaderEditor.CommentaryNode;74;-1419.583,533.0792;Float;False;3038.762;1386.438;Flickering and emission;15;18;71;16;68;63;69;17;66;64;67;61;65;56;77;78;;0.3726415,0.9514498,1,1;0;0
Node;AmplifyShaderEditor.SinTimeNode;56;-1344.4,1123.379;Float;True;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMaxOpNode;77;-1002.656,963.9825;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;61;-983.1148,1261.819;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;78;-683.2754,1132.079;Float;False;Property;_Continuousflickering;Continuous flickering;11;0;Create;True;0;0;False;0;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-985.8378,1538.471;Float;False;Property;_Flickeringintensity;Flickering intensity;10;0;Create;True;0;0;False;0;0;14.55;1;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-172.3413,1556.779;Float;False;Constant;_1;1;15;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-272.8212,1228.025;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;98;-1400.926,-354.4675;Float;False;Property;_Brightness;Brightness;2;0;Create;True;0;0;False;0;1;0.8;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;12;-1433.953,-958.6374;Float;True;Property;_Paintedmask;Painted mask;15;0;Create;True;0;0;False;0;b34437b3a84071b448b8a3264164ae12;b34437b3a84071b448b8a3264164ae12;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;17;-387.0074,930.6396;Float;False;Property;_Emissivecolor;Emissive color;3;1;[HDR];Create;True;0;0;False;0;0,0,0,0;1.844303,0.4055535,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1416.244,-663.5305;Float;True;Property;_Albedo;Albedo;14;0;Create;True;0;0;False;0;51ed1188220a98742ad3c1cb24213833;51ed1188220a98742ad3c1cb24213833;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMaxOpNode;66;132.0869,1189.859;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;362.6747,1002.701;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;95;-977.9243,-1017.764;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;97;-947.0267,-475.2675;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;69;216.34,1530.815;Float;True;Property;_Flickermask;Flicker mask;20;0;Create;True;0;0;False;0;7bf2cf885c389ff4aaa2749b35a58073;7bf2cf885c389ff4aaa2749b35a58073;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;96;-601.5256,-736.0508;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;16;-205.7993,678.7603;Float;True;Property;_Emissive;Emissive;19;0;Create;True;0;0;False;0;fc208b44be1bdff4a8cc42550aa2408d;fc208b44be1bdff4a8cc42550aa2408d;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;68;730.62,898.7475;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;41;-381.9552,-1437.533;Float;False;Property;_Tint;Tint;1;0;Create;True;0;0;False;0;0.6981132,0.5566645,0.4511392,0;0.6981132,0.5566645,0.4511392,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;13;-685.1962,-1205.072;Float;False;Property;_Paintedcolor;Painted color;0;0;Create;True;0;0;False;0;0.5566038,0.1444019,0.1444019,0;0,0.4528302,0.08156572,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;1018.25,795.3264;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;43;48.96292,-1193.271;Float;True;Overlay;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-208.3365,-1056.166;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;51;1271.396,-553.3268;Float;False;Property;_Desaturation;Desaturation;7;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;1644.415,1349.054;Float;True;Property;_Normal;Normal;17;0;Create;True;0;0;False;0;10afce0a9c4cf0e43a53f7bb1adf382d;10afce0a9c4cf0e43a53f7bb1adf382d;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;80;1677.58,-731.702;Float;False;Property;_Fresnelfalloff;Fresnel falloff;6;0;Create;True;0;0;False;0;1;5;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.DesaturateOpNode;55;1738.187,690.6838;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;42;51.32484,-501.4227;Float;True;Property;_Tintmask;Tint mask;16;0;Create;True;0;0;False;0;c0cb4e77c1a95e64091c6170db0d8480;c0cb4e77c1a95e64091c6170db0d8480;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;76;-712.8096,-273.8701;Float;False;1449.942;738.7083;Metallick smoothness;7;3;22;25;23;26;24;11;;0.3537736,1,0.5451297,1;0;0
Node;AmplifyShaderEditor.FresnelNode;79;2070.51,-816.6803;Float;True;Standard;TangentNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;14;2.952917,-925.175;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;464.3654,-904.7229;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;94;2183.584,-951.319;Float;False;Constant;_Float0;Float 0;19;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMinOpNode;93;2410.185,-813.9788;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;82;2202.386,-488.3745;Float;False;Property;_Fresnelcolor;Fresnel color;5;1;[HDR];Create;True;0;0;False;0;1,0.0235849,0.0235849,0;0,0.9956001,0.09370355,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;54;1897.636,325.2408;Float;False;Property;_Desaturationaffectsemission;Desaturation affects emission;8;0;Create;True;0;0;False;0;1;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-449.1956,225.6679;Float;False;Property;_Minimumsmoothnesspainted;Minimum smoothness (painted);13;0;Create;True;0;0;False;0;0;0.119;0;0.9;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-313.4379,349.8382;Float;False;Property;_Minimumsmoothness;Minimum smoothness;12;0;Create;True;0;0;False;0;0;0.573;0;0.9;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;548.6495,717.7595;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;29;850.4943,-593.8394;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;3;-662.8096,-125.3103;Float;True;Property;_MetalSmoothness;Metal Smoothness;18;0;Create;True;0;0;False;0;10754defc646d824c8151442ade4b5b7;10754defc646d824c8151442ade4b5b7;True;0;False;gray;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMaxOpNode;23;201.171,206.6548;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;26;98.9741,87.28218;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;92;2822.708,-499.8199;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;21;1290.605,-365.8694;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;85;3225.21,-96.30524;Float;False;Property;_Fresnel;Fresnel;4;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DesaturateOpNode;50;1796.469,-159.0599;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ToggleSwitchNode;62;1694.726,885.9152;Float;True;Property;_Flickering;Flickering;9;0;Create;True;0;0;False;0;1;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;11;497.132,-223.8701;Float;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;24;537.6695,98.14299;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3413.466,142.1859;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Robo shaders/Structures;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0.58;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;77;0;56;3
WireConnection;61;0;56;3
WireConnection;78;0;77;0
WireConnection;78;1;61;0
WireConnection;64;0;78;0
WireConnection;64;1;65;0
WireConnection;66;0;64;0
WireConnection;66;1;67;0
WireConnection;63;0;17;0
WireConnection;63;1;66;0
WireConnection;95;0;12;1
WireConnection;97;0;1;0
WireConnection;97;1;98;0
WireConnection;96;0;1;0
WireConnection;96;1;97;0
WireConnection;96;2;95;0
WireConnection;68;0;17;0
WireConnection;68;1;63;0
WireConnection;68;2;69;0
WireConnection;18;0;16;0
WireConnection;18;1;68;0
WireConnection;43;0;13;0
WireConnection;43;1;41;0
WireConnection;15;0;13;0
WireConnection;15;1;96;0
WireConnection;55;0;18;0
WireConnection;55;1;51;0
WireConnection;79;0;4;0
WireConnection;79;3;80;0
WireConnection;14;0;96;0
WireConnection;14;1;15;0
WireConnection;14;2;12;1
WireConnection;44;0;43;0
WireConnection;44;1;96;0
WireConnection;93;0;79;0
WireConnection;93;1;94;0
WireConnection;54;0;18;0
WireConnection;54;1;55;0
WireConnection;71;0;16;0
WireConnection;71;1;17;0
WireConnection;29;0;14;0
WireConnection;29;1;44;0
WireConnection;29;2;42;0
WireConnection;23;0;3;4
WireConnection;23;1;22;0
WireConnection;26;0;3;4
WireConnection;26;1;25;0
WireConnection;92;0;54;0
WireConnection;92;1;82;0
WireConnection;92;2;93;0
WireConnection;21;0;29;0
WireConnection;21;1;71;0
WireConnection;21;2;16;1
WireConnection;85;0;54;0
WireConnection;85;1;92;0
WireConnection;50;0;21;0
WireConnection;50;1;51;0
WireConnection;11;0;3;1
WireConnection;11;1;3;2
WireConnection;11;2;3;3
WireConnection;24;0;23;0
WireConnection;24;1;26;0
WireConnection;24;2;12;1
WireConnection;0;0;50;0
WireConnection;0;1;4;0
WireConnection;0;2;85;0
WireConnection;0;3;11;0
WireConnection;0;4;24;0
ASEEND*/
//CHKSM=9F218194EDEB1BF90B40B08C6AE656CC59319546