function getTable(tablename,divTable,divClass,id) {
    
    $.getJSON('/api/db/columns', "tablename=" + tablename+"&id="+id, function (json, a2, a3) {
        if (a2 == "success") {
            
            $(divTable).empty();
            var class_define = "[Table(\"" + tablename + "\")]<br/>";
            class_define += "public class " + tablename + "<br/>";
            class_define += "{<br/>";

            for (var i = 0; i < json.length; i++) {
                var item = json[i];
                var tr = "<tr><td>" + i + "</td><td>" + item.Name + "</td><td>" + item.Type + "</td><td>" + item.Length + "</td></tr>";
                $(tr).appendTo(divTable);
                if (item.Name.indexOf(" ") < 0) {
                    class_define += "   public " + getType(item.Type) + " " + item.Name + " {get;set;}<br/>";
                } else {
                    class_define += "   [Column(\"" + item.Name + "\")]<br/>";
                    class_define += "   public " + getType(item.Type) + " " + item.Name.replace(/ /g,"_") + " {get;set;}<br/>";
                }
            }
            class_define += "}<br/>";
            $(divClass).html(class_define);
        }

    });
}
function getType(type) {
    if (type == null) {
        return "Unknown";
    }
    var dict = {
        "bit": "bool",
        "smallint": "short",
        "tinyint": "byte",
        "int": "int?",
        "numeric": "Decimal",
        "decimal": "Decimal",
        "float": "float",
        "real": "Double",
        "money":"Decimal",
        "datetime": "DateTime?",
        "nvarchar": "String",
        "uniqueidentifier": "String",
        "varchar": "String",
        "char": "String",
        "text": "String",
        "nchar": "String",
    };
    return dict[type.toLowerCase()];

}