using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Run_test_Alex : Run_Desk {
    
    public override void MakeMinigames()
    {
        Interface inface;
        //inface = interface_mouse_screenspace;
        //inface = interface_leap;
        //inface = interface_xbox;
        inface = interface_wiimote;

        //AddMinigame(new tutorial_select_three(), inface);

        //AddMinigame(new tutorial_select_three(), inface);
        AddMinigame(new sort_by_color(), inface);

        //Xbox.onDown_A += () => Debug.Log("A button");
    }
}
