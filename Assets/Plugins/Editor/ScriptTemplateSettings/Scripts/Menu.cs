///-----------------------------------------------------------------
///   Author : Théo Sabattié                    
///   Date   : 14/06/2019 12:17
///-----------------------------------------------------------------

using Com.Sabattie.Theo.ScriptTemplateSettings;
using System;
using UnityEditor;

namespace Com.DefaultCompany.ScriptTemplateSettings {

    public static class Menu {
        private const string EDITOR_TEMPLATE_PATH = "Editor/82-C# Editor__Editor-NewEditor.cs.txt";
        private const string DRAWER_TEMPLATE_PATH = "Editor/82-C# Editor__Property Drawer-NewPropertyDrawer.cs.txt";
        private const string EDITOR_MENU = "Assets/Create/C# Editor/Editor";
        private const string DRAWER_MENU = "Assets/Create/C# Editor/Drawer";
        private const int PRIORITY = 80;

        [MenuItem(EDITOR_MENU, priority = PRIORITY)]
        public static void CreateEditor () {
            SetTargetedTypeThenCreateFromTemplate(EDITOR_TEMPLATE_PATH, "{0}Editor.cs");
        }

        [MenuItem(DRAWER_MENU, priority = PRIORITY)]
        public static void CreateDrawer () {
            SetTargetedTypeThenCreateFromTemplate(DRAWER_TEMPLATE_PATH, "{0}Drawer.cs");
        }

        [MenuItem(EDITOR_MENU, validate = true)]
        public static bool ValidCreateEditor()
        {
            return GetSelectedMonoScript();
        }

        [MenuItem(DRAWER_MENU, validate = true)]
        public static bool ValidCreateDrawer()
        {
            return GetSelectedMonoScript();
        }

        private static bool SetTargetedTypeThenCreateFromTemplate(string templatePath, string fileNameTemplate)
        {
            MonoScript script = GetSelectedMonoScript();

            if (!script)
                return false;

            Type type = script.GetClass();
            TargetedTypeProcessor.SetCurrentTargetedType(type);
            ScriptTemplateUtils.CreateScriptFromTemplate(templatePath, string.Format(fileNameTemplate, type.Name));

            return true;
        }

        public static MonoScript GetSelectedMonoScript()
        {
            return Selection.activeObject as MonoScript;
        }
    }
}