using System;
using UnityEngine;

namespace PandaEntertainer
{
    public class EmployeeCostumeContainer
    {
        public EmployeeCostume employeeCostume { get; private set;}

        public EmployeeCostumeContainer(string name,string costumeName, Color[] costumeColors)
        {
            employeeCostume =  ScriptableObject.CreateInstance<EmployeeCostume>();
            employeeCostume.name = name;
            employeeCostume.costumeName = costumeName;
            employeeCostume.customColors = costumeColors;

        }

        public void SetMalePartContainer(BodyPartsContainer container)
        {
            employeeCostume.bodyPartsMale = container;
        }

        public void SetFemalePartContainer(BodyPartsContainer container)
        {
            employeeCostume.bodyPartsFemale = container;
        }

    }
}

