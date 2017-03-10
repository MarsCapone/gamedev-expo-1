using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobbleArrow : MonoBehaviour {

    RectTransform rt;
    float initial, lower, upper;
    float val;
    public bool horizontalWobble = true;
    bool decreasing = false;
    float increment = 5f;

	// Use this for initialization
	void Start () {
        rt = GetComponent<RectTransform>();
        initial = horizontalWobble ? rt.position.x : rt.position.y;
        lower = initial - 25;
        upper = initial + 25;
        val = initial;
    }
	
	// Update is called once per frame
	void Update () {
        rt.position = horizontalWobble ? new Vector2(val, rt.position.y) : new Vector2(rt.position.x, val);
        val += (decreasing ? -1 : 1) * increment;
        if (decreasing && val < lower) decreasing = false;
        else if (!decreasing && val > upper) decreasing = true;
    }
}
