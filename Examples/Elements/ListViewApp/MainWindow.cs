using System;
using System.Collections.Generic;
using System.Linq;
using ClForms.Elements;
using ClForms.Elements.Menu;
using ListViewApp.Forms;
using ListViewApp.Models;

namespace ListViewApp
{
    /// <summary>
    /// The main Window of App
    /// </summary>
    public partial class MainWindow : Window
    {
        private ScreenType currentScreenType;
        public static List<CountryInfo> CountryInfos = new List<CountryInfo>()
        {
            new CountryInfo
            {
                Country = "Luxembourg",
                Alpha3 = "LUX",
                Gdp = 117312,
                Population = 604.24M,
                Percentage = 0.01M,
            },
            new CountryInfo
            {
                Country = "Switzerland",
                Alpha3 = "CHE",
                Gdp = 88279,
                Population = 8_570.15M,
                Percentage = 0.11M,
            },
            new CountryInfo
            {
                Country = "Norway",
                Alpha3 = "NOR",
                Gdp = 81336,
                Population = 5337.96M,
                Percentage = 0.07M,
            },
            new CountryInfo
            {
                Country = "Ireland",
                Alpha3 = "IRL",
                Gdp = 79376,
                Population = 4_818.69M,
                Percentage = 0.06M,
            },
            new CountryInfo
            {
                Country = "Iceland",
                Alpha3 = "ISL",
                Gdp = 76855,
                Population = 336.71M,
                Percentage = 0.00M,
            },
            new CountryInfo
            {
                Country = "Singapore",
                Alpha3 = "SGP",
                Gdp = 63249,
                Population = 5_757.50M,
                Percentage = 0.08M,
            },
            new CountryInfo
            {
                Country = "United States",
                Alpha3 = "USA",
                Gdp = 62808,
                Population = 327_096.26M,
                Percentage = 4.29M,
            },
            new CountryInfo
            {
                Country = "Denmark",
                Alpha3 = "DNK",
                Gdp = 61834,
                Population = 5_752.13M,
                Percentage = 0.08M,
            },
            new CountryInfo
            {
                Country = "Australia",
                Alpha3 = "AUS",
                Gdp = 57591,
                Population = 24_898.15M,
                Percentage = 0.33M,
            },
            new CountryInfo
            {
                Country = "Sweden",
                Alpha3 = "SWE",
                Gdp = 55767,
                Population = 9_971.63M,
                Percentage = 0.13M,
            },
            new CountryInfo
            {
                Country = "Netherlands",
                Alpha3 = "NLD",
                Gdp = 53557,
                Population = 17_059.56M,
                Percentage = 0.22M,
            },
            new CountryInfo
            {
                Country = "Austria",
                Alpha3 = "AUT",
                Gdp = 51205,
                Population = 8_891.38M,
                Percentage = 0.12M,
            },
            new CountryInfo
            {
                Country = "Finland",
                Alpha3 = "FIN",
                Gdp = 50111,
                Population = 5_522.59M,
                Percentage = 0.07M,
            },
            new CountryInfo
            {
                Country = "Hong Kong",
                Alpha3 = "HKG",
                Gdp = 48471,
                Population = 7_482.50M,
                Percentage = 0.10M,
            },
            new CountryInfo
            {
                Country = "Germany",
                Alpha3 = "DEU",
                Gdp = 47491,
                Population = 83_124.41M,
                Percentage = 1.09M,
            },
            new CountryInfo
            {
                Country = "Belgium",
                Alpha3 = "BEL",
                Gdp = 47270,
                Population = 11_482.18M,
                Percentage = 0.15M,
            },
        };

        /// <summary>
        /// Initialize a new instance <see cref="MainWindow"/>
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            GotoScreen(ScreenType.Welcome);
        }

        private void GotoScreen(ScreenType targetScreenType)
        {
            currentScreenType = targetScreenType;
            switch (targetScreenType)
            {
                case ScreenType.Welcome:
                    _ = new WelcomeForm { panel1 = { Parent = panel1 } };
                    break;

                case ScreenType.Base:
                    _ = new BaseListView(propMenuItem) { panel1 = { Parent = panel1 } };
                    statusBarLabel.Text = "Base properties";
                    break;

                case ScreenType.Headers:
                    _ = new HeaderForm(propMenuItem) { panel1 = { Parent = panel1 } };
                    statusBarLabel.Text = "Headers";
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(targetScreenType), targetScreenType, null);
            }

            if (currentScreenType != ScreenType.Welcome && MainMenu == null)
            {
                MainMenu = mainMenu1;
            }
        }

        protected override void InputActionInternal(ConsoleKeyInfo keyInfo)
        {
            base.InputActionInternal(keyInfo);
            if (keyInfo.Key == ConsoleKey.Spacebar && currentScreenType == ScreenType.Welcome)
            {
                GotoScreen((ScreenType) ((int) currentScreenType + 1));
            }
        }

        private void StepsMenuClick(object sender, System.EventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                foreach (var item in controlsMenuItem.Items
                    .Where(x => x is MenuItem)
                    .Cast<MenuItem>())
                {
                    item.Checked = false;
                }

                menuItem.Checked = true;
                GotoScreen((ScreenType) menuItem.Tag);
            }
        }
    }
}
