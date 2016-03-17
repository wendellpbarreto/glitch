﻿/*
Ultimate MMORPG Kit - WebScripts/SignUp - Client's script - skeletarik@gmail.com - 2013
This script is for signing up. It sends the information to the "signUp.php" script.
*/

var formText = ""; 				//This field is where the messages sent by PHP script will be in
var formEmail;					//Field for email
var formPassword;				//Field for password

var URL = "http://www.mywebsite/signUp.php"; 			//Change for your URL
var hash = "hashcode"; 									//Change your secret code, and remember to change into the PHP file too

function GetData(data : String){						//Receives the necessary information about character
	var dataS = data.Split(':'[0]);
	formEmail = dataS[0];
	formPassword = dataS[1];
	SignUp();
}

function SignUp() {
    var form = new WWWForm(); 							//New form connection
    form.AddField( "myform_hash", hash ); 				//Add your hash code to the field myform_hash, check that this variable name is the same as in PHP file
    form.AddField( "myform_email", formEmail );
    form.AddField( "myform_pass", formPassword );
    var w = WWW(URL, form); 							//Creating a var called 'w' and we sync with our URL and the form
    yield w; 											//Waiting for the form to check the PHP file, so our game dont just hang

    if (w.error != null) {
    	Debug.LogError("Ultimate MMORPG Kit error! Web Problems: "+w.error);
    } else {
        formText = w.text; 								//Returning the data our PHP told us. It is useful for debug 
		GameObject.Find("Code").GetComponent("MainMenuVik").SendMessage("AnswerSignUp", formText);
        w.Dispose(); 									//Clear our form in game
    }
}