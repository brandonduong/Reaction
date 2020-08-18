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

    int BulletDamage
    {
        get;
    }

    float BulletSpeed
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

    RecoilType RecoilType
    {
        get;
    }

    float RecoilScreenShake
    {
        get;
    }

    float FireCounter // Guns must have individual cooldowns
    {
        get;
        set;
    }

    string ShotSound
    {
        get;
    }
}

public class Pistol : MonoBehaviour, IGun
{
    public float FireRate { get; } = 5f;

    public int MaxAmmo { get; } = 2;

    public int BulletDamage { get; } = 50;

    public float BulletSpeed { get; } = 30f;

    public int CurrentAmmo { get; set; } = 2;

    public Vector2 RecoilForceForward { get; } = new Vector2(50, 0); // Recoil when shooting forward

    public Vector2 RecoilForceDownward { get; } = new Vector2(0, 70); // Recoil when shooting downward

    public Vector2 RecoilForceUpward { get; } = new Vector2(0, 100); // Recoil when shooting upward

    public RecoilType RecoilType { get; } = RecoilType.Gradual;

    public float RecoilScreenShake { get; } = 0.5f;

    public float FireCounter { get; set; } = 0f;

    public string ShotSound { get; } = "PistolGunShot"; // Name of the audio file to play when shot
}

public class Deagle : MonoBehaviour, IGun
{
    public float FireRate { get; } = 1f;

    public int MaxAmmo { get; } = 1;

    public int BulletDamage { get; } = 100;

    public float BulletSpeed { get; } = 60f;

    public int CurrentAmmo { get; set; } = 1;

    public Vector2 RecoilForceForward { get; } = new Vector2(100, 0); // new Vector2(5000, 0);

    public Vector2 RecoilForceDownward { get; } = new Vector2(0, 140); // new Vector2(0, 1400);

    public Vector2 RecoilForceUpward { get; } = new Vector2(0, 200); // new Vector2(0, 3000);

    public RecoilType RecoilType { get; } = RecoilType.Gradual; // RecoilType.Static;

    public float RecoilScreenShake { get; } = 1.5f;

    public float FireCounter { get; set; } = 0f;

    public string ShotSound { get; } = "DeagleGunShot"; // Name of the audio file to play when shot
}