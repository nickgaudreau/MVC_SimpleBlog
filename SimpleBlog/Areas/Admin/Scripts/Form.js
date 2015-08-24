$(document).ready(function() {

    // cerate a dialog box confirm deletion
    $("a[data-post]").click(function(e) {
        e.preventDefault();

        var $this = $(this);
        var message = $this.data("post");

        //if there is a message and they cancel on dialog box
        if (message && !confirm(message))
            return;

        // COPY/APPEND TO THEN CAPTURE AND POST TO THE REQUEST SO DELETION IS POSSIBLE (NO ERROR THROWN FOR NO ANTI FORGERY TOKEN)
        // get the input tag of the anti forge form
        var antiForgeryToken = $("#anti-forgery-form input");
        // add a hidden input, set that hideen input name to what the token had and his val
        var antiForgeryInput = $("<input type='hidden'").attr("name", antiForgeryToken.attr("name")).val(antiForgeryToken.val());
        $("<form>")
            .attr("method", "post")
            .attr("action", $this.attr("href"))
            .append(antiForgeryInput)
            .appendTo(document.body)
            .submit();
    });
});