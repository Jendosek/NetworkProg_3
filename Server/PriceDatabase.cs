public static class PriceDatabase
{
    private static int ssdPrice = 100;
    private static int hddPrice = 50;
    private static int ramPrice = 30;
    private static int cpuPrice = 200;
    
    private static string ssd = "SSD";
    private static string hdd = "HDD";
    private static string ram = "RAM";
    private static string cpu = "CPU";
    
    public static string GetPrice(string part)
    {
        string lowerPart = part.ToLower();

        if (lowerPart == "ssd")
            return $"Ціна {ssd}: ${ssdPrice}";
        if (lowerPart == "hdd")
            return $"Ціна {hdd}: ${hddPrice}";
        if (lowerPart == "ram")
            return $"Ціна {ram}: ${ramPrice}";
        if (lowerPart == "cpu")
            return $"Ціна {cpu}: ${cpuPrice}";
        return $"Запчастину {part} не знайдено.";
    }
}