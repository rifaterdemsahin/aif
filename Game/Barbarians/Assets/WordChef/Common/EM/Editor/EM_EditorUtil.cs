using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using SgLib.Editor;

namespace EasyMobile.Editor
{
    public static class EM_EditorUtil
    {
        /// <summary>
        /// Displays an editor dialog with the given title and message.
        /// </summary>
        /// <param name="title">the title.</param>
        /// <param name="message">the message.</param>
        public static void Alert(string title, string message)
        {
            EditorUtility.DisplayDialog(title, message, "OK");
        }

        /// <summary>
        /// Gets the main prefab.
        /// </summary>
        /// <returns>The main prefab.</returns>
        public static GameObject GetMainPrefab()
        {
            // AssetDatabase always uses forward slashes in names and paths, even on Windows.
            return AssetDatabase.LoadAssetAtPath(EM_Constants.MainPrefabPath, typeof(GameObject)) as GameObject;
        }

        /// <summary>
        /// Adds the module to prefab.
        /// </summary>
        /// <param name="prefab">Prefab.</param>
        /// <typeparam name="Module">The 1st type parameter.</typeparam>
        public static void AddModuleToPrefab<Module>(GameObject prefab) where Module : MonoBehaviour
        {
            if (prefab != null)
            {
                Module comp = prefab.GetComponent<Module>();
                if (comp == null)
                {
                    prefab.AddComponent<Module>();
                }
            }
        }

        /// <summary>
        /// Removes the module from prefab.
        /// </summary>
        /// <param name="prefab">Prefab.</param>
        /// <typeparam name="Module">The 1st type parameter.</typeparam>
        public static void RemoveModuleFromPrefab<Module>(GameObject prefab) where Module: MonoBehaviour
        {
            if (prefab != null)
            {
                Module comp = prefab.GetComponent<Module>();
                if (comp != null)
                {
                    Component.DestroyImmediate(comp, true);
                }
            }
        }

        /// <summary>
        /// Returns the first class found with the specified class name and (optional) namespace and assembly name.
        /// Returns null if no class found.
        /// </summary>
        /// <returns>The class.</returns>
        /// <param name="className">Class name.</param>
        /// <param name="nameSpace">Optional namespace of the class to find.</param>
        /// <param name="assemblyName">Optional assembly name.</param>
        public static System.Type FindClass(string className, string nameSpace = null, string assemblyName = null)
        {
            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly asm in assemblies)
            {
                // The assembly must match the given one if any.
                if (!string.IsNullOrEmpty(assemblyName) && !asm.GetName().Name.Equals(assemblyName))
                {
                    continue;
                }

                System.Type[] types = asm.GetTypes();
                foreach (System.Type t in types)
                {
                    // The namespace must match the given one if any. Note that the type may not have a namespace at all.
                    // Must be a class and of course class name must match the given one.
                    if (t.IsClass && t.Name.Equals(className) && (string.IsNullOrEmpty(nameSpace) || nameSpace.Equals(t.Namespace)))
                    {
                        return t;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Check if the given namespace exists within the current domain's assemblies.
        /// </summary>
        /// <returns><c>true</c>, if exists was namespaced, <c>false</c> otherwise.</returns>
        /// <param name="nameSpace">Name space.</param>
        /// <param name="assemblyName">Assembly name.</param>
        public static bool NamespaceExists(string nameSpace, string assemblyName = null)
        {
            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly asm in assemblies)
            {
                // The assembly must match the given one if any.
                if (!string.IsNullOrEmpty(assemblyName) && !asm.GetName().Name.Equals(assemblyName))
                {
                    continue;
                }
                    
                System.Type[] types = asm.GetTypes();
                foreach (System.Type t in types)
                {
                    // The namespace must match the given one if any. Note that the type may not have a namespace at all.
                    // Must be a class and of course class name must match the given one.
                    if (!string.IsNullOrEmpty(t.Namespace) && t.Namespace.Equals(nameSpace))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
