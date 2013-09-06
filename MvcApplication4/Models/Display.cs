using System;
using System.Collections;
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
        public int? GroupIndex { get; set; }
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
        public String Title { get; set; }
        public int? CellIndex { get; set; }
        
        [NotMapped]
        public String Detail { get; set; }

    }
    public class DisplayFactory<T>
    {
        private LocalDatabase db = new LocalDatabase();
        private T _instance;
        private List<Section> _sections;
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
            _sections = db.Sections.Where(section => section.ClassName.ToLower().Equals(className.ToLower())).ToList();
            _sections.ForEach(processSection);
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
                String pName = data.PropertyName;
                if (pName.Contains(" "))
                {
                    pName = pName.Replace(" ", "_");
                }
                PropertyInfo property = _instance.GetType().GetProperty(pName);
                Object value = property.GetValue(_instance);
                data.Title = data.PropertyName;
                data.Detail = value.ToString();
                data.CellIndex = index++;
            }
            catch (Exception e)
            { 
                //nothing for now.
            }
           
        }
        public List<Section> sections {
            get {
                return _sections;
            }
        }
        public List<CellData> items
        {
            get
            {
                if (_sections != null)
                {
                    return _sections.First().Items;
                }
                else
                {
                    return new List<CellData>();
                }
            }
        }
    
    }
    public class DisplayListFactory<T>
    {
        private IList _list;
        private Func<T, String> _title = null;
        private Func<T, String> _detail = null;
        private Func<T,String> _detailUrl =null;
        public DisplayListFactory(List<T> list) {
            _list = new ArrayList();
            processData(list);
        }
       
        public DisplayListFactory(List<T> list,Func<T,String> title,Func<T,String> detail,Func<T,String> url=null)
        {
            _list = new ArrayList();
            _title = title;
            _detail = detail;
            _detailUrl = url;
            processData(list);
        }
        private void processData(List<T> list)
        {
           
            list.ForEach(delegate(T item) {
                
                if (_title!=null && _detail!=null){
                    _list.Add(new {Title = _title(item),Detail=_detail(item),Url= _detailUrl!=null?_detailUrl(item):""});
                }else
                {
                    var items =new DisplayFactory<T>(item).sections;
                    if (items!=null && items.Count()>0){
                        _list.Add(items[0]);
                    }
                }
                
            });
        }
        public IList items
        {
            get
            {
                return _list;
            }
        }
    }
}