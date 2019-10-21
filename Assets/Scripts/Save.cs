using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public  int _hunger;
    public int _cleanliness;
    public int _happiness;
    public long _hungerTime;
    public long _cleanlinessTime;
    public long _happinessTime;
    public long _timeStarted;
    public int _color;

    public Save(int hunger, int cleanliness, int happiness, long hungerTime, long cleanlinessTime, long happinessTime, long timeStarted, int color)
    {
        _hunger = hunger;
        _cleanliness = cleanliness;
        _happiness = happiness;

        _hungerTime = hungerTime;
        _cleanlinessTime = cleanlinessTime;
        _happinessTime = happinessTime;

        _timeStarted = timeStarted;
        _color = color;
    }
}
