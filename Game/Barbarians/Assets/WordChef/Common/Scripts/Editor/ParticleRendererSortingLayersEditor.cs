using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using System;
#endif
#pragma warning disable 0618

//Expose SortingLayer  SortingOrder on MeshRenderer
//With nice drop down and revert to prefab functionality.

//Base exposing code by neror http://forum.unity3d.com/threads/212006-Drawing-order-of-Meshes-and-Sprites
//Get all sorting layer name and ID by guavaman  Ivan.Murashko http://answers.unity3d.com/questions/585108/how-do-you-access-sorting-layers-via-scripting.html
//Sorting Layer drop down menu, bold text on prefab override, revert to prefab and instant update on Order change functionality by 5argon

[CustomEditor(typeof(ParticleRenderer))]
[CanEditMultipleObjects]
public class ParticleRendererSortingLayersEditor : SortingLayersEditor
{

}