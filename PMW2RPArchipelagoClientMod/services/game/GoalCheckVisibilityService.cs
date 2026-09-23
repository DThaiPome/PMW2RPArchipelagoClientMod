using Archipelago.MultiClient.Net.Models;
using MelonLoader;
using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.client;
using UnityEngine;

namespace PMW2RPArchipelagoClientMod.services.game
{
    public class GoalCheckVisibilityService
    {
        private MelonMod _melonMod;
        private IUnlocksSource _unlocks;
        private IAPConnectionService _apConnectionService;

        private Dictionary<GoldenFruitItem, GameObject> _goldenFruitObjs = new Dictionary<GoldenFruitItem, GameObject>();
        private Dictionary<GoldenFruitItem, GameObject> _fruitParentObjs = new Dictionary<GoldenFruitItem, GameObject>();

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
                _syncPacVillageObjRefs();
                _shouldSync = false;
            }
            _syncGoldenFruitVisibility();
            _syncFruitsVisibility();
        }

        private void _ItemReceived(ItemInfo item)
        {
            _shouldSync = true;
        }

        public void _InitItems(IReadOnlyList<ItemInfo> items)
        {
            _shouldSync = true;
        }

        private void _syncPacVillageObjRefs()
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "PacVillage")
            {
                return;
            }

            _syncGoldenFruitObjRefs();
            _syncFruitParentObjRefs();
        }

        private void _syncGoldenFruitObjRefs()
        {
            _goldenFruitObjs.Clear();
            _syncObjRefsFromGoldenFruit(_goldenFruitObjs, _objFromFruit);
        }

        private void _syncFruitParentObjRefs()
        {
            _fruitParentObjs.Clear();
            _syncObjRefsFromGoldenFruit(_fruitParentObjs, _parentObjFromFruit);
        }

        private void _syncObjRefsFromGoldenFruit(Dictionary<GoldenFruitItem, GameObject> objs, Func<GoldenFruitItem, GameObject> fruitToObj)
        {

            for (GoldenFruitItem fruit = GoldenFruitItem.GoldenCherry; fruit < GoldenFruitItem.MAX; fruit++)
            {
                GameObject obj = fruitToObj(fruit);
                if (obj == null)
                {
                    continue;
                }
                objs[fruit] = obj;
            }
        }

        private void _syncGoldenFruitVisibility()
        {
            _syncObjVisibilityToGoldenFruitUnlocks(_goldenFruitObjs);
        }

        private void _syncFruitsVisibility()
        {
            _syncObjVisibilityToGoldenFruitUnlocks(_fruitParentObjs);
        }

        private void _syncObjVisibilityToGoldenFruitUnlocks(Dictionary<GoldenFruitItem, GameObject> objs)
        {
            foreach (var pair in objs)
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
                GoldenFruitItem.GoldenMelon => "SM_Fruit_Melon_Gold",
                _ => throw new NotImplementedException("weird golden fruit")
            });
        }

        private GameObject _parentObjFromFruit(GoldenFruitItem fruit)
        {
            return GameObject.Find(fruit switch
            {
                GoldenFruitItem.GoldenCherry => "Fruits_Cherries",
                GoldenFruitItem.GoldenStrawberry => "Fruits_Strawberry",
                GoldenFruitItem.GoldenApple => "Fruits_Apple",
                GoldenFruitItem.GoldenOrange => "Fruits_Orange",
                GoldenFruitItem.GoldenMelon => "Fruits_Melon",
                _ => throw new NotImplementedException("weird golden fruit")
            });
        }
    }
}
