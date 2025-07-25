

var transliterationControl;
function googleTransliterate(ids) {
    
    google.load("elements", "1", {
        packages: "transliteration"
    });

    function onLoad() {
       // var address = strRootPath + "DefaultDataEntry.aspx/getKeyboardType";
        var options = {
            sourceLanguage:
            google.elements.transliteration.LanguageCode.ENGLISH,
            destinationLanguage:
            google.elements.transliteration.LanguageCode.HINDI,

            shortcutKey: 'ctrl+g',
            transliterationEnabled: true
        };
        transliterationControl = new google.elements.transliteration.TransliterationControl(options);
        if (ids.length > 0) {
            transliterationControl.makeTransliteratable(ids);
        }
        //$.ajax({
        //    type: "POST",
        //    contentType: "application/json; charset=utf-8",
        //    url: address,
        //    dataType: "json",
        //    success: function (data, textStatus, jQxhr) {
        //        changeKeyboard(JSON.parse(data.d)["status"]);
        //    },
        //   // error: function (textStatus, errorThrown) { alert("Google Transliteration1 Status: " + textStatus.meaasge); alert("Error: " + errorThrown.message); }
        //});      
       
    }
    // here you make the first init when page load
    google.setOnLoadCallback(onLoad);
    // here we make the handlers for after the UpdatePanel update
    try{
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
    } catch (e) {
    }

    function InitializeRequest(sender, args) {
    }

// this is called to re-init the google after update panel updates.
    function EndRequest(sender, args) {
        onLoad();
    }
}



