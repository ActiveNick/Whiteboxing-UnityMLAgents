using System;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Use this editor script for cloud training based on AKS
/// </summary>
public class AzureDeploymentWindow : EditorWindow
{
    [MenuItem("ML on Azure/Train with AKS")]
    static void OnAzureLogin()
    {
        EditorWindow.GetWindow(typeof(AzureDeploymentWindow), false, "Train ML on Azure", true);
    }

    // TODO: This is a demo/Proof of Concept and is still a work in progress. Some suggestions:
    // 1. Should remember the user's storage acct name once set, or let them select from a dropdown using Azure SDKs to populate from their subscription
    // 2. This storage account default wouldn't be globally unique at scale
    // 3. Should remember the user's Training Taml file location once set
    // 4. Should inject the project name in the runID
    // 5. Add a textbox or selection list for the Azure region
    string storageAccountName = $"unityml{DateTime.Now.ToString("yyyyMMddHHmm")}";
    string trainerYamlPath = "D:\\Dev\\Git\\Unity-ML-Agents\\config\\trainer_config.yaml";
    string jobRunID = $"run-a";

    string environmentFile;
    string cmd;

    void OnGUI()
    {
        // Dialog header
        EditorGUILayout.LabelField("Train ML on Azure", EditorStyles.boldLabel);

        // Textbox: Trainer Yaml Path & file
        trainerYamlPath = EditorGUILayout.TextField("Trainer Config File",
            trainerYamlPath,
            new GUILayoutOption[]
            {
                        GUILayout.ExpandWidth(true),
                        GUILayout.MinWidth(200)
            });
        // Textbox: Azure Storage account name
        storageAccountName = EditorGUILayout.TextField("Storage Account Name", 
            storageAccountName,
            new GUILayoutOption[]
            {
                GUILayout.ExpandWidth(true),
                GUILayout.MinWidth(200)
            });
        // Textbox: RunID
        jobRunID = EditorGUILayout.TextField("Job Name (Run ID)", 
            jobRunID,
            new GUILayoutOption[]
            {
                GUILayout.ExpandWidth(true),
                GUILayout.MinWidth(200)
            });
        // selection Dropdown: Linux build path & file
        if (EditorGUILayout.DropdownButton(new GUIContent(environmentFile ?? "Choose Build Output"), FocusType.Keyboard))
        {
            environmentFile = EditorUtility.OpenFilePanel("Select Build Output", Directory.GetCurrentDirectory(), "x86_64");
        }

        if (!string.IsNullOrEmpty(cmd))
        {
            // The intent is that we'd run the process ourselves, either by shelling out to the script or by using Azure SDKs from
            // within the editor. For now we display the command and let the developer copy/paste it to their favorite console to run.
            GUILayout.Label("Run this command from the console:");

            var originalWrap = EditorStyles.label.wordWrap;
            EditorStyles.label.wordWrap = true;

            EditorGUILayout.SelectableLabel(
                cmd,
                new GUILayoutOption[]
                {
                    GUILayout.ExpandHeight(true),
                    GUILayout.MinHeight(90)
                });

            EditorStyles.label.wordWrap = originalWrap;

        }

        GUILayout.FlexibleSpace();
        
        if (GUILayout.Button(new GUIContent("Generate Deployment Command")))
        {
            // This command line is for PowerShell only, change extension for Bash
            // TODO: Provide an option to select between PowerShell or Bash in the editor dialog
            cmd = $".\\scripts\\train-on-aks.ps1 -storageAccountName {storageAccountName} -environmentName {Path.GetFileNameWithoutExtension(environmentFile)} -localVolume {Path.GetDirectoryName(environmentFile)} -trainerConfigPath {trainerYamlPath} -runid {jobRunID}";
        }
    }
}
