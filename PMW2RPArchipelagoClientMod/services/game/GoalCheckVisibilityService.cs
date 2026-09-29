using Archipelago.MultiClient.Net.Models;
using MelonLoader;
using PMW2RPArchipelagoClientMod.models.data;
using PMW2RPArchipelagoClientMod.services.client;
using IUnlocksSource = PMW2RPArchipelagoClientMod.services.items.v2.item.IUnlocksSource;
using UnityEngine;
using PMW2RPArchipelagoClientMod.services.items.v2.item.items;
using Il2Cpp;

namespace PMW2RPArchipelagoClientMod.services.game
{
    public class GoalCheckVisibilityService
    {
        private MelonMod _melonMod;
        private IUnlocksSource _unlocks;
        private IAPConnectionService _apConnectionService;

        private Dictionary<EFruits, GameObject> _goldenFruitObjs = new Dictionary<EFruits, GameObject>();
        private Dictionary<EFruits, GameObject> _fruitParentObjs = new Dictionary<EFruits, GameObject>();
        private Dictionary<PastKeyKind, GameObject> _keyObjs = new Dictionary<PastKeyKind, GameObject>();

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

            for (PastKeyKind key = PastKeyKind.WindyWoodsKey; key < PastKeyKind.MAX; key++)
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

        private void _syncObjRefsFromGoldenFruit(Dictionary<EFruits, GameObject> objs, Func<EFruits, GameObject> fruitToObj)
        {

            for (EFruits fruit = EFruits.Cherry; fruit < EFruits.MAX; fruit++)
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
            _syncObjVisibilityToUnlocks(_keyObjs, key => PastKeyItem.IsPastKeyReceived(_unlocks, key));
        }

        private void _syncObjVisibilityToGoldenFruitUnlocks(Dictionary<EFruits, GameObject> objs)
        {
            _syncObjVisibilityToUnlocks(objs, fruit => GoldenFruitItem.IsGoldenFruitReceived(_unlocks, fruit));
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

        private GameObject _objFromFruit(EFruits fruit)
        {
            return GameObject.Find(fruit switch
            {
                EFruits.Cherry => "SM_Fruit_Cherries_Gold",
                EFruits.Strawberry => "SM_Fruit_Berry_Gold",
                EFruits.Apple => "SM_Fruit_Apple_Gold",
                EFruits.Orange => "SM_Fruit_Orange_Gold",
                EFruits.Melon => "SM_Fruit_Melon_Gold",
                _ => throw new NotImplementedException("weird golden fruit")
            });
        }

        private GameObject _parentObjFromFruit(EFruits fruit)
        {
            return GameObject.Find(fruit switch
            {
                EFruits.Cherry => "Fruits_Cherries",
                EFruits.Strawberry => "Fruits_Strawberry",
                EFruits.Apple => "Fruits_Apple",
                EFruits.Orange => "Fruits_Orange",
                EFruits.Melon => "Fruits_Melon",
                _ => throw new NotImplementedException("weird golden fruit")
            });
        }

        private Transform _rootPosTransformFromKey(PastKeyKind key)
        {
            GameObject areaLockObj = GameObject.Find(key switch
            {
                PastKeyKind.WindyWoodsKey => "AreaLock_Area2_Past",
                PastKeyKind.ThunderSnowMountainKey => "AreaLock_Area3_Past",
                PastKeyKind.FieryCavernsKey => "AreaLock_Area4_Past",
                PastKeyKind.DimUnderwatersKey => "AreaLock_Area5_Past",
                PastKeyKind.GhostIslandKey => "AreaLock_Area6_Past",
                PastKeyKind.MAX => throw new NotImplementedException(),
                _ => throw new NotImplementedException("weird key")
            });
            if (areaLockObj == null)
            {
                return null;
            }

            return areaLockObj.transform.Find(key == PastKeyKind.GhostIslandKey ? "RootPos (1)" : "RootPos");
        }
    }
}
