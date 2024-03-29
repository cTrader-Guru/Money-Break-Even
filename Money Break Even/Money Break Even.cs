﻿/*  CTRADER GURU --> Indicator Template 1.0.6

    Homepage    : https://ctrader.guru/
    Telegram    : https://t.me/ctraderguru
    Twitter     : https://twitter.com/cTraderGURU/
    Facebook    : https://www.facebook.com/ctrader.guru/
    YouTube     : https://www.youtube.com/channel/UCKkgbw09Fifj65W5t5lHeCQ
    GitHub      : https://github.com/ctrader-guru

*/

using System;
using cAlgo.API;

namespace cAlgo
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class MoneyBreakEven : Robot
    {

        #region Enums

        public enum MyColors
        {

            AliceBlue,
            AntiqueWhite,
            Aqua,
            Aquamarine,
            Azure,
            Beige,
            Bisque,
            Black,
            BlanchedAlmond,
            Blue,
            BlueViolet,
            Brown,
            BurlyWood,
            CadetBlue,
            Chartreuse,
            Chocolate,
            Coral,
            CornflowerBlue,
            Cornsilk,
            Crimson,
            Cyan,
            DarkBlue,
            DarkCyan,
            DarkGoldenrod,
            DarkGray,
            DarkGreen,
            DarkKhaki,
            DarkMagenta,
            DarkOliveGreen,
            DarkOrange,
            DarkOrchid,
            DarkRed,
            DarkSalmon,
            DarkSeaGreen,
            DarkSlateBlue,
            DarkSlateGray,
            DarkTurquoise,
            DarkViolet,
            DeepPink,
            DeepSkyBlue,
            DimGray,
            DodgerBlue,
            Firebrick,
            FloralWhite,
            ForestGreen,
            Fuchsia,
            Gainsboro,
            GhostWhite,
            Gold,
            Goldenrod,
            Gray,
            Green,
            GreenYellow,
            Honeydew,
            HotPink,
            IndianRed,
            Indigo,
            Ivory,
            Khaki,
            Lavender,
            LavenderBlush,
            LawnGreen,
            LemonChiffon,
            LightBlue,
            LightCoral,
            LightCyan,
            LightGoldenrodYellow,
            LightGray,
            LightGreen,
            LightPink,
            LightSalmon,
            LightSeaGreen,
            LightSkyBlue,
            LightSlateGray,
            LightSteelBlue,
            LightYellow,
            Lime,
            LimeGreen,
            Linen,
            Magenta,
            Maroon,
            MediumAquamarine,
            MediumBlue,
            MediumOrchid,
            MediumPurple,
            MediumSeaGreen,
            MediumSlateBlue,
            MediumSpringGreen,
            MediumTurquoise,
            MediumVioletRed,
            MidnightBlue,
            MintCream,
            MistyRose,
            Moccasin,
            NavajoWhite,
            Navy,
            OldLace,
            Olive,
            OliveDrab,
            Orange,
            OrangeRed,
            Orchid,
            PaleGoldenrod,
            PaleGreen,
            PaleTurquoise,
            PaleVioletRed,
            PapayaWhip,
            PeachPuff,
            Peru,
            Pink,
            Plum,
            PowderBlue,
            Purple,
            Red,
            RosyBrown,
            RoyalBlue,
            SaddleBrown,
            Salmon,
            SandyBrown,
            SeaGreen,
            SeaShell,
            Sienna,
            Silver,
            SkyBlue,
            SlateBlue,
            SlateGray,
            Snow,
            SpringGreen,
            SteelBlue,
            Tan,
            Teal,
            Thistle,
            Tomato,
            Transparent,
            Turquoise,
            Violet,
            Wheat,
            White,
            WhiteSmoke,
            Yellow,
            YellowGreen

        }

        public enum CalcMode
        {

            Money,
            Percentage

        }

        #endregion

        #region Identity

        public const string NAME = "Money Break Even";

        public const string VERSION = "1.0.9";

        #endregion

        #region Params

        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://www.google.com/search?q=ctrader+guru+money+break+even")]
        public string ProductInfo { get; set; }

        [Parameter("Mode", Group = "Params", DefaultValue = CalcMode.Percentage)]
        public CalcMode MyCalcMode { get; set; }

        [Parameter("Net Profit Activation ( $ | % )", Group = "Params", DefaultValue = 10.0)]
        public double BEfrom { get; set; }

        [Parameter("Net Profit Target ( $ | % )", Group = "Params", DefaultValue = 3.0)]
        public double BE { get; set; }

        [Parameter("All Cross ?", Group = "Options", DefaultValue = false)]
        public bool GlobalTarget { get; set; }

        [Parameter("Auto Stop ?", Group = "Options", DefaultValue = true)]
        public bool AutoStop { get; set; }

        [Parameter("Remove Pending Orders ?", Group = "Options", DefaultValue = true)]
        public bool RemovePO { get; set; }

        [Parameter("Color Target Logic", Group = "Styles", DefaultValue = MyColors.Magenta)]
        public MyColors Boxcolortarget { get; set; }

        [Parameter("Color Positive Logic", Group = "Styles", DefaultValue = MyColors.DodgerBlue)]
        public MyColors Boxcolorpositive { get; set; }

        [Parameter("Color Negative Logic", Group = "Styles", DefaultValue = MyColors.Orange)]
        public MyColors Boxcolornegative { get; set; }

        [Parameter("Color Activated", Group = "Styles", DefaultValue = MyColors.DarkViolet)]
        public MyColors Boxcoloractive { get; set; }

        [Parameter("Vertical Position", Group = "Styles", DefaultValue = VerticalAlignment.Top)]
        public VerticalAlignment VAlign { get; set; }

        [Parameter("Horizontal Position", Group = "Styles", DefaultValue = HorizontalAlignment.Left)]
        public HorizontalAlignment HAlign { get; set; }


        #endregion

        #region Property

        private bool Activated;

        private double FixedBEfrom = 0;
        private double FixedBE = 0;
        private double FixedBalance = 0;

        #endregion

        #region cBot Events

        protected override void OnStart()
        {

            Print("{0} : {1}", NAME, VERSION);

            FixedBEfrom = BEfrom;
            FixedBE = BE;
            FixedBalance = Account.Balance;

            Activated = false;

            if (!GlobalTarget) { 
            
                OnTick();
            
            }
            else
            {

                Timer.Start(1);
                OnTimer();

            }

        }

        protected override void OnTimer()
        {

            if (GlobalTarget) Monitoring();

        }

        protected override void OnTick()
        {

            if (!GlobalTarget) Monitoring();

        }

        #endregion

        #region Private Methods

        void Monitoring()
        {

            if (MyCalcMode == CalcMode.Percentage)
            {

                BEfrom = Math.Round((FixedBalance / 100) * FixedBEfrom, 2);
                BE = Math.Round((FixedBalance / 100) * FixedBE, 2);

            }

            int nPositions = 0;

            double ttnp = 0.0;

            foreach (var Position in Positions)
            {

                if (!GlobalTarget && Position.SymbolName != SymbolName)
                    continue;

                ttnp += Position.NetProfit;
                nPositions++;

            }

            if (nPositions < 1)
            {

                Activated = false;

                FixedBalance = Account.Balance;

            }

            if (AutoStop && nPositions < 1)
                Stop();

            if (!Activated && ((BEfrom > BE && ttnp >= BEfrom) || (BEfrom < BE && ttnp <= BEfrom)))
                Activated = true;

            string scope = (GlobalTarget) ? "all cross" : "the current cross";
            string logica = (BEfrom == BE) ? "target" : (BEfrom > BE) ? "positive" : "negative";
            string direction = (BEfrom > BE) ? "less" : "greater";
            string netpt = String.Format("{0:0.00}", ttnp);

            string phrase = (Activated) ? string.Format("Activated, I will close all trades for\r\n{0} if net profit is {1} or equal {2}", scope, direction, BE) : string.Format("Relax, I'm monitoring {0} for\r\n{1} logic and waiting net profit reaches {2}", scope, logica, BEfrom);

            string[] items = 
            {

                "MONEY BREAK EVEN\r\n",
                phrase,
                "\r\nNET PROFIT\r\n{0}"

            };
            string info = string.Join("\r\n", items);

            info = string.Format(info, netpt);

            MyColors mycolor = (BEfrom == BE) ? Boxcolortarget : (BEfrom > BE) ? Boxcolorpositive : Boxcolornegative;

            if (Activated)
                mycolor = Boxcoloractive;

            Chart.DrawStaticText("BoxMBE", info, VAlign, HAlign, Color.FromName(mycolor.ToString("G")));

            if ((Activated && ((BEfrom > BE && ttnp <= BE) || (BEfrom < BE && ttnp >= BE))) || (BEfrom == BE && ((BE >= 0 && ttnp >= BE) || (BE < 0 && ttnp <= BE))))
            {

                foreach (var Position in Positions)
                {

                    if (!GlobalTarget && Position.SymbolName != SymbolName)
                        continue;

                    ClosePositionAsync(Position);

                }

                if (RemovePO)
                {

                    foreach (var order in PendingOrders)
                    {

                        if (!GlobalTarget && order.SymbolName != SymbolName)
                            continue;

                        CancelPendingOrderAsync(order);

                    }

                }

                Activated = false;

            }

        }

        #endregion

    }

}
