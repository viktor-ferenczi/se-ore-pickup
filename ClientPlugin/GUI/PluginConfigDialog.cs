using System;
using System.Text;
using Sandbox;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Utils;
using VRageMath;

namespace ClientPlugin.GUI
{
    public class PluginConfigDialog : MyGuiScreenBase
    {
        private const string Caption = "Ore Pickup Configuration";
        public override string GetFriendlyName() => "OrePickupDialog";

        private MyLayoutTable layoutTable;

        private MyGuiControlLabel enabledLabel;
        private MyGuiControlCheckbox enabledCheckbox;

        private MyGuiControlLabel collectIceLabel;
        private MyGuiControlCheckbox collectIceCheckbox;

        private MyGuiControlLabel collectStoneLabel;
        private MyGuiControlCheckbox collectStoneCheckbox;

        private MyGuiControlMultilineText infoText;
        private MyGuiControlButton closeButton;

        public PluginConfigDialog() : base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.3f, 0.42f), false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
        {
            EnabledBackgroundFade = true;
            m_closeOnEsc = true;
            m_drawEvenWithoutFocus = true;
            CanHideOthers = true;
            CanBeHidden = true;
            CloseButtonEnabled = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            RecreateControls(true);
        }

        public override void RecreateControls(bool constructor)
        {
            base.RecreateControls(constructor);

            CreateControls();
            LayoutControls();
        }

        private void CreateControls()
        {
            AddCaption(Caption);

            CreateCheckbox(out enabledLabel, out enabledCheckbox, OrePickup.Config.Enabled, value => OrePickup.Config.Enabled = value, "Enabled", "Enable the plugin");
            CreateCheckbox(out collectIceLabel, out collectIceCheckbox, OrePickup.Config.CollectIce, value => OrePickup.Config.CollectIce = value, "Collect ice");
            CreateCheckbox(out collectStoneLabel, out collectStoneCheckbox, OrePickup.Config.CollectStone, value => OrePickup.Config.CollectStone = value, "Collect stone");

            UpdateControls();

            enabledCheckbox.IsCheckedChanged += UpdateControls;

            infoText = new MyGuiControlMultilineText
            {
                Name = "InfoText",
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER,
                TextAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER,
                TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER,
                Text = new StringBuilder("These settings are saved\nand used in all worlds.")
            };

            closeButton = new MyGuiControlButton(originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER, text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: OnOk);
        }

        private void OnOk(MyGuiControlButton _)
        {
            OrePickup.Config.Save();
            CloseScreen();
        }

        private void CreateCheckbox(out MyGuiControlLabel labelControl, out MyGuiControlCheckbox checkboxControl, bool value, Action<bool> store, string label, string tooltip = null, bool enabled = true)
        {
            labelControl = new MyGuiControlLabel
            {
                Text = label,
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
                Enabled = enabled,
            };

            checkboxControl = new MyGuiControlCheckbox(toolTip: tooltip)
            {
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
                IsChecked = value,
                Enabled = enabled,
                CanHaveFocus = enabled
            };
            if (enabled)
            {
                checkboxControl.IsCheckedChanged += cb => store(cb.IsChecked);
            }
            else
            {
                checkboxControl.IsCheckedChanged += cb => { cb.IsChecked = value; };
            }
        }

        private void UpdateControls(MyGuiControlCheckbox _ = null)
        {
            var enabled = enabledCheckbox.IsChecked;
            collectIceCheckbox.Enabled = enabled;
            collectStoneCheckbox.Enabled = enabled;
        }

        private void LayoutControls()
        {
            layoutTable = new MyLayoutTable(this, new Vector2(-0.09f, -0.12f), new Vector2(0.24f, 0.3f));
            layoutTable.SetColumnWidths(120f, 180f);
            layoutTable.SetRowHeights(80f, 60f, 60f, 90f, 90f);

            layoutTable.Add(enabledCheckbox, MyAlignH.Right, MyAlignV.Center, 0, 0);
            layoutTable.Add(enabledLabel, MyAlignH.Left, MyAlignV.Center, 0, 1);

            layoutTable.Add(collectIceCheckbox, MyAlignH.Right, MyAlignV.Center, 1, 0);
            layoutTable.Add(collectIceLabel, MyAlignH.Left, MyAlignV.Center, 1, 1);
            
            layoutTable.Add(collectStoneCheckbox, MyAlignH.Right, MyAlignV.Center, 2, 0);
            layoutTable.Add(collectStoneLabel, MyAlignH.Left, MyAlignV.Center, 2, 1);

            layoutTable.Add(infoText, MyAlignH.Center, MyAlignV.Center, 3, 0, colSpan: 2);
            layoutTable.Add(closeButton, MyAlignH.Center, MyAlignV.Center, 4, 0, colSpan: 2);
        }
    }
}