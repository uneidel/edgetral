//var baseurl = "http://localhost:5000";
var baseurl = "http://127.0.0.1:8080";

$(document).ready(function() {
    $.ajax({
        type:"GET",
        url: baseurl + "/data"
    }).done(function(data) {

        $.each(data, function(index,value){
            var gd = CreateIndexedArray(value.sensorValues);
            $(".container").append("<div class='box'><div id='chart" +index +"'></div>");
            var yaxisname = value.filter.substring(value.filter.indexOf(".")+1);
            var minyaxis = Math.min.apply(null, value.sensorValues);
            var maxyaxis = Math.max.apply(null, value.sensorValues)

            GenericDrawChart(value.sensorName, yaxisname,minyaxis, maxyaxis, gd, "chart" + index);
        });      
    }).fail(function(data){
        alert(data.statusText);
    });
});

function CreateIndexedArray(data){
    var gd=[];
    for (var i =0; i< data.length; i++){
        gd.push([i, data[i]]);
    };
    return gd;
}
function GenericDrawChart(xaxislabel, yaxislabel,minyaxis, maxyaxis, data, classname){
   
    
      var plot1 = $.jqplot(classname, [data], {  
          series:[{showMarker:false}],
          axes:{
            xaxis:{
              label:xaxislabel
              
            },
            yaxis:{
              label:yaxislabel,
              min:minyaxis,
              max: maxyaxis
            }
          }
      });
}
    
Array.prototype.max = function(){
    return Math.max.apply(null, this);
}
Array.prototype.min = function(){
    return Math.min.apply(null, this);
}