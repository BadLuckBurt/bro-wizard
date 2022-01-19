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
            TextFile.CreateTextToken("Very well, that is good enough for me, %pcf. Now then,"), newLine,
            TextFile.CreateTextToken("it is my privilege to welcome you into the Brotherhood"), newLine,
            TextFile.CreateTextToken("of Galen. Show determination, and act ever to preserve"), newLine,
            TextFile.CreateTextToken("Daggerfall, and I foresee you will go far very indeed."), newLine,

            TextFile.CreateTextToken("As one of us, you will always find restful"), newLine,
            TextFile.CreateTextToken("accomodations made ready for you not only"), newLine,
            TextFile.CreateTextToken("within this tower, but across the inns of the"), newLine,
            TextFile.CreateTextToken("kingdom. As you rise through our ranks, greater"), newLine,
            TextFile.CreateTextToken("benefits will also follow with your responsibilities."), newLine,
            TextFile.CreateTextToken("But all that in due time - welcome once again, Initiate %pcf."), newLine,
        };

        protected static TextFile.Token[] eligibleTokens =
        {
            TextFile.CreateTextToken("You wish to join the Brotherhood? Your character and"), newLine,
            TextFile.CreateTextToken("skills indeed match what we are looking for among"), newLine,
            TextFile.CreateTextToken("our numbers, but what of your motivations? As a member,"), newLine,
            TextFile.CreateTextToken("your loyal service must be given unfailingly to the"), newLine,
            TextFile.CreateTextToken("Royal Magician, and thereby you must pledge your life"), newLine,
            TextFile.CreateTextToken("to unswervingly serve the throne of Daggerfall"), newLine,

            TextFile.CreateTextToken("Are you ready to make such a commitment, %pcf?"), newLine,
        };

        protected static TextFile.Token[] ineligibleLowSkillTokens =
        {
            TextFile.CreateTextToken("Hmm, well - your heart may be in the right"), newLine,
            TextFile.CreateTextToken("place, but you simply do not possess the"), newLine,
            TextFile.CreateTextToken("magical prowess we expect as a minimum from"), newLine,
            TextFile.CreateTextToken("members of the Brotherhood of Galen, I'm sorry."), newLine,
        };

        protected static TextFile.Token[] ineligibleBadRepTokens =
        {
            TextFile.CreateTextToken("I already know who you are, %pcf, and so am also aware of your"), newLine,
            TextFile.CreateTextToken("less than stellar reputation within the kingdom. I'm simply"), newLine,
            TextFile.CreateTextToken("cannot allow one such as you to become part of the Brotherhood."), newLine,
        };

        protected static TextFile.Token[] ineligibleLowIntTokens =
        {
            TextFile.CreateTextToken("I am sorry, %pcf, but the Brotherhood can only afford to"), newLine,
            TextFile.CreateTextToken("to entrust the security of the realm to someone of keen"), newLine,
            TextFile.CreateTextToken("mind. My advice? Perhaps the Fighters Guild can take you."), newLine,
        };

        protected static TextFile.Token[] promotionTokens =
        {
            TextFile.CreateTextToken("You have proven yourself worthy and skillful enough to"), newLine,
            TextFile.CreateTextToken("ascend from the rank of Initiate, %pcf. Henceforth, you"), newLine,
            TextFile.CreateTextToken("hold the rank of Apprentice. As of this moment, access"), newLine,
            TextFile.CreateTextToken("to our instructors, and their repositories of spells, will be given to you."), newLine,
        };

        protected static int[] intReqs = { 40, 40, 45, 45, 50, 55, 60, 65, 70, 75 }; // Bro?

        protected static string[] rankTitles = {
            "Initiate", "Apprentice", "Sorcerer", "Magus", "Battle Magus", "Wizard", "War Wizard", "Master Wizard", "Castellan of Magic", "Lord Warder"
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
