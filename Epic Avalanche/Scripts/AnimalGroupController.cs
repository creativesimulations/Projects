using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

namespace Animals
{

    public class AnimalGroupController : MonoBehaviour
{
    [SerializeField] private float animateRate = .05f;

    [SerializeField] List<List<GameObject>> animalGroups = new List<List<GameObject>>();
    [SerializeField] private List<GameObject> cows;
    [SerializeField] private List<GameObject> horses;
    [SerializeField] private List<GameObject> pigs;
    [SerializeField] private List<GameObject> chickens;
    [SerializeField] private List<GameObject> geese;
    [SerializeField] private List<GameObject> dogs;

    Animator Anim;
    private GameObject animalChosen;
    private AnimalAI chosenAnimalAI;
    private NavMeshAgent animalAgent;

    public delegate void OnStateChange(string state);
    public OnStateChange onStateChange;

        void Start()
        {
            AddAnimalGroupsToList();
            if(animalGroups.Count > 0)
            {
                InvokeRepeating("ChoseAnimalToAnimate", animateRate, animateRate);
            }
        }

        private void ChoseAnimalToAnimate()
        {
            RandomizeAnimal();
            chosenAnimalAI = animalChosen.GetComponent<AnimalAI>();
            if (!chosenAnimalAI.isMoving && !chosenAnimalAI.isDead)
            {
                AnimateRandomAnimal();
            }
        }

        private void RandomizeAnimal()
        {
            int groupIndex = UnityEngine.Random.Range(0, animalGroups.Count);
            int animalIndex = UnityEngine.Random.Range(0, animalGroups[groupIndex].Count);
            animalChosen = animalGroups[groupIndex][animalIndex];
        }

        private void AnimateRandomAnimal()
        {
            int newStateInt = UnityEngine.Random.Range(2, 4);
            chosenAnimalAI.GroupControllerSetState(newStateInt);
        }

        private void AddAnimalGroupsToList()
        {
            if (cows.Count > 0)
            {
                animalGroups.Add(cows);
            }
            if (horses.Count > 0)
            {
                animalGroups.Add(horses);
            }
            if (pigs.Count > 0)
            {
                animalGroups.Add(pigs);
            }
            if (chickens.Count > 0)
            {
                animalGroups.Add(chickens);
            }
            if (geese.Count > 0)
            {
                animalGroups.Add(geese);
            }
            if (dogs.Count > 0)
            {
                animalGroups.Add(dogs);
            }
        }






}

}
