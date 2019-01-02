/**
* Copyright 2019 Michael Pollind
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using UnityEngine;

namespace PandaEntertainer
{
    public class EmployeeCostumeContainer
    {
        public EmployeeCostumeContainer(string name, string costumeName, Color[] costumeColors)
        {
            EmployeeCostume = ScriptableObject.CreateInstance<EmployeeCostume>();
            EmployeeCostume.name = name;
            EmployeeCostume.costumeName = costumeName;
            EmployeeCostume.customColors = costumeColors;
        }

        public EmployeeCostume EmployeeCostume { get; }

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