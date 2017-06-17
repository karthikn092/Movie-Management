using System;

namespace MovieManagement.Models
{
    [Serializable]
    public class CheckBoxModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public int Value { get; set; }
    }
}