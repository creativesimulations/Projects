using UnityEngine;
using System.Collections;
using System;

public class ParticlePool
{
    int particleAmount;
    ParticleSystem[] ConsumeParticle;
    ParticleSystem[] TNTParticle;
    ParticleSystem[] TeleParticle;
    ParticleSystem[] GemParticle;
    ParticleSystem[] GlassParticle;
    ParticleSystem[] DestroyemyParticle;

    public ParticlePool( ParticleSystem ConsumePartPrefab, ParticleSystem TntPartPrefab, ParticleSystem TelePartPrefab, ParticleSystem GemPartPrefab, ParticleSystem GlassPartPrefab, ParticleSystem DestroyemyPartPrefab, int amount = 10)
    {
        particleAmount = amount;
        ConsumeParticle = new ParticleSystem[particleAmount];
        TNTParticle = new ParticleSystem[particleAmount];
        TeleParticle = new ParticleSystem[particleAmount];
        GemParticle = new ParticleSystem[particleAmount];
        GlassParticle = new ParticleSystem[particleAmount];
        DestroyemyParticle = new ParticleSystem[particleAmount];

        for (int i = 0; i < particleAmount; i++)
        {

            //Instantiate 10 Particles of each type
            ConsumeParticle[i] = GameObject.Instantiate(ConsumePartPrefab, new Vector3(0, 0, 0), new Quaternion()) as ParticleSystem;
            TNTParticle[i] = GameObject.Instantiate(TntPartPrefab, new Vector3(0, 0, 0), new Quaternion()) as ParticleSystem;
            TeleParticle[i] = GameObject.Instantiate(TelePartPrefab, new Vector3(0, 0, 0), new Quaternion()) as ParticleSystem;
            GemParticle[i] = GameObject.Instantiate(GemPartPrefab, new Vector3(0, 0, 0), new Quaternion()) as ParticleSystem;
            GlassParticle[i] = GameObject.Instantiate(GlassPartPrefab, new Vector3(0, 0, 0), new Quaternion()) as ParticleSystem;
            DestroyemyParticle[i] = GameObject.Instantiate(DestroyemyPartPrefab, new Vector3(0, 0, 0), new Quaternion()) as ParticleSystem;
        }
    }

    //Returns available GameObject
    public ParticleSystem getAvailabeParticle(int particleType)
    {
        ParticleSystem firstObject = null;

        //Destroy
        if (particleType == 0)
        {
            //Get the first GameObject
            firstObject = ConsumeParticle[0];
            //Move everything Up by one
            shiftUp(0);
        }

        //TNT
        else if (particleType == 1)
        {
            firstObject = TNTParticle[0];
            shiftUp(1);
        }

        //Tele
        else if (particleType == 2)
        {
            firstObject = TeleParticle[0];
            shiftUp(2);
        }

        //Gem
        else if (particleType == 3)
        {
            firstObject = GemParticle[0];
            shiftUp(3);
        }

        //Glass
        else if (particleType == 4)
        {
            firstObject = GlassParticle[0];
            shiftUp(4);
        }

        //Glass
        else if (particleType == 5)
        {
            firstObject = DestroyemyParticle[0];
            shiftUp(5);
        }


        return firstObject;
    }

    //Returns How much GameObject in the Array
    public int getAmount()
    {
        return particleAmount;
    }

    //Moves the GameObject Up by 1 and moves the first one to the last one
    private void shiftUp(int particleType)
    {
        //Get first GameObject
        ParticleSystem firstObject;

        //Consume
        if (particleType == 0)
        {
            firstObject = ConsumeParticle[0];
            //Shift the GameObjects Up by 1
            Array.Copy(ConsumeParticle, 1, ConsumeParticle, 0, ConsumeParticle.Length - 1);

            //(First one is left out)Now Put first GameObject to the Last one
            ConsumeParticle[ConsumeParticle.Length - 1] = firstObject;
        }

        //TNT
        else if (particleType == 1)
        {
            firstObject = TNTParticle[0];
            Array.Copy(TNTParticle, 1, TNTParticle, 0, TNTParticle.Length - 1);
            TNTParticle[TNTParticle.Length - 1] = firstObject;
        }

        //Tele
        else if (particleType == 2)
        {
            firstObject = TeleParticle[0];
            Array.Copy(TeleParticle, 1, TeleParticle, 0, TeleParticle.Length - 1);
            TeleParticle[TeleParticle.Length - 1] = firstObject;
        }

        //Gem
        else if (particleType == 3)
        {
            firstObject = GemParticle[0];
            Array.Copy(GemParticle, 1, GemParticle, 0, GemParticle.Length - 1);
            GemParticle[GemParticle.Length - 1] = firstObject;
        }

        //Glass
        else if (particleType == 4)
        {
            firstObject = GlassParticle[0];
            Array.Copy(GlassParticle, 1, GlassParticle, 0, GlassParticle.Length - 1);
            GlassParticle[GlassParticle.Length - 1] = firstObject;
        }

        //Destroyemy
        else if (particleType == 5)
        {
            firstObject = DestroyemyParticle[0];
            Array.Copy(DestroyemyParticle, 1, DestroyemyParticle, 0, DestroyemyParticle.Length - 1);
            DestroyemyParticle[DestroyemyParticle.Length - 1] = firstObject;
        }
    }
}