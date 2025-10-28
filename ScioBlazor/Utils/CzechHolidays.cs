using System;
using System.Collections.Generic;

namespace ScioBlazor.Utils;

public static class CzechHolidays
{
    public static HashSet<DateTime> GetForYear(int year)
    {
        var set = new HashSet<DateTime>();
        void Add(int m, int d) => set.Add(new DateTime(year, m, d));

        // Fixed-date public holidays
        Add(1, 1);   // New Year / Day of Restoration of the Czech State
        Add(5, 1);   // Labour Day
        Add(5, 8);   // Liberation Day
        Add(7, 5);   // Saints Cyril and Methodius Day
        Add(7, 6);   // Jan Hus Day
        Add(9, 28);  // St. Wenceslas Day
        Add(10, 28); // Independent Czechoslovak State Day
        Add(11, 17); // Struggle for Freedom and Democracy Day
        Add(12, 24); // Christmas Eve
        Add(12, 25); // Christmas Day
        Add(12, 26); // St. Stephen's Day

        // Movable holidays: Good Friday and Easter Monday
        var easter = ComputeEasterSunday(year);
        set.Add(easter.AddDays(-2)); // Good Friday
        set.Add(easter.AddDays(1));  // Easter Monday

        return set;
    }

    // Meeus/Jones/Butcher algorithm for Gregorian Easter Sunday
    private static DateTime ComputeEasterSunday(int y)
    {
        int a = y % 19;
        int b = y / 100;
        int c = y % 100;
        int d = b / 4;
        int e = b % 4;
        int f = (b + 8) / 25;
        int g = (b - f + 1) / 3;
        int h = (19 * a + b - d - g + 15) % 30;
        int i = c / 4;
        int k = c % 4;
        int l = (32 + 2 * e + 2 * i - h - k) % 7;
        int m = (a + 11 * h + 22 * l) / 451;
        int month = (h + l - 7 * m + 114) / 31;
        int day = ((h + l - 7 * m + 114) % 31) + 1;
        return new DateTime(y, month, day);
    }
}

