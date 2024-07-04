using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using NativeUI;

namespace SmartClamp
{
    public static class Menu
    {
        private static MenuPool menuPool;
        private static UIMenu mainMenu;

        static Menu()
        {
            menuPool = new MenuPool()
            {
                MouseEdgeEnabled = false,
                ControlDisablingEnabled = false
            };
            mainMenu = new UIMenu("Sabot", "Actions")
            {
                MouseControlsEnabled = false
            };
            menuPool.Add(mainMenu);
            VehicleControlMenu(mainMenu);
            menuPool.RefreshIndex();
        }

        internal static async void Toggle()
        {
            if (menuPool.IsAnyMenuOpen())
            {
                menuPool.CloseAllMenus();
            }
            else
            {
                mainMenu.Visible = true;
                while (menuPool.IsAnyMenuOpen())
                {
                    menuPool.ProcessMenus();
                    await BaseScript.Delay(0);
                }
            }
        }

        private static void VehicleControlMenu(UIMenu menu)
        {
            var clampItem = new UIMenuItem("Poser Sabot", "Installer le Sabot");
            clampItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
            menu.AddItem(clampItem);

            var stickerItem = new UIMenuItem("Poser Sticker", "Poser un sticker de saisie");
            stickerItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
            menu.AddItem(stickerItem);

            var removeClamp = new UIMenuItem("Retirer", "Enlever le sabot ou le sticker");
            menu.AddItem(removeClamp);
            menu.OnItemSelect += (sender, item, index) =>
            {
                if (item == stickerItem)
                {
                    Main.StickerHandler();
                }
                else if (item == clampItem)
                {
                    Main.ClampHandler();
                }
                else if (item == removeClamp)
                {
                    Main.UpdateObject();
                }
                Toggle();
            };
        }
    }
}
