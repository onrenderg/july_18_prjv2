// JScript File
function emptyTxt() {
    document.getElementById("txtUserID").value = "";
    document.getElementById("txtUserPass").value = "";

}

function hashPassword() {
   
    var sMessage = document.getElementById("txtUserPass").value;
    //alert(sMessage);
    if (sMessage.length <= 15) {
        var randomnumber = document.getElementById("hidHash").value;
        if (randomnumber == "") {
            randomnumber = Math.floor(100000 + Math.random() * 900000);
            document.getElementById("hidHash").value = randomnumber;
        }
        var mypass1 = SHA256(sMessage);//MD5(sMessage);
       // alert(mypass1 + "---hash: " + document.getElementById("hidHash").value);
        var mypass = SHA256(mypass1.toUpperCase() + randomnumber.toString()); //MD5(mypass1.toUpperCase() + randomnumber.toString());
        document.getElementById("txtUserPass").value = mypass;
        document.getElementById("hidHash").value = randomnumber;
        // alert(document.getElementById("txtUserPass").value + "---hash: " +document.getElementById("hidHash").value);
    }
}
function hashPassword(seed) {
   
    var sMessage = document.getElementById("txtUserPass").value;
    if (sMessage.length < 8) {
        alert('Min 8 characters are required');
    }
    else if (sMessage.length > 15) {
        alert('Max 15 characters are allowed');
    }
    else if (sMessage.length <= 15) {
        var randomnumber = seed;
       
        document.getElementById("hidHash").value = seed;
        if (randomnumber == "") {
         
            randomnumber = Math.floor(100000 + Math.random() * 900000);
          
            document.getElementById("hidHash").value = randomnumber;
           
        }
       // alert(sMessage);
        var mypass1 = SHA256(sMessage);//MD5(sMessage);
       // alert(mypass1);
        var mypass = SHA256(mypass1.toUpperCase() + document.getElementById("hidHash").value); //MD5(mypass1.toUpperCase() + randomnumber.toString());
        document.getElementById("txtUserPass").value = mypass1 + document.getElementById("hidHash").value;
        //document.getElementById("txtUserPass").value = mypass;
        document.getElementById("hidHash").value = randomnumber;
    }
   
}
function md5AuthUser(seed, operation) {
    if (operation == "add")
    { var userID = document.getElementById("txtUserID").value; }
    else
    { var userID = document.getElementById("ddlUserID").value; }
    var userPass = document.getElementById("txtUserPass").value;
    document.getElementById("hidHash").value = SHA256(userPass); //MD5(userPass);
    //document.getElementById("hidHash").value=MD5(userID+userPass);
}


function md5Authuserpass() {
    var sprypassword11 = new Spry.Widget.ValidationPassword("sprypassword1", { maxChars: 15, minChars: 8, minAlphaChars: 2, minNumbers: 2, minSpecialChars: 1 });
    var spryconf1 = new Spry.Widget.ValidationConfirm("spryconfirm", "sprypassword1", { validateOn: ['blur'] });

}
function md5Authuserpass2() {
    var sprypassword11 = new Spry.Widget.ValidationPassword("sprypassword11", { maxChars: 15, minChars: 8, minAlphaChars: 1, minNumbers: 1, minSpecialChars: 1 });
    var spryconf1 = new Spry.Widget.ValidationConfirm("spryconfirm1", "sprypassword11", { validateOn: ['blur'] });

}
function md5Authuserpass13() {
    var sMessage = document.getElementById("TxtRPassword").value;
    if (sMessage.length < 8 || sMessage.length > 15) {
        alert('Min 1 Number, min 1 Alphabet char, min 1 Special char, min 8 chars / max 15 chars required.');
        return;
    }
    var sMessageConfirm = document.getElementById("TxtRREPassword").value;
    if (!(sMessage == sMessageConfirm)) {
        alert('Passwords Don\'t Match!');
        return;
    }
    var mypass1 = SHA256(sMessage); //MD5(sMessage);
    var mypassC = SHA256(sMessageConfirm); //MD5(sMessageConfirm);
    document.getElementById("hidHashchangenew0").value = mypass1;
    document.getElementById("hidHashchangenew1").value = mypassC;
    document.getElementById("TxtRPassword").value = mypass1;
    document.getElementById("TxtRREPassword").value = mypassC;

}
function md5Authnewuserpass() {
    var sMessage = document.getElementById("TxtCUPassword").value;

    if (sMessage.length < 8 || sMessage.length > 15) {
        alert('Min 1 Number, min 1 Alphabet char, min 1 Special char, min 8 chars / max 15 chars required.');
        return;
    }
    var sMessageConfirm = document.getElementById("TxtCUREPassword").value;
    if (!(sMessage == sMessageConfirm)) {
        alert('Passwords Don\'t Match!!');
        return;
    }
    var mypass1 = SHA256(sMessage); //MD5(sMessage);
    var mypassC = SHA256(sMessageConfirm); //MD5(sMessageConfirm);
    document.getElementById("TxtCUPassword").value = mypass1;
    document.getElementById("TxtCUREPassword").value = mypassC;
    document.getElementById("HiddenField1").value = mypass1;
    document.getElementById("HiddenField2").value = mypassC;
}
function md5Authuserpass01() {
   
    var sMessageOld = document.getElementById("Passwordold").value;
   
    var sMessage = document.getElementById("password11").value;
    if (sMessage.length < 8 || sMessage.length > 15) {
        alert('Min 1 Number, min 1 Alphabet char, min 1 Special char, min 8 chars / max 15 chars required.');
        return;
    }
    var sMessageConfirm = document.getElementById("confirm").value;
    if (!(sMessage == sMessageConfirm)) {
        alert('Passwords Don\'t Match!!');
        return;
    }
   
    var randomnumber = document.getElementById("hidhash").value;
    var mypass1 = SHA256(sMessage); //MD5(sMessage);
    var mypassC = SHA256(sMessageConfirm); //MD5(sMessageConfirm);
    var mypass0 = SHA256(sMessageOld); //MD5(sMessageOld);
    var mypass01 = SHA256(mypass0.toUpperCase() + randomnumber.toString()); //MD5(mypass0.toUpperCase() + randomnumber.toString());
    document.getElementById("password11").value = mypass1;
    document.getElementById("confirm").value = mypassC;
    document.getElementById("Passwordold").value = mypass01;
    document.getElementById("hidHashchangenew").value = mypass1;
    document.getElementById("hidHashchangenewretype").value = mypassC;
    document.getElementById("hidhashold").value = mypass01;

}
function md5AuthuserpassUP01() {
    var sMessageOld = document.getElementById("ctl00_ContentPlaceHolder2_Passwordold").value;
    var sMessage = document.getElementById("ctl00_ContentPlaceHolder2_password11").value;
    if (sMessage.length < 8 || sMessage.length > 15) {
        alert('Min 1 Number, min 1 Alphabet char, min 1 Special char, min 8 chars / max 15 chars required.');
        return false;
    }
    var sMessageConfirm = document.getElementById("ctl00_ContentPlaceHolder2_confirm").value;
    if (!(sMessage == sMessageConfirm)) {
        alert('Passwords Don\'t Match!!');
        return false;
    }

    var randomnumber = document.getElementById("ctl00_ContentPlaceHolder2_hidhash").value;
    var mypass1 = SHA256(sMessage); //MD5(sMessage);
    var mypassC = SHA256(sMessageConfirm); //MD5(sMessageConfirm);
    var mypass0 = SHA256(sMessageOld); //MD5(sMessageOld);
    var mypass01 = SHA256(mypass0.toUpperCase() + randomnumber.toString()); //MD5(mypass0.toUpperCase() + randomnumber.toString());

   // alert(mypass0.toUpperCase() + " , randomnumber: " + '----------' + randomnumber.toString());
    document.getElementById("ctl00_ContentPlaceHolder2_password11").value = mypass1;
    document.getElementById("ctl00_ContentPlaceHolder2_confirm").value = mypassC;
    document.getElementById("ctl00_ContentPlaceHolder2_Passwordold").value = mypass01;
    document.getElementById("ctl00_ContentPlaceHolder2_hidHashchangenew").value = mypass1;
    document.getElementById("ctl00_ContentPlaceHolder2_hidHashchangenewretype").value = mypassC;
    document.getElementById("ctl00_ContentPlaceHolder2_hidhashold").value = mypass01;
    return true;
}
function md5AuthuserpassUpAdmin01() {
    var sMessageOld = document.getElementById("ctl00_Passwordold").value;
    var sMessage = document.getElementById("ctl00_password11").value;
    if (sMessage.length < 8 || sMessage.length > 15) {
        alert('Min 1 Number, min 1 Alphabet char, min 1 Special char, min 8 chars / max 15 chars required.');
        return false;
    }
    var sMessageConfirm = document.getElementById("ctl00_confirm").value;
    if (!(sMessage == sMessageConfirm)) {
        alert('Passwords Don\'t Match!!');
        return false;
    }

    var randomnumber = document.getElementById("ctl00_hidhash").value;
    var mypass1 = SHA256(sMessage); //MD5(sMessage);
    var mypassC = SHA256(sMessageConfirm); //MD5(sMessageConfirm);
    var mypass0 = SHA256(sMessageOld); //MD5(sMessageOld);
    var mypass01 = SHA256(mypass0.toUpperCase() + randomnumber.toString()); //MD5(mypass0.toUpperCase() + randomnumber.toString());

   // alert(mypass0.toUpperCase() + randomnumber + '----------' + mypass01);
    document.getElementById("ctl00_password11").value = mypass1;
    document.getElementById("ctl00_confirm").value = mypassC;
    document.getElementById("ctl00_Passwordold").value = mypass01;
    document.getElementById("ctl00_hidHashchangenew").value = mypass1;
    document.getElementById("ctl00_hidHashchangenewretype").value = mypassC;
    document.getElementById("ctl00_hidhashold").value = mypass01;
    return true;
}
function md5Authuserpass02() {
   
    var sMessageOld = document.getElementById("ctl00_ContentPlaceHolder2_Passwordold").value;
  
    var sMessage = document.getElementById("ctl00_ContentPlaceHolder2_password11").value;
    if (sMessage.length < 8 || sMessage.length > 15) {
        alert('Min 1 Number, min 1 Alphabet char, min 1 Special char, min 8 chars / max 15 chars required.');
        return;
    }
    var sMessageConfirm = document.getElementById("ctl00_ContentPlaceHolder2_confirm").value;
   
    if (!(sMessage == sMessageConfirm)) {
        alert('Passwords Don\'t Match!!');
        return;
    }
   
    var randomnumber = document.getElementById("ctl00_ContentPlaceHolder2_hidhash").value;
   
    var mypass1 = SHA256(sMessage); //MD5(sMessage);
    var mypassC = SHA256(sMessageConfirm); //MD5(sMessageConfirm);
    var mypass0 = SHA256(sMessageOld); //MD5(sMessageOld);
    var mypass01 = SHA256(mypass0.toUpperCase() + randomnumber.toString()); //MD5(mypass0.toUpperCase() + randomnumber.toString());
   
    // alert(mypass0.toUpperCase() + " , Randomnumber: " + '----------' + randomnumber.toString());
   
    document.getElementById("ctl00_ContentPlaceHolder2_password11").value = mypass1;
    document.getElementById("ctl00_ContentPlaceHolder2_confirm").value = mypassC;
    document.getElementById("ctl00_ContentPlaceHolder2_Passwordold").value = mypass01;
    document.getElementById("ctl00_ContentPlaceHolder2_hidHashchangenew").value = mypass1;
    document.getElementById("ctl00_ContentPlaceHolder2_hidHashchangenewretype").value = mypassC;
    document.getElementById("ctl00_ContentPlaceHolder2_hidhashold").value = mypass01;

}

