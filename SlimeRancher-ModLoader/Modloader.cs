using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SRModLoader
{
    public class Modloader : MonoBehaviour
    {
        public delegate void ModEventHandler();
        public static event ModEventHandler PhaseChanged;
        public static LoadPhase currentPhase;
        public enum LoadPhase { Loading, MainMenu, InGame }

        private bool duplicate = false;
        private int modCount = 0;

        public static void ChangePhase(LoadPhase newPhase)
        {
            currentPhase = newPhase;

            PhaseChanged?.Invoke();
        }

        public void Awake()
        {
            ChangePhase(LoadPhase.Loading);
            #region Mod Loading
            if (base.GetComponents<Modloader>().Length > 1)
            {
                this.duplicate = true;
                foreach (Modloader modloader in base.GetComponents<Modloader>())
                {
                    if (!modloader.duplicate)
                    {
                        UnityEngine.Object.Destroy(modloader);
                    }
                }
            }


            FileInfo[] files = new DirectoryInfo(Application.dataPath + "/Mods").GetFiles("*.dll");
            int num = files.Length;
            foreach (FileInfo fileInfo in files)
            {
                this.LoadMod(fileInfo, base.gameObject);
            }
            Debug.Log("Loaded " + num.ToString() + " mods");
            #endregion

        }

        public void Start()
        {
            ChangePhase(LoadPhase.MainMenu);
        }

        public void LoadMod(FileInfo file, GameObject root)
        {
            Assembly assembly = Assembly.LoadFrom(file.FullName);
            Type type = assembly.GetType("LoadMod");
            Debug.Log("Removing " + base.GetComponents(type).Length.ToString() + " duplicate(s) of " + file.Name);
            foreach (Component component in base.GetComponents(type))
            {
            	UnityEngine.Object.Destroy(component);
            }
            root.AddComponent(type);
            modCount++;
        }

        public void OnGUI()
        {
            if (currentPhase == LoadPhase.MainMenu)
            {
                GUILayout.Label(modCount + " Mods Loaded");
            }
        }
    }
}
