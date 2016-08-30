using Google.Maps;
using Google.Maps.Geocoding;

namespace PersonFinder.Geo
{
    public static class GeoData
    {
        public static Position GetPosition(string address)
        {
            var request = new GeocodingRequest
            {
                Address = address,
                Sensor = false
            };

            var response = new GeocodingService().GetResponse(request);

            if(response.Status == ServiceResponseStatus.ZeroResults)
                return new Position();

            return new Position
            {
                Latitude = response.Results[0].Geometry.Location.Latitude,
                Longitude = response.Results[0].Geometry.Location.Longitude
            };
        }
    }
}