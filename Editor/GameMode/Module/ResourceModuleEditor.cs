﻿//-----------------------------------------------------------------------
// <copyright>
//     Copyright (c) 2018 Zhang Yang. All rights reserved.
// </copyright>
// <describe> #资源模块编辑器# </describe>
// <email> yeozhang@qq.com </email>
// <time> #2018年12月15日 17点24分# </time>
//-----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Wanderer.GameFramework
{
    [CustomModuleEditor("Resource Module", 0.851f, 0.227f, 0.286f)]
    public class ResourceModuleEditor : ModuleEditorBase
    {
        private BuildTargetGroup _lastBuildTargetGroup;
        private string _lastScriptingDefineSymbols;

        public ResourceModuleEditor(string name, Color mainColor, GameMode gameMode)
            : base(name, mainColor, gameMode)
        {
            //获取当前的BuildTargetGroup
            _lastBuildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
            _lastScriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(_lastBuildTargetGroup);
        }

        public override void OnDrawGUI()
        {
            GUILayout.BeginVertical("HelpBox");

            GUILayout.BeginHorizontal("HelpBox");
            GUILayout.Label("Define", GUILayout.Width(50));
            string scriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(_lastBuildTargetGroup);
            _lastScriptingDefineSymbols = GUILayout.TextArea(_lastScriptingDefineSymbols);
            if (GUILayout.Button("OK", GUILayout.Width(40)) && !_lastScriptingDefineSymbols.Equals(scriptingDefineSymbols))
            {
                _lastBuildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(_lastBuildTargetGroup, _lastScriptingDefineSymbols);
            }
            GUILayout.EndHorizontal();

            ResourceUpdateType resUpdateType = (ResourceUpdateType)EditorGUILayout.EnumPopup("Resource Update Type", _gameMode.ResUpdateType);
            if (resUpdateType != _gameMode.ResUpdateType)
            {
                _gameMode.ResUpdateType = resUpdateType;
                //保存数据
                EditorUtility.SetDirty(_gameMode);
            }
            if (_gameMode.ResUpdateType != ResourceUpdateType.Editor
            &&_gameMode.ResUpdateType != ResourceUpdateType.None)
            {
                //        _gameMode.ResUpdateType =
                //(ResourceUpdateType)EditorGUILayout.EnumPopup("Resource Update Type", _gameMode.ResUpdateType);
                if (_gameMode.ResUpdateType == ResourceUpdateType.Update)
                {
                    string officialUpdatePath = EditorGUILayout.TextField("Official Update Path", _gameMode.ResOfficialUpdatePath);
                    if (!officialUpdatePath.Equals(_gameMode.ResOfficialUpdatePath))
                    {
                        _gameMode.ResOfficialUpdatePath = officialUpdatePath;
                        EditorUtility.SetDirty(_gameMode);
                    }
                    string testUpdatePath = EditorGUILayout.TextField("Test Update Path", _gameMode.ResTestUpdatePath);
                    if (!testUpdatePath.Equals(_gameMode.ResTestUpdatePath))
                    {
                        _gameMode.ResTestUpdatePath = testUpdatePath;
                        EditorUtility.SetDirty(_gameMode);
                    }
                    _gameMode.LocalPathType =
                        (PathType)EditorGUILayout.EnumPopup("Local Path Type", PathType.ReadWrite);
                    bool value = GUILayout.Toggle(_gameMode.DefaultInStreamingAsset, "Default In StreamingAsset");
                    if (value != _gameMode.DefaultInStreamingAsset)
                    {
                        _gameMode.DefaultInStreamingAsset = value;
                        EditorUtility.SetDirty(_gameMode);
                    }
                }
                else
                {
                    PathType localPathType = 
                        (PathType)EditorGUILayout.EnumPopup("Local Path Type", _gameMode.LocalPathType);
                    if( _gameMode.LocalPathType != localPathType)
                    {
                        _gameMode.LocalPathType=localPathType;
                        EditorUtility.SetDirty(_gameMode);
                    }
                }
                string path = "";
                switch (_gameMode.LocalPathType)
                {
                    case PathType.DataPath:
                        path = Application.dataPath;
                        break;
                    case PathType.ReadOnly:
                        path = Application.streamingAssetsPath;
                        break;
                    case PathType.ReadWrite:
                        path = Application.persistentDataPath;
                        break;
                    case PathType.TemporaryCache:
                        path = Application.temporaryCachePath;
                        break;
                }

                EditorGUILayout.LabelField("Path", path);
            }

            GUILayout.EndVertical();
        }


        public override void OnClose()
        {
        }

    }
}