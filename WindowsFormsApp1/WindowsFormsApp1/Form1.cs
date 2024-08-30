using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string getFeature { get; set; }
        private string getPathPageCs { get; set; }
        private string getPageCs { get; set; }
        private string getStepCs { get; set; }
        private string getReport { get; set; }
        private string getLocators { get; set; }
        private string getE2E { get; set; }
        private string getSupportPage { get; set; }
        private string splitingNamePage { get; set; }
        private StreamReader read;
        public string filePathXml { get; set; }
        public string getElement { get; set; }
        public StreamWriter write;
        private string keyAttribute { get; set; }
        private string byAttribute { get; set; }
        private string valueAttribute { get; set; }
        private string baseAttribute { get; set; }
        private string getTextCshtmlFormated { get; set; }
        private string setInsertScenarioSteps { get; set; }
        public string compareStringFromList { get; set; }
        public string stepDeclaration { get; set; }
        public string previewStepName { get; set; }
        public string getCsproj { get; set; }
        public string getFirstParam { get; set; }
        public int count = 0;
        public string getPreviewStep { get; set; }
        public string setParameter { get; set; }
        public string Type;
        public string RegexPattern;
        public List<string> listMethods = new List<string>();

        private void button1_Click_1(object sender, EventArgs e)
        {
            string teste = textBox1.Text.Replace("\"", "")
                      .Replace("<", "")
                      .Replace(">", "")
                      .Replace("element", "")
                      .Replace("@id=", "@id!")
                      .Trim();
            teste = teste.Substring(teste.LastIndexOf("key") + 0).Split()[0].Replace("key=", "");
            teste = teste.Split('.')[0];
            GenerateTemplateProject(teste);

        }


        internal void GenerateTemplateProject(string nameFeature)
        {
            //cypresss
            string folder2e2 = "cypress\\e2e";
            string folderSupportPages = "cypress\\support\\pages";
            //////////////////
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string[] directories = System.IO.Directory.GetDirectories(fbd.SelectedPath, "*", System.IO.SearchOption.AllDirectories);
                bool existe = false;
                string[] directoriesFiles = System.IO.Directory.GetFiles(fbd.SelectedPath, "*", System.IO.SearchOption.AllDirectories);

                //cypresss
                string[] cypressDirectories = Directory.GetDirectories(fbd.SelectedPath, "*", SearchOption.AllDirectories);
                string directory2e2 = cypressDirectories
       .FirstOrDefault(dir => dir.EndsWith(Path.Combine(folder2e2), StringComparison.OrdinalIgnoreCase));
                getE2E = directory2e2;
                string directorySupportPages = cypressDirectories
       .FirstOrDefault(dir => dir.EndsWith(Path.Combine(folderSupportPages), StringComparison.OrdinalIgnoreCase));
                getSupportPage = directorySupportPages;
                /////////////////////
                foreach (string file in directoriesFiles.Distinct().Concat(directories))
                {
                    if (existe == true) { break; }
                    string files = file;
                    switch (files)
                    {
                        case string x when file.Contains(".gitignore"):
                            continue;
                        case string x when file.Contains(".git"):
                            continue;
                        case string x when file.Contains(".vs"):
                            continue;
                        case string x when file.Contains("bin"):
                            continue;
                        case string x when file.Contains("obj"):
                            continue;
                        case string x when file.Contains("ms-persist.xml"):
                            continue;
                        case string x when file.EndsWith(".csproj"):
                            getCsproj = x;
                            continue;
                    }

                    if (file.EndsWith("_Features"))
                    {
                        getFeature = file + "\\" + nameFeature + ".feature";
                        if (!File.Exists(getFeature))
                        {
                            using (FileStream fs = File.Create(getFeature)) ;
                            using (StreamWriter wt = new StreamWriter(getFeature))
                            {
                                wt.WriteLine("##language:pt\nFeature: " + nameFeature + "\n  Insert the introduction of this feature\n\n");
                                wt.Close();
                            }
                        }
                    }

                    if (file.EndsWith("_Locators"))
                    {
                        string fullPath = Path.GetFullPath(file).TrimEnd(Path.DirectorySeparatorChar);
                        string splitNameReport = fullPath.Split('\\').Last();
                        getLocators = "\"" + splitNameReport + "\"";
                        string fileName = Path.GetFileName(file);
                        string fileNameExtension = file + "\\PageObjects.xml";
                        if (!File.Exists(fileNameExtension))
                            using (var writer = new StreamWriter(fileName))
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml("<locators>\n</locators>");
                                XmlDeclaration xmldecl;
                                xmldecl = doc.CreateXmlDeclaration("1.0", "ISO-8859-1", null);
                                XmlElement root = doc.DocumentElement;
                                doc.InsertBefore(xmldecl, root);
                                filePathXml = file + "\\PageObjects.xml";
                                doc.Save(file + "\\PageObjects.xml");
                            }
                        else
                        {
                            filePathXml = file + "\\PageObjects.xml";
                        }
                    }

                    //cypress
                    if (getSupportPage != null)
                    {
                        if (getSupportPage.EndsWith("pages"))
                        {
                            getLocators = getSupportPage + "\\" + "pageObjects.xml";
                            if (!File.Exists(getLocators))
                                using (var writer = new StreamWriter(getLocators))
                                {
                                    XmlDocument doc = new XmlDocument();
                                    doc.LoadXml("<locators>\n</locators>");
                                    XmlDeclaration xmldecl;
                                    xmldecl = doc.CreateXmlDeclaration("1.0", "ISO-8859-1", null);
                                    XmlElement root = doc.DocumentElement;
                                    doc.InsertBefore(xmldecl, root);
                                    filePathXml = getLocators;
                                    doc.Save(filePathXml);
                                }
                            filePathXml = getLocators;  
                        }
                    }

                    //cypresss
                    if (getE2E != null)
                    {
                        if (getE2E.EndsWith("e2e"))
                        {
                            getE2E = getE2E + "\\" + nameFeature + ".cy.js";
                            if (!File.Exists(getE2E))
                            {

                                using (FileStream fs = File.Create(getE2E)) ;
                                using (StreamWriter wt = new StreamWriter(getE2E))
                                {
                                    wt.WriteLine("import '../support/commands' \n\n" +
                                        "describe.only('---> Describe the test case title <---', () => {\n\n})");
                                    wt.Close();
                                }
                            }
                        }
                    }

                    if (file.EndsWith("_Pages"))
                    {
                        string fullPath = Path.GetFullPath(file).TrimEnd(Path.DirectorySeparatorChar);
                        string splitingNameFolder = fullPath.Split('\\').Last();
                        string nameSpace = Path.GetDirectoryName(fullPath);
                        string namePage = Path.GetFileName(fullPath);
                        splitingNamePage = namePage.Split('\\').Last();
                        string splitingNameSpace = nameSpace.Split('\\').Last();
                        getPathPageCs = file + "\\" + nameFeature + "_page.cs";
                        getPageCs = file + "\\" + nameFeature + "_page.cs";
                        if (!File.Exists(getPageCs))
                        {

                            using (FileStream fs = File.Create(getPathPageCs)) ;
                            using (StreamWriter wt = new StreamWriter(getPathPageCs))
                            {
                                wt.WriteLine("using Hook_Validator;\nusing OpenQA.Selenium;\nusing System.Threading;\n\n" +
                                    "namespace " + splitingNameSpace + "." + splitingNameFolder + "\n" +
                                    "{\n    class " + nameFeature + "_page : Tools\n    {\n\n        public bool startBrowser(string url)\n" +
                                    "        {\n            OpenPage(url);\n            return true;\n        }    }\n}");
                                wt.Close();
                            }
                        }
                    }

                    if (file.EndsWith("_Report"))
                    {
                        string fullPath = Path.GetFullPath(file).TrimEnd(Path.DirectorySeparatorChar);
                        string splitNameReport = fullPath.Split('\\').Last();
                        getReport = "\"" + splitNameReport + "\"";
                    }

                    if (file.EndsWith("_Resources"))
                    {
                        string headless = "\"--headless\"";
                        string none = "\"none\"";
                        string defaultChrome = "\"Chrome\"";
                        string hookClass = file + "\\Hooks.cs";
                        if (!File.Exists(file))
                        {
                            using (FileStream fs = File.Create(hookClass)) ;
                            using (StreamWriter wt = new StreamWriter(hookClass))
                            {
                                wt.WriteLine("using Hook_Validator;\nusing System;\nusing System.Collections.Generic;\nusing System.Text;\nusing TechTalk.SpecFlow;\n\n" +
                                    "namespace CoezzionPortalAdmin.Portal_Resources\n{\n    [Binding]\n    class Hooks\n    {\n\n        [BeforeTestRun]\n" +
                                    "        public static void BeforeInstallChrome()\n        {\n            Hook.AutoUpdateChrome();\n        }\n\n" +
                                    "        [BeforeTestRun]\n        public static void BeforeTestRun()\n        {\n            //Inserir o caminho da saída do relatório e screenshot\n" +
                                    "            Hook.BeforeTestRun(@" + getReport + ", @" + getLocators + ");\n        }\n\n        [BeforeFeature]\n        public static void BeforeFeature()\n" +
                                    "        {\n            Hook.BeforeFeature();\n        }\n\n        [BeforeScenario]\n        public static void BeforeScenario()\n" +
                                    "        {\n            /* -- Escolha Navegador -- */\n            //Edge\n            //Firefox\n            //Chrome\n\n" +
                                    "            /* -- Ativar ou desavitar headless -- */\n            string headlessAtivado = " + headless + ";\n" +
                                    "            string headlessDesativado = " + none + ";\n\n            /* -- Ativar ou desavitar modo mobile chrome, deve inserir o mesmo nome do device do chrome -- */\n" +
                                    "            string device = string.Empty;\n\n            Hook.BeforeScenario(" + defaultChrome + ", headlessDesativado, device);\n        }\n\n" +
                                    "        [AfterStep]\n        public static void AfterStep()\n        {\n            Hook.AfterStep();\n        }\n\n        [AfterScenario]\n" +
                                    "        public static void AfterScenario()\n        {\n            Hook.AfterScenario();\n        }\n\n        [AfterTestRun]\n        public static void AfterTestRun()\n" +
                                    "        {\n            Hook.AfterTestRun();\n        }\n    }\n}");
                                wt.Close();
                            }
                        }
                    }

                    if (file.EndsWith("_Steps"))
                    {
                        string fullPath = Path.GetFullPath(file).TrimEnd(Path.DirectorySeparatorChar);
                        string splitingNameFolder = fullPath.Split('\\').Last();
                        string nameSpace = Path.GetDirectoryName(fullPath);
                        string splitingNameSpace = nameSpace.Split('\\').Last();
                        getStepCs = file + "\\" + nameFeature + "_step.cs";
                        string namePageName = Path.GetFileName(getPathPageCs);
                        if (!File.Exists(getStepCs))
                        {
                            string splitingPageName = namePageName.Split('\\').Last().Replace(".cs", "");
                            using (FileStream fs = File.Create(getStepCs)) ;
                            using (StreamWriter wt = new StreamWriter(getStepCs))
                            {
                                wt.WriteLine("using TechTalk.SpecFlow;\nusing NUnit.Framework;\nusing " + splitingNameSpace + "." + "" + splitingNamePage + ";\n\n" +
                                    "namespace " + splitingNameSpace + "." + splitingNameFolder + " \n{\n    [Binding]" +
                                        "\n    class " + nameFeature + "_step : " + splitingPageName + "\n    {\n    }\n}");
                                wt.Close();
                            }
                        }
                    }
                }
            }

            //condition cypress
            if (getSupportPage == null)
            {
                addPlugins();
            }
            ReadTemplateCshtml();
        }
        public static string GetAssemblyDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        internal void ReadTemplateCshtml()
        {
            write = new StreamWriter(Path.Combine(GetAssemblyDirectory(), "_pageObject.txt"));
            write.Write(textBox1.Text.ToString());
            write.Close();
            string getTextMethodsFromCshtml = Path.Combine(GetAssemblyDirectory(), "_pageObject.txt");
            Thread.Sleep(500);
            read = new StreamReader(Path.Combine(GetAssemblyDirectory(), "_pageObject.txt"));
            bool existe = false;
            string pageObject = read.ReadLine().Trim();
            while (pageObject != null)
            {
                existe = true;
                if (pageObject.StartsWith("<element key"))
                {
                    getTextCshtmlFormated = pageObject.Replace("\"", "")
                      .Replace("<", "")
                      .Replace(">", "")
                      .Replace("element", "")
                      .Trim();
                    keyAttribute = getTextCshtmlFormated.Substring(getTextCshtmlFormated.LastIndexOf("key") + 0).Split()[0].Replace("key=", "");
                    int byIndex = getTextCshtmlFormated.LastIndexOf("by");
                    if (byIndex != -1)
                    {
                        byAttribute = getTextCshtmlFormated.Substring(getTextCshtmlFormated.LastIndexOf("by") + 0).Split()[0].Replace("by=", "");
                    }
                    else { }
                    if (getSupportPage != null)
                    {
                        valueAttribute = getTextCshtmlFormated.Substring(getTextCshtmlFormated.LastIndexOf("value") + 0).Replace("value=", "");
                        valueAttribute = valueAttribute.Remove(valueAttribute.Length - 12).Trim();
                        if (valueAttribute.StartsWith("baseValue")) { valueAttribute = null; }
                        baseAttribute = getTextCshtmlFormated.Substring(getTextCshtmlFormated.LastIndexOf("baseValue") + 0).Replace("baseValue=", "").Trim();
                        if (valueAttribute != null)
                        {
                            valueAttribute = getTextCshtmlFormated.Substring(getTextCshtmlFormated.LastIndexOf("value") + 0).Replace("value=", "");
                            valueAttribute = valueAttribute.Remove(valueAttribute.Length - 12).Trim();
                            baseAttribute = null;
                        }
                        else
                        {
                            baseAttribute = getTextCshtmlFormated.Substring(getTextCshtmlFormated.LastIndexOf("baseValue") + 0).Replace("baseValue=", "").Trim();
                            }
                    }
                    else
                    {
                        valueAttribute = getTextCshtmlFormated.Substring(getTextCshtmlFormated.LastIndexOf("value") + 0).Replace("value=", "");
                        valueAttribute = valueAttribute.Remove(valueAttribute.Length - 13).Trim();
                    }
                }
                else
                {
                    pageObject = read.ReadLine();
                    continue;
                }
                RemoveCaractersXml();
                InsertXml();
                pageObject = read.ReadLine();
            }
            if (existe == true)
            {
                UpdateXml();
                SortElementsXml();
                InsertFeatureSeparator();
                replaceObject();
            }
            read.Close();
            Thread.Sleep(300);
            List<string> list = File.ReadAllLines(getTextMethodsFromCshtml).Where(arg => !string.IsNullOrWhiteSpace(arg)).ToList();
            list.RemoveAll(x => x.StartsWith("<element key") || x.StartsWith(" <element key"));
            File.WriteAllLines(getTextMethodsFromCshtml, list);
            foreach (string item in list)
            {
                if (item.StartsWith("public") || item.StartsWith(" public"))
                {
                    string itemReplace = item.Replace("string ", "").Replace("int ", "");
                    listMethods.Add(itemReplace.Substring(itemReplace.LastIndexOf(' ') + 1));
                }
                //cypress
                else if (item.StartsWith("it") || item.StartsWith(" it"))
                {
                    listMethods.Add(item);
                }
            }
            Thread.Sleep(300);
            try
            {
                string firstString = list.FirstOrDefault(s => s.StartsWith("it") || s.StartsWith("public"));

                if (firstString.StartsWith("public") || firstString.StartsWith(" public"))
                {
                    insertScenarioSteps();
                }

                //cypress
                if (firstString.StartsWith("it") || firstString.StartsWith(" it"))
                {
                    string textScript;
                    using (var read = new StreamReader(Path.Combine(GetAssemblyDirectory(), "_pageObject.txt")))
                    {
                        textScript = read.ReadToEnd().Trim();
                    }
                    string scriptCode;
                    using (StreamReader readCypress = new StreamReader(getE2E))
                    {
                        scriptCode = readCypress.ReadToEnd();
                    }
                    int describeEndIndex = scriptCode.LastIndexOf('}');
                    string describeBlock = scriptCode.Substring(0, describeEndIndex);
                    string updatedDescribeBlock = describeBlock + "\n" + textScript;
                    string updatedScriptCode = scriptCode.Substring(describeEndIndex);
                    using (StreamWriter write = new StreamWriter(getE2E))
                    {
                        write.Write(updatedDescribeBlock + "\n" + updatedScriptCode);
                    }
                }
            }
            catch (Exception) { }
            Thread.Sleep(500);
            MessageBox.Show("Finalizado");
        }

        private void insertScenarioSteps()
        {
            string textScript = null;
            using (var read = new StreamReader(Path.Combine(GetAssemblyDirectory(), "_pageObject.txt")))
            {
                textScript = read.ReadToEnd().Trim();
                string lastBreakLine = textScript.Substring(textScript.LastIndexOf("}") + 1);
                using (StreamReader readFeature = new StreamReader(getFeature))
                {
                    stepDeclaration = readFeature.ReadToEnd();
                    string UpToLastParan = stepDeclaration.Substring(0, stepDeclaration.LastIndexOf("\n"));
                    int SecondToLast = UpToLastParan.LastIndexOf("\n");
                    string UpToSecondToLastParan = stepDeclaration.Substring(0, SecondToLast);
                    setInsertScenarioSteps = UpToSecondToLastParan + "\n" + lastBreakLine.Trim() + stepDeclaration.Substring(SecondToLast, stepDeclaration.Length - SecondToLast);
                    readFeature.Close();
                }
                using (StreamWriter wt = new StreamWriter(getFeature))
                {
                    wt.WriteLine(setInsertScenarioSteps);
                    wt.Close();
                }
                Thread.Sleep(250);
                List<string> resultOld = stepDeclaration.Split('\n').ToList();
                resultOld = resultOld.Select(s => s.Replace("\n", string.Empty)
                .Replace("\r", string.Empty)
                .Replace("\t", string.Empty))
                    .ToList();
                List<string> resultNew = setInsertScenarioSteps.Split('\n').ToList();
                resultNew = resultNew.Select(s => s.Replace("\n", string.Empty)
                .Replace("\r", string.Empty)
                .Replace("\t", string.Empty))
                    .ToList();
                var getEachStep = resultNew.Except(resultOld);
                var getEachStepPreview = resultNew.Except(resultOld);
                int pos = 1;
                List<string> teste = getEachStep.ToList();
                write = new StreamWriter(Path.Combine(GetAssemblyDirectory(), "_stepGenerator.txt"));
                foreach (string test in resultNew)
                {
                    write.WriteLine(test);
                }
                write.Close();

                foreach (string beforeItem in File.ReadLines(Path.Combine(GetAssemblyDirectory(), "_stepGenerator.txt")))
                {
                    foreach (string afterItem in File.ReadLines(Path.Combine(GetAssemblyDirectory(), "_stepGenerator.txt")).Skip(pos))
                    {
                        previewStepName = Convert.ToString(afterItem);
                        pos++;
                        break;
                    }
                    switch (beforeItem)
                    {
                        case string title when title.StartsWith("Cenario") || title.StartsWith("Cenário"):
                            continue;
                        case string title when title.StartsWith("Scenario") || title.StartsWith("Scenarios"):
                            continue;
                        case string title when title.StartsWith("Scenario Outline") || title.StartsWith("Scenario Template"):
                            continue;
                    }

                    string stepName = createStepDefinition(beforeItem);
                    try
                    {
                        using (StreamReader readStep = new StreamReader(getStepCs))
                        {
                            string scriptCodeStep = readStep.ReadToEnd();
                            string UpToLastIndex = scriptCodeStep.Substring(0, scriptCodeStep.LastIndexOf("}"));
                            int SecondToLastIndex = UpToLastIndex.LastIndexOf("}");
                            string UpToSecondToLastIndex = scriptCodeStep.Substring(0, SecondToLastIndex);
                            string insertMethodFromStep = UpToSecondToLastIndex + "\n" + stepName + "\n" + scriptCodeStep.Substring(SecondToLastIndex, scriptCodeStep.Length - SecondToLastIndex);
                            readStep.Close();
                            Thread.Sleep(250);
                            using (StreamWriter write = new StreamWriter(getStepCs))
                            {
                                write.Write(insertMethodFromStep);
                                write.Close();
                                Thread.Sleep(500);
                            }
                        }

                        using (StreamReader readPage = new StreamReader(getPageCs))
                        {
                            string scriptCode = readPage.ReadToEnd();
                            string UpToLastParan = scriptCode.Substring(0, scriptCode.LastIndexOf("}"));
                            int SecondToLast = UpToLastParan.LastIndexOf("}");
                            string UpToSecondToLastParan = scriptCode.Substring(0, SecondToLast);
                            textScript = textScript.Remove(textScript.LastIndexOf('}') + 0).Replace("bool \"", "bool ").Replace("\"()", "()");
                            string insertMethodComplete = UpToSecondToLastParan + "\n" + textScript + "\n" + scriptCode.Substring(SecondToLast, scriptCode.Length - SecondToLast);
                            readPage.Close();
                            Thread.Sleep(250);
                            using (StreamWriter write = new StreamWriter(getPageCs))
                            {
                                write.Write(insertMethodComplete);
                                write.Close();
                                Thread.Sleep(250);
                            }
                        }
                    }
                    catch (Exception) { }
                }
            }
        }

        private string createStepDefinition(string str)
        {
            string getMethodListName = listMethods[count];
            string getStepAnalyzer = Analyzer(str);
            string removeTypeStep = "@" + '"' + getStepAnalyzer.Substring(getStepAnalyzer.IndexOf(' ') + 1);
            removeTypeStep = removeTypeStep + '"';
            string splitGetFinalAnalyzer = getStepAnalyzer.Replace("'(.*)'", " ").Replace("(.*)", " ").Replace("  ", "");
            var words = splitGetFinalAnalyzer.Split(new[] { "_", " " }, StringSplitOptions.RemoveEmptyEntries);
            words = words
                .Select(word => char.ToUpper(word[0]) + word.Substring(1))
                .ToArray();
            string joinWords = string.Join(string.Empty, words);
            string methodStep = null;
            int RowFormat = getStepAnalyzer.Count(ch => ch.Equals("(.*)"));
            switch (getStepAnalyzer)
            {
                case string s when s.StartsWith("Given") || s.StartsWith("Dado"):
                    return methodStep = "[Given(" + removeTypeStep + ")]" + "\n" +
                       "public void " + joinWords + "(" + setParameter + ")" + "\n{\nAssert.IsTrue(startBrowser(value));\n}";
                case string s when s.StartsWith("When") || s.StartsWith("Quando"):
                    count++;
                    return methodStep = "[When(" + removeTypeStep + ")]" + "\n" +
                        getReturnMethodStep(joinWords, setParameter, getMethodListName);
                case string s when s.StartsWith("And") || s.StartsWith("E "):
                    count++;
                    return methodStep = "[When(" + removeTypeStep + ")]" + "\n" +
                        getReturnMethodStep(joinWords, setParameter, getMethodListName);
                case string s when s.StartsWith("But") || s.StartsWith("Mas"):
                    count++;
                    return methodStep = "[When(" + removeTypeStep + ")]" + "\n" +
                        getReturnMethodStep(joinWords, setParameter, getMethodListName);
                case string s when s.StartsWith("Entao"):
                    count++;
                    return methodStep = "[Then(" + removeTypeStep + ")]" + "\n" +
                        getReturnMethodStep(joinWords, setParameter, getMethodListName);
                case string s when s.StartsWith("Então"):
                    count++;
                    return methodStep = "[Then(" + removeTypeStep + ")]" + "\n" +
                        getReturnMethodStep(joinWords, setParameter, getMethodListName);
                case string s when s.StartsWith("Then"):
                    count++;
                    return methodStep = "[Then(" + removeTypeStep + ")]" + "\n" +
                        getReturnMethodStep(joinWords, setParameter, getMethodListName);
            }
            return methodStep;
        }

        public string getReturnMethodStep(string joinWords, string parameter, string getMethodListName)
        {
            if (parameter.Equals("Table table"))
            {
                return "public void " + joinWords + "(" + parameter + ")" +
                    "\n{" +
                    "\nforeach(TableRow row in table.Rows)\n{" +
                    "\nforeach(string value in row.Values)\n{" +
                    "\nAssert.IsTrue(" + getMethodListName + ");\n}" +
                    "\n}" +
                    "\n}";
            }
            else
            {
                return "public void " + joinWords + "(" + parameter + ")" + "\n{\nAssert.IsTrue(" + getMethodListName + ");\n}";
            }
        }

        public string Analyzer(string stepText)
        {
            string getDiferentCaracter = null;
            if (stepText.Contains("<"))
            {
                stepText = stepText.Replace("<", "'").Replace(">", "'");
                getDiferentCaracter = stepText;
            }
            var paramMatches = RecognizeQuotedTexts(stepText)
                .OrderBy(m => m.Index).ThenByDescending(m => m.Length);
            int countParameter = RecognizeQuotedTexts(stepText).Count();
            int countParameterInt = RecognizeIntegers(stepText).Count();
            setParameter = countParameter < 2 && countParameter > 0 ? "string value" :
            countParameter >= 2 ? "string value0, string value1" : countParameterInt < 2 && countParameterInt > 0 ? "string value" :
            countParameterInt >= 2 ? "string value0, string value1" : previewStepName.Trim().StartsWith("|") ? "Table table" : "";
            //: countParameterInt < 2 && countParameterInt > 0 ? "int value" :
            //countParameterInt >= 2 ? "int value0, int value1" : "";
            //var isNum = Regex.IsMatch(setParameter, @"\d");
            //if (isNum == true) { setParameter = "int value"; }
            int textIndex = 0;
            List<string> TextParts = new List<string>();
            foreach (var paramMatch in paramMatches)
            {
                if (paramMatch.Index < textIndex)
                    continue;

                TextParts.Add(stepText.Substring(textIndex, paramMatch.Index - textIndex));
                textIndex = paramMatch.Index + paramMatch.Length;
            }
            string getResult = null;
            string getSecondParam = null;
            for (int pos = 0; pos < TextParts.Count(); pos++)
            {
                if (pos == 0)
                {
                    if (getDiferentCaracter != null)
                    {
                        getDiferentCaracter = TextParts[0].Replace("'", "(.*)");
                        getResult = getDiferentCaracter;
                    }
                    else
                    {
                        getFirstParam = TextParts[0].Replace("'", "'(.*)'");
                        getResult = getFirstParam;
                    }
                }
                if (pos == 1)
                {
                    if (getDiferentCaracter != null)
                    {
                        getFirstParam = TextParts[0].Replace("'", "(.*)");
                        getDiferentCaracter = TextParts[1].Replace("'", "(.*)");
                        getResult = getFirstParam + getDiferentCaracter;
                        getResult = getResult.Replace("'(.*)'(.*)", "(.*)").Replace("(.*)(.*)", "(.*)");
                    }
                    else
                    {
                        getSecondParam = TextParts[1].Replace("'", "'(.*)'");
                        if (string.IsNullOrEmpty(getSecondParam) || getSecondParam.StartsWith(" "))
                        {
                            getSecondParam = getSecondParam.Insert(0, "'(.*)'");
                            getResult = getFirstParam + getSecondParam;
                            getResult = getResult.Replace("'(.*)''(.*)'", "'(.*)'");
                        }
                    }
                }
                //if (pos == 2)
                //{
                //    if (getDiferentCaracter != null)
                //    {
                //        getFirstParam = TextParts[0].Replace("'", "(.*)");
                //        getDiferentCaracter = TextParts[2].Replace("'", "(.*)");
                //        getResult = getFirstParam + getDiferentCaracter;
                //        getResult = getResult.Replace("'(.*)'(.*)", "(.*)").Replace("(.*)(.*)", "(.*)");
                //    }
                //    else
                //    {
                //        getSecondParam = TextParts[2].Replace("'", "'(.*)'");
                //        if (string.IsNullOrEmpty(getSecondParam) || getSecondParam.StartsWith(" "))
                //        {
                //            getSecondParam = getSecondParam.Insert(0, "'(.*)'");
                //            getResult = getFirstParam + getSecondParam;
                //            getResult = getResult.Replace("'(.*)''(.*)'", "'(.*)'");
                //        }
                //    }
                //}
                else { }
            }
            if (string.IsNullOrEmpty(getResult))
            {
                getResult = stepText;
            }
            return getResult;
        }

        public Regex quotesRe = new Regex(@"""+(?<param>.*?)""+|'+(?<param>.*?)'+|(?<param>\<.*?\>)");
        public IEnumerable<Capture> RecognizeQuotedTexts(string stepText)
        {
            return quotesRe.Matches(stepText).Cast<Match>().Select(m => (Capture)m.Groups["param"]);
        }

        public readonly Regex intRe = new Regex(@"-?\d+");
        public IEnumerable<Capture> RecognizeIntegers(string stepText)
        {
            return intRe.Matches(stepText).Cast<Capture>();
        }

        private void RemoveCaractersXml()
        {
            XDocument doc = XDocument.Load(filePathXml);
            doc.DescendantNodes().OfType<XComment>().Remove();
            doc.Save(filePathXml);
            Thread.Sleep(200);
        }

        private void InsertFeatureSeparator()
        {
            XDocument doc = XDocument.Load(filePathXml);
            doc.Root.Elements().GroupBy(x => ((string)x.Attribute("key")).Split('.')[0]).ToList()
                .ForEach(elementsGroupByKey =>
                {
                    elementsGroupByKey.Last().AddAfterSelf(new XComment(" - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - "));
                });
            doc.Save(filePathXml);
        }

        private void UpdateXml()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(filePathXml);
            IEnumerator ie = xml.SelectNodes("element").GetEnumerator();
            while (ie.MoveNext())
            {
                if ((ie.Current as XmlNode).Attributes["key"].Value == keyAttribute)
                {

                    //condition cypress
                    if (getSupportPage != null)
                    {
                        (ie.Current as XmlNode).Attributes["value"].Value = valueAttribute;
                        (ie.Current as XmlNode).Attributes["baseValue"].Value = baseAttribute;

                    }
                    else
                    {
                        (ie.Current as XmlNode).Attributes["by"].Value = byAttribute;
                        (ie.Current as XmlNode).Attributes["value"].Value = valueAttribute;
                        (ie.Current as XmlNode).Attributes["baseValue"].Value = (ie.Current as XmlNode).Attributes["baseValue"].Value;
                    }
                }
            }
            Thread.Sleep(250);
            xml.Save(filePathXml);
        }

        private void InsertXml()
        {
            removeElementDuplicatedXml();
            Thread.Sleep(200);
            XmlDocument xml = new XmlDocument();
            xml.Load(filePathXml);
            XmlNode root = xml.DocumentElement;
            XmlElement element = xml.CreateElement("element");
            XmlAttribute elementKey = xml.CreateAttribute("key");
            elementKey.Value = keyAttribute;
            element.Attributes.Append(elementKey);
            if (getSupportPage == null)
            {
                XmlAttribute byKey = xml.CreateAttribute("by");
                byKey.Value = byAttribute;
                element.Attributes.Append(byKey);
            }
            else { }
            XmlAttribute valueKey = xml.CreateAttribute("value");
            valueKey.Value = valueAttribute;
            element.Attributes.Append(valueKey);
            if (getSupportPage == null)
            {
                XmlAttribute baseValueKey = xml.CreateAttribute("baseValue");
                baseValueKey.Value = "\"\"";
                element.Attributes.Append(baseValueKey);
            }
            else
            {
                XmlAttribute baseValueKey = xml.CreateAttribute("baseValue");
                baseValueKey.Value = baseAttribute;
                element.Attributes.Append(baseValueKey);
            }
            root.InsertAfter(element, root.LastChild);
            Thread.Sleep(250);
            xml.Save(filePathXml);
        }

        private void removeElementDuplicatedXml()
        {
            //    var namesToRemove = new[]
            //{
            //        keyAttribute,
            //    };
            XDocument doc = XDocument.Load(filePathXml);
            doc.Root.Elements()
                .Where(x => keyAttribute.Contains((string)x.Attribute("key")))
                .Remove();
            Thread.Sleep(100);
            doc.Save(filePathXml);
            Thread.Sleep(200);
        }

        private void SortElementsXml()
        {
            XDocument doc = XDocument.Load(filePathXml);
            XElement ele = XElement.Load(filePathXml);
            var root = doc.Root;
            XElement result = new XElement("locators");
            var sorted =
                ele.Elements("element").OrderBy(s => (string)s.Attribute("key"));
            root.ReplaceAll(sorted);
            doc.Save(filePathXml);
        }

        private void replaceObject()
        {
            List<string> list = File.ReadAllLines(filePathXml).Where(arg => !string.IsNullOrWhiteSpace(arg)).ToList();
            List<string> listReplace = list.Select(x => x.Replace("&quot;", ""))
                .ToList();
            File.WriteAllLines(filePathXml, listReplace);
            Thread.Sleep(250);
        }

        private void addPlugins()
        {
            XDocument xdoc = XDocument.Load(getCsproj);
            xdoc.Descendants("PackageReference")
                .Where(x => x.Attribute("Include") != null)
                .Where(x => x.Attribute("Include").Value.Equals("Super_Automation"))
                .ToList()
                .ForEach(x => x.Remove());
            xdoc.Save(getCsproj);
            XmlDocument doc = new XmlDocument();
            doc.Load(getCsproj);
            XmlDocumentFragment docFrag = doc.CreateDocumentFragment();
            docFrag.InnerXml = "<ItemGroup> </ItemGroup>";
            doc.DocumentElement.AppendChild(docFrag);
            XmlNodeList nosItemGroup = doc.GetElementsByTagName("ItemGroup");
            foreach (XmlNode no in nosItemGroup)
            {

                List<string> plugins = new List<string>();
                plugins.Add("NUnit");
                plugins.Add("NUnit3TestAdapter");
                plugins.Add("ExtentReports");
                plugins.Add("Super_Automation");
                plugins.Add("MSTest.TestAdapter");
                plugins.Add("Microsoft.NET.Test.Sdk");
                plugins.Add("Specflow");
                plugins.Add("SpecFlow.NUnit");
                plugins.Add("SpecFlow.Tools.MsBuild.Generation");
                plugins.Add("DotNetSeleniumExtras.WaitHelpers");
                foreach (string item in plugins)
                {

                    XmlNode newNode = doc.CreateElement("PackageReference", no.NamespaceURI);
                    XmlAttribute newAtrib = doc.CreateAttribute("Include");
                    newAtrib.Value = item;
                    newNode.Attributes.Append(newAtrib);
                    XmlAttribute version = doc.CreateAttribute("Version");
                    version.Value = "*";
                    newNode.Attributes.Append(version);
                    no.AppendChild(newNode);
                }
                doc.Save(getCsproj);
            }
            XDocument docc = XDocument.Load(getCsproj);
            docc.Descendants("PackageReference").GroupBy(g => (string)g.Attribute("Include"))
                .SelectMany(g => g.Skip(1)).Remove();
            docc.Save(getCsproj);
            Thread.Sleep(200);
            List<string> list = File.ReadAllLines(getCsproj).Where(arg => !string.IsNullOrWhiteSpace(arg)).ToList();
            list.RemoveAll(x => x.Equals("  <ItemGroup />"));
            Thread.Sleep(200);
            File.WriteAllLines(getCsproj, list);
        }
    }
}


