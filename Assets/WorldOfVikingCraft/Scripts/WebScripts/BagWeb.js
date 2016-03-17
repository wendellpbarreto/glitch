/*
Ultimate MMORPG Kit - WebScripts/BagWeb - Client's script - skeletarik@gmail.com - 2013
This script is for updating	bag's contents. It sends the information to the "bag.php" script.
*/

var formText = ""; 	//This field is where the messages sent by PHP script will be in
var formEmail;		//Field for email
var formName;		//Field for character's name
var formBag;		//Field for character's bag (contents)

var URL = "http://www.warofdeath.com/MMORPG/bag.php"; 	//Change for your URL
var hash = "hashcode"; 									//Change your secret code, and remember to change into the PHP file too

function GetData(data : String){						//Receives the necessary information about character
	var dataS = data.Split('^'[0]);
	formEmail = dataS[0];
	formName = dataS[1];
	formBag = dataS[2];
	
	BagWeb();
}

function BagWeb() {

    var form = new WWWForm(); 							//New form connection
    form.AddField( "myform_hash", hash ); 				//Add your hash code to the field myform_hash, check that this variable name is the same as in PHP file
    form.AddField( "myform_email", formEmail );
   	form.AddField( "myform_name", formName );
   	form.AddField( "myform_bag", formBag );
   	
    var w = WWW(URL, form); 							//Creation a var called 'w' and we sync with our URL and the form

    yield w; 											//Waiting for the form to check the PHP file, so our game don't just hang

    if (w.error != null) {
        Debug.LogError("Ultimate MMORPG Kit error! Web Problems: "+w.error); 		//If there is an error
    } else {
        formText = w.text; 		//Returning the data our PHP told us. It is useful for debug
        w.Dispose(); 			//Clear our form in game
    }
}