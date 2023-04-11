using System;
using BepInEx;
using UnityEngine;
using Utilla;
using UnityEngine.UI;
using System.Collections;
using GorillaLocomotion;
using UnityEngine.XR;
using Random = UnityEngine.Random;

namespace MonkeSayMonkeDo
{
	[ModdedGamemode]
	[BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
	[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
	[ModdedGamemode("monkesquests", "MONKE'S QUESTS", Utilla.Models.BaseGamemode.Casual)]

	public class Plugin : BaseUnityPlugin
	{
        bool inRoom;

        #region ugly
        string[] commands = new string[] { "ENABLE A COSMETIC AND FIND ANOTHER PLAYER WITH THAT COSMETIC!", "DO A HANDSHAKE WITH A PLAYER!", "T-POSE FOR 10 SECONDS WITHOUT GETTING TAGGED!", "WAVE TO ANOTHER PLAYER! (THE PLAYER NEEDS TO WAVE BACK)",
			"STAND ON ONE LEG FOR 10 SECONDS WITHOUT GETTING TAGGED!", "BLOW A KISS TO ANOTHER PLAYER!", "MAKE A HEART SHAPE WITH YOUR HANDS AND GET ANOTHER PLAYER TO DO IT BACK!", "GIVE A THUMBS UP TO ANOTHER PLAYER! (PLAYER NEEDS TO GIVE A THUMBS UP BACK)",
			 "TAG 3 PEOPLE!", "DO A SILLY DANCE WITH A RANDOM MONKE!", "RUN TO THE MIRROR IN 10 SECONDS FROM STUMP OR 15 IDK", "(MY PERSONAL FAVORITE) EXPLAIN TO A RANDOM PLAYER WHY THE EARTH IS FLAT AND GET THEM TO AGREE WITH YOU!", "START A CONGA LINE WITH AT LEAST 3 PLAYERS!", "FIND A PURPLE MONKE!", "FIND 2 PEOPLE WITH THE BANANA HAT!",
		"BE THE LAST ONE TO SURVIVE IN INFECTION!","FIND SOMEONE WITH THE GHOST BALLOON!", "FIND SOMEONE SINGING A SONG!", "FIND SOMEONE THAT PLAYS ON THE QUEST 1!", "DO THE SECRET TUNNEL IN MOUNTAINS MAP!", "BE THE LAST ONE ALIVE IN HUNT!", "BE THE LAST ONE ALIVE IN PAINTBRAWL"};
		string[] monkes = new string[] { "MONKE SAYS: ", "MANKE SAYS:", "GIBBON SAYS:", "GORILLA SAYS:", "MANDRILL SAYS:", "CAPUCHIN SAYS:", "CHIN (MAKER) SAYS:", "BROMSTER SAYS (COOL):", "STM64 SAYS (ALSO COOL):" , "BLUESPRINGS SAYS (KEWL): ", "WORSTJE SAYS (FRIEND): "};
		#endregion

		public static Text monkeTextHeader;

		public static Text monkeText;

		bool beeButton;
		bool aButton;
		bool yButton;

		bool prevBeeButtonBool;
		bool prevaButtonBool;
		bool prevYButtonbool;

		int scoreWowie = -1;

        void Start()
		{
			Utilla.Events.GameInitialized += OnGameInitialized;
		}

		void OnEnable()
		{
			HarmonyPatches.ApplyHarmonyPatches();
		}

		void OnDisable()
		{
			monkeTextHeader.text = "GORILLA CODE OF CONDUCT";
			monkeText.text = "- NO RACISM, SEXISM, HOMOPHOBIA, TRANSPHOBIA, OR OTHER BIGOTRY" + "\n" + " - NO CHEATS OR MODS" + "\n" + " - DO NOT HARASS OTHER PLAYERS OR INTENTIONALLY MAKE THEM UNCOMFORTABLE" + "\n" + " - DO NOT TROLL OR GRIEF LOBBIES BY BEING UNCATCHABLE OR BY ESCAPING THE MAP.TRY TO MAKE SURE EVERYONE IS HAVING FUN" + "\n" + " - IF SOMEONE IS BREAKING THIS CODE, PLEASE REPORT THEM" + "\n" + " - PLEASE BE NICE GORILLAS AND HAVE A GOOD TIME";

			HarmonyPatches.RemoveHarmonyPatches();
		}

		void OnGameInitialized(object sender, EventArgs e)
		{
			monkeText = GameObject.Find("COC Text").GetComponent<Text>(); 
			monkeTextHeader = GameObject.Find("CodeOfConduct").GetComponent<Text>();

		}

		void Update()
		{
			int randomCommand = Random.Range(0, commands.Length);
			int randomMonke = Random.Range(0, monkes.Length);

			string command = commands[randomCommand];
			string monke = monkes[randomMonke];

			InputDevice leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
			leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out yButton);

			InputDevice rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
			rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out beeButton);
			rightController.TryGetFeatureValue(CommonUsages.primaryButton, out aButton);


			if (!inRoom)
				return;


			if (beeButton && !prevBeeButtonBool && !aButton && !yButton)
			{
				scoreWowie++;

				monkeTextHeader.text = monke;
				monkeText.text = command + "\n" + "\n" + "SCORE: " + scoreWowie;
			}
			if (aButton && !prevaButtonBool && !yButton && !beeButton)
            {
				scoreWowie = -1;
				monkeTextHeader.text = "GORILLA CODE OF CONDUCT";
				monkeText.text = "- NO RACISM, SEXISM, HOMOPHOBIA, TRANSPHOBIA, OR OTHER BIGOTRY" + "\n" + " - NO CHEATS OR MODS" + "\n" + " - DO NOT HARASS OTHER PLAYERS OR INTENTIONALLY MAKE THEM UNCOMFORTABLE" + "\n" + " - DO NOT TROLL OR GRIEF LOBBIES BY BEING UNCATCHABLE OR BY ESCAPING THE MAP.TRY TO MAKE SURE EVERYONE IS HAVING FUN" + "\n" + " - IF SOMEONE IS BREAKING THIS CODE, PLEASE REPORT THEM" + "\n" + " - PLEASE BE NICE GORILLAS AND HAVE A GOOD TIME";
			}
			if (yButton && !prevYButtonbool && !beeButton && !aButton)
            {
				scoreWowie--;

				monkeTextHeader.text = monke;
				monkeText.text = command + "\n" + "\n" + "SCORE: " + scoreWowie;
			}

			prevBeeButtonBool = beeButton;
			prevaButtonBool = aButton;
			prevYButtonbool = yButton;

			if(scoreWowie > 20)
            {
				monkeText.text = "WOW! YOU COMPLETED MONKE'S QUESTS!" + "\n" + "IF YOU HAVE ANY NEW FUN TASKS FOR THE MOD DONT BE AFRAID TO DM ME! 'CHINEESJE#0001'";
				scoreWowie = -1;
			}
		}


		[ModdedGamemodeJoin]
		public void OnJoin(string gamemode)
		{
			monkeTextHeader.text = "MONKE'S QUESTS!";
			monkeText.text = "PRESS 'A' ON YOUR CONTROLLER TO PLAY OR PRESS 'B' TO RESET THE BOARD!" + "\n"  + "\n" + "IF YOU WANT TO SKIP AN TASK YOU CAN PRESS 'Y' ON YOUR CONTROLLER BUT YOUR SCORE WILL DECREASE BY 1! " + "\n" + "\n" + "IF YOU HAVE COMPLETED A TASK PRESS 'A' TO GET AN POINT AND A NEW TASK!" + "\n"+ "\n" + "SINCE I OWN A QUEST THE INPUT BUTTON NAMES ARE MEANT FOR THE QUEST SO THE INPUT BUTTONS CAN BE A BIT DIFFERENT SORRY! ";

			inRoom = true;
		}

		[ModdedGamemodeLeave]
		public void OnLeave(string gamemode)
		{
			inRoom = false;
		}
	}
}
