﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Run_test_Dave : Run_Desk {
    
    public override void MakeMinigames()
    {
        AddMinigame( new select_three() );
    }
}
