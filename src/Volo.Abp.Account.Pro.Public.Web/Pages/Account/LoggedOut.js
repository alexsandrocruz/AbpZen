document.addEventListener("DOMContentLoaded", function (event) {
    setTimeout(function () {
        var redirectButton = document.getElementById("redirectButton");
        
        if(!redirectButton){
            return;
        }
            
        window.clientName = redirectButton.getAttribute("cname");
        window.location = redirectButton.getAttribute("href");
    }, 3000)
});
