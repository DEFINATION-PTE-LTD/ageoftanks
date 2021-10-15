// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Ciconia Studio/Effects/MirrorReflection/Deferred rendering/Fast Mirror" {
    Properties {
        [Space(15)][Header(Main Properties)]
        [Space(10)]_Color ("Color", Color) = (1,1,1,1)
        [HideInInspector]_ReflectionTex ("ReflectionTex", 2D) = "white" {}
        [Space(10)][MaterialToggle] _ScreenBlendingmode ("Screen Blending mode", Float ) = 0
        _MainTex ("Dirt map (Black&White)", 2D) = "black" {}
        [MaterialToggle] _EnableRGBchannels ("Enable RGB channels", Float ) = 0
        _Intensity ("Intensity", Range(0, 1.5)) = 1
        [Space(35)]_SpecColor ("Specular Color", Color) = (1,1,1,1)
        _SpecularIntensity1 ("Specular Intensity", Range(0, 1)) = 0.2
        [Space(15)][MaterialToggle] _UseDirtmapasGlossmap ("Use Dirt map as Gloss map", Float ) = 0.5
        _Glossiness ("Glossiness", Range(0, 1)) = 0.5

        [Space(45)][Header(Background Cubemap)]
        [Space(10)][MaterialToggle] _EnableCubemap ("Enable Cubemap", Float ) = 0
        _Cubemap ("Cubemap", Cube) = "_Skybox" {}
        [Space(10)]_CubemapIntensity ("Intensity", Range(0, 12)) = 1
        _CubemapBlur ("Blur", Range(0, 8)) = 0
        [Space(10)]_FresnelStrength ("Fresnel Strength", Range(0, 8)) = 0
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _ReflectionTex; uniform float4 _ReflectionTex_ST;
            uniform float4 _Color;
            uniform float _SpecularIntensity1;
            uniform float _Glossiness;
            uniform float _CubemapIntensity;
            uniform float _CubemapBlur;
            uniform float _FresnelStrength;
            uniform samplerCUBE _Cubemap;
            uniform fixed _EnableCubemap;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform fixed _UseDirtmapasGlossmap;
            uniform fixed _ScreenBlendingmode;
            uniform float _Intensity;
            uniform fixed _EnableRGBchannels;
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
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD11;
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
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float node_3386 = (_MainTex_var.r*_Intensity);
                float node_2724 = (1.0 - node_3386);
                float Glossiness = lerp( _Glossiness, (node_2724*_Glossiness), _UseDirtmapasGlossmap );
                float gloss = Glossiness;
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
                float3 CubemapSpec = (_EnableCubemap*((((0.95*pow(1.0-max(0,dot(normalDirection, viewDirection)),1.0))+0.05)*_FresnelStrength)+(_Cubemap_var.rgb*(_Cubemap_var.a*_CubemapIntensity))));
                float3 Specular = (_SpecColor.rgb*_SpecularIntensity1);
                float3 specularColor = Specular;
                float specularMonochrome = max( max(specularColor.r, specularColor.g), specularColor.b);
                float normTerm = (specPow + 8.0 ) / (8.0 * Pi);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*normTerm*specularColor;
                float3 indirectSpecular = (gi.indirect.specular + CubemapSpec)*specularColor;
                float3 specular = (directSpecular + indirectSpecular);
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                float4 _ReflectionTex_var = tex2D(_ReflectionTex,TRANSFORM_TEX(sceneUVs.rg, _ReflectionTex));
                float node_6695 = 1.0;
                float CubemapActive = _EnableCubemap;
                float3 node_4077 = (_Color.rgb*_ReflectionTex_var.rgb*lerp(float3(node_6695,node_6695,node_6695),(_ReflectionTex_var.a+saturate(((1.0 - _ReflectionTex_var.a)*CubemapSpec))),CubemapActive));
                float3 Albedo = lerp( lerp( (node_4077*node_2724*node_2724), saturate((node_4077+node_3386)), _ScreenBlendingmode ), (node_4077*(_MainTex_var.rgb*_Intensity)), _EnableRGBchannels );
                float3 diffuseColor = Albedo;
                diffuseColor *= 1-specularMonochrome;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor,1);
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
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _ReflectionTex; uniform float4 _ReflectionTex_ST;
            uniform float4 _Color;
            uniform float _SpecularIntensity1;
            uniform float _Glossiness;
            uniform float _CubemapIntensity;
            uniform float _CubemapBlur;
            uniform float _FresnelStrength;
            uniform samplerCUBE _Cubemap;
            uniform fixed _EnableCubemap;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform fixed _UseDirtmapasGlossmap;
            uniform fixed _ScreenBlendingmode;
            uniform float _Intensity;
            uniform fixed _EnableRGBchannels;
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
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float node_3386 = (_MainTex_var.r*_Intensity);
                float node_2724 = (1.0 - node_3386);
                float Glossiness = lerp( _Glossiness, (node_2724*_Glossiness), _UseDirtmapasGlossmap );
                float gloss = Glossiness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float3 Specular = (_SpecColor.rgb*_SpecularIntensity1);
                float3 specularColor = Specular;
                float specularMonochrome = max( max(specularColor.r, specularColor.g), specularColor.b);
                float normTerm = (specPow + 8.0 ) / (8.0 * Pi);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*normTerm*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 _ReflectionTex_var = tex2D(_ReflectionTex,TRANSFORM_TEX(sceneUVs.rg, _ReflectionTex));
                float node_6695 = 1.0;
                float4 _Cubemap_var = texCUBElod(_Cubemap,float4(viewReflectDirection,_CubemapBlur));
                float3 CubemapSpec = (_EnableCubemap*((((0.95*pow(1.0-max(0,dot(normalDirection, viewDirection)),1.0))+0.05)*_FresnelStrength)+(_Cubemap_var.rgb*(_Cubemap_var.a*_CubemapIntensity))));
                float CubemapActive = _EnableCubemap;
                float3 node_4077 = (_Color.rgb*_ReflectionTex_var.rgb*lerp(float3(node_6695,node_6695,node_6695),(_ReflectionTex_var.a+saturate(((1.0 - _ReflectionTex_var.a)*CubemapSpec))),CubemapActive));
                float3 Albedo = lerp( lerp( (node_4077*node_2724*node_2724), saturate((node_4077+node_3386)), _ScreenBlendingmode ), (node_4077*(_MainTex_var.rgb*_Intensity)), _EnableRGBchannels );
                float3 diffuseColor = Albedo;
                diffuseColor *= 1-specularMonochrome;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
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
            uniform sampler2D _ReflectionTex; uniform float4 _ReflectionTex_ST;
            uniform float4 _Color;
            uniform float _SpecularIntensity1;
            uniform float _Glossiness;
            uniform float _CubemapIntensity;
            uniform float _CubemapBlur;
            uniform float _FresnelStrength;
            uniform samplerCUBE _Cubemap;
            uniform fixed _EnableCubemap;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform fixed _UseDirtmapasGlossmap;
            uniform fixed _ScreenBlendingmode;
            uniform float _Intensity;
            uniform fixed _EnableRGBchannels;
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
                
                float4 _ReflectionTex_var = tex2D(_ReflectionTex,TRANSFORM_TEX(sceneUVs.rg, _ReflectionTex));
                float node_6695 = 1.0;
                float4 _Cubemap_var = texCUBElod(_Cubemap,float4(viewReflectDirection,_CubemapBlur));
                float3 CubemapSpec = (_EnableCubemap*((((0.95*pow(1.0-max(0,dot(normalDirection, viewDirection)),1.0))+0.05)*_FresnelStrength)+(_Cubemap_var.rgb*(_Cubemap_var.a*_CubemapIntensity))));
                float CubemapActive = _EnableCubemap;
                float3 node_4077 = (_Color.rgb*_ReflectionTex_var.rgb*lerp(float3(node_6695,node_6695,node_6695),(_ReflectionTex_var.a+saturate(((1.0 - _ReflectionTex_var.a)*CubemapSpec))),CubemapActive));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float node_3386 = (_MainTex_var.r*_Intensity);
                float node_2724 = (1.0 - node_3386);
                float3 Albedo = lerp( lerp( (node_4077*node_2724*node_2724), saturate((node_4077+node_3386)), _ScreenBlendingmode ), (node_4077*(_MainTex_var.rgb*_Intensity)), _EnableRGBchannels );
                float3 diffColor = Albedo;
                float3 Specular = (_SpecColor.rgb*_SpecularIntensity1);
                float3 specColor = Specular;
                float Glossiness = lerp( _Glossiness, (node_2724*_Glossiness), _UseDirtmapasGlossmap );
                float roughness = 1.0 - Glossiness;
                o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
