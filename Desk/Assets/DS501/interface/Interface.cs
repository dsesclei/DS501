using UnityEngine;
using System.Collections;

public interface Interface
{
    void init();
    void on();
    void off();

    void update();

    Vector2 get_ScreenspacePosition();
    Vector2 get_ScreenspacePosDelta();

    Quaternion get_Rotation();

    bool get_Button();
}
