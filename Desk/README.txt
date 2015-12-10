Still some stuff to add (timer and changing to the next minigame, many other things), but we should have everything we need to make minigame tasks.  We also only have mouse input; that will change soon.

If you need something else, let me know.

How to use it:

Open the Desk directory in Unity.

In DS501, we each have a Run file and a test scene with our names on it.
If you open your scene, it should run that run file.
In the run file, it creates an instance of a minigame.

Make a new class that inherits from Minigame, and change the run file to use your new class.  Put new minigames in DS501/minigames.

An example minigame is in DS501/minigames/select_three. It does a few things of interest:
 - selects objects
 - drags objects
 - places and moves objects in a plane out in front of the desk

It probably shows most of what we need.  If you want more, read on.

A lot of that is done with the "helper" member, which is a MinigameHelper object (in the Minigame.cs file).  It lets you:

Access these input vars:
    Quaternion  rotation;
    Vector2     screenspace_position;
    Vector2     screenspace_velocity;
    bool        action       = false;    // did action start this frame? (Is mouse click now?)
    bool        action_held  = false;    // is the action active? (Is the button held down?)

set a flag when the player wins:
    bool        success = false;

access the last clicked-on object:
    GameObject selected = null;

register events to different listeners:
    // listeners
    onButton = () => { };
    onMove   = () => { };
    onRotate = () => { };
    onSelect = (GameObject obj) => { };
    onDrag   = (GameObject obj) => { };

( the "action" variables and "onButton" all refer to the "action button", which will vary by input method)

show the cursor (must call if you want one):
	helper.showCursor()

get a random object to display
	helper.getAnObject()

get a plane in front of the desk at the specified distance:
	helper.getGamePlane( float depth = 0.4f)