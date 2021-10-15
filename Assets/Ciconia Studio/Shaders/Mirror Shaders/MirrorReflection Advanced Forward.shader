// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Ciconia Studio/Effects/MirrorReflection/Forward rendering/Advanced" {
    Properties {
        [Space(15)][Header(Main Properties)]
        [Space(10)]_MainTex ("Albedo", 2D) = "black" {}
        _AlbedoIntensity ("Intensity", Range(0, 1)) = 0
        [Space(35)]_SpecColor ("Specular Color", Color) = (1,1,1,1)
        _SpecGlossMap ("Specular map(Gloss A)", 2D) = "white" {}
        _SpecularIntensity1 ("Specular Intensity", Range(0, 8)) = 0.2
        _Glossiness ("Glossiness", Range(0, 2)) = 0.5
        [Space(35)]_BumpMap ("Normal map", 2D) = "bump" {}
        _NormalIntensity ("Normal Intensity", Range(0, 2)) = 1
        [Space(35)]_OcclusionMap ("Ambient Occlusion map", 2D) = "white" {}
        _AoIntensity ("Ao Intensity", Range(0, 2)) = 1
        [HideInInspector]_ReflectionTex ("ReflectionTex", 2D) = "bump" {}

        [Space(45)][Header(Reflection Properties)]
        [Space(10)][MaterialToggle] _InvertReflection ("Invert Reflection", Float ) = 0
        _Reflectionmask ("Reflection mask", 2D) = "white" {}
        [Space(10)]_ExpandMask ("Expand Mask", Range(0, 1)) = 1
        _ReflectionBlend ("Reflection Blend", Range(0, 1)) = 0
        [Space(15)]_ReflectionIntensity ("Reflection Intensity", Range(0, 5)) = 0.8
        [Space(15)][MaterialToggle] _SwitchColor ("Switch Color", Float ) = 0
        _MirrorTintWhite ("Mirror Tint (White)", Color) = (1,1,1,1)
        _MirrorTintBlack ("Mirror Tint (Black)", Color) = (1,1,1,1)
        [Space(25)]_SpecColor2 ("Specular Color", Color) = (1,1,1,1)
        _SpecularIntensity2 ("Specular Intensity", Range(0, 2)) = 0.2
        _Glossiness2 ("Glossiness", Range(0, 2)) = 0.5

        [Space(45)][Header(Damaged Properties)]
        [Space(10)]_BrokenColor ("Color", Color) = (1,1,1,1)
        _BrokenmapBW ("Broken map (B&W)", 2D) = "black" {}
        _BrokenmapIntensity ("Intensity", Range(0, 10)) = 1
        _TextureBlend ("Texture Blend", Range(0, 1)) = 0
        [Space(35)]_BrokenBumpMap ("Broken Glass (Normalmap)", 2D) = "bump" {}
        _GlassNormalIntensity ("Normal Intensity", Range(0, 2)) = 1
        [Space(10)]_Refraction ("Refraction", Range(0, 1)) = 0.2
        [Space(10)]_NormalBlend ("Normal Blend", Range(0, 1)) = 0

        [Space(45)][Header(Background Cubemap)]
        [Space(10)][MaterialToggle] _EnableCubemap ("Enable Cubemap", Float ) = 0
        _Cubemap ("Cubemap", Cube) = "_Skybox" {}
        _CubemapIntensity ("Intensity", Range(0, 12)) = 1
        [Space(15)]_CubemapReflectionBlend ("Reflection Blend", Range(0, 1)) = 0
        _CubemapBlur ("Blur", Range(0, 8)) = 0
        [Space(15)]_FresnelStrength ("Fresnel Strength", Range(0, 8)) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _ReflectionTex; uniform float4 _ReflectionTex_ST;
            uniform sampler2D _Reflectionmask; uniform float4 _Reflectionmask_ST;
            uniform float4 _MirrorTintBlack;
            uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
            uniform float _NormalIntensity;
            uniform sampler2D _OcclusionMap; uniform float4 _OcclusionMap_ST;
            uniform float _AoIntensity;
            uniform float4 _MirrorTintWhite;
            uniform sampler2D _SpecGlossMap; uniform float4 _SpecGlossMap_ST;
            uniform float _SpecularIntensity1;
            uniform float _Glossiness;
            uniform float4 _SpecColor2;
            uniform float _Glossiness2;
            uniform float _SpecularIntensity2;
            uniform float _ReflectionBlend;
            uniform float _ReflectionIntensity;
            uniform float _ExpandMask;
            uniform float _Refraction;
            uniform sampler2D _BrokenBumpMap; uniform float4 _BrokenBumpMap_ST;
            uniform sampler2D _BrokenmapBW; uniform float4 _BrokenmapBW_ST;
            uniform float _AlbedoIntensity;
            uniform fixed _InvertReflection;
            uniform fixed _SwitchColor;
            uniform float _BrokenmapIntensity;
            uniform float4 _BrokenColor;
            uniform float _CubemapIntensity;
            uniform float _CubemapBlur;
            uniform float _FresnelStrength;
            uniform samplerCUBE _Cubemap;
            uniform fixed _EnableCubemap;
            uniform float _CubemapReflectionBlend;
            uniform float _GlassNormalIntensity;
            uniform float _NormalBlend;
            uniform float _TextureBlend;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                float4 screenPos : TEXCOORD7;
                UNITY_FOG_COORDS(8)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD9;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #endif
                #ifdef DYNAMICLIGHTMAP_ON
                    o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap)));
                float3 _BrokenBumpMap_var = UnpackNormal(tex2D(_BrokenBumpMap,TRANSFORM_TEX(i.uv0, _BrokenBumpMap)));
                float3 node_7565_nrm_base = lerp(float3(0,0,1),_BumpMap_var.rgb,_NormalIntensity) + float3(0,0,1);
                float3 node_7565_nrm_detail = lerp(float3(0,0,1),_BrokenBumpMap_var.rgb,_GlassNormalIntensity) * float3(-1,-1,1);
                float3 node_7565_nrm_combined = node_7565_nrm_base*dot(node_7565_nrm_base, node_7565_nrm_detail)/node_7565_nrm_base.z - node_7565_nrm_detail;
                float3 node_7565 = node_7565_nrm_combined;
                float4 _Reflectionmask_var = tex2D(_Reflectionmask,TRANSFORM_TEX(i.uv0, _Reflectionmask));
                float _InvertReflection_var = lerp( _Reflectionmask_var.r, (1.0 - _Reflectionmask_var.r), _InvertReflection );
                float CubeMask = saturate(_InvertReflection_var);
                float node_4983 = saturate((CubeMask+_NormalBlend));
                float3 Normal = lerp(float3(0,0,1),node_7565,node_4983);
                float3 normalLocal = Normal;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float NormalBlend = node_4983;
                float2 Refraction = ((_BrokenBumpMap_var.rgb.rg*_Refraction)*NormalBlend);
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + Refraction;
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float4 _SpecGlossMap_var = tex2D(_SpecGlossMap,TRANSFORM_TEX(i.uv0, _SpecGlossMap));
                float node_1626 = 0.0;
                float node_5735 = lerp(2,1,_ExpandMask);
                float node_4213 = (1.0+(-1*node_5735));
                float RedMask = saturate((node_4213 + ( (lerp( _Reflectionmask_var.r, (1.0 - _Reflectionmask_var.r), _SwitchColor ) - node_1626) * (node_5735 - node_4213) ) / (1.0 - node_1626)));
                float Glossiness = lerp((_SpecGlossMap_var.a*_Glossiness),(_SpecGlossMap_var.a*_Glossiness2),RedMask);
                float gloss = Glossiness;
                float perceptualRoughness = 1.0 - Glossiness;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                    d.ambient = 0;
                    d.lightmapUV = i.ambientOrLightmapUV;
                #else
                    d.ambient = i.ambientOrLightmapUV;
                #endif
                #if UNITY_SPECCUBE_BLENDING || UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMin[0] = unity_SpecCube0_BoxMin;
                    d.boxMin[1] = unity_SpecCube1_BoxMin;
                #endif
                #if UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMax[0] = unity_SpecCube0_BoxMax;
                    d.boxMax[1] = unity_SpecCube1_BoxMax;
                    d.probePosition[0] = unity_SpecCube0_ProbePosition;
                    d.probePosition[1] = unity_SpecCube1_ProbePosition;
                #endif
                d.probeHDR[0] = unity_SpecCube0_HDR;
                d.probeHDR[1] = unity_SpecCube1_HDR;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float4 _Cubemap_var = texCUBElod(_Cubemap,float4(viewReflectDirection,_CubemapBlur));
                float3 CubemapSpec = (_EnableCubemap*((((0.95*pow(1.0-max(0,dot(normalDirection, viewDirection)),1.0))+0.05)*_FresnelStrength)+((_Cubemap_var.rgb*(_Cubemap_var.a*_CubemapIntensity))*saturate((CubeMask+_CubemapReflectionBlend)))));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 Specular = saturate(lerp((_SpecColor.rgb*_SpecGlossMap_var.rgb*_SpecularIntensity1),(_SpecColor2.rgb*_SpecGlossMap_var.rgb*_SpecularIntensity2),RedMask));
                float3 specularColor = Specular;
                float specularMonochrome;
                float2 node_2394 = float2(sceneUVs.r,(1.0 - sceneUVs.g));
                float4 _ReflectionTex_var = tex2D(_ReflectionTex,TRANSFORM_TEX(node_2394, _ReflectionTex));
                float node_5902 = 1.0;
                float CubemapActive = _EnableCubemap;
                float ReflMask = saturate((_ReflectionBlend+_InvertReflection_var));
                float NormalMask = _BumpMap_var.rgb.r;
                float node_629 = NormalMask;
                float BrokenControle = _Refraction;
                float node_8405 = 0.0;
                float4 _BrokenmapBW_var = tex2D(_BrokenmapBW,TRANSFORM_TEX(i.uv0, _BrokenmapBW));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 node_3389 = float3(1,1,1);
                float3 Diffusemap = lerp(((_MainTex_var.rgb*_AlbedoIntensity)*node_3389),node_3389,0.5);
                float3 node_5069 = Diffusemap;
                float3 Albedo = saturate(( lerp(saturate((_MirrorTintWhite.rgb*node_5069)),saturate((_MirrorTintBlack.rgb*node_5069)),saturate((1.0 - RedMask))) > 0.5 ? (1.0-(1.0-2.0*(lerp(saturate((_MirrorTintWhite.rgb*node_5069)),saturate((_MirrorTintBlack.rgb*node_5069)),saturate((1.0 - RedMask)))-0.5))*(1.0-saturate((1.0-(1.0-(((_ReflectionTex_var.rgb*lerp(float3(node_5902,node_5902,node_5902),(_ReflectionTex_var.a+saturate(((1.0 - _ReflectionTex_var.a)*CubemapSpec))),CubemapActive))*_ReflectionIntensity)*(ReflMask*(1.0 - (saturate((node_629+node_629))*BrokenControle)))))*(1.0-lerp(float3(node_8405,node_8405,node_8405),(((_BrokenColor.rgb*_BrokenmapBW_var.rgb*_BrokenmapBW_var.rgb*_BrokenmapBW_var.rgb)*_BrokenmapIntensity)*saturate((CubeMask+_TextureBlend))),BrokenControle)))))) : (2.0*lerp(saturate((_MirrorTintWhite.rgb*node_5069)),saturate((_MirrorTintBlack.rgb*node_5069)),saturate((1.0 - RedMask)))*saturate((1.0-(1.0-(((_ReflectionTex_var.rgb*lerp(float3(node_5902,node_5902,node_5902),(_ReflectionTex_var.a+saturate(((1.0 - _ReflectionTex_var.a)*CubemapSpec))),CubemapActive))*_ReflectionIntensity)*(ReflMask*(1.0 - (saturate((node_629+node_629))*BrokenControle)))))*(1.0-lerp(float3(node_8405,node_8405,node_8405),(((_BrokenColor.rgb*_BrokenmapBW_var.rgb*_BrokenmapBW_var.rgb*_BrokenmapBW_var.rgb)*_BrokenmapIntensity)*saturate((CubeMask+_TextureBlend))),BrokenControle))))) ));
                float3 diffuseColor = Albedo; // Need this for specular when using metallic
                diffuseColor = EnergyConservationBetweenDiffuseAndSpecular(diffuseColor, specularColor, specularMonochrome);
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                half surfaceReduction;
                #ifdef UNITY_COLORSPACE_GAMMA
                    surfaceReduction = 1.0-0.28*roughness*perceptualRoughness;
                #else
                    surfaceReduction = 1.0/(roughness*roughness + 1.0);
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                half grazingTerm = saturate( gloss + specularMonochrome );
                float3 indirectSpecular = (gi.indirect.specular + CubemapSpec);
                indirectSpecular *= FresnelLerp (specularColor, grazingTerm, NdotV);
                indirectSpecular *= surfaceReduction;
                float3 specular = (directSpecular + indirectSpecular);
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                float4 _OcclusionMap_var = tex2D(_OcclusionMap,TRANSFORM_TEX(i.uv0, _OcclusionMap));
                float Ao = saturate((1.0-(1.0-_OcclusionMap_var.r)*(1.0-lerp(1,0,_AoIntensity))));
                indirectDiffuse *= Ao; // Diffuse AO
                diffuseColor *= 1-specularMonochrome;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(lerp(sceneColor.rgb, finalColor,1),1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _ReflectionTex; uniform float4 _ReflectionTex_ST;
            uniform sampler2D _Reflectionmask; uniform float4 _Reflectionmask_ST;
            uniform float4 _MirrorTintBlack;
            uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
            uniform float _NormalIntensity;
            uniform float4 _MirrorTintWhite;
            uniform sampler2D _SpecGlossMap; uniform float4 _SpecGlossMap_ST;
            uniform float _SpecularIntensity1;
            uniform float _Glossiness;
            uniform float4 _SpecColor2;
            uniform float _Glossiness2;
            uniform float _SpecularIntensity2;
            uniform float _ReflectionBlend;
            uniform float _ReflectionIntensity;
            uniform float _ExpandMask;
            uniform float _Refraction;
            uniform sampler2D _BrokenBumpMap; uniform float4 _BrokenBumpMap_ST;
            uniform sampler2D _BrokenmapBW; uniform float4 _BrokenmapBW_ST;
            uniform float _AlbedoIntensity;
            uniform fixed _InvertReflection;
            uniform fixed _SwitchColor;
            uniform float _BrokenmapIntensity;
            uniform float4 _BrokenColor;
            uniform float _CubemapIntensity;
            uniform float _CubemapBlur;
            uniform float _FresnelStrength;
            uniform samplerCUBE _Cubemap;
            uniform fixed _EnableCubemap;
            uniform float _CubemapReflectionBlend;
            uniform float _GlassNormalIntensity;
            uniform float _NormalBlend;
            uniform float _TextureBlend;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                float4 screenPos : TEXCOORD7;
                LIGHTING_COORDS(8,9)
                UNITY_FOG_COORDS(10)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.screenPos = o.pos;
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap)));
                float3 _BrokenBumpMap_var = UnpackNormal(tex2D(_BrokenBumpMap,TRANSFORM_TEX(i.uv0, _BrokenBumpMap)));
                float3 node_7565_nrm_base = lerp(float3(0,0,1),_BumpMap_var.rgb,_NormalIntensity) + float3(0,0,1);
                float3 node_7565_nrm_detail = lerp(float3(0,0,1),_BrokenBumpMap_var.rgb,_GlassNormalIntensity) * float3(-1,-1,1);
                float3 node_7565_nrm_combined = node_7565_nrm_base*dot(node_7565_nrm_base, node_7565_nrm_detail)/node_7565_nrm_base.z - node_7565_nrm_detail;
                float3 node_7565 = node_7565_nrm_combined;
                float4 _Reflectionmask_var = tex2D(_Reflectionmask,TRANSFORM_TEX(i.uv0, _Reflectionmask));
                float _InvertReflection_var = lerp( _Reflectionmask_var.r, (1.0 - _Reflectionmask_var.r), _InvertReflection );
                float CubeMask = saturate(_InvertReflection_var);
                float node_4983 = saturate((CubeMask+_NormalBlend));
                float3 Normal = lerp(float3(0,0,1),node_7565,node_4983);
                float3 normalLocal = Normal;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float NormalBlend = node_4983;
                float2 Refraction = ((_BrokenBumpMap_var.rgb.rg*_Refraction)*NormalBlend);
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + Refraction;
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float4 _SpecGlossMap_var = tex2D(_SpecGlossMap,TRANSFORM_TEX(i.uv0, _SpecGlossMap));
                float node_1626 = 0.0;
                float node_5735 = lerp(2,1,_ExpandMask);
                float node_4213 = (1.0+(-1*node_5735));
                float RedMask = saturate((node_4213 + ( (lerp( _Reflectionmask_var.r, (1.0 - _Reflectionmask_var.r), _SwitchColor ) - node_1626) * (node_5735 - node_4213) ) / (1.0 - node_1626)));
                float Glossiness = lerp((_SpecGlossMap_var.a*_Glossiness),(_SpecGlossMap_var.a*_Glossiness2),RedMask);
                float gloss = Glossiness;
                float perceptualRoughness = 1.0 - Glossiness;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 Specular = saturate(lerp((_SpecColor.rgb*_SpecGlossMap_var.rgb*_SpecularIntensity1),(_SpecColor2.rgb*_SpecGlossMap_var.rgb*_SpecularIntensity2),RedMask));
                float3 specularColor = Specular;
                float specularMonochrome;
                float2 node_2394 = float2(sceneUVs.r,(1.0 - sceneUVs.g));
                float4 _ReflectionTex_var = tex2D(_ReflectionTex,TRANSFORM_TEX(node_2394, _ReflectionTex));
                float node_5902 = 1.0;
                float4 _Cubemap_var = texCUBElod(_Cubemap,float4(viewReflectDirection,_CubemapBlur));
                float3 CubemapSpec = (_EnableCubemap*((((0.95*pow(1.0-max(0,dot(normalDirection, viewDirection)),1.0))+0.05)*_FresnelStrength)+((_Cubemap_var.rgb*(_Cubemap_var.a*_CubemapIntensity))*saturate((CubeMask+_CubemapReflectionBlend)))));
                float CubemapActive = _EnableCubemap;
                float ReflMask = saturate((_ReflectionBlend+_InvertReflection_var));
                float NormalMask = _BumpMap_var.rgb.r;
                float node_629 = NormalMask;
                float BrokenControle = _Refraction;
                float node_8405 = 0.0;
                float4 _BrokenmapBW_var = tex2D(_BrokenmapBW,TRANSFORM_TEX(i.uv0, _BrokenmapBW));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 node_3389 = float3(1,1,1);
                float3 Diffusemap = lerp(((_MainTex_var.rgb*_AlbedoIntensity)*node_3389),node_3389,0.5);
                float3 node_5069 = Diffusemap;
                float3 Albedo = saturate(( lerp(saturate((_MirrorTintWhite.rgb*node_5069)),saturate((_MirrorTintBlack.rgb*node_5069)),saturate((1.0 - RedMask))) > 0.5 ? (1.0-(1.0-2.0*(lerp(saturate((_MirrorTintWhite.rgb*node_5069)),saturate((_MirrorTintBlack.rgb*node_5069)),saturate((1.0 - RedMask)))-0.5))*(1.0-saturate((1.0-(1.0-(((_ReflectionTex_var.rgb*lerp(float3(node_5902,node_5902,node_5902),(_ReflectionTex_var.a+saturate(((1.0 - _ReflectionTex_var.a)*CubemapSpec))),CubemapActive))*_ReflectionIntensity)*(ReflMask*(1.0 - (saturate((node_629+node_629))*BrokenControle)))))*(1.0-lerp(float3(node_8405,node_8405,node_8405),(((_BrokenColor.rgb*_BrokenmapBW_var.rgb*_BrokenmapBW_var.rgb*_BrokenmapBW_var.rgb)*_BrokenmapIntensity)*saturate((CubeMask+_TextureBlend))),BrokenControle)))))) : (2.0*lerp(saturate((_MirrorTintWhite.rgb*node_5069)),saturate((_MirrorTintBlack.rgb*node_5069)),saturate((1.0 - RedMask)))*saturate((1.0-(1.0-(((_ReflectionTex_var.rgb*lerp(float3(node_5902,node_5902,node_5902),(_ReflectionTex_var.a+saturate(((1.0 - _ReflectionTex_var.a)*CubemapSpec))),CubemapActive))*_ReflectionIntensity)*(ReflMask*(1.0 - (saturate((node_629+node_629))*BrokenControle)))))*(1.0-lerp(float3(node_8405,node_8405,node_8405),(((_BrokenColor.rgb*_BrokenmapBW_var.rgb*_BrokenmapBW_var.rgb*_BrokenmapBW_var.rgb)*_BrokenmapIntensity)*saturate((CubeMask+_TextureBlend))),BrokenControle))))) ));
                float3 diffuseColor = Albedo; // Need this for specular when using metallic
                diffuseColor = EnergyConservationBetweenDiffuseAndSpecular(diffuseColor, specularColor, specularMonochrome);
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                diffuseColor *= 1-specularMonochrome;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _ReflectionTex; uniform float4 _ReflectionTex_ST;
            uniform sampler2D _Reflectionmask; uniform float4 _Reflectionmask_ST;
            uniform float4 _MirrorTintBlack;
            uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
            uniform float4 _MirrorTintWhite;
            uniform sampler2D _SpecGlossMap; uniform float4 _SpecGlossMap_ST;
            uniform float _SpecularIntensity1;
            uniform float _Glossiness;
            uniform float4 _SpecColor2;
            uniform float _Glossiness2;
            uniform float _SpecularIntensity2;
            uniform float _ReflectionBlend;
            uniform float _ReflectionIntensity;
            uniform float _ExpandMask;
            uniform float _Refraction;
            uniform sampler2D _BrokenmapBW; uniform float4 _BrokenmapBW_ST;
            uniform float _AlbedoIntensity;
            uniform fixed _InvertReflection;
            uniform fixed _SwitchColor;
            uniform float _BrokenmapIntensity;
            uniform float4 _BrokenColor;
            uniform float _CubemapIntensity;
            uniform float _CubemapBlur;
            uniform float _FresnelStrength;
            uniform samplerCUBE _Cubemap;
            uniform fixed _EnableCubemap;
            uniform float _CubemapReflectionBlend;
            uniform float _TextureBlend;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float4 screenPos : TEXCOORD5;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5;
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                o.Emission = 0;
                
                float2 node_2394 = float2(sceneUVs.r,(1.0 - sceneUVs.g));
                float4 _ReflectionTex_var = tex2D(_ReflectionTex,TRANSFORM_TEX(node_2394, _ReflectionTex));
                float node_5902 = 1.0;
                float4 _Cubemap_var = texCUBElod(_Cubemap,float4(viewReflectDirection,_CubemapBlur));
                float4 _Reflectionmask_var = tex2D(_Reflectionmask,TRANSFORM_TEX(i.uv0, _Reflectionmask));
                float _InvertReflection_var = lerp( _Reflectionmask_var.r, (1.0 - _Reflectionmask_var.r), _InvertReflection );
                float CubeMask = saturate(_InvertReflection_var);
                float3 CubemapSpec = (_EnableCubemap*((((0.95*pow(1.0-max(0,dot(normalDirection, viewDirection)),1.0))+0.05)*_FresnelStrength)+((_Cubemap_var.rgb*(_Cubemap_var.a*_CubemapIntensity))*saturate((CubeMask+_CubemapReflectionBlend)))));
                float CubemapActive = _EnableCubemap;
                float ReflMask = saturate((_ReflectionBlend+_InvertReflection_var));
                float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap)));
                float NormalMask = _BumpMap_var.rgb.r;
                float node_629 = NormalMask;
                float BrokenControle = _Refraction;
                float node_8405 = 0.0;
                float4 _BrokenmapBW_var = tex2D(_BrokenmapBW,TRANSFORM_TEX(i.uv0, _BrokenmapBW));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 node_3389 = float3(1,1,1);
                float3 Diffusemap = lerp(((_MainTex_var.rgb*_AlbedoIntensity)*node_3389),node_3389,0.5);
                float3 node_5069 = Diffusemap;
                float node_1626 = 0.0;
                float node_5735 = lerp(2,1,_ExpandMask);
                float node_4213 = (1.0+(-1*node_5735));
                float RedMask = saturate((node_4213 + ( (lerp( _Reflectionmask_var.r, (1.0 - _Reflectionmask_var.r), _SwitchColor ) - node_1626) * (node_5735 - node_4213) ) / (1.0 - node_1626)));
                float3 Albedo = saturate(( lerp(saturate((_MirrorTintWhite.rgb*node_5069)),saturate((_MirrorTintBlack.rgb*node_5069)),saturate((1.0 - RedMask))) > 0.5 ? (1.0-(1.0-2.0*(lerp(saturate((_MirrorTintWhite.rgb*node_5069)),saturate((_MirrorTintBlack.rgb*node_5069)),saturate((1.0 - RedMask)))-0.5))*(1.0-saturate((1.0-(1.0-(((_ReflectionTex_var.rgb*lerp(float3(node_5902,node_5902,node_5902),(_ReflectionTex_var.a+saturate(((1.0 - _ReflectionTex_var.a)*CubemapSpec))),CubemapActive))*_ReflectionIntensity)*(ReflMask*(1.0 - (saturate((node_629+node_629))*BrokenControle)))))*(1.0-lerp(float3(node_8405,node_8405,node_8405),(((_BrokenColor.rgb*_BrokenmapBW_var.rgb*_BrokenmapBW_var.rgb*_BrokenmapBW_var.rgb)*_BrokenmapIntensity)*saturate((CubeMask+_TextureBlend))),BrokenControle)))))) : (2.0*lerp(saturate((_MirrorTintWhite.rgb*node_5069)),saturate((_MirrorTintBlack.rgb*node_5069)),saturate((1.0 - RedMask)))*saturate((1.0-(1.0-(((_ReflectionTex_var.rgb*lerp(float3(node_5902,node_5902,node_5902),(_ReflectionTex_var.a+saturate(((1.0 - _ReflectionTex_var.a)*CubemapSpec))),CubemapActive))*_ReflectionIntensity)*(ReflMask*(1.0 - (saturate((node_629+node_629))*BrokenControle)))))*(1.0-lerp(float3(node_8405,node_8405,node_8405),(((_BrokenColor.rgb*_BrokenmapBW_var.rgb*_BrokenmapBW_var.rgb*_BrokenmapBW_var.rgb)*_BrokenmapIntensity)*saturate((CubeMask+_TextureBlend))),BrokenControle))))) ));
                float3 diffColor = Albedo;
                float4 _SpecGlossMap_var = tex2D(_SpecGlossMap,TRANSFORM_TEX(i.uv0, _SpecGlossMap));
                float3 Specular = saturate(lerp((_SpecColor.rgb*_SpecGlossMap_var.rgb*_SpecularIntensity1),(_SpecColor2.rgb*_SpecGlossMap_var.rgb*_SpecularIntensity2),RedMask));
                float3 specColor = Specular;
                float specularMonochrome = max(max(specColor.r, specColor.g),specColor.b);
                diffColor *= (1.0-specularMonochrome);
                float Glossiness = lerp((_SpecGlossMap_var.a*_Glossiness),(_SpecGlossMap_var.a*_Glossiness2),RedMask);
                float roughness = 1.0 - Glossiness;
                o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
