using UnityEngine;

namespace PandaEntertainer
{
    public class EmployeeCostumeContainer
    {
        public EmployeeCostume EmployeeCostume { get; }

        public EmployeeCostumeContainer(string name,string costumeName, Color[] costumeColors)
        {
            EmployeeCostume =  ScriptableObject.CreateInstance<EmployeeCostume>();
            EmployeeCostume.name = name;
            EmployeeCostume.costumeName = costumeName;
            EmployeeCostume.customColors = costumeColors;

        }

        public void SetMalePartContainer(BodyPartsContainer container)
        {
            EmployeeCostume.bodyPartsMale = container;
        }

        public void SetFemalePartContainer(BodyPartsContainer container)
        {
            EmployeeCostume.bodyPartsFemale = container;
        }

    }
}

