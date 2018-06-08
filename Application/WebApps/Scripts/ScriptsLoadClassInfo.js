var _timeoutcount = 500;//ms
var textInput = document.getElementById("Nvs001txt_search_app_code");

// Init a timeout variable to be used below
var timeout = null;

// Listen for keystroke events
textInput.onkeyup = function (e) {

    // Clear the timeout if it has already been set.
    // This will prevent the previous task from executing
    // if it has been less than <MILLISECONDS>
    clearTimeout(timeout);

    // Make a new timeout set to go off in 800ms
    timeout = setTimeout(function () {
          alert('Input Value:' + textInput.value);
        // tìm kiếm ở đoạn này
    }, _timeoutcount);
};

