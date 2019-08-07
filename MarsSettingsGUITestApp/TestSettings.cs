using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsSettingsGUI;
using robotManager.Helpful;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;


[Serializable]
public class TestSetting : Settings
{
    [Setting]
    [Category("Category1")]
    [DisplayName("Toggle Value 1")]
    [Description("The first of the toggles")]
    public bool Toggle1 { get; set; }
    [Setting]
    [Category("Category1")]
    [DisplayName("Percent Value 1")]
    [ValueRange(30, 90)]
    [Order(1)]
    [Description("")]
    public int Percent1 { get; set; }
    [Setting]
    [Category("Category1")]
    [DisplayName("Int Value 1")]
    [Description("")]
    public long Long1 { get; set; }
    [Setting]
    [Category("Category1")]
    [DisplayName("Text Value 1")]
    [Description("")]
    public string Text1 { get; set; }
    [Setting]
    [Category("Category1")]
    [DisplayName("String List 0")]
    [Description("")]
    public List<string> StringList0 { get; set; }
    [Setting]
    [Category("Category1")]
    [DisplayName("Int List 0")]
    [Description("")]
    public List<int> IntList0 { get; set; }

    [Setting]
    [Category("Category2")]
    [DisplayName("Toggle Value 2")]
    [Description("")]
    public bool Toggle2 { get; set; }

    [Setting]
    [Category("Category2")]
    [DisplayName("String List 1")]
    [Description("")]
    public List<string> StringList1 { get; set; }
    [Setting]
    [Category("Category2")]
    [DisplayName("String List 2")]
    [Description("")]
    public List<string> StringList2 { get; set; }

    [Setting]
    [Category("Category2")]
    [DisplayName("String List 3")]
    [Description("")]
    public List<string> StringList3 { get; set; }
    [Setting]
    [Category("Category2")]
    [DisplayName("String List 4")]
    [Description("")]
    public List<string> StringList4 { get; set; }

    [Setting]
    [Category("Category2")]
    [DisplayName("String List 5")]
    [Description("")]
    public List<string> StringList5 { get; set; }
    [Setting]
    [Category("Category2")]
    [DisplayName("String List 6")]
    [Description("")]
    public List<string> StringList6 { get; set; }

    [Setting]
    [Category("Category3")]
    [DisplayName("Toggle Value 3")]
    [Description("")]
    public bool Toggle3 { get; set; }

    [Setting]
    [Category("Category4")]
    [DisplayName("Toggle Value 4")]
    [Description("")]
    public bool Toggle4 { get; set; }

    [Setting]
    [Category("Category1")]
    [DisplayName("Dropdown 1")]
    [Description("A tuple dropdown")]
    [DropdownList(new string[] { "Battle Shout", "Commanding Shout" })]
    public short Dropdown1 { get; set; }

    public TestSetting()
    {
        Percent1 = 30;
        Toggle1 = false;
        Text1 = "Test";
        Toggle2 = true;
        StringList0 = new List<string>();
        StringList1 = new List<string>();
        StringList2 = new List<string>();
        StringList3 = new List<string>();
        StringList4 = new List<string>();
        StringList5 = new List<string>();
        StringList6 = new List<string>();
        IntList0 = new List<int>();
        

        //Array1 = new List<string>(); //{ "Abc", "Def" };
    }

    public static TestSetting CurrentSetting { get; set; }

    public bool Save()
    {
        try
        {
            return Save("test.xml");
        }
        catch (Exception e)
        {
            Logging.WriteError("Test > Save(): " + e);
            return false;
        }
    }

    public static bool Load()
    {
        try
        {
            if (File.Exists("test.xml"))
            {
                CurrentSetting =
                    Load<TestSetting>("test.xml");
                return true;
            }
            CurrentSetting = new TestSetting();
        }
        catch (Exception e)
        {
            Logging.WriteError("Test > Load(): " + e);
        }
        return false;
    }
}
