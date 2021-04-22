Shader "AFS/VegetationStudio/Foliage Shader" {
	Properties {


		[Space(6)]
		[Enum(UnityEngine.Rendering.CullMode)] _Culling ("Culling", Float) = 0

		[Header(Base Settings)]
		[Space(4)]
		_Color 								("Color", Color) = (1,1,1,1)
		_ColorVariation 					("Color Variation", Color) = (1,1,1,0)
		_MainTex 							("Albedo (RGB) Alpha (A)", 2D) = "white" {}
		_Cutoff 							("Alpha Cutoff", Range(0,1)) = 0.3
		[NoScaleOffset] _BumpTransSpecMap	("Normal (GA) Trans(R) Smoothness(B)", 2D) = "bump" {}
		_SpecularReflectivity				("Specular Reflectivity", Color) = (0.2,0.2,0.2)
		_TranslucencyStrength 				("Translucency", Range(0,1)) = 0.5
		_BackfaceSmoothness 				("Backface Smoothness", Range(0,2)) = 1
		_BouncedLighting					("Bounced Lighting", Range(0.0, 5.0)) = 0.0
		_HorizonFade						("Horizon Fade", Range(0.0, 5.0)) = 1.0
		
		[Header(Wind Settings)]
		[Space(4)]
		_LeafTurbulence 					("Leaf Turbulence", Range(0,1)) = 0.2
		[KeywordEnum(Legacy Vertex Colors, UV4 And Vertex Colors, Vertex Colors)]
		_BendingControls 					("Bending Parameters", Float) = 0 // 0 = legacy vertex colors, 1 = uv4, 2 = vertex colors

		[Header(Rain Detail Settings)]
		[Space(4)]
		[Toggle(EFFECT_BUMP)] _RainDetails	("Enable Rain Details", Float) = 0
		[NoScaleOffset] _RainBumpMask 		("Rain Normal (GA) Mask (B)", 2D) = "bump" {}
		_RainTexScale 						("Rain Texture Scale", Float) = 4

		[Space(8)]
		[HideInInspector] _DebugMode ("Debug Mode", Float) = 1

		[Header(VegetationStudio Specials)]
		[Toggle] TOUCH_BEND("Use touch bend", Float) = 1
		_ForceRadius("    Force (X) Radius (Y)", Vector) = (1,2,0,0)

	}

	SubShader {
		Tags {
			"Queue"="AlphaTest"
			"IgnoreProjector"="True"
			"RenderType"="AFSFoliageBendingInstanced"
			"AfsMode"="Foliage"
			"DisableBatching" = "LODFading"
		}

		LOD 200
		Cull [_Culling]
		
		CGPROGRAM

		//	In case you want to get rid of the warning "Unrecognized #pragma surface ..." in 5.6. please comment "dithercrossfade"
			#pragma surface surf AFSSpecular vertex:AfsFoligeBendingGSFull addshadow fullforwardshadows dithercrossfade
			#pragma target 3.0

			#pragma multi_compile_instancing
			// #pragma instancing_options maxcount:50
			#pragma shader_feature EFFECT_BUMP

#pragma multi_compile __ LOD_FADE_CROSSFADE
#pragma multi_compile __ AFS_PRESERVELENGTH

// VS
#pragma multi_compile GPU_FRUSTUM_ON __
#pragma multi_compile TOUCH_BEND_ON __
#pragma multi_compile FAR_CULL_ON __

#pragma instancing_options procedural:setup

			#include "Includes/AfsPBSLighting.cginc"
			// Vertex Functions
			#include "TerrainEngine.cginc"
			#include "Includes/AfsFoliageBendingInstanced.cginc"

			fixed4 _Color;
			fixed4 _ColorVariation;
			sampler2D _MainTex;
			sampler2D _BumpTransSpecMap;
			fixed3 _SpecularReflectivity;
			fixed _BackfaceSmoothness;
			fixed _Cutoff;

			half _TranslucencyStrength;

	  		#if defined(EFFECT_BUMP)
				sampler2D _RainBumpMask;
				float _RainTexScale;
			#endif

			// Global vars
			float _AfsRainamount;
			float2 _AfsSpecFade;

			struct Input {
				float2 uv_MainTex;
				fixed4 color : COLOR0;	// color.a = AO
				float3 worldNormal;
				float facingSign : VFACE;
				INTERNAL_DATA
				UNITY_DITHER_CROSSFADE_COORDS
			};

			#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
				struct IndirectShaderData {
					float4x4 PositionMatrix;
					float4x4 InversePositionMatrix;
					float4 ControlData;
				};

				#if defined(SHADER_API_GLCORE) || defined(SHADER_API_D3D11) || defined(SHADER_API_GLES3) || defined(SHADER_API_METAL) || defined(SHADER_API_VULKAN) || defined(SHADER_API_PSSL) || defined(SHADER_API_XBOXONE)
					StructuredBuffer<IndirectShaderData> IndirectShaderDataBuffer;
					StructuredBuffer<IndirectShaderData> VisibleShaderDataBuffer;
				#endif	
			#endif

// VS touch react
			#ifdef TOUCH_BEND_ON
				sampler2D	_TouchReact_Buffer;
				float4		_TouchReact_Pos;
				float2		_ForceRadius;
			#endif

			void setup() {
				#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
					#ifdef GPU_FRUSTUM_ON
						unity_ObjectToWorld = VisibleShaderDataBuffer[unity_InstanceID].PositionMatrix;
						unity_WorldToObject = VisibleShaderDataBuffer[unity_InstanceID].InversePositionMatrix;
					#else
						unity_ObjectToWorld = IndirectShaderDataBuffer[unity_InstanceID].PositionMatrix;
						unity_WorldToObject = IndirectShaderDataBuffer[unity_InstanceID].InversePositionMatrix;
					#endif
				#endif
			}

			void AfsFoligeBendingGSFull (inout appdata_full v, out Input o) 
			{
				UNITY_INITIALIZE_OUTPUT(Input,o);

				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				float DistanceToCam = distance(worldPos, _WorldSpaceCameraPos);
				
				float4 bendingCoords;
				bendingCoords.rg = v.color.rg;
			//	Legacy Bending:		Primary and secondary bending stored in vertex color blue
			//	New Bending:		Primary and secondary bending stored in uv4: x = primary bending / y = secondary
			//	VeretxColors Only: 	Primary in vertex color A, secondary bending in vertex color blue
				bendingCoords.ba = (_BendingControls == 2) ? v.color.ab : v.texcoord3.xy;
				bendingCoords.ba = (_BendingControls == 0) ? v.color.bb : bendingCoords.ba;

			//	Add variation only if the shader uses UV4
				float variation = (_BendingControls == 1) ? v.color.b * 2 : 0;
				float storedVariation = variation;

			//	VS touch bending
#ifdef TOUCH_BEND_ON
				float3 pivot = float3(unity_ObjectToWorld[0].w, unity_ObjectToWorld[1].w, unity_ObjectToWorld[2].w);
				float2 tbPos = saturate((float2(pivot.x, -pivot.z) - _TouchReact_Pos.xz) / _TouchReact_Pos.w);
				float2 touchBend = tex2Dlod(_TouchReact_Buffer, float4(tbPos, 0, 0));
				touchBend.y *= 1.0 - length(tbPos - 0.5) * 2; // clip texture "clamp" bugs
				if (touchBend.y > 0.01) {
					float influence = saturate(1.0 - abs((touchBend.x * 10000.0f) - pivot.y) * _ForceRadius.y /*radius or falloff*/);
					v.vertex.xyz += float3(1, -0.5, 1) * _ForceRadius.x * influence * v.color.a;
				}
#endif

	
				AfsAnimateVertex (v.vertex, v.normal, v.tangent, float3(0,0,0), 0.0f, bendingCoords, variation);

			//	Store Fade for specular highlights
				//float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				v.color.r = variation; // (_BendingControls == 2) ? 1.0 - saturate(storedVariation) : variation;
				v.color.b = saturate( /*bug: abs*/( _AfsSpecFade.x - DistanceToCam ) / _AfsSpecFade.y);
				v.normal = normalize(v.normal);
				v.tangent.xyz = normalize(v.tangent.xyz);

			//	Set proper v.color.a
				v.color.a = (_BendingControls == 2) ? 1 : v.color.a;

				UNITY_TRANSFER_DITHER_CROSSFADE(o, v.vertex)
			}


			void surf (Input IN, inout SurfaceOutputAFSSpecular o) {

			#if UNITY_VERSION < 2017
				UNITY_APPLY_DITHER_CROSSFADE(IN)
			#endif

				half4 c = tex2D (_MainTex, IN.uv_MainTex.xy) * _Color;
			//	Do early alpha test
				clip(c.a - _Cutoff);
				
			//	Add Color Variation
				o.Albedo = lerp(c.rgb, (c.rgb * _ColorVariation.rgb), IN.color.r * _ColorVariation.a);
				o.Alpha = c.a;

				fixed4 trngls = tex2D(_BumpTransSpecMap, IN.uv_MainTex.xy);
				o.Smoothness = trngls.b;
				o.Specular = _SpecularReflectivity;

			//	Backface Smoothness
				o.Smoothness = (IN.facingSign > 0) ? o.Smoothness : o.Smoothness * _BackfaceSmoothness;
				
				o.Normal = UnpackNormalDXT5nm(trngls) * half3(1,1,IN.facingSign);
				o.VertexNormal = WorldNormalVector(IN, half3(0,0,IN.facingSign) );
				
				o.Occlusion = IN.color.a;

			// 	Get Rain Detail Texture
				#if defined(EFFECT_BUMP)
					fixed4 RainTexSample = tex2D(_RainBumpMask, IN.uv_MainTex.xy * _RainTexScale);
				#endif
				
			//	Add Rain
				if (_AfsRainamount > 0.0f) {
					float3 worldNormal = WorldNormalVector (IN, o.Normal);
				 	float Rainamount = saturate(_AfsRainamount * worldNormal.y);
					float porosity = saturate( ((1-o.Smoothness) - 0.5) / 0.4 );
					// Calc diffuse factor
					float factor = lerp(1.0, 0.2, porosity);
					// Water influence on material BRDF
					o.Albedo *= lerp(1.0, factor, Rainamount); // Attenuate diffuse
					o.Smoothness = lerp(o.Smoothness, 0.9, Rainamount);
					// Lerp specular Color towards IOR of Water
					o.Specular = lerp(o.Specular, unity_ColorSpaceDielectricSpec.rgb * 0.5, Rainamount);
					// Add rain drop normal
					#if defined(EFFECT_BUMP)
						fixed Mask = RainTexSample.g;
						float wetBlend = saturate(Mask + Rainamount - 1);
						float3 Normal = UnpackNormalDXT5nm(RainTexSample);
						Normal = lerp(half3(0,0,1), Normal, saturate(wetBlend * 10) );
						o.Normal = BlendNormals(o.Normal, Normal);
					#endif
				}
			//	Fade out smoothness and translucency
				o.Smoothness *= IN.color.b;
				o.Translucency = trngls.r * _TranslucencyStrength * IN.color.b;
			}
		ENDCG
	}
}
