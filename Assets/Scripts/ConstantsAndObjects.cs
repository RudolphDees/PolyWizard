using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Timers;

namespace ConstantsAndObjects
{
    public class Constants
    {
        public static Color getFireColor(){
            return new Color(1,0,0,1);
        }
        public static Color getWaterColor(){
            return new Color(0,0,1,1);
        }
        public static Color32 getEarthColor(){
            return new Color32(135,58,2,255);
        }
        public static Color getLightningColor(){
            return new Color(1,1,0,1);
        }
    }
}