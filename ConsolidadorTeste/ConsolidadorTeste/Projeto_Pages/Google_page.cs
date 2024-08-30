using Hook_Validator;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using TechTalk.SpecFlow;

namespace ConsolidadorTeste.Projeto_Pages
{
    class Google_page : Tools
    {
        private static string keyAttribute { get; set; }

        private static string byAttribute { get; set; }

        private static string valueAttribute { get; set; }
        public static string getCorrection { get; set; }
        private static string filePath = System.IO.Directory.GetParent(System.IO.Directory.GetParent(System.IO.Directory.GetParent(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)).FullName).FullName).FullName + @"\Projeto_Locators\";
        private static string filePathTxt = System.IO.Directory.GetParent(System.IO.Directory.GetParent(System.IO.Directory.GetParent(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)).FullName).FullName).FullName + @"\Teste.txt";
        private static By newByValue { get; set; }

        public bool startBrowser(string url)
        {
            string filePath2 = System.IO.Directory.GetParent(System.IO.Directory.GetParent(System.IO.Directory.GetParent(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)).FullName).FullName).FullName + @"\Projeto_Locators\PageObjects.xml";
            XmlDocument xmlUpdate = new XmlDocument();
            xmlUpdate.Load(filePath2);
            XmlNode roott = xmlUpdate.DocumentElement;
            IEnumerator iee = roott.SelectNodes("element").GetEnumerator();
            string getCorrection = "div#mount_0_0_nu > div > div > div";
            string escaped = System.Security.SecurityElement.Escape(getCorrection);
            string unescaped = System.Net.WebUtility.HtmlDecode(escaped);
            while (iee.MoveNext())
            {
                if ((iee.Current as XmlNode).Attributes["key"].Value == "Google.btnPesquisa")
                {
                    (iee.Current as XmlNode).Attributes["by"].Value = "id";
                    (iee.Current as XmlNode).Attributes["value"].Value = unescaped;
                    (iee.Current as XmlNode).Attributes["value"].Value = unescaped;
                }
            }
            xmlUpdate.Save(filePath2);
            filePath2 = System.IO.Directory.GetParent(System.IO.Directory.GetParent(System.IO.Directory.GetParent(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)).FullName).FullName).FullName + @"\Projeto_Locators\PageObjects.xml";
            xmlUpdate = new XmlDocument();
            xmlUpdate.Load(filePath2);
            while (iee.MoveNext())
            {
                if ((iee.Current as XmlNode).Attributes["key"].Value == "Google.btnPesquisa")
                {
                    (iee.Current as XmlNode).Attributes["value"].Value.Replace("&gt;", ">");
                }
            }

            List<string> list = File.ReadAllLines(filePath2).Where(arg => !string.IsNullOrWhiteSpace(arg)).ToList();
            List<string> listReplace = list.Select(x => x.Replace("&gt;", ">")
            .Replace("&lt", "<")
            .Replace("&apos;", "'")
            .Replace("&quot;", "\"")
            .Replace("&amp;", "&"))
                .ToList();
            File.WriteAllLines(filePath2, listReplace);

            OpenPage(url);

            return true;
        }
        
        public bool inserirNome(string value)
        {
            WaitElement("Google.btnGoogle");
            return true;
        }

        public static bool WaitElements(string locator, int timeout = 15)
        {
            By byLocator = GetLocatorTeste(locator);
            IWebElement element = Selenium.driver.FindElement(byLocator);
            int count = 0;
            do
            {
                try
                {
                    return element.Displayed && element.Enabled;
                }
                catch (Exception)
                {
                    Thread.Sleep(500);
                    count++;
                }

            } while (count < timeout * 2);

            return false;
        }

        public static IWebElement WaitElementt(string locator, int timeout = 10)
        {
            By byLocator = GetLocatorTeste(locator);
            IWebElement element = Selenium.driver.FindElement(byLocator);
            WaitElementSelfCorrection(element, 15);
            return element;
        }


        public static By GetLocatorTeste(string locator)
        {
            string featureName = FeatureContext.Current.FeatureInfo.Title;
            foreach (string xmlName in Directory.GetFiles(filePath, "*.xml", SearchOption.TopDirectoryOnly)
                .Select(Path
                .GetFileName)
                .ToArray())
            {
                List<XmlNode> xmlNodeList = new List<XmlNode>();
                string[] filePaths = Directory.GetFiles(filePath);
                for (int i = 0, j = filePaths.Length; i < j; i++)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(filePaths[i]);
                    XmlNode rootNode = doc.SelectSingleNode("locators");
                    foreach (XmlNode node in rootNode.SelectNodes("element"))
                        xmlNodeList.Add(node);
                }
                keyAttribute = null;
                valueAttribute = null;
                byAttribute = null;
                foreach (XmlNode node in xmlNodeList)
                {
                    var teste = node.Attributes["key"].Value.ToString().Trim();
                    if (node.Attributes["key"].Value.ToString().Trim().Equals(locator))
                    {
                        keyAttribute = node.Attributes["key"].Value.ToString().Trim();
                        valueAttribute = node.Attributes["value"].Value.ToString().Trim();
                        byAttribute = node.Attributes["by"].Value.ToString().Trim();
                        break;
                    }
                }
            }
            By b = null;
            if (valueAttribute != null)
            {

                switch (byAttribute)
                {
                    case "id":
                        b = By.Id(valueAttribute);
                        break;
                    case "class":
                        b = By.ClassName(valueAttribute);
                        break;
                    case "css":
                        b = By.CssSelector(valueAttribute);
                        break;
                    case "xpath":
                        b = By.XPath(valueAttribute);
                        break;
                    case "name":
                        b = By.Name(valueAttribute);
                        break;
                    case "tag":
                        b = By.TagName(valueAttribute);
                        break;
                    case "link":
                        b = By.LinkText(valueAttribute);
                        break;
                    default:
                        break;
                }
            }

            try
            {
                IWebElement element = Selenium.driver.FindElement(b);
                IntelligentUpdateXml(locator, element);
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                IntelligentUpdateXml(locator);
            }
            if (b == null) throw new NotFoundException("Não foi localizado o locator ( " + locator + " )");
            b = newByValue;
            return b;
        }

        private static bool WaitElementSelfCorrection(IWebElement element, int timeout)
        {
            int count = 0;
            do
            {
                try
                {
                    return element.Displayed && element.Enabled;
                }
                catch (Exception)
                {
                    Thread.Sleep(500);
                    count++;
                }
            } while (count < timeout * 2);

            return false;
        }

        private static void IntelligentUpdateXml(string locator)
        {
            foreach (string xmlName in Directory.GetFiles(filePath, "*.xml", SearchOption.TopDirectoryOnly)
              .Select(Path
              .GetFileName)
              .ToArray())
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(filePath + xmlName);
                XmlNode root = xml.DocumentElement;
                IEnumerator ie = root.SelectNodes("element").GetEnumerator();
                while (ie.MoveNext())
                {
                    if ((ie.Current as XmlNode).Attributes["key"].Value == locator)
                    {
                        getCorrection = (ie.Current as XmlNode).Attributes["baseValue"].Value;
                    }
                }
                string idElement = getElementId();
                string cssElement = getElementCssSelector();
                string xpathElement = getElementRelativeXpath();
                string by = null, value = null;

                if (string.IsNullOrEmpty(idElement))
                {
                    if (cssElement.Length < xpathElement.Length)
                    {
                        by = "css";
                        value = cssElement;
                        newByValue = By.CssSelector(value);
                    }
                    else
                    {
                        by = "xpath";
                        value = xpathElement;
                        newByValue = By.XPath(value);
                    }
                }
                else
                {
                    by = "id";
                    value = idElement;
                    newByValue = By.Id(value);
                }
                XmlDocument xmlUpdate = new XmlDocument();
                xmlUpdate.Load(filePath + xmlName);
                XmlNode roott = xmlUpdate.DocumentElement;
                IEnumerator iee = roott.SelectNodes("element").GetEnumerator();
                while (iee.MoveNext())
                {
                    if ((iee.Current as XmlNode).Attributes["key"].Value == locator)
                    {
                        (iee.Current as XmlNode).Attributes["by"].Value = by;
                        (iee.Current as XmlNode).Attributes["value"].Value = value;
                    }
                }
                Thread.Sleep(250);
                xmlUpdate.Save(HttpUtility.HtmlDecode(filePath + xmlName));
            }
        }

        private static void IntelligentUpdateXml(string locator, IWebElement element)
        {
            foreach (string xmlName in Directory.GetFiles(filePath, "*.xml", SearchOption.TopDirectoryOnly)
                .Select(Path
                .GetFileName)
                .ToArray())
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(filePath + xmlName);
                XmlNode root = xml.DocumentElement;
                IEnumerator ie = root.SelectNodes("element").GetEnumerator();
                while (ie.MoveNext())
                {
                    if ((ie.Current as XmlNode).Attributes["key"].Value == locator)
                    {
                        if (string.IsNullOrEmpty((ie.Current as XmlNode).Attributes["baseValue"].Value))
                        {
                            getCorrection = getElementAbsoluteXpath(element);
                        }
                        else
                        {
                            getCorrection = (ie.Current as XmlNode).Attributes["baseValue"].Value;
                        }
                    }
                }
                xml.Save(filePath + xmlName);
                string idElement = getElementId();
                string cssElement = getElementCssSelector();
                string xpathElement = getElementRelativeXpath();
                string by = null, value = null;

                if (string.IsNullOrEmpty(idElement))
                {
                    if (cssElement.Length < xpathElement.Length)
                    {
                        by = "css";
                        value = cssElement;
                        newByValue = By.CssSelector(value);
                    }
                    else
                    {
                        by = "xpath";
                        value = xpathElement;
                        newByValue = By.XPath(value);
                    }
                }
                else
                {
                    by = "id";
                    value = idElement;
                    newByValue = By.Id(value);
                }
                XmlDocument xmlUpdate = new XmlDocument();
                xmlUpdate.Load(filePath + xmlName);
                XmlNode roott = xmlUpdate.DocumentElement;
                IEnumerator iee = roott.SelectNodes("element").GetEnumerator();
                while (iee.MoveNext())
                {
                    if ((iee.Current as XmlNode).Attributes["key"].Value == locator)
                    {
                        (iee.Current as XmlNode).Attributes["by"].Value = by;
                        (iee.Current as XmlNode).Attributes["value"].Value = value;
                        (iee.Current as XmlNode).Attributes["baseValue"].Value = getCorrection;
                    }
                }
                Thread.Sleep(250);
                xmlUpdate.Save(HttpUtility.HtmlDecode(filePath + xmlName));
            }
        }

        public static string getElementAbsoluteXpath(IWebElement element)
        {
            IJavaScriptExecutor jsExec = Selenium.driver as IJavaScriptExecutor;
            string _result = (string)jsExec.ExecuteScript(
    @"
function getPathTo(element) {
    if (element === document.body)
        return '/html/' + element.tagName.toLowerCase();

    var ix = 0;
    var siblings = element.parentNode.childNodes;
    for (var i = 0; i < siblings.length; i++) {
        var sibling = siblings[i];
        if (sibling === element)
        {
            return getPathTo(element.parentNode) + '/' + element.tagName.toLowerCase() + '[' + (ix + 1) + ']';
        }
        if (sibling.nodeType === 1 && sibling.tagName === element.tagName)
            ix++;
    }
}
var element = arguments[0];
var xpath = '';
xpath = getPathTo(element);
return xpath;
", element);
            return _result;
        }

        public static string getElementId()
        {
            IWebElement webElement = Selenium.driver.FindElement(By.XPath(getCorrection));
            IJavaScriptExecutor jsExec = Selenium.driver as IJavaScriptExecutor;
            string _result = QuoteLocator((string)jsExec.ExecuteScript(
    @"
var ELEMENT_NODE = 1;
function getId(element) {
         var selector = ''; 
         if (element instanceof Element && element.nodeType === ELEMENT_NODE && element.id) {
             selector = element.id;
         }
         return selector;
     }
var element = arguments[0];
var id = '';
id = getId(element);
return id;
", webElement));
            return _result;
        }

        //        public static string getElementCssSelector()
        //        {
        //            IWebElement webElement = Selenium.driver.FindElement(By.XPath(getCorrection));
        //            IJavaScriptExecutor jsExec = Selenium.driver as IJavaScriptExecutor;
        //            string _result = QuoteLocator((string)jsExec.ExecuteScript(
        //    @"
        //var ELEMENT_NODE = 1;
        //function getCss(element) {
        //         if (!(element instanceof Element))
        //             return;
        //         var path = [];
        //         while (element.nodeType === ELEMENT_NODE) {
        //             var selector = element.nodeName.toLowerCase();
        //             if (element.id) {
        //                 if (element.id.indexOf('-') > -1) {
        //                     selector += '[id = ""' + element.id + '""]';
        //                 } else {
        //                     selector += '#' + element.id;
        //                 }
        //                 path.unshift(selector);
        //                 break;
        //             } else {
        //                 var element_sibling = element;
        //                 var sibling_cnt = 1;
        //                 while (element_sibling = element_sibling.previousElementSibling) {
        //                     if (element_sibling.nodeName.toLowerCase() == selector)
        //                         sibling_cnt++;
        //                 }
        //                 if (sibling_cnt != 1)
        //                     selector += ':nth-of-type(' + sibling_cnt + ')';
        //             }
        //             path.unshift(selector);
        //             element = element.parentNode;
        //         }
        //         return path.join(' > ');
        //}
        //var element = arguments[0];
        //var css = '';
        //css = getCss(element);
        //return css;
        //", webElement));
        //            return _result;
        //        }
        public static string getElementCssSelector()
        {
            IWebElement webElement = Selenium.driver.FindElement(By.XPath(getCorrection));
            IJavaScriptExecutor jsExec = Selenium.driver as IJavaScriptExecutor;
            string _result = QuoteLocator((string)jsExec.ExecuteScript(
    @"
function getCss(element) {
         return `${element.nodeName}${element.id ? '#'+element.id : ''}${element.getAttribute('class') ? '.'+element.getAttribute('class').split(' ').join('.') : ''}`;
}
var element = arguments[0];
var css = '';
css = getCss(element);
return css;
", webElement));
            return _result;
        }

        //        public static string getElementRelativeXpath()
        //        {
        //            IWebElement webElement = Selenium.driver.FindElement(By.XPath(getCorrection));
        //            IJavaScriptExecutor jsExec = Selenium.driver as IJavaScriptExecutor;
        //            string _result = QuoteLocator((string)jsExec.ExecuteScript(
        //    @"
        //var ELEMENT_NODE = 1;
        //function getRelativeXpath(element) {
        //    var element_sibling, siblingTagName, siblings, cnt, sibling_count;
        //        var elementTagName = element.tagName.toLowerCase();
        //        if (element.id != '') {
        //            return 'id(""' + element.id + '"")';
        //            // alternative : 
        //            // return '*[@id=""' + element.id + '""]';
        //        } else if (element.name && document.getElementsByName(element.name).length === 1) {
        //            return '//' + elementTagName + '[@name=""' + element.name + '""]';
        //        }
        //        if (element === document.body) {
        //            return '/html/' + elementTagName;
        //        }
        //        sibling_count = 0;
        //        siblings = element.parentNode.childNodes;
        //        siblings_length = siblings.length;
        //        for (cnt = 0; cnt < siblings_length; cnt++) {
        //            var element_sibling = siblings[cnt];
        //            if (element_sibling.nodeType !== ELEMENT_NODE) { // not ELEMENT_NODE
        //                continue;
        //            }
        //            if (element_sibling === element) {
        //                return getPathTo(element.parentNode) + '/' + elementTagName + '[' + (sibling_count + 1) + ']';
        //            }
        //            if (element_sibling.nodeType === 1 && element_sibling.tagName.toLowerCase() === elementTagName) {
        //                sibling_count++;
        //            }
        //        }
        //    }
        //var element = arguments[0];
        //var xpath = '';
        //xpath = getRelativeXpath(element);
        //return xpath;
        //", webElement));
        //            return _result;
        //        }

        public static string getElementRelativeXpath()
        {
            IWebElement webElement = Selenium.driver.FindElement(By.XPath(getCorrection));
            IJavaScriptExecutor jsExec = Selenium.driver as IJavaScriptExecutor;
            string _result = QuoteLocator((string)jsExec.ExecuteScript(
    @"
function getXPath(element){
    if(element.hasAttribute(""id"")){
        return '//' + element.tagName.toLowerCase() + '[@id=""' + element.id + '""]';
    }

    if(element.hasAttribute(""class"")){
        return '//' + element.tagName.toLowerCase() + '[@class=""' + element.getAttribute(""class"") + '""]';
    }

    if(element.hasAttribute(""name"")){
        return '//' + element.tagName.toLowerCase() + '[@name=""' + element.getAttribute(""name"") + '""]';
    }

    var old = '/' + element.tagName.toLowerCase();
    var new_path = this.xpath(element.parentNode) + old;

    return new_path;
}
var element = arguments[0];
var css = '';
css = getXPath(element);
return css;
", webElement));
            return _result;
        }

     
        public static string QuoteLocator(string locator)
        {
            locator = locator.Replace("\\", "'").Replace("\"", "'").Replace("''", "'").Trim('\'');
            locator = locator.Replace("{", "{{");
            locator = locator.Replace("}", "}}");
            locator = locator.Replace("/\" /> ", "\" /> ").Trim();
            return locator;
        }
    }
}

