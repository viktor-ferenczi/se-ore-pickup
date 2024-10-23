using ClientPlugin.Settings.Elements;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Reflection;
using VRage.Utils;
using VRageMath;


namespace ClientPlugin.Settings
{
    internal enum Layout
    {
        None,
        Simple,
    }

    internal class Generator
    {
        private readonly List<Tuple<IBase, string, Func<object>, Action<object>>> Config;
        public readonly string Name;
        private string FriendlyName => Name.ToLower().Replace(' ', '_');
        private readonly Dictionary<Layout, List<MyGuiControlBase>> Storage = new Dictionary<Layout, List<MyGuiControlBase>>();
        private readonly Dictionary<Layout, Screen> Screens = new Dictionary<Layout, Screen>();
        public List<List<MyGuiControlBase>> Controls { get; private set; }
        public Layout ActiveLayout { get; private set; } = Layout.None;

        public Generator(Type config)
        {
            Config = ExtractGuiConfig(config);
            Name = ExtractGuiName(config);
        }

        public MyGuiScreenBase GetLayoutScreen()
        {
            if (!Screens.ContainsKey(ActiveLayout))
            {
                LayoutControls(ActiveLayout);
            }

            return Screens[ActiveLayout];
        }

        private static string ExtractGuiName(Type model)
        {
            object name = model.GetField("Title",
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Static).GetValue(null);

            return (string)name;
        }

        private static List<Tuple<IBase, string, Func<object>, Action<object>>> ExtractGuiConfig(Type model)
        {
            var config = new List<Tuple<IBase, string, Func<object>, Action<object>>>();

            foreach (var propertyInfo in model.GetProperties())
            {
                string name = propertyInfo.Name;

                object defaultInstance = model.GetField("Current",
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Static).GetValue(null);

                object getter() => propertyInfo.GetValue(defaultInstance);
                void setter(object value) => propertyInfo.SetValue(defaultInstance, value);

                foreach (var attribute in propertyInfo.GetCustomAttributes())
                {
                    if (attribute is IBase baseElement)
                    {
                        config.Add(Tuple.Create(baseElement, name, (Func<object>)getter, (Action<object>)setter));
                    }
                }
            }

            return config;
        }

        public void CreateControls()
        {
            Controls = new List<List<MyGuiControlBase>>();

            foreach (var item in Config)
            {
                Controls.Add(item.Item1.GetElements(item.Item2, item.Item3, item.Item4));
            }
        }

        private List<MyGuiControlBase> GetControls()
        {
            CreateControls();
            LayoutControls(ActiveLayout);

        }

        public void LayoutControls(Layout layout)
        {
            if (Controls.Count == 0)
            {
                CreateControls();
            }

            switch (layout)
            {
                case Layout.None:
                    LayoutNone();
                    break;
                case Layout.Simple:
                    LayoutSimple();
                    break;
                default:
                    throw new Exception("Undefined layout.");

            }

            ActiveLayout = layout;
        }

        private void LayoutNone()
        {
            Screen screen;
            if (!Screens.ContainsKey(Layout.None))
            {
                screen = new Screen(FriendlyName);
                Screens[Layout.None] = screen;
            }
            else
            {
                screen = Screens[Layout.None];
            }

            foreach (var group in Controls)
            {
                foreach (var control in group)
                {
                    if (!screen.Controls.Contains(control))
                    {
                        screen.Controls.Add(control);
                    }
                    control.Position = Vector2.Zero;
                }
            }

            screen.AddCaption(Name);
        }

        private void LayoutSimple()
        {
            Vector2 screenSize = new Vector2(0.9f, 0.9f);

            Screen screen;
            if (!Screens.ContainsKey(Layout.Simple))
            {
                screen = new Screen(FriendlyName, size: screenSize);
                Screens[Layout.Simple] = screen;
            }
            else
            {
                screen = Screens[Layout.Simple];
            }

            screen.AddCaption(Name);

            float ElementHeight = 0.03f;
            float ElementPadding = 0.015f;
            float GetIndexOffset(int index)
            {
                return index * (ElementHeight + ElementPadding);
            }

            MyGuiControlParent parent;
            MyGuiControlScrollablePanel scroll;
            if (!Storage.ContainsKey(Layout.Simple))
            {
                parent = new MyGuiControlParent()
                {
                    OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM,
                    Position = new Vector2(0f, 0.5f * screenSize.Y),
                    Size = new Vector2(0.8f, 0.8f),
                };

                scroll = new MyGuiControlScrollablePanel(parent)
                {
                    BackgroundTexture = null,
                    BorderHighlightEnabled = false,
                    BorderEnabled = false,
                    OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM,
                    Position = new Vector2(0f, 0.5f * screenSize.Y),
                    Size = new Vector2(0.8f, 0.8f),
                    ScrollbarVEnabled = true,
                    CanFocusChildren = true,
                    ScrolledAreaPadding = new MyGuiBorderThickness(0.005f),
                    DrawScrollBarSeparator = true,
                };

                Storage[Layout.Simple] = new List<MyGuiControlBase>() { parent, scroll };
            }
            else
            {
                parent = (MyGuiControlParent)Storage[Layout.Simple][0];
                scroll = (MyGuiControlScrollablePanel)Storage[Layout.Simple][1];
            }

            for (int i = 0; i < Controls.Count; i++)
            {
                var elements = Controls[i];
                float y_pos = -0.5f * scroll.Size.Y + GetIndexOffset(i);

                for (int j = 0; j < elements.Count; j++)
                {
                    var element = elements[j];

                    float x_pos = -0.5f * scroll.Size.X;
                    if (elements.Count > 1)
                    {
                        x_pos += scroll.ScrolledAreaSize.X / (elements.Count - 1) * j;
                    }

                    element.Position = new Vector2(x_pos, y_pos);

                    if (j == 0)
                    {
                        element.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
                    }
                    else if (j == elements.Count - 1)
                    {
                        element.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
                    }
                    else
                    {
                        element.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
                    }

                    if (!parent.Controls.Contains(element))
                    {
                        parent.Controls.Add(element);
                    }
                }
            }

            if (!screen.Controls.Contains(scroll))
            {
                screen.Controls.Add(scroll);
            }
        }
    }
}
