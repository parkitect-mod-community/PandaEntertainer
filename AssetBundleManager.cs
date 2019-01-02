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
    public class AssetBundleManager
    {
        public readonly GameObject Body;
        public readonly GameObject Head;

        public AssetBundleManager(Main main)
        {
            var dsc = System.IO.Path.DirectorySeparatorChar;
            var assetBundle = AssetBundle.LoadFromFile(main.Path + dsc + "assetbundle" + dsc + "assetpack");

            Head = assetBundle.LoadAsset<GameObject>("5bc917ea4eecb4beea26792b912a314b");
            Body = assetBundle.LoadAsset<GameObject>("81b749044290b496eba80cd27935f395");
            assetBundle.Unload(false);
        }
    }
}