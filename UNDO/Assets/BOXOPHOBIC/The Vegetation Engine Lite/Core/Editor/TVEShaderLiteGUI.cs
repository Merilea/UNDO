//Cristian Pop - https://boxophobic.com/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Boxophobic.StyledGUI;
using TheVegetationEngineLite;

public class TVEShaderLiteGUI : ShaderGUI
{
    bool multiSelection = false;

    public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
    {
        base.AssignNewShaderToMaterial(material, oldShader, newShader);

        TVELiteUtils.SetMaterialSettings(material);
    }

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
    {
        var material0 = materialEditor.target as Material;
        var materials = materialEditor.targets;

        if (materials.Length > 1)
            multiSelection = true;

        // Used for impostors only
        if (material0.HasProperty("_IsInitialized"))
        {
            if (material0.GetFloat("_IsInitialized") > 0)
            {
                DrawDynamicInspector(material0, materialEditor, props);
            }
            else
            {
                DrawInitInspector(material0);
            }
        }
        else
        {
            DrawDynamicInspector(material0, materialEditor, props);
        }

        foreach (Material material in materials)
        {
            TVELiteUtils.SetMaterialSettings(material);
        }
    }

    void DrawDynamicInspector(Material material, MaterialEditor materialEditor, MaterialProperty[] props)
    {
        var splitLine = material.shader.name.Split(char.Parse("/"));
        var splitCount = splitLine.Length;
        var title = splitLine[splitCount - 1];
        var subtitle = splitLine[splitCount - 2];

        StyledGUI.DrawInspectorBanner(title, "Lite / " + subtitle);

        var customPropsList = new List<MaterialProperty>();

        if (multiSelection)
        {
            for (int i = 0; i < props.Length; i++)
            {
                var prop = props[i];

                if (prop.flags == MaterialProperty.PropFlags.HideInInspector)
                    continue;

                if (prop.name == "unity_Lightmaps")
                    continue;

                if (prop.name == "unity_LightmapsInd")
                    continue;

                if (prop.name == "unity_ShadowMasks")
                    continue;

                customPropsList.Add(prop);
            }
        }
        else
        {
            for (int i = 0; i < props.Length; i++)
            {
                var prop = props[i];
                var internalName = prop.name;

                if (prop.flags == MaterialProperty.PropFlags.HideInInspector)
                {
                    continue;
                }

                if (GetPropertyVisibility(material, internalName))
                {
                    customPropsList.Add(prop);
                }
            }
        }

        //Draw Custom GUI
        for (int i = 0; i < customPropsList.Count; i++)
        {
            var property = customPropsList[i];
            var displayName = GetPropertyDisplay(material, property);

            if (customPropsList[i].type == MaterialProperty.PropType.Texture)
            {
                materialEditor.TexturePropertySingleLine(new GUIContent(displayName, ""), customPropsList[i]);
            }
            else
            {
                materialEditor.ShaderProperty(customPropsList[i], displayName);
            }
        }

        GUILayout.Space(10);

        StyledGUI.DrawInspectorCategory("Advanced Settings");

        GUILayout.Space(10);

        DrawPivotsSupport(material);
        materialEditor.EnableInstancingField();

        GUILayout.Space(10);

        DrawRenderQueue(material, materialEditor);

        GUILayout.Space(10);

        DrawCopySettingsFromGameObject(material);

        GUILayout.Space(15);

        DrawPoweredByTheVegetationEngine();
    }

    void DrawInitInspector(Material material)
    {
        var splitLine = material.shader.name.Split(char.Parse("/"));
        var splitCount = splitLine.Length;
        var title = splitLine[splitCount - 1];

        StyledGUI.DrawInspectorBanner(title, "Lite | Impostors");

        GUILayout.Space(5);

        EditorGUILayout.HelpBox("The original material properties are not copied to the Impostor material. Drag the game object the impostor is baked from to the field below to copy the properties!", MessageType.Error, true);

        GUILayout.Space(10);

        DrawCopySettingsFromGameObject(material);

        GUILayout.Space(10);
    }

    bool GetPropertyVisibility(Material material, string internalName)
    {
        bool valid = true;
        var shaderName = material.shader.name;

        if (internalName == "unity_Lightmaps")
            valid = false;

        if (internalName == "unity_LightmapsInd")
            valid = false;

        if (internalName == "unity_ShadowMasks")
            valid = false;

        if (internalName.Contains("_Banner"))
            valid = false;

        if (internalName == "_SpecColor")
            valid = false;

        if (material.HasProperty("_RenderMode"))
        {
            if (material.GetInt("_RenderMode") == 0 && internalName == "_RenderZWrite")
                valid = false;
        }

        bool hasRenderNormals = false;

        if (material.HasProperty("_render_normals"))
        {
            hasRenderNormals = true;
        }

        if (!hasRenderNormals)
        {
            if (internalName == "_RenderNormals")
                valid = false;
        }

        //if (!shaderName.Contains("Vertex Lit"))
        //{
        //    if (internalName == "_RenderDirect")
        //        valid = false;
        //    if (internalName == "_RenderShadow")
        //        valid = false;
        //    if (internalName == "_RenderAmbient")
        //        valid = false;
        //}

        if (material.HasProperty("_RenderCull"))
        {
            if (material.GetInt("_RenderCull") == 2 && internalName == "_RenderNormals")
                valid = false;
        }

        if (material.HasProperty("_RenderClip"))
        {
            if (material.GetInt("_RenderClip") == 0)
            {
                if (internalName == "_AlphaClipValue")
                    valid = false;
                //if (internalName == "_AlphaFeatherValue")
                //    valid = false;
                //if (internalName == "_FadeMode")
                //    valid = false;
                //if (internalName == "_FadeGlobalValue")
                //    valid = false;
                //if (internalName == "_FadeConstantValue")
                //    valid = false;
                //if (internalName == "_FadeCameraValue")
                //    valid = false;
                //if (internalName == "_FadeGlancingValue")
                //    valid = false;
                //if (internalName == "_FadeHorizontalValue")
                //    valid = false;
                //if (internalName == "_FadeVerticalValue")
                //    valid = false;
                //if (internalName == "_SpaceRenderFade")
                //    valid = false;
                //if (internalName == "_MainAlphaValue")
                //    valid = false;
                //if (internalName == "_SecondAlphaValue")
                //    valid = false;
                //if (internalName == "_DetailAlphaMode")
                //    valid = false;
                //if (internalName == "_DetailFadeMode")
                //    valid = false;
                //if (internalName == "_EmissiveAlphaValue")
                //    valid = false;
            }
        }

        if (!material.HasProperty("_AlphaFeatherValue"))
        {
            if (internalName == "_RenderCoverage")
                valid = false;
        }

        if (material.GetTag("RenderPipeline", false) != "HDRenderPipeline")
        {
            if (internalName == "_RenderDecals")
                valid = false;
            if (internalName == "_RenderSSR")
                valid = false;
        }

        if (material.HasProperty("_RenderPacked"))
        {
            if (material.GetInt("_RenderPacked") == 1 && internalName == "_MainOcclusionValue")
                valid = false;
            if (material.GetInt("_RenderPacked") == 1 && internalName == "_SecondOcclusionValue")
                valid = false;
            if (material.GetInt("_RenderPacked") == 1 && internalName == "_TerrainOcclusionValue")
                valid = false;
        }

        bool showFadeSpace = false;

        if (material.HasProperty("_FadeGlobalValue") || material.HasProperty("_FadeConstantValue") || material.HasProperty("_FadeCameraValue") || material.HasProperty("_FadeGlancingValue") || material.HasProperty("_FadeHorizontalValue"))
        {
            showFadeSpace = true;
        }

        if (!showFadeSpace)
        {
            if (internalName == "_SpaceRenderFade")
                valid = false;
        }

        bool showGlobalsCat = false;

        if (material.HasProperty("_GlobalColors") || material.HasProperty("_GlobalOverlay") || material.HasProperty("_GlobalWetness") || material.HasProperty("_GlobalEmissive") || material.HasProperty("_GlobalSize") || material.HasProperty("_GlobalConform") || material.HasProperty("_GlobalHeight"))
        {
            showGlobalsCat = true;
        }

        if (!showGlobalsCat)
        {
            if (internalName == "_CategoryGlobals")
                valid = false;
        }

        bool showGlobalLayers = false;

        if (material.HasProperty("_LayerColorsValue") || material.HasProperty("_LayerExtrasValue") || material.HasProperty("_LayerMotionValue") || material.HasProperty("_LayerReactValue"))
        {
            showGlobalLayers = true;
        }

        if (!showGlobalLayers)
        {
            if (internalName == "_SpaceGlobalLayers")
                valid = false;
        }

        bool showGlobalLocals = false;

        if (material.HasProperty("_ColorsIntensityValue") || material.HasProperty("_ColorsVariationValue") || material.HasProperty("_AlphaVariationValue") || material.HasProperty("_OverlayVariationValue") || material.HasProperty("_OverlayProjectionValue") || material.HasProperty("_ConformOffsetValue"))
        {
            showGlobalLocals = true;
        }

        if (!showGlobalLocals)
        {
            if (internalName == "_SpaceGlobalLocals")
                valid = false;
        }

        bool showGlobalOptions = false;

        if (material.HasProperty("_ColorsPositionMode") || material.HasProperty("_ExtrasPositionMode"))
        {
            showGlobalOptions = true;
        }

        if (!showGlobalOptions)
        {
            if (internalName == "_SpaceGlobalOptions")
                valid = false;
        }

        bool showMainMaskMessage = false;

        if (material.HasProperty("_MainMaskMinValue"))
        {
            showMainMaskMessage = true;
        }

        if (!showMainMaskMessage)
        {
            if (internalName == "_MessageMainMask")
                valid = false;
        }

        if (material.HasProperty("_MainColorMode"))
        {
            if (material.GetInt("_MainColorMode") == 0)
            {
                if (internalName == "_MainColorTwo")
                    valid = false;
            }
        }

        if (!material.HasProperty("_SecondColor"))
        {
            if (internalName == "_CategoryDetail")
                valid = false;
            if (internalName == "_SecondUVsMode")
                valid = false;
        }

        if (material.HasProperty("_SecondColorMode"))
        {
            if (material.GetInt("_SecondColorMode") == 0)
            {
                if (internalName == "_SecondColorTwo")
                    valid = false;
            }
        }

        //bool showTerrainSettings = false;

        //if (material.HasProperty("_ThirdColor") || material.HasProperty("_TerrainColor"))
        //{
        //    showTerrainSettings = true;
        //}

        //if (!showTerrainSettings)
        //{
        //    if (internalName == "_CategoryTerrain")
        //        valid = false;
        //    if (internalName == "_MessageTerrain")
        //        valid = false;
        //    if (internalName == "_ThirdUVsMode")
        //        valid = false;
        //}

        bool showSecondMaskMessage = false;

        if (material.HasProperty("_SecondMaskMinValue"))
        {
            showSecondMaskMessage = true;
        }

        if (!showSecondMaskMessage)
        {
            if (internalName == "_MessageSecondMask")
                valid = false;
        }

        if (!material.HasProperty("_VertexOcclusionColor"))
        {
            if (internalName == "_CategoryOcclusion")
                valid = false;
            if (internalName == "_EndOcclusion")
                valid = false;
            if (internalName == "_MessageOcclusion")
                valid = false;
        }

        if (!material.HasProperty("_GradientColorOne"))
        {
            if (internalName == "_CategoryGradient")
                valid = false;
        }

        if (!material.HasProperty("_NoiseColorOne"))
        {
            if (internalName == "_CategoryNoise")
                valid = false;
        }

        if (!material.HasProperty("_MatcapValue"))
        {
            if (internalName == "_CategoryMatcap")
                valid = false;
        }

        if (!material.HasProperty("_RimLightColor"))
        {
            if (internalName == "_CategoryRimLight")
                valid = false;
        }

        if (material.HasProperty("_SubsurfaceValue"))
        {
            if (material.GetTag("RenderPipeline", false) != "HDRenderPipeline" || shaderName.Contains("Standard"))
            {
                if (internalName == "_SubsurfaceDiffusion")
                    valid = false;
                if (internalName == "_SpaceSubsurface")
                    valid = false;
                if (internalName == "_MessageSubsurface")
                    valid = false;
            }

            // Standard Render Pipeline
            if (internalName == "_Translucency")
                valid = false;
            if (internalName == "_TransNormalDistortion")
                valid = false;
            if (internalName == "_TransScattering")
                valid = false;
            if (internalName == "_TransDirect")
                valid = false;
            if (internalName == "_TransAmbient")
                valid = false;
            if (internalName == "_TransShadow")
                valid = false;

            // Universal Render Pipeline
            if (internalName == "_TransStrength")
                valid = false;
            if (internalName == "_TransNormal")
                valid = false;

            if (material.GetTag("RenderPipeline", false) == "HDRenderPipeline" || shaderName.Contains("Standard Lit") || shaderName.Contains("Simple Lit") || shaderName.Contains("Vertex Lit"))
            {
                if (internalName == "_SubsurfaceNormalValue")
                    valid = false;
                if (internalName == "_SubsurfaceDirectValue")
                    valid = false;
                if (internalName == "_SubsurfaceAmbientValue")
                    valid = false;
                if (internalName == "_SubsurfaceShadowValue")
                    valid = false;
            }
        }
        else
        {
            if (internalName == "_CategorySubsurface")
                valid = false;
            if (internalName == "_SubsurfaceDiffusion")
                valid = false;
            if (internalName == "_SpaceSubsurface")
                valid = false;
            if (internalName == "_MessageSubsurface")
                valid = false;

            if (internalName == "_SubsurfaceScatteringValue")
                valid = false;
            if (internalName == "_SubsurfaceAngleValue")
                valid = false;
            if (internalName == "_SubsurfaceNormalValue")
                valid = false;
            if (internalName == "_SubsurfaceDirectValue")
                valid = false;
            if (internalName == "_SubsurfaceAmbientValue")
                valid = false;
            if (internalName == "_SubsurfaceShadowValue")
                valid = false;
        }

        if (!material.HasProperty("_emissive_intensity_value"))
        {
            if (internalName == "_CategoryEmissive")
                valid = false;
            if (internalName == "_EmissiveMode")
                valid = false;
            if (internalName == "_EmissiveFlagMode")
                valid = false;
            if (internalName == "_EmissiveIntensityMode")
                valid = false;
            if (internalName == "_EmissiveIntensityValue")
                valid = false;
        }

        if (!material.HasProperty("_PerspectivePushValue"))
        {
            if (internalName == "_CategoryPerspective")
                valid = false;
            if (internalName == "_EndPerspective")
                valid = false;
        }

        if (!material.HasProperty("_SizeFadeStartValue"))
        {
            if (internalName == "_CategorySizeFade")
                valid = false;
            if (internalName == "_EndSizeFade")
                valid = false;
        }

        bool hasMotion = false;

        if (material.HasProperty("_MotionHighlightColor") || material.HasProperty("_MotionAmplitude_10") || material.HasProperty("_MotionAmplitude_20") || material.HasProperty("_MotionAmplitude_30"))
        {
            hasMotion = true;
        }

        if (!hasMotion)
        {
            if (internalName == "_CategoryMotion")
                valid = false;
        }

        bool hasMotionGlobals = false;

        if (material.HasProperty("_MotionHighlightColor") || material.HasProperty("_MotionFacingValue"))
        {
            hasMotionGlobals = true;
        }

        if (!hasMotionGlobals)
        {
            if (internalName == "_SpaceMotionGlobals")
                valid = false;
        }

        bool hasMotionLocals = false;

        if (material.HasProperty("_MotionValue_20") || material.HasProperty("_MotionValue_30") || material.HasProperty("_MotionNormal_32") || material.HasProperty("_MainMaskMotionValue"))
        {
            hasMotionLocals = true;
        }

        if (!hasMotionLocals)
        {
            if (internalName == "_SpaceMotionLocals")
                valid = false;
        }

        //if (material.HasProperty("_VertexDataMode"))
        //{
        //    if (material.GetInt("_VertexDataMode") == 1)
        //    {
        //        if (internalName == "_ColorsPositionMode")
        //            valid = false;
        //        if (internalName == "_ExtrasPositionMode")
        //            valid = false;
        //        if (internalName == "_SpaceGlobalPosition")
        //            valid = false;
        //        if (internalName == "_NoisePositionMode")
        //            valid = false;
        //        if (internalName == "_GlobalSize")
        //            valid = false;
        //        if (internalName == "_CategorySizeFade")
        //            valid = false;
        //        if (internalName == "_SizeFadeStartValue")
        //            valid = false;
        //        if (internalName == "_SizeFadeEndValue")
        //            valid = false;
        //        if (internalName == "_SpaceMotionGlobals")
        //            valid = false;
        //        if (internalName == "_MotionFacingValue")
        //            valid = false;
        //        if (internalName == "_MotionPosition_10")
        //            valid = false;
        //        if (internalName == "_MotionAmplitude_22")
        //            valid = false;
        //    }
        //}

        if (material.HasProperty("_VertexVariationMode"))
        {
            var value = material.GetInt("_VertexVariationMode");

            if (value == 0 || !showGlobalsCat)
            {
                if (internalName == "_MessageMotionVariation")
                    valid = false;
            }

            if (value == 0 || !hasMotion)
            {
                if (internalName == "_MessageMotionVariation")
                    valid = false;
            }
        }

        return valid;
    }

    string GetPropertyDisplay(Material material, MaterialProperty property)
    {
        var displayName = property.displayName;
        var internalName = property.name;
        var shaderName = material.shader.name;

        if (internalName == "_AI_Parallax")
        {
            GUILayout.Space(10);
        }

        if (internalName == "_SecondAlbedoTex")
        {
            GUILayout.Space(10);
        }

        if (internalName == "_ThirdAlbedoTex")
        {
            GUILayout.Space(10);
        }

        if (internalName == "_EmissiveTex")
        {
            GUILayout.Space(10);
        }

        if (internalName == "_AI_Clip")
        {
            displayName = "Impostor Alpha Treshold";
        }

        if (internalName == "_MainColor")
        {
            if (material.HasProperty("_MainColorMode"))
            {
                if (material.GetInt("_MainColorMode") == 1)
                {
                    displayName = displayName + "A";
                }
            }
        }

        if (internalName == "_SecondColor")
        {
            if (material.HasProperty("_SecondColorMode"))
            {
                if (material.GetInt("_SecondColorMode") == 1)
                {
                    displayName = displayName + "A";
                }
            }
        }

        if (EditorGUIUtility.currentViewWidth > 550)
        {
            if (internalName == "_MainMetallicValue")
            {
                if (shaderName.Contains("Standard Lit") || shaderName.Contains("Subsurface Lit"))
                {
                    displayName = displayName + " (Mask Red)";
                }
            }

            if (internalName == "_MainOcclusionValue")
            {
                displayName = displayName + " (Mask Green)";
            }

            if (internalName == "_MainSmoothnessValue")
            {
                if (shaderName.Contains("Standard Lit") || shaderName.Contains("Subsurface Lit"))
                {
                    displayName = displayName + " (Mask Alpha)";
                }
            }

            if (internalName == "_MainMaskRemap")
            {
                displayName = displayName + " (Mask Blue)";
            }

            if (internalName == "_SecondMetallicValue")
            {
                if (shaderName.Contains("Standard Lit") || shaderName.Contains("Subsurface Lit"))
                {
                    displayName = displayName + " (Mask Red)";
                }
            }

            if (internalName == "_SecondOcclusionValue")
            {
                displayName = displayName + " (Mask Green)";
            }

            if (internalName == "_SecondSmoothnessValue")
            {
                if (shaderName.Contains("Standard Lit") || shaderName.Contains("Subsurface Lit"))
                {
                    displayName = displayName + " (Mask Alpha)";
                }
            }

            if (internalName == "_SecondMaskRemap")
            {
                displayName = displayName + " (Mask Blue)";
            }

            if (internalName == "_DetailMeshMode" || internalName == "_DetailMeshRemap")
            {
                if (material.HasProperty("_DetailMeshMode"))
                {
                    if (material.GetInt("_DetailMeshMode") == 0)
                    {
                        displayName = displayName + " (Vertex Blue)";
                    }
                    else if (material.GetInt("_DetailMeshMode") == 1)
                    {
                        displayName = displayName + " (World Normals)";
                    }
                }
            }

            if (internalName == "_DetailMaskMode" || internalName == "_DetailMaskRemap")
            {
                displayName = displayName + " (Mask Blue)";
            }

            if (internalName == "_VertexOcclusionRemap")
            {
                displayName = displayName + " (Vertex Green)";
            }

            if (internalName == "_GradientMaskRemap")
            {
                displayName = displayName + " (Vertex Alpha)";
            }
        }

        return displayName;
    }

    void DrawPivotsSupport(Material material)
    {
        if (material.HasProperty("_VertexPivotMode"))
        {
            if (material.shader.name.Contains("Impostors"))
            {
                EditorGUILayout.HelpBox("Procedural Pivots are not supported for impostor shaders!", MessageType.Error);
            }
            else
            {
                EditorGUILayout.HelpBox("Use the Procedural Pivots option to support large patches of grass or small cover plants when Motion Bending is used!", MessageType.Info);
            }

            GUILayout.Space(10);

            var pivot = material.GetInt("_VertexPivotMode");

            bool toggle = false;

            if (pivot > 0.5f)
            {
                toggle = true;
            }

            toggle = EditorGUILayout.Toggle("Enable Procedural Pivots ", toggle);

            if (toggle)
            {
                material.SetInt("_VertexPivotMode", 1);
            }
            else
            {
                material.SetInt("_VertexPivotMode", 0);
            }
        }
    }

    void DrawRenderQueue(Material material, MaterialEditor materialEditor)
    {
        if (material.HasProperty("_RenderQueue") && material.HasProperty("_RenderPriority"))
        {
            var queue = material.GetInt("_RenderQueue");
            var priority = material.GetInt("_RenderPriority");

            queue = EditorGUILayout.Popup("Render Queue Mode", queue, new string[] { "Auto", "Priority", "User Defined" });

            if (queue == 0)
            {
                priority = 0;
            }
            else if (queue == 1)
            {
                priority = EditorGUILayout.IntSlider("Render Priority", priority, -100, 100);
            }
            else
            {
                priority = 0;
                materialEditor.RenderQueueField();
            }

            material.SetInt("_RenderQueue", queue);
            material.SetInt("_RenderPriority", priority);
        }
    }

    void DrawCopySettingsFromGameObject(Material material)
    {
        Object inputObject = null;
        inputObject = (Object)EditorGUILayout.ObjectField("Copy Settings From Object", inputObject, typeof(Object), true);

        if (inputObject != null)
        {
            if (inputObject.GetType() == typeof(GameObject))
            {
                var gameObject = (GameObject)inputObject;

                var oldMaterials = gameObject.GetComponent<MeshRenderer>().sharedMaterials;

                if (oldMaterials != null)
                {
                    for (int i = 0; i < oldMaterials.Length; i++)
                    {
                        var oldMaterial = oldMaterials[i];

                        if (oldMaterial != null)
                        {
                            CopyMaterialProperties(oldMaterial, material);

                            if (oldMaterial.HasProperty("_IsPlantShader"))
                            {
                                var newShaderName = material.shader.name;
                                newShaderName = newShaderName.Replace("Vertex", "XXX");
                                newShaderName = newShaderName.Replace("Simple", "XXX");
                                newShaderName = newShaderName.Replace("Standard", "XXX");
                                newShaderName = newShaderName.Replace("Subsurface", "XXX");

                                if (oldMaterial.shader.name.Contains("Vertex"))
                                {
                                    newShaderName = newShaderName.Replace("XXX", "Vertex");
                                }

                                if (oldMaterial.shader.name.Contains("Simple"))
                                {
                                    newShaderName = newShaderName.Replace("XXX", "Simple");
                                }

                                if (oldMaterial.shader.name.Contains("Standard"))
                                {
                                    newShaderName = newShaderName.Replace("XXX", "Standard");
                                }

                                if (oldMaterial.shader.name.Contains("Subsurface"))
                                {
                                    newShaderName = newShaderName.Replace("XXX", "Subsurface");
                                }

                                if (Shader.Find(newShaderName) != null)
                                {
                                    material.shader = Shader.Find(newShaderName);
                                }

                                if (!oldMaterial.HasProperty("_SubsurfaceValue"))
                                {
                                    material.SetFloat("_SubsurfaceValue", 0);
                                }
                            }

                            material.SetFloat("_IsInitialized", 1);

                        }
                    }
                }
            }

            if (inputObject.GetType() == typeof(Material))
            {
                var oldMaterial = (Material)inputObject;

                if (oldMaterial != null)
                {
                    CopyMaterialProperties(oldMaterial, material);

                    if (oldMaterial.HasProperty("_IsPlantShader"))
                    {
                        var newShaderName = material.shader.name;
                        newShaderName = newShaderName.Replace("Vertex", "XXX");
                        newShaderName = newShaderName.Replace("Simple", "XXX");
                        newShaderName = newShaderName.Replace("Standard", "XXX");
                        newShaderName = newShaderName.Replace("Subsurface", "XXX");

                        if (oldMaterial.shader.name.Contains("Vertex"))
                        {
                            newShaderName = newShaderName.Replace("XXX", "Vertex");
                        }

                        if (oldMaterial.shader.name.Contains("Simple"))
                        {
                            newShaderName = newShaderName.Replace("XXX", "Simple");
                        }

                        if (oldMaterial.shader.name.Contains("Standard"))
                        {
                            newShaderName = newShaderName.Replace("XXX", "Standard");
                        }

                        if (oldMaterial.shader.name.Contains("Subsurface"))
                        {
                            newShaderName = newShaderName.Replace("XXX", "Subsurface");
                        }

                        if (Shader.Find(newShaderName) != null)
                        {
                            material.shader = Shader.Find(newShaderName);
                        }

                        if (!oldMaterial.HasProperty("_SubsurfaceValue"))
                        {
                            material.SetFloat("_SubsurfaceValue", 0);
                        }
                    }

                    material.SetFloat("_IsInitialized", 1);
                }
            }

            if (inputObject.GetType() == typeof(TerrainLayer))
            {
                var layer = (TerrainLayer)inputObject;

                if (layer != null)
                {
                    if (layer.diffuseTexture != null)
                    {
                        material.SetTexture("_ThirdAlbedoTex", layer.diffuseTexture);
                    }

                    if (layer.normalMapTexture != null)
                    {
                        material.SetTexture("_ThirdNormalTex", layer.normalMapTexture);
                    }

                    if (layer.maskMapTexture != null)
                    {
                        material.SetTexture("_ThirdMaskTex", layer.maskMapTexture);
                    }

                    material.SetVector("_ThirdSpecularColor", layer.specular);
                    material.SetFloat("_ThirdNormalValue", layer.normalScale);
                    material.SetFloat("_ThirdMetallicValue", layer.metallic);
                    material.SetFloat("_ThirdSmoothnessValue", layer.smoothness);

                    material.SetVector("_ThirdUVs", new Vector4(layer.tileSize.x, layer.tileSize.y, layer.tileOffset.x, layer.tileOffset.y));
                }
            }

            inputObject = null;
        }
    }

    void CopyMaterialProperties(Material oldMaterial, Material newMaterial)
    {
        var oldShader = oldMaterial.shader;
        var newShader = newMaterial.shader;

        for (int i = 0; i < ShaderUtil.GetPropertyCount(oldShader); i++)
        {
            for (int j = 0; j < ShaderUtil.GetPropertyCount(newShader); j++)
            {
                var propertyName = ShaderUtil.GetPropertyName(oldShader, i);
                var propertyType = ShaderUtil.GetPropertyType(oldShader, i);

                if (propertyName == ShaderUtil.GetPropertyName(newShader, j))
                {
                    if (propertyType == ShaderUtil.ShaderPropertyType.Color || propertyType == ShaderUtil.ShaderPropertyType.Vector)
                    {
                        newMaterial.SetVector(propertyName, oldMaterial.GetVector(propertyName));
                    }

                    if (propertyType == ShaderUtil.ShaderPropertyType.Float || propertyType == ShaderUtil.ShaderPropertyType.Range)
                    {
                        newMaterial.SetFloat(propertyName, oldMaterial.GetFloat(propertyName));
                    }

                    if (propertyType == ShaderUtil.ShaderPropertyType.TexEnv)
                    {
                        newMaterial.SetTexture(propertyName, oldMaterial.GetTexture(propertyName));
                    }
                }
            }
        }
    }

    void DrawPoweredByTheVegetationEngine()
    {
        var styleLabelCentered = new GUIStyle(EditorStyles.label)
        {
            richText = true,
            wordWrap = true,
            alignment = TextAnchor.MiddleCenter,
        };

        Rect lastRect0 = GUILayoutUtility.GetLastRect();
        EditorGUI.DrawRect(new Rect(0, lastRect0.yMax, 1000, 1), new Color(0, 0, 0, 0.4f));

        GUILayout.Space(10);

        GUILayout.Label("<size=10><color=#808080>Powered by The Vegetation Engine. Get the full version for more features!</color></size>", styleLabelCentered);

        Rect labelRect = GUILayoutUtility.GetLastRect();

        if (GUI.Button(labelRect, "", new GUIStyle()))
        {
            Application.OpenURL("http://u3d.as/1H9u");
        }

        GUILayout.Space(5);
    }
}

