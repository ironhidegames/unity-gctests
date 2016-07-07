// Attach this to a GUIText to make a frames/second indicator.
//
// It calculates frames/second over each updateInterval,
// so the display does not keep changing wildly.
//
// It is also fairly accurate at very low FPS counts (<10).
// We do this not by simply counting frames per interval, but
// by accumulating FPS for each frame. This way we end up with
// correct overall FPS even if the interval renders something like
// 5.5 frames

var updateInterval = 0.5;
 
private var accum = 0.0; // FPS accumulated over the interval
private var frames = 0; // Frames drawn over the interval
private var timeleft : float; // Left time for current interval

private var timeSinceStart : float;

function Start()
{
    if( !GetComponent.<UnityEngine.UI.Text>() )
    {
        print ("FramesPerSecond needs a UnityEngine.UI.Text component!");
        enabled = false;
        return;
    }
    timeleft = updateInterval;  
    timeSinceStart = Time.realtimeSinceStartup;
}
 
function Update()
{
	var newDeltaTime = Time.realtimeSinceStartup - timeSinceStart;

	timeSinceStart = Time.realtimeSinceStartup;

	if (newDeltaTime > 0) {
    	timeleft -= newDeltaTime;
	    accum += Time.timeScale / newDeltaTime;
	    ++frames;
    }
 
    // Interval ended - update GUI text and start new interval
    if( timeleft <= 0.0 )
    {
        // display two fractional digits (f2 format)
        GetComponent.<UnityEngine.UI.Text>().text = "" + (accum/frames).ToString("f2");
        timeleft = updateInterval;
        accum = 0.0;
        frames = 0;
    }
}