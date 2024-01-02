using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Scriptables;

namespace Contagion
{
    public class ContagionCanvasController : MonoBehaviour
    {
        public ContagionController contagionController;
        [SerializeField] ContagionTextScriptableObject _contagionText;
        [SerializeField] private TextMeshProUGUI daysTMP;
        [SerializeField] private TextMeshProUGUI infectedTMP;
        [SerializeField] private TextMeshProUGUI immuneTMP;
        [SerializeField] private TextMeshProUGUI deadTMP;
        [SerializeField] private TextMeshProUGUI _contagionName;
        [SerializeField] private TextMeshProUGUI _populationSample;
        [SerializeField] private TextMeshProUGUI _contagionRate;
        [SerializeField] private TextMeshProUGUI _lethality;
        [SerializeField] private TextMeshProUGUI _incubationPeriod;
        [SerializeField] private TextMeshProUGUI _infectionPeriod;
        private int daysPassed = 0;
        private int illNum = 0;
        private int immuneNum = 0;
        private int deadNum = 0;

        void Start()
        {
            ContagionController.onCount += CountDays;
            ContagionController.onUpdateIll += UpdateInfected;
            ContagionController.onUpdateImmune += UpdateImmune;
            ContagionController.onUpdateDead += UpdateDead;
            SetContagionTexts();
        }

        private void CountDays()
        {
            InvokeRepeating("Counting", 1, 1f);
        }

        private void Counting()
        {
            daysPassed += 1;
            daysTMP.text = daysPassed.ToString();
        }

        private void UpdateInfected()
        {
            illNum += 1;
            infectedTMP.text = illNum.ToString();
        }
        private void UpdateImmune()
        {
            immuneNum += 1;
            immuneTMP.text = immuneNum.ToString();
        }
        private void UpdateDead()
        {
            deadNum += 1;
            deadTMP.text = deadNum.ToString();
        }

        private void SetContagionTexts()
        {
            _contagionName.text = "Contagion: " + _contagionText._contagionName;
            _populationSample.text = "Population Sample: " + _contagionText._populationSample;
            _contagionRate.text = "Contagion Rate: " + _contagionText._contagionRate + "%";
            _lethality.text = "Lethality: " + _contagionText._lethality + "%";
            _incubationPeriod.text = "Incubation Period: " + _contagionText._incubationPeriod;
            _infectionPeriod.text = "Infection Period: " + _contagionText._infectionPeriod;
        }
    }
}
