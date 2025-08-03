using System.Collections.Generic;
using Core.Entity;
using Core.Upgrade;
using UnityEngine;

namespace Core.UI
{
    public class LootScreenUI : MonoBehaviour
    {
        [SerializeField] private GameObject lootScreenGo;

        [SerializeField] private LootElementUI[] lootElements;

        private bool m_startUpgrade = true;

        private void Start()
        {
            HideLootScreen();
        }

        public void ShowLootScreen(List<EUpgrades> upgrades)
        {
            GameManager.instance.uiManager.TextArea.IsShowingActionsOrLoot = true;

            lootScreenGo.SetActive(true);

            for (int i = 0; i < lootElements.Length; i++)
            {
                LootElementUI lootElement = lootElements[i];
                lootElement.UpdateInfos(upgrades[i]);
            }
        }

        public void OnUpgradeSelected(EUpgrades upgrade)
        {
            HideLootScreen();
            UpgradeData data = DataManager.GetData<UpgradeData>(upgrade.ToString());

            Player player = GameManager.instance.fightManager.Player;

            foreach (var bonus in data.bonusList)
            {
                if (bonus.ModifyStat())
                {
                    player.UpdateStat(bonus.targetedStat, bonus.value);
                }

                if (bonus.UnlockAbility()) player.UnlockAbility(bonus.ability);
            }

            if (m_startUpgrade)
            {
                GameManager.instance.fightManager.StartFirstFight(GameManager.instance.director.GetNextEnemy());
                m_startUpgrade = false;
            }
        }

        public void HideLootScreen()
        {
            lootScreenGo.SetActive(false);
            GameManager.instance.uiManager.TextArea.IsShowingActionsOrLoot = false;
        }
    }
}