using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] ParticleSystem SideDustParticle;
    [SerializeField] ParticleSystem BottomDustParticle;
    [SerializeField] Transform LeftDustSpot;
    [SerializeField] Transform BottomDustSpot;
    [SerializeField] Transform RightDustSpot;

    SpriteRenderer playerSprite;

    void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
    }

    public void EmitDust()
    {
        if (playerSprite.flipX)
        {
            Instantiate(SideDustParticle, RightDustSpot.position, Quaternion.Euler(0, 90f,0));
        }
        else
        {
            Instantiate(SideDustParticle, LeftDustSpot.position, Quaternion.Euler(0, -90f, 0));
        }
    }

    public void EmitDustOnBothSides()
    {
        Instantiate(SideDustParticle, RightDustSpot.position+new Vector3(0.2f,0), Quaternion.Euler(0, 90f, 0));

        Instantiate(SideDustParticle, LeftDustSpot.position + new Vector3(-0.2f, 0), Quaternion.Euler(0, -90f, 0));
    }

    public void EmitBottomDust()
    {
        Instantiate(BottomDustParticle, BottomDustSpot.position, Quaternion.Euler(-90, 0, 0));
    }

    public void EmitBottomRightParticle()
    {
        Instantiate(SideDustParticle, RightDustSpot.position + new Vector3(0,-0.2f), Quaternion.Euler(90f, -90f, 0));
    }

    public void EmitBottomLeftParticle()
    {
        Instantiate(SideDustParticle, LeftDustSpot.position + new Vector3(0, -0.2f), Quaternion.Euler(90f, -90f, 0));
    }
}
