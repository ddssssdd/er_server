using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ExpenseReportServer.Models
{
    
    [Table("Section")]
    public class Section
    {
        [Key]
        public int Id { get; set; }
        public String ClassName { get; set; }
        public String Title { get; set; }
        public String Icon { get; set; }
        [ForeignKey("section_id")]
        public virtual List<CellData> Items { get; set; }
    }
    [Table("CellData")]
    public class CellData
    {
        [Key]
        public int Id { get; set; }
        public int section_id { get; set; }
        public String PropertyName { get; set; }
        [NotMapped]
        public int Index { get; set; }
        [NotMapped]
        public String Title { get; set; }
        [NotMapped]
        public String Detail { get; set; }

    }
    public class DisplayFactory<T>
    {
        private LocalDatabase db = new LocalDatabase();
        private T _instance;
        public List<Section> sections;
        public DisplayFactory(T instance)
        {
            _instance = instance;
            if (_instance != null)
            {
                loadSettings();
            }
        }
        protected void loadSettings()
        {
            String className = _instance.GetType().Name;
            sections = db.Sections.Where(section => section.ClassName.ToLower().Equals(className.ToLower())).ToList();
            sections.ForEach(processSection);
        }
        protected void processSection(Section section)
        {
            index = 0;
            section.Items.ForEach(processData);
        }
        protected int index;
        protected void processData(CellData data)
        {
            try
            {
                PropertyInfo property = _instance.GetType().GetProperty(data.PropertyName);
                Object value = property.GetValue(_instance);
                data.Title = data.PropertyName;
                data.Detail = value.ToString();
                data.Index = index++;
            }
            catch (Exception e)
            { 
                //nothing for now.
            }
           
        }
    
    }
}