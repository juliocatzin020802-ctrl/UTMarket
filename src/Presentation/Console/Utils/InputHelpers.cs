using System;
using System.Globalization;

namespace UTMarket.Presentation.Console.Utils;

/// <summary>
/// Helper methods for capturing and validating user input from the console.
/// Implemented based on prompt 15_Ejercicio_03.txt.
/// </summary>
public static class InputHelpers
{
    /// <summary>
    /// Prompts the user for a date and validates the input. Retries until a valid date is entered.
    /// </summary>
    /// <param name="prompt">The message to display to the user.</param>
    /// <param name="format">Optional: The expected date format (e.g., "yyyy-MM-dd"). If null, uses culture-invariant. </param>
    /// <returns>A valid DateTime object.</returns>
    public static DateTime ReadDate(string prompt, string? format = null)
    {
        DateTime date;
        while (true)
        {
            System.Console.Write($"{prompt} (Formato: yyyy-MM-dd): ");
            string? input = System.Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                System.Console.WriteLine("Error: La fecha no puede estar vacía. Intente de nuevo.");
                continue;
            }

            if (format != null)
            {
                if (DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    return date;
                }
            }
            else
            {
                if (DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    return date;
                }
            }
            System.Console.WriteLine($"Error: Formato de fecha inválido. Por favor, use el formato 'yyyy-MM-dd' o un formato reconocido por su cultura.");
        }
    }

    /// <summary>
    /// Prompts the user for a date range (start and end dates) and validates their logical consistency.
    /// </summary>
    /// <returns>A tuple containing the valid start and end dates.</returns>
    public static (DateTime StartDate, DateTime EndDate) ReadDateRange()
    {
        DateTime startDate;
        DateTime endDate;

        while (true)
        {
            startDate = ReadDate("Fecha de Inicio");
            endDate = ReadDate("Fecha de Fin");

            if (endDate < startDate)
            {
                System.Console.WriteLine("Error: La Fecha de Fin no puede ser anterior a la Fecha de Inicio. Intente de nuevo.");
            }
            else
            {
                return (startDate, endDate);
            }
        }
    }
}
