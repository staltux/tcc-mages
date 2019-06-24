using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;


    public static class Helper
    {
        public static List<T> FindComponentsInChildrenWithTag<T>(GameObject parent, string tag) where T : Component
        {
            if (parent == null) throw new System.ArgumentNullException();
            if (string.IsNullOrEmpty(tag)) throw new System.ArgumentNullException();

            List<T> list = new List<T>(parent.GetComponentsInChildren<T>());

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if( !list[i].CompareTag(tag))
                {
                    list.RemoveAt(i);
                }
            }

            return list;
        }

        public static T FindComponentInChildrenWithTag<T>(GameObject parent, string tag) where T : Component
        {
            if (parent == null) throw new System.ArgumentNullException();
            if (string.IsNullOrEmpty(tag)) throw new System.ArgumentNullException();

            List<T> list = new List<T>(parent.GetComponentsInChildren<T>());

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].CompareTag(tag))
                {
                    return list[i];
                }
            }

            throw new System.InvalidOperationException();
        }

        public static T FindComponentInChildrenWithName<T>(GameObject parent, string name) where T : Component
        {
            if (parent == null) throw new System.ArgumentNullException();
            if (string.IsNullOrEmpty(name)) throw new System.ArgumentNullException();

            var go = FindGameObjectWithName(parent,name);
            if (go == null) throw new System.InvalidOperationException();

            var component = go.GetComponent<T>();
            if (component == null) throw new System.InvalidOperationException();

            return component;
        }

        public static GameObject FindGameObjectWithName(GameObject parent, string name)
        {

            if (parent.name.Equals(name))
            {
                return parent;
            }

            var c = parent.transform.childCount;

            for (int i = 0; i < c; i++)
            {
                var children = parent.transform.GetChild(i).gameObject;
                var result = FindGameObjectWithName(children, name);
                if (result != null) return result;
            }

            return null;
        }



#if UNITY_EDITOR

        [MenuItem("Arcane/RebuildCardList")]
        public static void RebuildCardList()
        {
            var listFileName = "Assets/Cards/CardList.asset" ;
            string[] guids = AssetDatabase.FindAssets("t:ScriptableCard", null);
            foreach (string guid in guids)
            {
                Debug.Log(AssetDatabase.GUIDToAssetPath(guid));
            }

            var asset = (ScriptableCardList) AssetDatabase.LoadMainAssetAtPath(listFileName);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<ScriptableCardList>();
                AssetDatabase.CreateAsset(asset, listFileName);
            }
                

            asset.cards = new ScriptableCard[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                asset.cards[i] = (ScriptableCard) AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[i]));
            }


            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        [MenuItem("Arcane/SaveGame")]
        public static void SaveGame()
        {
            var listFileName = Application.persistentDataPath + "/save.asset";
            Debug.Log(listFileName);

            string[] guids = AssetDatabase.FindAssets("t:ScriptableCard", null);

            var asset = new ScriptableCardList();
           
            asset.cards = new ScriptableCard[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                asset.cards[i] = (ScriptableCard)AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[i]));
            }

            BinaryFormatter formatter = new BinaryFormatter();
            var stream = new FileStream(listFileName, FileMode.Create);
            formatter.Serialize(stream,asset);
            stream.Close();
        }

        [MenuItem("Arcane/LoadGame")]
        public static void LoadGame()
        {
            var listFileName = Application.persistentDataPath + "/save.asset";

            BinaryFormatter formatter = new BinaryFormatter();
            var stream = new FileStream(listFileName, FileMode.Open);
            var asset =  (ScriptableCardList) formatter.Deserialize(stream);
            stream.Close();

            listFileName = "Assets/Cards/SaveGame.asset";
            AssetDatabase.CreateAsset(asset, listFileName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;


        }
#endif
    }
