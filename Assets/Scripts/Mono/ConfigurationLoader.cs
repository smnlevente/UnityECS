using System;
using System.IO;
using UnityEngine;

public class ConfigurationLoader : Singleton<ConfigurationLoader>
{
    public Configuration.Player player;
    public Configuration.Player enemy;
    public Configuration.BattlefieldSettings settings;
    public Configuration.TeamPresets[] teamPresets;

    private void Awake()
    {
        var configFileString = File.ReadAllText($"{Application.streamingAssetsPath}/Configuration.json");
        var config = JsonUtility.FromJson<Configuration>(configFileString);
        player = config.playerOne;
        enemy = config.playerTwo;
        settings = config.settings;
        teamPresets = config.teamPresets;
    }

    [Serializable]
    public sealed class Configuration
    {
        public BattlefieldSettings settings;
        public Player playerOne;
        public Player playerTwo;
        public TeamPresets[] teamPresets;
        [Serializable]
        public sealed class Player
        {
            public int xGridCount;
            public int yGridCount;
            public int unitHealth;
            public int unitDPS;
            public int unitRange;
            public int unitSpeed;
            public int[] teamStructure;
        }

        [Serializable]
        public sealed class BattlefieldSettings
        {
            public float offset;
            public int xPadding;
            public int yPadding;
        }

        [Serializable]
        public sealed class TeamPresets
        {
            public int[] team;
        }
    }
}
