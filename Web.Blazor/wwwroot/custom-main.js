SystemSettings = {
    setMapHtmlCode: function () {
        var mapSrc = "";
        var mapCodeField = document.getElementById("map-html-code");
        var mapFrameField = document.getElementById("map-frame-field");
        if (mapCodeField) {
            mapSrc = mapCodeField.getAttribute("src");
            if (mapFrameField)
                mapFrameField.setAttribute("src", mapSrc); 
        }
    }
}