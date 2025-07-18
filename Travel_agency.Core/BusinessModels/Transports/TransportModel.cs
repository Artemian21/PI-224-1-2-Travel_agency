﻿namespace Travel_agency.Core.BusinessModels.Transports;

public class TransportModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Type { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public DateTime DepartureDate { get; set; }
    public DateTime ArrivalDate { get; set; }
    public decimal Price { get; set; }
}