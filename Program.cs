using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;

class Program
{
static void Main(string[] args)
{
// Inicializar el navegador Edge
EdgeOptions edgeOptions = new EdgeOptions();

    // Crear una instancia del controlador de Edge WebDriver
    IWebDriver driver = new EdgeDriver(edgeOptions);

    // Abrir el sitio web de Google
    driver.Navigate().GoToUrl("https://www.google.com");

    // Encontrar el campo de búsqueda por su nombre y escribir una consulta
    IWebElement searchBox = driver.FindElement(By.Name("q"));
    searchBox.SendKeys("Pruebas automatizadas con Selenium");

    // Volver a buscar el botón de búsqueda antes de hacer clic en él
    IWebElement searchButton = null;
    int retries = 0;
    while (searchButton == null && retries < 10)
    {
        try
        {
            searchButton = driver.FindElement(By.CssSelector("input[type='submit'][value='Buscar con Google']"));
            if (!searchButton.Displayed || !searchButton.Enabled)
            {
                searchButton = null;
            }
        }
        catch (StaleElementReferenceException)
        {
            // Elemento se volvió obsoleto, volver a intentar buscarlo
            retries++;
        }
    }

    // Si se encontró el botón, hacer clic en él
    if (searchButton != null)
    {
        searchButton.Click();
    }

    // Esperar un momento para que carguen los resultados
    System.Threading.Thread.Sleep(3000);

    // Obtener el título de la página actual
    string pageTitle = driver.Title;

    // Verificar si el título contiene la consulta de búsqueda
    if (pageTitle.Contains("Pruebas automatizadas con Selenium"))
    {
        // La prueba se completó con éxito
        Console.WriteLine("Prueba exitosa: El título contiene la consulta de búsqueda.");

        // Crear un archivo HTML que muestre el resultado de la prueba
        string html = @"
        <html>
        <head>
            <title>Resultado de la prueba</title>
        </head>
        <body>
            <h1>Prueba automatizada con Selenium</h1>
            <p>La prueba se completó con éxito.</p>
        </body>
        </html>";

        // Guardar el archivo HTML
        File.WriteAllText("resultado_de_la_prueba.html", html);

        // Abrir el archivo HTML en el navegador web predeterminado
        var uri = "resultado_de_la_prueba.html";
        var psi = new System.Diagnostics.ProcessStartInfo();
        psi.UseShellExecute = true;
        psi.FileName = uri;
        System.Diagnostics.Process.Start(psi);
    }
    else
    {
        // La prueba falló
        Console.WriteLine("Prueba fallida: El título no coincide con la consulta de búsqueda.");
    }
    
}
}