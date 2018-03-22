using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SRModLoader;

namespace SRML_ExampleMod
{
    public class MinimalRequirement : MonoBehaviour
    {
        public Modloader.LoadPhase currentPhase;

        void OnEnable()
        {
            Modloader.PhaseChanged += PhaseChange;
        }

        void OnDisable()
        {
            Modloader.PhaseChanged -= PhaseChange;
        }

        public void PhaseChange()
        {
            currentPhase = Modloader.currentPhase;
        }
    }
    public class ExampleMod: MonoBehaviour
    {
        public Modloader.LoadPhase currentPhase;

        void OnEnable()
        {
            Modloader.PhaseChanged += PhaseChange;
        }

        void OnDisable()
        {
            Modloader.PhaseChanged -= PhaseChange;
        }

        public void PhaseChange()
        {
            currentPhase = Modloader.currentPhase;
        }

        public void OnGUI()
        {
            GUILayout.BeginArea(new Rect(16, Screen.height - 528, 512, 512));

            GUILayout.Label("Example Mod");
            GUILayout.Label("Current Phase: " + currentPhase);
            if (currentPhase == Modloader.LoadPhase.Loading)
            {
                GUILayout.Label("You probably will not see this text as your mod is still loading.");
                GUILayout.Label("This phase would be the time to make any changes or load in any assets that you need.");
            }
            if (currentPhase == Modloader.LoadPhase.MainMenu)
            {
                GUILayout.Label("You will see this text when you are in the main menu.");
                GUILayout.Label("This phase may be usefull for things like changing mod settings.");
            }
            if (currentPhase == Modloader.LoadPhase.InGame)
            {
                GUILayout.Label("When you see this text you should be in game.");
                GUILayout.Label("Now would be the time to run all your normal code.");
            }
            GUILayout.Label("The phases are loaded by 3 different scripts.\n" +
                "The Loading phase is called when the Modloader is first opened and loaded, it then runs its own code to load your mod.\n" +
                "The MainMenu phase is called once the main menu loads and remains open.\n" +
                "The InGame phase is called when the HudUI script is loaded from the game as it should be one of the first scripts to load.");
            GUILayout.EndArea();
        }
    }
}


public class LoadMod : MonoBehaviour
{
    // Required for Mod Loader to load mod
    public void Awake()
    {
        gameObject.AddComponent<SRML_ExampleMod.ExampleMod>(); // Adds your namespace.class to a component allowing it to run.

        Destroy(this); // Removes the Loader Mod.
    }
}