using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {

    private void Start()
    {
        
    }

    private void Test_1()
    {
        //This code is used to print all words being used in the game.
        string result = "";
        for (int world = 0; world < 6; world++)
        {
            for (int sub = 0; sub < 5; sub++)
            {
                int num = Superpow.Utils.GetNumLevels(world, sub);
                for (int level = 0; level < num; level++)
                {
                    var gameLevel = Resources.Load<GameLevel>("World_" + world + "/SubWorld_" + sub + "/Level_" + level);
                    result += gameLevel.word + "\n";
                }
            }
        }
        Debug.Log(result);
    }

    private void Test_2()
    {
        for (int world = 0; world < 6; world++)
        {
            for (int sub = 0; sub < 5; sub++)
            {
                int num = Superpow.Utils.GetNumLevels(world, sub);
                for (int level = 0; level < num; level++)
                {
                    var gameLevel = Resources.Load<GameLevel>("World_" + world + "/SubWorld_" + sub + "/Level_" + level);

                    var answers = CUtils.BuildListFromString<string>(gameLevel.answers);
                    if (gameLevel.word.Contains("a") && gameLevel.word.Contains("m") && answers[0].Length == 2)
                    {
                        print(world + "|" + sub + "|" + level);
                    }
                }
            }
        }
    }
}
