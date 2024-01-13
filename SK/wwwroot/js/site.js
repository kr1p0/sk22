
"use strict";

var clickedUpperTableRowGlobal;
var clickedLowerTableRowGlobal;
var changes = 0;
var connection = new signalR.HubConnectionBuilder().withUrl("/NotificationHub").build();

//input and placeholder onload
document.addEventListener("DOMContentLoaded", function () {
    kr1p0styleInput();
    displayAlert();
 
    connection.start();
    connection.on("ReceiveMessage", function (message) {
        changes++;
        let notiContainer = document.getElementById("changesNotiContainer")
        let noti = document.getElementById("changesNoti")
        notiContainer.classList.remove("changesNotiContainerCls")
        notiContainer.classList.add("changesNotiContainerClsRed")
        noti.innerText = changes > 9 ? "!" : changes;
    });



    /*
   $(window).resize(function () {
       if ($(window).width() < 600) {
           document.getElementById("tableButtonDelete").innerHTML = "";
       }
   })
   */

})


//refresh table if every row is collpased
function refreshTable() {
    var table = document.getElementById("caseTable");
    var lastRowIndex = table.rows.length - 1;

    for (let i = 1; i < lastRowIndex + 1; i++) {
        if (table.rows[i].getElementsByTagName('div')[0].classList.contains('show') || table.rows[i].getElementsByTagName('div')[0].classList.contains('collapsing')) {
            return;
        }
    }
    $("#tableContainer").load(location.href + " #tableContainer"); // reload div. Whitespace important
}

function kr1p0styleInput() {
    let customInputs = document.getElementsByClassName("kr1CustomInputContainer")
    for (let i = 0; i < customInputs.length; i++) {
        if (customInputs[i].children[1] != undefined) {
            if (customInputs[i].children[1].value != "" || customInputs[i].children[1].type == "datetime-local"
                || customInputs[i].children[1].type == "date" || customInputs[i].children[1].type == "time") {
                customInputs[i].children[0].classList.add("kr1SpanOnFocus");
            }
        }

    }
}

//display Images after input dialog
function onUserDataImageSelected(event) {

    let imgEl = document.createElement("img");
    document.getElementById("displayUploadedImg").appendChild(imgEl);


    var selectedFile = event.target.files[0];
    var reader = new FileReader();

    imgEl.title = selectedFile.name;

    reader.onload = function (event) {
        imgEl.src = event.target.result;
    };

    reader.readAsDataURL(selectedFile);


}




//stop dropdownmenu from hidding on click
$(".dropdown").click(function (e) {
    e.stopPropagation();
})


function displayAlert() {
    if (sessionStorage.getItem("passedInfo") != null) {
        let msg = sessionStorage.getItem("passedInfo");
        SlideUpAlert(msg, (msg == "Nieudana operacja") ? false : true)
        sessionStorage.clear();
    }
    //sessionStorage.setItem("passedInfo", "tresctresc");

    let passAlert = document.getElementsByClassName("passAlert");



    for (let i = 0; i < passAlert.length; i++) {

        if (passAlert[i].value != "") {
            SlideUpAlert(passAlert[i].value, true)
            passAlert[i] = "";
            document.getElementsByClassName("passAlert").value = "";
        }
    }
}



//CREATES SLIDEUP ALERT 
function SlideUpAlert(msg, success) {
    let background;
    let fontColor;
    if (success == false) {
        background = "rgba(	252, 76, 87, 0.7)";
        fontColor = "#000"
    }
    else {
        background = "rgba(	255, 206, 50, 0.7)";
        fontColor = "#000"
    }

    var el = document.createElement("div");
    el.id = "ele";

    el.setAttribute("style", `position: fixed; ; text-align:center; bottom: 25px\
			;left: 50%;transform: translate(-50%, 0); background: ${background}\
			;font-size:1.2em; color: ${fontColor} ; border-radius: 7px; padding:12px 40px ;display:none;\
			; z-index:3; font-weight:600`);

    el.innerHTML = msg;
    document.body.appendChild(el);
    setTimeout(function () {
        $("#ele").slideDown(400);
    }
        , 700);

    //aditional
    setTimeout(function () {
        $("#ele").slideUp(1200);
    }
        , 1100);

    setTimeout(function () {
        $("#ele").stop();
    }
        , 1300);

    setTimeout(function () {
        $("#ele").slideDown(400);
    }
        , 1300);
    //aditional


    setTimeout(function () {
        $("#ele").slideUp(400);
    }
        , 3500);

    setTimeout(function () {
        document.body.removeChild(el);
    }
        , 4300);
}


//input and placeholder onfocus
function kr1CustomInputFocusIn(sender) {
    let customPlaceHolder = sender.children[0];
    let customInput = sender.children[1]
    customPlaceHolder.classList.add("kr1SpanOnFocus")
    if (customInput.value == "" && document.activeElement != customInput && customInput.type
        != "datetime-local" && customInput.type != "date" && customInput.type != "time") {
        sender.children[0].classList.remove("kr1SpanOnFocus")
    }
}

//send data of marked table row
function markTableRow(sender, isMarked, source = "") {
    //if source == mainPage
    if (source == "") {
        //for collapse animation before action
        document.getElementById("caseTable").rows[clickedUpperTableRowGlobal].click()
    }


    /*
  
    connection.invoke("SendMessage", "ssss").catch(function (err) {
        return console.error(err.toString());
    });


*/



    $.ajax({
        type: "POST",
        url: "/CaseOverview/?handler=AjaxmarkTableRow",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: {
            idToMark: sender,
            boolMark: (isMarked == "inactive") ? false : true, //if is already marked - reverse
        },
        success: function (data) {
            if (source == "viewCasePage") {
                SlideUpAlert("(✓) Powodzenie", true);
            }
            else {
                $("#tableContainer").load(location.href + " #tableContainer"); // reload div. Whitespace important
            }

            //to avoid changes notification for source of change
            let notiContainer = document.getElementById("changesNotiContainer")
            let noti = document.getElementById("changesNoti")
            notiContainer.classList.remove("changesNotiContainerClsRed")
            notiContainer.classList.add("changesNotiContainerCls")
            noti.innerText = ""
            changes = 0;
        },
        failure: function (response) {

        },
        error: function (response) {

        },
        complete: function (response) {
        }
    });
}

//remove case
function removeCase(sender, source = "") {
    //if source == mainPage
    if (source == "") {
        //for collapse animation before action
        document.getElementById("caseTable").rows[clickedUpperTableRowGlobal].click()
    }
    $.ajax({
        type: "POST",
        url: "/CaseOverview/?handler=AjaxRemoveCase",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: {
            idToMark: sender,
        },
        success: function (data) {
            if (data == true) {
                SlideUpAlert("(✓) Usunięto", true)
            }
            else {
                SlideUpAlert("(🗙) Nieudana operacja", false)
            }

            if (source == "viewCasePage") {
                setTimeout(function () {
                    var newLocation = location.toString()
                    newLocation = newLocation.substring(0, newLocation.indexOf('addcase'));
                    newLocation += "CaseOverview";
                    window.location.href = newLocation;
                }
                    , 2500);

            }
            else {
                $("#tableContainer").load(location.href + " #tableContainer"); // reload div. Whitespace important
            }

        },
        failure: function (response) {

        },
        error: function (response) {

        },
        complete: function (response) {
        }
    });
}

//hilight case
function hilightCase(sender, isHighlighted, source = "") {

    //if source == mainPage
    if (source == "") {
        //for collapse animation before action
        document.getElementById("caseTable").rows[clickedUpperTableRowGlobal].click()
    }


    $.ajax({
        type: "POST",
        url: "/CaseOverview/?handler=AjaxHilightCase",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: {
            idToMark: sender,
            boolMark: (isHighlighted == "y") ? false : true, //if is already highlighted - reverse
        },
        success: function (data) {
            if (source == "viewCasePage") {
                SlideUpAlert("(✓) Powodzenie", true);
            }
            else {
                $("#tableContainer").load(location.href + " #tableContainer"); // reload div. Whitespace important
            }

        },
        failure: function (response) {

        },
        error: function (response) {

        },
        complete: function (response) {

        }
    });
}

function saveTextArea(caseId) {
    let thisTable = document.getElementById("caseTable");
    let bottomTextArea = thisTable.rows[clickedLowerTableRowGlobal].querySelectorAll(".kr1CustomTextArea2")[0];
    let upperTextArea = thisTable.rows[clickedLowerTableRowGlobal].querySelectorAll(".kr1CustomTextArea")[0];
    let textAreaContent = upperTextArea.value + "\n" + bottomTextArea.value;

    //update value in upper textArea and adjust height
    upperTextArea.style.display = "";
    upperTextArea.value += (upperTextArea.value == "" ? "" : "\n") + bottomTextArea.value
    bottomTextArea.value = "";
    upperTextArea.style.height = "auto";
    upperTextArea.style.height = upperTextArea.scrollHeight + "px";

    $.ajax({
        type: "POST",
        url: "/CaseOverview/?handler=AjaxUpdateCaseDescription",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: {
            caseId: caseId,
            description: textAreaContent,
        },
        success: function (data) {
            SlideUpAlert("(✓) Powodzenie", true);
            thisTable.rows[clickedLowerTableRowGlobal].querySelectorAll(".kr1CustomTextArea2")[0].style.backgroundColor = "#a9f6ba";

            setTimeout(function () {
                thisTable.rows[clickedLowerTableRowGlobal].querySelectorAll(".kr1CustomTextArea2")[0].style.backgroundColor = "";
            }
                , 300);

            thisTable.rows[clickedLowerTableRowGlobal].querySelectorAll(".tableButtonSaveTextArea")[0].style.display = "none";


        },
        failure: function (response) {

        },
        error: function (response) {

        },
        complete: function (response) {
        }
    });
}

//show button from current row
function onChangeCaseOvervewTextArea(sender) {
    sender.closest('.kr1CustomInputOuterContainer').querySelectorAll(".tableButtonSaveTextArea")[0].style.display = "block";
}

//add images to array for saving it later
var filesArr = [];
function addImage2formData() {
    let files = $('#fileUpload').prop("files");
    filesArr.push(files[0]);
}

//prefent from form submit byt validation works
function onloadAddCase() {
    $("#addCourseForm").on("submit", function (e) {
        e.preventDefault()
        saveCase();
    });
}

//save case form and images
function saveCase() {

    var formData = new FormData();
    for (let i = 0; i < filesArr.length; i++) {
        formData.append("uploadedFile", filesArr[i]);
    }
    filesArr = []; //empty



    if (document.getElementById("caseStartTime").value != "" && document.getElementById("caseStartDate").value == "") {
        document.getElementById("caseStartDate").value = prepareDateNow();
    }
    if (document.getElementById("caseEndTime").value != "" && document.getElementById("caseEndDate").value == "") {
        document.getElementById("caseEndDate").value = prepareDateNow();
    }
    var CaseObj = {
        FirstName: document.getElementById("caseFirstName").value,
        LastName: document.getElementById("caseLastName").value,
        Description: document.getElementById("caseDescription").value,
        Email: document.getElementById("caseEmail").value,
        Telephone: document.getElementById("casePhone").value,
        UnigueNumber: document.getElementById("caseUniqueNum").value,
        Model: document.getElementById("casemodel").value,
        Manufacturer: document.getElementById("caseManufacturer").value,
        StartDate: document.getElementById("caseStartDate").value + " " + document.getElementById("caseStartTime").value,
        EndDate: document.getElementById("caseEndDate").value + " " + document.getElementById("caseEndTime").value,
        Notification: document.getElementById("caseNotifi").checked ? 'y' : 'n'
    };


    for (var key in CaseObj) {
        formData.append(key, CaseObj[key]);
    }


    $.ajax({
        type: "POST",
        url: "/AddCase?handler=AjaxAddCase",
        //cache: false,
        contentType: false,
        processData: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: formData,
        success: function (data) {
            if (data == false) {
                sessionStorage.setItem("passedInfo", "Nieudana operacja");
            }
            else {
                sessionStorage.setItem("passedInfo", "(✓) Dodano");
            }

            var newLocation = location.toString()
            newLocation = newLocation.substring(0, newLocation.indexOf('addcase'));
            newLocation += "CaseOverview";
            window.location.href = newLocation;
        },
        failure: function (response) {
            SlideUpAlert("Nieudana operacja", false)
        },
        error: function (response) {
            SlideUpAlert("Nieudana operacja", false)
        },
        complete: function (response) {

        }
    });

}





/*
function searchCaseOverview() {

    if (event.key === 'Enter') {
        let searchString = document.getElementById("searchCaseTable").value.toLowerCase();
        var newLocation = location.toString()
        //adding to existing url filter
        var endOfUrl = (newLocation.indexOf('?') < 1) ? ""
            : newLocation.substring(newLocation.indexOf('?') + 1, newLocation.length);
        newLocation = newLocation.substring(0, newLocation.indexOf('CaseOverview'));
        newLocation += `CaseOverview?searchString=${searchString}&${endOfUrl}`;
        window.location.href = newLocation;
  
        $.ajax({
            type: "POST",
            url: "/CaseOverview/?handler=AjaxSearchCase",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: {
                searchString: searchString.toLowerCase()
            },
            success: function (data) {
                alert("");
                $("#tableContainer").load(location.href + " #caseTable"); // reload div. Whitespace important

            },
            failure: function (response) {

            },
            error: function (response) {

            },
            complete: function (response) {
            }
        });
      
    }
}
*/


https://stackoverflow.com/questions/39275719/razor-and-ajax-how-to-reload-a-table-upon-successful-ajax-call
//gets case images when row is clicked 
//$("#caseTable td").click(function () {
$(document).on("click", "#caseTable td", function (e) {
    var thisTable = document.getElementById("caseTable");
    var columnNum = parseInt($(this).index()) + 1;
    var rowNum = parseInt($(this).parent().index());
    var hiddenIdValue = thisTable.rows[rowNum + 1].cells[0].lastElementChild.value;
    var lastRowIndex = thisTable.rows.length - 1;

    //react only when first from two rows clicked
    if (rowNum % 2 == 0) {
        clickedUpperTableRowGlobal = rowNum + 1;
        clickedLowerTableRowGlobal = rowNum + 2
    }
    else {
        clickedLowerTableRowGlobal = rowNum + 1
    }


    //auto height  for textArea in clicked row
    let upperTextArea = thisTable.rows[clickedLowerTableRowGlobal].querySelectorAll(".kr1CustomTextArea")[0];
    if (upperTextArea != undefined) {
        // hide if there is none content
        if (upperTextArea.value == "") {
            upperTextArea.style.display = "none";
        }
        upperTextArea.style.height = "auto";
        upperTextArea.style.height = upperTextArea.scrollHeight + "px";
    }


    //if is empty
    if (thisTable.rows[rowNum + 2].cells[0].children[0].querySelector(".displayUploadedImgCaseTable") != null) {
        if (thisTable.rows[rowNum + 2].cells[0].children[0].querySelector(".displayUploadedImgCaseTable").innerHTML.length < 3) {
            thisTable.rows[rowNum + 2].cells[0].children[0].querySelector(".awaitImageSpinner").style.display = "";
        }
    }


  
    //var hiddenIdValue = thisTable.rows[rowNum].cells[0].lastElementChild.value;
   
    if (thisTable.rows[rowNum + 1].getAttribute('aria-expanded') == "true") {
        //thisTable.rows[rowNum + 1].style.setProperty("background-color", "#5d616815", "important");
        thisTable.rows[rowNum + 1].classList.add("selectedRowBorder");
    }
    else {
        //thisTable.rows[rowNum + 1].style.setProperty("background-color", "", "important");
        thisTable.rows[rowNum + 1].classList.remove("selectedRowBorder");
    }
 

    //thisTable.rows[rowNum + 2].cells[0].children[0].style.backgroundColor  = "red";
    //let tempDiv = document.createElement("div");
    //tempDiv.classList.add("caseTableImages");
    //thisTable.rows[rowNum + 2].cells[0].children[0].querySelector(".displayUploadedImgCaseTable").appendChild(tempDiv);

    //if is empty
    if (thisTable.rows[rowNum + 2].cells[0].children[0].querySelector(".displayUploadedImgCaseTable") != null
        && thisTable.rows[rowNum + 2].cells[0].children[0].querySelector(".displayUploadedImgCaseTable").innerHTML.length < 3) {
        $.ajax({
            type: "POST",
            url: "/CaseOverview/?handler=AjaxGetCaseImages",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: {
                caseId: hiddenIdValue,
            },
            success: function (data) {
                if (thisTable.rows[rowNum + 2].cells[0].children[0].querySelector(".displayUploadedImgCaseTable") != null) {
                    thisTable.rows[rowNum + 2].cells[0].children[0].querySelector(".displayUploadedImgCaseTable").innerHTML = "";
                    let tempDiv = document.createElement("div");

                    data.forEach(function (element) {
                        //tempDiv.innerHTML += `<a href="../uploads/img/${element}" target="_blank"> <img class="caseTableImages" src = "../uploads/img/${element}"></a>`
                        tempDiv.innerHTML += `<a href="javascript:openBase64InNewTab ('${element}')" > <img class="caseTableImages" src="data:image/jpg;base64,${element}"> </a>`

                    });
                    thisTable.rows[rowNum + 2].cells[0].children[0].querySelector(".displayUploadedImgCaseTable").appendChild(tempDiv);
                }
                thisTable.rows[rowNum + 2].cells[0].children[0].querySelector(".awaitImageSpinner").style.display = "none";


            },
            failure: function (response) {
                thisTable.rows[rowNum + 2].cells[0].children[0].querySelector(".awaitImageSpinner").style.display = "none";
            },
            error: function (response) {
                thisTable.rows[rowNum + 2].cells[0].children[0].querySelector(".awaitImageSpinner").style.display = "none";
            },
            complete: function (response) {
                thisTable.rows[rowNum + 2].cells[0].children[0].querySelector(".awaitImageSpinner").style.display = "none";
            }
        });
    }





});




//####################################################################################################### EditPage

//prevent form submit
function onloadEditCase() {
    $("#editCourseForm").on("submit", function (e) {
        e.preventDefault()
        editCase();
    });
}

//display Images after input dialog
function displayImageSelectedEditPage(event) {

    let imgEl = document.createElement("img");
    let divEl = document.createElement("div");
    document.getElementById("displayUploadedImgEdit").appendChild(divEl).appendChild(imgEl);

    var selectedFile = event.target.files[0];
    var reader = new FileReader();

    imgEl.title = selectedFile.name;

    reader.onload = function (event) {
        imgEl.src = event.target.result;
    };

    reader.readAsDataURL(selectedFile);
}

//generates arr of files to save on edit page
var editFilesArr = [];
function addImage2formDataEditPage() {
    let files = $('#fileUploadEdit').prop("files");
    editFilesArr.push(files[0]);
}


// generates list of images to remove on submit
var imageId2RemoveList = [];
function removeImageEditPage(imageId, sender) {
    imageId2RemoveList.push(imageId)
    sender.parentElement.style.opacity = "0.25";
}


//update case data and images
function editCase() {
    //remove images by imageId2RemoveList
    /*
    if (imageId2RemoveList.length > 0) {
        $.ajax({
            type: "POST",
            url: "/editcase/?handler=AjaxRemoveImage",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: {
                imageIdList: imageId2RemoveList,
            },
            success: function (data) {
                //$("#displayUploadedImgEdit").load(location.href + " #displayUploadedImgEdit"); // reload div. Whitespace important
            },
            failure: function (response) {

            },
            error: function (response) {

            },
            complete: function (response) {
            }
        });
    }
    */




    let formData = new FormData();

    //remove images by imageId2RemoveList
    for (let i = 0; i < imageId2RemoveList.length; i++) {
        formData.append("fileUploadRemove", imageId2RemoveList[i]);
    }

    //update form data
    for (let i = 0; i < editFilesArr.length; i++) {
        formData.append("fileUploadEdit", editFilesArr[i]);
    }

    //empty
    editFilesArr = [];


    if (document.getElementById("caseStartTimeEdit").value != "" && document.getElementById("caseStartDateEdit").value == "") {
        document.getElementById("caseStartDateEdit").value = prepareDateNow();
    }
    if (document.getElementById("caseEndTimeEdit").value != "" && document.getElementById("caseEndDateEdit").value == "") {
        document.getElementById("caseEndDateEdit").value = prepareDateNow();
    }
    let CaseObj = {
        CaseId: document.getElementById("caseId2Edit").value,
        FirstName: document.getElementById("caseFirstNameEdit").value,
        LastName: document.getElementById("caseLastNameEdit").value,
        Description: document.getElementById("caseDescriptionEdit").value,
        Email: document.getElementById("caseEmailEdit").value,
        Telephone: document.getElementById("casePhoneEdit").value,
        UnigueNumber: document.getElementById("caseUniqueNumEdit").value,
        Model: document.getElementById("casemodelEdit").value,
        Manufacturer: document.getElementById("caseManufacturerEdit").value,
        StartDate: document.getElementById("caseStartDateEdit").value + " " + document.getElementById("caseStartTimeEdit").value,
        EndDate: document.getElementById("caseEndDateEdit").value + " " + document.getElementById("caseEndTimeEdit").value,
        Notification: document.getElementById("caseNotifiEdit").checked ? 'y' : 'n'
    };


    for (var key in CaseObj) {
        formData.append(key, CaseObj[key]);
    }


    $.ajax({
        type: "POST",
        url: "/EditCase?handler=AjaxEditCase",
        //cache: false,
        contentType: false,
        processData: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: formData,
        success: function (data) {

            if (data == false) {
                sessionStorage.setItem("passedInfo", "Nieudana operacja");
            }
            else {
                sessionStorage.setItem("passedInfo", "(✓) Powodzenie");
            }

            var newLocation = location.toString()
            newLocation = newLocation.substring(0, newLocation.indexOf('addcase'));
            newLocation += "CaseOverview";
            window.location.href = newLocation;
        },
        failure: function (response) {
            SlideUpAlert("Nieudana operacja", false)
        },
        error: function (response) {
            SlideUpAlert("Nieudana operacja", false)
        },
        complete: function (response) {


        }
    });

}



//#############################################################CaseTable

function CaseTableOnload() {
    //for autorefreshing timeLeft every minute
    setInterval(function () {
        refreshTimeLeft();
        console.log("###TimeLeft: auto refresh")
    }, 5 * 1000); // 60 * 1000 milsec


    /*
    // for scrolling to opened row via qr code link. 
    if (document.getElementById("selectedIdCaseTable").value != "") {
        var val = document.getElementById("selectedIdCaseTable").value;
        var thisTable = document.getElementById("caseTable");
        var lastRowIndex = thisTable.rows.length - 1;

        for (let i = 1; i < lastRowIndex + 1; i++) {
            if (thisTable.rows[i].cells[0].lastElementChild.value == val) {
                thisTable.rows[i].cells[0].click();

                setTimeout(function () {
                    thisTable.rows[i - 1].scrollIntoView()
                }
                    , 300);

            }
        }

    }
    */
}


function openBase64InNewTab(data) {

    var newTab = window.open();
    newTab.document.body.innerHTML = `<img src="data:image/jpg;base64,${data}" width="fit-content">`;

    /*
    var byteCharacters = atob(data);
    var byteNumbers = new Array(byteCharacters.length);
    for (var i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    var byteArray = new Uint8Array(byteNumbers);
    var file = new Blob([byteArray], { type: mimeType + ';base64' });
    var fileURL = URL.createObjectURL(file);
    window.open(fileURL);
    */
}


function displayRecoverPassClick() {
    document.getElementById("passRecoveryFormSubmitBtn").style.display = "block";
    document.getElementById("loginFormSubmitBtn").style.display = "none";
    $("#loginFormPasswordContainer").slideUp();
    document.getElementById("recoverPassLinkContainer").innerHTML = ' <a href="javascript: displayLoginPanelClick()">Panel logowania</a>';
    document.getElementById("loginFormLeftColumnId").classList.add("loginFormLeftColumnAlternativeBackgroundImage");
}

function displayLoginPanelClick() {
    document.getElementById("passRecoveryFormSubmitBtn").style.display = "none";
    document.getElementById("loginFormSubmitBtn").style.display = "block";
    $("#loginFormPasswordContainer").slideDown();
    document.getElementById("recoverPassLinkContainer").innerHTML = ' <a href="javascript: displayRecoverPassClick(this)">Odzyskaj hasło</a>';
    document.getElementById("loginFormLeftColumnId").classList.remove("loginFormLeftColumnAlternativeBackgroundImage");
}


//update time left without refrersh
function refreshTimeLeft() {
    let thisTable = document.getElementById("caseTable");
    let lastRowIndex = thisTable.rows.length - 1;

    for (let i = 1; i < lastRowIndex + 1; i++) {
        if (thisTable.rows[i].querySelectorAll(".hiddenEndDateVal")[0] != undefined
            && thisTable.rows[i].querySelectorAll(".displayTimeLeftSpan")[0] != undefined) {

            let endDate = thisTable.rows[i].querySelectorAll(".hiddenEndDateVal")[0].value;
            let allMinutesLeft = convertDateTime(endDate);
            let hoursLeft = Math.floor(allMinutesLeft / 60);
            let minutesLeft = allMinutesLeft % 60;
            let daysLeft = Math.floor(hoursLeft / 24);
            hoursLeft = daysLeft == 0 ? hoursLeft : hoursLeft - 24 * daysLeft;
            let timeLeft = `${daysLeft}d : ${hoursLeft}h : ${minutesLeft}min`;
            let displayTimeLeftSpanArr = thisTable.rows[i].querySelectorAll(".displayTimeLeftSpan")
            let progressBarColor = allMinutesLeft >= 180 ? "#5bde94" : "#eb506c";

            for (let i = 0; i < displayTimeLeftSpanArr.length; i++) {
                displayTimeLeftSpanArr[i].innerText = timeLeft;

                if (allMinutesLeft <= 0) {
                    displayTimeLeftSpanArr[i].parentNode.parentNode.innerHTML =
                        ' <img class="timeIsUpRowImg" src = "../img/black/patch-exclamation.svg">'
                    displayTimeLeftSpanArr[i].parentNode.remove();
                }
            }



            if (thisTable.rows[i].querySelectorAll(".customProgBar")[0] != undefined) {
                let customProgBarArr = thisTable.rows[i].querySelectorAll(".customProgBar");
                let progress = Math.floor(allMinutesLeft * 100 / 1440) + "%";

                for (let i = 0; i < customProgBarArr.length; i++) {
                    customProgBarArr[i].style.width = progress;
                    customProgBarArr[i].style.backgroundColor = progressBarColor;
                }

            }
        }


    }

}



//convert dateTime send from server
function convertDateTime(dateTimeArg) {
    let dateAndTime = dateTimeArg.split(" ");
    let date = dateAndTime[0].split(".");
    let dd = date[0];
    let mm = date[1];
    let yyyy = date[2];

    let time = dateAndTime[1].split(":");
    let h = time[0];
    let m = time[1];

    let result = new Date(yyyy + "-" + mm + "-" + dd + " " + h + ":" + m);
    let diff = Math.abs(result - new Date());
    let diffInMInutes = Math.floor(diff / 60000);
    return diffInMInutes;
}


function prepareDateNow() {
    var currentDate = new Date()
    var currentMonthNumber = currentDate.getMonth() + 1;
    currentMonthNumber = currentMonthNumber < 10 ? "0" + currentMonthNumber : currentMonthNumber;
    var currentYear = currentDate.getFullYear();
    var currentDay = currentDate.getDate();
    currentDay = currentDay < 10 ? "0" + currentDay : currentDay;

    return currentYear + "-" + currentMonthNumber + "-" + currentDay;
}

function appendDateToInput() {
    let thisTable = document.getElementById("caseTable");
    let bottomTextArea = thisTable.rows[clickedLowerTableRowGlobal].querySelectorAll(".kr1CustomTextArea2")[0];
    let detNow = new Date();
    let hours = detNow.getHours();
    let minutes = detNow.getMinutes();
    minutes = minutes < 10 ? "0" + minutes : minutes;

    let newLine = bottomTextArea.value != "" ? "\n" : "";

    bottomTextArea.value += newLine + prepareDateNow() + "  " + hours + ":" + minutes + " ";
    bottomTextArea.focus();
    onChangeCaseOvervewTextArea(bottomTextArea);
}

function displayDictonaryWindow() {
    let thisTable = document.getElementById("caseTable");
    let ele = document.createElement("div");
    ele.innerHTML = `<div onclick='this.remove()' class="dictionaryWindow">${dict()}</div>`;
    thisTable.rows[clickedLowerTableRowGlobal].querySelectorAll(".textareaCont2")[0].appendChild(ele)

    function dict() {
        let li =
            [
                'Przekazano do serwisu gwarancyjnego',
                'Oczekuje na informację zwrotną',
                'Nieudany kontakt',
                'Powiadomiono',
                'Naprawiono',
                'Oczekuje na podzespoły',
                'Brak podzespołów'
            ]
        let el =
            `<div class="" onclick="appendWordToInput(this)"><img src="/img/white/plus-circle-fill.svg" ><span>${li[0]}</span></div><br/>` +
            `<div class="" onclick="appendWordToInput(this)"><img src="/img/white/plus-circle-fill.svg" ><span>${li[1]}</span></div><br/>` +
            `<div class="" onclick="appendWordToInput(this)"><img src="/img/white/plus-circle-fill.svg" ><span>${li[2]}</span></div><br/>` +
            `<div class="" onclick="appendWordToInput(this)"><img src="/img/white/plus-circle-fill.svg" ><span>${li[3]}</span></div><br/>` +
            `<div class="" onclick="appendWordToInput(this)"><img src="/img/white/plus-circle-fill.svg" ><span>${li[4]}</span></div><br/>` +
            `<div class="" onclick="appendWordToInput(this)"><img src="/img/white/plus-circle-fill.svg" ><span>${li[5]}</span></div><br/>` +
            `<div class="" onclick="appendWordToInput(this)"><img src="/img/white/plus-circle-fill.svg" ><span>${li[6]}</span></div><br/>`
        return el;
    }


}
function appendWordToInput(sender) {
    let thisTable = document.getElementById("caseTable");
    let newWord = sender.children[1].innerText;
    let bottomTextArea = thisTable.rows[clickedLowerTableRowGlobal].querySelectorAll(".kr1CustomTextArea2")[0];
    bottomTextArea.value += newWord;
    onChangeCaseOvervewTextArea(bottomTextArea);
}


closeReadOnlyInfo()
function closeReadOnlyInfo(sender) {
    //sessionStorage.setItem("displayReadOnlyInfo", "false");
    if (sessionStorage.getItem("displayReadOnlyInfo") != "false") {
        document.getElementById("readOnlyInfo").style.display = "block";
    }
    if (sender) {
        sender.parentElement.style.display = "none"
        sessionStorage.setItem("displayReadOnlyInfo", "false");
    }
}
