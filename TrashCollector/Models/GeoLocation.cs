public class GeoLocation
{
    public Info info { get; set; }
    public Options options { get; set; }
    public Result[] results { get; set; }
}

public class Info
{
    public int statuscode { get; set; }
    public Copyright copyright { get; set; }
    public object[] messages { get; set; }
}

public class Copyright
{
    public string text { get; set; }
    public string imageUrl { get; set; }
    public string imageAltText { get; set; }
}

public class Options
{
    public int maxResults { get; set; }
    public bool thumbMaps { get; set; }
    public bool ignoreLatLngInput { get; set; }
}

public class Result
{
    public Providedlocation providedLocation { get; set; }
    public Location[] locations { get; set; }
}

public class Providedlocation
{
    public string location { get; set; }
}

public class Location
{
    public string street { get; set; }
    public string adminArea6 { get; set; }
    public string adminArea6Type { get; set; }
    public string adminArea5 { get; set; }
    public string adminArea5Type { get; set; }
    public string adminArea4 { get; set; }
    public string adminArea4Type { get; set; }
    public string adminArea3 { get; set; }
    public string adminArea3Type { get; set; }
    public string adminArea1 { get; set; }
    public string adminArea1Type { get; set; }
    public string postalCode { get; set; }
    public string geocodeQualityCode { get; set; }
    public string geocodeQuality { get; set; }
    public bool dragPoint { get; set; }
    public string sideOfStreet { get; set; }
    public string linkId { get; set; }
    public string unknownInput { get; set; }
    public string type { get; set; }
    public Latlng latLng { get; set; }
    public Displaylatlng displayLatLng { get; set; }
    public string mapUrl { get; set; }
}

public class Latlng
{
    public float lat { get; set; }
    public float lng { get; set; }
}

public class Displaylatlng
{
    public float lat { get; set; }
    public float lng { get; set; }
}