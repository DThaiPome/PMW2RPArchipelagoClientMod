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
        private Dictionary<PastKeyItem, GameObject> _keyObjs = new Dictionary<PastKeyItem, GameObject>();

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
                _syncPastMapObjRefs();
                _shouldSync = false;
            }
            _syncGoldenFruitVisibility();
            _syncFruitsVisibility();
            _syncKeysVisibility();
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

        private void _syncPastMapObjRefs()
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "StageSelect_Past")
            {
                return;
            }

            _cloneAndSyncKeyObjs();
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

        private void _cloneAndSyncKeyObjs()
        {
            foreach (GameObject obj in _keyObjs.Values)
            {
                if (obj != null)
                {
                    UnityEngine.Object.Destroy(obj);
                }
            }
            _keyObjs.Clear();

            GameObject areaLockObj = GameObject.Find("AreaLock_Area2_Past");
            if (areaLockObj == null)
            {
                return;
            }

            GameObject rootKeyObj = areaLockObj.transform.Find("RootPos/P1/Root_Key")?.gameObject ?? null;
            if (rootKeyObj == null)
            {
                return;
            }

            for (PastKeyItem key = PastKeyItem.WindyWoodsKey; key < PastKeyItem.MAX; key++)
            {
                Transform rootPos = _rootPosTransformFromKey(key);
                if (rootPos == null)
                {
                    continue;
                }

                var newKeyObj = UnityEngine.Object.Instantiate(rootKeyObj);
                GameObject keyModelObj = newKeyObj.transform.Find("SM_Fruit_Key")?.gameObject ?? null;
                if (keyModelObj == null)
                {
                    UnityEngine.Object.Destroy(newKeyObj);
                    continue;
                }

                keyModelObj.SetActive(true);
                newKeyObj.transform.position = rootPos.position;
                newKeyObj.transform.rotation = rootPos.rotation;
                _keyObjs[key] = newKeyObj;
            }
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

        private void _syncKeysVisibility()
        {
            _syncObjVisibilityToUnlocks(_keyObjs, _unlocks.PastKeys.Contains);
        }

        private void _syncObjVisibilityToGoldenFruitUnlocks(Dictionary<GoldenFruitItem, GameObject> objs)
        {
            _syncObjVisibilityToUnlocks(objs, _unlocks.GoldenFruit.Contains);
        }

        private void _syncObjVisibilityToUnlocks<T>(Dictionary<T, GameObject> objs, Func<T, bool> isUnlocked)
        {
            foreach (var pair in objs)
            {
                GameObject obj = pair.Value;
                if (obj == null)
                {
                    continue;
                }
                T item = pair.Key;
                obj.SetActive(isUnlocked(item));
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

        private Transform _rootPosTransformFromKey(PastKeyItem key)
        {
            GameObject areaLockObj = GameObject.Find(key switch
            {
                PastKeyItem.WindyWoodsKey => "AreaLock_Area2_Past",
                PastKeyItem.ThunderSnowMountainKey => "AreaLock_Area3_Past",
                PastKeyItem.FieryCavernsKey => "AreaLock_Area4_Past",
                PastKeyItem.DimUnderwatersKey => "AreaLock_Area5_Past",
                PastKeyItem.GhostIslandKey => "AreaLock_Area6_Past",
                PastKeyItem.MAX => throw new NotImplementedException(),
                _ => throw new NotImplementedException("weird key")
            });
            if (areaLockObj == null)
            {
                return null;
            }

            return areaLockObj.transform.Find(key == PastKeyItem.GhostIslandKey ? "RootPos (1)" : "RootPos");
        }
    }
}
