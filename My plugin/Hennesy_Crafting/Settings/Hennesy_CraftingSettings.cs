using ExileCore.Shared.Attributes;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using ImGuiNET;

namespace Hennesy_Crafting.Settings
{
    public class Hennesy_CraftingSettings : ISettings
    {
        //Mandatory setting to allow enabling/disabling your plugin
        public ToggleNode Enable { get; set; } = new ToggleNode(false);

        [Menu("Configuración", 0)]
        public ToggleNode Configuration { get; set; }



        [Menu("Ver HUD", 1, 0)]
        public ToggleNode Ver_HUD { get; set; } = new ToggleNode(false);

        [Menu("Ver Items Mods", 2, 0)]
        public ToggleNode Ver_Items_Mods { get; set; } = new ToggleNode(true);

        [Menu("Debug Mode", 3, 0)]
        public ToggleNode debugMode { get; set; } = new ToggleNode(false);

        //Item Mods
        public ItemLevelSettings ItemLevel { get; set; } = new ItemLevelSettings();
        public ItemModsSettings ItemMods { get; set; } = new ItemModsSettings();
        public WeaponDpsSettings WeaponDps { get; set; } = new WeaponDpsSettings();

        

        //Put all your settings here if you can.
        //There's a bunch of ready-made setting nodes,
        //nested menu support and even custom callbacks are supported.
        //If you want to override DrawSettings instead, you better have a very good reason.
    }
}