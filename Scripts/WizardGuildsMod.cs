// Project:         Wizard Guilds Mod for Daggerfall Unity (http://www.dfworkshop.net)
// Copyright:       Copyright (C) 2022 Broseidon
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Author:          Broseidon
// Contributor:     Hazelnut

using System;
using System.Collections.Generic;
using UnityEngine;
using DaggerfallConnect.Arena2;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Guilds;
using DaggerfallWorkshop.Game.Questing;
using DaggerfallWorkshop.Game.Utility.ModSupport;

namespace WizardGuilds
{
    public class WizardGuildsMod : MonoBehaviour
    {
        static Mod mod;

        [Invoke(StateManager.StateTypes.Start, 0)]
        public static void Init(InitParams initParams)
        {
            mod = initParams.Mod;
            var go = new GameObject(mod.Title);
            go.AddComponent<WizardGuildsMod>();
        }

        void Awake()
        {
            InitMod();
            mod.IsReady = true;
        }

        public static void InitMod()
        {
            Debug.Log("Begin mod init: WizardGuilds");

            // Register the new faction id's
            if (RegisterFactionIds())
            {
                // Register the Guild classes
                if (!GuildManager.RegisterCustomGuild(FactionFile.GuildGroups.GGroup18, typeof(BrotherhoodOfGalenGuild)))
                    throw new Exception("GuildGroup GGroup18 is already in use, unable to register Brotherhood Of Galen Guild.");

                // Register the quest list
                if (!QuestListsManager.RegisterQuestList("WizardGuilds"))
                    throw new Exception("Quest list name is already in use, unable to register WizardGuilds quest list.");

                // Register the Brotherhood of Galen quest service id
                Services.RegisterGuildService(1101, GuildServices.Quests);

                // Register the spell seller service
                Services.RegisterGuildService(1121, GuildServices.BuySpells);
                // Register the daedra summoning service
                Services.RegisterGuildService(1122, GuildServices.DaedraSummoning);
                // Register the spell maker service
                Services.RegisterGuildService(1123, GuildServices.MakeSpells);
                // Register the training service id
                Services.RegisterGuildService(1124, GuildServices.Training);
                // Register the indentification service id
                Services.RegisterGuildService(1125, GuildServices.Identify);
                // Register the buy potions service id
                Services.RegisterGuildService(1126, GuildServices.BuyPotions);
                // Register the make potions service id
                Services.RegisterGuildService(1127, GuildServices.MakePotions);
                // Register the make potions service id
                Services.RegisterGuildService(1128, GuildServices.MakeMagicItems);
            }
            else
                throw new Exception("Faction id's are already in use, unable to register factions for Wizard Guild.");

            Debug.Log("Finished mod init: WizardGuilds");
        }

        private static bool RegisterFactionIds()
        {
            // Guild factions:

            bool success = FactionFile.RegisterCustomFaction(1100, new FactionFile.FactionData()
            {
                id = 1100,
                parent = (int)FactionFile.FactionIDs.Daggerfall,
                type = 4,
                name = "Harlwystyr",
                summon = -1,
                region = 18,    // Daggerfall
                power = 25,     // Bro: change these lines as you want them... I copied from Lord Verathon in my RR mod
                face = 12,
                race = 2,
                flat1 = (183 << 7) + 20,
                sgroup = 3,     // Bro: no more changes now
                ggroup = 18,
                children = new List<int>() { 1101 }
            });
            success = FactionFile.RegisterCustomFaction(1101, new FactionFile.FactionData()
            {
                id = 1101,
                parent = 1100,
                type = 2,
                name = "The Brotherhood of Galen",
                summon = -1,
                region = -1,
                power = 30,
                enemy1 = (int)FactionFile.FactionIDs.The_Knights_of_the_Rose,   // do you want factions to be enemies - we should discuss?
                face = -1,
                race = -1,
                sgroup = 2,
                ggroup = 18,
                children = null
            });


            // Generic sub-factions

            success = FactionFile.RegisterCustomFaction(1120, new FactionFile.FactionData()
            {
                id = 1120,
                parent = 0,
                type = 2,
                name = "Generic Wizards",
                summon = -1,
                region = -1,
                power = 5,
                face = -1,
                race = -1,
                sgroup = 7,
                ggroup = 18,
                children = new List<int>() { 1121, 1122, 1123, 1124, 1125, 1126, 1127, 1128 }
            }) && success;
            success = FactionFile.RegisterCustomFaction(1121, new FactionFile.FactionData()
            {
                id = 1121,
                parent = 1120,
                type = 2,
                name = "Wizard Spell Sellers",
                summon = -1,
                region = -1,
                power = 10,
                face = -1,
                race = -1,
                sgroup = 2,
                ggroup = 18,
                children = null
            }) && success;
            success = FactionFile.RegisterCustomFaction(1122, new FactionFile.FactionData()
            {
                id = 1122,
                parent = 1120,
                type = 2,
                name = "Wizard Summoners",
                summon = -1,
                region = -1,
                power = 15,
                face = -1,
                race = -1,
                sgroup = 2,
                ggroup = 18,
                children = null
            }) && success;
            success = FactionFile.RegisterCustomFaction(1123, new FactionFile.FactionData()
            {
                id = 1123,
                parent = 1120,
                type = 2,
                name = "Wizard Spell Makers",
                summon = -1,
                region = -1,
                power = 15,
                face = -1,
                race = -1,
                sgroup = 2,
                ggroup = 18,
                children = null
            }) && success;
            success = FactionFile.RegisterCustomFaction(1124, new FactionFile.FactionData()
            {
                id = 1124,
                parent = 1120,
                type = 2,
                name = "Wizard Trainers",
                summon = -1,
                region = -1,
                power = 15,
                face = -1,
                race = -1,
                sgroup = 2,
                ggroup = 18,
                children = null
            }) && success;
            success = FactionFile.RegisterCustomFaction(1125, new FactionFile.FactionData()
            {
                id = 1125,
                parent = 1120,
                type = 2,
                name = "Wizard Indentifiers",
                summon = -1,
                region = -1,
                power = 15,
                face = -1,
                race = -1,
                sgroup = 2,
                ggroup = 18,
                children = null
            }) && success;
            success = FactionFile.RegisterCustomFaction(1126, new FactionFile.FactionData()
            {
                id = 1126,
                parent = 1120,
                type = 2,
                name = "Wizard Apothecaries",
                summon = -1,
                region = -1,
                power = 15,
                face = -1,
                race = -1,
                sgroup = 2,
                ggroup = 18,
                children = null
            }) && success;
            success = FactionFile.RegisterCustomFaction(1127, new FactionFile.FactionData()
            {
                id = 1127,
                parent = 1120,
                type = 2,
                name = "Wizard Mixers",
                summon = -1,
                region = -1,
                power = 15,
                face = -1,
                race = -1,
                sgroup = 2,
                ggroup = 18,
                children = null
            }) && success;
            success = FactionFile.RegisterCustomFaction(1128, new FactionFile.FactionData()
            {
                id = 1128,
                parent = 1120,
                type = 2,
                name = "Wizard Enchanters",
                summon = -1,
                region = -1,
                power = 15,
                face = -1,
                race = -1,
                sgroup = 2,
                ggroup = 18,
                children = null
            }) && success;
            return success;
        }

    }
}