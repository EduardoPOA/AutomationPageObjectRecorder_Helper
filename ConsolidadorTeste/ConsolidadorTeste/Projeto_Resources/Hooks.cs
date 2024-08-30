using Hook_Validator;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace ConsolidadorTeste.Projeto_Resources
{
    [Binding]
    class Hooks
    {
        private static string filePath = System.IO.Directory.GetParent(System.IO.Directory.GetParent(System.IO.Directory.GetParent(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)).FullName).FullName).FullName + @"\Teste.txt";

        [BeforeTestRun]
        public static void BeforeInstallChrome()
        {
            if (!File.Exists(filePath))
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine();
                    sw.Close();
                }
            }
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            //Inserir o caminho da saída do relatório e screenshot
            Hook.BeforeTestRun(@"Projeto_Report", @"Projeto_Locators");
        }

        [BeforeFeature]
        public static void BeforeFeature()
        {
            Hook.BeforeFeature();
        }

        [BeforeScenario]
        public static void BeforeScenario()
        {
            /* -- Escolha Navegador -- */
            //Edge
            //Firefox
            //Chrome

            /* -- Ativar ou desavitar headless -- */
            string headlessAtivado = "--headless";
            string headlessDesativado = "none";

            /* -- Ativar ou desavitar modo mobile chrome, deve inserir o mesmo nome do device do chrome -- */
            string device = string.Empty;

            Hook.BeforeScenario("Chrome", headlessDesativado, device);
        }

        [AfterStep]
        public static void AfterStep()
        {
            Hook.AfterStep();
        }

        [AfterScenario]
        public static void AfterScenario()
        {
            Hook.AfterScenario();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            Hook.AfterTestRun();
        }
    }
}
