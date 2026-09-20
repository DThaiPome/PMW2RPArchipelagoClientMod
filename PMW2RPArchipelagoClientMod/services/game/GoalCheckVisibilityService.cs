using Archipelago.MultiClient.Net.Models;
using Il2Cpp;
using Il2CppUI;
using MelonLoader;
using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PMW2RPArchipelagoClientMod.services.game
{
    public class GoalCheckVisibilityService
    {
        private MelonMod _melonMod;
        private IUnlocksSource _unlocks;
        private IAPConnectionService _apConnectionService;

        private Dictionary<GoldenFruitItem, GameObject> _goldenFruitObjs = new Dictionary<GoldenFruitItem, GameObject>();

        private bool _shouldSync = false;

        public GoalCheckVisibilityService(MelonMod melonMod,
            IUnlocksSource unlocks,
            IAPConnectionService apConnectionService)
        {
            _melonMod = melonMod;
            _unlocks = unlocks;
            _apConnectionService = apConnectionService;

            _apConnectionService.ItemReceived += _ItemReceived;
            _apConnectionService.InitItems += _InitItems;
        }

        public void OnSceneWasInitialized()
        {
            _shouldSync = true;
        }

        public void OnLateUpdate()
        {
            if (_shouldSync)
            {
                _syncGoldenFruits();
                _shouldSync = false;
            }
            _syncGoldenFruitVisibility();
        }

        private void _ItemReceived(ItemInfo item)
        {
            _shouldSync = true;
        }

        public void _InitItems(IReadOnlyList<ItemInfo> items)
        {
            _shouldSync = true;
        }

        private void _syncGoldenFruits()
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "PacVillage")
            {
                return;
            }

            _goldenFruitObjs.Clear();

            for (GoldenFruitItem fruit = GoldenFruitItem.GoldenCherry; fruit < GoldenFruitItem.MAX; fruit++)
            {
                GameObject obj = _objFromFruit(fruit);
                if (obj == null)
                {
                    continue;
                }
                _goldenFruitObjs[fruit] = obj;
                obj.SetActive(_unlocks.GoldenFruit.Contains(fruit));
            }
        }

        private void _syncGoldenFruitVisibility()
        {
            foreach (var pair in  _goldenFruitObjs)
            {
                GameObject obj = pair.Value;
                if (obj == null)
                {
                    continue;
                }
                GoldenFruitItem fruit = pair.Key;
                obj.SetActive(_unlocks.GoldenFruit.Contains(fruit));
            }
        }

        private GameObject _objFromFruit(GoldenFruitItem fruit)
        {
            return GameObject.Find(fruit switch
            {
                GoldenFruitItem.GoldenCherry => "SM_Fruit_Cherries_Gold",
                GoldenFruitItem.GoldenStrawberry => "SM_Fruit_Berry_Gold",
                GoldenFruitItem.GoldenApple => "SM_Fruit_Apple_Gold",
                GoldenFruitItem.GoldenOrange => "SM_Fruit_Orange_Gold",
                GoldenFruitItem.GoldenMelon => "SM_Fruit_Melon_Gold"
            });
        }
    }
}
