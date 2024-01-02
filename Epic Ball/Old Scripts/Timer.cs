using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    [HideInInspector] public float currentTime;
    public float timeStart;
    bool timerActive = false;
    Ball ball;
    public List<int> goldTimes = new List<int> ();
    public List<int> silverTimes = new List<int> ();
    PlayerPrefsController playerPrefsController;

    void Start()
    {
        ball = FindObjectOfType<Ball> ();
        playerPrefsController = FindObjectOfType<PlayerPrefsController>();
        currentTime = Time.time;
        MakeGoldTimes();
        MakeSilverTimes ();
    }

    void Update()
    {
        if (timerActive)
        {
            timeStart += Time.deltaTime;
        }
    }
    
    private void MakeGoldTimes()
    {
        goldTimes.Add (1);//
        goldTimes.Add (13);//  Gem Collecting
        goldTimes.Add (20);//  Simple
        goldTimes.Add (20);//  Stairs
        goldTimes.Add (16);//  Scitter Scatter
        goldTimes.Add (35);//  Shapes
        goldTimes.Add (25);//  Cave In
        goldTimes.Add (45);//  Rollers From Hell
        goldTimes.Add (55);//  Temple
        goldTimes.Add (95);//  Consume
        goldTimes.Add (25);//  Back to the Beginning
        goldTimes.Add (45);//  Tele Collect
        goldTimes.Add (45);//  Shrapnel
        goldTimes.Add (40);//  Divided
        goldTimes.Add (80);//  Explode
        goldTimes.Add (30);//  Constructs
        goldTimes.Add (145);//  Down We Go
        goldTimes.Add (100);//  Slider
        goldTimes.Add (125);//  Lever
        goldTimes.Add (50);//  Dodger Field
        goldTimes.Add (150);//  Coniption
        goldTimes.Add (45);//  It's Raining
        goldTimes.Add (130);//  Falling Bridges
        goldTimes.Add (140);//  Tele Maze
        goldTimes.Add (55);//  Balance Beams
        goldTimes.Add (450);//  Enemy City
        goldTimes.Add (120);//  Demolition Gauntlet
        goldTimes.Add (85);//  Suspended
        goldTimes.Add (65);//  Warehouse
        goldTimes.Add (140);//  Jumpy
        goldTimes.Add (50);//  Omg
        goldTimes.Add (140);//  Holy Cow
        goldTimes.Add (125);//  Mines
        goldTimes.Add (140);//  TRAPS
        goldTimes.Add (120);//  =)
        goldTimes.Add (180);//  Race
        goldTimes.Add (435);//  Push Blocks
        goldTimes.Add (360);//  Towers
        goldTimes.Add (140);//  Labyrinth
        goldTimes.Add (255);//  The Chase
        goldTimes.Add (260);//  Ballerina
        goldTimes.Add (55);//  Ball Pool
        goldTimes.Add (165);//  Tippy
    }

    private void MakeSilverTimes()
    {
        silverTimes.Add (2);//
        silverTimes.Add (20);//  Gem Collecting
        silverTimes.Add (30);//  Simple
        silverTimes.Add (30);//  Stairs
        silverTimes.Add (23);//  Scitter Scatter
        silverTimes.Add (45);//  Shapes
        silverTimes.Add (35);//  Cave In
        silverTimes.Add (60);//  Rollers From Hell
        silverTimes.Add (75);//  Temple
        silverTimes.Add (120);//  Consume
        silverTimes.Add (40);//  Back to the Beginning
        silverTimes.Add (60);//  Tele Collect
        silverTimes.Add (60);//  Shrapnel
        silverTimes.Add (55);//  Divided
        silverTimes.Add (90);//  Explode
        silverTimes.Add (40);//  Constructs
        silverTimes.Add (165);//  Down We Go
        silverTimes.Add (120);//  Slider
        silverTimes.Add (140);//  Lever
        silverTimes.Add (60);//  Dodger Field
        silverTimes.Add (170);//  Coniption
        silverTimes.Add (65);//  It's Raining
        silverTimes.Add (150);//  Falling Bridges 
        silverTimes.Add (170);//  Tele Maze
        silverTimes.Add (75);//  Balance Beams
        silverTimes.Add (490);//  Consumer City
        silverTimes.Add (140);//  Demolition Gauntlet
        silverTimes.Add (100);//  Suspended
        silverTimes.Add (80);//  Warehouse
        silverTimes.Add (160);//  Jumpy
        silverTimes.Add (70);//  Omg
        silverTimes.Add (165);//  Holy Cow
        silverTimes.Add (145);//  Mines
        silverTimes.Add (180);//  TRAPS
        silverTimes.Add (150);//  =)
        silverTimes.Add (210);//  Race
        silverTimes.Add (500);//  Push Blocks
        silverTimes.Add (400);//  Towers
        silverTimes.Add (165);//  Labyrinth
        silverTimes.Add (255);//  The Chase
        silverTimes.Add (290);//  Ballerina
        silverTimes.Add (70);//  Ball Pool
        silverTimes.Add (180);//  Tippy
    }
    
    public void StartTimer()
    {
        timeStart = 0;
        timerActive = true;
    }

    public void EndTimer()
    {
        timerActive = false;
    }

    public void SaveTime(int levelTimeKey)
    {
        EndTimer ();
        int lastTime = PlayerPrefs.GetInt ("LEVEL_" + levelTimeKey + "_TIME_KEY");
        if (lastTime > (int) timeStart || lastTime == 0)
        {
            PlayerPrefs.SetInt ("LEVEL_" + levelTimeKey + "_TIME_KEY", (int) timeStart);
        }
    }

    public int GetTime(int levelNumber)
    {
        if (PlayerPrefs.HasKey ("LEVEL_" + levelNumber + "_TIME_KEY"))
        {
            int keyTime = PlayerPrefs.GetInt ("LEVEL_" + levelNumber + "_TIME_KEY");
            return keyTime;
        }
        else
        {
            return 0;
        }
    }
    
    public string StringTime(int endTime)
    {
        return
        Mathf.Floor (endTime / 60).ToString ("00") + ":" + Mathf.FloorToInt (endTime % 60).ToString ("00");
    }

}
