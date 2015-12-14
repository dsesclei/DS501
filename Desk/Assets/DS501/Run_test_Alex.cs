using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Run_test_Alex : Run_Desk {
    
    public override void MakeMinigames()
    {
        Interface inface;
        //inface = interface_leap;
        //inface = interface_xbox;
        inface = interface_wiimote;

        AddMinigame(new tutorial_select_three(), inface);
    }
}
