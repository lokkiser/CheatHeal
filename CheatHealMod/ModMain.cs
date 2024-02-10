using System.Collections.Generic;
using System.Diagnostics;
using GnollModLoader;
using Game;
using Game.GUI;
using Game.GUI.Controls;
using System.Collections;
using System.Linq;
using System;
using System.Security.Principal;
using System.Reflection;
using System.Text;
using Game.BehaviorTree;

namespace GnollMods.CheatHealMod
{
    public class ModMain : IGnollMod
    {
        public static ModMain instance;

        private const float _updatePeriod = 1.0f;
        private GClass0 _hudPanel;
        private CheatHealPanel _CheatHealPanel;
        private float _timeSinceLastUpdate = 0.0f;

        public string Name { get { return "CheatHeal"; } }

        public string Description { get { return "Tired of 'nomes dyin', eh? I got a thing for you!"; } }

        public string BuiltWithLoaderVersion { get { return "G1.13"; } }

        public int RequireMinPatchVersion { get { return 13; } }

        public ModMain()
        {
            instance = this;
        }

        public void OnEnable(HookManager hookManager)
        {
            hookManager.InGameHUDInit += HookManager_InGameHUDInit;
            hookManager.UpdateInGame += HookManager_UpdateInGame;
        }

        public void OnDisable(HookManager hookManager)
        {
            hookManager.InGameHUDInit -= HookManager_InGameHUDInit;
            hookManager.UpdateInGame -= HookManager_UpdateInGame;
        }

        public bool IsDefaultEnabled()
        {
            return true;
        }

        public bool NeedsRestartOnToggle()
        {
            return false;
        }

        private void HookManager_UpdateInGame(float realTimeDelta, float gameTimeDelta)
        {
            _timeSinceLastUpdate += gameTimeDelta;
            while (_timeSinceLastUpdate > _updatePeriod)
            {
                _timeSinceLastUpdate -= _updatePeriod;

                Update();
            }
        }

        private void HookManager_InGameHUDInit(Game.GUI.InGameHUD inGameHUD, Game.GUI.Controls.Manager manager)
        {
            this._hudPanel = inGameHUD.gclass0_0;
            this._CheatHealPanel = new CheatHealPanel(manager);
            _CheatHealPanel.Visible = false;
            _hudPanel.Add(_CheatHealPanel);
        }

        private void Update()
        {
            Faction playerFaction = GnomanEmpire.Instance.World.AIDirector.PlayerFaction;
            int numInFight = 0;
            _CheatHealPanel.ClearContents();

            /*            foreach (var _my in playerFaction.Members)
                        {
                            _my.Value.body_0.list_0[0].BodyPart.bodyPartStatus_0 = BodyPartStatus.Good;
                        }*/
            string _desiredname = ""; //debug

                foreach (KeyValuePair<uint, Character> gnome in playerFaction.Members)
            {
                //gnome.Value.Update(0.01f);
                if (gnome.Value.Name() == _desiredname)
                {
                    System.Console.WriteLine("Food, water, sleep {0}, {1} , {2}", gnome.Value.NameAndTitle(), !gnome.Value.IsHealthy(), gnome.Value.Body.IsDyingOfThirst);
                    System.Console.WriteLine("Food, water, sleep {0}, {1} , {2}", gnome.Value.body_0.float_6, gnome.Value.body_0.float_7, gnome.Value.body_0.float_3);
                }

                if (gnome.Value.StatusText() == "In Combat" || !gnome.Value.IsHealthy() || gnome.Value.Body.IsStarving || gnome.Value.Body.IsDyingOfThirst || gnome.Value.Body.IsSuffocating ) // in combat or not Healty
                {
                    numInFight++;
/*                    if (gnome.Value.Name() == "Binkle")
                    {
                        System.Console.WriteLine("gnome.Value {0}, {1}", gnome.Value.NameAndTitle(), gnome.Value.body_0.StatusEffects.ToList());
                        //System.Console.WriteLine("gnome.ToString() {0}", gnome.ToString());
                        System.Console.WriteLine("gnome.Value.body_0 {0}", gnome.Value.body_0);
                        //gnome.Value.body_0.list_0[0].BodyPart.bodyPartStatus_0 = BodyPartStatus.Good;

                        System.Console.WriteLine("gnome.Value.Body.BodySections[0] {0}", gnome.Value.Body.BodySections[0]);
                    }*/
                //System.Console.WriteLine(gnome.Value.body_0.StatusEffects[1]);
                //System.Console.WriteLine(gnome.Value.body_0.StatusEffects.Keys);
                //System.Console.WriteLine(Game.HealthStatusAilment);
                //System.Console.WriteLine(Game.HealthStatusEffect);
                //namespace Gamepublic enum HealthStatusAilment
                //{
                //Unconcious,
                //Dazed,
                //Faint,
                //Winded,
                //Dizzy,
                //FallenOver,
                //Grounded,
                //Blind,
                //ZombieVirus
                //}
                var button = _CheatHealPanel.AddButton(gnome.Value.NameAndTitle());
                    var _templist = new List<string>() {};

                    var myvar = gnome;
                    var _gnomename = gnome.Value.Name();
                    var _gnome_is_injured = false;
                    for (uint i = 0; i < gnome.Value.Body.BodySections.Count; i++)
                    {
                        if (gnome.Value.Body.BodySections[Convert.ToInt32(i)].Status == Game.BodySectionStatus.Missing ||  gnome.Value.Body.NeedsBandage()) 
                        {
                            _gnome_is_injured = true;
                        }

                    }
                    if (gnome.Value.IsHealthy() == false || _gnome_is_injured) button.ToolTip.Text = "Needs heealing";
                    if (gnome.Value.StatusText() == "In Combat") button.ToolTip.Text = "Just fighting";


                        button.Click += (sender, e) =>
                    {
                        //gnome.Value.SetBehavior(BehaviorType.PlayerCharacter); //reset AI?
                        //gnome.Value.FindJob();
                        if (gnome.Value.Body.CanDodge == false) //make dodge
                        {
                            gnome.Value.body_0.float_10 = 0f;
                        }
                        if (!gnome.Value.body_0.IsDead)// failsafe against raising dead and throwing exception
                        { 
                            //for (int i = 0; i < gnome.Value.body_0.dictionary_6.Count; i++)
                            foreach(KeyValuePair<Game.HealthStatusAilment, Game.HealthStatusEffect> illness in gnome.Value.body_0.dictionary_6) //heals affictions
                            {
                                System.Console.WriteLine("i gnome.Value.body_0.StatusEffects {0}, {1}", illness, gnome.Value.body_0.StatusEffects);
                                //System.Console.WriteLine("(int) i, Convert.ToInt32(i) {0}, {1}", (int)i, Convert.ToInt32(i));
                                gnome.Value.body_0.dictionary_6[illness.Key].Amount = 0.01f;
                                gnome.Value.body_0.dictionary_6[illness.Key].RecoveryRate = 100f;
                                //'System.Collections.Generic.KeyValuePair<Game.HealthStatusAilment, Game.HealthStatusEffect>'
                                //to 'System.Collections.Generic.KeyValuePair<string, Game.HealthStatusEffect>'
                            }
                            for (uint i = 0; i < gnome.Value.Body.BodySections.Count; i++) //heal limbs
                            {
                                if (gnome.Value.Body.BodySections[Convert.ToInt32(i)].Status == Game.BodySectionStatus.Missing)
                                {
                                    //new System.Collections.Generic.Mscorlib_DictionaryDebugView<uint, Game.Character>(GnomanEmpire.Instance.World.AIDirector.PlayerFaction.Members).Items[0]
                                    /*                                System.Console.WriteLine(playerFaction.Members.; 
                                                                    System.Console.WriteLine(playerFaction.Members.Members[i]);*/
                                    System.Console.WriteLine("(int) i, Convert.ToInt32(i) {0}, {1}", (int)i, Convert.ToInt32(i));
                                    //gnome.Value.body_0.list_0[Convert.ToInt32(i)].BodyPart.bodyPartStatus_0 = BodyPartStatus.Good;
                                    gnome.Value.body_0.list_0[Convert.ToInt32(i)].bodySectionStatus_0 = Game.BodySectionStatus.Good;

                                    //GnomanEmpire.Instance.World.AIDirector.PlayerFaction.Members[i].body_0.list_0[0].bodySectionStatus_0 = BodySectionStatus.Good;
                                    /*                                gnome.Value.Body.list_0[0].Status = Game.BodySectionStatus.Good;
                                                                    gnome.Value.Body.BodySections[i].Status = Game.BodySectionStatus.Good;*/
                                }
                            }
                            if (gnome.Value.body_0.float_6 < 100f) gnome.Value.body_0.float_6 = 300f; //food
                            if (gnome.Value.body_0.float_7 < 100f) gnome.Value.body_0.float_7 = 300f; //water
                            if (gnome.Value.body_0.float_3 < 100f) gnome.Value.body_0.float_3 = 300f; // sleep
                            /*                        foreach (Game.BodySection _bodypart in gnome.Value.Body.BodySections) 
                                                    {

                                                    }*/


                            gnome.Value.HealDestroyedBodySection();
                            GnomanEmpire.Instance.Camera.MoveTo(gnome.Value.Position, true, true);

                            /*
                            public BodySection NeedsArtificialLimb()
                            {
                                foreach (BodySection item in list_0)
                                {
                                    if (item.NeedsArtificialLimb())
                                    {
                                        return item;
                                    }
                                }

                                return null;
                            }
                            */
                        }



                    };
                }

                //body_0.StatusEffects TODO:



    }



            if (numInFight == 0)
            {
                _CheatHealPanel.Visible = false;
                return;
            }




            //                _hudPanel.Width - _CheatHealPanel.Width - _CheatHealPanel.margins_0.Left,
            _CheatHealPanel.SetPosition(
                _CheatHealPanel.margins_0.Left,
                _hudPanel.Height - _CheatHealPanel.Height - _CheatHealPanel.margins_0.Bottom - 50);
            _CheatHealPanel.Visible = true;
        }

    }
}
