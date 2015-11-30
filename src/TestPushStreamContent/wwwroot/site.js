if (!!window.EventSource) {
    var source = new EventSource('/api/eventstream');
    source.addEventListener('open', function (e) {
        document.body.innerHTML += 'Connection was opened<br>';
    }, false);

    source.addEventListener('error', function (e) {
        if (e.readyState == EventSource.CLOSED) {
            document.body.innerHTML += 'Connection was closed<br>';
        }
    }, false);

    source.addEventListener('message', function (e) {
        console.log("Received data");
        document.body.innerHTML += e.data + '<br>';
    }, false);

} else {
    // Result to xhr polling :(
    document.body.innerHTML = 'Failed to create EventSource';
}