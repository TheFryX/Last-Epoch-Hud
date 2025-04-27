using System.Collections.Generic;
using UnityEngine;

namespace Mod
{
    [System.Serializable]
    public class ModSettings
    {
        // Stan dropdownów
        public bool npcDrawingsDropdown;
        public bool npcClassificationsDropdown;
        public bool itemDrawingsDropdown;
        public bool gamePatchesDropdown;
        public bool riskyOptionsDropdown;
        public bool skillsDropdown;
        public bool autoPotDistanceDropdown;

        // Umiejêtnoœci
        public float skillQ = 5f;
        public float skillW = 6f;
        public float skillE = 5f;
        public float skillR = 17f;

        // Auto klikacz
        public bool autoQ;
        public bool autoW;
        public bool autoE;
        public bool autoR;

        // Nowe opcje
        public float autoPotDistance = 1.0f;
        public float volumeLevel = 50.0f;
        public bool riskyOption1;
        public bool riskyOption2;

        // Auto poty
        public bool autoHealthPot;
        public bool autoManaPot;

        // Okno
        public float windowX = 20f;
        public float windowY = 20f;
        public float windowWidth = 300f;
        public float windowHeight = 500f;

        // S³owniki
        public Dictionary<string, bool> npcDrawings = new Dictionary<string, bool>
        {
            { "Enemies", true }, { "Allies", true }, { "Neutrals", false }
        };

        public Dictionary<string, bool> npcClassifications = new Dictionary<string, bool>
        {
            { "Bosses", true }, { "Elites", true }, { "Common", false }
        };

        public Dictionary<string, bool> itemDrawings = new Dictionary<string, bool>
        {
            { "Epic", true }, { "Rare", true }, { "Common", false }
        };

        // Modyfikacje
        public bool removeFog;
        public bool cameraZoomUnlock;
        public bool minimapZoomUnlock;
        public bool mapHack;
        public bool playerLantern;
        public bool useLootFilter;
    }
}