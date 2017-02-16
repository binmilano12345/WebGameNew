using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using AppConfig;
using System;
using System.IO;
//using Utilities;

namespace AppUsMobile.Modules.Sound {
    public class SoundAssetBundleHelper : MonoBehaviour {

        public static SoundAssetBundleHelper Instance;

        public List<AudioClip> clips = new List<AudioClip>();
        public List<Sound.SoundId> ids = new List<Sound.SoundId>();
        public Dictionary<Sound.SoundId, AudioClip> MapSound = new Dictionary<Sound.SoundId, AudioClip>();

        void Awake() {
            //if (Instance != null) {
            //	Destroy (gameObject);
            //	return;
            //}
            if (Instance != null)
                Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void OnDestroy() {
            Instance = null;
        }

        void Start() {
            for (int i = 0; i < clips.Count && i < ids.Count; i++) {
                MapSound.Add(ids[i], clips[i]);
            }
            Sound.InitSound(this);
        }

        #region Sound
        public class StringValueAttribute : Attribute {
            public string Value { get; set; }
        }
        public class Sound {
            public enum SoundId {
                [StringValue(Value = "Sounds/Background_Music")]
                Background_Music,
                [StringValue(Value = "Sounds/Button_Click")]
                Button_Click,

                //Emotion Chat
                [StringValue(Value = "Sounds/Emoticon/1181315")]
                Emotion_1,
                [StringValue(Value = "Sounds/Emoticon/1181321")]
                Emotion_2,
                [StringValue(Value = "Sounds/Emoticon/1181322")]
                Emotion_3,
                [StringValue(Value = "Sounds/Emoticon/1181329")]
                Emotion_4,
                [StringValue(Value = "Sounds/Emoticon/1181330")]
                Emotion_5,
                [StringValue(Value = "Sounds/Emoticon/1181332")]
                Emotion_6,
                [StringValue(Value = "Sounds/Emoticon/1181336")]
                Emotion_7,
                [StringValue(Value = "Sounds/Emoticon/1181338")]
                Emotion_8,
                [StringValue(Value = "Sounds/Emoticon/1182338")]
                Emotion_9,
                [StringValue(Value = "Sounds/Emoticon/1181356")]
                Emotion_10,
                [StringValue(Value = "Sounds/Emoticon/1181357")]
                Emotion_11,
                [StringValue(Value = "Sounds/Emoticon/1181362")]
                Emotion_12,
                [StringValue(Value = "Sounds/Emoticon/1181363")]
                Emotion_13,
                [StringValue(Value = "Sounds/Emoticon/1181369")]
                Emotion_14,
                [StringValue(Value = "Sounds/Emoticon/1181370")]
                Emotion_15,
                [StringValue(Value = "Sounds/Emoticon/1181376")]
                Emotion_16,
                [StringValue(Value = "Sounds/Emoticon/1181382")]
                Emotion_17,
                [StringValue(Value = "Sounds/Emoticon/1181384")]
                Emotion_18,

                // SendGift
                [StringValue(Value = "Sounds/SendGift/ani_Bomb")]
                Bomb,
                [StringValue(Value = "Sounds/SendGift/ani_Egg")]
                Egg,
                [StringValue(Value = "Sounds/SendGift/ani_Flower")]
                Flower,
                [StringValue(Value = "Sounds/SendGift/ani_PourWater")]
                PourWater,
                [StringValue(Value = "Sounds/SendGift/ani_BrickThrow")]
                Brick,
                [StringValue(Value = "Sounds/SendGift/ani_Like")]
                Like,
                [StringValue(Value = "Sounds/SendGift/ani_Ice_cream")]
                Cream,

                //Ingame
                [StringValue(Value = "Sounds/InGame/Countdown_Tiktok")]
                Countdown_Tiktok,
                [StringValue(Value = "Sounds/InGame/Eatting_Piece")]
                Eatting_Piece,
                [StringValue(Value = "Sounds/InGame/King_Being_Attacked")]
                King_Being_Attacked,
                [StringValue(Value = "Sounds/InGame/Level_Up")]
                Level_Up,
                [StringValue(Value = "Sounds/InGame/Moving_Piece")]
                Moving_Piece,
                [StringValue(Value = "Sounds/InGame/Player_Get_In_Table")]
                Player_Get_In_Table,
                [StringValue(Value = "Sounds/InGame/Ready")]
                Ready,
                [StringValue(Value = "Sounds/InGame/Win_Game")]
                Win_Game,


            }

            //static AudioSource[] Sounds = new AudioSource[100];
            //static List<AudioSource> Sounds = new List<AudioSource>(100);
            static Dictionary<SoundId, AudioSource> Sounds = new Dictionary<SoundId, AudioSource>();
            static SoundId[] AutoPlaySounds = new SoundId[] { };
            static SoundId[] AutoPlayLoopSounds = new SoundId[] { SoundId.Background_Music };

            /// <summary>
            /// co danh dau am thanh duoc mo hay ko, 0: disable, 1: enable
            /// </summary>
            private static int enable = 1;

            private static int enable_bgsound = 1;

            public static bool is_init_done = false;


            static SoundAssetBundleHelper soundHelper = new SoundAssetBundleHelper();

            internal static void InitSound(SoundAssetBundleHelper SoundHelper) {
                Debug.Log("Init Sound");
                soundHelper = SoundHelper;
                //Debug.LogError("InitSound  length= " + soundHelper.MapSound.Count);
                enable = PlayerPrefs.GetInt("sound_enable", 1);
                enable_bgsound = PlayerPrefs.GetInt("bgsound_enable", 1);

                //gameObject.AddComponent<AudioListener>(); Tam thoi comment 25/8/2016 by ronaldo

                //load file trong thu muc resource

                Dictionary<string, SoundId> Maps = new Dictionary<string, SoundId>();
                foreach (var field in typeof(SoundId).GetFields()) {
                    //Debug.LogError("------------------------------ InitSOund------------------------");
                    var attribute = (StringValueAttribute[])field.GetCustomAttributes(typeof(StringValueAttribute), false);

                    if (attribute.Length == 0)
                        continue;
                    Maps[attribute[0].Value] = (SoundId)field.GetValue(null);
                    //Debug.LogError("------------------------------ InitSOund------------------------" + Maps[attribute[0].Value]);
                }


#if UNITY_EDITOR
                string myPath = "Assets/Resources/Sounds";
                //                GetSoundFilesFromDirectory(gameObject, Maps, myPath);
                LoadAllSoundFromConfig(soundHelper.gameObject, Maps);
#else
				LoadAllSoundFromConfig(soundHelper.gameObject, Maps);
#endif

                is_init_done = true;
            }

            private static void GetSoundFilesFromDirectory(GameObject gameObject, Dictionary<string, SoundId> Maps, string dirpath) {
                DirectoryInfo dir = new DirectoryInfo(dirpath);
                FileInfo[] info = dir.GetFiles("*.*");
                DirectoryInfo[] dinfo = dir.GetDirectories();
                foreach (FileInfo f in info) {
                    if (f.Extension == ".ogg" || f.Extension == ".mp3" || f.Extension == ".wav") {
                        string tempName = (dirpath + "/" + f.Name).Replace("Assets/Resources/", "");
                        string path = tempName.Substring(0, tempName.IndexOf('.'));
                        if (!Maps.ContainsKey(path)) {
                            //LogMng.LogError("Sound", "Config has not key: " + path + ", " + Maps.ContainsKey(path));
                            continue;
                        }
                        //Logger.Log("Sound", "key: " + path + ", " + Maps.ContainsKey(path));
                        //                        LoadSound(gameObject, Maps[path], path);
                        Debug.LogError("Try To Get Sound");
                        LoadSoundFromBundle(gameObject, Maps[path], path);
                    } else if (f.Extension != ".meta") {
                        Debug.LogError("Unknown Extension: " + (dirpath + "/" + f.Name));
                    }
                }

                foreach (DirectoryInfo d in dinfo) {

                    string tempName = d.Name;
                    GetSoundFilesFromDirectory(gameObject, Maps, dirpath + "/" + tempName);
                }
            }

            private static void LoadAllSoundFromConfig(GameObject gameObject, Dictionary<string, SoundId> Maps) {
                //Debug.LogError("Maps.Keys.Count= " + Maps.Keys.Count);
                //Debug.LogError("LoadAllSoundFromConfig soundHelper.MapSound length= " + soundHelper.MapSound.Count);
                Dictionary<string, SoundId>.KeyCollection.Enumerator enu = Maps.Keys.GetEnumerator();
                while (enu.MoveNext()) {
                    string path = enu.Current;
                    //                    LoadSound(gameObject, Maps[path], path);
                    //Debug.LogError("path: " + path);
                    LoadSoundFromBundle(gameObject, Maps[path], path);
                }
                PlayLoopSound(SoundId.Background_Music);
            }


            private static void LoadSound(GameObject gameObject, SoundId id, string path) {
                //Sounds[(int)id] = gameObject.AddComponent<AudioSource>();
                //Sounds[(int)id].clip = Resources.Load(path, typeof(AudioClip)) as AudioClip;

                AudioSource asrc = gameObject.AddComponent<AudioSource>();
                asrc.clip = Resources.Load(path, typeof(AudioClip)) as AudioClip;

                Sounds[id] = asrc;
            }

            private static void LoadSoundFromBundle(GameObject gameObject, SoundId id, string path) {
                //Debug.LogError("LoadSoundFromBundle id= " + id.ToString());
                //Sounds[(int)id] = gameObject.AddComponent<AudioSource>();
                //Sounds[(int)id].clip = Resources.Load(path, typeof(AudioClip)) as AudioClip;

                //				asrc.clip = GlobalVariable.AssetBundleSound.clips[GlobalVariable.AssetBundleSound.ids.IndexOf(id)];


                if (soundHelper != null) {
                    if (soundHelper.MapSound.ContainsKey(id)) {
                        AudioSource asrc = gameObject.AddComponent<AudioSource>();
                        asrc.clip = soundHelper.MapSound[id];
                        Sounds[id] = asrc;
                        //Debug.LogError("BUNDLE : " + asrc.clip.name);
                    } else {
                        Debug.LogError("NOT BUNDLE : " + id);
                    }
                } else {
                    Debug.LogError("SoundAssetBundleHelper.Instance nulllll");
                }



            }

            public static bool ENABLE {
                get {
                    return enable == 1;
                }
                set {
                    int temp = value ? 1 : 0;
                    //if (temp == enable)
                    //    return;

                    enable = temp;
                    //if (!value)
                    //    StopSound ();
                    PlayerPrefs.SetInt("sound_enable", enable);
                    PlayerPrefs.Save();
                }
            }

            public static bool ENABLE_BGSOUND {
                get {
                    return enable_bgsound == 1;
                }
                set {
                    int temp = value ? 1 : 0;
                    //if (temp == enable_bgsound)
                    //    return;

                    enable_bgsound = temp;
                    // enable_bgsound = 0; // temp lock
                    if (!value)
                        StopBackgroundSound();
                    else {
                        for (int i = 0; i < AutoPlayLoopSounds.Length; i++) {
                            continue;
                            PlayLoopSound(AutoPlayLoopSounds[i]);
                        }
                    }
                    PlayerPrefs.SetInt("bgsound_enable", enable_bgsound);
                    PlayerPrefs.Save();
                }
            }

            public static void ChangeBackgroundSound(SoundId bgsoundid) {
                return;
                StopBackgroundSound();
                AutoPlayLoopSounds[0] = bgsoundid;
                if (ENABLE_BGSOUND)
                    PlayLoopSound(AutoPlayLoopSounds[0]);
            }

            /// <summary>
            /// play 1 sound trong khoang thoi gian duration, goi ngoai mainthread
            /// </summary>
            /// <param name="soundId">tham so sound co the sua tuy theo api</param>
            /// <param name="duration">tham so thoi gian co the sua tuy vao api</param>
            public static void PlaySound(SoundId soundId) {

                //LogMng.Log("Sound", "PlaySound " + soundId + ", " + ENABLE);
                try {
                    if (ENABLE && soundId != null && Sounds[soundId] != null) {
                        //play sound
                        Sounds[soundId].loop = false;
                        Sounds[soundId].Play();
                    }
                } catch (Exception exception) {
                    Debug.LogError("PlaySound error: " + exception.Message);
                }
            }

            public static void PlayLoopSound(SoundId soundId) {
                try {
                    //LogMng.Log("Sound", "PlayLoopSound " + soundId + ", " + ENABLE_BGSOUND);
                    if (ENABLE_BGSOUND && soundId != null && Sounds[soundId] != null) {
                        //play sound

                        Sounds[soundId].loop = true;
                        Sounds[soundId].Play();
                    }
                } catch (Exception exception) {
                    Debug.LogError("PlayLoopSound error: " + exception.Message);

                }
            }

            /// <summary>
            /// sStop all sound
            /// </summary>
            public static void StopSound() {
                Dictionary<SoundId, AudioSource>.Enumerator elements = Sounds.GetEnumerator();
                while (elements.MoveNext())
                    if (elements.Current.Value != null)
                        elements.Current.Value.Stop();
                //for (int i = 0; i < Sounds.Length; i++)
                //    if(Sounds[i] != null)
                //        Sounds [i].Stop ();
            }

            public static void StopBackgroundSound() {
                try {
                    for (int i = 0; i < AutoPlayLoopSounds.Length; i++) {
                        Sounds[AutoPlayLoopSounds[i]].Stop();
                    }
                } catch (Exception exception) {
                    Debug.LogError("StopBackgroundSound error: " + exception.Message);

                }
            }


            public static void PauseSound(SoundId soundId) {
                try {
                    if (soundId != null && Sounds[soundId] != null) {
                        Sounds[soundId].Pause();
                    }
                } catch (Exception exception) {
                    Debug.LogError("PauseSound error: " + exception.Message);

                }
            }
        }

        #endregion
    }
}