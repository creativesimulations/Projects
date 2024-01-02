using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Core
{
    public class EventTrigger : MonoBehaviour
    {

      //  public delegate void OnJumpOutAction(int i);
        // Set up the event called OnDeath to be published, using the delegate OnDeathAction
       // public static event OnJumpOutAction OnJumpOut;

        private void Start()
        {
        }

        private void Update()
        {
            if (Input.anyKeyDown && !(Input.GetMouseButtonDown(0)
                || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
            {
                string keyPressed = Input.inputString;
            //    if (keyPressed == "1" || keyPressed == "2" || keyPressed == "3" || keyPressed == "4" || keyPressed == "5" || keyPressed == "6" || keyPressed == "7" || keyPressed == "8")
            //    {
            //        humanSpawner.ChooseShipToSpawn(keyPressed);
            //    }
            //    else if (keyPressed == "q" || keyPressed == "w" || keyPressed == "e" || keyPressed == "r" || keyPressed == "t" || keyPressed == "y" || keyPressed == "u" || keyPressed == "i")
            //    {
            //        alien1Spawner.ChooseShipToSpawn(keyPressed);
            //    }
            //    else if (keyPressed == "a" || keyPressed == "s" || keyPressed == "d" || keyPressed == "f" || keyPressed == "g" || keyPressed == "h" || keyPressed == "j" || keyPressed == "k")
            //    {
            //        alien2Spawner.ChooseShipToSpawn(keyPressed);
            //    }
            //    else if (keyPressed == "z" || keyPressed == "x" || keyPressed == "c" || keyPressed == "v" || keyPressed == "b" || keyPressed == "n" || keyPressed == "m" || keyPressed == ",")
            //    {
            //        alien3Spawner.ChooseShipToSpawn(keyPressed);
            //    }

            //    else if (Input.GetKeyDown(KeyCode.F1) && OnJumpOut != null)
            //    {
            //        int i = 1;
            //        // Announce jump out to subscibers
            //        OnJumpOut(i);
            //    }

            //    else if (Input.GetKeyDown(KeyCode.F2) && OnJumpOut != null)
            //    {
            //        int i = 2;
            //        // Announce jump out to subscibers
            //        OnJumpOut(i);
            //    }
            //    else if (Input.GetKeyDown(KeyCode.F3) && OnJumpOut != null)
            //    {
            //        int i = 3;
            //        // Announce jump out to subscibers
            //        OnJumpOut(i);
            //    }
            //    else if(Input.GetKeyDown(KeyCode.F4) && OnJumpOut != null)
            //    {
            //        int i = 4;
            //        // Announce jump out to subscibers
            //        OnJumpOut(i);
            //    }
            }
        }

    }
}