// Project:         Wizards Guild for Daggerfall Unity (http://www.dfworkshop.net)
// Copyright:       Copyright (C) 2022 Broseidon
// License:         MIT License (http://www.opensource.org/licenses/mit-license.php)
// Author:          Broseidon
// Contributor:     Hazelnut

using System.Collections.Generic;
using DaggerfallConnect;
using DaggerfallConnect.Arena2;
using DaggerfallWorkshop.Game.Entity;

namespace DaggerfallWorkshop.Game.Guilds
{
    // much of this class will be moved to abstract parent to each of 4 region wizards sects

    public class BrotherhoodOfGalenGuild : Guild
    {
        #region Constants

        private const int factionId = 1101; 

        #endregion

        #region Static Data

        protected static TextFile.Token newLine = TextFile.CreateFormatToken(TextFile.Formatting.JustifyCenter);

        // Guild messages - must clone any that contain macros before returning.

        protected static TextFile.Token[] welcomeTokens =
        {
            TextFile.CreateTextToken("Excellent, %pcn, welcome to the Wizards! "), newLine, newLine,

            TextFile.CreateTextToken(" "), newLine,
            TextFile.CreateTextToken(" "), newLine,
            TextFile.CreateTextToken(" "), newLine,
            TextFile.CreateTextToken(" "), newLine,
            TextFile.CreateTextToken(" "), newLine,
            TextFile.CreateTextToken(" "), newLine,
        };

        protected static TextFile.Token[] eligibleTokens =
        {
            TextFile.CreateTextToken("Hmm, yes, you seem like a good sort.... tbc"), newLine,
            TextFile.CreateTextToken(" blah blah "), newLine, newLine,

            TextFile.CreateTextToken("We offer classes in stuff "), newLine,
        };

        protected static TextFile.Token[] ineligibleLowSkillTokens =
        {
            TextFile.CreateTextToken("I am sad to say that you are not eligible to join our guild."), newLine,
            TextFile.CreateTextToken("We only accept members who are not you. "), newLine,
            TextFile.CreateTextToken("other skills such as climbing, "), newLine,
            TextFile.CreateTextToken("lockpicking, or stealth. "), newLine,
        };

        protected static TextFile.Token[] ineligibleBadRepTokens =
        {
            TextFile.CreateTextToken("I am sad to say that you are ineligible to join our guild."), newLine,
            TextFile.CreateTextToken("Your reputation amongst scholars is such that we do not "), newLine,
            TextFile.CreateTextToken("wish to be associated with you, even for simple field work. "), newLine,
        };

        protected static TextFile.Token[] ineligibleLowIntTokens =
        {
            TextFile.CreateTextToken("Sorry, %pcf, you do not exhibit the intellect we require "), newLine,
            TextFile.CreateTextToken("from our recruits. Perhaps a less scholarly guild, such "), newLine,
            TextFile.CreateTextToken("as the Fighters guild, would be more suited to your aptitude. "), newLine,
        };

        protected static TextFile.Token[] promotionTokens =
        {
            TextFile.CreateTextToken("Congratulations, %pcf. Because of your outstanding work for "), newLine,
            TextFile.CreateTextToken("the guild, we have promoted you to the rank of %lev. "), newLine,
            TextFile.CreateTextToken("Keep up the good work, and continue to study hard. "), newLine,
        };

        protected static int[] intReqs = { 30, 35, 40, 45, 50, 55, 60, 65, 65, 65 }; // Bro?

        protected static string[] rankTitles = {
            "Initiate", "Apprentice", "Sorcerer", "Magus", "Battle-Magus", "Wizard", "War Wizard", "Master Wizard", "Castellan of Magic", "Lord Warder"
        };

        protected static List<DFCareer.Skills> guildSkills = new List<DFCareer.Skills>() {
                DFCareer.Skills.Alteration,
                DFCareer.Skills.Destruction,
                DFCareer.Skills.Illusion,
                DFCareer.Skills.Mysticism,
                DFCareer.Skills.Restoration,
                DFCareer.Skills.Thaumaturgy,
                DFCareer.Skills.LongBlade,
                DFCareer.Skills.BluntWeapon,
                DFCareer.Skills.Etiquette,
            };

        protected static List<DFCareer.Skills> trainingSkills = guildSkills;     // Bro? no differnt skills can be trained?

        #endregion

        #region Properties

        public override string[] RankTitles { get { return rankTitles; } }

        public override List<DFCareer.Skills> GuildSkills { get { return guildSkills; } }

        public override List<DFCareer.Skills> TrainingSkills { get { return trainingSkills; } }

        #endregion

        #region Guild Membership and Faction

        public static int FactionId { get { return factionId; } }

        public override int GetFactionId()
        {
            return factionId;
        }

        #endregion

        #region Guild Ranks

        protected override int CalculateNewRank(PlayerEntity playerEntity)
        {
            int newRank = base.CalculateNewRank(playerEntity);
            int peINT = playerEntity.Stats.GetPermanentStatValue(DFCareer.Stats.Intelligence);
            while (peINT < intReqs[newRank])
                newRank--;
            return newRank;
        }

        public override TextFile.Token[] TokensPromotion(int newRank)
        {
            return (TextFile.Token[]) promotionTokens.Clone();
        }

        #endregion

        #region Benefits

        public override bool CanRest()
        {
            return IsMember();
        }

        public override bool HallAccessAnytime()
        {
            return (rank >= 4); // Bro?
        }

        #endregion

        #region Service Access:

        public override bool CanAccessLibrary()
        {
            return (rank >= 3);
        }

        public override bool CanAccessService(GuildServices service)
        {
            switch (service)
            {
                case GuildServices.Training:
                    return IsMember();
                case GuildServices.Quests:
                    return true;
                case GuildServices.Identify:
                    return (rank >= 2);
                case GuildServices.BuyPotions:
                    return (rank >= 4);
                case GuildServices.MakePotions:
                    return (rank >= 4);
                case GuildServices.DaedraSummoning:
                    return (rank >= 9);
                case GuildServices.MakeMagicItems:
                    return (rank >= 7);
            }

            return false;
        }

        #endregion

        #region Joining

        public override bool IsEligibleToJoin(PlayerEntity playerEntity)
        {
            // TODO: Check mutual exclusive guilds here and give message that you are member so not elligible. (maybe use Tokens below?)

            // Check reputation & skills
            int rep = playerEntity.FactionData.GetReputation(GetFactionId());
            int high, low;
            CalculateNumHighLowSkills(playerEntity, 0, out high, out low);
            return (rep >= rankReqReputation[0] && high > 0 && low + high > 1 &&
                    playerEntity.Stats.GetPermanentStatValue(DFCareer.Stats.Intelligence) >= intReqs[0]);
        }

        public override TextFile.Token[] TokensIneligible(PlayerEntity playerEntity)
        {
            TextFile.Token[] msg = ineligibleLowSkillTokens;
            if (GetReputation(playerEntity) < 0)
                msg = ineligibleBadRepTokens;
            if (playerEntity.Stats.GetPermanentStatValue(DFCareer.Stats.Intelligence) < intReqs[0])
                msg = (TextFile.Token[]) ineligibleLowIntTokens.Clone();
            return msg;
        }
        public override TextFile.Token[] TokensEligible(PlayerEntity playerEntity)
        {
            return eligibleTokens;
        }
        public override TextFile.Token[] TokensWelcome()
        {
            return (TextFile.Token[]) welcomeTokens.Clone();
        }

        #endregion

    }

}
