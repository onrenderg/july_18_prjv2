var expChk1 = new Array("SCRIPT ", " SCRIPT", " SCRIPT ", "INSERT ", " INSERT", " INSERT ", "DELETE ", " DELETE", " DELETE ", "DROP ", " DROP", " DROP ", "UPDATE ", " UPDATE", "IFRAME ", " IFRAME", " IFRAME ", "FRAME ", " FRAME", " FRAME ", " <", "< ", "<", " < ", " >", "> ", ">", " > ", "SRC ", " SRC", " SRC ", "SRC");
var errMsg = new String();

function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}
function AllowNumbers() {
    var ch = String.fromCharCode(event.keyCode);
    var filter = /[0-9]/;
    if (!filter.test(ch)) {
        event.returnValue = false;
        return false;
    }
    else {
        return true;
    }
}
function AllowNumberswithslash() {
    var ch = String.fromCharCode(event.keyCode);
    var filter = /[0-9/]/;
    if (!filter.test(ch)) {
        event.returnValue = false;
        return false;
    }
    else { return true; }
}
function AllowNumberswithdot() {
    var ch = String.fromCharCode(event.keyCode);
    var filter = /[0-9.]/;
    if (!filter.test(ch)) {
        event.returnValue = false;
        return false;
    }
    else {
        return true;
    }
}
function AllowNumberswithchar() {
    var ch = String.fromCharCode(event.keyCode);
    var filter = /[a-zA-Z0-9]/;
    if (!filter.test(ch)) {
        event.returnValue = false;
        return false;
    }
    else { return true; }
}

function AllowNumberswithchardESH() {
    var ch = String.fromCharCode(event.keyCode);
    var filter = /[a-zA-Z0-9-_]/;
    if (!filter.test(ch)) {
        event.returnValue = false;
        return false;
    }
    else { return true; }
}

function ValidateControlsInvalidChar() {
    var Valid = true;
    try {
        oInputs = document.getElementsByTagName('input');
        var id = "";
        var i = 0;

        while (i < oInputs.length) {
            if (oInputs[i].type == 'text') {
                id = oInputs[i];
                var strChk = id.value;
                ////Reserved Words    
                if (valkeywords(id.value, id)) {
                    alert('Invalid keywords in the field' + id.value);
                    id.focus();
                    Valid = false;
                    break;
                }
                ////reserved characters   
                //var chkStr = /^[0-9०-९\p{L}\p{M}' \s \,\.\:\;\-\+\%\*\@\(\)\/\?_]+$/
                var chkStr = /[\-0-9A-Za-zª²-³µ¹-º¼-¾À-ÖØ-öø-ƺƼ-ƿǄǆ-Ǉǉ-Ǌǌ-Ǳǳ-ʓʕ-ʯͰ-ͳͶ-ͷͻ-ͽΆΈ-ΊΌΎ-ΡΣ-ϵϷ-ҁ\@\,\|\।\.\;\:\-\%\*\(\)\?\+/\\u0488-ԣԱ-Ֆա-և֊־٠-٩\u06de۰-۹߀-߉\u0900-ॾ\u0982-\u0983\u09be-\u09c0\u09c7-\u09c8\u09cb-\u09cc\u09d7০-৯৴-৹\u0a03\u0a3e-\u0a40੦-੯\u0a83\u0abe-\u0ac0\u0ac9\u0acb-\u0acc૦-૯\u0b02-\u0b03\u0b3e\u0b40\u0b47-\u0b48\u0b4b-\u0b4c\u0b57୦-୯\u0bbe-\u0bbf\u0bc1-\u0bc2\u0bc6-\u0bc8\u0bca-\u0bcc\u0bd7௦-௲\u0c01-\u0c03\u0c41-\u0c44౦-౯౸-౾\u0c82-\u0c83\u0cbe\u0cc0-\u0cc4\u0cc7-\u0cc8\u0cca-\u0ccb\u0cd5-\u0cd6೦-೯\u0d02-\u0d03\u0d3e-\u0d40\u0d46-\u0d48\u0d4a-\u0d4c\u0d57൦-൵\u0d82-\u0d83\u0dcf-\u0dd1\u0dd8-\u0ddf\u0df2-\u0df3๐-๙໐-໙༠-༳\u0f3e-\u0f3f\u0f7f\u102b-\u102c\u1031\u1038\u103b-\u103c၀-၉\u1056-\u1057\u1062-\u1064\u1067-\u106d\u1083-\u1084\u1087-\u108c\u108f-႙Ⴀ-Ⴥ፩-፼\u16ee-\u16f0\u17b6\u17be-\u17c5\u17c7-\u17c8០-៩៰-៹᠆᠐-᠙\u1923-\u1926\u1929-\u192b\u1930-\u1931\u1933-\u1938᥆-᥏\u19b0-\u19c0\u19c8-\u19c9᧐-᧙\u1a19-\u1a1b\u1b04\u1b35\u1b3b\u1b3d-\u1b41\u1b43-\u1b44᭐-᭙\u1b82\u1ba1\u1ba6-\u1ba7\u1baa᮰-᮹\u1c24-\u1c2b\u1c34-\u1c35᱀-᱉᱐-᱙ᴀ-ᴫᵢ-ᵷᵹ-ᶚḀ-ἕἘ-Ἕἠ-ὅὈ-Ὅὐ-ὗὙὛὝὟ-ώᾀ-ᾇᾐ-ᾗᾠ-ᾧᾰ-ᾴᾶ-Άιῂ-ῄῆ-Ήῐ-ΐῖ-Ίῠ-Ῥῲ-ῴῶ-Ώ‐-―⁰-ⁱ⁴-⁹ⁿ-₉\u20dd-\u20e0\u20e2-\u20e4ℂℇℊ-ℓℕℙ-ℝℤΩℨK-ℭℯ-ℴℹℼ-ℿⅅ-ⅉⅎ⅓-\u2188①-⒛⓪-⓿❶-➓Ⰰ-Ⱞⰰ-ⱞⱠ-Ɐⱱ-ⱼⲀ-ⳤ⳽ⴀ-ⴥ⸗⸚\u3007〜\u3021-\u3029〰\u3038-\u303a゠㆒-㆕㈠-㈩㉑-㉟㊀-㊉㊱-㊿꘠-꘩Ꙁ-ꙟꙢ-ꙭ\ua670-\ua672Ꚁ-ꚗꜢ-ꝯꝱ-ꞇꞋ-ꞌ\ua823-\ua824\ua827\ua880-\ua881\ua8b4-\ua8c3꣐-꣙꤀-꤉\ua952-\ua953\uaa2f-\uaa30\uaa33-\uaa34\uaa4d꩐-꩙ﬀ-ﬆﬓ-ﬗ︱-︲﹘﹣－０-９Ａ-Ｚａ-ｚ]|\ud800[\udd07-\udd33\udd40-\udd78\udd8a\udf20-\udf23\udf41\udf4a\udfd1-\udfd5]|\ud801[\udc00-\udc4f\udca0-\udca9]|\ud802[\udd16-\udd19\ude40-\ude47]|\ud809[\udc00-\udc62]|\ud834[\udd65-\udd66\udd6d-\udd72\udf60-\udf71]|\ud835[\udc00-\udc54\udc56-\udc9c\udc9e-\udc9f\udca2\udca5-\udca6\udca9-\udcac\udcae-\udcb9\udcbb\udcbd-\udcc3\udcc5-\udd05\udd07-\udd0a\udd0d-\udd14\udd16-\udd1c\udd1e-\udd39\udd3b-\udd3e\udd40-\udd44\udd46\udd4a-\udd50\udd52-\udea5\udea8-\udec0\udec2-\udeda\udedc-\udefa\udefc-\udf14\udf16-\udf34\udf36-\udf4e\udf50-\udf6e\udf70-\udf88\udf8a-\udfa8\udfaa-\udfc2\udfc4-\udfcb\udfce-\udfff]/
                if (strChk != "") {
                    //if (strChk.match(chkStr)) {
                    if (!strChk.match(chkStr)) {
                        alert('Invalid characters in the field ' + id.value);
                        id.focus();
                        Valid = false;
                        break;
                    }
                }
            }
            i++;
        }
    }
    catch (err) {
    }
    if (Valid) {
        return true
    }
    else { return false; }
}


function valkeywords(strChk, fieldName) {
    //Reserved Words

    for (i = 0; i < expChk1.length; i++) {
        var chk = new RegExp(expChk1[i]);
        if (strChk.toUpperCase().match(chk)) {
            errMsg = "Use synonym of word " + expChk1[i] + " .";
            return true;
        }
    }
    return false;
}


function EncriptNewPass() {
  
    var userPass = document.getElementById("ContentPlaceHolder1_txtUserPassword").value;
    alert("ContentPlaceHolder1_txtUserPassword: " + userPass);
    if (userPass == "") {
        alert('Please Enter User Password');
        document.getElementById("ContentPlaceHolder1_txtUserPassword").focus();
        return false;
    }
    else if (document.getElementById("ContentPlaceHolder1_txtUserPasswordC").value == "") {
        alert('Please Enter Confirm Password');
        document.getElementById("ContentPlaceHolder1_txtUserPasswordC").focus();
        return false;
    }
    else if (userPass != document.getElementById("ContentPlaceHolder1_txtUserPasswordC").value) {
        alert('Confirm Password Mismatch');
        document.getElementById("ContentPlaceHolder1_txtUserPasswordC").focus();
        return false;
    }
    else {
        document.getElementById("ContentPlaceHolder1_txtUserPassword").value = "Hacking Not Allowed.";
        document.getElementById("ContentPlaceHolder1_txtUserPasswordC").value = "Hacking Not Allowed.";
        document.getElementById("ContentPlaceHolder1_hid1").value = SHA256(userPass);
     
        alert("ContentPlaceHolder1_hid1: "+document.getElementById("ContentPlaceHolder1_hid1").value);
    }
   
}


