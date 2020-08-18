using UnityEngine;
using System.Collections;
public class Guns : MonoBehaviour
{
}
public interface IGun
{
    float FireRate
    {
        get;
    }

    int MaxAmmo
    {
        get;
    }

    int CurrentAmmo
    {
        get;
        set;
    }

    Vector2 RecoilForceForward
    {
        get;
    }

    Vector2 RecoilForceDownward
    {
        get;
    }

    Vector2 RecoilForceUpward
    {
        get;
    }
}
public class Pistol : MonoBehaviour, IGun
{
    public float FireRate { get; } = 10f;

    public int MaxAmmo { get; } = 2;

    public int CurrentAmmo { get; set; } = 2;

    public Vector2 RecoilForceForward { get; } = new Vector2(50, 0);

    public Vector2 RecoilForceDownward { get; } = new Vector2(0, 70);

    public Vector2 RecoilForceUpward { get; } = new Vector2(0, 100);
}

public class Deagle : MonoBehaviour, IGun
{
    public float FireRate { get; } = 1f;

    public int MaxAmmo { get; } = 1;

    public int CurrentAmmo { get; set; } = 1;

    public Vector2 RecoilForceForward { get; } = new Vector2(100, 0);

    public Vector2 RecoilForceDownward { get; } = new Vector2(0, 140);

    public Vector2 RecoilForceUpward { get; } = new Vector2(0, 200);
}
