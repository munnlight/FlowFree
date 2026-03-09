using System;

[Serializable]
public class DotData
{
    public int x;
    public int y;
    public string color;
}

[Serializable]
public class LevelData
{
    public int dotpair;
    public DotData[] dots;
}