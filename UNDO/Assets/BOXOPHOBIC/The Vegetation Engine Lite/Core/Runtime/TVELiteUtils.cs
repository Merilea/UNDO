// Cristian Pop - https://boxophobic.com/

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TheVegetationEngineLite
{
    public class TVELiteUtils
    {
        public static void SetMaterialSettings(Material material)
        {
            if (!material.HasProperty("_IsLiteShader"))
            {
                return;
            }

            var shaderName = material.shader.name;

            if (material.HasProperty("_IsVersion"))
            {
                var version = material.GetInt("_IsVersion");

                if (version < 1100)
                {
                    if (material.shader.name == ("BOXOPHOBIC/The Vegetation Engine Lite/Geometry/Plant Standard Lit"))
                    {
                        material.SetFloat("_SubsurfaceValue", 0);
                        material.SetFloat("_MotionValue_30", 0);
                    }

                    if (material.HasProperty("_DetailMeshMode"))
                    {
                        var mode = material.GetInt("_DetailMeshMode");

                        if (mode == 0)
                        {
                            material.SetFloat("_DetailMeshMode", 30);
                        }

                        if (mode == 1)
                        {
                            material.SetFloat("_DetailMeshMode", 0);
                        }
                    }

                    material.SetInt("_IsVersion", 1100);
                }

                // Bumped version because 1200 was used before by mistake
                if (version < 1201)
                {
                    if (material.HasProperty("_EmissiveColor"))
                    {
                        var color = material.GetColor("_EmissiveColor");

                        if (material.GetColor("_EmissiveColor").r > 0 || material.GetColor("_EmissiveColor").g > 0 || material.GetColor("_EmissiveColor").b > 0)
                        {
                            material.SetInt("_EmissiveMode", 1);
                        }
                    }

                    material.SetInt("_IsVersion", 1201);
                }

                // Refresh is needed to apply new keywords
                if (version < 1230)
                {
                    material.SetInt("_IsVersion", 1230);
                }
            }

            // Set Internal Render Values
            if (material.HasProperty("_RenderMode"))
            {
                material.SetInt("_render_mode", material.GetInt("_RenderMode"));
            }

            if (material.HasProperty("_RenderCull"))
            {
                material.SetInt("_render_cull", material.GetInt("_RenderCull"));
            }

            if (material.HasProperty("_RenderZWrite"))
            {
                material.SetInt("_render_zw", material.GetInt("_RenderZWrite"));
            }

            if (material.HasProperty("_RenderClip"))
            {
                material.SetInt("_render_clip", material.GetInt("_RenderClip"));
            }

            if (material.HasProperty("_RenderSpecular"))
            {
                material.SetInt("_render_specular", material.GetInt("_RenderSpecular"));
            }

            // Set Render Mode
            if (material.HasProperty("_RenderMode"))
            {
                int mode = material.GetInt("_RenderMode");
                int zwrite = material.GetInt("_RenderZWrite");
                int queue = 0;
                int priority = 0;
                int decals = 0;
                int clip = 0;

                if (material.HasProperty("_RenderQueue") && material.HasProperty("_RenderPriority"))
                {
                    queue = material.GetInt("_RenderQueue");
                    priority = material.GetInt("_RenderPriority");
                }

                if (material.GetTag("RenderPipeline", false) == "HDRenderPipeline")
                {
                    if (material.HasProperty("_RenderDecals"))
                    {
                        decals = material.GetInt("_RenderDecals");
                    }
                }

                if (material.HasProperty("_RenderClip"))
                {
                    clip = material.GetInt("_RenderClip");
                }

                // User Defined, render type changes needed
                if (queue == 2)
                {
                    if (material.renderQueue == 2000)
                    {
                        material.SetOverrideTag("RenderType", "Opaque");
                    }

                    if (material.renderQueue > 2449 && material.renderQueue < 3000)
                    {
                        material.SetOverrideTag("RenderType", "AlphaTest");
                    }

                    if (material.renderQueue > 2999)
                    {
                        material.SetOverrideTag("RenderType", "Transparent");
                    }
                }

                // Opaque
                if (mode == 0)
                {
                    if (queue != 2)
                    {
                        material.SetOverrideTag("RenderType", "AlphaTest");
                        //material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest + priority;

                        if (clip == 0)
                        {
                            if (decals == 0)
                            {
                                material.renderQueue = 2000 + priority;
                            }
                            else
                            {
                                material.renderQueue = 2225 + priority;
                            }
                        }
                        else
                        {
                            if (decals == 0)
                            {
                                material.renderQueue = 2450 + priority;
                            }
                            else
                            {
                                material.renderQueue = 2475 + priority;
                            }
                        }
                    }

                    // Standard and Universal Render Pipeline
                    material.SetInt("_render_src", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_render_dst", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_render_zw", 1);
                    material.SetInt("_render_premul", 0);

                    // Set Main Color alpha to 1
                    if (material.HasProperty("_MainColor"))
                    {
                        var color = material.GetColor("_MainColor");
                        material.SetColor("_MainColor", new Color(color.r, color.g, color.b, 1.0f));
                    }

                    if (material.HasProperty("_MainColorTwo"))
                    {
                        var color = material.GetColor("_MainColorTwo");
                        material.SetColor("_MainColorTwo", new Color(color.r, color.g, color.b, 1.0f));
                    }

                    // HD Render Pipeline
                    material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
                    material.DisableKeyword("_ENABLE_FOG_ON_TRANSPARENT");

                    material.DisableKeyword("_BLENDMODE_ALPHA");
                    material.DisableKeyword("_BLENDMODE_ADD");
                    material.DisableKeyword("_BLENDMODE_PRE_MULTIPLY");

                    material.SetInt("_RenderQueueType", 1);
                    material.SetInt("_SurfaceType", 0);
                    material.SetInt("_BlendMode", 0);
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_AlphaSrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_AlphaDstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.SetInt("_TransparentZWrite", 1);
                    material.SetInt("_ZTestDepthEqualForOpaque", 3);

                    if (clip == 0)
                    {
                        material.SetInt("_ZTestGBuffer", 4);
                    }
                    else
                    {
                        material.SetInt("_ZTestGBuffer", 3);
                    }

                    //material.SetInt("_ZTestGBuffer", 4);
                    material.SetInt("_ZTestTransparent", 4);

                    material.SetShaderPassEnabled("TransparentBackface", false);
                    material.SetShaderPassEnabled("TransparentBackfaceDebugDisplay", false);
                    material.SetShaderPassEnabled("TransparentDepthPrepass", false);
                    material.SetShaderPassEnabled("TransparentDepthPostpass", false);
                }
                // Transparent
                else
                {
                    if (queue != 2)
                    {
                        material.SetOverrideTag("RenderType", "Transparent");
                        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent + priority;
                    }

                    // Standard and Universal Render Pipeline
                    material.SetInt("_render_src", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_render_dst", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_render_premul", 0);

                    // HD Render Pipeline
                    material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                    material.EnableKeyword("_ENABLE_FOG_ON_TRANSPARENT");

                    material.EnableKeyword("_BLENDMODE_ALPHA");
                    material.DisableKeyword("_BLENDMODE_ADD");
                    material.DisableKeyword("_BLENDMODE_PRE_MULTIPLY");

                    material.SetInt("_RenderQueueType", 5);
                    material.SetInt("_SurfaceType", 1);
                    material.SetInt("_BlendMode", 0);
                    material.SetInt("_SrcBlend", 1);
                    material.SetInt("_DstBlend", 10);
                    material.SetInt("_AlphaSrcBlend", 1);
                    material.SetInt("_AlphaDstBlend", 10);
                    material.SetInt("_ZWrite", zwrite);
                    material.SetInt("_TransparentZWrite", zwrite);
                    material.SetInt("_ZTestDepthEqualForOpaque", 4);
                    material.SetInt("_ZTestGBuffer", 4);
                    material.SetInt("_ZTestTransparent", 4);

                    material.SetShaderPassEnabled("TransparentBackface", true);
                    material.SetShaderPassEnabled("TransparentBackfaceDebugDisplay", true);
                    material.SetShaderPassEnabled("TransparentDepthPrepass", true);
                    material.SetShaderPassEnabled("TransparentDepthPostpass", true);
                }
            }

            if (shaderName.Contains("Prop"))
            {
                material.SetShaderPassEnabled("MotionVectors", false);
            }
            else
            {
                material.SetShaderPassEnabled("MotionVectors", true);
            }

            // Set Receive Mode in HDRP
            if (material.GetTag("RenderPipeline", false) == "HDRenderPipeline")
            {
                if (material.HasProperty("_RenderDecals"))
                {
                    int decals = material.GetInt("_RenderDecals");

                    if (decals == 0)
                    {
                        material.EnableKeyword("_DISABLE_DECALS");
                    }
                    else
                    {
                        material.DisableKeyword("_DISABLE_DECALS");
                    }
                }

                if (material.HasProperty("_RenderSSR"))
                {
                    int ssr = material.GetInt("_RenderSSR");

                    if (ssr == 0)
                    {
                        material.EnableKeyword("_DISABLE_SSR");

                        material.SetInt("_StencilRef", 0);
                        material.SetInt("_StencilRefDepth", 0);
                        material.SetInt("_StencilRefDistortionVec", 4);
                        material.SetInt("_StencilRefGBuffer", 2);
                        material.SetInt("_StencilRefMV", 32);
                        material.SetInt("_StencilWriteMask", 6);
                        material.SetInt("_StencilWriteMaskDepth", 8);
                        material.SetInt("_StencilWriteMaskDistortionVec", 4);
                        material.SetInt("_StencilWriteMaskGBuffer", 14);
                        material.SetInt("_StencilWriteMaskMV", 40);
                    }
                    else
                    {
                        material.DisableKeyword("_DISABLE_SSR");

                        material.SetInt("_StencilRef", 0);
                        material.SetInt("_StencilRefDepth", 8);
                        material.SetInt("_StencilRefDistortionVec", 4);
                        material.SetInt("_StencilRefGBuffer", 10);
                        material.SetInt("_StencilRefMV", 40);
                        material.SetInt("_StencilWriteMask", 6);
                        material.SetInt("_StencilWriteMaskDepth", 8);
                        material.SetInt("_StencilWriteMaskDistortionVec", 4);
                        material.SetInt("_StencilWriteMaskGBuffer", 14);
                        material.SetInt("_StencilWriteMaskMV", 40);
                    }
                }
            }

            // Set Cull Mode
            if (material.HasProperty("_RenderCull"))
            {
                int cull = material.GetInt("_RenderCull");

                material.SetInt("_CullMode", cull);
                material.SetInt("_TransparentCullMode", cull);
                material.SetInt("_CullModeForward", cull);

                // Needed for HD Render Pipeline
                material.DisableKeyword("_DOUBLESIDED_ON");
            }

            // Set Clip Mode
            if (material.HasProperty("_RenderClip"))
            {
                int clip = material.GetInt("_RenderClip");
                float cutoff = 0.5f;

                if (material.HasProperty("_AlphaClipValue"))
                {
                    cutoff = material.GetFloat("_AlphaClipValue");
                }

                if (clip == 0)
                {
                    material.DisableKeyword("TVE_ALPHA_CLIP");

                    material.SetInt("_render_coverage", 0);
                }
                else
                {
                    material.EnableKeyword("TVE_ALPHA_CLIP");

                    if (material.HasProperty("_RenderCoverage") && material.HasProperty("_AlphaFeatherValue"))
                    {
                        material.SetInt("_render_coverage", material.GetInt("_RenderCoverage"));
                    }
                    else
                    {
                        material.SetInt("_render_coverage", 0);
                    }
                }

                material.SetFloat("_Cutoff", cutoff);

                // HD Render Pipeline
                material.SetFloat("_AlphaCutoff", cutoff);
                material.SetFloat("_AlphaCutoffPostpass", cutoff);
                material.SetFloat("_AlphaCutoffPrepass", cutoff);
                material.SetFloat("_AlphaCutoffShadow", cutoff);
            }

            // Set Normals Mode
            if (material.HasProperty("_RenderNormals") && material.HasProperty("_render_normals"))
            {
                int normals = material.GetInt("_RenderNormals");

                // Standard, Universal, HD Render Pipeline
                // Flip 0
                if (normals == 0)
                {
                    material.SetVector("_render_normals", new Vector4(-1, -1, -1, 0));
                    material.SetVector("_DoubleSidedConstants", new Vector4(-1, -1, -1, 0));
                }
                // Mirror 1
                else if (normals == 1)
                {
                    material.SetVector("_render_normals", new Vector4(1, 1, -1, 0));
                    material.SetVector("_DoubleSidedConstants", new Vector4(1, 1, -1, 0));
                }
                // None 2
                else if (normals == 2)
                {
                    material.SetVector("_render_normals", new Vector4(1, 1, 1, 0));
                    material.SetVector("_DoubleSidedConstants", new Vector4(1, 1, 1, 0));
                }
            }

#if UNITY_EDITOR
            // Assign Default HD Foliage profile
            if (material.HasProperty("_SubsurfaceDiffusion"))
            {
                // Workaround when the old HDRP 12 diffusion is not found
                if (material.GetFloat("_SubsurfaceDiffusion") == 3.5648174285888672f && AssetDatabase.GUIDToAssetPath("78322c7f82657514ebe48203160e3f39") == "")
                {
                    material.SetFloat("_SubsurfaceDiffusion", 0);
                }

                // Workaround when the old HDRP 14 diffusion is not found
                if (material.GetFloat("_SubsurfaceDiffusion") == 2.6486763954162598f && AssetDatabase.GUIDToAssetPath("879ffae44eefa4412bb327928f1a96dd") == "")
                {
                    material.SetFloat("_SubsurfaceDiffusion", 0);
                }

                // Search for one of Unity's diffusion profile
                if (material.GetFloat("_SubsurfaceDiffusion") == 0)
                {
                    // HDRP 12 Profile
                    if (AssetDatabase.GUIDToAssetPath("78322c7f82657514ebe48203160e3f39") != "")
                    {
                        material.SetFloat("_SubsurfaceDiffusion", 3.5648174285888672f);
                        material.SetVector("_SubsurfaceDiffusion_Asset", new Vector4(228889264007084710000000000000000000000f, 0.000000000000000000000000012389357880079404f, 0.00000000000000000000000000000000000076932702684439582f, 0.00018220426863990724f));
                    }

                    // HDRP 14 Profile
                    if (AssetDatabase.GUIDToAssetPath("879ffae44eefa4412bb327928f1a96dd") != "")
                    {
                        material.SetFloat("_SubsurfaceDiffusion", 2.6486763954162598f);
                        material.SetVector("_SubsurfaceDiffusion_Asset", new Vector4(-36985449400010195000000f, 20.616847991943359f, -0.00000000000000000000000000052916750040661612f, -1352014335655804900f));
                    }

                    // HDRP 16 Profile
                    if (AssetDatabase.GUIDToAssetPath("2384dbf2c1c420f45a792fbc315fbfb1") != "")
                    {
                        material.SetFloat("_SubsurfaceDiffusion", 3.8956573009490967f);
                        material.SetVector("_SubsurfaceDiffusion_Asset", new Vector4(-8695930962161997000000000000000f, -50949593547561853000000000000000f, -0.010710084810853004f, -0.0000000055696536271909736f));
                    }
                }
            }
#endif

            // Set Detail Mode
            if (material.HasProperty("_DetailMode") && material.HasProperty("_SecondColor"))
            {
                if (material.GetInt("_DetailMode") == 0)
                {
                    material.DisableKeyword("TVE_DETAIL");
                }
                else
                {
                    material.EnableKeyword("TVE_DETAIL");
                }

                if (material.HasProperty("_SecondUVsMode"))
                {
                    var mode = material.GetInt("_SecondUVsMode");

                    // Main
                    if (mode == 0)
                    {
                        material.SetVector("_second_uvs_mode", new Vector4(1, 0, 0, 0));
                    }
                    // Baked
                    else if (mode == 1)
                    {
                        material.SetVector("_second_uvs_mode", new Vector4(0, 1, 0, 0));
                    }
                    // Planar
                    else if (mode == 2)
                    {
                        material.SetVector("_second_uvs_mode", new Vector4(0, 0, 1, 0));
                    }
                    // Unused
                    else if (mode == 3)
                    {
                        material.SetVector("_second_uvs_mode", new Vector4(0, 0, 0, 1));
                    }
                }

                if (material.HasProperty("_DetailMeshMode"))
                {
                    var mode = material.GetInt("_DetailMeshMode");

                    if (mode == 10)
                    {
                        material.SetVector("_detail_mesh_mode", new Vector4(1, 0, 0, 0));
                    }
                    else if (mode == 20)
                    {
                        material.SetVector("_detail_mesh_mode", new Vector4(0, 1, 0, 0));
                    }
                    else if (mode == 30)
                    {
                        material.SetVector("_detail_mesh_mode", new Vector4(0, 0, 1, 0));
                    }
                    else if (mode == 40)
                    {
                        material.SetVector("_detail_mesh_mode", new Vector4(0, 0, 0, 1));
                    }
                }
            }

            if (material.HasProperty("_emissive_intensity_value"))
            {
                // Set Emissive Mode
                if (material.HasProperty("_EmissiveMode"))
                {
                    if (material.GetInt("_EmissiveMode") == 0)
                    {
                        material.DisableKeyword("TVE_EMISSIVE");
                    }
                    else
                    {
                        material.EnableKeyword("TVE_EMISSIVE");
                    }
                }

                // Set Intensity Mode
                if (material.HasProperty("_EmissiveIntensityMode") && material.HasProperty("_EmissiveIntensityValue"))
                {
                    float mode = material.GetInt("_EmissiveIntensityMode");
                    float value = material.GetFloat("_EmissiveIntensityValue");

                    if (mode == 0)
                    {
                        material.SetFloat("_emissive_intensity_value", value);
                    }
                    else if (mode == 1)
                    {
                        material.SetFloat("_emissive_intensity_value", (12.5f / 100.0f) * Mathf.Pow(2f, value));
                    }
                }

                // Set GI Mode
                if (material.HasProperty("_EmissiveFlagMode"))
                {
                    int mode = material.GetInt("_EmissiveFlagMode");

                    if (mode == 0)
                    {
                        material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
                    }
                    else if (mode == 1)
                    {
                        material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.AnyEmissive;
                    }
                    else if (mode == 2)
                    {
                        material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.BakedEmissive;
                    }
                    else if (mode == 3)
                    {
                        material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                    }
                }
            }

            if (material.HasProperty("_VertexOcclusionMaskMode"))
            {
                var mode = material.GetInt("_VertexOcclusionMaskMode");

                if (mode == 10)
                {
                    material.SetVector("_vertex_occlusion_mask_mode", new Vector4(1, 0, 0, 0));
                }
                else if (mode == 20)
                {
                    material.SetVector("_vertex_occlusion_mask_mode", new Vector4(0, 1, 0, 0));
                }
                else if (mode == 30)
                {
                    material.SetVector("_vertex_occlusion_mask_mode", new Vector4(0, 0, 1, 0));
                }
                else if (mode == 40)
                {
                    material.SetVector("_vertex_occlusion_mask_mode", new Vector4(0, 0, 0, 1));
                }
            }

            if (material.HasProperty("_MotionVariationMode"))
            {
                var mode = material.GetInt("_MotionVariationMode");

                if (mode == 10)
                {
                    material.SetVector("_motion_variation_mode", new Vector4(1, 0, 0, 0));
                }
                else if (mode == 20)
                {
                    material.SetVector("_motion_variation_mode", new Vector4(0, 1, 0, 0));
                }
                else if (mode == 30)
                {
                    material.SetVector("_motion_variation_mode", new Vector4(0, 0, 1, 0));
                }
                else if (mode == 40)
                {
                    material.SetVector("_motion_variation_mode", new Vector4(0, 0, 0, 1));
                }
            }

            if (material.HasProperty("_MotionMaskMode_20"))
            {
                var mode = material.GetInt("_MotionMaskMode_20");

                if (mode == 10)
                {
                    material.SetVector("_motion_mask_mode_20", new Vector4(1, 0, 0, 0));
                }
                else if (mode == 20)
                {
                    material.SetVector("_motion_mask_mode_20", new Vector4(0, 1, 0, 0));
                }
                else if (mode == 30)
                {
                    material.SetVector("_motion_mask_mode_20", new Vector4(0, 0, 1, 0));
                }
                else if (mode == 40)
                {
                    material.SetVector("_motion_mask_mode_20", new Vector4(0, 0, 0, 1));
                }
            }

            if (material.HasProperty("_MotionMaskMode_30"))
            {
                var mode = material.GetInt("_MotionMaskMode_30");

                if (mode == 10)
                {
                    material.SetVector("_motion_mask_mode_30", new Vector4(1, 0, 0, 0));
                }
                else if (mode == 20)
                {
                    material.SetVector("_motion_mask_mode_30", new Vector4(0, 1, 0, 0));
                }
                else if (mode == 30)
                {
                    material.SetVector("_motion_mask_mode_30", new Vector4(0, 0, 1, 0));
                }
                else if (mode == 40)
                {
                    material.SetVector("_motion_mask_mode_30", new Vector4(0, 0, 0, 1));
                }
            }

            // Set Legacy props for external bakers
            if (material.HasProperty("_AlphaClipValue"))
            {
                material.SetFloat("_Cutoff", material.GetFloat("_AlphaClipValue"));
            }

            // Set Legacy props for external bakers
            if (material.HasProperty("_MainColor"))
            {
                material.SetColor("_Color", material.GetColor("_MainColor"));
            }

            // Set BlinnPhong Spec Color
            if (material.HasProperty("_SpecColor"))
            {
                material.SetColor("_SpecColor", Color.white);
            }

            if (material.HasProperty("_MainAlbedoTex"))
            {
                material.SetTexture("_MainTex", material.GetTexture("_MainAlbedoTex"));
            }

            if (material.HasProperty("_MainNormalTex"))
            {
                material.SetTexture("_BumpMap", material.GetTexture("_MainNormalTex"));
            }

            if (material.HasProperty("_MainUVs"))
            {
                material.SetTextureScale("_MainTex", new Vector2(material.GetVector("_MainUVs").x, material.GetVector("_MainUVs").y));
                material.SetTextureOffset("_MainTex", new Vector2(material.GetVector("_MainUVs").z, material.GetVector("_MainUVs").w));

                material.SetTextureScale("_BumpMap", new Vector2(material.GetVector("_MainUVs").x, material.GetVector("_MainUVs").y));
                material.SetTextureOffset("_BumpMap", new Vector2(material.GetVector("_MainUVs").z, material.GetVector("_MainUVs").w));
            }

            if (material.HasProperty("_SubsurfaceValue"))
            {
                // Subsurface Standard Render Pipeline
                material.SetFloat("_Translucency", material.GetFloat("_SubsurfaceScatteringValue"));
                material.SetFloat("_TransScattering", material.GetFloat("_SubsurfaceAngleValue"));
                material.SetFloat("_TransNormalDistortion", material.GetFloat("_SubsurfaceNormalValue"));
                material.SetFloat("_TransDirect", material.GetFloat("_SubsurfaceDirectValue"));
                material.SetFloat("_TransAmbient", material.GetFloat("_SubsurfaceAmbientValue"));
                material.SetFloat("_TransShadow", material.GetFloat("_SubsurfaceShadowValue"));

                //Subsurface Universal Render Pipeline
                material.SetFloat("_TransStrength", material.GetFloat("_SubsurfaceScatteringValue"));
                material.SetFloat("_TransNormal", material.GetFloat("_SubsurfaceNormalValue"));
            }

#if UNITY_EDITOR
            // Set internals for impostor baking 
            if (material.HasProperty("_VertexOcclusionColor"))
            {
                material.SetInt("_HasOcclusion", 1);
            }
            else
            {
                material.SetInt("_HasOcclusion", 0);
            }

            if (material.HasProperty("_GradientColorOne"))
            {
                material.SetInt("_HasGradient", 1);
            }
            else
            {
                material.SetInt("_HasGradient", 0);
            }

            if (material.HasProperty("_emissive_intensity_value"))
            {
                material.SetInt("_HasEmissive", 1);
            }
            else
            {
                material.SetInt("_HasEmissive", 0);
            }
#endif
        }

#if UNITY_EDITOR
        // Material Utils
        public static void UnloadMaterialFromMemory(Material material)
        {
            var shader = material.shader;

            for (int i = 0; i < ShaderUtil.GetPropertyCount(shader); i++)
            {
                if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
                {
                    var propName = ShaderUtil.GetPropertyName(shader, i);
                    var texture = material.GetTexture(propName);

                    if (texture != null)
                    {
                        Resources.UnloadAsset(texture);
                    }
                }
            }
        }
#endif
    }
}
