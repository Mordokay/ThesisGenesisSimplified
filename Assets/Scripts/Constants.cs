using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour {

    public const int MINIMUM_SCORE_FOR_MESSAGE = 15;
    public const int MINIMUM_FRIENDSHIP_LEVEL = 0;

    public static List<string> tagNames = new List<string>() {
        "Destruction",
        "Fence",
        "Mining",
        "Rock",
        "Strength",
        "Gathering",
        "Wood",
        "Lumberjack",
        "Bravery",
        "Cactus" };

    public enum tagTypes
    {
        Destruction,
        Fence,
        Mining,
        Rock,
        Strength,
        Gathering,
        Wood,
        Lumberjack,
        Bravery,
        Cactus
    }
}
