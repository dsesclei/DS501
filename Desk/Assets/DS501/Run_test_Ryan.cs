using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Run_test_Ryan : Run_Desk {
    
    public override void MakeMinigames()
    {
        AddMinigame( new sort_by_color() );
    }
}
