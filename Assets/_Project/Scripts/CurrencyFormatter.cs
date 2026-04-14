using UnityEngine;

public static class CurrencyFormatter
{
    private static readonly string[] Suffixes = { "", "K", "M", "B", "T" };

    public static string Format(double value)
    {
        if (value < 1000) return value.ToString("F0");

        int index = 0;
        double temp = value;

        while (temp >= 1000)
        {
            temp /= 1000;
            index++;
        }

        if (index < Suffixes.Length)
        {
            // K, M, B, T formatı: Her zaman 1 basamak (Örn: 320.5K)
            return temp.ToString("F1") + Suffixes[index];
        }
        else
        {
            // AA, AB, AC... formatı
            int aaIndex = index - Suffixes.Length;
            char first = (char)('A' + (aaIndex / 26));
            char second = (char)('A' + (aaIndex % 26));
            return temp.ToString("F1") + first + second;
        }
    }
}