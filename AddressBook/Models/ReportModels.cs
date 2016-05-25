﻿using System.ComponentModel.DataAnnotations;

namespace AddressBook.Models
{
  public class PhoneList
  {
    public int PersonId { get; set; }

    [Display(ResourceType = typeof (Resources), Name = "PhoneList_PersonName")]
    public string PersonName { get; set; }

    public int PhoneId { get; set; }

    [Display(ResourceType = typeof(Resources), Name = "Phone_PhoneNumber")]
    public string PersonPhone { get; set; }
  }

  public class PhoneCount
  {
    public int PersonId { get; set; }

    [Display(ResourceType = typeof (Resources), Name = "PhoneList_PersonName")]
    public string PersonName { get; set; }

    [Display(Name = "Телефоны")]
    public int Phones { get; set; }
  }
}