#region File Description
//-----------------------------------------------------------------------------
// GameSettings.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using directives
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using RacingGame.Helpers;
using System.Threading;
#if NETFX_CORE
using Serializable = System.Runtime.Serialization.DataContractAttribute;
#endif
#endregion

namespace RacingGame.Properties
{
    /// <summary>
    /// Game settings, stored in a custom xml file. The reason for this is
    /// we want to be able to store our game data on the Xbox360 too.
    /// On the PC we could just use a Settings/config file and have all the
    /// code autogenerated for us, but this way it works both on PC and Xbox.
    /// Note: The default instance for the game settings is in this class,
    /// this way we get the same behaviour as for normal Settings files!
    /// </summary>
    [Serializable]
    public class GameSettings
    {
        #region Default
        /// <summary>
        /// Filename for our game settings file.
        /// </summary>
        const string SettingsFilename = "RacingGameSettings.xml";

        /// <summary>
        /// Default instance for our game settings.
        /// </summary>
        private static GameSettings defaultInstance = new GameSettings();

        /// <summary>
        /// Need saving the game settings file? Only set to true if
        /// we really changed some game setting here.
        /// </summary>
        private static bool needSave = false;

        /// <summary>
        /// Default
        /// </summary>
        /// <returns>Game settings</returns>
        public static GameSettings Default
        {
            get
            {
                return defaultInstance;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Create game settings, don't allow public constructor!
        /// </summary>
        private GameSettings()
        {
        }

        /// <summary>
        /// Create game settings. This constructor helps us to only load the
        /// GameSettings once, not again if GameSettings is recreated by
        /// the Deserialization process.
        /// </summary>
        /// <param name="loadSettings">Load settings</param>
        public static void Initialize()
        {
            Load();
        }
        #endregion

        #region Load
        /// <summary>
        /// Load
        /// </summary>
        public static void Load()
        {
            bool saveImmediately = false;
            needSave = false;

            FileHelper.StorageContainerMRE.WaitOne();
            FileHelper.StorageContainerMRE.Reset();

            try
            {
                //TODO: Use Nick Gravlyn's easy storage?
				/*
                StorageDevice storageDevice = FileHelper.XnaUserDevice;
                if ((storageDevice != null) && storageDevice.IsConnected)
                {
                    IAsyncResult async = storageDevice.BeginOpenContainer("RacingGame", null, null);

                    async.AsyncWaitHandle.WaitOne();

                    using (StorageContainer container =
                        storageDevice.EndOpenContainer(async))
                    {
                        async.AsyncWaitHandle.Close();
                        if (container.FileExists(SettingsFilename))
                        {
                            using (Stream file = container.OpenFile(SettingsFilename,
                                FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                if (file.Length > 0)
                                {
                                    GameSettings loadedGameSettings =
                                        (GameSettings)new XmlSerializer(
                                        typeof(GameSettings)).Deserialize(file);
                                    if (loadedGameSettings != null)
                                        defaultInstance = loadedGameSettings;
                                }
                                else
                                {
                                    // If the file is empty, just create a new file with the 
                                    // default settings.
                                    needSave = true;
                                    saveImmediately = true;
                                }
                            }
                        }
                        else
                        {
                            // Create new file after exiting
                            needSave = true;
                        }
                    }
                }*/
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine("Settings Load Failure: " + exc.ToString());
            }

            FileHelper.StorageContainerMRE.Set();

            if (saveImmediately)
            {
                Save();
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Save
        /// </summary>
        public static void Save()
        {
            // No need to save if everything is up to date.
            if (needSave == false)
                return;

            needSave = false;

            FileHelper.StorageContainerMRE.WaitOne();
            FileHelper.StorageContainerMRE.Reset();

            // Open a storage container
            try
            {
				/*
                StorageDevice storageDevice = FileHelper.XnaUserDevice;
                if ((storageDevice != null) && storageDevice.IsConnected)
                {
                    IAsyncResult async = storageDevice.BeginOpenContainer("RacingGame", null, null);

                    async.AsyncWaitHandle.WaitOne();

                    using (StorageContainer container = storageDevice.EndOpenContainer(async))
                    {
                        async.AsyncWaitHandle.Close();
                        using (Stream file = container.CreateFile(SettingsFilename))
                        {
                            // Save everything in this class with help of the XmlSerializer.
                            new XmlSerializer(typeof(GameSettings)).
                                Serialize(file, defaultInstance);
                        }
                    }
                }*/
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine("Settings Load Failure: " + exc.ToString());
            }

            FileHelper.StorageContainerMRE.Set();
        }

        /// <summary>
        /// Sets all of the graphical settings to their minimum possible
        /// values and saves the changes.
        /// </summary>
        public static void SetMinimumGraphics()
        {
            GameSettings.Default.ResolutionWidth = GameSettings.MinimumResolutionWidth;
            GameSettings.Default.ResolutionHeight = GameSettings.MinimumResolutionHeight;
            GameSettings.Default.ShadowMapping = false;
            GameSettings.Default.HighDetail = false;
            GameSettings.Default.PostScreenEffects = false;
            GameSettings.Save();
        }
        #endregion

        #region Setting variables with properties
        /// <summary>
        /// Highscores
        /// </summary>
        string highscores = "";
        /// <summary>
        /// Highscores
        /// </summary>
        /// <returns>String</returns>
        public string Highscores
        {
            get
            {
                return highscores;
            }
            set
            {
                if (highscores != value)
                    needSave = true;
                highscores = value;
            }
        }

        /// <summary>
        /// Player name
        /// </summary>
        string playerName = "Player";
        /// <summary>
        /// Player name
        /// </summary>
        /// <returns>String</returns>
        public string PlayerName
        {
            get
            {
                return playerName;
            }
            set
            {
                if (playerName != value)
                    needSave = true;
                playerName = value;
            }
        }

        public const int MinimumResolutionWidth = 640;

        /// <summary>
        /// Resolution width
        /// </summary>
        int resolutionWidth = 0;
        /// <summary>
        /// Resolution width
        /// </summary>
        /// <returns>Int</returns>
        public int ResolutionWidth
        {
            get
            {
                return resolutionWidth;
            }
            set
            {
                if (resolutionWidth != value)
                    needSave = true;
                resolutionWidth = value;
            }
        }

        public const int MinimumResolutionHeight = 480;

        /// <summary>
        /// Resolution height
        /// </summary>
        int resolutionHeight = 0;
        /// <summary>
        /// Resolution height
        /// </summary>
        /// <returns>Int</returns>
        public int ResolutionHeight
        {
            get
            {
                return resolutionHeight;
            }
            set
            {
                if (resolutionHeight != value)
                    needSave = true;
                resolutionHeight = value;
            }
        }

        /// <summary>
        /// Fullscreen
        /// </summary>
        bool fullscreen = true;
        /// <summary>
        /// Fullscreen
        /// </summary>
        /// <returns>Bool</returns>
        public bool Fullscreen
        {
            get
            {
                return fullscreen;
            }
            set
            {
                if (fullscreen != value)
                    needSave = true;
                fullscreen = value;
            }
        }

        bool postScreenEffects = true;
        /// <summary>
        /// Post screen effects
        /// </summary>
        /// <returns>Bool</returns>
        public bool PostScreenEffects
        {
            get
            {
                return postScreenEffects;
            }
            set
            {
                if (postScreenEffects != value)
                    needSave = true;
                postScreenEffects = value;
            }
        }

        bool shadowMapping = true;
        /// <summary>
        /// ShadowMapping
        /// </summary>
        /// <returns>Bool</returns>
        public bool ShadowMapping
        {
            get
            {
                return shadowMapping;
            }
            set
            {
                if (shadowMapping != value)
                    needSave = true;
                shadowMapping = value;
            }
        }

        bool highDetail = true;
        /// <summary>
        /// HighDetail
        /// </summary>
        /// <returns>Bool</returns>
        public bool HighDetail
        {
            get
            {
                return highDetail;
            }
            set
            {
                if (highDetail != value)
                    needSave = true;
                highDetail = value;
            }
        }

        /// <summary>
        /// Sound volume
        /// </summary>
        float soundVolume = 0.8f;
        /// <summary>
        /// Sound volume
        /// </summary>
        /// <returns>Float</returns>
        public float SoundVolume
        {
            get
            {
                return soundVolume;
            }
            set
            {
                if (soundVolume != value)
                    needSave = true;
                soundVolume = value;
            }
        }

        /// <summary>
        /// Music volume
        /// </summary>
        float musicVolume = 0.6f;
        /// <summary>
        /// Music volume
        /// </summary>
        /// <returns>Float</returns>
        public float MusicVolume
        {
            get
            {
                return musicVolume;
            }
            set
            {
                if (musicVolume != value)
                    needSave = true;
                musicVolume = value;
            }
        }

        /// <summary>
        /// Controller sensitivity
        /// </summary>
        float controllerSensitivity = 0.5f;
        /// <summary>
        /// Controller sensitivity
        /// </summary>
        /// <returns>Float</returns>
        public float ControllerSensitivity
        {
            get
            {
                return controllerSensitivity;
            }
            set
            {
                if (controllerSensitivity != value)
                    needSave = true;
                controllerSensitivity = value;
            }
        }
        #endregion
    }
}
