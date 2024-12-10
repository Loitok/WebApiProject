﻿namespace BLL.DTOs
{
    public class LocationDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string BusinessId { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public string D365LocationNumber { get; set; }
        public string ClientLocationNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string ProvinceOrState { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Attention { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public bool IsArchived { get; set; }
        public int ClientId { get; set; }
        public ClientDto Client { get; }
    }
}
