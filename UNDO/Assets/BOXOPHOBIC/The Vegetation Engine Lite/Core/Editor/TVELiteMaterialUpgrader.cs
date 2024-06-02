// Cristian Pop - https://boxophobic.com/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Boxophobic.StyledGUI;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace TheVegetationEngineLite
{
    public class TVELiteMaterialUpgrader : EditorWindow
    {
        float GUI_HALF_EDITOR_WIDTH = 220;

        List<string> activeScenePaths;

        bool processAllProjectMaterials = false;

        bool requiresSceneRestart = false;
        bool validMaterialsUpgraded = false;

        Color bannerColor;
        string bannerText;
        static TVELiteMaterialUpgrader window;
        //Vector2 scrollPosition = Vector2.zero;

        [MenuItem("Window/BOXOPHOBIC/The Vegetation Engine Lite/Material Upgrader", false, 2007)]
        public static void ShowWindow()
        {
            window = GetWindow<TVELiteMaterialUpgrader>(false, "Material Upgrader Lite", true);
            window.minSize = new Vector2(600, 300);
        }

        void OnEnable()
        {
            bannerColor = new Color(0.6f, 0.6f, 0.6f);
            bannerText = "Material Upgrader Lite";
        }

        void OnGUI()
        {
            GUI_HALF_EDITOR_WIDTH = this.position.width / 2.0f - 24;

            StyledGUI.DrawWindowBanner(bannerColor, bannerText);

            GUILayout.BeginHorizontal();
            GUILayout.Space(15);

            GUILayout.BeginVertical();

            //scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(this.position.width - 28), GUILayout.Height(this.position.height - 120));

            if (EditorApplication.isCompiling)
            {
                GUI.enabled = false;
            }

            if (validMaterialsUpgraded)
            {
                EditorGUILayout.HelpBox("All valid project materials have been upgraded! You can run the upgrader any time if needed!", MessageType.Info, true);
            }
            else
            {

                EditorGUILayout.HelpBox("Material upgrading is required when switching render pipelines or when upgrading to a newer version. Run the material upgrader after the packages using the Lite shaders are installed!", MessageType.Info, true);
            }

            GUILayout.Space(15);

            if (GUILayout.Button("Upgrade Project Materials", GUILayout.Height(24)))
            {
                GetCurrentScenesSaving();
                UpgradeAllMaterials();
                RestartActiveScenes();

                validMaterialsUpgraded = true;

                GUIUtility.ExitGUI();
            }

            GUILayout.Space(15);

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Process All Project Materials", "When disabled, only the materials with the TVE Material naming convention are processed!"), GUILayout.Width(GUI_HALF_EDITOR_WIDTH - 100));
            processAllProjectMaterials = EditorGUILayout.Toggle(processAllProjectMaterials);
            GUILayout.EndHorizontal();

            GUI.enabled = true;

            //GUILayout.EndScrollView();

            GUILayout.EndVertical();

            GUILayout.Space(13);
            GUILayout.EndHorizontal();
        }

        void UpgradeAllMaterials()
        {
            var allMaterialGUIDs = AssetDatabase.FindAssets("t:material", null);

            int count = 0;

            if (processAllProjectMaterials)
            {
                foreach (var guids in allMaterialGUIDs)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guids);

                    var material = AssetDatabase.LoadAssetAtPath<Material>(path);
                    TVELiteUtils.SetMaterialSettings(material);

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    TVELiteUtils.UnloadMaterialFromMemory(material);

                    count++;

                    EditorUtility.DisplayProgressBar("The Vegetatin Engine Lite", "Processing " + Path.GetFileName(path), (float)count * (1.0f / (float)allMaterialGUIDs.Length));
                }
            }
            else
            {
                var liteMaterialPaths = new List<string>();
                var liteCollectionPaths = AssetDatabase.FindAssets("glob:\"" + "*.litecollection" + "\"");

                for (int i = 0; i < liteCollectionPaths.Length; i++)
                {
                    var collectionPath = AssetDatabase.GUIDToAssetPath(liteCollectionPaths[i]);
                    var collectionFolder = Path.GetDirectoryName(collectionPath);
                    var collectionMaterials = Directory.GetFiles(collectionFolder, "*.mat", SearchOption.AllDirectories);

                    if (collectionMaterials != null)
                    {
                        liteMaterialPaths.AddRange(collectionMaterials);
                    }
                }

                foreach (var path in liteMaterialPaths)
                {
                    var material = AssetDatabase.LoadAssetAtPath<Material>(path);
                    TVELiteUtils.SetMaterialSettings(material);

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    TVELiteUtils.UnloadMaterialFromMemory(material);

                    count++;

                    EditorUtility.DisplayProgressBar("The Vegetatin Engine Lite", "Processing " + Path.GetFileName(path), (float)count * (1.0f / (float)liteMaterialPaths.Count));
                }
            }

            EditorUtility.ClearProgressBar();
            Debug.Log("<b>[The Vegetation Engine Lite]</b> " + "All valid project materials have been updated!");
        }

        void GetCurrentScenesSaving()
        {
            activeScenePaths = new List<string>();

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                activeScenePaths.Add(scene.path);
            }

            for (int i = 0; i < activeScenePaths.Count; i++)
            {
                var activeScenePath = activeScenePaths[i];

                if (activeScenePath == "")
                {
                    var saveScene = EditorUtility.DisplayDialog("Save Untitled Scene?", "The current scene is not saved to disk! Would you like to save it?", "Save New Scene", "No");

                    if (saveScene)
                    {
                        var currentScene = SceneManager.GetSceneByPath(activeScenePath);

                        var savePath = EditorUtility.SaveFilePanelInProject("Save Scene", "New Scene", "unity", "Save scene to disk!", "Assets");

                        if (savePath != "")
                        {
                            EditorSceneManager.SaveScene(currentScene, savePath);
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();

                            activeScenePaths[i] = savePath;
                        }
                    }
                }
                else
                {
                    var currentScene = SceneManager.GetSceneByPath(activeScenePath);

                    if (currentScene.isDirty)
                    {
                        var saveScene = EditorUtility.DisplayDialog("Save Scene " + currentScene.name + "?", "The current scene is modified! Would you like to save it?", "Save Scene", "No");

                        if (saveScene)
                        {
                            EditorSceneManager.SaveScene(currentScene);
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();
                        }
                    }
                }
            }

            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            requiresSceneRestart = true;
        }

        void RestartActiveScenes()
        {
            if (requiresSceneRestart)
            {
                for (int i = 0; i < activeScenePaths.Count; i++)
                {
                    var activeScenePath = activeScenePaths[i];

                    if (activeScenePath != "")
                    {
                        var scene = SceneManager.GetSceneByPath(activeScenePath);

                        if (i == 0)
                        {
                            EditorSceneManager.OpenScene(activeScenePath);
                        }
                        else
                        {
                            EditorSceneManager.OpenScene(activeScenePath, OpenSceneMode.Additive);
                        }

                        EditorSceneManager.SaveScene(scene);
                    }

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    requiresSceneRestart = false;
                }
            }
        }
    }
}
